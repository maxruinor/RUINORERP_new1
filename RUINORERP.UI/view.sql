create VIEW View_Inventory
as 
SELECT
  a.Inventory_ID,
	b.ProductNo,
	b.SKU,
	b.CNName,
	b.prop,
	b.Specifications,
	b.Model,
	a.Quantity,
	b.Type_ID,
	b.Unit_ID,
	b.Category_ID,
	b.CustomerVendor_ID,
	b.DepartmentID,
	b.SourceType,
	b.Brand,
	b.Rack_ID,
	a.Alert_Quantity,
    a.On_the_way_Qty,
	a.Sale_Qty,
	a.MakingQty,
	a.NotOutQty,
	a.Inv_Cost,
	a.ProdDetailID,
	a.Location_ID,
	b.BOM_ID,
    a.LatestStorageTime,
    a.LatestOutboundTime
FROM
	tb_Inventory a,
	View_ProdDetail b 
WHERE
	a.ProdDetailID= b.ProdDetailID


create VIEW View_ProdProperty
as 
SELECT
	R.ProdBaseID,
	R.ProdDetailID,
	P.Property_ID,
	P.PropertyName,
	PV.PropertyValueID,
	PV.PropertyValueName 
FROM
	tb_Prod_Attr_Relation R
	LEFT JOIN tb_ProdProperty P ON R.Property_ID= P.Property_ID
	LEFT JOIN tb_ProdPropertyValue PV ON R.PropertyValueID= PV.PropertyValueID



create VIEW View_ProdPropGoup
as 
SELECT DISTINCT
	D.ProdDetailID,
	STUFF(
		( SELECT ',' + PropertyValueName FROM View_ProdProperty PN WHERE PN.ProdDetailID= T.ProdDetailID FOR XML PATH ( '' ) ),
		1,
		1,
		'' 
	) AS prop,
	B.ProdBaseID 
FROM
	View_ProdProperty T
	INNER JOIN tb_Prod B ON T.ProdBaseID = B.ProdBaseID
	INNER JOIN tb_ProdDetail D ON t.ProdDetailID = d.ProdDetailID
		 Where B.Is_enabled=1





		 -----------------============
    DROP VIEW View_ProdDetail
	 GO
	 create VIEW View_ProdDetail
as 
SELECT
  D.ProdBaseID,
	D.SKU,
	D.ProdDetailID,
	B.CNName,
	B.Specifications,
	Y.Quantity,
	T.prop,
	B.ProductNo,
	B.Unit_ID,
	B.Model,
	B.Category_ID,
	B.CustomerVendor_ID,
	B.DepartmentID,
	B.ENName,
	B.Brand,
	B.Images,
	Y.Location_ID,
	B.Rack_ID,
	Y.On_the_way_Qty,
	Y.Sale_Qty,
	Y.Alert_Quantity,
	Y.MakingQty,
	Y.NotOutQty,
	D.Is_available,
	D.Is_enabled,
	B.Notes,
	B.Type_ID,
	B.SalePublish,
	B.ShortCode,
	B.SourceType,
	D.BarCode,
	Y.Inv_Cost,
	D.Standard_Price,
	D.Discount_Price,
	D.Market_Price,
	D.Wholesale_Price,
	D.Transfer_Price,
	D.Weight,
	D.BOM_ID 
FROM
	dbo.View_ProdPropGoup AS T
	INNER JOIN dbo.tb_Prod AS B ON T.ProdBaseID = B.ProdBaseID
	INNER JOIN dbo.tb_ProdDetail AS D ON T.ProdDetailID = D.ProdDetailID
	LEFT OUTER JOIN dbo.tb_Inventory AS Y ON Y.ProdDetailID = D.ProdDetailID 

--WHERE
--	( B.Is_enabled = 1 ) 
	--AND ( B.isdeleted = 0 )

	--as 的一些列名规则可以参数到其他 。整体全部修改一次。订单的日期，明细的日期，摘要在明细中。主表叫备注
create view View_PurOrderItems
as 
SELECT
	m.PurOrder_ID,
	m.PurOrderNo,
	m.CustomerVendor_ID,
	m.Employee_ID,
	m.DepartmentID,
	m.Paytype_ID,
	m.SOrder_ID,
	m.PDID,
	m.PurDate,
	m.PreDeliveryDate as OrderPreDeliveryDate,
	m.ShippingCost,
	m.TotalQty,
	m.TotalTaxAmount,
	m.Notes,
	m.Arrival_date,
	m.TotalAmount,
	m.IsIncludeTax,
	m.KeepAccountsType,
	m.PrePayMoney,
	m.Deposit,
	m.TaxDeductionType,
	m.DataStatus,
	m.ApprovalOpinions,
	m.ApprovalStatus,
	m.ApprovalResults,
	m.Approver_by,
	m.Approver_at,
	m.isdeleted,
	m.Created_at,
	m.Created_by,
	m.Modified_at,
	m.Modified_by,
	m.RefBillID,
	m.RefNO,
	m.RefBizType,
	m.PrintStatus,
	c.PurOrder_ChildID,
	c.ProdDetailID,
	c.Location_ID,
	c.Quantity,
	c.UnitPrice,
	c.Discount,
	c.TransactionPrice,
	c.TaxRate,
	c.TaxAmount,
	c.TotalAmount as SubtotalAmount,
	c.IsGift,
	c.PreDeliveryDate as ItemPreDeliveryDate,
	c.CustomertModel,
	c.DeliveredQuantity,
	c.IncludingTax,
	c.Notes as Summary,
	c.property 
