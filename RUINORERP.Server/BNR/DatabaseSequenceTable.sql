-- 创建序号表
CREATE TABLE SequenceNumbers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SequenceKey NVARCHAR(255) NOT NULL UNIQUE,  -- 序号键，唯一标识一个序号序列
    CurrentValue BIGINT NOT NULL DEFAULT 0,     -- 当前序号值
    LastUpdated DATETIME2 NOT NULL DEFAULT GETDATE(), -- 最后更新时间
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()    -- 创建时间
);

-- 创建索引以提高查询性能
CREATE INDEX IX_SequenceNumbers_SequenceKey ON SequenceNumbers (SequenceKey);

-- 添加注释
EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = N'序号表，用于存储各种业务序号的当前值',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE',  @level1name = 'SequenceNumbers';

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = N'序号键，唯一标识一个序号序列',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE',  @level1name = 'SequenceNumbers',
    @level2type = N'COLUMN', @level2name = 'SequenceKey';

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = N'当前序号值',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE',  @level1name = 'SequenceNumbers',
    @level2type = N'COLUMN', @level2name = 'CurrentValue';

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = N'最后更新时间',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE',  @level1name = 'SequenceNumbers',
    @level2type = N'COLUMN', @level2name = 'LastUpdated';