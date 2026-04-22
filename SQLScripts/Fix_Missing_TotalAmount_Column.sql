-- =============================================
-- 修复 tb_FM_Invoice 表缺少 TotalAmount 列的问题
-- 错误：列名 'TotalAmount' 无效
-- 作者: Watson
-- 时间: 2026-04-21
-- =============================================

BEGIN TRANSACTION
GO

PRINT '========================================'
PRINT '开始检查并修复 tb_FM_Invoice 表结构'
PRINT '========================================'
PRINT ''

-- 1. 检查 TotalAmount 列是否存在
IF EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_Invoice' AND COLUMN_NAME = 'TotalAmount'
)
BEGIN
    PRINT '✓ tb_FM_Invoice.TotalAmount 列已存在'
    
    -- 显示列信息
    SELECT 
        COLUMN_NAME AS 列名,
        DATA_TYPE AS 数据类型,
        CHARACTER_MAXIMUM_LENGTH AS 长度,
        NUMERIC_PRECISION AS 精度,
        NUMERIC_SCALE AS 小数位,
        IS_NULLABLE AS 是否可空,
        COLUMN_DEFAULT AS 默认值
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_Invoice' AND COLUMN_NAME = 'TotalAmount'
END
ELSE
BEGIN
    PRINT '❌ tb_FM_Invoice.TotalAmount 列不存在，正在添加...'
    
    -- 添加 TotalAmount 列
    ALTER TABLE [tb_FM_Invoice] 
    ADD [TotalAmount] [money] NULL DEFAULT 0
    
    PRINT '✓ 已成功添加 TotalAmount 列'
    
    -- 添加列说明
    EXEC sys.sp_addextendedproperty 
        @name=N'MS_Description', 
        @value=N'发票总金额（不含税）',
        @level0type=N'SCHEMA',@level0name=N'dbo', 
        @level1type=N'TABLE',@level1name=N'tb_FM_Invoice', 
        @level2type=N'COLUMN',@level2name=N'TotalAmount'
        
    PRINT '✓ 已添加列说明'
END
GO

PRINT ''
PRINT '========================================'
PRINT '检查其他可能的缺失列'
PRINT '========================================'
PRINT ''

-- 2. 检查我们最近添加的其他字段
DECLARE @MissingColumns TABLE (ColumnName NVARCHAR(100))

-- 检查 InvoiceType
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_Invoice' AND COLUMN_NAME = 'InvoiceType'
)
BEGIN
    INSERT INTO @MissingColumns VALUES ('InvoiceType')
    PRINT '❌ 缺少列: InvoiceType'
END

-- 检查 VerificationStatus
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_Invoice' AND COLUMN_NAME = 'VerificationStatus'
)
BEGIN
    INSERT INTO @MissingColumns VALUES ('VerificationStatus')
    PRINT '❌ 缺少列: VerificationStatus'
END

-- 检查 TotalLinkedAmount
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_Invoice' AND COLUMN_NAME = 'TotalLinkedAmount'
)
BEGIN
    INSERT INTO @MissingColumns VALUES ('TotalLinkedAmount')
    PRINT '❌ 缺少列: TotalLinkedAmount'
END

-- 检查 UnlinkedAmount
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_Invoice' AND COLUMN_NAME = 'UnlinkedAmount'
)
BEGIN
    INSERT INTO @MissingColumns VALUES ('UnlinkedAmount')
    PRINT '❌ 缺少列: UnlinkedAmount'
END

-- 检查 ExpectedPaymentDate
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_Invoice' AND COLUMN_NAME = 'ExpectedPaymentDate'
)
BEGIN
    INSERT INTO @MissingColumns VALUES ('ExpectedPaymentDate')
    PRINT '❌ 缺少列: ExpectedPaymentDate'
END

-- 检查 ActualPaymentDate
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_Invoice' AND COLUMN_NAME = 'ActualPaymentDate'
)
BEGIN
    INSERT INTO @MissingColumns VALUES ('ActualPaymentDate')
    PRINT '❌ 缺少列: ActualPaymentDate'
END

IF NOT EXISTS (SELECT 1 FROM @MissingColumns)
BEGIN
    PRINT '✓ 所有新增列都已存在'
END
ELSE
BEGIN
    PRINT ''
    PRINT '⚠ 发现缺失的列，建议执行迁移脚本:'
    PRINT '   SQLScripts/Migration_InvoicePaymentRelation.sql'
END
GO

PRINT ''
PRINT '========================================'
PRINT '检查 tb_FM_PaymentRecord 表'
PRINT '========================================'
PRINT ''

-- 3. 检查 tb_FM_PaymentRecord 的新增字段
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_PaymentRecord' AND COLUMN_NAME = 'TotalLinkedInvoiceAmount'
)
BEGIN
    PRINT '❌ tb_FM_PaymentRecord 缺少列: TotalLinkedInvoiceAmount'
END
ELSE
BEGIN
    PRINT '✓ tb_FM_PaymentRecord.TotalLinkedInvoiceAmount 已存在'
END

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_PaymentRecord' AND COLUMN_NAME = 'UnlinkedInvoiceAmount'
)
BEGIN
    PRINT '❌ tb_FM_PaymentRecord 缺少列: UnlinkedInvoiceAmount'
END
ELSE
BEGIN
    PRINT '✓ tb_FM_PaymentRecord.UnlinkedInvoiceAmount 已存在'
END
GO

PRINT ''
PRINT '========================================'
PRINT '检查完成'
PRINT '========================================'
PRINT ''
PRINT '下一步操作：'
PRINT '1. 如果有缺失列，执行完整迁移脚本'
PRINT '2. 重新测试级联删除功能'
PRINT '3. 启用SqlSugar SQL日志以便调试'
PRINT '========================================'
GO

COMMIT TRANSACTION
GO
