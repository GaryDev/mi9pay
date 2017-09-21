/**
GatewayPaymentApp CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentApp_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]'))
ALTER TABLE [dbo].[GatewayPaymentApp] DROP CONSTRAINT [FK_GatewayPaymentApp_GatewayPaymentMerchant]
GO

/**
GatewayPaymentAccount CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentApp]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount] DROP CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentApp]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount] DROP CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMerchant]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount] DROP CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMethod]
GO

/**
GatewayPaymentCustomer CONSTRAINT
**/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GatewayPaymentCustomer_TSID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GatewayPaymentCustomer] DROP CONSTRAINT [DF_GatewayPaymentCustomer_TSID]
END
GO

/**
GatewayPaymentOrder CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentCustomer]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentCustomer]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMerchant]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMethod]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderType]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderType]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GatewayPaymentOrder_TSID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GatewayPaymentOrder] DROP CONSTRAINT [DF_GatewayPaymentOrder_TSID]
END
GO

/**
GatewayPaymentOrderDetail CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrderDetail_GatewayPaymentOrder]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderDetail]'))
ALTER TABLE [dbo].[GatewayPaymentOrderDetail] DROP CONSTRAINT [FK_GatewayPaymentOrderDetail_GatewayPaymentOrder]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GatewayPaymentOrderDetail_TSID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GatewayPaymentOrderDetail] DROP CONSTRAINT [DF_GatewayPaymentOrderDetail_TSID]
END
GO

/**
GatewayPaymentStore CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStore_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]'))
ALTER TABLE [dbo].[GatewayPaymentStore] DROP CONSTRAINT [FK_GatewayPaymentStore_GatewayPaymentMerchant]
GO


/**
GatewayPaymentStorePaymentMethod CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] DROP CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethod]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] DROP CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] DROP CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]
GO

/**
GatewayPaymentMethodTypeJoin CONSTRAINT
**/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]'))
ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin] DROP CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethod]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethodType]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]'))
ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin] DROP CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethodType]
GO

