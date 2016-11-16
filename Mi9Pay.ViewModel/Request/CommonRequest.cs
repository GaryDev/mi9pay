using ICanPay;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class CommonRequest : IValidatableObject
    {
        public string merchant_id { get; set; }
        public string store_id { get; set; }
        public string payment_method { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(merchant_id))
                throw new ArgumentException("商家id不能为空");

            if (string.IsNullOrWhiteSpace(store_id))
                throw new ArgumentException("门店id不能为空");

            GatewayType parseType;
            if (!Enum.TryParse(payment_method, true, out parseType))
                throw new ArgumentException("支付类型代码不正确（支付宝=alipay,微信=wechat）");

            ValidateFields(validationContext);

            yield return new ValidationResult("");
        }

        protected virtual void ValidateFields(ValidationContext validationContext)
        {
        }
    }
}
