/****** Object:  Table [dbo].[GatewayPaymentPosition]    Script Date: 11/16/2016 13:25:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentPosition]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentPosition]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentPosition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentPosition](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[PositionID] [int] NOT NULL,
	[PositionName] [nvarchar](50) NULL,
 CONSTRAINT [PK_GatewayPaymentPosition] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
