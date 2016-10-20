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

