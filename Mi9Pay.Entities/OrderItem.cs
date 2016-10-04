using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class OrderItem
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
