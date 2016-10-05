using Mi9Pay.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Http.Tracing;
using System.Net.Http;

namespace Mi9Pay.Web.ActionFilters
{
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!TimeHelper.IsValid())
                throw new Exception("感谢使用Mi9Pay支付网关，您的使用权已到期");

            var actionDescriptor = filterContext.ActionDescriptor;
            string controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = actionDescriptor.ActionName;

            if (actionName != "LongPolling")
            {
                HttpMethod httpMethod = new HttpMethod(HttpContext.Current.Request.HttpMethod);
                Uri requestUrl = HttpContext.Current.Request.Url;
                HttpRequestMessage request = new HttpRequestMessage(httpMethod, requestUrl);

                HttpConfiguration config = new HttpConfiguration();
                config.Services.Replace(typeof(ITraceWriter), new NLogHelper());
                var trace = config.Services.GetTraceWriter();
                trace.Info(request,
                    "Controller : " + controllerName + Environment.NewLine +
                    "Action : " + actionName, "JSON", filterContext.ActionParameters.Values.ToArray());
            }
        }
    }
}