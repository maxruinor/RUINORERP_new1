-- ====================================
-- 单据图片存储优化 - 数据库迁移脚本
-- ====================================
-- 创建日期: 2026-01-20
-- 目的: 添加HasAttachment标志位，优化查询性能
-- ====================================

-- 步骤1: 为核心业务表添加HasAttachment标志位
-- ---------------------------------------

-- 销售订单表
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID('tb_SaleOrder')
    AND name = 'HasAttachment'
)
BEGIN
    ALTER TABLE tb_SaleOrder ADD HasAttachment BIT DEFAULT 0;
    PRINT '已为 tb_SaleOrder 添加 HasAttachment 字段';
END

-- 产品表
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID('tb_Prod')
    AND name = 'HasAttachment'
)
BEGIN
    ALTER TABLE tb_Prod ADD HasAttachment BIT DEFAULT 0;
    PRINT '已为 tb_Prod 添加 HasAttachment 字段';
END

-- 产品明细表
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID('tb_ProdDetail')
    AND name = 'HasAttachment'
)
BEGIN
    ALTER TABLE tb_ProdDetail ADD HasAttachment BIT DEFAULT 0;
    PRINT '已为 tb_ProdDetail 添加 HasAttachment 字段';
END

-- 费用报销主表
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID('tb_FM_ExpenseClaim')
    AND name = 'HasAttachment'
)
BEGIN
    ALTER TABLE tb_FM_ExpenseClaim ADD HasAttachment BIT DEFAULT 0;
    PRINT '已为 tb_FM_ExpenseClaim 添加 HasAttachment 字段';
END

-- 付款记录主表
IF NOT EXISTS (
    SELECT * FROM sys.columns
    WHERE object_id = OBJECT_ID('tb_FM_PaymentRecord')
    AND name = 'HasAttachment'
)
BEGIN
    ALTER TABLE tb_FM_PaymentRecord ADD HasAttachment BIT DEFAULT 0;
    PRINT '已为 tb_FM_PaymentRecord 添加 HasAttachment 字段';
END

-- 步骤2: 创建性能优化索引
-- ---------------------------------------

-- 业务关联表查询优化索引
IF NOT EXISTS (
    SELECT * FROM sys.indexes
    WHERE name = 'IX_FS_BusinessRelation_Query'
    AND object_id = OBJECT_ID('tb_FS_BusinessRelation')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_FS_BusinessRelation_Query
    ON tb_FS_BusinessRelation(BusinessType, BusinessId, RelatedField, IsDetailTable)
    WHERE IsActive = 1 AND isdeleted = 0;

    PRINT '已创建索引 IX_FS_BusinessRelation_Query';
END

-- 业务关联表字段查询优化索引
IF NOT EXISTS (
    SELECT * FROM sys.indexes
    WHERE name = 'IX_FS_BusinessRelation_Field'
    AND object_id = OBJECT_ID('tb_FS_BusinessRelation')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_FS_BusinessRelation_Field
    ON tb_FS_BusinessRelation(RelatedField, BusinessNo, IsActive, isdeleted);

    PRINT '已创建索引 IX_FS_BusinessRelation_Field';
END

-- 文件存储表哈希值索引（用于去重）
IF NOT EXISTS (
    SELECT * FROM sys.indexes
    WHERE name = 'IX_FS_FileStorageInfo_Hash'
    AND object_id = OBJECT_ID('tb_FS_FileStorageInfo')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_FS_FileStorageInfo_Hash
    ON tb_FS_FileStorageInfo(HashValue)
    WHERE HashValue IS NOT NULL;

    PRINT '已创建索引 IX_FS_FileStorageInfo_Hash';
END

-- 步骤3: 初始化HasAttachment标志位
-- ---------------------------------------

-- 更新销售订单的HasAttachment标志
UPDATE so
SET so.HasAttachment = CASE WHEN br.RelationId IS NOT NULL THEN 1 ELSE 0 END
FROM tb_SaleOrder so
LEFT JOIN tb_FS_BusinessRelation br
    ON br.BusinessType = (SELECT BizType FROM tb_BizType WHERE TypeName = '销售订单')
    AND br.BusinessId = so.SOrder_ID
    AND br.IsActive = 1
    AND br.isdeleted = 0;

PRINT '已更新 tb_SaleOrder 的 HasAttachment 标志位';

-- 更新产品的HasAttachment标志
UPDATE p
SET p.HasAttachment = CASE
    WHEN p.ImagesPath IS NOT NULL AND LEN(p.ImagesPath) > 0 THEN 1
    WHEN br.RelationId IS NOT NULL THEN 1
    ELSE 0
END
FROM tb_Prod p
LEFT JOIN tb_FS_BusinessRelation br
    ON br.BusinessType = (SELECT BizType FROM tb_BizType WHERE TypeName = '产品')
    AND br.BusinessId = p.Prod_ID
    AND br.IsActive = 1
    AND br.isdeleted = 0;

PRINT '已更新 tb_Prod 的 HasAttachment 标志位';

-- 步骤4: 创建同步HasAttachment标志的触发器（可选）
-- ---------------------------------------

-- 销售订单图片关联变更触发器
IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'tr_SaleOrder_UpdateAttachment')
BEGIN
    DROP TRIGGER tr_SaleOrder_UpdateAttachment;
    PRINT '已删除旧触发器 tr_SaleOrder_UpdateAttachment';
END

GO

CREATE TRIGGER tr_SaleOrder_UpdateAttachment
ON tb_FS_BusinessRelation
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @BizType_SaleOrder INT = (SELECT TOP 1 BizType FROM tb_BizType WHERE TypeName = '销售订单');

    -- 更新插入或更新的记录
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        UPDATE so
        SET HasAttachment = 1
        FROM tb_SaleOrder so
        INNER JOIN inserted i
            ON i.BusinessType = @BizType_SaleOrder
            AND i.BusinessId = so.SOrder_ID
            AND i.IsActive = 1
            AND i.isdeleted = 0
        WHERE so.HasAttachment = 0;
    END

    -- 更新删除的记录（检查是否还有其他关联）
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        UPDATE so
        SET HasAttachment = CASE
            WHEN EXISTS (
                SELECT 1 FROM tb_FS_BusinessRelation br
                WHERE br.BusinessType = @BizType_SaleOrder
                AND br.BusinessId = so.SOrder_ID
                AND br.IsActive = 1
                AND br.isdeleted = 0
            ) THEN 1 ELSE 0
        END
        FROM tb_SaleOrder so
        INNER JOIN deleted d
            ON d.BusinessType = @BizType_SaleOrder
            AND d.BusinessId = so.SOrder_ID;
    END
END

PRINT '已创建触发器 tr_SaleOrder_UpdateAttachment';

GO

PRINT '========================================';
PRINT '数据库迁移完成！';
PRINT '========================================';
PRINT '已完成的操作:';
PRINT '  1. 添加HasAttachment标志位到核心业务表';
PRINT '  2. 创建性能优化索引';
PRINT '  3. 初始化现有数据的HasAttachment标志';
PRINT '  4. 创建自动同步触发器';
PRINT '========================================';
