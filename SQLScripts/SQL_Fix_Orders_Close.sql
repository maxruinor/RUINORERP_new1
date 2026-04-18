-- =============================================
-- 修复SQL：针对2026年4月17-18日已审核出库单的修复
-- 目标：更新销售订单明细的出库数量以及订单的DataStatus=8
-- 条件：出库总数量 = 订单总数量，但明细未正确累加
-- =============================================

-- 步骤1：更新订单明细的 TotalDeliveredQty（从出库明细累加）
-- 仅针对满足条件的订单（查询4的数据）
UPDATE sod
SET sod.TotalDeliveredQty = ISNULL(sodetail.出库实际数量, 0)
FROM tb_SaleOrderDetail sod
INNER JOIN tb_SaleOrder so ON sod.SOrder_ID = so.SOrder_ID
INNER JOIN tb_SaleOut sout ON so.SOrder_ID = sout.SOrder_ID AND sout.isdeleted = 0
INNER JOIN (
    SELECT 
        SaleOut_MainID,
        SaleOrderDetail_ID,
        SUM(Quantity) AS 出库实际数量
    FROM tb_SaleOutDetail
    WHERE SaleOrderDetail_ID IS NOT NULL
    GROUP BY SaleOut_MainID, SaleOrderDetail_ID
) sodetail ON sout.SaleOut_MainID = sodetail.SaleOut_MainID 
    AND sod.SaleOrderDetail_ID = sodetail.SaleOrderDetail_ID
INNER JOIN (
    SELECT 
        sout.SOrder_ID,
        SUM(sod2.Quantity) AS 出库总数量
    FROM tb_SaleOut sout
    INNER JOIN tb_SaleOutDetail sod2 ON sout.SaleOut_MainID = sod2.SaleOut_MainID
    WHERE sout.isdeleted = 0
      AND sout.DataStatus = 4
    GROUP BY sout.SOrder_ID
) sod_sum ON so.SOrder_ID = sod_sum.SOrder_ID
WHERE so.isdeleted = 0
  AND so.DataStatus = 4
  AND sout.DataStatus = 4
  AND CONVERT(DATE, ISNULL(sout.OutDate, '1900-01-01')) BETWEEN '2026-04-17' AND '2026-04-18'
  AND sod_sum.出库总数量 = so.TotalQty;

-- 步骤2：更新订单主表的 TotalDeliveredQty（汇总所有明细的已交付数量） 销售订单主表没有TotalDeliveredQty字段
UPDATE so
SET so.TotalDeliveredQty = ISNULL((
    SELECT SUM(TotalDeliveredQty) 
    FROM tb_SaleOrderDetail 
    WHERE SOrder_ID = so.SOrder_ID
), 0)
FROM tb_SaleOrder so
WHERE so.isdeleted = 0
  AND EXISTS (
      SELECT 1 FROM tb_SaleOut sout
      WHERE sout.SOrder_ID = so.SOrder_ID
        AND sout.isdeleted = 0
        AND sout.DataStatus = 4
        AND CONVERT(DATE, ISNULL(sout.OutDate, '1900-01-01')) BETWEEN '2026-04-17' AND '2026-04-18'
  );

-- 步骤3：将满足结案条件的订单状态更新为8（结案）
-- 仅针对2026年4月17-18日完全出库的订单
UPDATE so
SET so.DataStatus = 8
FROM tb_SaleOrder so
INNER JOIN (
    SELECT 
        sout.SOrder_ID,
        SUM(sod2.Quantity) AS 出库总数量
    FROM tb_SaleOut sout
    INNER JOIN tb_SaleOutDetail sod2 ON sout.SaleOut_MainID = sod2.SaleOut_MainID
    WHERE sout.isdeleted = 0
      AND sout.DataStatus = 4
    GROUP BY sout.SOrder_ID
) sod_sum ON so.SOrder_ID = sod_sum.SOrder_ID
WHERE so.isdeleted = 0
  AND so.DataStatus = 4
  AND sod_sum.出库总数量 = so.TotalQty
  AND EXISTS (
      SELECT 1 FROM tb_SaleOut sout2
      WHERE sout2.SOrder_ID = so.SOrder_ID
        AND sout2.isdeleted = 0
        AND sout2.DataStatus = 4
        AND CONVERT(DATE, ISNULL(sout2.OutDate, '1900-01-01')) BETWEEN '2026-04-17' AND '2026-04-18'
  );

-- 步骤4：验证更新结果（可选）
SELECT 
    so.SOrder_ID,
    so.SOrderNo,
    so.DataStatus AS 更新后状态,
    so.TotalQty AS 订单总数量,
    so.TotalDeliveredQty AS 已交付数量,
    CASE 
        WHEN so.TotalQty = so.TotalDeliveredQty 
        THEN '结案成功' 
        ELSE '数据异常' 
    END AS 验证结果
FROM tb_SaleOrder so
WHERE so.DataStatus = 8
  AND so.isdeleted = 0
ORDER BY so.SOrder_ID DESC;
