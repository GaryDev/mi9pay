using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class OrderRefundRequest
    {
        public string InvoiceNo { get; set; }
        public string TradeNo { get; set; }
        public string RefundAmount { get; set; }
        public string RefundReason { get; set; }
    }
}
