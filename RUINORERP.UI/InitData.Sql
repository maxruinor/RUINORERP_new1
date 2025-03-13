CREATE TABLE [dbo].[LogDetails](
    -- 建议添加自增主键（根据实际需求可选）
    [LogId] INT IDENTITY(1,1) PRIMARY KEY,
    
    [LogDate] DATETIME NOT NULL DEFAULT GETDATE(),  -- 日志时间（默认当前时间）
    [LogThread] NVARCHAR(255) NOT NULL,             -- 线程信息
    [LogLevel] NVARCHAR(50) NOT NULL,               -- 日志级别（如INFO/ERROR）
    [LogLogger] NVARCHAR(255) NOT NULL,             -- 日志记录器名称
    [LogActionClick] NVARCHAR(1000) NULL,           -- 用户操作行为
    [LogMessage] NVARCHAR(MAX) NOT NULL,            -- 日志详细信息
    [UserName] NVARCHAR(255) NULL,                  -- 用户名（允许未登录情况）
    [UserIP] VARCHAR(45) NULL                       -- 支持IPv6最长格式
);



--产品属性类型值数据初始化
SET IDENTITY_INSERT [dbo].[tb_ProdPropertyType] ON
GO
--INSERT INTO [dbo].[tb_ProdPropertyType] ([PropertyType_ID], [PropertyTypeName], [PropertyTypeDesc]) VALUES (N'1', N'请选择', NULL)

INSERT INTO [dbo].[tb_ProdPropertyType] ([PropertyType_ID], [PropertyTypeName], [PropertyTypeDesc]) VALUES (N'1', N'单属性', NULL)
GO
INSERT INTO [dbo].[tb_ProdPropertyType] ([PropertyType_ID], [PropertyTypeName], [PropertyTypeDesc]) VALUES (N'2', N'可配置多属性', NULL)
GO
INSERT INTO [dbo].[tb_ProdPropertyType] ([PropertyType_ID], [PropertyTypeName], [PropertyTypeDesc]) VALUES (N'3', N'捆绑', NULL)
GO
INSERT INTO [dbo].[tb_ProdPropertyType] ([PropertyType_ID], [PropertyTypeName], [PropertyTypeDesc]) VALUES (N'4', N'虚拟', NULL)
GO
SET IDENTITY_INSERT [dbo].[tb_ProdPropertyType] OFF
GO
