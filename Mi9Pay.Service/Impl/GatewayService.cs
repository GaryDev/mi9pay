using ICanPay;
using ICanPay.Configs;
using Mi9Pay.DataModel;
using Mi9Pay.Entities;
using Mi9Pay.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mi9Pay.Service
{
    public class GatewayService : IGatewayService
    {
        private readonly GatewayRepository _repository;

        public GatewayService(GatewayRepository repository)
        {
            _repository = repository;
        }

        public void ValidateRequestParameter(Dictionary<string, string> requestParameter)
        {
            string appid = requestParameter["app_id"];
            GatewayPaymentApp app = GetGatewayPaymentApp(appid);

            string requestSign = requestParameter["sign"] as string;
            requestParameter.Remove("sign");

            string sortedString = SignatureUtil.CreateSortedParams(requestParameter);
            string serverSign = SignatureUtil.CreateSignature(sortedString + app.Appkey);
            //string serverSign = requestSign;    // For test

            if (requestSign != serverSign)
                throw new Exception("签名验证失败");
        }

        public OrderRequest RecieveRequestForm(Dictionary<string, string> requestParameter)
        {
            OrderRequest request = new OrderRequest();

            request.AppId = requestParameter["app_id"];
            request.InvoiceNumber = requestParameter["invoice"] as string;
            //request.InvoiceNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString(); // For test
            request.TotalAmount = Convert.ToDecimal(requestParameter["amount"]) / 100;
            request.Currency = requestParameter["currency"];

            int itemCount = requestParameter.Keys.Count(k => k.StartsWith("item_"));
            for (int i = 0; i < itemCount; i++)
            {
                string itemNameKey = string.Format("item_{0}_name", i);
                string itemAmountKey = string.Format("item_{0}_amount", i);
                string itemQtyKey = string.Format("item_{0}_quantity", i);
                if (requestParameter.ContainsKey(itemNameKey) &&
                    requestParameter.ContainsKey(itemAmountKey) &&
                    requestParameter.ContainsKey(itemQtyKey))
                {
                    OrderItem item = new OrderItem
                    {
                      Name = requestParameter[itemNameKey],
                      Quantity = Convert.ToDecimal(requestParameter[itemQtyKey]),
                      Amount = Convert.ToDecimal(requestParameter[itemAmountKey])
                    };
                    request.PayItems.Add(item);
                }
            }

            request.DoneUrl = requestParameter["done_url"];
            request.NotifyUrl = requestParameter["notify_url"];

            return request;
        }

        public MemoryStream CreatePaymentQRCode(OrderRequest orderRequest, GatewayType gatewayType)
        {
            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            paymentSetting.Order.Amount = (double)orderRequest.TotalAmount;
            paymentSetting.Order.Id = orderRequest.InvoiceNumber;
            paymentSetting.Order.Subject = "MPOS订单编号" + orderRequest.InvoiceNumber;

            return paymentSetting.PaymentQRCode();
        }

        public OrderPaymentResponse QueryPayment(string appId, string invoiceNumber, GatewayType gatewayType)
        {
            OrderPaymentResponse response = new OrderPaymentResponse();

            PaymentSetting paymentSetting = InitPaymentSetting(new OrderRequest { AppId = appId }, gatewayType);
            paymentSetting.Order.Id = invoiceNumber;

            QueryResult result = paymentSetting.QueryForResult();
            if (result != null)
            {            
                response = new OrderPaymentResponse
                {
                    return_code = "SUCCESS",
                    order = new OrderPayment
                    {
                        uuid = result.TradeNo,
                        invoice = invoiceNumber,
                        status = "PAID",
                        paid_amount = result.PaidAmount,
                        amount = result.Amount,
                        currency = result.Currency
                    }
                };
            }
            return response;
        }

        public string BuildReturnUrl(OrderRequest request, OrderPaymentResponse response)
        {
            GatewayPaymentApp app = GetGatewayPaymentApp(request.AppId);

            Dictionary<string, string> parameters = BuildUrlParameter(request, response);
            string signString = SignatureUtil.CreateSortedParams(parameters);
            parameters.Add("sign", SignatureUtil.CreateSignature(signString + app.Appkey));

            string queryString = SignatureUtil.CreateSortedParams(parameters);
            return string.Format("{0}?{1}", request.DoneUrl, queryString);
        }

        private Dictionary<string, string> BuildUrlParameter(OrderRequest request, OrderPaymentResponse response)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("app_id", request.AppId);
            parameters.Add("notification_id", Guid.NewGuid().ToString());
            parameters.Add("invoice", request.InvoiceNumber);
            parameters.Add("currency", request.Currency);
            parameters.Add("order_uuid", response.order.uuid);
            parameters.Add("status", response.order.status);
            parameters.Add("amount", response.order.amount);
            parameters.Add("paid_amount", response.order.paid_amount);
            parameters.Add("payment_date", DateTime.Now.ToString("yyyyMMddHHmmss"));
            return parameters;
        }

        public IEnumerable<GatewayType> GetGatewayTypes()
        {
            IEnumerable<GatewayPaymentMethod> paymentMethods = GetGatewayPaymentMethods();

            List<GatewayType> gatewayTypes = new List<GatewayType>();
            paymentMethods.ToList().ForEach(m => {
                GatewayType gatewayType;
                if (Enum.TryParse(m.Code, out gatewayType))
                    gatewayTypes.Add(gatewayType);
            });

            return gatewayTypes;
        }

        private GatewayPaymentApp GetGatewayPaymentApp(string appId)
        {
            GatewayPaymentApp app = _repository.AppRepository.Get(x => x.Appid == appId);
            if (app == null)
                throw new Exception("无效的appid");

            return app;
        }

        private IEnumerable<GatewayPaymentMethod> GetGatewayPaymentMethods()
        {
            IEnumerable<GatewayPaymentMethod> paymentMethods = _repository.MethodRepository.GetAll();
            if (paymentMethods == null || paymentMethods.ToList().Count == 0)
                throw new Exception("未配置支付方式");

            return paymentMethods;
        }

        private GatewayPaymentAccount GetGatewayPaymentAccount(string appId, GatewayType gatewayType)
        {
            GatewayPaymentApp app = GetGatewayPaymentApp(appId);

            string payCode = Enum.GetName(typeof(GatewayType), gatewayType);
            GatewayPaymentMethod payMethod = GetGatewayPaymentMethods().ToList().FirstOrDefault(m => string.Compare(m.Code, payCode, true) == 0);
            if (payMethod == null)
                throw new Exception("支付方式获取失败");

            GatewayPaymentAccount account = app.GatewayPaymentAccount.FirstOrDefault(c => c.GatewayPaymentMethod == payMethod.UniqueId);
            if (account == null)
                throw new Exception("对应支付方式账号获取失败");

            return account;
        }

        private PaymentSetting InitPaymentSetting(OrderRequest orderRequest, GatewayType gatewayType)
        {
            GatewayPaymentAccount account = GetGatewayPaymentAccount(orderRequest.AppId, gatewayType);

            PaymentSetting paymentSetting = new PaymentSetting(gatewayType);
            paymentSetting.SetGatewayParameterValue("appid", account.Appid);
            paymentSetting.Merchant.UserName = account.Mchid;
            paymentSetting.Merchant.Key = account.Mchkey;
            paymentSetting.Merchant.PublicKey = account.Publickey;
            if (!string.IsNullOrWhiteSpace(orderRequest.NotifyUrl))
                paymentSetting.Merchant.NotifyUrl = new Uri(orderRequest.NotifyUrl);

            return paymentSetting;
        }
    }
}