FROM
	dbo.tb_PurOrder AS m
	RIGHT JOIN dbo.tb_PurOrderDetail AS c ON m.PurOrder_ID = c.PurOrder_ID


	====================以上是生成视图  视图生成时用PD文件 才有列的注释


	-- 添加或更新视图列的注释
EXEC sp_addextendedproperty 
@name = N'MS_Description',  -- 注释的名称，MS_Description 是用于列注释的标准名称
@value = N'这里是您的注释', -- 您想要为列添加的注释文本
@level0type = N'Schema', @level0name = 'dbo', -- Schema 名称，这里是 dbo
@level1type = N'View',  @level1name = 'YourViewName', -- 视图名称
@level2type = N'Column', @level2name = 'YourColumnName'; -- 列名称

EXEC sp_addextendedproperty 
@name = N'MS_Description',  -- 注释的名称，MS_Description 是用于列注释的标准名称
@value = N'订单编号', -- 您想要为列添加的注释文本
@level0type = N'Schema', @level0name = 'dbo', -- Schema 名称，这里是 dbo
@level1type = N'View',  @level1name = 'View_PurOrderItems', -- 视图名称
@level2type = N'Column', @level2name = 'PurOrderNo'; -- 列名称





SELECT  DISTINCT Prod_Detail_ID,
STUFF((SELECT ','+PropertyValueName FROM v_Property PN WHERE PN.Prod_Detail_ID=T.Prod_Detail_ID  FOR XML PATH('')), 1, 1, '') AS prop ,Prod_Base_ID,
FROM v_Property as T
--FROM tb_Prod_Attr_Relation AS T

--ok
SELECT  DISTINCT T.Prod_Detail_ID,
STUFF((SELECT ','+PropertyValueName FROM v_Property PN WHERE PN.Prod_Detail_ID=T.Prod_Detail_ID  FOR XML PATH('')), 1, 1, '') AS 属性
,B.CNName as 品名,B.ShortCode as 助记码,D.BarCode as 条码,B.Model as 型号,B.Specifications as 规格,B.Unit_ID as 单位,D.SKU
FROM v_Property AS T INNER  JOIN tb_Prod_Base B ON T.Prod_Base_ID=B.Prod_Base_ID 
INNER JOIN tb_Prod_Detail D on T.Prod_Detail_ID=D.Prod_Detail_ID


--SELECT DISTINCT Name, STUFF((SELECT ‘,’ + Course FROM Student WHERE Name = T.Name FOR XML PATH(’’) ), 1, 1, ‘’) AS Course FROM Student AS T


------------------

select
   B.Prod_Base_ID,
   D.Prod_Detail_ID,
   prop,
   B.CNName,
   B.Unit_ID,
   B.Model,
   B.Category_ID,
   B.CustomerVendor_ID,
   B.DepartmentID,
   B.Specifications,
   B.ENName,
   B.Brand,
   B.Location_ID,
   B.PropertyType_ID,
   B.Is_available,
   B.Is_enabled,
   B.Notes,
   B.ProductNo,
   B.SalePublish,
   B.ShortCode,
   B.SourceType,
   D.BarCode,
   D.Cost_price,
   D.Discount_price,
   D.Market_price,
   D.SKU,
   D.Sales_price,
   D.Transfer_price,
   D.Weight
from
   View_ProdPropGoup T
   INNER JOIN tb_Prod_Base B on  T.Prod_Base_ID = B.Prod_Base_ID
   INNER JOIN tb_Prod_Detail D on  T.Prod_Detail_ID = d.Prod_Detail_ID
go













---------------------------报表
drop view View_SaleOrderPerformance
go
create VIEW View_SaleOrderPerformance
as 
SELECT  m.SOrderNo as 订单号,sum(s.SubtotalTransactionAmount) as 成交金额,sum(s.TaxSubtotalAmount) as 税额,sum(s.UntaxedAmont) as 未税本位币
FROM tb_SaleOrder m,tb_SaleOrderDetail s WHERE m.SOrder_ID=s.SOrder_ID
GROUP BY SOrderNo

GO



----------
--按订单查业绩

SELECT m.Employee_ID,tb_Employee.Employee_Name,sum(m.ShipCost) as 运费,sum(s.SubtotalTransactionAmount) as 成交金额,sum(s.TaxSubtotalAmount) as 税额,sum(s.UntaxedAmont) as 未税本位币
FROM  tb_SaleOrder m INNER JOIN tb_SaleOrderDetail s on m.SOrder_ID=s.SOrder_ID
LEFT JOIN tb_Employee  on m.Employee_ID=tb_Employee.Employee_ID
where  m.Created_at<'2024-1-31'
--and m.TotalAmount!=0
GROUP BY m.Employee_ID,tb_Employee.Employee_Name
ORDER BY 未税本位币 desc


--查出多余的字段定义
SELECT * from tb_P4Field bb WHERE FieldInfo_ID in (
SELECT FieldInfo_ID from (SELECT MAX(FieldInfo_ID) as FieldInfo_ID, FieldName,ClassPath,MenuID  from tb_FieldInfo WHERE IsChild=1 GROUP BY FieldName,ClassPath,MenuID HAVING COUNT(*)>1) aa )