-- =============================================
-- 检测SQL：找出采购订单满足结案条件但订单状态未更新为8（结案）的情况
-- 执行时间：2026-04-18
-- 结案条件：采购订单所有明细都已入库完成
--   条件1：订单主表 TotalQty = 订单明细 DeliveredQuantity 之和
--   条件2：订单明细 Quantity 之和 = 订单明细 DeliveredQuantity 之和
--   条件3：订单主表 TotalUndeliveredQty = 0
-- 关联逻辑：入库明细根据 ProdDetailID + Location_ID 计算已入库数量
-- =============================================

-- 1. 检测哪些采购订单应该结案但实际未结案（基于主表TotalQty）
SELECT 
    po.PurOrder_ID AS 订单ID,
    po.PurOrderNo AS 订单编号,
    po.DataStatus AS 当前订单状态,
    po.TotalQty AS 订单主表总数量,
    po.TotalUndeliveredQty AS 主表未交数量,
    ISNULL(pod_sum.明细已入库总数, 0) AS 明细已入库总数,
    CASE 
        WHEN po.TotalQty = ISNULL(pod_sum.明细已入库总数, 0) 
        THEN '满足结案条件' 
        ELSE '不满足结案条件' 
    END AS 结案判断
FROM tb_PurOrder po
LEFT JOIN (
    SELECT 
        PurOrder_ID,
        SUM(DeliveredQuantity) AS 明细已入库总数
    FROM tb_PurOrderDetail
    GROUP BY PurOrder_ID
) pod_sum ON po.PurOrder_ID = pod_sum.PurOrder_ID
WHERE po.isdeleted = 0
  AND po.DataStatus < 8
  AND po.TotalQty = ISNULL(pod_sum.明细已入库总数, 0)
ORDER BY po.PurOrder_ID;


-- 2. 检测哪些采购订单应该结案但实际未结案（基于明细汇总）
SELECT 
    po.PurOrder_ID AS 订单ID,
    po.PurOrderNo AS 订单编号,
    po.DataStatus AS 当前订单状态,
    ISNULL(pod_sum.明细订单总数, 0) AS 明细订单数量之和,
    ISNULL(pod_sum.明细已入库总数, 0) AS 明细已入库数量之和,
    CASE 
        WHEN ISNULL(pod_sum.明细订单总数, 0) = ISNULL(pod_sum.明细已入库总数, 0) 
        THEN '满足结案条件' 
        ELSE '不满足结案条件' 
    END AS 结案判断
FROM tb_PurOrder po
LEFT JOIN (
    SELECT 
        PurOrder_ID,
        SUM(Quantity) AS 明细订单总数,
        SUM(DeliveredQuantity) AS 明细已入库总数
    FROM tb_PurOrderDetail
    GROUP BY PurOrder_ID
) pod_sum ON po.PurOrder_ID = pod_sum.PurOrder_ID
WHERE po.isdeleted = 0
  AND po.DataStatus < 8
  AND ISNULL(pod_sum.明细订单总数, 0) = ISNULL(pod_sum.明细已入库总数, 0)
ORDER BY po.PurOrder_ID;


-- 3. 综合检测：结合上述两种条件，找出所有应结案而未结案的订单
SELECT 
    po.PurOrder_ID AS 订单ID,
    po.PurOrderNo AS 订单编号,
    po.DataStatus AS 当前订单状态,
    po.TotalQty AS 主表总数量,
    po.TotalUndeliveredQty AS 主表未交数量,
    ISNULL(pod_sum.明细订单总数, 0) AS 明细订单数量之和,
    ISNULL(pod_sum.明细已入库总数, 0) AS 明细已入库数量之和,
    CASE 
        WHEN po.TotalQty = ISNULL(pod_sum.明细已入库总数, 0) 
            AND ISNULL(pod_sum.明细订单总数, 0) = ISNULL(pod_sum.明细已入库总数, 0)
            AND po.TotalUndeliveredQty = 0
        THEN '满足结案条件'
        ELSE '不满足结案条件'
    END AS 结案判断
