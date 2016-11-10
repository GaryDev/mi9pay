using Mi9Pay.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mi9Pay.Service;
using ICanPay;
using Mi9Pay.Web.Helpers;

namespace Mi9Pay.Web.Controllers
{
    [RoutePrefix("payment")]
    public class PaymentController : BaseController
    {
        public PaymentController(IGatewayService gatewayService) : base(gatewayService)
        {
        }

        [Route("billdownload")]
        [HttpPost]
        public JsonResult BillDownload(BillDownloadRequest request)
        {
            try
            {
                string storeId = request.store_id;
                string billDate = request.bill_date;
                GatewayType type = request.payment_method.ToEnum<GatewayType>();

                string[] storeIdArray = storeId.Split(",".ToCharArray());

                int retCount = _gatewayService.DownloadBill(storeIdArray, billDate, type);
                return Json(new BillDownloadResponse { return_code = "SUCCESS", return_msg = "OK", process_count = retCount });
            }
            catch (Exception ex)
            {
                return Json(new BaseResponse { return_code = "FAIL", return_msg = ex.Message });
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            filterContext.HttpContext.Response.StatusCode = 200;
            filterContext.Result = new JsonResult
            {
                Data = new { return_code = "FAIL", return_msg = ex.Message },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}