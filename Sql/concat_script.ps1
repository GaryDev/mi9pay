Remove-Item Mi9PaySQL_Tabel_Create.sql

Get-Content ConstraintDrop_GatewayPaymentTables.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentMethod.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentOrderType.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentOrderStatus.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentUser.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentApp.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentMerchant.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentAccount.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentStore.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentStorePaymentMethod.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentCustomer.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentOrder.sql | Add-Content Mi9PaySQL_Tabel_Create.sql
Get-Content GatewayPaymentOrderDetail.sql | Add-Content Mi9PaySQL_Tabel_Create.sql