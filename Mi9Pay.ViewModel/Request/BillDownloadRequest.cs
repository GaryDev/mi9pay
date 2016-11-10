using ICanPay;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class BillDownloadRequest : IValidatableObject
    {
        public string store_id { get; set; }
        public string bill_date { get; set; }
        public string payment_method { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(store_id))
                throw new ArgumentException("门店id不能为空");

            DateTime parseDate;
            if (!DateTime.TryParseExact(bill_date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parseDate))
                throw new ArgumentException("账单日期格式不正确，请使用yyyy-mm-dd格式");

            GatewayType parseType;
            if (!Enum.TryParse(payment_method, true, out parseType))
                throw new ArgumentException("支付类型代码不正确（支付宝=alipay,微信=wechat）");

            yield return new ValidationResult("");
        }
    }
}
