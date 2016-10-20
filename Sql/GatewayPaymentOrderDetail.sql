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


