using Mi9Pay.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Service
{
    public class BillServiceWechat : IBillService
    {
        public int ImportData(GatewayRepository repository, string data)
        {
            int count = 0;
            string[] dataRow = data.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < dataRow.Length - 2; i++)
            {
                string row = dataRow[i];
                string[] rowValues = row.Split(",".ToCharArray());
            }

            return count;
        }
    }
}
