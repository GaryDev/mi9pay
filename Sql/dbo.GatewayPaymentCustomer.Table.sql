/****** Object:  Table [dbo].[GatewayPaymentCustomer]    Script Date: 11/16/2016 13:25:42 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__GatewayPay__TSID__4B35A300]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GatewayPaymentCustomer] DROP CONSTRAINT [DF__GatewayPay__TSID__4B35A300]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentCustomer]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentCustomer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentCustomer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentCustomer](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[District] [nvarchar](50) NOT NULL,
	[Street] [nvarchar](50) NOT NULL,
	[ZipCode] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[TSID] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_GatewayPaymentCustomer] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
