-- ============================================
-- 库存快照功能测试SQL脚本
-- ============================================

-- 1. 查看当前库存快照数据
SELECT TOP 20 
    SnapshotID,
    ProdDetailID,
    Location_ID,
    Quantity,
    SnapshotTime,
    Notes
FROM tb_InventorySnapshot
ORDER BY SnapshotTime DESC;

-- 2. 统计每天的快照数量
SELECT 
    CAST(SnapshotTime AS DATE) as SnapshotDate,
    COUNT(*) as SnapshotCount
FROM tb_InventorySnapshot
GROUP BY CAST(SnapshotTime AS DATE)
ORDER BY SnapshotDate DESC;

-- 3. 查看特定商品的快照历史
DECLARE @ProdDetailID BIGINT = 1234567890; -- 替换为实际的商品ID

SELECT 
    SnapshotTime,
    Quantity,
    CostFIFO,
    CostMovingWA,
    Notes
FROM tb_InventorySnapshot
WHERE ProdDetailID = @ProdDetailID
ORDER BY SnapshotTime DESC;

-- 4. 查看过期快照（超过12个月）
SELECT COUNT(*) as ExpiredCount
FROM tb_InventorySnapshot
WHERE SnapshotTime < DATEADD(MONTH, -12, GETDATE());

-- 5. 手动清理过期快照（测试用）
-- 注意：生产环境请使用工作流自动清理
/*
DELETE FROM tb_InventorySnapshot
WHERE SnapshotTime < DATEADD(MONTH, -12, GETDATE());
*/

-- 6. 查看最近的库存变化
SELECT TOP 50
    ProdDetailID,
    Location_ID,
    Quantity,
    LatestStorageTime,
    LatestOutboundTime
FROM tb_Inventory
WHERE LatestStorageTime > DATEADD(DAY, -1, GETDATE())
   OR LatestOutboundTime > DATEADD(DAY, -1, GETDATE())
ORDER BY ISNULL(LatestStorageTime, LatestOutboundTime) DESC;

-- 7. 检查是否有库存没有快照
SELECT 
    i.ProdDetailID,
    i.Location_ID,
    i.Quantity
FROM tb_Inventory i
LEFT JOIN tb_InventorySnapshot s 
    ON i.ProdDetailID = s.ProdDetailID 
    AND i.Location_ID = s.Location_ID
WHERE s.SnapshotID IS NULL;

-- 8. 验证快照数据准确性（对比最新快照和当前库存）
SELECT TOP 20
    i.ProdDetailID,
    i.Location_ID,
    i.Quantity as CurrentQuantity,
    s.Quantity as SnapshotQuantity,
    i.Quantity - s.Quantity as Difference,
    s.SnapshotTime
FROM tb_Inventory i
INNER JOIN (
    SELECT 
        ProdDetailID,
        Location_ID,
        Quantity,
        SnapshotTime,
        ROW_NUMBER() OVER (PARTITION BY ProdDetailID, Location_ID ORDER BY SnapshotTime DESC) as rn
    FROM tb_InventorySnapshot
) s ON i.ProdDetailID = s.ProdDetailID 
    AND i.Location_ID = s.Location_ID
    AND s.rn = 1
WHERE i.Quantity <> s.Quantity
ORDER BY ABS(i.Quantity - s.Quantity) DESC;

-- 9. 查看快照表大小
EXEC sp_spaceused 'tb_InventorySnapshot';

-- 10. 按月统计快照数据量
SELECT 
    YEAR(SnapshotTime) as Year,
    MONTH(SnapshotTime) as Month,
    COUNT(*) as RecordCount,
    MIN(SnapshotTime) as FirstSnapshot,
    MAX(SnapshotTime) as LastSnapshot
FROM tb_InventorySnapshot
GROUP BY YEAR(SnapshotTime), MONTH(SnapshotTime)
ORDER BY Year DESC, Month DESC;
