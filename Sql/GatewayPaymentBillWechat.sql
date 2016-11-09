/**
GatewayPaymentBillWechat
**/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentBillWechat]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentBillWechat]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentBillWechat](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreId] [nvarchar](20) NOT NULL,
	[TradeTime] [nvarchar](20) NOT NULL,
	[GHId] [nvarchar](20) NOT NULL,
	[MchId] [nvarchar](20) NOT NULL,
	[SubMch] [nvarchar](10) NOT NULL,
	[DeviceId] [nvarchar](16) NOT NULL,
	[TradeNo] [nvarchar](30) NOT NULL,
	[OrderNumber] [nvarchar](30) NOT NULL,
	[OpenId] [nvarchar](30) NOT NULL,
	[TradeType] [nvarchar](10) NOT NULL,
	[TradeStatus] [nvarchar](10) NOT NULL,
	[Bank] [nvarchar](16) NOT NULL,
	[Currency] [nvarchar](20) NOT NULL,
	[TotalAmount] [nvarchar](20) NOT NULL,
	[PromoAmount] [nvarchar](20) NOT NULL,
	[RefundTradeNo] [nvarchar](10) NULL,
	[RefundOrderNo] [nvarchar](10) NULL,
	[RefundAmount] [nvarchar](20) NULL,
	[RefundPromoAmount] [nvarchar](20) NULL,
	[RefundType] [nvarchar](10) NULL,
	[RefundStatus] [nvarchar](10) NULL,
	[ProductName] [nvarchar](30) NOT NULL,
	[DataPackage] [nvarchar](10) NULL,
	[Fee] [nvarchar](10) NOT NULL,
	[Rate] [nvarchar](10) NOT NULL,
	[TSID] [datetime] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentBillWechat] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



