/**
GatewayPaymentOrderType
**/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderType]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrderType]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GatewayPaymentOrderType](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentOrderType] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderType]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[GatewayPaymentOrderType] WHERE [Code] = 'MOSAIC')
	BEGIN
		insert into GatewayPaymentOrderType
		values (NEWID(), N'MOSAIC', N'Mosaic')
	END
END
*/


