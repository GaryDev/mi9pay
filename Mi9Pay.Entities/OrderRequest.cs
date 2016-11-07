using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class OrderRequest
    {
        public const string SessionKey = "orderRequest";

        public string AppId { get; set; }
        public string Signature { get; set; }
        public string DoneUrl { get; set; }
        public string NotifyUrl { get; set; }

        public int StoreId { get; set; }
        public string InvoiceNumber { get; set; }
        public string Currency { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingFee { get; set; }

        public string PayMethod { get; set; }
        public string PaymentCombine { get; set; }
        public List<PaymentOrderDetail> PayItems { get; set; }

        public PaymentOrderCustomer Customer { get; set; }

        public OrderRequest()
        {
            PayItems = new List<PaymentOrderDetail>();
        }
    }
}
