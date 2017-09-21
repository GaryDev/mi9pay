/**
GatewayPaymentApp CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentApp_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]'))
ALTER TABLE [dbo].[GatewayPaymentApp] DROP CONSTRAINT [FK_GatewayPaymentApp_GatewayPaymentMerchant]
GO

/**
GatewayPaymentAccount CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentApp]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount] DROP CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentApp]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount] DROP CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMerchant]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount] DROP CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMethod]
GO

/**
GatewayPaymentCustomer CONSTRAINT
**/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GatewayPaymentCustomer_TSID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GatewayPaymentCustomer] DROP CONSTRAINT [DF_GatewayPaymentCustomer_TSID]
END
GO

/**
GatewayPaymentOrder CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentCustomer]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentCustomer]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMerchant]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMethod]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderType]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderType]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GatewayPaymentOrder_TSID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [DF_GatewayPaymentOrder_TSID]
END
GO

/**
GatewayPaymentOrderDetail CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrderDetail_GatewayPaymentOrder]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderDetail]'))
ALTER TABLE [dbo].[GatewayPaymentOrderDetail] DROP CONSTRAINT [FK_GatewayPaymentOrderDetail_GatewayPaymentOrder]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GatewayPaymentOrderDetail_TSID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GatewayPaymentOrderDetail] DROP CONSTRAINT [DF_GatewayPaymentOrderDetail_TSID]
END
GO

/**
GatewayPaymentStore CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStore_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]'))
ALTER TABLE [dbo].[GatewayPaymentStore] DROP CONSTRAINT [FK_GatewayPaymentStore_GatewayPaymentMerchant]
GO


/**
GatewayPaymentStorePaymentMethod CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] DROP CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethod]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] DROP CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] DROP CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]
GO

/**
GatewayPaymentMethodTypeJoin CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]'))
ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin] DROP CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethod]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethodType]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]'))
ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin] DROP CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethodType]
GO

/**
GatewayPaymentUser CONSTRAINT
**/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentPosition]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] DROP CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentPosition]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] DROP CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentStore]
GO