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


