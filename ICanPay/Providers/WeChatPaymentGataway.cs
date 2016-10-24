using ICanPay.Configs;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;

namespace ICanPay.Providers
{
    /// <summary>
    /// ΢��֧������
    /// </summary>
    /// <remarks>
    /// ʹ��ģʽ��ʵ��΢��֧��
    /// </remarks>
    public sealed class WeChatPaymentGataway : GatewayBase, IPaymentWithCode, IQueryNow
    {

        #region ˽���ֶ�

        private static Logger logger = LogManager.GetCurrentClassLogger();

        const string payGatewayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        const string queryGatewayUrl = "https://api.mch.weixin.qq.com/pay/orderquery";
        const string microPayGatewayUrl = "https://api.mch.weixin.qq.com/pay/micropay";
        const string reverseGatewayUrl = "https://api.mch.weixin.qq.com/secapi/pay/reverse";

        #endregion


        #region ���캯��

        /// <summary>
        /// ��ʼ��΢��֧������
        /// </summary>
        public WeChatPaymentGataway()
        {
        }


        /// <summary>
        /// ��ʼ��΢��֧������
        /// </summary>
        /// <param name="gatewayParameterData">����֪ͨ�����ݼ���</param>
        public WeChatPaymentGataway(List<GatewayParameter> gatewayParameterData)
            : base(gatewayParameterData)
        {
        }

        #endregion


        public override GatewayType GatewayType
        {
            get { return GatewayType.WeChatPayment; }
        }

        public override PaymentNotifyMethod PaymentNotifyMethod
        {
            get { return PaymentNotifyMethod.ServerNotify; }
        }

        protected override bool CheckNotifyData()
        {
            if (IsSuccessResult())
            {
                ReadNotifyOrderParameter();
                return true;
            }

            return false;
        }

        public string GetPaymentQRCodeContent()
        {
            InitPaymentOrderParameter();
            return GetWeixinPaymentUrl(PostOrder(ConvertGatewayParameterDataToXml(), payGatewayUrl));
        }

        public PaymentResult BarcodePayment()
        {
            InitBarcodePaymentParameter();
            return GetBarcodePaymentResult(PostOrder(ConvertGatewayParameterDataToXml(), microPayGatewayUrl));
        }

        public bool CancelOrder(int retries = 0)
        {
            if (retries > 10) return false;

            InitQueryOrderParameter();
            return CheckCancelResult(PostOrder(ConvertGatewayParameterDataToXml(), reverseGatewayUrl), retries);
        }

        public bool QueryNow()
        {
            InitQueryOrderParameter();
            return CheckQueryResult(PostOrder(ConvertGatewayParameterDataToXml(), queryGatewayUrl));
        }

        public PaymentResult QueryForResult()
        {
            InitQueryOrderParameter();
            return ParseQueryResult(PostOrder(ConvertGatewayParameterDataToXml(), queryGatewayUrl));
        }

        /// <summary>
        /// ��ʼ��֧�������Ĳ���
        /// </summary>
        private void InitPaymentOrderParameter()
        {          
            SetGatewayParameterValue("appid", Merchant.AppId);
            SetGatewayParameterValue("mch_id", Merchant.UserName);
            SetGatewayParameterValue("nonce_str", GenerateNonceString());
            SetGatewayParameterValue("body", Order.Subject);
            SetGatewayParameterValue("out_trade_no", Order.Id);
            SetGatewayParameterValue("total_fee", (Order.Amount * 100).ToString());
            SetGatewayParameterValue("spbill_create_ip", "127.0.0.1");
            SetGatewayParameterValue("notify_url", Merchant.NotifyUrl.ToString());
            SetGatewayParameterValue("trade_type", "NATIVE");
            SetGatewayParameterValue("product_id", Order.Id);
            SetGatewayParameterValue("sign", GetSign());    // ǩ����Ҫ��������ã�����ȱ�ٲ�����
        }

        private void InitBarcodePaymentParameter()
        {
            SetGatewayParameterValue("appid", Merchant.AppId);
            SetGatewayParameterValue("mch_id", Merchant.UserName);
            SetGatewayParameterValue("nonce_str", GenerateNonceString());
            SetGatewayParameterValue("body", Order.Subject);
            SetGatewayParameterValue("out_trade_no", Order.Id);
            SetGatewayParameterValue("total_fee", (Order.Amount * 100).ToString());
            SetGatewayParameterValue("spbill_create_ip", "127.0.0.1");
            SetGatewayParameterValue("sign", GetSign());    // ǩ����Ҫ��������ã�����ȱ�ٲ�����
        }

        /// <summary>
        /// ��ʼ����ѯ��������
        /// </summary>
        private void InitQueryOrderParameter()
        {
            SetGatewayParameterValue("appid", Merchant.AppId);
            SetGatewayParameterValue("mch_id", Merchant.UserName);
            SetGatewayParameterValue("out_trade_no", Order.Id);
            SetGatewayParameterValue("nonce_str", GenerateNonceString());
            SetGatewayParameterValue("sign", GetSign());    // ǩ����Ҫ��������ã�����ȱ�ٲ�����
        }

