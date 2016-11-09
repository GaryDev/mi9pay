using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class OrderPayment
    {
        public string uuid { get; set; }
        public string invoice { get; set; }
        public string status { get; set; }
        public string amount { get; set; }
        public string paid_amount { get; set; }
        public string currency { get; set; }
    }
}
