using Mi9Pay.PayProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class BillDownloadRequest : CommonRequest
    {
        public string bill_date { get; set; }

        protected override void ValidateFields(ValidationContext validationContext)
        {
            base.ValidateFields(validationContext);

            DateTime parseDate;
            if (!DateTime.TryParseExact(bill_date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out parseDate))
                throw new ArgumentException("账单日期格式不正确，请使用yyyy-mm-dd格式");
        }
    }
}
