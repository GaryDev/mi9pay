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
    
    public partial class GatewayPaymentMerchant
    {
        public GatewayPaymentMerchant()
        {
            this.GatewayPaymentStore = new HashSet<GatewayPaymentStore>();
            this.GatewayPaymentAccount = new HashSet<GatewayPaymentAccount>();
            this.GatewayPaymentApp = new HashSet<GatewayPaymentApp>();
            this.GatewayPaymentOrder = new HashSet<GatewayPaymentOrder>();
        }
    
        public System.Guid UniqueId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<GatewayPaymentStore> GatewayPaymentStore { get; set; }
        public virtual ICollection<GatewayPaymentAccount> GatewayPaymentAccount { get; set; }
        public virtual ICollection<GatewayPaymentApp> GatewayPaymentApp { get; set; }
        public virtual ICollection<GatewayPaymentOrder> GatewayPaymentOrder { get; set; }
    }
}
