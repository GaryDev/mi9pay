/**
GatewayPaymentUser
**/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewayPaymentUser]') AND type in (N'U'))
DROP TABLE [dbo].[GatewayPaymentUser]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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

/*
ALTER TABLE [dbo].[GatewayPaymentUser]  WITH NOCHECK ADD  CONSTRAINT [FK_GatewayPaymentUser_StoreId] FOREIGN KEY([StoreID])
REFERENCES [dbo].[store] ([store_code_id])
NOT FOR REPLICATION 
GO

ALTER TABLE [dbo].[GatewayPaymentUser] CHECK CONSTRAINT [FK_GatewayPaymentUser_StoreId]
GO
*/

