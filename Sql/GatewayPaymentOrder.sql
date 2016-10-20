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


