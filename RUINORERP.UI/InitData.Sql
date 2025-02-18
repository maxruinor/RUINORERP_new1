



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
