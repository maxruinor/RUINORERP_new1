-- =============================================
-- 数据库迁移脚本: tb_FS_BusinessRelation 表增强
-- 版本: 1.0
-- 创建时间: 2026-01-18
-- 说明: 添加 BusinessId、IsDetailTable、DetailId 字段以提升性能和功能
-- =============================================

USE [YourDatabaseName]  -- 请替换为实际数据库名称
GO

PRINT '开始执行文件关联表增强迁移脚本...';
GO

-- 1. 添加 BusinessId 字段 (业务主键ID)
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID('tb_FS_BusinessRelation')
    AND name = 'BusinessId'
)
BEGIN
    PRINT '添加 BusinessId 字段...';
    ALTER TABLE tb_FS_BusinessRelation
    ADD BusinessId BIGINT NULL;

    PRINT 'BusinessId 字段添加成功。';
END
ELSE
BEGIN
    PRINT 'BusinessId 字段已存在,跳过添加。';
END
GO

-- 2. 为 BusinessId 字段创建索引
IF NOT EXISTS (
    SELECT * FROM sys.indexes
    WHERE object_id = OBJECT_ID('tb_FS_BusinessRelation')
    AND name = 'IX_BusinessRelation_BusinessId'
)
BEGIN
    PRINT '为 BusinessId 字段创建索引...';
    CREATE INDEX IX_BusinessRelation_BusinessId
    ON tb_FS_BusinessRelation(BusinessId)
    WHERE BusinessId IS NOT NULL;

    PRINT 'BusinessId 索引创建成功。';
END
ELSE
BEGIN
    PRINT 'BusinessId 索引已存在,跳过创建。';
END
GO

-- 3. 添加 IsDetailTable 字段 (是否明细表文件)
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID('tb_FS_BusinessRelation')
    AND name = 'IsDetailTable'
)
BEGIN
    PRINT '添加 IsDetailTable 字段...';
    ALTER TABLE tb_FS_BusinessRelation
    ADD IsDetailTable BIT NOT NULL DEFAULT(0);

    PRINT 'IsDetailTable 字段添加成功。';
END
ELSE
BEGIN
    PRINT 'IsDetailTable 字段已存在,跳过添加。';
END
GO

-- 4. 添加 DetailId 字段 (明细表主键ID)
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID('tb_FS_BusinessRelation')
    AND name = 'DetailId'
)
BEGIN
    PRINT '添加 DetailId 字段...';
    ALTER TABLE tb_FS_BusinessRelation
    ADD DetailId BIGINT NULL;

    PRINT 'DetailId 字段添加成功。';
END
ELSE
BEGIN
    PRINT 'DetailId 字段已存在,跳过添加。';
END
GO

-- 5. 为 DetailId 字段创建索引
IF NOT EXISTS (
    SELECT * FROM sys.indexes
    WHERE object_id = OBJECT_ID('tb_FS_BusinessRelation')
    AND name = 'IX_BusinessRelation_DetailId'
)
BEGIN
    PRINT '为 DetailId 字段创建索引...';
    CREATE INDEX IX_BusinessRelation_DetailId
    ON tb_FS_BusinessRelation(DetailId)
    WHERE DetailId IS NOT NULL;

    PRINT 'DetailId 索引创建成功。';
END
ELSE
BEGIN
    PRINT 'DetailId 索引已存在,跳过创建。';
END
GO

-- 6. 数据迁移: 回填 BusinessId (可选,根据实际业务表结构调整)
-- 注意: 下面的SQL需要根据您的实际表结构和业务逻辑进行调整
-- 示例: 从销售订单表回填 BusinessId
PRINT '开始回填 BusinessId 数据...';
GO

-- 示例1: 销售订单的 BusinessId 回填
-- UPDATE br
-- SET br.BusinessId = so.SOrder_ID
-- FROM tb_FS_BusinessRelation br
-- INNER JOIN tb_SaleOrder so ON br.BusinessNo = so.SOrderNo AND br.BusinessType = 1  -- BizType.销售订单的值
-- WHERE br.BusinessId IS NULL;
-- GO

-- 示例2: 费用报销单的 BusinessId 回填
-- UPDATE br
-- SET br.BusinessId = ec.ClaimMainID
-- FROM tb_FS_BusinessRelation br
-- INNER JOIN tb_FM_ExpenseClaim ec ON br.BusinessNo = ec.ClaimNo AND br.BusinessType = 2  -- BizType.费用报销单的值
-- WHERE br.BusinessId IS NULL;
-- GO

-- 示例3: 付款单的 BusinessId 回填
-- UPDATE br
-- SET br.BusinessId = pr.PaymentRecord_ID
-- FROM tb_FS_BusinessRelation br
-- INNER JOIN tb_FM_PaymentRecord pr ON br.BusinessNo = pr.PaymentNo AND br.BusinessType = 3  -- BizType.付款单的值
-- WHERE br.BusinessId IS NULL;
-- GO

PRINT 'BusinessId 数据回填完成。';
GO

-- 7. 验证迁移结果
PRINT '============================================';
PRINT '迁移完成! 验证结果:';
PRINT '============================================';

SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_FS_BusinessRelation'
AND COLUMN_NAME IN ('BusinessId', 'IsDetailTable', 'DetailId')
ORDER BY ORDINAL_POSITION;
GO

-- 检查新字段的索引
SELECT
    i.name AS IndexName,
    i.type_desc AS IndexType,
    i.is_unique,
    i.is_unique_constraint,
    STUFF((
        SELECT ', ' + c.name
        FROM sys.index_columns ic
        INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
        WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id
        ORDER BY ic.key_ordinal
        FOR XML PATH(''), TYPE
    ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS Columns
FROM sys.indexes i
WHERE i.object_id = OBJECT_ID('tb_FS_BusinessRelation')
AND i.name IN ('IX_BusinessRelation_BusinessId', 'IX_BusinessRelation_DetailId')
ORDER BY i.name;
GO

-- 统计业务关联表中的数据
PRINT '============================================';
PRINT '数据统计:';
PRINT '============================================';

SELECT
    COUNT(*) AS TotalRecords,
    COUNT(BusinessId) AS BusinessIdFilled,
    COUNT(CASE WHEN IsDetailTable = 0 THEN 1 END) AS MainTableRecords,
    COUNT(CASE WHEN IsDetailTable = 1 THEN 1 END) AS DetailTableRecords,
    COUNT(DetailId) AS DetailIdFilled
FROM tb_FS_BusinessRelation;
GO

PRINT '============================================';
PRINT '数据库迁移脚本执行完成!';
PRINT '============================================';
GO

-- 使用说明:
-- 1. 替换 YourDatabaseName 为实际数据库名称
-- 2. 根据实际业务调整第6步的数据回填SQL
-- 3. 执行前请备份数据库
-- 4. 建议在非高峰期执行
-- 5. 执行后请验证数据和索引是否正确
