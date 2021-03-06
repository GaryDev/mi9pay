﻿using Mi9Pay.Service;
using Mi9Pay.Web.Actions;
using Mi9Pay.Web.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Mi9Pay.Web.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("gatewaymgr/api/v1")]
    public partial class GatewayMgrController : GatewayMgrBaseController
    {
        private readonly GatewayMgrAction _gatewayMgrAction;

        public GatewayMgrController(IGatewayMgrService gatewayMgrService)
            : base(gatewayMgrService)
        {
            _gatewayMgrAction = new GatewayMgrAction(gatewayMgrService);
        }
    }
}