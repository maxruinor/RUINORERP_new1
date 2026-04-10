-- =============================================
-- 配方表成本字段增强迁移脚本(简化版)
-- 版本: 2.0
-- 日期: 2025-04-09
-- 说明: 为tb_BOM_SDetail表添加实时成本字段,UnitCost作为固定成本保留
-- =============================================

BEGIN TRANSACTION;

-- 1. 添加实时成本字段(系统自动更新,允许NULL,默认0)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_BOM_SDetail') AND name = 'RealTimeCost')
BEGIN
    ALTER TABLE tb_BOM_SDetail 
    ADD RealTimeCost MONEY NULL DEFAULT 0;
    
    EXEC sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'实时成本(采购入库/缴库时自动更新,作为缴库单成本依据)', 
        @level0type = N'SCHEMA', @level0name = N'dbo',
        @level1type = N'TABLE', @level1name = N'tb_BOM_SDetail',
        @level2type = N'COLUMN', @level2name = N'RealTimeCost';
        
    PRINT '✓ 已添加 RealTimeCost 字段(允许NULL,默认0)';
END
ELSE
BEGIN
    PRINT '⚠ RealTimeCost 字段已存在,跳过';
END

-- 1.5 添加实时成本小计字段
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_BOM_SDetail') AND name = 'SubtotalRealTimeCost')
BEGIN
    ALTER TABLE tb_BOM_SDetail 
    ADD SubtotalRealTimeCost MONEY NULL DEFAULT 0;
    
    EXEC sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'实时成本小计(= RealTimeCost * UsedQty,系统自动计算)', 
        @level0type = N'SCHEMA', @level0name = N'dbo',
        @level1type = N'TABLE', @level1name = N'tb_BOM_SDetail',
        @level2type = N'COLUMN', @level2name = N'SubtotalRealTimeCost';
        
    PRINT '✓ 已添加 SubtotalRealTimeCost 字段(允许NULL,默认0)';
END
ELSE
BEGIN
    PRINT '⚠ SubtotalRealTimeCost 字段已存在,跳过';
END

-- 2. 数据迁移: 将现有UnitCost值同步到RealTimeCost和SubtotalRealTimeCost(初始值)
UPDATE tb_BOM_SDetail 
SET RealTimeCost = UnitCost,
    SubtotalRealTimeCost = SubtotalUnitCost
WHERE (RealTimeCost IS NULL OR RealTimeCost = 0) 
   AND (SubtotalRealTimeCost IS NULL OR SubtotalRealTimeCost = 0);

PRINT '✓ 已完成历史数据迁移(将UnitCost/SubtotalUnitCost同步到实时成本字段)';

-- 3. 更新UnitCost字段说明(标记为预估成本)
EXEC sp_updateextendedproperty 
    @name = N'MS_Description', 
    @value = N'预估成本(手工录入的标准成本,用于预算控制)', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tb_BOM_SDetail',
    @level2type = N'COLUMN', @level2name = N'UnitCost';

PRINT '✓ 已更新 UnitCost 字段说明为"预估成本"';

-- 4. 创建索引优化查询性能
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_BOM_SDetail_RealTimeCost')
BEGIN
    CREATE NONCLUSTERED INDEX IX_BOM_SDetail_RealTimeCost 
    ON tb_BOM_SDetail(RealTimeCost)
    INCLUDE (ProdDetailID, BOM_ID, UsedQty);
    
    PRINT '✓ 已创建 RealTimeCost 索引';
END

-- 5. 添加约束: 确保成本非负(允许NULL)
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_BOM_SDetail_Cost_NonNegative')
BEGIN
    ALTER TABLE tb_BOM_SDetail 
    ADD CONSTRAINT CK_BOM_SDetail_Cost_NonNegative 
    CHECK ((RealTimeCost IS NULL OR RealTimeCost >= 0) AND UnitCost >= 0);
    
    PRINT '✓ 已添加成本非负约束(允许NULL)';
END

COMMIT TRANSACTION;

PRINT '';
PRINT '========================================';
PRINT '迁移完成!';
PRINT '========================================';
PRINT '字段说明:';
PRINT '  - UnitCost: 预估成本(手工录入的标准成本,用于预算控制)';
PRINT '  - RealTimeCost: 实时成本(系统根据采购/缴库自动更新,用于业务执行)';
PRINT '  - SubtotalRealTimeCost: 实时成本小计(= RealTimeCost * UsedQty)';
PRINT '';
PRINT '成本使用优先级: RealTimeCost > UnitCost';
PRINT '';
PRINT '后续操作建议:';
PRINT '  1. BOM编辑界面: UnitCost(预估成本)可编辑,RealTimeCost只读显示';
PRINT '  2. 修改缴库单审核逻辑,优先使用RealTimeCost';
PRINT '  3. 实现采购入库时自动更新RealTimeCost的逻辑';
PRINT '========================================';
