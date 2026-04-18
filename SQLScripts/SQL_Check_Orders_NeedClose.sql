-- =============================================
-- 检测SQL：找出满足结案条件但订单状态未更新为8（结案）的情况
-- 执行时间：2026-04-18
-- 结案条件：订单所有明细都已出库完成
--   条件1：订单主表 TotalQty = 订单明细 TotalDeliveredQty 之和
--   条件2：订单明细 Quantity 之和 = 订单明细 TotalDeliveredQty 之和
-- =============================================

-- 1. 检测哪些订单应该结案但实际未结案（基于主表TotalQty）
SELECT 
    so.SOrder_ID AS 订单ID,
    so.SOrderNo AS 订单编号,
    so.DataStatus AS 当前订单状态,
    so.TotalQty AS 订单主表总数量,
    ISNULL(sod_sum.明细已交付总数, 0) AS 明细已交付总数,
    CASE 
        WHEN so.TotalQty = ISNULL(sod_sum.明细已交付总数, 0) 
        THEN '满足结案条件' 
        ELSE '不满足结案条件' 
    END AS 结案判断
FROM tb_SaleOrder so
LEFT JOIN (
    SELECT 
        SOrder_ID,
        SUM(TotalDeliveredQty) AS 明细已交付总数
    FROM tb_SaleOrderDetail
    GROUP BY SOrder_ID
) sod_sum ON so.SOrder_ID = sod_sum.SOrder_ID
WHERE so.isdeleted = 0
  AND so.DataStatus < 8
  AND so.TotalQty = ISNULL(sod_sum.明细已交付总数, 0)
ORDER BY so.SOrder_ID;


-- 2. 检测哪些订单应该结案但实际未结案（基于明细汇总）
SELECT 
    so.SOrder_ID AS 订单ID,
    so.SOrderNo AS 订单编号,
    so.DataStatus AS 当前订单状态,
    ISNULL(sod_sum.明细订单总数, 0) AS 明细订单数量之和,
    ISNULL(sod_sum.明细已交付总数, 0) AS 明细已交付数量之和,
    CASE 
        WHEN ISNULL(sod_sum.明细订单总数, 0) = ISNULL(sod_sum.明细已交付总数, 0) 
        THEN '满足结案条件' 
        ELSE '不满足结案条件' 
    END AS 结案判断
FROM tb_SaleOrder so
LEFT JOIN (
    SELECT 
        SOrder_ID,
        SUM(Quantity) AS 明细订单总数,
        SUM(TotalDeliveredQty) AS 明细已交付总数
    FROM tb_SaleOrderDetail
    GROUP BY SOrder_ID
) sod_sum ON so.SOrder_ID = sod_sum.SOrder_ID
WHERE so.isdeleted = 0
  AND so.DataStatus <8
  AND ISNULL(sod_sum.明细订单总数, 0) = ISNULL(sod_sum.明细已交付总数, 0)
ORDER BY so.SOrder_ID;


-- 3. 综合检测：结合上述两种条件，找出所有应结案而未结案的订单
SELECT 
    so.SOrder_ID AS 订单ID,
    so.SOrderNo AS 订单编号,
    so.DataStatus AS 当前订单状态,
    so.TotalQty AS 主表总数量,
    ISNULL(sod_sum.明细订单总数, 0) AS 明细订单数量之和,
    ISNULL(sod_sum.明细已交付总数, 0) AS 明细已交付数量之和,
    CASE 
        WHEN so.TotalQty = ISNULL(sod_sum.明细已交付总数, 0) 
            AND ISNULL(sod_sum.明细订单总数, 0) = ISNULL(sod_sum.明细已交付总数, 0)
        THEN '满足结案条件'
        ELSE '不满足结案条件'
    END AS 结案判断
FROM tb_SaleOrder so
LEFT JOIN (
    SELECT 
        SOrder_ID,
        SUM(Quantity) AS 明细订单总数,
        SUM(TotalDeliveredQty) AS 明细已交付总数
    FROM tb_SaleOrderDetail
    GROUP BY SOrder_ID
) sod_sum ON so.SOrder_ID = sod_sum.SOrder_ID
WHERE so.isdeleted = 0
  AND so.DataStatus < 8
  AND so.TotalQty = ISNULL(sod_sum.明细已交付总数, 0)
  AND ISNULL(sod_sum.明细订单总数, 0) = ISNULL(sod_sum.明细已交付总数, 0)
ORDER BY so.SOrder_ID;


-- 4. 检测2026年4月17-18日已审核出库单的数量累加问题
--    仅检测：出库明细总量 = 订单总数量 但 TotalDeliveredQty 未正确累加的情况
SELECT 
    so.SOrderNo AS 订单编号,
    so.DataStatus AS 订单状态,
    sod.SaleOrderDetail_ID AS 订单明细ID,
    sod.ProdDetailID AS 货品ID,
    sod.Quantity AS 订单明细数量,
    sod.TotalDeliveredQty AS 订单明细已交付,
    ISNULL(sodetail.出库实际数量, 0) AS 出库明细实际数量,
    sod_sum.出库总数量 AS 该订单出库总数量,
    so.TotalQty AS 订单总数量,
    CASE 
        WHEN sod.TotalDeliveredQty = ISNULL(sodetail.出库实际数量, 0) 
        THEN '已正确累加' 
        ELSE '未正确累加' 
    END AS 累加状态
FROM tb_SaleOrder so
INNER JOIN tb_SaleOrderDetail sod ON so.SOrder_ID = sod.SOrder_ID
INNER JOIN tb_SaleOut sout ON so.SOrder_ID = sout.SOrder_ID AND sout.isdeleted = 0
LEFT JOIN (
    SELECT 
        SaleOut_MainID,
        SaleOrderDetail_ID,
        SUM(Quantity) AS 出库实际数量
    FROM tb_SaleOutDetail
    WHERE SaleOrderDetail_ID IS NOT NULL
    GROUP BY SaleOut_MainID, SaleOrderDetail_ID
) sodetail ON sout.SaleOut_MainID = sodetail.SaleOut_MainID 
    AND sod.SaleOrderDetail_ID = sodetail.SaleOrderDetail_ID
LEFT JOIN (
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
  AND sod_sum.出库总数量 = so.TotalQty  -- 出库总数量 = 订单总数量
  AND sod.TotalDeliveredQty <> ISNULL(sodetail.出库实际数量, 0)  -- 但明细未正确累加
ORDER BY so.SOrderNo, sod.SaleOrderDetail_ID;
