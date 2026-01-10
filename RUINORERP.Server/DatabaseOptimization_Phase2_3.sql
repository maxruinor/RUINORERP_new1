-- ========================================
-- Phase 2.3: 数据库索引优化脚本
-- 创建日期: 2026-01-11
-- 目的: 优化查询性能，提升查询速度50%+
-- ========================================

USE [RUINORERP_DB]; -- 请根据实际数据库名称修改

-- ========================================
-- 1. 销售出库明细复合索引
-- 用途: 优化安全库存计算中的销售历史查询
-- 优化前: 全表扫描 View_SaleOutItems
-- 优化后: 使用索引快速定位产品销售记录
-- ========================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_View_SaleOutItems_ProdDate' AND object_id = OBJECT_ID('View_SaleOutItems'))
BEGIN
    PRINT '创建销售出库明细复合索引...';
    
    -- 注意: 对于View，不能直接创建索引，需要在基础表上创建
    -- 如果View_SaleOutItems是视图，请在基础表上创建索引
    -- 如果View_SaleOutItems是表，则直接创建索引
    
    -- 假设 View_SaleOutItems 是基于 tb_SaleOutDetail 的视图
    IF OBJECT_ID('tb_SaleOutDetail', 'U') IS NOT NULL
    BEGIN
        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_tb_SaleOutDetail_ProdDate' AND object_id = OBJECT_ID('tb_SaleOutDetail'))
        BEGIN
            CREATE NONCLUSTERED INDEX IX_tb_SaleOutDetail_ProdDate
            ON tb_SaleOutDetail (ProdDetailID, OutDate DESC)
            INCLUDE (SaleOutID, Quantity);
            
            PRINT '已创建 tb_SaleOutDetail 复合索引 IX_tb_SaleOutDetail_ProdDate';
        END
        ELSE
        BEGIN
            PRINT '索引 IX_tb_SaleOutDetail_ProdDate 已存在';
        END
    END
END
GO

-- ========================================
-- 2. 提醒规则索引
-- 用途: 优化规则筛选和查询
-- ========================================

IF OBJECT_ID('tb_ReminderRule', 'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_tb_ReminderRule_Enabled_Type' AND object_id = OBJECT_ID('tb_ReminderRule'))
    BEGIN
        PRINT '创建提醒规则复合索引...';
        CREATE NONCLUSTERED INDEX IX_tb_ReminderRule_Enabled_Type
        ON tb_ReminderRule (IsEnabled, ReminderBizType)
        INCLUDE (JsonConfig);
        
        PRINT '已创建 tb_ReminderRule 复合索引 IX_tb_ReminderRule_Enabled_Type';
    END
    ELSE
    BEGIN
        PRINT '索引 IX_tb_ReminderRule_Enabled_Type 已存在';
    END
END
GO

-- ========================================
-- 3. 产品SKU索引
-- 用途: 优化SKU查找和去重
-- ========================================

IF OBJECT_ID('tb_ProdDetail', 'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_tb_ProdDetail_SKU' AND object_id = OBJECT_ID('tb_ProdDetail'))
    BEGIN
        PRINT '创建产品SKU唯一索引...';
        CREATE UNIQUE NONCLUSTERED INDEX UQ_tb_ProdDetail_SKU
        ON tb_ProdDetail (SKU);
        
        PRINT '已创建 tb_ProdDetail 唯一索引 UQ_tb_ProdDetail_SKU';
    END
    ELSE
    BEGIN
        PRINT '索引 UQ_tb_ProdDetail_SKU 已存在';
    END
END
GO

-- ========================================
-- 4. 库存表索引优化
-- 用途: 优化库存查询和批量查找
-- ========================================

IF OBJECT_ID('tb_Inventory', 'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_tb_Inventory_ProdDetailID' AND object_id = OBJECT_ID('tb_Inventory'))
    BEGIN
        PRINT '创建库存表复合索引...';
        CREATE NONCLUSTERED INDEX IX_tb_Inventory_ProdDetailID
        ON tb_Inventory (ProdDetailID, WarehouseID)
        INCLUDE (Quantity, UnitID);
        
        PRINT '已创建 tb_Inventory 复合索引 IX_tb_Inventory_ProdDetailID';
    END
    ELSE
    BEGIN
        PRINT '索引 IX_tb_Inventory_ProdDetailID 已存在';
    END
END
GO

-- ========================================
-- 5. 库存视图索引 (如果支持)
-- 用途: 优化View_Inventory查询
-- ========================================

