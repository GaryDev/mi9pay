using Mi9Pay.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mi9Pay.Service;
using ICanPay;

namespace Mi9Pay.Web.Controllers
{
    [RoutePrefix("payment")]
    public class PaymentController : BaseController
    {
        public PaymentController(IGatewayService gatewayService) : base(gatewayService)
        {
        }

        [Route("bill/download")]
        [HttpPost]
        public JsonResult BillDownload()
        {
            try
            {
                string storeId = "3";
                string[] storeIdArray = storeId.Split(",".ToCharArray());
                string billDate = "2016-10-10";

                int retCount = _gatewayService.DownloadBill(storeIdArray, billDate, GatewayType.WeChat);
                return Json(new BillDownloadResponse { return_code = "SUCCESS", return_msg = "OK", process_count = retCount });
            }
            catch (Exception ex)
            {
                return Json(new BaseResponse { return_code = "FAIL", return_msg = ex.Message });
            }
        }
    }
}