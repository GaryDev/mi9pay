using Mi9Pay.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Mi9Pay.Service
{
    public class BillServiceWechat : BillService
    {
        public override bool ClearData()
        {
            bool success = false;
            try
            {
                var billData = Repository.BillWechat.GetMany(b => 
                    b.StoreId == StoreId && b.GatewayPaymentMerchant == Merchant && b.TradeTime.StartsWith(BillDate));
                if (billData.Count() > 0)
                {
                    using (var scope = new TransactionScope())
                    {
                        Repository.BillWechat.Delete(b => 
                            b.StoreId == StoreId && b.GatewayPaymentMerchant == Merchant && b.TradeTime.StartsWith(BillDate));
                        Repository.Save();
                        scope.Complete();
                    }
                }
                success = true;
            }
            catch (Exception)
            {
                
            }
            return success;
        }

        public override int ImportData(string data)
        {
            int count = 0;
            DateTime createTime = DateTime.Now;
            try
            {
                using (var scope = new TransactionScope())
                {
                    string[] dataRow = data.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 1; i < dataRow.Length - 2; i++)
                    {
                        string row = dataRow[i];
                        string[] rowValues = row.Split(",".ToCharArray());
                        rowValues = rowValues.Select(r => string.IsNullOrWhiteSpace(r) ? string.Empty : r.Replace("`", "")).ToArray();

                        GatewayPaymentBillWechat bill = new GatewayPaymentBillWechat
                        {
                            UniqueId = Guid.NewGuid()
                            ,StoreId = StoreId
                            ,GatewayPaymentMerchant = Merchant
                            ,TradeTime = rowValues[0]
                            ,GHId = rowValues[1]
                            ,MchId = rowValues[2]
                            ,SubMch = rowValues[3]
                            ,DeviceId = rowValues[4]
                            ,TradeNo = rowValues[5]
                            ,OrderNumber = rowValues[6]
                            ,OpenId = rowValues[7]
                            ,TradeType = rowValues[8]
                            ,TradeStatus = rowValues[9]
                            ,Bank = rowValues[10]
                            ,Currency = rowValues[11]
                            ,TotalAmount = rowValues[12]
                            ,PromoAmount = rowValues[13]
                            ,RefundTradeNo = rowValues[14]
                            ,RefundOrderNo = rowValues[15]
                            ,RefundAmount = rowValues[16]
                            ,RefundPromoAmount = rowValues[17]
                            ,RefundType = rowValues[18]
                            ,RefundStatus = rowValues[19]
                            ,ProductName = rowValues[20]
                            ,DataPackage = rowValues[21]
                            ,Fee = rowValues[22]
                            ,Rate = rowValues[23]
                            ,TSID = createTime
                        };
                        Repository.BillWechat.Insert(bill);
                        count++;
                    }
                    if (count > 0)
                    {
                        Repository.Save();
                        scope.Complete();
                    }
                }
            }
            catch (Exception)
            {
                count = -1;
            }
            return count;
        }
    }
}
