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
    
    public partial class GatewayPaymentOrderStatus
    {
        public GatewayPaymentOrderStatus()
        {
            this.GatewayPaymentOrder = new HashSet<GatewayPaymentOrder>();
        }
    
        public System.Guid UniqueId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<GatewayPaymentOrder> GatewayPaymentOrder { get; set; }
    }
}
