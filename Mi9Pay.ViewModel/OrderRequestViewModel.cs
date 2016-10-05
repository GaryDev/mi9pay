using ICanPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class OrderRequestViewModel
    {
        public string InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public IEnumerable<PaymentMethodViewModel> PaymentMethodList { get; set; }
    }
}
