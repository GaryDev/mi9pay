IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentStorePaymentMethod]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentStorePaymentMethod](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[GatewayPaymentStore] [uniqueidentifier] NOT NULL,
	[PaymentMethodDefault] [bit] NULL,
	[GatewayPaymentMethodTypeJoin] [uniqueidentifier] NULL,
 CONSTRAINT [PK_GatewayPaymentStorePaymentMethod] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin] FOREIGN KEY([GatewayPaymentMethodTypeJoin])
REFERENCES [dbo].[GatewayPaymentMethodTypeJoin] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] CHECK CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore] FOREIGN KEY([GatewayPaymentStore])
REFERENCES [dbo].[GatewayPaymentStore] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] CHECK CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]
GO
