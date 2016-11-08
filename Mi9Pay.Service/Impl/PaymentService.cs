using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICanPay;
using Mi9Pay.Entities;

namespace Mi9Pay.Service
{
    public partial class GatewayService
    {
        public int DownloadBill(string storeId, string billDate, GatewayType gatewayType)
        {
            OrderRequest orderRequest = new OrderRequest { StoreId = Convert.ToInt32(storeId) };
            PaymentSetting paymentSetting = InitPaymentSetting(orderRequest, gatewayType);
            paymentSetting.Bill.BillDate = billDate;

            string billData = paymentSetting.QueryBill();
            int dataCount = 0;
            if (!string.IsNullOrEmpty(billData))
            {
                IBillService billService = null;
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
                    dataCount = billService.ImportData(_repository, billData);
            }
            return dataCount;
        }
        
    }
}
