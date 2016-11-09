using Mi9Pay.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Service
{
    public abstract class BillService
    {
        public GatewayRepository Repository { get; set; }
        public string StoreId { get; set; }
        public string BillDate { get; set; }

        public abstract bool ClearData();
        public abstract int ImportData(string data);

        public int DownloadBill(string data)
        {
            int count = 0;

            if (ClearData())
                count = ImportData(data);

            return count;
        }
    }
}
