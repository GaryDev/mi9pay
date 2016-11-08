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

namespace Mi9Pay.Service
{
    public class BillServiceAlipay : IBillService
    {
        public int ImportData(GatewayRepository repository, string dataAddress)
        {
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
                            string[] contents = File.ReadAllLines(csvFileName, Encoding.Default);
                            for (int i = 4; i < contents.Length - 4; i++)
                            {
                                string row = contents[i];
                                if (string.IsNullOrWhiteSpace(row)) continue;

                                string[] rowValues = row.Split(",".ToCharArray());
                            }
                            File.Delete(csvFileName);
                        }
                    }
                }
                File.Delete(fileName);
            }

            return 0;
        }

        private string RandomFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString();
        }
    }
}
