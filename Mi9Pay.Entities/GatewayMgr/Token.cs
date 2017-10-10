using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mi9Pay.Entities.GatewayMgr
{
    public class Token
    {
        public Guid UniqueId { get; set; }
        public string AuthToken { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public User User { get; set; }
    }
}