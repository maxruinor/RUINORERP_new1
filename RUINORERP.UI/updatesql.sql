UPDATE a
SET a.PurOrder_NO = b.PurOrderNo  FROM tb_PurEntry a , tb_PurOrder b
WHERE
	a.PurOrder_ID= b.PurOrder_ID


暂时无法在设计器直接操作，只能用代码

Alter table tb_PurOrderDetail add property varchar(255)  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '属性', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_PurOrderDetail', @level2type = N'COLUMN', @level2name = N'property';
go




Alter table tb_Employee add PhoneNumber varchar(50)  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '手机号', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_Employee', @level2type = N'COLUMN', @level2name = N'PhoneNumber';
go



Alter table tb_SaleOrderDetail add property varchar(255)  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '属性', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_SaleOrderDetail', @level2type = N'COLUMN', @level2name = N'property';
go


tb_SaleOutDetail



ALTER TABLE tb_SaleOrder
ALTER COLUMN SaleDate datetime NOT NULL


Alter table tb_SaleOutDetail add property varchar(255)  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '属性', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_SaleOutDetail', @level2type = N'COLUMN', @level2name = N'property';
go

------------------------------------------销售出库明细

Alter table tb_SaleOutDetail add UnitPrice money  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '单价', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_SaleOutDetail', @level2type = N'COLUMN', @level2name = N'UnitPrice';
go

Alter table tb_SaleOutDetail add Discount decimal(5,3)  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '折扣', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_SaleOutDetail', @level2type = N'COLUMN', @level2name = N'Discount';
go


Alter table tb_SaleOutDetail add UntaxedAmont money  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '未税本位币', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_SaleOutDetail', @level2type = N'COLUMN', @level2name = N'UntaxedAmont';
go

Alter table tb_SaleOutDetail add CommissionAmont money  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '抽成金额', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_SaleOutDetail', @level2type = N'COLUMN', @level2name = N'CommissionAmont';
go

Alter table tb_SaleOutDetail add DeliveredQty int  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '已出库数量', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_SaleOutDetail', @level2type = N'COLUMN', @level2name = N'DeliveredQty';
go

Alter table tb_SaleOutDetail add TaxSubtotalAmount money  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '税额', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_SaleOutDetail', @level2type = N'COLUMN', @level2name = N'TaxSubtotalAmount';
go

ALTER TABLE tb_PurEntry
ALTER COLUMN EntryDate datetime NOT NULL


Alter table tb_PurEntryDetail add property varchar(255)  --添加一列
GO

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '属性', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_PurEntryDetail', @level2type = N'COLUMN', @level2name = N'property';
go


---------------------



Alter table tb_CustomerVendor add Is_available bit default 1  --添加一列
GO
Alter table tb_CustomerVendor add Is_enabled bit default 1  --添加一列
GO
Alter table tb_CustomerVendor add isdeleted bit default 0  --添加一列
GO

ALTER TABLE tb_CustomerVendor
ALTER COLUMN isdeleted bit NOT NULL

--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '是否启用', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_CustomerVendor', @level2type = N'COLUMN', @level2name = N'Is_enabled';
go
--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '是否可用', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_CustomerVendor', @level2type = N'COLUMN', @level2name = N'Is_available';
go
--上面列加注释
EXEC sp_addextendedproperty @name = N'MS_Description', @value = '逻辑删除', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tb_CustomerVendor', @level2type = N'COLUMN', @level2name = N'isdeleted';
go


--将费用表改为明细表。再添加一个费用主表
exec sp_rename 'tb_FM_OtherExpense', 'tb_FM_OtherExpenseDetail' 

-- 添加外键关系
ALTER TABLE 从表 ADD CONSTRAINT fk_fromtable_to_maintable FOREIGN KEY (主表id) REFERENCES 主表(id);


--增加长度
alter table tb_FM_OtherExpenseDetail 
alter column ExpenseName varchar(300)

Alter table tb_Currency add Is_BaseCurrency bit default 0  --添加一列


Alter table tb_SaleOutReDetail add TaxRate decimal(5,2) not null default 0  --添加一列

Alter table tb_FM_OtherExpense add ApprovedAmount money not null default 0  --添加一列


-------------添加外键关系  上面是自己下面是关联的表，建好字段后建关系

Alter table tb_FM_OtherExpenseDetail add ProjectGroup_ID bigint  --添加一列
GO
Alter table tb_SaleOrder add ProjectGroup_ID bigint  --添加一列
GO


Alter table tb_RoleInfo add RolePropertyID bigint  --添加一列



Alter table tb_SaleOut add SaleOrderNo varchar(50)   --添加一列


Alter table tb_MenuInfo add DefaultLayout text   --添加一列