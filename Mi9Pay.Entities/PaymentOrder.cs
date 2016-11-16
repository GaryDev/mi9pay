using Mi9Pay.PayProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class PaymentOrder
    {
        public int StoreId { get; set; }
        public string InvoiceNumber { get; set; }
        public string TradeNumber { get; set; }
        public string Subject { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingFee { get; set; }
        public List<PaymentOrderDetail> PayItems { get; set; }

        public string OrderType { get; set; }
        public GatewayType GatewayType { get; set; }
        public PaymentOrderStatus Status { get; set; }
        public Guid StorePaymentMethod { get; set; }

        public PaymentOrderMerchant Merchant { get; set; }
        public PaymentOrderCustomer Customer { get; set; }


        public PaymentOrder()
        {
            PayItems = new List<PaymentOrderDetail>();
        }
    }

    public enum PaymentOrderStatus
    {
        UNPAID,
        PAID,
        REFUND,
        CLOSED
    }
}
