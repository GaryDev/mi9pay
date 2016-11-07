﻿using AutoMapper;
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

        [Route("version")]
        [HttpGet]
        public JsonResult GatewayInfo()
        {
            AssemblyHelper assemblyHelper = new AssemblyHelper(GetType().Assembly);
            return Json(new { version = assemblyHelper.FileVersion }, JsonRequestBehavior.AllowGet);
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

                IEnumerable<PaymentCombine> paymentCombineList = _gatewayService.GetPaymentCombineList(orderRequest.StoreId);
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<PaymentCombine, PaymentCombineViewModel>();
                    cfg.CreateMap<PaymentMethod, PaymentMethodViewModel>();
                    cfg.CreateMap<PaymentScanMode, PaymentScanModeViewModel>();
                });
                order.PaymentCombineList = Mapper.Map<IEnumerable<PaymentCombine>, IEnumerable<PaymentCombineViewModel>>(paymentCombineList);

                //IEnumerable<PaymentMethod> paymentMethods = _gatewayService.GetPaymentMethods(orderRequest.StoreId);
                //Mapper.Initialize(cfg => cfg.CreateMap<PaymentMethod, PaymentMethodViewModel>());
                //order.PaymentMethodList = Mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<PaymentMethodViewModel>>(paymentMethods);

                //IEnumerable<ScanMode> scanModeList = _gatewayService.GetScanModeList();
                //Mapper.Initialize(cfg => cfg.CreateMap<ScanMode, ScanModeViewModel>());
                //order.ScanModeList = Mapper.Map<IEnumerable<ScanMode>, IEnumerable<ScanModeViewModel>>(scanModeList);

                return View(order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("qrcode")]
        [HttpGet]
        public JsonResult PaymentQRCode(OrderRequest orderRequest, string method, string cid)
        {
            try
            {
                string imgTag = string.Empty;
                GatewayType type = method.ToEnum<GatewayType>();
                orderRequest.PayMethod = method;
                orderRequest.PaymentCombine = cid;

                MemoryStream ms = _gatewayService.CreatePaymentQRCode(orderRequest, type, cid);
                if (ms != null)
                {
                    imgTag = string.Format("<img src='data:image/png;base64,{0}' />", Convert.ToBase64String(ms.ToArray()));
                }
                return Json(new { return_code = "SUCCESS", img = imgTag, return_msg = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { return_code = "FAIL", img = string.Empty, return_msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("barcode")]
        [HttpPost]
        public JsonResult BarcodePayment(OrderRequest request, string method, string barcode, string cid)
        {
            try
            {
                GatewayType type = method.ToEnum<GatewayType>();
                request.PayMethod = method;
                request.PaymentCombine = cid;

                OrderPaymentResponse result = _gatewayService.BarcodePayment(request, type, barcode, cid);

                string returnUrl = string.Empty;
                if (result.IsSuccess())
                {
                    returnUrl = _gatewayService.BuildReturnUrl(request, result);
                }

                return Json(new { return_code = result.return_code, return_url = returnUrl, return_msg = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { return_code = "FAIL", img = string.Empty, return_msg = ex.Message });
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
                int storeId = 0;
                foreach (PaymentCombine payCombine in _gatewayService.GetPaymentCombineList(storeId))
                {
                    GatewayType type = payCombine.PaymentMethod.Code.ToEnum<GatewayType>();
                    result = _gatewayService.QueryPayment(app_id, invoice, type, payCombine.StorePaymentMethod);
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
        public JsonResult LongPolling(OrderRequest request)
        {
            try
            {
                GatewayType type = request.PayMethod.ToEnum<GatewayType>();
                OrderPaymentResponse result = _gatewayService.QueryPayment(request.AppId, request.InvoiceNumber, type, request.PaymentCombine);

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

            //if (filterContext.HttpContext.Request.HttpMethod == "GET")
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.Result = new JsonResult
                {
                    Data = new { return_code = "FAIL", return_msg = ex.Message },
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