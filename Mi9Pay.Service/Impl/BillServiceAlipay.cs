using Ionic.Zip;
using Mi9Pay.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Mi9Pay.Service
{
    public class BillServiceAlipay : BillService
    {
        public override bool ClearData()
        {
            bool success = false;
            try
            {
                var billData = Repository.BillAlipay.GetMany(b => b.StoreId == StoreId && b.StartTime.StartsWith(BillDate));
                if (billData.Count() > 0)
                {
                    using (var scope = new TransactionScope())
                    {
                        Repository.BillAlipay.Delete(b => b.StoreId == StoreId && b.StartTime.StartsWith(BillDate));
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

        public override int ImportData(string dataAddress)
        {
            int count = 0;

            string appDir = null;
            if (string.IsNullOrEmpty(AppDomain.CurrentDomain.BaseDirectory))
                appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            else
                appDir = AppDomain.CurrentDomain.BaseDirectory;

            string tempDir = appDir + "\\Temp\\";
            string fileName = tempDir + RandomFileName() + ".zip";

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(dataAddress, fileName);
            }

            if (File.Exists(fileName))
            {
                using (ZipFile zip = new ZipFile(fileName, Encoding.Default))
                {
                    var csvFile = zip.Entries.SingleOrDefault(e => e.FileName.EndsWith("业务明细.csv"));
                    if (csvFile != null)
                    {
                        csvFile.Extract(tempDir, ExtractExistingFileAction.OverwriteSilently);
                        string csvFileName = tempDir + csvFile.FileName;
                        if (File.Exists(csvFileName))
                        {
                            DateTime createTime = DateTime.Now;
                            try
                            {
                                using (var scope = new TransactionScope())
                                {
                                    string[] contents = File.ReadAllLines(csvFileName, Encoding.Default);
                                    for (int i = 5; i < contents.Length - 4; i++)
                                    {
                                        string row = contents[i];
                                        if (string.IsNullOrWhiteSpace(row)) continue;

                                        string[] rowValues = row.Split(",".ToCharArray());
                                        rowValues = rowValues.Select(r => string.IsNullOrWhiteSpace(r) ? string.Empty : r).ToArray();

                                        GatewayPaymentBillAlipay bill = new GatewayPaymentBillAlipay
                                        {
                                            UniqueId = Guid.NewGuid()
                                            ,StoreId = StoreId
                                            ,TradeNo = rowValues[0]
                                            ,OrderNumber = rowValues[1]
                                            ,TradeType = rowValues[2]
                                            ,ProductName = rowValues[3]
                                            ,StartTime = rowValues[4]
                                            ,EndTime = rowValues[5]
                                            ,StoreNumber = rowValues[6]
                                            ,StoreName = rowValues[7]
                                            ,Operator = rowValues[8]
                                            ,Terminal = rowValues[9]
                                            ,MchAccount = rowValues[10]
                                            ,TotalAmount = rowValues[11]
                                            ,ReceiveAmount = rowValues[12]
                                            ,AlipayRPAmount = rowValues[13]
                                            ,AlipayJFAmount = rowValues[14]
                                            ,AlipayPromoAmount = rowValues[15]
                                            ,MchPromoAmount = rowValues[16]
                                            ,CouponAmount = rowValues[17]
                                            ,CouponName = rowValues[18]
                                            ,MchRPAmount = rowValues[19]
                                            ,CardAmount = rowValues[20]
                                            ,RefundNumber = rowValues[21]
                                            ,Fee = rowValues[22]
                                            ,Profit = rowValues[23]
                                            ,Note = rowValues[24]
                                            ,TSID = createTime
                                        };

                                        Repository.BillAlipay.Insert(bill);
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
                            File.Delete(csvFileName);
                        }
                    }
                }
                File.Delete(fileName);
            }

            return count;
        }

        private string RandomFileName()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
