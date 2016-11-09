using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class OrderPaymentResponse : BaseResponse
    {
        public OrderPayment order { get; set; }
    }
}
