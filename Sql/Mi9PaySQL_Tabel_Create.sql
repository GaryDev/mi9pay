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

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMethod]
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

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStore_StoreId]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]'))
ALTER TABLE [dbo].[GatewayPaymentStore] DROP CONSTRAINT [FK_GatewayPaymentStore_StoreId]
GO

/**
GatewayPaymentStorePaymentMethod CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] DROP CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethod]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] DROP CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]
GO


/**
GatewayPaymentUser CONSTRAINT
**/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_StoreId]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] DROP CONSTRAINT [FK_GatewayPaymentUser_StoreId]
GO
/**
GatewayPaymentMethod
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethod]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentMethod]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentMethod](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](20) NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentMethod] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethod]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentMethod] WHERE [Code] = 'wechatpayment')
	BEGIN
		insert into GatewayPaymentMethod
		values (NEWID(), 'wechatpayment', N'微信', N'微信支付')
	END
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentMethod] WHERE [Code] = 'alipay')
	BEGIN
		insert into GatewayPaymentMethod
		values (NEWID(), 'alipay', N'支付宝', N'支付宝支付')
	END
END
*/

/**
GatewayPaymentOrderType
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderType]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrderType]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentOrderType](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentOrderType] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderType]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentOrderType] WHERE [Code] = 'MOSAIC')
	BEGIN
		insert into GatewayPaymentOrderType
		values (NEWID(), N'MOSAIC', N'Mosaic')
	END
END
*/


/**
GatewayPaymentOrderStatus
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderStatus]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrderStatus]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentOrderStatus](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentOrderStatus] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderStatus]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentOrderStatus] WHERE [Code] = 'UNPAID')
	BEGIN
		insert into GatewayPaymentOrderStatus
		values (NEWID(), N'UNPAID', N'未支付')
	END
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentOrderStatus] WHERE [Code] = 'PAID')
	BEGIN
		insert into GatewayPaymentOrderStatus
		values (NEWID(), N'PAID', N'已支付')
	END
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentOrderStatus] WHERE [Code] = 'REFUND')
	BEGIN
		insert into GatewayPaymentOrderStatus
		values (NEWID(), N'REFUND', N'已退款')
	END
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentOrderStatus] WHERE [Code] = 'CLOSED')
	BEGIN
		insert into GatewayPaymentOrderStatus
		values (NEWID(), N'CLOSED', N'已关闭')
	END
END
*/

/**
GatewayPaymentUser
**/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentUser]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentUser](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[UserCode] [nvarchar](20) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[PassWord] [nvarchar](50) NOT NULL,
	[StoreID] [int] NOT NULL,
	[PositionID] [int] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentUser] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
ALTER TABLE [dbo].[GatewayPaymentUser]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentUser_StoreId] FOREIGN KEY([StoreID])
REFERENCES [dbo].[store] ([store_code_id])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentUser] CHECK CONSTRAINT [FK_GatewayPaymentUser_StoreId]
GO
*/

/**
GatewayPaymentApp
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentApp]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentApp](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Appid] [nvarchar](20) NOT NULL,
	[Appkey] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentApp] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/**
GatewayPaymentMerchant
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMerchant]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentMerchant]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentMerchant](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_GatewayPaymentMerchant] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/**
GatewayPaymentAccount
**/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentAccount]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentAccount](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Appid] [nvarchar](50) NOT NULL,
	[Mchid] [nvarchar](50) NOT NULL,
	[Mchkey] [nvarchar](1000) NULL,
	[MchPublickey] [nvarchar](500) NULL,
	[Publickey] [nvarchar](500) NULL,
	[GatewayPaymentApp] [uniqueidentifier] NOT NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
	[GatewayPaymentMethod] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentAccount] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GatewayPaymentAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentApp] FOREIGN KEY([GatewayPaymentApp])
REFERENCES [dbo].[GatewayPaymentApp] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentAccount] CHECK CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentApp]
GO

ALTER TABLE [dbo].[GatewayPaymentAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMerchant] FOREIGN KEY([GatewayPaymentMerchant])
REFERENCES [dbo].[GatewayPaymentMerchant] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentAccount] CHECK CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMerchant]
GO

ALTER TABLE [dbo].[GatewayPaymentAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMethod] FOREIGN KEY([GatewayPaymentMethod])
REFERENCES [dbo].[GatewayPaymentMethod] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentAccount] CHECK CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMethod]
GO