FROM tb_PurOrder po
LEFT JOIN (
    SELECT 
        PurOrder_ID,
        SUM(Quantity) AS 明细订单总数,
        SUM(DeliveredQuantity) AS 明细已入库总数
    FROM tb_PurOrderDetail
    GROUP BY PurOrder_ID
) pod_sum ON po.PurOrder_ID = pod_sum.PurOrder_ID
WHERE po.isdeleted = 0
  AND po.DataStatus < 8
  AND po.TotalQty = ISNULL(pod_sum.明细已入库总数, 0)
  AND ISNULL(pod_sum.明细订单总数, 0) = ISNULL(pod_sum.明细已入库总数, 0)
ORDER BY po.PurOrder_ID;


-- 4. 检测2026年4月17-18日已审核采购入库单的数量累加问题
--    关联逻辑：根据 ProdDetailID + Location_ID 计算入库数量
SELECT 
    po.PurOrderNo AS 订单编号,
    po.DataStatus AS 订单状态,
    pod.PurOrder_ChildID AS 订单明细ID,
    pod.ProdDetailID AS 货品ID,
    pod.Location_ID AS 库位ID,
    pod.Quantity AS 订单明细数量,
    pod.DeliveredQuantity AS 订单明细已入库,
    pod.UndeliveredQty AS 订单明细未入库,
    ISNULL(pentry_sum.入库实际数量, 0) AS 入库明细实际数量,
    pod_sum.入库总数量 AS 该订单入库总数量,
    po.TotalQty AS 订单总数量,
    po.TotalUndeliveredQty AS 订单未交总数量,
    CASE 
        WHEN pod.DeliveredQuantity = ISNULL(pentry_sum.入库实际数量, 0) 
        THEN '已正确累加' 
        ELSE '未正确累加' 
    END AS 累加状态
FROM tb_PurOrder po
INNER JOIN tb_PurOrderDetail pod ON po.PurOrder_ID = pod.PurOrder_ID
INNER JOIN tb_PurEntry pentry_main ON po.PurOrder_ID = pentry_main.PurOrder_ID AND pentry_main.isdeleted = 0
LEFT JOIN (
    SELECT 
        pe.PurOrder_ID,
        ped.ProdDetailID,
        ped.Location_ID,
        SUM(ped.Quantity) AS 入库实际数量
    FROM tb_PurEntryDetail ped
    INNER JOIN tb_PurEntry pe ON ped.PurEntryID = pe.PurEntryID
    WHERE pe.isdeleted = 0
      AND pe.DataStatus = 4
    GROUP BY pe.PurOrder_ID, ped.ProdDetailID, ped.Location_ID
) pentry_sum ON po.PurOrder_ID = pentry_sum.PurOrder_ID 
    AND pod.ProdDetailID = pentry_sum.ProdDetailID 
    AND pod.Location_ID = pentry_sum.Location_ID
LEFT JOIN (
    SELECT 
        pentry2.PurOrder_ID,
        SUM(pod2.Quantity) AS 入库总数量
    FROM tb_PurEntry pentry2
    INNER JOIN tb_PurEntryDetail pod2 ON pentry2.PurEntryID = pod2.PurEntryID
    WHERE pentry2.isdeleted = 0
      AND pentry2.DataStatus = 4
    GROUP BY pentry2.PurOrder_ID
) pod_sum ON po.PurOrder_ID = pod_sum.PurOrder_ID
WHERE po.isdeleted = 0
  AND po.DataStatus = 4
  AND pentry_main.DataStatus = 4
  AND CONVERT(DATE, ISNULL(pentry_main.EntryDate, '1900-01-01')) BETWEEN '2026-04-17' AND '2026-04-18'
  AND pod_sum.入库总数量 = po.TotalQty
ORDER BY po.PurOrderNo, pod.PurOrder_ChildID;
