using Com.Alipay.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICanPay.Providers.Extended
{
    public class AlipayBillDownloadContentBuilder : JsonBuilder
    {
        public string bill_type { get; set; }
        public string bill_date { get; set; }

        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}
