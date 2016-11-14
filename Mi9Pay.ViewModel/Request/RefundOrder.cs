using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class RefundOrder
    {
        public string invoice_no { get; set; }
        public string trade_no { get; set; }
        public string refund_amount { get; set; }
        public string refund_reason { get; set; }
    }
}
