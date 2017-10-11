using Mi9Pay.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Mi9Pay.Web.ActionFilters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string TOKEN_KEY = "Token";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            if (filterContext.Request.Headers.Contains(TOKEN_KEY))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(TOKEN_KEY).First();

                var provider = filterContext.ControllerContext.Configuration
                    .DependencyResolver.GetService(typeof(IGatewayMgrService)) as IGatewayMgrService;
                // Validate Token
                if (provider != null && !provider.ValidateToken(tokenValue))
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
                    filterContext.Response = responseMessage;
                }
            }
            else
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}