        private void ReadNotifyOrderParameter()
        {
            Order.Id = GetGatewayParameterValue("out_trade_no");
            Order.Amount = Convert.ToInt32(GetGatewayParameterValue("total_fee")) * 0.01;
        }


        /// <summary>
        /// ��������ַ���
        /// </summary>
        /// <returns></returns>
        private string GenerateNonceString()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }


        /// <summary>
        /// ����������ת����XML
        /// </summary>
        /// <returns></returns>
        private string ConvertGatewayParameterDataToXml()
        {
            StringBuilder xmlBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(xmlBuilder, settings))
            {
                writer.WriteStartElement("xml");
                foreach (var item in GetSortedGatewayParameter())
                {
                    writer.WriteElementString(item.Key, item.Value);
                }
                writer.WriteEndElement();
                writer.Flush();
            }

            return xmlBuilder.ToString();
        }


        /// <summary>
        /// ���ǩ��
        /// </summary>
        /// <returns></returns>
        private string GetSign()
        {
            StringBuilder signBuilder = new StringBuilder();
            foreach (var item in GetSortedGatewayParameter())
            {
                // ��ֵ�Ĳ�����sign����������ǩ��
                if (!string.IsNullOrEmpty(item.Value) && string.Compare("sign", item.Key) != 0)
                {
                    signBuilder.AppendFormat("{0}={1}&", item.Key, item.Value);
                }
            }

            signBuilder.Append("key=" + Merchant.Key);
            return Utility.GetMD5(signBuilder.ToString());
        }


        /// <summary>
        /// �ύ����
        /// </summary>
        /// <param name="orderXml">������XML����</param>
        /// <param name="gatewayUrl">����URL</param>
        /// <returns></returns>
        private string PostOrder(string orderXml, string gatewayUrl)
        {
            byte[] dataByte = Encoding.UTF8.GetBytes(orderXml);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(gatewayUrl);
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = dataByte.Length;

            try
            {
                using (Stream outStream = request.GetRequestStream())
                {
                    outStream.Write(dataByte, 0, dataByte.Length);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        if (reader != null)
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                request.Abort();
            }

            return string.Empty;
        }


        /// <summary>
        /// ���΢��֧����URL
        /// </summary>
        /// <param name="resultXml">�����������ص�����</param>
        /// <returns></returns>
        private string GetWeixinPaymentUrl(string resultXml)
        {
            // ��Ҫ�����֮ǰ���������Ĳ����������Խ��յ��Ĳ�����ɸ��š�
            ClearGatewayParameterData();
            ReadResultXml(resultXml);
            if (IsSuccessResult())
            {
                return GetGatewayParameterValue("code_url");
            }
            else
            {
                WriteErrorLog("GetWeixinPaymentUrl", resultXml);
            }

            return string.Empty;
        }

        
        /// <summary>
        /// �Ƿ��ǳɹ��Ľ��
        /// </summary>
        /// <param name="parma"></param>
        /// <returns></returns>
        private bool IsSuccessResult()
        {
            if (ValidateResult() && ValidateSign())
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// ��֤���صĽ��
        /// </summary>
        /// <returns></returns>
        private bool ValidateResult()
        {
            if (string.Compare(GetGatewayParameterValue("return_code"), "SUCCESS") == 0 && 
                string.Compare(GetGatewayParameterValue("result_code"), "SUCCESS") == 0)
            {
                return true;
            }

            return false;

        }


        /// <summary>
        /// ��֤ǩ��
        /// </summary>
        /// <returns></returns>
        private bool ValidateSign()
        {
            if (string.Compare(GetGatewayParameterValue("sign"), GetSign()) == 0)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// ����ѯ���
        /// </summary>
        /// <param name="resultXml">��ѯ�����XML</param>
        /// <returns></returns>
        private bool CheckQueryResult(string resultXml)
        {
            // ��Ҫ�����֮ǰ��ѯ�����Ĳ����������Խ��յ��Ĳ�����ɸ��š�
            ClearGatewayParameterData();
            ReadResultXml(resultXml);
            if (IsSuccessResult())
            {
               if(string.Compare(Order.Id, GetGatewayParameterValue("out_trade_no")) == 0 &&
                  string.Compare("SUCCESS", GetGatewayParameterValue("trade_state"), true) == 0)
               {
                   return true;
               }
            }

            return false;
        }

        /// <summary>
        /// ���֧�����
        /// </summary>
        /// <param name="resultXml">֧�������XML</param>
        /// <returns></returns>
        private bool CheckPaymentResult(string resultXml)
        {
            // ��Ҫ�����֮ǰ��ѯ�����Ĳ����������Խ��յ��Ĳ�����ɸ��š�
            ClearGatewayParameterData();
            ReadResultXml(resultXml);
            if (IsSuccessResult())
            {
                if (string.Compare(Order.Id, GetGatewayParameterValue("out_trade_no")) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// ��鳷�����
        /// </summary>
        /// <param name="resultXml">���������XML</param>
        /// <returns></returns>
        private bool CheckCancelResult(string resultXml, int retries)
        {
            // ��Ҫ�����֮ǰ��ѯ�����Ĳ����������Խ��յ��Ĳ�����ɸ��š�
            ClearGatewayParameterData();
            ReadResultXml(resultXml);

            if (GetGatewayParameterValue("return_code") != "SUCCESS")
                return false;

            if (GetGatewayParameterValue("result_code") == "SUCCESS" && GetGatewayParameterValue("recall") == "N")
            {
                return true;
            }
            else if (GetGatewayParameterValue("recall") == "Y")
            {
                return CancelOrder(++retries);
            }

            return false;
        }

        private PaymentResult ParseQueryResult(string resultXml)
        {
            PaymentResult result = new PaymentResult();
            result.SuccessFlag = 2;

            if (CheckQueryResult(resultXml))
            {
                result.SuccessFlag = 1;
                result.TradeNo = GetGatewayParameterValue("transaction_id");
                result.Amount = GetGatewayParameterValue("total_fee");
                result.PaidAmount = GetGatewayParameterValue("total_fee");
                result.Currency = GetGatewayParameterValue("fee_type");
                return result;
            }
            else
            {
                string errorCode = GetGatewayParameterValue("err_code");
                if (errorCode == "ORDERNOTEXIST")
                {
                    result.SuccessFlag = 0;
                }
                WriteErrorLog("ParseQueryResult", resultXml);
            }

            return null;
        }

        private PaymentResult GetBarcodePaymentResult(string resultXml)
        {
            if (CheckPaymentResult(resultXml))
            {
                PaymentResult result = new PaymentResult();
                result.TradeNo = GetGatewayParameterValue("transaction_id");
                result.Amount = GetGatewayParameterValue("total_fee");
                result.PaidAmount = GetGatewayParameterValue("total_fee");
                result.Currency = GetGatewayParameterValue("fee_type");
                return result;
            }
            else
            {
                WriteErrorLog("GetBarcodePaymentResult", resultXml);

                if (GetGatewayParameterValue("return_code") == "FAIL")
                    throw new Exception(GetGatewayParameterValue("return_msg"));

                string errorCode = GetGatewayParameterValue("err_code");
                string errorMsg = GetGatewayParameterValue("err_code_des");
                if (errorCode == "USERPAYING" || errorCode == "SYSTEMERROR")
                {
                    PaymentResult queryResult = LoopQuery();
                    if (queryResult != null)
                    {
                        return queryResult;
                    }
                    else
                    {
                        if (!CancelOrder())
                            throw new Exception(resultXml);
                    }
                }

                throw new Exception(errorMsg);
            }
        }

        private PaymentResult LoopQuery()
        {
            Order.Id = GetGatewayParameterValue("out_trade_no");
            
            //ȷ��֧���Ƿ�ɹ�,ÿ��һ��ʱ���ѯһ�ζ���������ѯ10��
            int queryTimes = 10;//��ѯ����������
            while (queryTimes-- > 0)
            {
                PaymentResult result = QueryForResult();
                //�����Ҫ������ѯ����ȴ�3s�����
                if (result != null && result.SuccessFlag == 2)
                {
                    Thread.Sleep(3000);
                    continue;
                }
                //��ѯ�ɹ�,���ض�����ѯ�ӿڷ��ص�����
                else if (result != null && result.SuccessFlag == 1)
                {
                    return result;
                }
                //��������ʧ�ܣ�ֱ�ӷ���ˢ��֧���ӿڷ��صĽ����ʧ��ԭ�����err_code������
                else
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// ������ص�����
        /// </summary>
        private void ClearGatewayParameterData()
        {
            GatewayParameterData.Clear();
        }

        
        /// <summary>
        /// ��ʼ����ʾ�ѳɹ����յ�֧��֪ͨ������
        /// </summary>
        private void InitProcessSuccessParameter()
        {
            SetGatewayParameterValue("return_code", "SUCCESS");
        }


        public override void WriteSucceedFlag()
        {
            // ��Ҫ�����֮ǰ���յ���֪ͨ�Ĳ��������������ɱ�־�ɹ����յ�֪ͨ��XML��ɸ��š�
            ClearGatewayParameterData();
            InitProcessSuccessParameter();
            HttpContext.Current.Response.Write(ConvertGatewayParameterDataToXml());
        }

        private void WriteErrorLog(string method, string xml)
        {
            logger.Info(method + Environment.NewLine);
            logger.Info(string.Format("<============{0}============>", "XML��Ϣ") + Environment.NewLine);
            logger.Info(string.Format("{0}", xml) + Environment.NewLine);
            logger.Info("<==================================>" + Environment.NewLine);
        }
    }
}
