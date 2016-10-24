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
    /// 微信支付网关
    /// </summary>
    /// <remarks>
    /// 使用模式二实现微信支付
    /// </remarks>
    public sealed class WeChatPaymentGataway : GatewayBase, IPaymentWithCode, IQueryNow
    {

        #region 私有字段

        private static Logger logger = LogManager.GetCurrentClassLogger();

        const string payGatewayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        const string queryGatewayUrl = "https://api.mch.weixin.qq.com/pay/orderquery";
        const string microPayGatewayUrl = "https://api.mch.weixin.qq.com/pay/micropay";
        const string reverseGatewayUrl = "https://api.mch.weixin.qq.com/secapi/pay/reverse";

        #endregion


        #region 构造函数

        /// <summary>
        /// 初始化微信支付网关
        /// </summary>
        public WeChatPaymentGataway()
        {
        }


        /// <summary>
        /// 初始化微信支付网关
        /// </summary>
        /// <param name="gatewayParameterData">网关通知的数据集合</param>
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
        /// 初始化支付订单的参数
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
            SetGatewayParameterValue("sign", GetSign());    // 签名需要在最后设置，以免缺少参数。
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
            SetGatewayParameterValue("sign", GetSign());    // 签名需要在最后设置，以免缺少参数。
        }

        /// <summary>
        /// 初始化查询订单参数
        /// </summary>
        private void InitQueryOrderParameter()
        {
            SetGatewayParameterValue("appid", Merchant.AppId);
            SetGatewayParameterValue("mch_id", Merchant.UserName);
            SetGatewayParameterValue("out_trade_no", Order.Id);
            SetGatewayParameterValue("nonce_str", GenerateNonceString());
            SetGatewayParameterValue("sign", GetSign());    // 签名需要在最后设置，以免缺少参数。
        }

        private void ReadNotifyOrderParameter()
        {
            Order.Id = GetGatewayParameterValue("out_trade_no");
            Order.Amount = Convert.ToInt32(GetGatewayParameterValue("total_fee")) * 0.01;
        }


        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns></returns>
        private string GenerateNonceString()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }


        /// <summary>
        /// 将网关数据转换成XML
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
        /// 获得签名
        /// </summary>
        /// <returns></returns>
        private string GetSign()
        {
            StringBuilder signBuilder = new StringBuilder();
            foreach (var item in GetSortedGatewayParameter())
            {
                // 空值的参数与sign参数不参与签名
                if (!string.IsNullOrEmpty(item.Value) && string.Compare("sign", item.Key) != 0)
                {
                    signBuilder.AppendFormat("{0}={1}&", item.Key, item.Value);
                }
            }

            signBuilder.Append("key=" + Merchant.Key);
            return Utility.GetMD5(signBuilder.ToString());
        }


        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderXml">订单的XML内容</param>
        /// <param name="gatewayUrl">网关URL</param>
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
        /// 获得微信支付的URL
        /// </summary>
        /// <param name="resultXml">创建订单返回的数据</param>
        /// <returns></returns>
        private string GetWeixinPaymentUrl(string resultXml)
        {
            // 需要先清除之前创建订单的参数，否则会对接收到的参数造成干扰。
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
        /// 是否是成功的结果
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
        /// 验证返回的结果
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
        /// 验证签名
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
        /// 检查查询结果
        /// </summary>
        /// <param name="resultXml">查询结果的XML</param>
        /// <returns></returns>
        private bool CheckQueryResult(string resultXml)
        {
            // 需要先清除之前查询订单的参数，否则会对接收到的参数造成干扰。
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
        /// 检查支付结果
        /// </summary>
        /// <param name="resultXml">支付结果的XML</param>
        /// <returns></returns>
        private bool CheckPaymentResult(string resultXml)
        {
            // 需要先清除之前查询订单的参数，否则会对接收到的参数造成干扰。
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
        /// 检查撤销结果
        /// </summary>
        /// <param name="resultXml">撤销结果的XML</param>
        /// <returns></returns>
        private bool CheckCancelResult(string resultXml, int retries)
        {
            // 需要先清除之前查询订单的参数，否则会对接收到的参数造成干扰。
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
            
            //确认支付是否成功,每隔一段时间查询一次订单，共查询10次
            int queryTimes = 10;//查询次数计数器
            while (queryTimes-- > 0)
            {
                PaymentResult result = QueryForResult();
                //如果需要继续查询，则等待3s后继续
                if (result != null && result.SuccessFlag == 2)
                {
                    Thread.Sleep(3000);
                    continue;
                }
                //查询成功,返回订单查询接口返回的数据
                else if (result != null && result.SuccessFlag == 1)
                {
                    return result;
                }
                //订单交易失败，直接返回刷卡支付接口返回的结果，失败原因会在err_code中描述
                else
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// 清除网关的数据
        /// </summary>
        private void ClearGatewayParameterData()
        {
            GatewayParameterData.Clear();
        }

        
        /// <summary>
        /// 初始化表示已成功接收到支付通知的数据
        /// </summary>
        private void InitProcessSuccessParameter()
        {
            SetGatewayParameterValue("return_code", "SUCCESS");
        }


        public override void WriteSucceedFlag()
        {
            // 需要先清除之前接收到的通知的参数，否则会对生成标志成功接收到通知的XML造成干扰。
            ClearGatewayParameterData();
            InitProcessSuccessParameter();
            HttpContext.Current.Response.Write(ConvertGatewayParameterDataToXml());
        }

        private void WriteErrorLog(string method, string xml)
        {
            logger.Info(method + Environment.NewLine);
            logger.Info(string.Format("<============{0}============>", "XML信息") + Environment.NewLine);
            logger.Info(string.Format("{0}", xml) + Environment.NewLine);
            logger.Info("<==================================>" + Environment.NewLine);
        }
    }
}
