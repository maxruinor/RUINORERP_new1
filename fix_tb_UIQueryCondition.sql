-- 修复 tb_UIQueryCondition 表缺失的列
-- 执行日期: 2025-01-13
-- 说明: 为表添加缺失的 Default2、EnableDefault2 和 DiffDays2 列

USE [erpnew]
GO

-- 检查并添加 Default2 列
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_UIQueryCondition]') AND name = 'Default2')
BEGIN
    ALTER TABLE [dbo].[tb_UIQueryCondition] ADD [Default2] varchar(255) NULL
    PRINT '已添加 Default2 列'
END
ELSE
BEGIN
    PRINT 'Default2 列已存在'
END
GO

-- 检查并添加 EnableDefault2 列
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_UIQueryCondition]') AND name = 'EnableDefault2')
BEGIN
    ALTER TABLE [dbo].[tb_UIQueryCondition] ADD [EnableDefault2] bit NULL
    PRINT '已添加 EnableDefault2 列'
END
ELSE
BEGIN
    PRINT 'EnableDefault2 列已存在'
END
GO

-- 检查并添加 DiffDays2 列
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_UIQueryCondition]') AND name = 'DiffDays2')
BEGIN
    ALTER TABLE [dbo].[tb_UIQueryCondition] ADD [DiffDays2] int NULL
    PRINT '已添加 DiffDays2 列'
END
ELSE
BEGIN
    PRINT 'DiffDays2 列已存在'
END
GO

-- 添加列的描述信息(如果存在则跳过)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_UIQueryCondition]') AND name = 'Default2')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.extended_properties
                WHERE major_id = OBJECT_ID('dbo.tb_UIQueryCondition')
                AND minor_id = (SELECT column_id FROM sys.columns
                             WHERE object_id = OBJECT_ID('dbo.tb_UIQueryCondition')
                             AND name = 'Default2'))
    BEGIN
        EXEC sp_addextendedproperty 'MS_Description', '默认值2', 'user', 'dbo', 'table', 'tb_UIQueryCondition', 'column', 'Default2'
        PRINT '已添加 Default2 列的描述'
    END
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_UIQueryCondition]') AND name = 'EnableDefault2')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.extended_properties
                WHERE major_id = OBJECT_ID('dbo.tb_UIQueryCondition')
                AND minor_id = (SELECT column_id FROM sys.columns
                             WHERE object_id = OBJECT_ID('dbo.tb_UIQueryCondition')
                             AND name = 'EnableDefault2'))
    BEGIN
        EXEC sp_addextendedproperty 'MS_Description', '启用默认值2', 'user', 'dbo', 'table', 'tb_UIQueryCondition', 'column', 'EnableDefault2'
        PRINT '已添加 EnableDefault2 列的描述'
    END
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_UIQueryCondition]') AND name = 'DiffDays2')
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.extended_properties
                WHERE major_id = OBJECT_ID('dbo.tb_UIQueryCondition')
                AND minor_id = (SELECT column_id FROM sys.columns
                             WHERE object_id = OBJECT_ID('dbo.tb_UIQueryCondition')
                             AND name = 'DiffDays2'))
    BEGIN
        EXEC sp_addextendedproperty 'MS_Description', '差异天数2', 'user', 'dbo', 'table', 'tb_UIQueryCondition', 'column', 'DiffDays2'
        PRINT '已添加 DiffDays2 列的描述'
    END
END
GO

PRINT 'tb_UIQueryCondition 表结构修复完成'
GO
