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
    
    public partial class GatewayPaymentNotifyQueue
    {
        public System.Guid UniqueId { get; set; }
        public string OrderNumber { get; set; }
        public string NotifyUrl { get; set; }
        public string PostData { get; set; }
        public string PostDataFormat { get; set; }
        public System.DateTime SendDate { get; set; }
        public System.DateTime LastSendDate { get; set; }
        public int NextInterval { get; set; }
        public int ProcessedCount { get; set; }
        public string Processed { get; set; }
    }
}