IF OBJECT_ID('View_Inventory', 'V') IS NOT NULL
BEGIN
    -- 注意: 标准视图不支持索引，需要创建索引视图（需要SCHEMABINDING）
    -- 这里只是示例，实际使用时需要根据具体需求调整
    
    PRINT '注意: 视图 View_Inventory 是标准视图，不支持直接创建索引';
    PRINT '如需优化View_Inventory查询，请考虑: 1. 在基础表上创建索引 2. 考虑使用索引视图 3. 缓存查询结果';
END
GO

-- ========================================
-- 6. 会话相关索引 (如果存在会话表)
-- ========================================

-- 如果有会话相关表，可以添加以下索引
-- IF OBJECT_ID('tb_Session', 'U') IS NOT NULL
-- BEGIN
--     IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_tb_Session_LastActivity' AND object_id = OBJECT_ID('tb_Session'))
--     BEGIN
--         PRINT '创建会话表索引...';
--         CREATE NONCLUSTERED INDEX IX_tb_Session_LastActivity
--         ON tb_Session (LastActivityTime DESC)
--         INCLUDE (SessionID, UserID);
--         
--         PRINT '已创建 tb_Session 索引 IX_tb_Session_LastActivity';
--     END
-- END
-- GO

-- ========================================
-- 7. 索引使用统计查询
-- 用途: 监控索引使用情况
-- ========================================

SELECT
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    i.is_unique AS IsUnique,
    s.user_seeks AS UserSeeks,
    s.user_scans AS UserScans,
    s.user_lookups AS UserLookups,
    s.user_updates AS UserUpdates,
    s.last_user_seek AS LastUserSeek,
    s.last_user_scan AS LastUserScan,
    p.rows AS TableRows
FROM sys.indexes i
LEFT JOIN sys.dm_db_index_usage_stats s ON s.object_id = i.object_id AND s.index_id = i.index_id
LEFT JOIN sys.partitions p ON p.object_id = i.object_id AND p.index_id = i.index_id
WHERE OBJECT_NAME(i.object_id) IN ('tb_SaleOutDetail', 'tb_ReminderRule', 'tb_ProdDetail', 'tb_Inventory')
ORDER BY TableName, i.name;
GO

-- ========================================
-- 8. 索引碎片分析
-- 用途: 检查索引碎片情况，决定是否需要重建索引
-- ========================================

SELECT
    OBJECT_NAME(ips.object_id) AS TableName,
    i.name AS IndexName,
    ips.avg_fragmentation_in_percent AS FragmentationPercent,
    ips.page_count AS PageCount,
    CASE
        WHEN ips.avg_fragmentation_in_percent > 30 THEN '需要重建'
        WHEN ips.avg_fragmentation_in_percent > 10 THEN '需要重组'
        ELSE '正常'
    END AS ActionRequired
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'LIMITED') ips
INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE OBJECT_NAME(ips.object_id) IN ('tb_SaleOutDetail', 'tb_ReminderRule', 'tb_ProdDetail', 'tb_Inventory')
ORDER BY ips.avg_fragmentation_in_percent DESC;
GO

-- ========================================
-- 执行说明
-- ========================================
/*
1. 执行此脚本前，请确保:
   - 数据库已备份
   - 在测试环境先验证
   - 选择业务低峰期执行

2. 索引创建可能需要的时间:
   - 索引1 (销售出库明细): 10-30分钟，取决于数据量
   - 索引2 (提醒规则): 1-5分钟
   - 索引3 (产品SKU): 5-10分钟
   - 索引4 (库存表): 5-15分钟

3. 预期效果:
   - 销售历史查询速度提升 50-80%
   - 规则查询速度提升 60-90%
   - SKU查找速度提升 70-95%

4. 监控建议:
   - 执行后定期运行索引使用统计查询
   - 监控查询性能改进情况
   - 关注索引碎片，定期重建

5. 回滚方案:
   如需删除索引，执行以下SQL:
   DROP INDEX IX_tb_SaleOutDetail_ProdDate ON tb_SaleOutDetail;
   DROP INDEX IX_tb_ReminderRule_Enabled_Type ON tb_ReminderRule;
   DROP INDEX UQ_tb_ProdDetail_SKU ON tb_ProdDetail;
   DROP INDEX IX_tb_Inventory_ProdDetailID ON tb_Inventory;
*/

PRINT '========================================';
PRINT 'Phase 2.3 数据库索引优化脚本执行完成';
PRINT '========================================';
