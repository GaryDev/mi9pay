IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentApp](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Appid] [nvarchar](20) NOT NULL,
	[Appkey] [nvarchar](50) NOT NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentApp] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentApp_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]'))
ALTER TABLE [dbo].[GatewayPaymentApp]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentApp_GatewayPaymentMerchant] FOREIGN KEY([GatewayPaymentMerchant])
REFERENCES [dbo].[GatewayPaymentMerchant] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentApp_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]'))
ALTER TABLE [dbo].[GatewayPaymentApp] CHECK CONSTRAINT [FK_GatewayPaymentApp_GatewayPaymentMerchant]
GO
