using AutoMapper;
using ICanPay;
using ICanPay.Configs;
using Mi9Pay.DataModel;
using Mi9Pay.Entities;
using Mi9Pay.Service.Helper;
using Mi9Pay.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace Mi9Pay.Service
{
    public partial class GatewayService : IGatewayService
    {
        private const int Multiplicator = 1000000;
        private const int AmountMultiplicator = 100;        

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

            requestParameter.Add("sign", requestSign);
        }

        public OrderRequest RecieveRequestForm(Dictionary<string, string> requestParameter)
        {
            OrderRequest request = new OrderRequest();

            request.AppId = requestParameter["app_id"];
            //request.InvoiceNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString(); // For test
            request.InvoiceNumber = requestParameter["invoice"];
            if (requestParameter.Keys.Contains("store_id"))
                request.StoreId = Convert.ToInt32(requestParameter["store_id"]);
            else
                request.StoreId = ParseStoreId(requestParameter["invoice"]);
            request.Currency = requestParameter["currency"];
            request.TotalAmount = Convert.ToDecimal(requestParameter["amount"]) / AmountMultiplicator;
            if (requestParameter.Keys.Contains("discount"))
                request.Discount = Convert.ToDecimal(requestParameter["discount"]) / AmountMultiplicator;
            if (requestParameter.Keys.Contains("shipping_fee"))
                request.ShippingFee = Convert.ToDecimal(requestParameter["shipping_fee"]) / AmountMultiplicator;
            if (requestParameter.Keys.Contains("tax"))
                request.Tax = Convert.ToDecimal(requestParameter["tax"]) / AmountMultiplicator;
            
            if (requestParameter.Keys.Contains("s_a_first_name") && requestParameter.Keys.Contains("s_a_last_name"))
            {
                request.Customer = new PaymentOrderCustomer
                {
                    FirstName = requestParameter["s_a_first_name"],
                    LastName = requestParameter["s_a_last_name"],
                    Country = requestParameter.Keys.Contains("s_a_country") ? requestParameter["s_a_country"] : string.Empty,
                    State = requestParameter.Keys.Contains("s_a_state") ? requestParameter["s_a_state"] : string.Empty,
                    City = requestParameter.Keys.Contains("s_a_city") ? requestParameter["s_a_city"] : string.Empty,
                    District = requestParameter.Keys.Contains("s_a_district") ? requestParameter["s_a_district"] : string.Empty,
                    Street = requestParameter.Keys.Contains("s_a_street") ? requestParameter["s_a_street"] : string.Empty,
                    ZipCode = requestParameter.Keys.Contains("s_a_zipcode") ? requestParameter["s_a_zipcode"] : string.Empty,
                    PhoneNumber = requestParameter.Keys.Contains("s_a_phone") ? requestParameter["s_a_phone"] : string.Empty,
                    Address = requestParameter.Keys.Contains("s_a_district") ? requestParameter["s_a_district"] : string.Empty
                };
            }

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
                    PaymentOrderDetail item = new PaymentOrderDetail
                    {
                      ItemName = requestParameter[itemNameKey],
                      ItemQty = Convert.ToDecimal(requestParameter[itemQtyKey]),
                      ItemAmount = Convert.ToDecimal(requestParameter[itemAmountKey]) / AmountMultiplicator
                    };
                    request.PayItems.Add(item);
                }
            }

            request.Signature = requestParameter["sign"];
            request.DoneUrl = requestParameter["done_url"];
            request.NotifyUrl = requestParameter["notify_url"];
            if (requestParameter.Keys.Contains("notify_dataformat"))
                request.NotifyDataFormat = requestParameter["notify_dataformat"];

            return request;
        }

        public MemoryStream CreatePaymentQRCode(OrderRequest orderRequest, GatewayType gatewayType, string cid)
        {
            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            SetPaymentSettingOrder(paymentSetting, orderRequest);

            MemoryStream ms = paymentSetting.PaymentQRCode();
            if (ms != null)
            {
                CreatePaymentOrder(orderRequest, gatewayType, paymentSetting.Order.Subject, cid);
            }
            return ms;
        }

        public OrderPaymentResponse BarcodePayment(OrderRequest orderRequest, GatewayType gatewayType, string barcode, string cid)
        {
            OrderPaymentResponse response = new OrderPaymentResponse();

            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            SetPaymentSettingOrder(paymentSetting, orderRequest);
            paymentSetting.SetGatewayParameterValue("auth_code", barcode);

            string invoiceNumber = orderRequest.InvoiceNumber;
            CreatePaymentOrder(orderRequest, gatewayType, paymentSetting.Order.Subject, cid);

            PaymentResult result = paymentSetting.BarcodePayment();
            if (result != null)
            {
                UpdatePaymentOrder(invoiceNumber, gatewayType, result.TradeNo, cid);

                result.InvoiceNo = invoiceNumber;
                response = BuildOrderPaymentResponse(result);
            }

            return response;
        }

        public OrderPaymentResponse QueryPayment(string appId, string invoiceNumber, GatewayType gatewayType, string cid)
        {
            OrderPaymentResponse response = new OrderPaymentResponse();

            OrderRequest orderRequest = new OrderRequest { AppId = appId, StoreId = ParseStoreId(invoiceNumber) };
            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            paymentSetting.Order.Id = invoiceNumber;

            PaymentResult result = paymentSetting.QueryForResult();
            if (result != null)
            {
                UpdatePaymentOrder(invoiceNumber, gatewayType, result.TradeNo, cid);

                result.InvoiceNo = invoiceNumber;
                response = BuildOrderPaymentResponse(result);
            }
            return response;
        }

        private OrderPaymentResponse BuildOrderPaymentResponse(PaymentResult result)
        {
            return new OrderPaymentResponse
            {
                return_code = "SUCCESS",
                order = new OrderPayment
                {
                    notification_id = Guid.NewGuid().ToString(),
                    uuid = result.TradeNo,
                    invoice = result.InvoiceNo,
                    status = "PAID",
                    paid_amount = result.PaidAmount,
                    amount = result.Amount,
                    currency = result.Currency
                }
            };
        }

        public string BuildReturnUrl(OrderRequest request, OrderPaymentResponse response)
        {
            Dictionary<string, string> parameters = BuildUrlParameter(request, response);
            string queryString = SignatureUtil.CreateSortedParams(parameters);
            return string.Format("{0}?{1}", request.DoneUrl, queryString);
        }

        public void PaymentNotify(OrderRequest request, OrderPaymentResponse response)
        {
            if (string.IsNullOrWhiteSpace(request.NotifyUrl))
                return;

            DateTime sendDateTime = DateTime.Now;
            bool rawData = true;
            if (string.Compare(request.NotifyDataFormat, "json", true) == 0)
                rawData = false;

            Dictionary<string, string> parameters = BuildUrlParameter(request, response);
            string postData = rawData ? SignatureUtil.CreateSortedParams(parameters) : new JavaScriptSerializer().Serialize(parameters);
            bool notifySuccess = WebClientHelper.PostData(request.NotifyUrl, postData, rawData);

            NotifyQueue queue = new NotifyQueue
            {
                UniqueId = Guid.Parse(response.order.notification_id),
                OrderNumber = request.InvoiceNumber,
                NotifyUrl = request.NotifyUrl,
                PostData = postData,
                PostDataFormat = rawData ? NotifyDataFormat.RAW : NotifyDataFormat.JSON,
                SendDate = sendDateTime,
                LastSendDate = sendDateTime,
                ProcessedCount = 1,
                NextInterval = notifySuccess ? 0 : NotifyConfig.NotifyStrategy[1],
                Processed = notifySuccess ? "Y" : "N"
            };
            CreateNotifyQueue(queue);
        }

        private Dictionary<string, string> BuildUrlParameter(OrderRequest request, OrderPaymentResponse response)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("app_id", request.AppId);
            parameters.Add("invoice", request.InvoiceNumber);
            parameters.Add("currency", request.Currency);
            parameters.Add("notification_id", response.order.notification_id);
            parameters.Add("order_uuid", response.order.uuid);
            parameters.Add("status", response.order.status);
            parameters.Add("amount", response.order.amount);
            parameters.Add("paid_amount", response.order.paid_amount);
            parameters.Add("payment_date", DateTime.Now.ToString("yyyyMMddHHmmss"));

            GatewayPaymentApp app = GetGatewayPaymentApp(request.AppId);
            string signString = SignatureUtil.CreateSortedParams(parameters);
            parameters.Add("sign", SignatureUtil.CreateSignature(signString + app.Appkey));

            return parameters;
        }

        public IEnumerable<GatewayType> GetGatewayTypes(string invoice)
        {
            int storeId = ParseStoreId(invoice);
            IEnumerable<PaymentMethod> paymentMethods = GetPaymentMethodList(storeId);

            List<GatewayType> gatewayTypes = new List<GatewayType>();
            paymentMethods.ToList().ForEach(m => {
                GatewayType gatewayType;
                if (Enum.TryParse(m.Code, out gatewayType))
                    gatewayTypes.Add(gatewayType);
            });

            return gatewayTypes;
        }

        public IEnumerable<PaymentMethod> GetPaymentMethodList(int storeId)
        {
            List<GatewayPaymentMethodTypeJoinResult> paymentMethodCombinations = GetPaymentMethodCombinations(storeId).ToList();
            List<GatewayPaymentMethod> storePaymentMethods = new List<GatewayPaymentMethod>();
            paymentMethodCombinations.ForEach(x => {
                storePaymentMethods.Add(x.PaymentCombine.GatewayPaymentMethod1);
            });

            Mapper.Initialize(cfg => cfg.CreateMap<GatewayPaymentMethod, PaymentMethod>());
            List<PaymentMethod> listPaymentMethod = Mapper.Map<List<GatewayPaymentMethod>, List<PaymentMethod>>(storePaymentMethods);
            return listPaymentMethod;
        }

        public IEnumerable<PaymentScanMode> GetPaymentScanModeList()
        {
            List<PaymentScanMode> listScanMode = new List<PaymentScanMode>();

            listScanMode.Add(new PaymentScanMode { Code = "qrcode", Name = "二维码", IsDefault = true });
            listScanMode.Add(new PaymentScanMode { Code = "barcode", Name = "支付条码", IsDefault = false });

            return listScanMode;
        }

        public IEnumerable<PaymentCombine> GetPaymentCombineList(int storeId)
        {
            List<GatewayPaymentMethodTypeJoinResult> paymentMethodCombinations = GetPaymentMethodCombinations(storeId).ToList();
            
            List<PaymentCombine> listPaymentCombine = new List<PaymentCombine>();
            paymentMethodCombinations.ForEach(x => {
                PaymentCombine pc = new PaymentCombine
                {
                    CombineId = x.PaymentCombine.UniqueId.ToString(),
                    StorePaymentMethod = x.StorePaymentMethod.ToString(),
                    PaymentMethod = new PaymentMethod { Code = x.PaymentCombine.GatewayPaymentMethod1.Code, Name = x.PaymentCombine.GatewayPaymentMethod1.Name },
                    PaymentScanMode = new PaymentScanMode { Code = x.PaymentCombine.GatewayPaymentMethodType1.Code, Name = x.PaymentCombine.GatewayPaymentMethodType1.Name },
                    IsDefault = x.IsDefault
                };
                listPaymentCombine.Add(pc);
            });

            return listPaymentCombine.OrderBy(x => x.PaymentMethod.Code + x.PaymentScanMode.Code);
        }

        private int ParseStoreId(string invoiceNumber)
        {
            int invoice;

            if (int.TryParse(invoiceNumber, out invoice))
                return invoice / Multiplicator;

            return -1;
        }

        private PaymentSetting InitPaymentSetting(OrderRequest orderRequest, GatewayType gatewayType, GatewayPaymentAccount account = null)
        {
            GatewayPaymentAccount paymentAccount = account;
            if (paymentAccount == null)
                paymentAccount = GetGatewayPaymentAccount(orderRequest.StoreId, gatewayType);

            PaymentSetting paymentSetting = new PaymentSetting(gatewayType);
            //paymentSetting.SetGatewayParameterValue("appid", account.Appid);
            paymentSetting.Merchant.AppId = paymentAccount.Appid;
            paymentSetting.Merchant.UserName = paymentAccount.Mchid;
            paymentSetting.Merchant.Key = paymentAccount.Mchkey;
            paymentSetting.Merchant.PublicKey = paymentAccount.Publickey;

            if (!string.IsNullOrWhiteSpace(orderRequest.NotifyUrl))
                paymentSetting.Merchant.NotifyUrl = new Uri(orderRequest.NotifyUrl);

            if (gatewayType == GatewayType.Alipay)
                paymentSetting.SetGatewayParameterValue("storeid", orderRequest.StoreId.ToString());

            return paymentSetting;
        }

        private void SetPaymentSettingOrder(PaymentSetting paymentSetting, OrderRequest orderRequest)
        {
            paymentSetting.Order.Id = orderRequest.InvoiceNumber;
            paymentSetting.Order.Subject = "MPOS订单编号" + orderRequest.InvoiceNumber;
            paymentSetting.Order.Amount = (double)orderRequest.TotalAmount;
            paymentSetting.Order.DiscountAmount = (double)orderRequest.Discount;
        }

        private void CreatePaymentOrder(OrderRequest orderRequest, GatewayType gatewayType, string orderSubject, string cid)
        {
            if (!PaymentOrderExisted(orderRequest.InvoiceNumber, orderRequest.StoreId, gatewayType, cid))
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<OrderRequest, PaymentOrder>();
                });
                PaymentOrder paymentOrder = Mapper.Map<OrderRequest, PaymentOrder>(orderRequest);
                if (paymentOrder != null)
                {
                    paymentOrder.OrderType = "MOSAIC";
                    paymentOrder.Subject = orderSubject;
                    paymentOrder.GatewayType = gatewayType;
                    paymentOrder.StorePaymentMethod = Guid.Parse(cid);
                    paymentOrder.Status = PaymentOrderStatus.UNPAID;
                    CreatePaymentOrder(paymentOrder);
                }
            }
        }

        private void UpdatePaymentOrder(string invoiceNumber, GatewayType gatewayType, string tradeNo, string cid)
        {
            PaymentOrder paymentOrder = new PaymentOrder
            {
                InvoiceNumber = invoiceNumber,
                GatewayType = gatewayType,
                StorePaymentMethod = Guid.Parse(cid),
                TradeNumber = tradeNo,
                Status = PaymentOrderStatus.PAID
            };
            UpdatePaymentOrder(paymentOrder);
        }
    }
}
