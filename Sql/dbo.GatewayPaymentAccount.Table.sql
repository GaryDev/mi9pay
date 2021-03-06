IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentAccount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentAccount](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Appid] [nvarchar](50) NOT NULL,
	[Mchid] [nvarchar](50) NOT NULL,
	[Mchkey] [nvarchar](1000) NULL,
	[MchPublickey] [nvarchar](500) NULL,
	[Publickey] [nvarchar](500) NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
	[GatewayPaymentMethod] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentAccount] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMerchant] FOREIGN KEY([GatewayPaymentMerchant])
REFERENCES [dbo].[GatewayPaymentMerchant] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount] CHECK CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMerchant]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMethod] FOREIGN KEY([GatewayPaymentMethod])
REFERENCES [dbo].[GatewayPaymentMethod] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount] CHECK CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMethod]
GO
