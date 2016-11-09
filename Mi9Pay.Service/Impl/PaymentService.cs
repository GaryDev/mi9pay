using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICanPay;
using Mi9Pay.Entities;
using Mi9Pay.DataModel;

namespace Mi9Pay.Service
{
    public partial class GatewayService
    {
        public int DownloadBill(string[] storeIdArray, string billDate, GatewayType gatewayType)
        {
            if (storeIdArray == null || storeIdArray.Length < 1)
                return 0;

            int dataCount = 0;
            foreach (string storeId in storeIdArray)
            {
                if (string.IsNullOrWhiteSpace(storeId)) continue;

                dataCount += DownloadBill(storeId, billDate, gatewayType);
            }

            return dataCount;
        }

        private int DownloadBill(string storeId, string billDate, GatewayType gatewayType)
        {
            int id = Convert.ToInt32(storeId);
            GatewayPaymentAccount paymentAccount = GetGatewayPaymentAccount(id, gatewayType);
            if (paymentAccount == null)
                return 0;

            OrderRequest orderRequest = new OrderRequest { StoreId = id };
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
                    billService.StoreId = storeId;
                    billService.BillDate = billDate;

                    dataCount = billService.DownloadBill(billData);
                }
            }
            return dataCount;
        }
        
    }
}
