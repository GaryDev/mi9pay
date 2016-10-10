﻿using AutoMapper;
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

            return request;
        }

        public MemoryStream CreatePaymentQRCode(OrderRequest orderRequest, GatewayType gatewayType)
        {
            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            paymentSetting.Order.Amount = (double)orderRequest.TotalAmount;
            paymentSetting.Order.Id = orderRequest.InvoiceNumber;
            string orderSubject = "MPOS订单编号" + orderRequest.InvoiceNumber;
            paymentSetting.Order.Subject = orderSubject;

            MemoryStream ms = paymentSetting.PaymentQRCode();
            if (ms != null)
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
                    paymentOrder.Status = PaymentOrderStatus.UNPAID;
                    CreatePaymentOrder(paymentOrder);
                }
            }
            return ms;
        }

        public OrderPaymentResponse QueryPayment(string appId, string invoiceNumber, GatewayType gatewayType)
        {
            OrderPaymentResponse response = new OrderPaymentResponse();

            OrderRequest orderRequest = new OrderRequest { AppId = appId, StoreId = ParseStoreId(invoiceNumber) };
            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            paymentSetting.Order.Id = invoiceNumber;

            QueryResult result = paymentSetting.QueryForResult();
            if (result != null)
            {
                PaymentOrder paymentOrder = new PaymentOrder
                {
                    InvoiceNumber = invoiceNumber,
                    GatewayType = gatewayType,
                    TradeNumber = result.TradeNo,
                    Status = PaymentOrderStatus.PAID
                };
                UpdatePaymentOrder(paymentOrder);

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

        public IEnumerable<PaymentMethod> GetPaymentMethods()
        {
            IEnumerable<GatewayPaymentMethod> paymentMethods = GetGatewayPaymentMethods();

            Mapper.Initialize(cfg => cfg.CreateMap<GatewayPaymentMethod, PaymentMethod>());
            return Mapper.Map<IEnumerable<GatewayPaymentMethod>, IEnumerable<PaymentMethod>>(paymentMethods);
        }

        private int ParseStoreId(string invoiceNumber)
        {
            int invoice;

            if (int.TryParse(invoiceNumber, out invoice))
                return invoice / Multiplicator;

            return -1;
        }

        private PaymentSetting InitPaymentSetting(OrderRequest orderRequest, GatewayType gatewayType)
        {
            GatewayPaymentAccount account = GetGatewayPaymentAccount(orderRequest.StoreId, gatewayType);

            PaymentSetting paymentSetting = new PaymentSetting(gatewayType);

            paymentSetting.SetGatewayParameterValue("appid", account.Appid);
            if (gatewayType == GatewayType.Alipay)
                paymentSetting.SetGatewayParameterValue("storeid", orderRequest.StoreId.ToString());

            paymentSetting.Merchant.UserName = account.Mchid;
            paymentSetting.Merchant.Key = account.Mchkey;
            paymentSetting.Merchant.PublicKey = account.Publickey;

            if (!string.IsNullOrWhiteSpace(orderRequest.NotifyUrl))
                paymentSetting.Merchant.NotifyUrl = new Uri(orderRequest.NotifyUrl);

            return paymentSetting;
        }
    }
}
