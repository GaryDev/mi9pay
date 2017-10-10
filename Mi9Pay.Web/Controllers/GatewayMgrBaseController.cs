using Mi9Pay.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Mi9Pay.Web.Controllers
{
    public class GatewayMgrBaseController : ApiController
    {
        protected readonly IGatewayMgrService _gatewayMgrService;

        public GatewayMgrBaseController(IGatewayMgrService gatewayMgrService)
        {
            _gatewayMgrService = gatewayMgrService;
        }
    }
}