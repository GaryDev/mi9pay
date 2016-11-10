/**
GatewayPaymentBillAlipay
**/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentBillAlipay]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentBillAlipay]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentBillAlipay](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreId] [nvarchar](20) NOT NULL,
	[TradeNo] [nvarchar](30) NOT NULL,
	[OrderNumber] [nvarchar](30) NOT NULL,
	[TradeType] [nvarchar](10) NOT NULL,
	[ProductName] [nvarchar](30) NOT NULL,
	[StartTime] [nvarchar](20) NOT NULL,
	[EndTime] [nvarchar](20) NOT NULL,
	[StoreNumber] [nvarchar](20) NOT NULL,
	[StoreName] [nvarchar](30) NOT NULL,
	[Operator] [nvarchar](20) NOT NULL,
	[Terminal] [nvarchar](20) NOT NULL,
	[MchAccount] [nvarchar](20) NOT NULL,
	[TotalAmount] [nvarchar](20) NOT NULL,
	[ReceiveAmount] [nvarchar](20) NOT NULL,
	[AlipayRPAmount] [nvarchar](20) NOT NULL,
	[AlipayJFAmount] [nvarchar](20) NOT NULL,
	[AlipayPromoAmount] [nvarchar](20) NOT NULL,
	[MchPromoAmount] [nvarchar](20) NOT NULL,
	[CouponAmount] [nvarchar](20) NOT NULL,
	[CouponName] [nvarchar](20) NOT NULL,
	[MchRPAmount] [nvarchar](20) NOT NULL,
	[CardAmount] [nvarchar](20) NOT NULL,
	[RefundNumber] [nvarchar](30) NOT NULL,
	[Fee] [nvarchar](10) NOT NULL,
	[Profit] [nvarchar](10) NOT NULL,	
	[Note] [nvarchar](50) NOT NULL,	
	[TSID] [datetime] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentBillAlipay] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



