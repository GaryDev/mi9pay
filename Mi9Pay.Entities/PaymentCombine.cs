using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class PaymentCombine
    {
        public string CombineId { get; set; }
        public bool IsDefault { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public PaymentScanMode PaymentScanMode { get; set; }
    }
}
