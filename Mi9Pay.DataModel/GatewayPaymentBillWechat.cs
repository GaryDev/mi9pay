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
    
    public partial class GatewayPaymentBillWechat
    {
        public System.Guid UniqueId { get; set; }
        public string StoreId { get; set; }
        public System.Guid GatewayPaymentMerchant { get; set; }
        public string TradeTime { get; set; }
        public string GHId { get; set; }
        public string MchId { get; set; }
        public string SubMch { get; set; }
        public string DeviceId { get; set; }
        public string TradeNo { get; set; }
        public string OrderNumber { get; set; }
        public string OpenId { get; set; }
        public string TradeType { get; set; }
        public string TradeStatus { get; set; }
        public string Bank { get; set; }
        public string Currency { get; set; }
        public string TotalAmount { get; set; }
        public string PromoAmount { get; set; }
        public string RefundTradeNo { get; set; }
        public string RefundOrderNo { get; set; }
        public string RefundAmount { get; set; }
        public string RefundPromoAmount { get; set; }
        public string RefundType { get; set; }
        public string RefundStatus { get; set; }
        public string ProductName { get; set; }
        public string DataPackage { get; set; }
        public string Fee { get; set; }
        public string Rate { get; set; }
        public System.DateTime TSID { get; set; }
    }
}
