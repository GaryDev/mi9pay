using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.DataModel
{
    public class GatewayPaymentMethodTypeJoinResult
    {
        public GatewayPaymentMethodTypeJoin PaymentCombine { get; set; }
        public Guid StorePaymentMethod { get; set; }
        public bool IsDefault { get; set; }
    }
}
