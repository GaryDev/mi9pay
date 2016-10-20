using Aop.Api;
using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using ICanPay.Configs;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace ICanPay.Providers
{
    /// <summary>
    /// 支付宝网关
    /// </summary>
    /// <remarks>
    /// 当前支付宝的实现仅支持MD5密钥。
    /// </remarks>
    public sealed class AlipayGateway : GatewayBase, IPaymentForm, IPaymentUrl, IPaymentQRCode, IQueryNow
    {

        #region 私有字段

        private static Logger logger = LogManager.GetCurrentClassLogger();

        const string payGatewayUrl = "https://mapi.alipay.com/gateway.do";
        const string emailRegexString = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        static Encoding pageEncoding = Encoding.GetEncoding("gb2312");

        private IAlipayTradeService f2fPayService;

        #endregion


        #region 构造函数

        /// <summary>
        /// 初始化支付宝网关
        /// </summary>
        public AlipayGateway()
        {
            
        }


        /// <summary>
        /// 初始化支付宝网关
        /// </summary>
        /// <param name="gatewayParameterData">网关通知的数据集合</param>
        public AlipayGateway(List<GatewayParameter> gatewayParameterData)
            : base(gatewayParameterData)
        {
        }

        #endregion


        #region 属性

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
                // 通过RequestType、UserAgent来判断是否为服务器通知
                if (string.Compare(HttpContext.Current.Request.RequestType, "POST") == 0 &&
                    string.Compare(HttpContext.Current.Request.UserAgent, "Mozilla/4.0") == 0)
                {
                    return PaymentNotifyMethod.ServerNotify;
                }

                return PaymentNotifyMethod.AutoReturn;
            }
        }


        #endregion


        #region 方法

        public string BuildPaymentForm()
        {
            ValidatePaymentOrderParameter();
            InitOrderParameter();

            return GetFormHtml(payGatewayUrl);
        }


        /// <summary>
        /// 初始化订单参数
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
            SetGatewayParameterValue("sign", GetOrderSign());    // 签名需要在最后设置，以免缺少参数。
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
        /// 获得用于签名的参数字符串
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
        /// 验证支付订单的参数设置
        /// </summary>
        private void ValidatePaymentOrderParameter()
        {
            if (string.IsNullOrEmpty(GetGatewayParameterValue("seller_email")))
            {
                throw new ArgumentNullException("seller_email", "订单缺少seller_email参数，seller_email是卖家支付宝账号的邮箱。" +
                                                "你需要使用PaymentSetting<T>.SetGatewayParameterValue(\"seller_email\", \"youname@email.com\")方法设置卖家支付宝账号的邮箱。");
            }

            if (!IsEmail(GetGatewayParameterValue("seller_email")))
            {
                throw new ArgumentException("Email格式不正确", "seller_email");
            }
        }


        protected override bool CheckNotifyData()
        {
            if (ValidateAlipayNotify() && ValidateAlipayNotifySign())
            {
                // 支付状态是否为成功。TRADE_FINISHED（普通即时到账的交易成功状态，TRADE_SUCCESS（开通了高级即时到账或机票分销产品后的交易成功状态）
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
        /// 验证支付宝通知的签名
        /// </summary>
        private bool ValidateAlipayNotifySign()
        {
            // 验证通知的签名
            if (string.Compare(GetGatewayParameterValue("sign"), GetOrderSign()) == 0)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 将网关参数的集合排序
        /// </summary>
        /// <param name="coll">原网关参数的集合</param>
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
        /// 获得订单的签名。
        /// </summary>
        private string GetOrderSign()
        {
            // 获得MD5值时需要使用GB2312编码，否则主题中有中文时会提示签名异常，并且MD5值必须为小写。
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
        /// 验证网关的通知Id是否有效
        /// </summary>
        private bool ValidateAlipayNotify()
        {
            // 浏览器自动返回的通知Id会在验证后1分钟失效，
            // 服务器异步通知的通知Id则会在输出标志成功接收到通知的success字符串后失效。
            if (string.Compare(Utility.ReadPage(GetValidateAlipayNotifyUrl()), "true") == 0)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 获得验证支付宝通知的Url
        /// </summary>
        private string GetValidateAlipayNotifyUrl()
        {
            return string.Format("{0}?service=notify_verify&partner={1}&notify_id={2}", payGatewayUrl, Merchant.UserName,
                                 GetGatewayParameterValue("notify_id"));
        }


        /// <summary>
        /// 是否是正确格式的Email地址
        /// </summary>
        /// <param name="emailAddress">Email地址</param>
        public bool IsEmail(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                return false;
            }

            return Regex.IsMatch(emailAddress, emailRegexString);
        }

        public string GetPaymentQRCodeContent()
        {
            InitF2FPayService();
            AlipayTradePrecreateContentBuilder builder = BuildPrecreateContent();
            return GetAlipayPaymentUrl(PostOrder(builder));
        }

        private void InitF2FPayService()
        {
            string serverUrl = AlipayConfig.ServerUrl;
            string appid = GetGatewayParameterValue("appid");
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
            builder.time_expire = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
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

        public bool QueryNow()
        {
            InitF2FPayService();
            AlipayF2FQueryResult queryResult = DoQuery();
            return IsQuerySuccess(queryResult);
        }

        public QueryResult QueryForResult()
        {
            InitF2FPayService();
            QueryResult result = null;
            AlipayF2FQueryResult queryResult = DoQuery();
            if (IsQuerySuccess(queryResult))
            {
                result = new QueryResult();
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

        private void WriteErrorLog(string method, AopResponse response, bool showAccountInfo = true)
        {
            logger.Info(string.Format("{0} ==> Msg: {1}", method, response.Msg) + Environment.NewLine);
            logger.Info(string.Format("{0} ==> SubMsg: {1}", method, response.SubMsg) + Environment.NewLine);

            if (showAccountInfo)
            {
                string serverUrl = AlipayConfig.ServerUrl;
                string appid = GetGatewayParameterValue("appid");
                string mchKey = Merchant.Key;
                string publicKey = Merchant.PublicKey;

                logger.Info(string.Format("<============{0}============>", "支付账号信息") + Environment.NewLine);
                logger.Info(string.Format("网关地址：{0}", serverUrl) + Environment.NewLine);
                logger.Info(string.Format("支付账号Appid：{0}", appid) + Environment.NewLine);
                logger.Info(string.Format("支付账号mchKey：{0}", mchKey) + Environment.NewLine);
                logger.Info(string.Format("支付账号publicKey：{0}", publicKey) + Environment.NewLine);
                logger.Info("<=======================================>" + Environment.NewLine);
            }
            
        }

        #endregion

    }
}