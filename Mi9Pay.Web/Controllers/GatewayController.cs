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
using VmOrderPaymentResponse = Mi9Pay.ViewModel.OrderPaymentResponse;
using VmOrderPayment = Mi9Pay.ViewModel.OrderPayment;

namespace Mi9Pay.Web.Controllers
{
    [RoutePrefix("gateway")]
    public class GatewayController : BaseController
    {
        public GatewayController(IGatewayService gatewayService) : base(gatewayService)
        {
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

                Entities.OrderPaymentResponse result = _gatewayService.BarcodePayment(request, type, barcode, cid);

                string returnUrl = string.Empty;
                if (result.IsSuccess())
                {
                    returnUrl = _gatewayService.BuildReturnUrl(request, result);
                    _gatewayService.PaymentNotify(request, result);
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

                Entities.OrderPaymentResponse result = null;
                VmOrderPaymentResponse vmOrderPayment = new VmOrderPaymentResponse();
                int storeId = 0;
                foreach (PaymentCombine payCombine in _gatewayService.GetPaymentCombineList(storeId))
                {
                    GatewayType type = payCombine.PaymentMethod.Code.ToEnum<GatewayType>();
                    result = _gatewayService.QueryPayment(app_id, invoice, type, payCombine.StorePaymentMethod);
                    if (result != null && result.IsSuccess())
                    {
                        Mapper.Initialize(cfg => {
                            cfg.CreateMap<Entities.OrderPaymentResponse, VmOrderPaymentResponse>();
                            cfg.CreateMap<Entities.OrderPayment, VmOrderPayment>();
                        });
                        vmOrderPayment = Mapper.Map<Entities.OrderPaymentResponse, VmOrderPaymentResponse>(result);
                        vmOrderPayment.return_msg = "OK";
                        break;
                    }
                }
                
                return Json(vmOrderPayment, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new BaseResponse() { return_code = "FAIL", return_msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [Route("order/polling")]
        [HttpGet]
        public JsonResult LongPolling(OrderRequest request)
        {
            try
            {
                GatewayType type = request.PayMethod.ToEnum<GatewayType>();
                Entities.OrderPaymentResponse result = _gatewayService.QueryPayment(request.AppId, request.InvoiceNumber, type, request.PaymentCombine);

                string returnUrl = string.Empty;
                if (result.IsSuccess())
                {
                    returnUrl = _gatewayService.BuildReturnUrl(request, result);
                    _gatewayService.PaymentNotify(request, result);
                }
                return Json(new { return_code = result.return_code, return_url = returnUrl, return_msg = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { return_code = "FAIL", return_url = string.Empty, return_msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        
    }
}