using Mi9Pay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mi9Pay.Web.Binder
{
    public class OrderRequestBinder : IModelBinder
    {
        private const string sessionKey = OrderRequest.SessionKey;

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            OrderRequest request = (OrderRequest)controllerContext.HttpContext.Session[sessionKey];
            if (request == null)
            {
                request = new OrderRequest();
                controllerContext.HttpContext.Session[sessionKey] = request;
            }
            return request;
        }
    }
}