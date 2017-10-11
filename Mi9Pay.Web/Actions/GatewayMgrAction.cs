using Mi9Pay.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Mi9Pay.Web.Actions
{
    public partial class GatewayMgrAction
    {
        private readonly HttpRequestMessage _request;
        private readonly IGatewayMgrService _gatewayMgrService;

        public GatewayMgrAction(IGatewayMgrService gatewayMgrService)
        {
            _gatewayMgrService = gatewayMgrService;
            _request = (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
        }
    }
}