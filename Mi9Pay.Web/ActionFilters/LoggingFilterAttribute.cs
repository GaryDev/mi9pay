﻿using Mi9Pay.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Http.Tracing;
using System.Net.Http;
using Mi9Pay.Config;

namespace Mi9Pay.Web.ActionFilters
{
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;
            string controllerName = actionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            string actionName = actionDescriptor.ActionName;

            if ((controllerName == "gateway" && actionName == "Order") && !TimeHelper.IsValid())
                throw new Exception("感谢使用Mi9Pay支付网关，功能试用已到期");

            if (AppConfig.IsLogEnabled && actionName != "LongPolling")
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