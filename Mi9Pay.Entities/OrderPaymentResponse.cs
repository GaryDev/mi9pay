using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class OrderPaymentResponse
    {
        public string return_code { get; set; }
        public OrderPayment order { get; set; }

        public bool IsSuccess()
        {
            return return_code == "SUCCESS";
        }
    }
}
