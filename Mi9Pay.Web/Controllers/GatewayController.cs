using AutoMapper;
using ICanPay;
using Mi9Pay.Entities;
using Mi9Pay.Service;
using Mi9Pay.ViewModel;
using Mi9Pay.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace Mi9Pay.Web.Controllers
{
    [RoutePrefix("gateway")]
    public class GatewayController : Controller
    {
        private readonly IGatewayService _gatewayService;

        public GatewayController(IGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        [Route("order")]
        [HttpPost]
        public ActionResult Order(FormCollection form)
        {
            try
            {
                Dictionary<string, string> parameters = form.CovertToDictionary();
                _gatewayService.ValidateRequestParameter(parameters);

                OrderRequest orderRequest = _gatewayService.RecieveRequestForm(parameters);
                ControllerContext.HttpContext.Session[OrderRequest.SessionKey] = orderRequest;

                Mapper.Initialize(cfg => cfg.CreateMap<OrderRequest, OrderRequestViewModel>());
                OrderRequestViewModel order = Mapper.Map<OrderRequest, OrderRequestViewModel>(orderRequest);

                IEnumerable<PaymentMethod> paymentMethods = _gatewayService.GetPaymentMethods();
                Mapper.Initialize(cfg => cfg.CreateMap<PaymentMethod, PaymentMethodViewModel>());
                order.PaymentMethodList = Mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<PaymentMethodViewModel>>(paymentMethods);

                return View(order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("qrcode")]
        [HttpGet]
        public JsonResult PaymentQRCode(OrderRequest orderRequest, string method)
        {
            try
            {
                string imgTag = string.Empty;

                GatewayType type = method.ToEnum<GatewayType>();
                orderRequest.PayMethod = method;
                MemoryStream ms = _gatewayService.CreatePaymentQRCode(orderRequest, type);
                if (ms != null)
                {
                    imgTag = string.Format("<img src='data:image/png;base64,{0}' />", Convert.ToBase64String(ms.ToArray()));
                }
                return Json(new { img = imgTag }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { img = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("order/query")]
        [HttpGet]
        public JsonResult QueryPaymentStatus(string app_id, string invoice, string sign)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("app_id", app_id);
                parameters.Add("invoice", invoice);
                parameters.Add("sign", sign);
                _gatewayService.ValidateRequestParameter(parameters);

                OrderPaymentResponse result = null;
                OrderPaymentResponseViewModel vmOrderPayment = new OrderPaymentResponseViewModel();
                foreach (GatewayType type in _gatewayService.GetGatewayTypes())
                {
                    result = _gatewayService.QueryPayment(app_id, invoice, type);
                    if (result != null && result.IsSuccess())
                    {
                        Mapper.Initialize(cfg => {
                            cfg.CreateMap<OrderPaymentResponse, OrderPaymentResponseViewModel>();
                            cfg.CreateMap<OrderPayment, OrderPaymentViewModel>();
                        });
                        vmOrderPayment = Mapper.Map<OrderPaymentResponse, OrderPaymentResponseViewModel>(result);
                        vmOrderPayment.return_msg = "OK";
                        break;
                    }
                }
                
                return Json(vmOrderPayment, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new OrderPaymentResponseViewModel() { return_code = "FAIL", return_msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [Route("order/polling")]
        [HttpGet]
        public JsonResult LongPolling(OrderRequest request, string invoice)
        {
            try
            {
                GatewayType type = request.PayMethod.ToEnum<GatewayType>();
                OrderPaymentResponse result = _gatewayService.QueryPayment(request.AppId, invoice, type);

                string returnUrl = string.Empty;
                if (result.IsSuccess())
                {
                    returnUrl = _gatewayService.BuildReturnUrl(request, result);
                }
                return Json(new { return_code = result.return_code, return_url = returnUrl, return_msg = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { return_code = "FAIL", return_url = string.Empty, return_msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
        }
    }
}