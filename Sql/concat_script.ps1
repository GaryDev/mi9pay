Remove-Item Mi9PaySQL_Tabel_Create.sql

Get-Content ConstraintDrop_GatewayPaymentTables.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentMethod.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentOrderType.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentOrderStatus.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentMerchant.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentApp.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentAccount.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentStore.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentMethodType.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentMethodTypeJoin.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentStorePaymentMethod.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentPosition.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentUser.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentCustomer.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentOrder.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentOrderDetail.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentNotifyQueue.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentBillAlipay.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content dbo.GatewayPaymentBillWechat.Table.sql | Add-Content Mi9PaySQL_Tabel_Create.sql