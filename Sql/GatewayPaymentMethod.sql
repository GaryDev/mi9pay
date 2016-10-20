/**
GatewayPaymentMethod
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethod]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentMethod]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentMethod](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](20) NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentMethod] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethod]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentMethod] WHERE [Code] = 'wechatpayment')
	BEGIN
		insert into GatewayPaymentMethod
		values (NEWID(), 'wechatpayment', N'微信', N'微信支付')
	END
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentMethod] WHERE [Code] = 'alipay')
	BEGIN
		insert into GatewayPaymentMethod
		values (NEWID(), 'alipay', N'支付宝', N'支付宝支付')
	END
END
*/

