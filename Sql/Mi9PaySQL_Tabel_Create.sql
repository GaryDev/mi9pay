
-- 支付网关 - 支付方式

CREATE TABLE [dbo].[GatewayPaymentMethod](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GatewayPaymentMethod] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

insert into GatewayPaymentMethod
values (NEWID(), 'wechatpayment', N'微信', N'微信支付')
insert into GatewayPaymentMethod
values (NEWID(), 'alipay', N'支付宝', N'支付宝支付')

-- 支付网关 - 系统AppId

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

-- 支付网关 - 门店和支付账号对应关系

CREATE TABLE [dbo].[GatewayPaymentStore](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreId] [int] NOT NULL,
	[StoreName] [nvarchar](50) NULL,
	[GatewayPaymentAccount] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentStore] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GatewayPaymentStore]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStore_GatewayPaymentAccount] FOREIGN KEY([GatewayPaymentAccount])
REFERENCES [dbo].[GatewayPaymentAccount] ([UniqueId])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentStore] CHECK CONSTRAINT [FK_GatewayPaymentStore_GatewayPaymentAccount]
GO

ALTER TABLE [dbo].[GatewayPaymentStore]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentStore_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([store_code_id])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentStore] CHECK CONSTRAINT [FK_GatewayPaymentStore_StoreId]
GO

-- 支付网关 - 支付账号
/*
Appid // 微信wx000000000000000 /支付宝 00000000000000
Mchid // 微信mch_id / 支付宝 pid
Mchkey // 微信mch_key / 支付宝 merchant_private_key
MchPublickey // 微信NULL / 支付宝 merchant_public_key
Publickey    // 微信NULL / 支付宝 alipay_public_key
*/
CREATE TABLE [dbo].[GatewayPaymentAccount](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Appid] [nvarchar](50) NOT NULL,
	[Mchid] [nvarchar](50) NOT NULL,
	[Mchkey] [nvarchar](1000) NOT NULL,
	[MchPublickey] [nvarchar](500) NULL,
	[Publickey] [nvarchar](500) NULL,
	[GatewayPaymentApp] [uniqueidentifier] NOT NULL,
	[GatewayPaymentMethod] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentAccount] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GatewayPaymentAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentApp] FOREIGN KEY([GatewayPaymentApp])
REFERENCES [dbo].[GatewayPaymentApp] ([UniqueId])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentAccount] CHECK CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentApp]
GO

ALTER TABLE [dbo].[GatewayPaymentAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMethod] FOREIGN KEY([GatewayPaymentMethod])
REFERENCES [dbo].[GatewayPaymentMethod] ([UniqueId])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentAccount] CHECK CONSTRAINT [FK_GatewayPaymentAccount_GatewayPaymentMethod]
GO

-- 支付订单管理系统 - 用户

CREATE TABLE [dbo].[GatewayPaymentUser](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[UserCode] [nvarchar](20) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[PassWord] [nvarchar](50) NOT NULL,
	[StoreID] [int] NOT NULL,
	[PositionID] [int] NOT NULL,
 CONSTRAINT [PK_GatewayPaymentUser] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GatewayPaymentUser]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentUser_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([store_code_id])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentUser] CHECK CONSTRAINT [FK_GatewayPaymentUser_StoreId]
GO


-- 支付订单管理系统 - 支付订单状态

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

insert into GatewayPaymentOrderStatus
values (NEWID(), N'UNPAID', N'未支付')
insert into GatewayPaymentOrderStatus
values (NEWID(), N'PAID', N'已支付')
insert into GatewayPaymentOrderStatus
values (NEWID(), N'REFUND', N'已退款')
insert into GatewayPaymentOrderStatus
values (NEWID(), N'CLOSED', N'已关闭')

-- 支付订单管理系统 - 支付订单来源

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

insert into GatewayPaymentOrderType
values (NEWID(), N'MOSAIC', N'Mosaic')

-- 支付订单管理系统 - 支付订单客户

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
GO

-- 支付订单管理系统 - 支付订单

CREATE TABLE [dbo].[GatewayPaymentOrder](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[StoreID] [int] NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[OrderType] [uniqueidentifier] NOT NULL,
	[TradeNumber] [nvarchar](100) NULL,
	[TotalAmount] [numeric](16, 2) NOT NULL,
	[Discount] [numeric](12, 2) NULL,
	[Tax] [numeric](12, 2) NULL,
	[ShippingFee] [numeric](12, 2) NULL,
	[GatewayPaymentMethod] [uniqueidentifier] NOT NULL,
    [GatewayPaymentOrderStatus] [uniqueidentifier] NOT NULL,
	[GatewayPaymentCustomer] [uniqueidentifier] NULL,
	[TSID] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_GatewayPaymentOrder] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMethod] FOREIGN KEY([GatewayPaymentMethod])
REFERENCES [dbo].[GatewayPaymentMethod] ([UniqueId])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentMethod]
GO

ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderStatus] FOREIGN KEY([GatewayPaymentOrderStatus])
REFERENCES [dbo].[GatewayPaymentOrderStatus] ([UniqueId])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderStatus]
GO

ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderType] FOREIGN KEY([OrderType])
REFERENCES [dbo].[GatewayPaymentOrderType] ([UniqueId])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentOrderType]
GO

ALTER TABLE [dbo].[GatewayPaymentOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentCustomer] FOREIGN KEY([GatewayPaymentCustomer])
REFERENCES [dbo].[GatewayPaymentCustomer] ([UniqueId])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentOrder] CHECK CONSTRAINT [FK_GatewayPaymentOrder_GatewayPaymentCustomer]
GO


-- 支付订单管理系统 - 支付订单明细

CREATE TABLE [dbo].[GatewayPaymentOrderDetail](
	[UniqueId] [uniqueidentifier] NOT NULL,
	[GatewayPaymentOrder] [uniqueidentifier] NOT NULL,
	[ItemName] [nvarchar](100) NOT NULL,
	[ItemQty] [numeric](12, 2) NOT NULL,
	[ItemAmount] [numeric](12, 2) NOT NULL,
	[TSID] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_GatewayPaymentOrderDetail] PRIMARY KEY CLUSTERED 
(
	[UniqueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GatewayPaymentOrderDetail]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentOrderDetail_GatewayPaymentOrder] FOREIGN KEY([GatewayPaymentOrder])
REFERENCES [dbo].[GatewayPaymentOrder] ([UniqueId])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[GatewayPaymentOrderDetail] CHECK CONSTRAINT [FK_GatewayPaymentOrderDetail_GatewayPaymentOrder]
GO




