-- =============================================
-- 发票与收付款关联管理模块 - 数据库迁移脚本
-- 作者: Watson
-- 创建时间: 2026-04-21
-- 说明: 为现有财务模块添加强化的发票-收付款关联功能
-- =============================================

BEGIN TRANSACTION
GO

-- =============================================
-- 1. 创建发票-收付款关联表
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_FM_InvoicePaymentRelation]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[tb_FM_InvoicePaymentRelation](
        [RelationId] [bigint] IDENTITY(1,1) NOT NULL,
        [InvoiceId] [bigint] NOT NULL,
        [InvoiceNo] [varchar](60) NULL,
        [PaymentId] [bigint] NOT NULL,
        [PaymentNo] [varchar](30) NULL,
        [ARAPId] [bigint] NULL,
        
        -- 关联金额（支持部分核销）
        [ForeignLinkedAmount] [money] NOT NULL DEFAULT 0,
        [LocalLinkedAmount] [money] NOT NULL DEFAULT 0,
        
        -- 核销状态：0-未核销 1-部分核销 2-完全核销
        [VerificationStatus] [int] NOT NULL DEFAULT 0,
        [VerifiedDate] [datetime] NULL,
        
        -- 审计字段
        [Created_at] [datetime] NULL,
        [Created_by] [bigint] NULL,
        [Modified_at] [datetime] NULL,
        [Modified_by] [bigint] NULL,
        [isdeleted] [bit] NOT NULL DEFAULT 0,
        
        CONSTRAINT [PK_tb_FM_InvoicePaymentRelation] PRIMARY KEY CLUSTERED ([RelationId] ASC),
        CONSTRAINT [UK_Invoice_Payment] UNIQUE NONCLUSTERED ([InvoiceId], [PaymentId])
    ) ON [PRIMARY]
    
    PRINT '✓ 表 tb_FM_InvoicePaymentRelation 创建成功'
END
ELSE
BEGIN
    PRINT '⚠ 表 tb_FM_InvoicePaymentRelation 已存在，跳过创建'
END
GO

-- 创建索引
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_InvoiceId' AND object_id = OBJECT_ID(N'[dbo].[tb_FM_InvoicePaymentRelation]'))
    CREATE INDEX [IX_InvoiceId] ON [tb_FM_InvoicePaymentRelation]([InvoiceId])
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_PaymentId' AND object_id = OBJECT_ID(N'[dbo].[tb_FM_InvoicePaymentRelation]'))
    CREATE INDEX [IX_PaymentId] ON [tb_FM_InvoicePaymentRelation]([PaymentId])
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ARAPId' AND object_id = OBJECT_ID(N'[dbo].[tb_FM_InvoicePaymentRelation]'))
    CREATE INDEX [IX_ARAPId] ON [tb_FM_InvoicePaymentRelation]([ARAPId])
GO

-- 添加外键约束
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Relation_Invoice' AND parent_object_id = OBJECT_ID(N'[dbo].[tb_FM_InvoicePaymentRelation]'))
BEGIN
    ALTER TABLE [dbo].[tb_FM_InvoicePaymentRelation] WITH CHECK ADD 
        CONSTRAINT [FK_Relation_Invoice] FOREIGN KEY([InvoiceId])
        REFERENCES [tb_FM_Invoice]([InvoiceId])
    PRINT '✓ 外键 FK_Relation_Invoice 创建成功'
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Relation_Payment' AND parent_object_id = OBJECT_ID(N'[dbo].[tb_FM_InvoicePaymentRelation]'))
BEGIN
    ALTER TABLE [dbo].[tb_FM_InvoicePaymentRelation] WITH CHECK ADD 
        CONSTRAINT [FK_Relation_Payment] FOREIGN KEY([PaymentId])
        REFERENCES [tb_FM_PaymentRecord]([PaymentId])
    PRINT '✓ 外键 FK_Relation_Payment 创建成功'
END
GO

-- 添加扩展属性说明
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description', N'SCHEMA', N'dbo', N'TABLE', N'tb_FM_InvoicePaymentRelation', NULL, NULL))
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发票与收付款关联表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_FM_InvoicePaymentRelation'
GO

-- =============================================
-- 2. 扩展 tb_FM_Invoice 表
-- =============================================

-- 添加发票类型字段
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_FM_Invoice]') AND name = 'InvoiceType')
BEGIN
    ALTER TABLE [tb_FM_Invoice] ADD [InvoiceType] [int] NOT NULL DEFAULT 0
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发票类型：0-销项发票 1-进项发票' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_FM_Invoice', @level2type=N'COLUMN',@level2name=N'InvoiceType'
    PRINT '✓ 字段 tb_FM_Invoice.InvoiceType 添加成功'
END
GO