/**
GatewayPaymentUser CONSTRAINT
**/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentPosition]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] DROP CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentPosition]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] DROP CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentStore]
GO
/****** Object:  Table [dbo].[GatewayPaymentMethod]    Script Date: 11/16/2016 13:25:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethod]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentMethod]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethod]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentMethod](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](20) NULL,
 CONSTRAINT [PK_GatewayPaymentMethod] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GatewayPaymentOrderType]    Script Date: 11/16/2016 13:25:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderType]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrderType]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentOrderType](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentOrderType] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GatewayPaymentOrderStatus]    Script Date: 11/16/2016 13:25:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderStatus]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrderStatus]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentOrderStatus](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentOrderStatus] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GatewayPaymentMerchant]    Script Date: 11/16/2016 13:25:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMerchant]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentMerchant]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMerchant]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentMerchant](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_GatewayPaymentMerchant] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentApp](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Appid] [nvarchar](20) NOT NULL,
	[Appkey] [nvarchar](50) NOT NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentApp] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentApp_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]'))
ALTER TABLE [dbo].[GatewayPaymentApp]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentApp_GatewayPaymentMerchant] FOREIGN KEY([GatewayPaymentMerchant])
REFERENCES [dbo].[GatewayPaymentMerchant] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentApp_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentApp]'))
ALTER TABLE [dbo].[GatewayPaymentApp] CHECK CONSTRAINT [FK_GatewayPaymentApp_GatewayPaymentMerchant]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentAccount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentAccount](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Appid] [nvarchar](50) NOT NULL,
	[Mchid] [nvarchar](50) NOT NULL,
	[Mchkey] [nvarchar](1000) NULL,
	[MchPublickey] [nvarchar](500) NULL,
	[Publickey] [nvarchar](500) NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
	[GatewayPaymentMethod] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentAccount] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMerchant] FOREIGN KEY([GatewayPaymentMerchant])
REFERENCES [dbo].[GatewayPaymentMerchant] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount] CHECK CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMerchant]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMethod] FOREIGN KEY([GatewayPaymentMethod])
REFERENCES [dbo].[GatewayPaymentMethod] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentAccount_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentAccount]'))
ALTER TABLE [dbo].[GatewayPaymentAccount] CHECK CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMethod]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentStore]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentStore](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreId] [int] NOT NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
	[StoreName] [nvarchar](50) NULL,
 CONSTRAINT [PK_GatewayPaymentStore] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStore_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]'))
ALTER TABLE [dbo].[GatewayPaymentStore]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStore_GatewayPaymentMerchant] FOREIGN KEY([GatewayPaymentMerchant])
REFERENCES [dbo].[GatewayPaymentMerchant] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStore_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStore]'))
ALTER TABLE [dbo].[GatewayPaymentStore] CHECK CONSTRAINT [FK_GatewayPaymentStore_GatewayPaymentMerchant]
GO
/****** Object:  Table [dbo].[GatewayPaymentMethodType]    Script Date: 11/16/2016 13:25:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodType]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentMethodType]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentMethodType](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentMethodType] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentMethodTypeJoin]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentMethodTypeJoin](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[GatewayPaymentMethod] [uniqueidentifier] NOT NULL,
	[GatewayPaymentMethodType] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentMethodTypeJoin] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]'))
ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethod] FOREIGN KEY([GatewayPaymentMethod])
REFERENCES [dbo].[GatewayPaymentMethod] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]'))
ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin] CHECK CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethod]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethodType]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]'))
ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethodType] FOREIGN KEY([GatewayPaymentMethodType])
REFERENCES [dbo].[GatewayPaymentMethodType] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethodType]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentMethodTypeJoin]'))
ALTER TABLE [dbo].[GatewayPaymentMethodTypeJoin] CHECK CONSTRAINT [FK_GatewayPaymentMethodTypeJoin_GatewayPaymentMethodType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentStorePaymentMethod]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentStorePaymentMethod](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[GatewayPaymentStore] [uniqueidentifier] NOT NULL,
	[PaymentMethodDefault] [bit] NULL,
	[GatewayPaymentMethodTypeJoin] [uniqueidentifier] NULL,
 CONSTRAINT [PK_GatewayPaymentStorePaymentMethod] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin] FOREIGN KEY([GatewayPaymentMethodTypeJoin])
REFERENCES [dbo].[GatewayPaymentMethodTypeJoin] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] CHECK CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentMethodTypeJoin]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore] FOREIGN KEY([GatewayPaymentStore])
REFERENCES [dbo].[GatewayPaymentStore] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentStorePaymentMethod]'))
ALTER TABLE [dbo].[GatewayPaymentStorePaymentMethod] CHECK CONSTRAINT [FK_GatewayPaymentStorePaymentMethod_GatewayPaymentStore]
GO
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
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentUser]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentUser](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[UserCode] [nvarchar](20) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[PassWord] [nvarchar](50) NOT NULL,
	[GatewayPaymentStore] [uniqueidentifier] NOT NULL,
	[GatewayPaymentPosition] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentUser] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentPosition]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentPosition] FOREIGN KEY([GatewayPaymentPosition])
REFERENCES [dbo].[GatewayPaymentPosition] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentPosition]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] CHECK CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentPosition]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentStore] FOREIGN KEY([GatewayPaymentStore])
REFERENCES [dbo].[GatewayPaymentStore] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] CHECK CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentStore]
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
	[TSID] [datetime] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentCustomer] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

ALTER TABLE [dbo].[GatewayPaymentCustomer] ADD CONSTRAINT [DF_GatewayPaymentCustomer_TSID] DEFAULT (getdate()) FOR [TSID]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrder]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentOrder](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreID] [int] NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[TotalAmount] [numeric](16, 2) NOT NULL,
	[Discount] [numeric](12, 2) NULL,
	[Tax] [numeric](12, 2) NULL,
	[ShippingFee] [numeric](12, 2) NULL,
	[GatewayPaymentOrderStatus] [uniqueidentifier] NOT NULL,
	[TSID] [datetime] NOT NULL,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[TradeNumber] [nvarchar](100) NULL,
	[OrderType] [uniqueidentifier] NOT NULL,
	[GatewayPaymentCustomer] [uniqueidentifier] NULL,
	[GatewayPaymentStorePaymentMethod] [uniqueidentifier] NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentOrder] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

ALTER TABLE [dbo].[GatewayPaymentOrder] ADD CONSTRAINT [DF_GatewayPaymentOrder_TSID] DEFAULT (getdate()) FOR [TSID]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentCustomer]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentCustomer] FOREIGN KEY([GatewayPaymentCustomer])
REFERENCES [dbo].[GatewayPaymentCustomer] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentCustomer]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentCustomer]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMerchant] FOREIGN KEY([GatewayPaymentMerchant])
REFERENCES [dbo].[GatewayPaymentMerchant] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentMerchant]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMerchant]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderStatus] FOREIGN KEY([GatewayPaymentOrderStatus])
REFERENCES [dbo].[GatewayPaymentOrderStatus] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderType]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderType] FOREIGN KEY([OrderType])
REFERENCES [dbo].[GatewayPaymentOrderType] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentOrderType]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod] FOREIGN KEY([GatewayPaymentStorePaymentMethod])
REFERENCES [dbo].[GatewayPaymentStorePaymentMethod] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrder]'))
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentStorePaymentMethod]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderDetail]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentOrderDetail]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentOrderDetail](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[GatewayPaymentOrder] [uniqueidentifier] NOT NULL,
	[ItemName] [nvarchar](100) NOT NULL,
	[ItemQty] [numeric](12, 2) NOT NULL,
	[ItemAmount] [numeric](12, 2) NOT NULL,
	[TSID] [datetime] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentOrderDetail] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

ALTER TABLE [dbo].[GatewayPaymentOrderDetail] ADD CONSTRAINT [DF_GatewayPaymentOrderDetail_TSID] DEFAULT (getdate()) FOR [TSID]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrderDetail_GatewayPaymentOrder]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderDetail]'))
ALTER TABLE [dbo].[GatewayPaymentOrderDetail]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrderDetail_GatewayPaymentOrder] FOREIGN KEY([GatewayPaymentOrder])
REFERENCES [dbo].[GatewayPaymentOrder] ([UniqueId])
NOT FOR REPLICATION
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentOrderDetail_GatewayPaymentOrder]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentOrderDetail]'))
ALTER TABLE [dbo].[GatewayPaymentOrderDetail] CHECK CONSTRAINT [FK_GatewayPaymentOrderDetail_GatewayPaymentOrder]
GO
/****** Object:  Table [dbo].[GatewayPaymentNotifyQueue]    Script Date: 11/16/2016 13:25:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentNotifyQueue]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentNotifyQueue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentNotifyQueue]') AND type in (N'U'))
BEGIN
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
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GatewayPaymentBillAlipay]    Script Date: 11/16/2016 13:25:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentBillAlipay]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentBillAlipay]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentBillAlipay]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentBillAlipay](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreId] [nvarchar](20) NOT NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
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
END
GO
/****** Object:  Table [dbo].[GatewayPaymentBillWechat]    Script Date: 11/16/2016 13:25:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentBillWechat]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentBillWechat]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentBillWechat]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewayPaymentBillWechat](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreId] [nvarchar](20) NOT NULL,
	[GatewayPaymentMerchant] [uniqueidentifier] NOT NULL,
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
END
GO
