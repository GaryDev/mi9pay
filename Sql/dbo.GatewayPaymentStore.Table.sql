IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentStore]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]') AND type in (N'U'))
BEGIN
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
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStore_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]'))
ALTER TABLE [dbo].[GatewayPaymentStore]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStore_GatewayPaymentMerchant] FOREIGN KEY([GatewayPaymentMerchant])
REFERENCES [dbo].[GatewayPaymentMerchant] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStore_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]'))
ALTER TABLE [dbo].[GatewayPaymentStore] CHECK CONSTRAINT [FK_GatewayPaymentStore_GatewayPaymentMerchant]
GO
