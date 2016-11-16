using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using Mi9Pay.PayProvider.Configs;
using Mi9Pay.PayProvider.Providers.Extended;
using Mi9Pay.Config;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Mi9Pay.PayProvider.Providers
{
    /// <summary>
    /// ֧��������
    /// </summary>
    /// <remarks>
    /// ��ǰ֧������ʵ�ֽ�֧��MD5��Կ��
    /// </remarks>
    public sealed class AlipayGateway : GatewayBase, IPaymentForm, IPaymentUrl, IPaymentWithCode, IQueryNow
    {

        #region ˽���ֶ�

        private static Logger logger = LogManager.GetCurrentClassLogger();

        const string payGatewayUrl = "https://mapi.alipay.com/gateway.do";
        const string emailRegexString = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        static Encoding pageEncoding = Encoding.GetEncoding("gb2312");

        private IAlipayTradeService f2fPayService;
        private IAopClient aopClient;

        #endregion


        #region ���캯��

        /// <summary>
        /// ��ʼ��֧��������
        /// </summary>
        public AlipayGateway()
        {
            
        }


        /// <summary>
        /// ��ʼ��֧��������
        /// </summary>
        /// <param name="gatewayParameterData">����֪ͨ�����ݼ���</param>
        public AlipayGateway(List<GatewayParameter> gatewayParameterData)
            : base(gatewayParameterData)
        {
        }

        #endregion


        #region ����

        public override GatewayType GatewayType
        {
            get
            {
                return GatewayType.Alipay;
            }
        }


        public override PaymentNotifyMethod PaymentNotifyMethod
        {
            get
            {
                // ͨ��RequestType��UserAgent���ж��Ƿ�Ϊ������֪ͨ
                if (string.Compare(HttpContext.Current.Request.RequestType, "POST") == 0 &&
                    string.Compare(HttpContext.Current.Request.UserAgent, "Mozilla/4.0") == 0)
                {
                    return PaymentNotifyMethod.ServerNotify;
                }

                return PaymentNotifyMethod.AutoReturn;
            }
        }


        #endregion


        #region ����

        #region ԭ��ܴ���
        public string BuildPaymentForm()
        {
            ValidatePaymentOrderParameter();
            InitOrderParameter();

            return GetFormHtml(payGatewayUrl);
        }


        /// <summary>
        /// ��ʼ����������
        /// </summary>
        private void InitOrderParameter()
        {
            SetGatewayParameterValue("service", "create_direct_pay_by_user");
            SetGatewayParameterValue("partner", Merchant.UserName);
            SetGatewayParameterValue("notify_url", Merchant.NotifyUrl.ToString());
            SetGatewayParameterValue("return_url", Merchant.NotifyUrl.ToString());
            SetGatewayParameterValue("sign_type", "MD5");
            SetGatewayParameterValue("subject", Order.Subject);
            SetGatewayParameterValue("out_trade_no", Order.Id);
            SetGatewayParameterValue("total_fee", Order.Amount.ToString());
            SetGatewayParameterValue("payment_type", "1");
            SetGatewayParameterValue("_input_charset", "gb2312");
            SetGatewayParameterValue("sign", GetOrderSign());    // ǩ����Ҫ��������ã�����ȱ�ٲ�����
        }


        public string BuildPaymentUrl()
        {
            ValidatePaymentOrderParameter();
            InitOrderParameter();

            return string.Format("{0}?{1}", payGatewayUrl, GetPaymentQueryString());
        }


        private string GetPaymentQueryString()
        {
            StringBuilder urlBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> item in GetSortedGatewayParameter())
            {
                urlBuilder.AppendFormat("{0}={1}&", item.Key, item.Value);
            }

            return urlBuilder.ToString().TrimEnd('&');
        }


        /// <summary>
        /// �������ǩ���Ĳ����ַ���
        /// </summary>
        private string GetSignParameter()
        {
            StringBuilder signBuilder = new StringBuilder();
            foreach(KeyValuePair<string, string> item in GetSortedGatewayParameter())
            {
                if (string.Compare("sign", item.Key) != 0 && string.Compare("sign_type", item.Key) != 0)
                {
                    signBuilder.AppendFormat("{0}={1}&", item.Key, item.Value);
                }
            }

            return signBuilder.ToString().TrimEnd('&');
        }



        /// <summary>
        /// ��֤֧�������Ĳ�������
        /// </summary>
        private void ValidatePaymentOrderParameter()
        {
            if (string.IsNullOrEmpty(GetGatewayParameterValue("seller_email")))
            {
                throw new ArgumentNullException("seller_email", "����ȱ��seller_email������seller_email������֧�����˺ŵ����䡣" +
                                                "����Ҫʹ��PaymentSetting<T>.SetGatewayParameterValue(\"seller_email\", \"youname@email.com\")������������֧�����˺ŵ����䡣");
            }

            if (!IsEmail(GetGatewayParameterValue("seller_email")))
            {
                throw new ArgumentException("Email��ʽ����ȷ", "seller_email");
            }
        }


        protected override bool CheckNotifyData()
        {
            if (ValidateAlipayNotify() && ValidateAlipayNotifySign())
            {
                // ֧��״̬�Ƿ�Ϊ�ɹ���TRADE_FINISHED����ͨ��ʱ���˵Ľ��׳ɹ�״̬��TRADE_SUCCESS����ͨ�˸߼���ʱ���˻��Ʊ������Ʒ��Ľ��׳ɹ�״̬��
                if (string.Compare(GetGatewayParameterValue("trade_status"), "TRADE_FINISHED") == 0 ||
                    string.Compare(GetGatewayParameterValue("trade_status"), "TRADE_SUCCESS") == 0)
                {
                    Order.Amount = double.Parse(GetGatewayParameterValue("total_fee"));
                    Order.Id = GetGatewayParameterValue("out_trade_no");

                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// ��֤֧����֪ͨ��ǩ��
        /// </summary>
        private bool ValidateAlipayNotifySign()
        {
            // ��֤֪ͨ��ǩ��
            if (string.Compare(GetGatewayParameterValue("sign"), GetOrderSign()) == 0)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// �����ز����ļ�������
        /// </summary>
        /// <param name="coll">ԭ���ز����ļ���</param>
        private SortedList<string, string> GatewayParameterDataSort(ICollection<GatewayParameter> coll)
        {
            SortedList<string, string> list = new SortedList<string, string>();
            foreach (GatewayParameter item in coll)
            {
                list.Add(item.Name, item.Value);
            }

            return list;
        }


        /// <summary>
        /// ��ö�����ǩ����
        /// </summary>
        private string GetOrderSign()
        {
            // ���MD5ֵʱ��Ҫʹ��GB2312���룬����������������ʱ����ʾǩ���쳣������MD5ֵ����ΪСд��
            return Utility.GetMD5(GetSignParameter() + Merchant.Key, pageEncoding).ToLower();
        }


        public override void WriteSucceedFlag()
        {
            if (PaymentNotifyMethod == PaymentNotifyMethod.ServerNotify)
            {
                System.Web.HttpContext.Current.Response.Write("success");
            }
        }


        /// <summary>
        /// ��֤���ص�֪ͨId�Ƿ���Ч
        /// </summary>
        private bool ValidateAlipayNotify()
        {
            // ������Զ����ص�֪ͨId������֤��1����ʧЧ��
            // �������첽֪ͨ��֪ͨId����������־�ɹ����յ�֪ͨ��success�ַ�����ʧЧ��
            if (string.Compare(Utility.ReadPage(GetValidateAlipayNotifyUrl()), "true") == 0)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// �����֤֧����֪ͨ��Url
        /// </summary>
        private string GetValidateAlipayNotifyUrl()
        {
            return string.Format("{0}?service=notify_verify&partner={1}&notify_id={2}", payGatewayUrl, Merchant.UserName,
                                 GetGatewayParameterValue("notify_id"));
        }


        /// <summary>
        /// �Ƿ�����ȷ��ʽ��Email��ַ
        /// </summary>
        /// <param name="emailAddress">Email��ַ</param>
        public bool IsEmail(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                return false;
            }

            return Regex.IsMatch(emailAddress, emailRegexString);
        }
        #endregion

        private void InitF2FPayService()
        {
            string serverUrl = AlipayConfig.ServerUrl;
            string appid = Merchant.AppId;
            string mchKey = Merchant.Key;
            string publicKey = Merchant.PublicKey;

            f2fPayService = F2FBiz.CreateClientInstance(
                serverUrl,
                appid, //AlipayConfig.appId, 
                mchKey, //AlipayConfig.merchant_private_key, 
                AlipayConfig.version,
                AlipayConfig.sign_type,
                publicKey, //AlipayConfig.alipay_public_key, 
                AlipayConfig.charset
           );
        }

        public void InitAopClient()
        {
            string serverUrl = AlipayConfig.ServerUrl;
            string appid = Merchant.AppId;
            string mchKey = Merchant.Key;
            string publicKey = Merchant.PublicKey;

            aopClient = new DefaultAopClient(
                serverUrl,
                appid, //AlipayConfig.appId, 
                mchKey, //AlipayConfig.merchant_private_key, 
                "json",
                AlipayConfig.version,
                AlipayConfig.sign_type,
                publicKey, //AlipayConfig.alipay_public_key, 
                AlipayConfig.charset
           );
        }

        #region ��ά��Ԥ֧��
        public string GetPaymentQRCodeContent()
        {
            InitF2FPayService();
            AlipayTradePrecreateContentBuilder builder = BuildPrecreateContent();
            return GetAlipayPaymentUrl(PostOrder(builder));
        }

        private AlipayTradePrecreateContentBuilder BuildPrecreateContent()
        {
            AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();

            builder.out_trade_no = Order.Id;
            builder.body = Order.Subject;
            builder.total_amount = Order.Amount.ToString();
            builder.discountable_amount = Order.DiscountAmount.ToString();
            builder.undiscountable_amount = (Order.Amount - Order.DiscountAmount).ToString();
            builder.operator_id = AlipayConfig.operId;
            builder.subject = Order.Subject;
            //builder.time_expire = "5m"; //DateTime.Now.AddMinutes(5).ToString();
            builder.store_id = GetGatewayParameterValue("storeid"); //AlipayConfig.storeId;
            builder.seller_id = Merchant.UserName; //AlipayConfig.pid;
            
            return builder;
        }

        private string GetAlipayPaymentUrl(AlipayF2FPrecreateResult result)
        {
            if (result.Status != ResultEnum.SUCCESS)
            {
                WriteErrorLog("GetAlipayPaymentUrl", result.response);
            }
            return result.Status == ResultEnum.SUCCESS ? result.response.QrCode : string.Empty;
        }

        private AlipayF2FPrecreateResult PostOrder(AlipayTradePrecreateContentBuilder builder)
        {
            AlipayF2FPrecreateResult precreateResult = f2fPayService.tradePrecreate(builder);
            return precreateResult;
        }
        #endregion

        #region ����֧��
        public PaymentResult BarcodePayment()
        {
            InitF2FPayService();
            AlipayTradePayContentBuilder builder = BuildPayContent();
            PaymentResult result = GetPaymentResult(PostOrder(builder));
            return result;
        }

        private AlipayTradePayContentBuilder BuildPayContent()
        {
            AlipayTradePayContentBuilder builder = new AlipayTradePayContentBuilder();

            builder.out_trade_no = Order.Id;
            builder.body = Order.Subject;
            builder.total_amount = Order.Amount.ToString();
            builder.discountable_amount = Order.DiscountAmount.ToString();
            builder.undiscountable_amount = (Order.Amount - Order.DiscountAmount).ToString();
            builder.operator_id = AlipayConfig.operId;
            builder.subject = Order.Subject;
            builder.timeout_express = "5m";
            builder.store_id = GetGatewayParameterValue("storeid"); //AlipayConfig.storeId;
            builder.seller_id = Merchant.UserName; //AlipayConfig.pid;
            builder.auth_code = GetGatewayParameterValue("auth_code");
            builder.scene = "bar_code";

            return builder;
        }

        private AlipayF2FPayResult PostOrder(AlipayTradePayContentBuilder builder)
        {
            AlipayF2FPayResult payResult = f2fPayService.tradePay(builder);
            return payResult;
        }

        private PaymentResult GetPaymentResult(AlipayF2FPayResult result)
        {
            PaymentResult paymentResult = null;
            if (result.Status == ResultEnum.SUCCESS)
            {
                paymentResult = new PaymentResult();
                paymentResult.TradeNo = result.response.TradeNo;
                paymentResult.Amount = (Convert.ToDecimal(result.response.TotalAmount) * 100).ToString("0");
                paymentResult.PaidAmount = (Convert.ToDecimal(result.response.BuyerPayAmount) * 100).ToString("0");
            }
            else
            {
                WriteErrorLog("GetPaymentResult", result.response);
                throw new Exception(result.response.SubMsg);
            }
            return paymentResult;
        }

        #endregion

        #region ������ѯ
        public bool QueryNow()
        {
            InitF2FPayService();
            AlipayF2FQueryResult queryResult = DoQuery();
            return IsQuerySuccess(queryResult);
        }

        public PaymentResult QueryForResult()
        {
            InitF2FPayService();
            PaymentResult result = null;
            AlipayF2FQueryResult queryResult = DoQuery();
            if (IsQuerySuccess(queryResult))
            {
                result = new PaymentResult();
                result.TradeNo = queryResult.response.TradeNo;
                result.Amount = (Convert.ToDecimal(queryResult.response.TotalAmount) * 100).ToString("0");
                result.PaidAmount = (Convert.ToDecimal(queryResult.response.BuyerPayAmount) * 100).ToString("0");
            }            
            return result;
        }

        private AlipayF2FQueryResult DoQuery()
        {
            string out_trade_no = Order.Id;
            AlipayF2FQueryResult queryResult = f2fPayService.tradeQuery(out_trade_no);
            return queryResult;
        }

        private bool IsQuerySuccess(AlipayF2FQueryResult queryResult)
        {
            if (queryResult != null && queryResult.Status != ResultEnum.SUCCESS)
            {
                WriteErrorLog("IsQuerySuccess", queryResult.response, false);
            }
            return queryResult != null && queryResult.Status == ResultEnum.SUCCESS;
        }
        #endregion

        #region �˵���ѯ

        public string QueryBill()
        {
            InitAopClient();

            AlipayBillDownloadContentBuilder builder = new AlipayBillDownloadContentBuilder();
            builder.bill_type = "trade";
            builder.bill_date = Bill.BillDate;

            AlipayDataDataserviceBillDownloadurlQueryRequest request = new AlipayDataDataserviceBillDownloadurlQueryRequest();
            request.BizContent = builder.BuildJson();
            AlipayDataDataserviceBillDownloadurlQueryResponse response = aopClient.Execute(request);

            if (response.IsError)
            {
                WriteErrorLog("QueryBill", response);
            }

            return !response.IsError ? response.BillDownloadUrl : string.Empty;
        }

        #endregion

        #region �����˿�

        public bool RefundPayment()
        {
            InitF2FPayService();
            AlipayF2FRefundResult result = PostOrder(BuildRefundContent());
            return ValidateRefundResult(result);
        }

        private bool ValidateRefundResult(AlipayF2FRefundResult result)
        {
            if (result.Status != ResultEnum.SUCCESS)
            {
                WriteErrorLog("ValidateRefundResult", result.response);
            }
            return result.Status == ResultEnum.SUCCESS;
        }

        private AlipayTradeRefundContentBuilder BuildRefundContent()
        {
            AlipayTradeRefundContentBuilder builder = new AlipayTradeRefundContentBuilder();

            builder.out_trade_no = Order.Id;
            builder.trade_no = Order.TradeNo;
            builder.out_request_no = Order.RefundRequestNo;
            builder.refund_amount = Order.Amount.ToString();
            builder.refund_reason = Order.RefundReason;

            return builder;
        }

        private AlipayF2FRefundResult PostOrder(AlipayTradeRefundContentBuilder builder)
        {
            AlipayF2FRefundResult refundResult = f2fPayService.tradeRefund(builder);
            return refundResult;
        }

        #endregion

        private void WriteErrorLog(string method, AopResponse response, bool showAccountInfo = true)
        {
            if (!AppConfig.IsLogEnabled)
                return;

            if (response != null)
            {
                if (response.SubMsg == "���ײ�����")
                    return;

                logger.Info(string.Format("{0} ==> Msg: {1}", method, response.Msg) + Environment.NewLine);
                logger.Info(string.Format("{0} ==> SubMsg: {1}", method, response.SubMsg) + Environment.NewLine);
            }

            if (showAccountInfo)
            {
                string serverUrl = AlipayConfig.ServerUrl;
                string appid = Merchant.AppId;
                string mchKey = Merchant.Key;
                string publicKey = Merchant.PublicKey;

                logger.Info(string.Format("<============{0}============>", "֧���˺���Ϣ") + Environment.NewLine);
                logger.Info(string.Format("���ص�ַ��{0}", serverUrl) + Environment.NewLine);
                logger.Info(string.Format("֧���˺�Appid��{0}", appid) + Environment.NewLine);
                logger.Info(string.Format("֧���˺�mchKey��{0}", mchKey) + Environment.NewLine);
                logger.Info(string.Format("֧���˺�publicKey��{0}", publicKey) + Environment.NewLine);
                logger.Info("<=======================================>" + Environment.NewLine);
            }
            
        }
        #endregion

    }
}