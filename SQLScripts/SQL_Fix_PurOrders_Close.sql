-- =============================================
-- 修复SQL：针对2026年4月17-18日已审核采购入库单的修复
-- 目标：更新采购订单明细的已入库数量、未交数量以及订单的DataStatus=8
-- 逻辑：
--   1. 从入库明细(已审核)根据 PurOrder_ChildID 计算实际入库数量
--   2. 更新订单明细的 DeliveredQuantity 和 UndeliveredQty
--   3. 汇总订单明细更新主表的 TotalUndeliveredQty
--   4. 判断并更新完全入库的订单状态为8（结案）
-- 关联逻辑：入库明细通过 PurOrder_ChildID 直接关联订单明细
-- =============================================

-- 步骤1：更新订单明细的 DeliveredQuantity 和 UndeliveredQty
--       根据已审核的采购入库单明细，按 PurOrder_ChildID 计算实际入库数量
--       仅针对2026年4月17-18日的入库数据
UPDATE pod
SET pod.DeliveredQuantity = ISNULL(pentry_sum.入库实际数量, 0),
    pod.UndeliveredQty = pod.Quantity - ISNULL(pentry_sum.入库实际数量, 0)
FROM tb_PurOrderDetail pod
INNER JOIN tb_PurOrder po ON pod.PurOrder_ID = po.PurOrder_ID
INNER JOIN tb_PurEntry pentry_main ON po.PurOrder_ID = pentry_main.PurOrder_ID AND pentry_main.isdeleted = 0
INNER JOIN (
    SELECT 
        ped.PurOrder_ChildID,
        SUM(ped.Quantity) AS 入库实际数量
    FROM tb_PurEntryDetail ped
    INNER JOIN tb_PurEntry pe ON ped.PurEntryID = pe.PurEntryID
    WHERE ped.PurOrder_ChildID IS NOT NULL
      AND pe.isdeleted = 0
      AND pe.DataStatus = 4
    GROUP BY ped.PurOrder_ChildID
) pentry_sum ON pod.PurOrder_ChildID = pentry_sum.PurOrder_ChildID
WHERE po.isdeleted = 0
  AND po.DataStatus = 4
  AND pentry_main.DataStatus = 4
  AND CONVERT(DATE, ISNULL(pentry_main.EntryDate, '1900-01-01')) BETWEEN '2026-04-17' AND '2026-04-18';


-- 步骤2：更新采购订单主表的 TotalUndeliveredQty（汇总所有明细的未入库数量）
--       TotalUndeliveredQty = 明细 UndeliveredQty 之和
UPDATE po
SET po.TotalUndeliveredQty = ISNULL((
    SELECT SUM(pod2.UndeliveredQty) 
    FROM tb_PurOrderDetail pod2
    WHERE pod2.PurOrder_ID = po.PurOrder_ID
), 0)
FROM tb_PurOrder po
WHERE po.isdeleted = 0
  AND EXISTS (
      SELECT 1 FROM tb_PurEntry pentry
      WHERE pentry.PurOrder_ID = po.PurOrder_ID
        AND pentry.isdeleted = 0
        AND pentry.DataStatus = 4
        AND CONVERT(DATE, ISNULL(pentry.EntryDate, '1900-01-01')) BETWEEN '2026-04-17' AND '2026-04-18'
  );


-- 步骤3：将满足结案条件的订单状态更新为8（结案）
--       结案条件：TotalUndeliveredQty = 0 且 DataStatus = 4（已审核）
--       仅针对2026年4月17-18日有入库记录的订单
UPDATE po
SET po.DataStatus = 8
FROM tb_PurOrder po
WHERE po.isdeleted = 0
  AND po.DataStatus = 4
  AND po.TotalUndeliveredQty = 0
  AND EXISTS (
      SELECT 1 FROM tb_PurEntry pentry
      WHERE pentry.PurOrder_ID = po.PurOrder_ID
        AND pentry.isdeleted = 0
        AND pentry.DataStatus = 4
        AND CONVERT(DATE, ISNULL(pentry.EntryDate, '1900-01-01')) BETWEEN '2026-04-17' AND '2026-04-18'
  );


-- 步骤4：验证更新结果
--       查看已更新的订单状态
SELECT 
    po.PurOrder_ID,
    po.PurOrderNo,
    po.DataStatus AS 更新后状态,
    po.TotalQty AS 订单总数量,
    po.TotalUndeliveredQty AS 未入库数量,
    CASE 
        WHEN po.TotalUndeliveredQty = 0 
        THEN '结案成功' 
        ELSE '数据异常' 
    END AS 验证结果
FROM tb_PurOrder po
WHERE po.DataStatus = 8
  AND po.isdeleted = 0
  AND EXISTS (
      SELECT 1 FROM tb_PurEntry pentry
      WHERE pentry.PurOrder_ID = po.PurOrder_ID
        AND pentry.isdeleted = 0
        AND pentry.DataStatus = 4
        AND CONVERT(DATE, ISNULL(pentry.EntryDate, '1900-01-01')) BETWEEN '2026-04-17' AND '2026-04-18'
  )
ORDER BY po.PurOrder_ID DESC;


-- 步骤5：验证明细更新结果（可选）
--       抽查部分明细的更新情况
SELECT TOP 20
    po.PurOrderNo AS 订单编号,
    pod.PurOrder_ChildID AS 明细ID,
    pod.Quantity AS 订单数量,
    pod.DeliveredQuantity AS 已入库,
    pod.UndeliveredQty AS 未入库,
    CASE 
        WHEN pod.Quantity = pod.DeliveredQuantity + pod.UndeliveredQty
        THEN '数据正确'
        ELSE '数据异常'
    END AS 验证
FROM tb_PurOrderDetail pod
INNER JOIN tb_PurOrder po ON pod.PurOrder_ID = po.PurOrder_ID
WHERE po.isdeleted = 0
  AND EXISTS (
      SELECT 1 FROM tb_PurEntry pentry
      WHERE pentry.PurOrder_ID = po.PurOrder_ID
        AND pentry.isdeleted = 0
        AND pentry.DataStatus = 4
        AND CONVERT(DATE, ISNULL(pentry.EntryDate, '1900-01-01')) BETWEEN '2026-04-17' AND '2026-04-18'
  )
ORDER BY po.PurOrderNo;
