/****** Object:  Table [dbo].[GatewayPaymentUser]    Script Date: 11/16/2016 13:25:43 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentPosition]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] DROP CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentPosition]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] DROP CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentStore]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentPosition]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] DROP CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentPosition]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewayPaymentUser_GatewayPaymentStore]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]'))
ALTER TABLE [dbo].[GatewayPaymentUser] DROP CONSTRAINT [FK_GatewayPaymentUser_GatewayPaymentStore]
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
