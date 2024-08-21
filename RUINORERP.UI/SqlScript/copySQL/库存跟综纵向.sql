USE [erpnew]
GO

drop proc Proc_InventoryTracking
go
/****** Object:  StoredProcedure [dbo].[Proc_InventoryTracking]    Script Date: 04/24/2024 19:20:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Proc_InventoryTracking]
@Location_ID as varchar(50),
@ProdDetailID as varchar(50)
as 
begin
SELECT 经营历程, ProdDetailID,ProductNo,SKU ,CNName,Specifications,prop,业务类型,单据编号,库位 AS Location_ID,数量,日期 from (
SELECT '进出明细' as 经营历程,  vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'期初库存' as 业务类型 ,'' as 单据编号, a.Location_ID as 库位,InitQty as 数量,InitInvDate as 日期 from tb_Inventory a LEFT JOIN tb_OpeningInventory b on a.Inventory_ID=b.Inventory_ID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=a.ProdDetailID and vp.Location_ID=a.Location_ID
 WHERE a.Location_ID=@Location_ID  and a.ProdDetailID=@ProdDetailID
union ALL
SELECT '进出明细' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'采购入库' as 业务类型 ,PurEntryNo as 单据编号, pc.Location_ID as 库位,  pc.Quantity as 数量,EntryDate as 日期 from  tb_PurEntry pm LEFT JOIN tb_PurEntryDetail pc on pm.PurEntryID=pc.PurEntryID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=pc.ProdDetailID and vp.Location_ID=pc.Location_ID
WHERE pc.Location_ID=@Location_ID  and pc.ProdDetailID=@ProdDetailID
union ALL
SELECT '进出明细' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'采购退回' as 业务类型,PurEntryNo as 单据编号 , pc.Location_ID as 库位,  -pc.Quantity as 数量 ,ReturnDate as 日期 from  tb_PurEntryRe pm LEFT JOIN tb_PurEntryReDetail pc on pm.PurEntryRe_ID=pc.PurEntryRe_ID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=pc.ProdDetailID and vp.Location_ID=pc.Location_ID 
WHERE pc.Location_ID=@Location_ID  and pc.ProdDetailID=@ProdDetailID
union ALL
SELECT '进出明细' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'销售出库' as 业务类型 ,SaleOutNo as 单据编号, sc.Location_ID as 库位,  -sc.Quantity as 数量 ,OutDate as 日期 from  tb_SaleOut sm LEFT JOIN tb_SaleOutDetail sc on sm.SaleOut_MainID=sc.SaleOut_MainID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=sc.ProdDetailID and vp.Location_ID=sc.Location_ID
 WHERE sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID
union ALL
SELECT '进出明细' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'销售退回' as 业务类型 ,ReturnNo as 单据编号, sc.Location_ID as 库位,  sc.Quantity as 数量 ,ReturnDate as 日期 from  tb_SaleOutRe sm LEFT JOIN tb_SaleOutReDetail sc on sm.SaleOutRe_ID=sc.SaleOutRe_ID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=sc.ProdDetailID and vp.Location_ID=sc.Location_ID 
WHERE sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID
union ALL
SELECT '进出明细' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'退货翻新领用' as 业务类型,ReturnNo as 单据编号 , sc.Location_ID as 库位,  -sc.Quantity as 数量 ,ReturnDate as 日期 from tb_SaleOutRe sm LEFT JOIN tb_SaleOutReRefurbishedMaterialsDetail sc on sm.SaleOutRe_ID=sc.SaleOutRe_ID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=sc.ProdDetailID and vp.Location_ID=sc.Location_ID 
WHERE sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID
union ALL
SELECT '进出明细' as 经营历程,vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'其它出库' as 业务类型,BillNo as 单据编号 , sc.Location_ID as 库位,  -Qty as 数量 ,Out_date as 日期 from  tb_StockOut sm LEFT JOIN tb_StockOutDetail sc on sm.MainID=sc.MainID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=sc.ProdDetailID and vp.Location_ID=sc.Location_ID 
WHERE sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID
union ALL
SELECT '进出明细' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'其它入库' as 业务类型, BillNo as 单据编号 , sc.Location_ID as 库位,  Qty as 数量 ,Enter_Date as 日期 from  tb_StockIN sm LEFT JOIN tb_StockINDetail sc on sm.MainID=sc.MainID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=sc.ProdDetailID and vp.Location_ID=sc.Location_ID
 WHERE sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID
union ALL
SELECT '进出明细' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'库存盘点' as 业务类型,CheckNo as 单据编号 , sm.Location_ID as 库位,  DiffQty as 数量 ,Check_date as 日期 from tb_Stocktake sm LEFT JOIN tb_StocktakeDetail sc on sm.MainID=sc.MainID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=sc.ProdDetailID and vp.Location_ID=sm.Location_ID 
WHERE  sm.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID
union ALL
SELECT '进出明细' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'领料单' as 业务类型,MaterialRequisitionNO as 单据编号 , sc.Location_ID as 库位,  -ActualSentQty as 数量 ,DeliveryDate as 日期 from tb_MaterialRequisition sm LEFT JOIN tb_MaterialRequisitionDetail sc on sm.MR_ID=sc.MR_ID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=sc.ProdDetailID and vp.Location_ID=sc.Location_ID 
WHERE (sm.DataStatus=4 or sm.DataStatus=8)  and sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID
union ALL
SELECT '进出明细' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'退料单' as 业务类型,BillNo as 单据编号 , sc.Location_ID as 库位,  sc.Quantity as 数量 ,ReturnDate as 日期 from tb_MaterialReturn sm LEFT JOIN tb_MaterialReturnDetail sc on sm.MRE_ID=sc.MRE_ID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=sc.ProdDetailID and vp.Location_ID=sc.Location_ID 
union ALL
SELECT '进出明细' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'缴库' as 业务类型,DeliveryBillNo as 单据编号 , sc.Location_ID as 库位,  Qty as 数量 ,DeliveryDate as 日期 from tb_FinishedGoodsInv sm LEFT JOIN tb_FinishedGoodsInvDetail sc on sm.FG_ID=sc.FG_ID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=sc.ProdDetailID and vp.Location_ID=sc.Location_ID 
WHERE (sm.DataStatus=4 or sm.DataStatus=8)  and sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID
union ALL
SELECT '最后结余' as 经营历程, vp.ProdDetailID,vp.ProductNo,vp.SKU,vp.CNName,vp.Specifications,vp.prop,'期末库存' as 业务类型,'' as 单据编号 , a.Location_ID as 库位,  a.Quantity as 数量 ,GETDATE() as 日期 from tb_Inventory a LEFT JOIN tb_OpeningInventory b on a.Inventory_ID=b.Inventory_ID INNER JOIN View_ProdDetail vp on vp.ProdDetailID=a.ProdDetailID and vp.Location_ID=a.Location_ID 
WHERE a.Location_ID=@Location_ID  and a.ProdDetailID=@ProdDetailID) as AA
end