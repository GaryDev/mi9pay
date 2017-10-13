using Mi9Pay.Config;
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
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            if (!filterContext.Request.Headers.Contains(AppConstants.KEY_TOKEN))
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.NotAcceptable) { ReasonPhrase = "Token cannot be empty" };
            }
            else if (!filterContext.Request.Headers.Contains(AppConstants.KEY_CLIENT_TIME))
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.NotAcceptable) { ReasonPhrase = "Client time cannot be empty" };
            }
            else
            {
                var tokenValue = filterContext.Request.Headers.GetValues(AppConstants.KEY_TOKEN).First();
                var clientTime = filterContext.Request.Headers.GetValues(AppConstants.KEY_CLIENT_TIME).First();

                var provider = filterContext.ControllerContext.Configuration
                    .DependencyResolver.GetService(typeof(IGatewayMgrService)) as IGatewayMgrService;
                if (provider != null)
                {
                    if (!provider.ValidateToken(tokenValue))
                    {
                        filterContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = string.Format("Token {0} is invalid", tokenValue) };
                    }
                }                
            }

            base.OnActionExecuting(filterContext);
        }
    }
}