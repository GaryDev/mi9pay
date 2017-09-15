using Mi9Pay.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mi9Pay.Service;
using Mi9Pay.Web.Helpers;
using Mi9Pay.Entities;
using Mi9Pay.PayProvider;

namespace Mi9Pay.Web.Controllers
{
    [RoutePrefix("payment")]
    public class PaymentController : BaseController
    {
        public PaymentController(IGatewayService gatewayService) 
            : base(gatewayService)
        {
        }

        [Route("billdownload")]
        [HttpPost]
        public JsonResult BillDownload(BillDownloadRequest request)
        {
            try
            {
                string merchantCode = request.merchant_id;
                string storeId = request.store_id;
                string billDate = request.bill_date;
                GatewayType type = request.payment_method.ToEnum<GatewayType>();

                string[] storeIdArray = storeId.Split(",".ToCharArray());

                int retCount = _gatewayService.DownloadBill(merchantCode, storeIdArray, billDate, type);
                return Json(new SuccessResponse().AddData("process_count", retCount.ToString()));
            }
            catch (Exception ex)
            {
                return Json(new ErrorResponse(ex.Message));
            }
        }

        [Route("refund")]
        [HttpPost]
        public JsonResult Refund(RefundRequest request)
        {
            try
            {
                OrderRefundRequest refundRequest = new OrderRefundRequest
                {
                    InvoiceNo = request.order.invoice_no,
                    TradeNo = request.order.trade_no,
                    RefundAmount = request.order.refund_amount,
                    RefundReason = request.order.refund_reason
                };
                GatewayType type = request.payment_method.ToEnum<GatewayType>();

                _gatewayService.RefundPayment(request.merchant_id, request.store_id, refundRequest, type);
                return Json(new SuccessResponse());
            }
            catch (Exception ex)
            {
                return Json(new ErrorResponse(ex.Message));
            }
        }
    }
}