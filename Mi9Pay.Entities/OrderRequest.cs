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
        public string InvoiceNumber { get; set; }
        public string Signature { get; set; }

        public string DoneUrl { get; set; }
        public string NotifyUrl { get; set; }

        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }

        public string PayMethod { get; set; }
        public List<OrderItem> PayItems { get; set; }

        public decimal ShippingFee { get; set; }
        public string ShippingFirstName { get; set; }
        public string ShippingLastName { get; set; }
        public string ShippingMobile { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingProvice { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingDistrict { get; set; }
        public string ShippingStreet { get; set; }

        public OrderRequest()
        {
            PayItems = new List<OrderItem>();
        }
    }
}
