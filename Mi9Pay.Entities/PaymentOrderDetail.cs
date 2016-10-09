using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class PaymentOrderDetail
    {
        public string ItemName { get; set; }
        public decimal ItemQty { get; set; }
        public decimal ItemAmount { get; set; }
    }
}
