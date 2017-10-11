using Mi9Pay.Web.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mi9Pay.Web.Controllers
{
    public partial class GatewayMgrController
    {
        [Route("merchant/all")]
        [HttpGet]
        public HttpResponseMessage GetAllMerchants()
        {
            return _gatewayMgrAction.GetAllMerchants();
        }
    }
}
