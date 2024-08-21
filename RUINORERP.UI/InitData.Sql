

 CREATE TABLE [dbo].[Log] (
     [Id] [int] IDENTITY (1, 1) NOT NULL,
     [Date] [datetime] NOT NULL,
     [Thread] [varchar] (255) NOT NULL,
     [Level] [varchar] (50) NOT NULL,
     [Logger] [varchar] (255) NOT NULL,
     [Message] [varchar] (4000) NOT NULL,
     [Exception] [varchar] (2000) NULL
 )
 ---添加了自定义字段，方便查找问题所在模块用户等
 CREATE TABLE [dbo].[Logs] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [Level]      NVARCHAR (MAX) NULL,
    [Logger]     NVARCHAR (MAX) NULL,
    [Message]    NVARCHAR (MAX) NULL,
    [Date]       DATETIME       NOT NULL,
    [Exception]  NVARCHAR (MAX) NULL,
    [ModName]    NVARCHAR (MAX) NULL,
    [UserName]   NVARCHAR (MAX) NULL,
    [Mac]        NVARCHAR (MAX) NULL,
    [IP]         NVARCHAR (MAX) NULL,
    [Url]        NVARCHAR (MAX) NULL,
    [ActionName] NVARCHAR (MAX) NULL,
    [Name]       NVARCHAR (MAX) NULL
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
