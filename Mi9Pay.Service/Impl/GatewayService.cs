using AutoMapper;
using Mi9Pay.DataModel;
using Mi9Pay.Entities;
using Mi9Pay.PayProvider;
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

        public OrderRequest ValidateRequestParameter(Dictionary<string, string> requestParameter)
        {
            string appid = requestParameter["app_id"];
            GatewayPaymentApp app = GetGatewayPaymentApp(appid);
            string appKey = app.Appkey;

            string requestSign = requestParameter["sign"] as string;
            requestParameter.Remove("sign");

            string sortedString = SignatureUtil.CreateSortedParams(requestParameter);
            string serverSign = SignatureUtil.CreateSignature(sortedString + appKey);
            //string serverSign = requestSign;    // For test

            if (requestSign != serverSign)
                throw new Exception("签名验证失败");

            requestParameter.Add("sign", requestSign);

            OrderRequest request = new OrderRequest();
            request.AppId = appid;
            //request.InvoiceNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString(); // For test
            request.InvoiceNumber = requestParameter["invoice"];
            if (requestParameter.Keys.Contains("store_id"))
                request.StoreId = Convert.ToInt32(requestParameter["store_id"]);
            else
                request.StoreId = ParseStoreId(requestParameter["invoice"]);

            request.Merchant = new PaymentOrderMerchant
            {
                AppId = appid
            };
            GatewayPaymentMerchant merchant = app.GatewayPaymentMerchant1;
            if (merchant != null)
            {
                request.Merchant.UniqueId = merchant.UniqueId;
                request.Merchant.Code = merchant.Code;
                request.Merchant.Name = merchant.Name;
            }
            return request;
        }

        public OrderRequest RecieveRequestForm(Dictionary<string, string> requestParameter)
        {
            OrderRequest request = ValidateRequestParameter(requestParameter);
            
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

        public MemoryStream CreatePaymentQRCode(OrderRequest orderRequest, GatewayType gatewayType)
        {
            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            SetPaymentSettingOrder(paymentSetting, orderRequest);

            MemoryStream ms = paymentSetting.PaymentQRCode();
            if (ms != null)
            {
                SavePaymentOrder(orderRequest, paymentSetting.Order.Subject);
            }
            return ms;
        }

        public OrderPaymentResponse BarcodePayment(OrderRequest orderRequest, GatewayType gatewayType, string barcode)
        {
            OrderPaymentResponse response = new OrderPaymentResponse();

            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            SetPaymentSettingOrder(paymentSetting, orderRequest);
            paymentSetting.SetGatewayParameterValue("auth_code", barcode);

            string invoiceNumber = orderRequest.InvoiceNumber;
            SavePaymentOrder(orderRequest, paymentSetting.Order.Subject);

            PaymentResult result = paymentSetting.BarcodePayment();
            if (result != null)
            {
                CompletePaymentOrder(orderRequest, result.TradeNo);

                result.InvoiceNo = invoiceNumber;
                response = BuildOrderPaymentResponse(result);
            }

            return response;
        }

        public OrderPaymentResponse QueryPayment(OrderRequest orderRequest, GatewayType gatewayType)
        {
            OrderPaymentResponse response = new OrderPaymentResponse();

            string invoiceNumber = orderRequest.InvoiceNumber;
            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            paymentSetting.Order.Id = invoiceNumber;

            PaymentResult result = paymentSetting.QueryForResult();
            if (result != null)
            {
                CompletePaymentOrder(orderRequest, result.TradeNo);

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
            bool rawData = false;
            if (string.IsNullOrWhiteSpace(request.NotifyDataFormat) || 
                string.Compare(request.NotifyDataFormat, "raw", true) == 0)
                rawData = true;

            Dictionary<string, string> parameters = BuildUrlParameter(request, response);
            string postData = rawData ? SignatureUtil.CreateSortedParams(parameters) : new JavaScriptSerializer().Serialize(parameters);

            NotifyAsyncParameter parameter = new NotifyAsyncParameter
            {
                NotifyPostInfo = new NotifyPostInfo { PostUrl = request.NotifyUrl, PostData = postData, IsRawData = rawData },
                NotifyQueue = new NotifyQueue
                {
                    UniqueId = Guid.Parse(response.order.notification_id),
                    OrderNumber = request.InvoiceNumber,
                    NotifyUrl = request.NotifyUrl,
                    PostData = postData,
                    PostDataFormat = rawData ? NotifyDataFormat.RAW : NotifyDataFormat.JSON,
                    SendDate = sendDateTime,
                    LastSendDate = sendDateTime,
                    ProcessedCount = 1
                },
                PostAction = CreateNotifyQueue
            };
            WebClientHelper.SendNotification(parameter);
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
            parameters.Add("payment_date", DateTime.Now.ToString("yyyy-MM-dd"));

            GatewayPaymentApp app = GetGatewayPaymentApp(request.AppId);
            string signString = SignatureUtil.CreateSortedParams(parameters);
            parameters.Add("sign", SignatureUtil.CreateSignature(signString + app.Appkey));

            return parameters;
        }

        public IEnumerable<PaymentMethod> GetPaymentMethodList(int storeId, Guid merchant)
        {
            List<GatewayPaymentMethodTypeJoinResult> paymentMethodCombinations = GetPaymentMethodCombinations(storeId, merchant).ToList();
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

        public IEnumerable<PaymentCombine> GetPaymentCombineList(int storeId, Guid merchant)
        {
            List<GatewayPaymentMethodTypeJoinResult> paymentMethodCombinations = GetPaymentMethodCombinations(storeId, merchant).ToList();
            
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
                paymentAccount = GetGatewayPaymentAccount(orderRequest, gatewayType);

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

        private void SavePaymentOrder(OrderRequest orderRequest, string orderSubject)
        {
            GatewayPaymentOrder order = GetGatewayPaymentOrder(orderRequest);
            if (order == null)
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<OrderRequest, PaymentOrder>();
                });
                PaymentOrder paymentOrder = Mapper.Map<OrderRequest, PaymentOrder>(orderRequest);
                if (paymentOrder != null)
                {
                    paymentOrder.OrderType = "MOSAIC";
                    paymentOrder.Subject = orderSubject;
                    paymentOrder.StorePaymentMethod = Guid.Parse(orderRequest.PaymentCombine);
                    paymentOrder.Status = PaymentOrderStatus.UNPAID;
                    CreatePaymentOrder(paymentOrder);
                }
            }
            else
            {
                order.GatewayPaymentStorePaymentMethod = Guid.Parse(orderRequest.PaymentCombine);
                UpdatePaymentOrder(order);
            }
        }

        private void CompletePaymentOrder(OrderRequest orderRequest, string tradeNo)
        {
            GatewayPaymentOrder order = GetGatewayPaymentOrder(orderRequest);
            if (order != null)
            {
                order.TradeNumber = tradeNo;
                order.GatewayPaymentOrderStatus = GetGatewayPaymentOrderStatus(PaymentOrderStatus.PAID).UniqueId;
                UpdatePaymentOrder(order);
            }
        }
    }
}
