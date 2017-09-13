using Mi9Pay.Service;
using Mi9Pay.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mi9Pay.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IGatewayService _gatewayService;

        public BaseController(IGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            //if (filterContext.HttpContext.Request.HttpMethod == "GET")
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.Result = new JsonResult
                {
                    Data = new ErrorResponse(ex.Message),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary(model)
                };
            }
        }
    }
}