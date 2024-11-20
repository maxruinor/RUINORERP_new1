--参考SHCMS  纵向  横向 库存跟踪
SELECT Location_ID,Quantity from  tb_Inventory  a WHERE a.Location_ID=1740618960329641984 and a.ProdDetailID=1742078588850671616
GO

-- SELECT   * from tb_Inventory a LEFT JOIN tb_OpeningInventory b on a.Inventory_ID=b.Inventory_ID WHERE a.Location_ID=1740618960329641984  and ProdDetailID=1742078588850671616


--还要加入 出入标记,注意盘点要排查期初盘点
---添加新的业务后。实例也要手动添加一下。

drop proc Proc_InventoryTracking_Horizontal
go
create proc Proc_InventoryTracking_Horizontal
@Location_ID as varchar(50),
@ProdDetailID as varchar(50)
as 
begin
 
 SELECT  库位, ProdDetailID, SUM(期初) as 期初,  SUM(采购入库) AS 采购入库 , SUM(采购退回) as 采购退回,SUM(销售出库) as 销售出库 , SUM(销售退回) as 销售退回, SUM(其他出库) as 其他出库, SUM(其他入库) as 其他入库 , SUM(库存盘点) as 库存盘点,sum(缴库) as 缴库 , SUM(期末库存) as 期末库存 from (
SELECT  Location_ID as 库位, ProdDetailID, SUM(InitQty) as 期初,  0 AS 采购入库, 0 as 采购退回,0 as 销售出库 , 0 as 销售退回, 0 as 其他出库, 0 as 其他入库 , 0 as 库存盘点 ,0 as 缴库, 0 as 期末库存 from tb_Inventory a LEFT JOIN tb_OpeningInventory b on a.Inventory_ID=b.Inventory_ID  WHERE Location_ID=@Location_ID  and ProdDetailID=@ProdDetailID GROUP BY Location_ID,ProdDetailID
union ALL
SELECT pc.Location_ID as 库位, ProdDetailID ,0 as 期初, sum(Quantity) as 采购入库, 0 as 采购退回,0 as 销售出库 , 0 as 销售退回, 0 as 其他出库, 0 as 其他入库 , 0 as 库存盘点 ,0 as 缴库, 0 as 期末库存 from  tb_PurEntry pm LEFT JOIN tb_PurEntryDetail pc on pm.PurEntryID=pc.PurEntryID  WHERE pc.Location_ID=@Location_ID  and pc.ProdDetailID=@ProdDetailID GROUP BY Location_ID,ProdDetailID
union ALL
SELECT pc.Location_ID as 库位, ProdDetailID, 0 as 期初,  0 AS 采购入库,   -SUM(Quantity) as 采购退回 ,0 as 销售出库 , 0 as 销售退回, 0 as 其他出库, 0 as 其他入库 , 0 as 库存盘点 ,0 as 缴库, 0 as 期末库存 from  tb_PurEntryRe pm LEFT JOIN tb_PurEntryReDetail pc on pm.PurEntryRe_ID=pc.PurEntryRe_ID   WHERE pc.Location_ID=@Location_ID  and pc.ProdDetailID=@ProdDetailID GROUP BY Location_ID,ProdDetailID
union ALL
SELECT    sc.Location_ID as 库位, ProdDetailID, 0 as 期初,  0 AS 采购入库, 0 as 采购退回,-SUM(Quantity) as 销售出库 ,0 as 销售退回, 0 as 其他出库, 0 as 其他入库 , 0 as 库存盘点,0 as 缴库 , 0 as 期末库存 from  tb_SaleOut sm LEFT JOIN tb_SaleOutDetail sc on sm.SaleOut_MainID=sc.SaleOut_MainID WHERE sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID GROUP BY Location_ID,ProdDetailID 
union ALL
SELECT sc.Location_ID as 库位, ProdDetailID,0 as 期初,  0 AS 采购入库, 0 as 采购退回,0 as 销售出库 ,  sum(Quantity) as 销售退回 , 0 as 其他出库, 0 as 其他入库 , 0 as 库存盘点 ,0 as 缴库, 0 as 期末库存  from  tb_SaleOutRe sm LEFT JOIN tb_SaleOutReDetail sc on sm.SaleOutRe_ID=sc.SaleOutRe_ID  WHERE sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID GROUP BY Location_ID,ProdDetailID
union ALL
SELECT  sc.Location_ID as 库位, ProdDetailID,0 as 期初,  0 AS 采购入库, 0 as 采购退回,0 as 销售出库 ,  0 as 销售退回 , -SUM(Qty) as 其他出库, 0 as 其他入库 , 0 as 库存盘点 ,0 as 缴库, 0 as 期末库存 from  tb_StockOut sm LEFT JOIN tb_StockOutDetail sc on sm.MainID=sc.MainID  WHERE sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID GROUP BY Location_ID,ProdDetailID
union ALL
SELECT sc.Location_ID as 库位, ProdDetailID,0 as 期初,  0 AS 采购入库, 0 as 采购退回,0 as 销售出库 ,  0 as 销售退回 , 0 as 其他出库, SUM(Qty) as 其他入库 , 0 as 库存盘点 ,0 as 缴库, 0 as 期末库存  from  tb_StockIN sm LEFT JOIN tb_StockINDetail sc on sm.MainID=sc.MainID  WHERE sc.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID GROUP BY Location_ID,ProdDetailID
union ALL
SELECT sm.Location_ID as 库位, ProdDetailID,0 as 期初,  0 AS 采购入库, 0 as 采购退回,0 as 销售出库 ,  0 as 销售退回 , 0 as 其他出库, 0 as 其他入库 , sum(DiffQty) as 库存盘点,0 as 缴库 , 0 as 期末库存 from tb_Stocktake sm LEFT JOIN tb_StocktakeDetail sc on sm.MainID=sc.MainID  WHERE sm.CheckMode!=2 and sm.Location_ID=@Location_ID  and sc.ProdDetailID=@ProdDetailID GROUP BY Location_ID,ProdDetailID
union ALL
SELECT  Location_ID as 库位, ProdDetailID,0 as 期初,  0 AS 采购入库, 0 as 采购退回,0 as 销售出库 ,  0 as 销售退回 , 0 as 其他出库, 0 as 其他入库 ,  0 as 库存盘点 ,0 as 缴库,0 as 期末库存 from tb_Inventory a LEFT JOIN tb_OpeningInventory b on a.Inventory_ID=b.Inventory_ID  WHERE a.Location_ID=@Location_ID  and ProdDetailID=@ProdDetailID  GROUP BY Location_ID,ProdDetailID
union ALL
SELECT  Location_ID as 库位, ProdDetailID,0 as 期初,  0 AS 采购入库, 0 as 采购退回,0 as 销售出库 ,  0 as 销售退回 , 0 as 其他出库, 0 as 其他入库 ,  0 as 库存盘点,sum(Qty) as 缴库 , 0 as 期末库存 from tb_FinishedGoodsInv a LEFT JOIN tb_FinishedGoodsInvDetail b on a.FG_ID=b.FG_ID  WHERE b.Location_ID=@Location_ID  and ProdDetailID=@ProdDetailID  GROUP BY Location_ID,ProdDetailID
) AS HH GROUP BY 库位,ProdDetailID

end

go

exec Proc_InventoryTracking_Horizontal   '1740618960329641984','1742078588850671616'


--exec Proc_InventoryTracking '2008-4-5','2008-9-01','001','产品编码'