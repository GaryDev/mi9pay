﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GatewayPayEntities : DbContext
    {
        public GatewayPayEntities()
            : base("name=GatewayPayEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<GatewayPaymentMethod> GatewayPaymentMethod { get; set; }
        public DbSet<GatewayPaymentOrderDetail> GatewayPaymentOrderDetail { get; set; }
        public DbSet<GatewayPaymentOrderStatus> GatewayPaymentOrderStatus { get; set; }
        public DbSet<GatewayPaymentOrderType> GatewayPaymentOrderType { get; set; }
        public DbSet<GatewayPaymentCustomer> GatewayPaymentCustomer { get; set; }
        public DbSet<GatewayPaymentMerchant> GatewayPaymentMerchant { get; set; }
        public DbSet<GatewayPaymentStore> GatewayPaymentStore { get; set; }
        public DbSet<GatewayPaymentMethodType> GatewayPaymentMethodType { get; set; }
        public DbSet<GatewayPaymentMethodTypeJoin> GatewayPaymentMethodTypeJoin { get; set; }
        public DbSet<GatewayPaymentPosition> GatewayPaymentPosition { get; set; }
        public DbSet<GatewayPaymentStorePaymentMethod> GatewayPaymentStorePaymentMethod { get; set; }
        public DbSet<GatewayPaymentUser> GatewayPaymentUser { get; set; }
        public DbSet<GatewayPaymentBillWechat> GatewayPaymentBillWechat { get; set; }
        public DbSet<GatewayPaymentBillAlipay> GatewayPaymentBillAlipay { get; set; }
        public DbSet<GatewayPaymentNotifyQueue> GatewayPaymentNotifyQueue { get; set; }
        public DbSet<GatewayPaymentAccount> GatewayPaymentAccount { get; set; }
        public DbSet<GatewayPaymentApp> GatewayPaymentApp { get; set; }
        public DbSet<GatewayPaymentOrder> GatewayPaymentOrder { get; set; }
    }
}
