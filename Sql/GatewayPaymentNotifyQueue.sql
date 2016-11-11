/**
GatewayPaymentNotifyQueue
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentNotifyQueue]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentNotifyQueue]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentNotifyQueue](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[OrderNumber] [nvarchar](20) NOT NULL,
	[NotifyUrl] [nvarchar](200) NOT NULL,
	[PostData] [nvarchar](4000) NOT NULL,
	[PostDataFormat] [nvarchar](10) NOT NULL,
	[SendDate] [datetime] NOT NULL,
	[LastSendDate] [datetime] NOT NULL,
	[NextInterval] [int] NOT NULL,
	[ProcessedCount] [int] NOT NULL,
	[Processed] [char](1) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentNotifyQueue] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


