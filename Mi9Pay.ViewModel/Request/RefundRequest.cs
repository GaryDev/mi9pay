using ICanPay;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class RefundRequest : CommonRequest
    {
        public RefundOrder order { get; set; }

        protected override void ValidateFields(ValidationContext validationContext)
        {
            base.ValidateFields(validationContext);

            if (order == null)
                throw new ArgumentException("退款单不能为空");

            if (string.IsNullOrWhiteSpace(order.invoice_no))
                throw new ArgumentException("订单号不能为空");

            if (string.IsNullOrWhiteSpace(order.trade_no))
                throw new ArgumentException("交易号不能为空");

            double refundAmount;
            if (string.IsNullOrWhiteSpace(order.refund_amount) || !double.TryParse(order.refund_amount, out refundAmount))
                throw new ArgumentException("退款金额不能为空");
        }
    }
}
