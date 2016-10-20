/**
GatewayPaymentOrderStatus
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderStatus]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrderStatus]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentOrderStatus](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentOrderStatus] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderStatus]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentOrderStatus] WHERE [Code] = 'UNPAID')
	BEGIN
		insert into GatewayPaymentOrderStatus
		values (NEWID(), N'UNPAID', N'未支付')
	END
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentOrderStatus] WHERE [Code] = 'PAID')
	BEGIN
		insert into GatewayPaymentOrderStatus
		values (NEWID(), N'PAID', N'已支付')
	END
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentOrderStatus] WHERE [Code] = 'REFUND')
	BEGIN
		insert into GatewayPaymentOrderStatus
		values (NEWID(), N'REFUND', N'已退款')
	END
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentOrderStatus] WHERE [Code] = 'CLOSED')
	BEGIN
		insert into GatewayPaymentOrderStatus
		values (NEWID(), N'CLOSED', N'已关闭')
	END
END
*/

