using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class OrderResponse
    {
        public string AppId { get; set; }
        public string InvoiceNumber { get; set; }
        public string NotificationId { get; set; }
        public string TradeNo { get; set; }
        public string Status { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string PaidAmount { get; set; }
        public string PaymentDate { get; set; }
        public string Signature { get; set; }
        public string PayCenterSign { get; set; }
        public string SignParams { get; set; }
    }
}
