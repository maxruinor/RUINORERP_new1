--查出重复字段
SELECT * from tb_P4Field bb WHERE FieldInfo_ID = 1740603351386689543

(
SELECT FieldInfo_ID   from ( 

SELECT MAX(FieldInfo_ID) AS FieldInfo_ID , FieldName,ClassPath,MenuID  from tb_FieldInfo WHERE IsChild=1 GROUP BY FieldName,ClassPath,MenuID HAVING COUNT(FieldName)>1 

) aa )


-----关联式更新数据
update A
set A1=B.B1,A2=B.B2,A3=B.B3,A4=B.B4
from A,B
where A.AID=B.BID
--------------------------------------------------------------
UPDATE tb_SaleOut set ProjectGroup_ID=B.ProjectGroup_ID
from tb_SaleOut A,tb_SaleOrder B WHERE a.SOrder_ID=b.SOrder_ID
and B.SOrder_ID=1742451678940106752



---------修复采购订单和入库数据


SELECT * from tb_PurOrder WHERE PurOrderNo='PO7E81B003'

UPDATE tb_PurOrder  set  TotalQty=20000 , TotalAmount=1100 , DataStatus=8 WHERE PurOrderNo='PO7E81B003'


SELECT * from tb_PurOrderDetail WHERE PurOrder_ID='1745285521581674496'

UPDATE tb_PurOrderDetail set DeliveredQuantity=20000,Quantity=20000,UnitPrice=0.055,TransactionPrice=0.055 WHERE  PurOrder_ChildID='1745285521615228928'

UPDATE tb_PurOrder set DataStatus=8 WHERE PurOrder_ID in (


SELECT PurOrder_ID, PurOrderNo as 订单号,sum(Quantity) 订单数量,sum(DeliveredQuantity) as 已交数量 FROM 
(


SELECT PurOrderNo,tb_PurOrder.PurOrder_ID, tb_PurOrderDetail.Quantity,tb_PurOrderDetail.DeliveredQuantity 
FROM tb_PurOrder
JOIN tb_PurOrderDetail ON tb_PurOrder.PurOrder_ID = tb_PurOrderDetail.PurOrder_ID
WHERE 


tb_PurOrderDetail.DeliveredQuantity = tb_PurOrderDetail.Quantity 

AND tb_PurOrder.DataStatus=2


)  AS TT GROUP BY PurOrderNo,PurOrder_ID


)


-----------------
--重复建了客户资料。
--将一个替换另一个后。删除
--目前是销售订单和出库单
SELECT * from  tb_CustomerVendor WHERE CVName like '%深圳创新安电子科技有限公司%'
GO

SELECT * FROM tb_SaleOrder WHERE CustomerVendor_ID =1742468592378712064
GO
SELECT * from tb_SaleOut WHERE   CustomerVendor_ID =1742468592378712064
GO

UPDATE tb_SaleOrder set CustomerVendor_ID=1742450060072980480 WHERE CustomerVendor_ID =1742468592378712064
UPDATE tb_SaleOut set CustomerVendor_ID=1742450060072980480 WHERE CustomerVendor_ID =1742468592378712064
UPDATE tb_StockOut set  CustomerVendor_ID=1747521040147419136 WHERE CustomerVendor_ID =1798964468835815424




--检测出产品中设置的默认配方不属于他自己的配方
SELECT TOP
	  1000 * FROM	[View_ProdDetail] [it] 
WHERE	 BOM_ID IS NOT NULL
	 AND ProdDetailID NOT IN (SELECT ProdDetailID FROM tb_BOM_S WHERE BOM_ID=IT.BOM_ID )



	 ---连接池相关
	 SELECT COUNT(*) as '当前连接数' FROM sys.dm_exec_sessions;---获取当前目前系统连接数明细汇总

select   hostname 主机名,count(*) 连接数量  from   master.dbo.sysprocesses group by hostname order by count(*) desc  ---获取当前目前系统连接数明细

SELECT @@MAX_CONNECTIONS ---获取默认系统连接数

--查看当前数据库连接数详情信息？
sp_who2

exec sp_who 'sa';

-- exec sp_who2 'sa';
-- exec sp_who @loginame='sa';


--------找出BOM表中的母件的SKU和ID 可能不对应的情况。
SELECT SKU from  tb_BOM_S GROUP BY SKU HAVING COUNT(SKU)>1   --如果有行数则不对。之前是BOM母件的SKU和产品ID没有对应上。



--------权限中的字段数据添加时重复了。要删除重复的。优化保留启用可用修改过的数据
WITH CTE AS (
    SELECT
        MenuID,
        EntityName,
        FieldName,
        FieldText,
        ClassPath,
        IsForm,
        IsEnabled,
        IsChild,
        ChildEntityName,
        ROW_NUMBER() OVER (PARTITION BY MenuID, EntityName, FieldName, FieldText, ClassPath, IsForm, IsEnabled, IsChild, ChildEntityName ORDER BY (SELECT NULL)) AS rn
    FROM
        tb_FieldInfo
    WHERE
        MenuID = 1740601443896922112 
        AND IsChild = 1
)
DELETE FROM CTE WHERE rn > 1;