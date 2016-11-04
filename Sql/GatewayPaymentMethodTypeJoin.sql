/**
GatewayPaymentMethodTypeJoin
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentMethodTypeJoin]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentMethodTypeJoin](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[GatewayPaymentMethod] [uniqueidentifier] NOT NULL,
	[GatewayPaymentMethodType] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentMethodTypeJoin] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethod] FOREIGN KEY([GatewayPaymentMethod])
REFERENCES [dbo].[GatewayPaymentMethod] ([UniqueId])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin] CHECK CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethod]
GO

ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethodType] FOREIGN KEY([GatewayPaymentMethodType])
REFERENCES [dbo].[GatewayPaymentMethodType] ([UniqueId])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin] CHECK CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethodType]
GO
