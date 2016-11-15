//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mi9Pay.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class GatewayPaymentOrder
    {
        public GatewayPaymentOrder()
        {
            this.GatewayPaymentOrderDetail = new HashSet<GatewayPaymentOrderDetail>();
        }
    
        public System.Guid UniqueId { get; set; }
        public int StoreID { get; set; }
        public string Subject { get; set; }
        public decimal TotalAmount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> ShippingFee { get; set; }
        public System.Guid GatewayPaymentOrderStatus { get; set; }
        public System.DateTime TSID { get; set; }
        public string OrderNumber { get; set; }
        public string TradeNumber { get; set; }
        public System.Guid OrderType { get; set; }
        public Nullable<System.Guid> GatewayPaymentCustomer { get; set; }
        public Nullable<System.Guid> GatewayPaymentStorePaymentMethod { get; set; }
        public System.Guid GatewayPaymentMerchant { get; set; }
    
        public virtual GatewayPaymentCustomer GatewayPaymentCustomer1 { get; set; }
        public virtual GatewayPaymentMerchant GatewayPaymentMerchant1 { get; set; }
        public virtual GatewayPaymentOrderStatus GatewayPaymentOrderStatus1 { get; set; }
        public virtual GatewayPaymentOrderType GatewayPaymentOrderType { get; set; }
        public virtual GatewayPaymentStorePaymentMethod GatewayPaymentStorePaymentMethod1 { get; set; }
        public virtual ICollection<GatewayPaymentOrderDetail> GatewayPaymentOrderDetail { get; set; }
    }
}
