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
                int retCount = _gatewayService.DownloadBill("3", "2016-11-07", GatewayType.Alipay);

                string message = retCount > 0 ? "OK" : "订单数据不存在";
                return Json(new BaseResponseViewModel { return_code = "SUCCESS", return_msg = message });
            }
            catch (Exception ex)
            {
                return Json(new BaseResponseViewModel { return_code = "FAIL", return_msg = ex.Message });
            }
        }
    }
}