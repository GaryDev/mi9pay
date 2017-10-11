using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities.GatewayMgr
{
    public class Merchant
    {
        public Guid UniqueId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