/**
GatewayPaymentStore
**/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentStore]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentStore](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreId] [int] NOT NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
	[StoreName] [nvarchar](50) NULL,
 CONSTRAINT [PK_GatewayPaymentStore] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GatewayPaymentStore]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStore_GatewayPaymentMerchant] FOREIGN KEY([GatewayPaymentMerchant])
REFERENCES [dbo].[GatewayPaymentMerchant] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentStore] CHECK CONSTRAINT [FK_GatewayPaymentStore_GatewayPaymentMerchant]
GO

/*
ALTER TABLE [dbo].[GatewayPaymentStore]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStore_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[store] ([store_code_id])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentStore] CHECK CONSTRAINT [FK_GatewayPaymentStore_StoreId]
GO
*/

/**
GatewayPaymentStorePaymentMethod
**/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentStorePaymentMethod]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentStorePaymentMethod](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[GatewayPaymentStore] [uniqueidentifier] NOT NULL,
	[GatewayPaymentMethod] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentStorePaymentMethod] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethod] FOREIGN KEY([GatewayPaymentMethod])
REFERENCES [dbo].[GatewayPaymentMethod] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] CHECK CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethod]
GO

ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore] FOREIGN KEY([GatewayPaymentStore])
REFERENCES [dbo].[GatewayPaymentStore] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] CHECK CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]
GO


/**
GatewayPaymentCustomer
**/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentCustomer]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentCustomer]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentCustomer](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[District] [nvarchar](50) NOT NULL,
	[Street] [nvarchar](50) NOT NULL,
	[ZipCode] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[TSID] [datetime] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentCustomer] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GatewayPaymentCustomer] ADD CONSTRAINT [DF_GatewayPaymentCustomer_TSID]  DEFAULT (getdate()) FOR [TSID]
GO


/**
GatewayPaymentOrder
**/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrder]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentOrder](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreID] [int] NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[TotalAmount] [numeric](16, 2) NOT NULL,
	[Discount] [numeric](12, 2) NULL,
	[Tax] [numeric](12, 2) NULL,
	[ShippingFee] [numeric](12, 2) NULL,
	[GatewayPaymentMethod] [uniqueidentifier] NOT NULL,
	[GatewayPaymentOrderStatus] [uniqueidentifier] NOT NULL,
	[TSID] [datetime] NOT NULL,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[TradeNumber] [nvarchar](100) NULL,
	[OrderType] [uniqueidentifier] NOT NULL,
	[GatewayPaymentCustomer] [uniqueidentifier] NULL,
 CONSTRAINT [PK_GatewayPaymentOrder] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentCustomer] FOREIGN KEY([GatewayPaymentCustomer])
REFERENCES [dbo].[GatewayPaymentCustomer] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentCustomer]
GO

ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMethod] FOREIGN KEY([GatewayPaymentMethod])
REFERENCES [dbo].[GatewayPaymentMethod] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMethod]
GO

ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderStatus] FOREIGN KEY([GatewayPaymentOrderStatus])
REFERENCES [dbo].[GatewayPaymentOrderStatus] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]
GO

ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderType] FOREIGN KEY([OrderType])
REFERENCES [dbo].[GatewayPaymentOrderType] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderType]
GO

ALTER TABLE [dbo].[GatewayPaymentOrder] ADD CONSTRAINT [DF_GatewayPaymentOrder_TSID]  DEFAULT (getdate()) FOR [TSID]
GO


/**
GatewayPaymentOrderDetail
**/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderDetail]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrderDetail]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentOrderDetail](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[GatewayPaymentOrder] [uniqueidentifier] NOT NULL,
	[ItemName] [nvarchar](100) NOT NULL,
	[ItemQty] [numeric](12, 2) NOT NULL,
	[ItemAmount] [numeric](12, 2) NOT NULL,
	[TSID] [datetime] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentOrderDetail] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GatewayPaymentOrderDetail]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrderDetail_GatewayPaymentOrder] FOREIGN KEY([GatewayPaymentOrder])
REFERENCES [dbo].[GatewayPaymentOrder] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentOrderDetail] CHECK CONSTRAINT [FK_GatewayPaymentOrderDetail_GatewayPaymentOrder]
GO

ALTER TABLE [dbo].[GatewayPaymentOrderDetail] ADD CONSTRAINT [DF_GatewayPaymentOrderDetail_TSID]  DEFAULT (getdate()) FOR [TSID]
GO


