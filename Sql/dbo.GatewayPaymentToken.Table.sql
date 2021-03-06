IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentToken]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentToken]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentToken]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentToken](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[AuthToken] [nvarchar](50) NOT NULL,
	[IssuedOn] [datetime] NOT NULL,
	[ExpiresOn] [datetime] NOT NULL,
	[GatewayPaymentUser] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentToken] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentToken_GatewayPaymentUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentToken]'))
ALTER TABLE [dbo].[GatewayPaymentToken]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentToken_GatewayPaymentUser] FOREIGN KEY([GatewayPaymentUser])
REFERENCES [dbo].[GatewayPaymentUser] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentToken_GatewayPaymentUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentToken]'))
ALTER TABLE [dbo].[GatewayPaymentToken] CHECK CONSTRAINT [FK_GatewayPaymentToken_GatewayPaymentUser]
GO
