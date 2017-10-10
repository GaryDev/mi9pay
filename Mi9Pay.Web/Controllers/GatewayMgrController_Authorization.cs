using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Mi9Pay.Service;
using Mi9Pay.Web.ActionFilters;

namespace Mi9Pay.Web.Controllers
{
    public partial class GatewayMgrController
    {
        [Route("auth/login")]
        [HttpPost]
        public HttpResponseMessage GetToken()
        {
            if (System.Threading.Thread.CurrentPrincipal != null
                    && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var userId = basicAuthenticationIdentity.UserId;
                    return GetAuthToken(userId);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        private HttpResponseMessage GetAuthToken(int userId)
        {
            var token = _gatewayMgrService.GenerateToken(userId);
            if (token == null)
                return Request.CreateResponse(HttpStatusCode.InternalServerError);

            var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
            response.Headers.Add("Token", token.AuthToken);
            response.Headers.Add("Access-Control-Expose-Headers", "Token");
            return response;
        }
    }
}