using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class OrderPaymentResponseViewModel
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }
        public OrderPaymentViewModel order { get; set; }
    }
}
