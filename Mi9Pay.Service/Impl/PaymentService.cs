using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mi9Pay.Entities;
using Mi9Pay.DataModel;
using Mi9Pay.PayProvider;

namespace Mi9Pay.Service
{
    public partial class GatewayService
    {
        public int DownloadBill(string merchantCode, string[] storeIdArray, string billDate, GatewayType gatewayType)
        {
            if (storeIdArray == null || storeIdArray.Length < 1)
                return 0;

            int dataCount = 0;
            foreach (string storeId in storeIdArray)
            {
                if (string.IsNullOrWhiteSpace(storeId)) continue;

                dataCount += DownloadBill(merchantCode, storeId, billDate, gatewayType);
            }

            return dataCount;
        }

        private int DownloadBill(string merchantCode, string storeId, string billDate, GatewayType gatewayType)
        {
            OrderRequest orderRequest = BuildOrderRequest(merchantCode, storeId);

            GatewayPaymentAccount paymentAccount = GetGatewayPaymentAccount(orderRequest, gatewayType, true);
            if (paymentAccount == null)
                return 0;

            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType, paymentAccount);
            paymentSetting.Bill.BillDate = billDate;

            string billData = paymentSetting.QueryBill();

            int dataCount = 0;
            if (!string.IsNullOrEmpty(billData))
            {
                BillService billService = null;
                switch (gatewayType)
                {
                    case GatewayType.Alipay:
                        billService = new BillServiceAlipay();
                        break;
                    case GatewayType.WeChat:
                        billService = new BillServiceWechat();
                        break;
                }
                if (billService != null)
                {
                    billService.Repository = _repository;
                    billService.Merchant = orderRequest.Merchant.UniqueId;
                    billService.StoreId = storeId;
                    billService.BillDate = billDate;

                    dataCount = billService.DownloadBill(billData);
                }
            }
            return dataCount;
        }

        public void RefundPayment(string merchantCode, string storeId, OrderRefundRequest refundRequest, GatewayType gatewayType)
        {
            ValidatePaymentOrderStatus(refundRequest.InvoiceNo, refundRequest.TradeNo);

            OrderRequest orderRequest = BuildOrderRequest(merchantCode, storeId);
            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            paymentSetting.Order.Id = refundRequest.InvoiceNo;
            paymentSetting.Order.TradeNo = refundRequest.TradeNo;
            paymentSetting.Order.RefundRequestNo = "RF" + refundRequest.InvoiceNo;
            paymentSetting.Order.Amount = double.Parse(refundRequest.RefundAmount);
            paymentSetting.Order.RefundReason = refundRequest.RefundReason;

            bool success = paymentSetting.RefundPayment();
            if (success)
                UpdatePaymentOrderStatus(refundRequest.InvoiceNo, refundRequest.TradeNo, PaymentOrderStatus.REFUND);

            throw new Exception("退款失败");
        }

        private OrderRequest BuildOrderRequest(string merchantCode, string storeId)
        {
            return new OrderRequest
            {
                StoreId = Convert.ToInt32(storeId),
                Merchant = new PaymentOrderMerchant
                {
                    UniqueId = GetGatewayPaymentMerchant(merchantCode).UniqueId
                }
            };
        }

    }
}
