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