-- 添加核销状态字段
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_FM_Invoice]') AND name = 'VerificationStatus')
BEGIN
    ALTER TABLE [tb_FM_Invoice] ADD [VerificationStatus] [int] NOT NULL DEFAULT 0
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'核销状态：0-未核销 1-部分核销 2-完全核销' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_FM_Invoice', @level2type=N'COLUMN',@level2name=N'VerificationStatus'
    PRINT '✓ 字段 tb_FM_Invoice.VerificationStatus 添加成功'
END
GO

-- 添加已关联总额字段
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_FM_Invoice]') AND name = 'TotalLinkedAmount')
BEGIN
    ALTER TABLE [tb_FM_Invoice] ADD [TotalLinkedAmount] [money] NULL DEFAULT 0
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已关联收付款总额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_FM_Invoice', @level2type=N'COLUMN',@level2name=N'TotalLinkedAmount'
    PRINT '✓ 字段 tb_FM_Invoice.TotalLinkedAmount 添加成功'
END
GO

-- 添加未关联金额字段
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_FM_Invoice]') AND name = 'UnlinkedAmount')
BEGIN
    ALTER TABLE [tb_FM_Invoice] ADD [UnlinkedAmount] [money] NULL DEFAULT 0
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未关联金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_FM_Invoice', @level2type=N'COLUMN',@level2name=N'UnlinkedAmount'
    PRINT '✓ 字段 tb_FM_Invoice.UnlinkedAmount 添加成功'
END
GO

-- 添加预期收付款日期字段
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_FM_Invoice]') AND name = 'ExpectedPaymentDate')
BEGIN
    ALTER TABLE [tb_FM_Invoice] ADD [ExpectedPaymentDate] [datetime] NULL
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预期收付款日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_FM_Invoice', @level2type=N'COLUMN',@level2name=N'ExpectedPaymentDate'
    PRINT '✓ 字段 tb_FM_Invoice.ExpectedPaymentDate 添加成功'
END
GO

-- 添加实际收付款日期字段
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_FM_Invoice]') AND name = 'ActualPaymentDate')
BEGIN
    ALTER TABLE [tb_FM_Invoice] ADD [ActualPaymentDate] [datetime] NULL
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际收付款日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_FM_Invoice', @level2type=N'COLUMN',@level2name=N'ActualPaymentDate'
    PRINT '✓ 字段 tb_FM_Invoice.ActualPaymentDate 添加成功'
END
GO

-- =============================================
-- 3. 扩展 tb_FM_PaymentRecord 表
-- =============================================

-- 添加已关联发票总额字段
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_FM_PaymentRecord]') AND name = 'TotalLinkedInvoiceAmount')
BEGIN
    ALTER TABLE [tb_FM_PaymentRecord] ADD [TotalLinkedInvoiceAmount] [money] NULL DEFAULT 0
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已关联发票总额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_FM_PaymentRecord', @level2type=N'COLUMN',@level2name=N'TotalLinkedInvoiceAmount'
    PRINT '✓ 字段 tb_FM_PaymentRecord.TotalLinkedInvoiceAmount 添加成功'
END
GO

-- 添加未关联发票金额字段
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[tb_FM_PaymentRecord]') AND name = 'UnlinkedInvoiceAmount')
BEGIN
    ALTER TABLE [tb_FM_PaymentRecord] ADD [UnlinkedInvoiceAmount] [money] NULL DEFAULT 0
    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未关联发票金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tb_FM_PaymentRecord', @level2type=N'COLUMN',@level2name=N'UnlinkedInvoiceAmount'
    PRINT '✓ 字段 tb_FM_PaymentRecord.UnlinkedInvoiceAmount 添加成功'
END
GO

-- =============================================
-- 4. 初始化历史数据（可选）
-- =============================================
-- 如果已有发票和应收应付数据，可以建立初始关联
/*
UPDATE inv
SET inv.VerificationStatus = CASE 
    WHEN rap.ForeignPaidAmount >= rap.TotalForeignPayableAmount THEN 2
    WHEN rap.ForeignPaidAmount > 0 THEN 1
    ELSE 0 
END,
inv.TotalLinkedAmount = ISNULL(rap.ForeignPaidAmount, 0),
inv.UnlinkedAmount = ISNULL(rap.TotalForeignPayableAmount, 0) - ISNULL(rap.ForeignPaidAmount, 0)
FROM tb_FM_Invoice inv
INNER JOIN tb_FM_ReceivablePayable rap ON inv.InvoiceId = rap.InvoiceId
WHERE inv.VerificationStatus = 0
*/

COMMIT TRANSACTION
GO

PRINT ''
PRINT '========================================'
PRINT '发票与收付款关联管理模块 - 迁移完成'
PRINT '========================================'
PRINT '新增表: tb_FM_InvoicePaymentRelation'
PRINT '扩展表: tb_FM_Invoice (+6字段)'
PRINT '扩展表: tb_FM_PaymentRecord (+2字段)'
PRINT '========================================'
GO
