/**
GatewayPaymentApp
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentApp]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentApp](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Appid] [nvarchar](20) NOT NULL,
	[Appkey] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentApp] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


