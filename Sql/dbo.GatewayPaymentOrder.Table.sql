IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrder]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentOrder](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreID] [int] NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[TotalAmount] [numeric](16, 2) NOT NULL,
	[Discount] [numeric](12, 2) NULL,
	[Tax] [numeric](12, 2) NULL,
	[ShippingFee] [numeric](12, 2) NULL,
	[GatewayPaymentOrderStatus] [uniqueidentifier] NOT NULL,
	[TSID] [datetime] NOT NULL,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[TradeNumber] [nvarchar](100) NULL,
	[OrderType] [uniqueidentifier] NOT NULL,
	[GatewayPaymentCustomer] [uniqueidentifier] NULL,
	[GatewayPaymentStorePaymentMethod] [uniqueidentifier] NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentOrder] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

ALTER TABLE [dbo].[GatewayPaymentOrder] ADD CONSTRAINT [DF_GatewayPaymentOrder_TSID] DEFAULT (getdate()) FOR [TSID]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentCustomer]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentCustomer] FOREIGN KEY([GatewayPaymentCustomer])
REFERENCES [dbo].[GatewayPaymentCustomer] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentCustomer]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentCustomer]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMerchant] FOREIGN KEY([GatewayPaymentMerchant])
REFERENCES [dbo].[GatewayPaymentMerchant] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMerchant]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderStatus] FOREIGN KEY([GatewayPaymentOrderStatus])
REFERENCES [dbo].[GatewayPaymentOrderStatus] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderType]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderType] FOREIGN KEY([OrderType])
REFERENCES [dbo].[GatewayPaymentOrderType] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderType]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod] FOREIGN KEY([GatewayPaymentStorePaymentMethod])
REFERENCES [dbo].[GatewayPaymentStorePaymentMethod] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod]
GO
