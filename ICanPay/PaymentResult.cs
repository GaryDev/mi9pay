using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICanPay
{
    public class PaymentResult
    {
        public string TradeNo { get; set; }
        public string InvoiceNo { get; set; }
        public string Amount { get; set; }
        public string PaidAmount { get; set; }
        public string PaymentDate { get; set; }
        public string Currency { get; set; }

        public int SuccessFlag { get; set; }
    }
}
