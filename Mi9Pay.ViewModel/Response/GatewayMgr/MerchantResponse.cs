using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel.Response.GatewayMgr
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MerchantResponse
    {
        [JsonProperty]
        public string code { get; set; }

        [JsonProperty]
        public string name { get; set; }
    }
}
