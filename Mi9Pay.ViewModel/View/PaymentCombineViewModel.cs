using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class PaymentCombineViewModel
    {
        public string CombineId { get; set; }
        public string Code
        {
            get
            {
                return string.Format("{0}:{1}", PaymentMethod.Code.ToLower(), PaymentScanMode.Code.ToLower());
            }
        }
        public string Name
        {
            get
            {
                return string.Format("{0}{1}", PaymentMethod.Name, PaymentScanMode.Name);
            }
        }

        public string StorePaymentMethod { get; set; }
        public bool IsDefault { get; set; }


        public PaymentMethodViewModel PaymentMethod { get; set; }
        public PaymentScanModeViewModel PaymentScanMode { get; set; }

    }
}
