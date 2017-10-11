using Mi9Pay.Entities.GatewayMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class PaymentOrderMerchant : Merchant
    {
        public string AppId { get; set; }
    }
}
