-- =============================================
-- 流程导航图功能完整数据库脚本
-- 包含表、索引、视图、存储过程、函数和示例数据
-- 支持多级结构的层级关系管理
-- 注意：时间更新通过代码实现，不使用数据库触发器
-- =============================================

-- 1. 创建流程导航图定义表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tb_ProcessNavigation' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[tb_ProcessNavigation] (
        [ProcessNavID] [bigint] IDENTITY(1,1) NOT NULL,
        [ProcessNavName] [varchar](200) NULL,
        [Description] [varchar](500) NULL,
        [Version] [int] NOT NULL DEFAULT 1,
        [GraphXml] [text] NULL,
        [GraphJson] [text] NULL,
        [ModuleID] [bigint] NULL,
        [NavigationLevel] [int] NOT NULL DEFAULT 2,
        [ParentNavigationID] [bigint] NULL,
        [HierarchyLevel] [int] NOT NULL DEFAULT 1,
        [SortOrder] [int] NOT NULL DEFAULT 0,
        [CreateUserID] [bigint] NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [IsDefault] [bit] NOT NULL DEFAULT 0,
        [Category] [varchar](100) NULL,
        [Tags] [varchar](300) NULL,
        [CreateTime] [datetime] NOT NULL ,
        [UpdateTime] [datetime] NOT NULL,
        CONSTRAINT [PK_tb_ProcessNavigation] PRIMARY KEY CLUSTERED ([ProcessNavID] ASC)
    )
    
    PRINT '创建表 tb_ProcessNavigation 成功'
END
ELSE
BEGIN
    PRINT '表 tb_ProcessNavigation 已存在'
END

-- 2. 创建流程导航图节点表
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tb_ProcessNavigationNode' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[tb_ProcessNavigationNode] (
        [NodeID] [bigint] IDENTITY(1,1) NOT NULL,
        [ProcessNavID] [bigint] NOT NULL,
        [NodeCode] [varchar](100) NULL,
        [NodeName] [varchar](200) NULL,
        [Description] [varchar](500) NULL,
        [BusinessType] [int] NOT NULL DEFAULT 0,
        [MenuID] [bigint] NULL,
        [ModuleID] [bigint] NULL,
        [ChildNavigationID] [bigint] NULL,
        [FormName] [varchar](200) NULL,
        [ClassPath] [varchar](300) NULL,
        [NodeColor] [varchar](50) NULL,
        [PositionX] [float] NOT NULL DEFAULT 0,
        [PositionY] [float] NOT NULL DEFAULT 0,
        [Width] [float] NOT NULL DEFAULT 140,
        [Height] [float] NOT NULL DEFAULT 80,
        [NodeType] [varchar](50) NULL,
        [SortOrder] [int] NOT NULL DEFAULT 0,
        [CreateTime] [datetime] NOT NULL ,
        [UpdateTime] [datetime] NOT NULL ,
        CONSTRAINT [PK_tb_ProcessNavigationNode] PRIMARY KEY CLUSTERED ([NodeID] ASC)
    )
    
    PRINT '创建表 tb_ProcessNavigationNode 成功'
END
ELSE
BEGIN
    PRINT '表 tb_ProcessNavigationNode 已存在'
END

-- 3. 创建外键约束
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FK_tb_ProcessNavigation_tb_ModuleDefinition' AND xtype='F')
BEGIN
    ALTER TABLE [dbo].[tb_ProcessNavigation] WITH NOCHECK ADD 
    CONSTRAINT [FK_tb_ProcessNavigation_tb_ModuleDefinition] FOREIGN KEY([ModuleID])
    REFERENCES [dbo].[tb_ModuleDefinition] ([ModuleID])
    ON DELETE SET NULL
    PRINT '创建外键 FK_tb_ProcessNavigation_tb_ModuleDefinition 成功'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FK_tb_ProcessNavigation_tb_ProcessNavigation_Parent' AND xtype='F')
BEGIN
    ALTER TABLE [dbo].[tb_ProcessNavigation] WITH NOCHECK ADD 
    CONSTRAINT [FK_tb_ProcessNavigation_tb_ProcessNavigation_Parent] FOREIGN KEY([ParentNavigationID])
    REFERENCES [dbo].[tb_ProcessNavigation] ([ProcessNavID])
    ON DELETE NO ACTION
    PRINT '创建外键 FK_tb_ProcessNavigation_tb_ProcessNavigation_Parent 成功'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FK_tb_ProcessNavigationNode_tb_ProcessNavigation' AND xtype='F')
BEGIN
    ALTER TABLE [dbo].[tb_ProcessNavigationNode] WITH NOCHECK ADD 
    CONSTRAINT [FK_tb_ProcessNavigationNode_tb_ProcessNavigation] FOREIGN KEY([ProcessNavID])
    REFERENCES [dbo].[tb_ProcessNavigation] ([ProcessNavID])
    ON DELETE CASCADE
    PRINT '创建外键 FK_tb_ProcessNavigationNode_tb_ProcessNavigation 成功'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FK_tb_ProcessNavigationNode_tb_MenuInfo' AND xtype='F')
BEGIN
    ALTER TABLE [dbo].[tb_ProcessNavigationNode] WITH NOCHECK ADD 
    CONSTRAINT [FK_tb_ProcessNavigationNode_tb_MenuInfo] FOREIGN KEY([MenuID])
    REFERENCES [dbo].[tb_MenuInfo] ([MenuID])
    ON DELETE SET NULL
    PRINT '创建外键 FK_tb_ProcessNavigationNode_tb_MenuInfo 成功'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FK_tb_ProcessNavigationNode_tb_ModuleDefinition' AND xtype='F')
BEGIN
    ALTER TABLE [dbo].[tb_ProcessNavigationNode] WITH NOCHECK ADD 
    CONSTRAINT [FK_tb_ProcessNavigationNode_tb_ModuleDefinition] FOREIGN KEY([ModuleID])
    REFERENCES [dbo].[tb_ModuleDefinition] ([ModuleID])
    ON DELETE SET NULL
    PRINT '创建外键 FK_tb_ProcessNavigationNode_tb_ModuleDefinition 成功'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FK_tb_ProcessNavigationNode_tb_ProcessNavigation_Child' AND xtype='F')
BEGIN
    ALTER TABLE [dbo].[tb_ProcessNavigationNode] WITH NOCHECK ADD 
    CONSTRAINT [FK_tb_ProcessNavigationNode_tb_ProcessNavigation_Child] FOREIGN KEY([ChildNavigationID])
    REFERENCES [dbo].[tb_ProcessNavigation] ([ProcessNavID])
    ON DELETE NO ACTION
    PRINT '创建外键 FK_tb_ProcessNavigationNode_tb_ProcessNavigation_Child 成功'
END

-- 4. 创建索引
IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='IX_tb_ProcessNavigation_IsActive' AND id=OBJECT_ID('tb_ProcessNavigation'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tb_ProcessNavigation_IsActive] ON [dbo].[tb_ProcessNavigation] ([IsActive] ASC)
    PRINT '创建索引 IX_tb_ProcessNavigation_IsActive 成功'
END

IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='IX_tb_ProcessNavigation_ModuleID' AND id=OBJECT_ID('tb_ProcessNavigation'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tb_ProcessNavigation_ModuleID] ON [dbo].[tb_ProcessNavigation] ([ModuleID] ASC)
    PRINT '创建索引 IX_tb_ProcessNavigation_ModuleID 成功'
END

IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='IX_tb_ProcessNavigation_NavigationLevel' AND id=OBJECT_ID('tb_ProcessNavigation'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tb_ProcessNavigation_NavigationLevel] ON [dbo].[tb_ProcessNavigation] ([NavigationLevel] ASC)
    PRINT '创建索引 IX_tb_ProcessNavigation_NavigationLevel 成功'
END

-- 添加层级相关索引
IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='IX_tb_ProcessNavigation_Hierarchy' AND id=OBJECT_ID('tb_ProcessNavigation'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tb_ProcessNavigation_Hierarchy] ON [dbo].[tb_ProcessNavigation]
    (HierarchyLevel ASC, SortOrder ASC)
    PRINT '创建索引 IX_tb_ProcessNavigation_Hierarchy 成功'
END

-- 添加父导航ID索引
IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='IX_tb_ProcessNavigation_ParentNavigationID' AND id=OBJECT_ID('tb_ProcessNavigation'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tb_ProcessNavigation_ParentNavigationID] ON [dbo].[tb_ProcessNavigation]
    (ParentNavigationID ASC)
    PRINT '创建索引 IX_tb_ProcessNavigation_ParentNavigationID 成功'
END

IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='IX_tb_ProcessNavigationNode_ProcessNavID' AND id=OBJECT_ID('tb_ProcessNavigationNode'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tb_ProcessNavigationNode_ProcessNavID] ON [dbo].[tb_ProcessNavigationNode] ([ProcessNavID] ASC)
    PRINT '创建索引 IX_tb_ProcessNavigationNode_ProcessNavID 成功'
END

IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='IX_tb_ProcessNavigationNode_BusinessType' AND id=OBJECT_ID('tb_ProcessNavigationNode'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tb_ProcessNavigationNode_BusinessType] ON [dbo].[tb_ProcessNavigationNode] ([BusinessType] ASC)
    PRINT '创建索引 IX_tb_ProcessNavigationNode_BusinessType 成功'
END

IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='IX_tb_ProcessNavigationNode_MenuID' AND id=OBJECT_ID('tb_ProcessNavigationNode'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tb_ProcessNavigationNode_MenuID] ON [dbo].[tb_ProcessNavigationNode] ([MenuID] ASC)
    PRINT '创建索引 IX_tb_ProcessNavigationNode_MenuID 成功'
END

-- 5. 创建视图
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='V_tb_ProcessNavigationDetail' AND xtype='V')
BEGIN
    EXEC('
    CREATE VIEW [dbo].[V_tb_ProcessNavigationDetail]
    AS
    SELECT 
        pn.ProcessNavID,
        pn.ProcessNavName,
        pn.Description,
        pn.Version,
        pn.GraphXml,
        pn.GraphJson,
        pn.ModuleID,
        md.ModuleName,
        pn.NavigationLevel,
        pn.HierarchyLevel,
        pn.ParentNavigationID,
        parent.ProcessNavName AS ParentNavigationName,
        pn.CreateUserID,
        pn.IsActive,
        pn.IsDefault,
        pn.Category,
        pn.Tags,
        pn.CreateTime,
        pn.UpdateTime,
        COUNT(pnn.NodeID) AS NodeCount
    FROM [dbo].[tb_ProcessNavigation] pn
    LEFT JOIN [dbo].[tb_ModuleDefinition] md ON pn.ModuleID = md.ModuleID
    LEFT JOIN [dbo].[tb_ProcessNavigation] parent ON pn.ParentNavigationID = parent.ProcessNavID
    LEFT JOIN [dbo].[tb_ProcessNavigationNode] pnn ON pn.ProcessNavID = pnn.ProcessNavID
    WHERE pn.IsActive = 1
    GROUP BY 
        pn.ProcessNavID, pn.ProcessNavName, pn.Description, pn.Version, pn.ModuleID, md.ModuleName,
        pn.NavigationLevel, pn.HierarchyLevel, pn.ParentNavigationID, parent.ProcessNavName, pn.CreateUserID,
        pn.IsActive, pn.IsDefault, pn.Category, pn.Tags, pn.CreateTime, pn.UpdateTime
    ')
    
    PRINT '创建视图 V_tb_ProcessNavigationDetail 成功'
END

-- 6. 创建存储过程
-- 先添加层级结构相关的存储过程
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SP_SetProcessNavigationHierarchy' AND xtype='P')
BEGIN
    EXEC('CREATE PROCEDURE [dbo].[SP_SetProcessNavigationHierarchy]
        @ChildProcessNavID BIGINT,
        @ParentProcessNavID BIGINT = NULL
    AS
    BEGIN
        SET NOCOUNT ON
        BEGIN TRANSACTION
        
        BEGIN TRY
            -- 检查循环引用
            IF @ParentProcessNavID IS NOT NULL
            BEGIN
                -- 检测是否存在循环引用
                DECLARE @HasCycle BIT = 0
                DECLARE @CheckID BIGINT = @ParentProcessNavID
                
                WHILE @CheckID IS NOT NULL
                BEGIN
                    IF @CheckID = @ChildProcessNavID
                    BEGIN
                        SET @HasCycle = 1
                        BREAK
                    END
                    
                    SELECT @CheckID = ParentNavigationID 
                    FROM [dbo].[tb_ProcessNavigation] 
                    WHERE ProcessNavID = @CheckID
                END
                
                IF @HasCycle = 1
                BEGIN
                    RAISERROR(''循环引用检测：不能将父流程设置为子流程的后代'', 16, 1)
                    ROLLBACK TRANSACTION
                    RETURN
                END
                
                -- 检查层级深度限制
                DECLARE @ParentHierarchyLevel INT
                SELECT @ParentHierarchyLevel = HierarchyLevel 
                FROM [dbo].[tb_ProcessNavigation] 
                WHERE ProcessNavID = @ParentProcessNavID
                
                IF @ParentHierarchyLevel >= 4 -- 层级深度最大为4，加上当前流程最多5级
                BEGIN
                    RAISERROR(''层级深度限制：最多支持5级流程嵌套'', 16, 1)
                    ROLLBACK TRANSACTION
                    RETURN
                END
            END
            
            -- 设置父子关系
            UPDATE [dbo].[tb_ProcessNavigation]
            SET 
                ParentNavigationID = @ParentProcessNavID,
                HierarchyLevel = CASE 
                    WHEN @ParentProcessNavID IS NULL THEN 1
                    ELSE (SELECT HierarchyLevel + 1 FROM [dbo].[tb_ProcessNavigation] WHERE ProcessNavID = @ParentProcessNavID)
                END,
                UpdateTime = GETDATE()
            WHERE ProcessNavID = @ChildProcessNavID
            
            -- 更新所有子流程的层级深度
            DECLARE @CurrentLevel INT = (SELECT HierarchyLevel FROM [dbo].[tb_ProcessNavigation] WHERE ProcessNavID = @ChildProcessNavID)
            
            -- 使用递归CTE更新所有子流程
            ;WITH ChildProcesses AS (
                SELECT 
                    ProcessNavID, 
                    @CurrentLevel AS NewLevel
                FROM [dbo].[tb_ProcessNavigation]
                WHERE ParentNavigationID = @ChildProcessNavID
                
                UNION ALL
                
                SELECT 
                    p.ProcessNavID, 
                    c.NewLevel + 1
                FROM [dbo].[tb_ProcessNavigation] p
                JOIN ChildProcesses c ON p.ParentNavigationID = c.ProcessNavID
            )
            UPDATE p
            SET 
                p.HierarchyLevel = c.NewLevel,
                p.UpdateTime = GETDATE()
            FROM [dbo].[tb_ProcessNavigation] p
            JOIN ChildProcesses c ON p.ProcessNavID = c.ProcessNavID
            
            COMMIT TRANSACTION
            PRINT ''设置流程导航图层级关系成功''
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION
            PRINT ''错误：'' + ERROR_MESSAGE()
        END CATCH
    END
    ')
    
    PRINT '创建存储过程 SP_SetProcessNavigationHierarchy 成功'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SP_GetChildProcessNavigations' AND xtype='P')
BEGIN
    EXEC('CREATE PROCEDURE [dbo].[SP_GetChildProcessNavigations]
        @ParentProcessNavID BIGINT,
        @IncludeAllLevels BIT = 0
    AS
    BEGIN
        SET NOCOUNT ON
        
        IF @IncludeAllLevels = 1
        BEGIN
            -- 获取所有层级的子流程（递归）
            ;WITH ChildProcesses AS (
                SELECT 
                    ProcessNavID, 
                    ProcessNavName, 
                    Description, 
                    NavigationLevel, 
                    HierarchyLevel,
                    ParentNavigationID,
                    1 AS LevelDepth
                FROM [dbo].[tb_ProcessNavigation]
                WHERE ParentNavigationID = @ParentProcessNavID AND IsActive = 1
                
                UNION ALL
                
                SELECT 
                    p.ProcessNavID, 
                    p.ProcessNavName, 
                    p.Description, 
                    p.NavigationLevel, 
                    p.HierarchyLevel,
                    p.ParentNavigationID,
                    c.LevelDepth + 1
                FROM [dbo].[tb_ProcessNavigation] p
                JOIN ChildProcesses c ON p.ParentNavigationID = c.ProcessNavID
                WHERE p.IsActive = 1
            )
            SELECT *
            FROM ChildProcesses
            ORDER BY HierarchyLevel, ProcessNavName
        END
        ELSE
        BEGIN
            -- 只获取直接子流程
            SELECT 
                ProcessNavID, 
                ProcessNavName, 
                Description, 
                NavigationLevel, 
                HierarchyLevel,
                ParentNavigationID
            FROM [dbo].[tb_ProcessNavigation]
            WHERE ParentNavigationID = @ParentProcessNavID AND IsActive = 1
            ORDER BY ProcessNavName
        END
    END
    ')
    
    PRINT '创建存储过程 SP_GetChildProcessNavigations 成功'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SP_GetProcessNavigationPath' AND xtype='P')
BEGIN
    EXEC('CREATE PROCEDURE [dbo].[SP_GetProcessNavigationPath]
        @ProcessNavID BIGINT
    AS
    BEGIN
        SET NOCOUNT ON
        
        -- 使用递归CTE获取完整路径
        ;WITH NavigationPath AS (
            SELECT 
                ProcessNavID, 
                ProcessNavName, 
                ParentNavigationID,
                HierarchyLevel,
                CAST(ProcessNavName AS NVARCHAR(MAX)) AS PathName,
                1 AS PathDepth
            FROM [dbo].[tb_ProcessNavigation]
            WHERE ProcessNavID = @ProcessNavID
            
            UNION ALL
            
            SELECT 
                p.ProcessNavID, 
                p.ProcessNavName, 
                p.ParentNavigationID,
                p.HierarchyLevel,
                CAST(p.ProcessNavName + '' > '' + c.PathName AS NVARCHAR(MAX)),
                c.PathDepth + 1
            FROM [dbo].[tb_ProcessNavigation] p
            JOIN NavigationPath c ON p.ProcessNavID = c.ParentNavigationID
        )
        SELECT 
            ProcessNavID, 
            ProcessNavName, 
            ParentNavigationID,
            HierarchyLevel,
            PathName,
            PathDepth
        FROM NavigationPath
        ORDER BY PathDepth DESC
    END
    ')
    
    PRINT '创建存储过程 SP_GetProcessNavigationPath 成功'
END

-- 创建层级关系视图
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='V_tb_ProcessNavigationHierarchy' AND xtype='V')
BEGIN
    EXEC('CREATE VIEW [dbo].[V_tb_ProcessNavigationHierarchy]
    AS
    SELECT 
        p.ProcessNavID,
        p.ProcessNavName,
        p.ParentNavigationID,
        p.HierarchyLevel,
        p.SortOrder,
        p.NavigationLevel,
        p.IsActive,
        p.IsDefault,
        parent.ProcessNavName AS ParentName
    FROM [dbo].[tb_ProcessNavigation] p
    LEFT JOIN [dbo].[tb_ProcessNavigation] parent ON p.ParentNavigationID = parent.ProcessNavID
    WHERE p.IsActive = 1
    ')
    
    PRINT '创建视图 V_tb_ProcessNavigationHierarchy 成功'
END

-- 创建层级结构函数
IF OBJECT_ID('dbo.FN_GetProcessNavigationTreeLevel', 'FN') IS NOT NULL
BEGIN
    DROP FUNCTION dbo.FN_GetProcessNavigationTreeLevel
    PRINT '已删除存在的FN_GetProcessNavigationTreeLevel函数，准备重新创建'
END

EXEC('CREATE FUNCTION [dbo].[FN_GetProcessNavigationTreeLevel]
(@RootProcessNavID BIGINT = NULL)
RETURNS TABLE
AS
RETURN
(
    WITH ProcessTree AS (
        SELECT 
            ProcessNavID,
            ProcessNavName,
            ParentNavigationID,
            HierarchyLevel,
            NavigationLevel,
            CAST(ProcessNavID AS VARCHAR(MAX)) AS PathID,
            CAST(ProcessNavName AS VARCHAR(MAX)) AS PathName,
            0 AS TreeLevel,
            IsActive
        FROM [dbo].[tb_ProcessNavigation]
        WHERE (@RootProcessNavID IS NULL AND ParentNavigationID IS NULL) 
            OR ProcessNavID = @RootProcessNavID
        
        UNION ALL
        
        SELECT 
            p.ProcessNavID,
            p.ProcessNavName,
            p.ParentNavigationID,
            p.HierarchyLevel,
            p.NavigationLevel,
            CAST(t.PathID + ''/'' + CAST(p.ProcessNavID AS VARCHAR(MAX)) AS VARCHAR(MAX)),
            CAST(t.PathName + '' > '' + p.ProcessNavName AS VARCHAR(MAX)),
            t.TreeLevel + 1,
            p.IsActive
        FROM [dbo].[tb_ProcessNavigation] p
        JOIN ProcessTree t ON p.ParentNavigationID = t.ProcessNavID
    )
    SELECT * FROM ProcessTree WHERE IsActive = 1
)
')

PRINT '创建函数 FN_GetProcessNavigationTreeLevel 成功'

-- 继续创建原有存储过程
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SP_GetProcessNavigationByModule' AND xtype='P')
BEGIN
    EXEC('
    CREATE PROCEDURE [dbo].[SP_GetProcessNavigationByModule]
        @ModuleID BIGINT = NULL,
        @NavigationLevel INT = NULL,
        @HierarchyLevel INT = NULL
    AS
    BEGIN
        SET NOCOUNT ON
        
        SELECT 
            pn.ProcessNavID,
            pn.ProcessNavName,
            pn.Description,
            pn.Version,
            pn.ModuleID,
            md.ModuleName,
            pn.NavigationLevel,
            pn.HierarchyLevel,
            pn.ParentNavigationID,
            parent.ProcessNavName AS ParentNavigationName,
            pn.IsActive,
            pn.IsDefault,
            pn.Category,
            pn.Tags,
            pn.CreateTime,
            pn.UpdateTime,
            COUNT(pnn.NodeID) AS NodeCount
        FROM [dbo].[tb_ProcessNavigation] pn
        LEFT JOIN [dbo].[tb_ModuleDefinition] md ON pn.ModuleID = md.ModuleID
        LEFT JOIN [dbo].[tb_ProcessNavigation] parent ON pn.ParentNavigationID = parent.ProcessNavID
        LEFT JOIN [dbo].[tb_ProcessNavigationNode] pnn ON pn.ProcessNavID = pnn.ProcessNavID
        WHERE pn.IsActive = 1
        AND (@ModuleID IS NULL OR pn.ModuleID = @ModuleID)
        AND (@NavigationLevel IS NULL OR pn.NavigationLevel = @NavigationLevel)
        AND (@HierarchyLevel IS NULL OR pn.HierarchyLevel = @HierarchyLevel)
        GROUP BY 
            pn.ProcessNavID, pn.ProcessNavName, pn.Description, pn.Version, pn.ModuleID, md.ModuleName,
            pn.NavigationLevel, pn.HierarchyLevel, pn.ParentNavigationID, parent.ProcessNavName, pn.IsActive,
            pn.IsDefault, pn.Category, pn.Tags, pn.CreateTime, pn.UpdateTime
        ORDER BY pn.HierarchyLevel, pn.IsDefault DESC, pn.ProcessNavName
    END
    ')
    
    PRINT '创建存储过程 SP_GetProcessNavigationByModule 成功'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SP_GetProcessNavigationNodes' AND xtype='P')
BEGIN
    EXEC('
    CREATE PROCEDURE [dbo].[SP_GetProcessNavigationNodes]
        @ProcessNavID BIGINT
    AS
    BEGIN
        SET NOCOUNT ON
        
        SELECT 
            pnn.NodeID,
            pnn.ProcessNavID,
            pnn.NodeCode,
            pnn.NodeName,
            pnn.Description,
            pnn.BusinessType,
            pnn.MenuID,
            mi.MenuName,
            pnn.ModuleID,
            md.ModuleName,
            pnn.ChildNavigationID,
            child.ProcessNavName AS ChildNavigationName,
            pnn.FormName,
            pnn.ClassPath,
            pnn.NodeColor,
            pnn.PositionX,
            pnn.PositionY,
            pnn.Width,
            pnn.Height,
            pnn.NodeType,
            pnn.SortOrder,
            pnn.CreateTime,
            pnn.UpdateTime
        FROM [dbo].[tb_ProcessNavigationNode] pnn
        LEFT JOIN [dbo].[tb_MenuInfo] mi ON pnn.MenuID = mi.MenuID
        LEFT JOIN [dbo].[tb_ModuleDefinition] md ON pnn.ModuleID = md.ModuleID
        LEFT JOIN [dbo].[tb_ProcessNavigation] child ON pnn.ChildNavigationID = child.ProcessNavID
        WHERE pnn.ProcessNavID = @ProcessNavID
        ORDER BY pnn.SortOrder, pnn.NodeID
    END
    ')
    
    PRINT '创建存储过程 SP_GetProcessNavigationNodes 成功'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SP_SetDefaultProcessNavigation' AND xtype='P')
BEGIN
    EXEC('
    CREATE PROCEDURE [dbo].[SP_SetDefaultProcessNavigation]
        @ProcessNavID BIGINT,
        @ModuleID BIGINT = NULL
    AS
    BEGIN
        SET NOCOUNT ON
        BEGIN TRANSACTION
        
        -- 取消同模块或同级别的默认标记
        IF @ModuleID IS NOT NULL
        BEGIN
            UPDATE [dbo].[tb_ProcessNavigation]
            SET IsDefault = 0
            WHERE ModuleID = @ModuleID AND IsActive = 1
        END
        ELSE
        BEGIN
            UPDATE [dbo].[tb_ProcessNavigation]
            SET IsDefault = 0
            WHERE NavigationLevel = (SELECT NavigationLevel FROM [dbo].[tb_ProcessNavigation] WHERE ProcessNavID = @ProcessNavID)
            AND IsActive = 1
        END
        
        -- 设置新的默认
        UPDATE [dbo].[tb_ProcessNavigation]
        SET IsDefault = 1, UpdateTime = GETDATE()
        WHERE ProcessNavID = @ProcessNavID
        
        COMMIT TRANSACTION
    END
    ')
    
    PRINT '创建存储过程 SP_SetDefaultProcessNavigation 成功'
END

-- 7. 创建函数
IF OBJECT_ID('dbo.FN_GetProcessNavigationStats', 'FN') IS NOT NULL
BEGIN
    DROP FUNCTION dbo.FN_GetProcessNavigationStats
    PRINT '已删除存在的FN_GetProcessNavigationStats函数，准备重新创建'
END

EXEC('    
CREATE FUNCTION [dbo].[FN_GetProcessNavigationStats]()
RETURNS TABLE
AS
RETURN
(
    SELECT 
        pn.ProcessNavID,
        pn.ProcessNavName,
        pn.ModuleID,
        md.ModuleName,
        pn.NavigationLevel,
        pn.HierarchyLevel,
        pn.Category,
        COUNT(pnn.NodeID) AS NodeCount,
        pn.CreateTime,
        pn.UpdateTime
    FROM [dbo].[tb_ProcessNavigation] pn
    LEFT JOIN [dbo].[tb_ModuleDefinition] md ON pn.ModuleID = md.ModuleID
    LEFT JOIN [dbo].[tb_ProcessNavigationNode] pnn ON pn.ProcessNavID = pnn.ProcessNavID
    WHERE pn.IsActive = 1
    GROUP BY 
        pn.ProcessNavID, pn.ProcessNavName, pn.ModuleID, md.ModuleName,
        pn.NavigationLevel, pn.HierarchyLevel, pn.Category, pn.CreateTime, pn.UpdateTime
)
')

PRINT '创建函数 FN_GetProcessNavigationStats 成功'

-- 8. 插入示例数据
-- 检查是否已有示例数据
IF NOT EXISTS (SELECT 1 FROM [dbo].[tb_ProcessNavigation] WHERE ProcessNavName LIKE '%ERP%')
BEGIN
    -- 插入总图
    INSERT INTO [dbo].[tb_ProcessNavigation] (
        ProcessNavName, Description, Version, NavigationLevel, HierarchyLevel,
        IsActive, IsDefault, Category, Tags, CreateTime, UpdateTime
    ) VALUES (
        'ERP系统总览图', 'ERP系统主要功能模块总览', 1, 0, 1, 1, 1, 
        '系统总览', 'ERP,总览,主流程', GETDATE(), GETDATE()
    )
    
    DECLARE @TotalNavID BIGINT = SCOPE_IDENTITY()
    
    -- 插入模块级导航图
    INSERT INTO [dbo].[tb_ProcessNavigation] (
        ProcessNavName, Description, Version, ModuleID, NavigationLevel, ParentNavigationID,
        HierarchyLevel, IsActive, IsDefault, Category, Tags, CreateTime, UpdateTime
    ) VALUES 
    ('采购管理流程', '采购管理相关业务流程', 1, NULL, 1, @TotalNavID, 2, 1, 1, '采购管理', '采购,供应商,订单', GETDATE(), GETDATE()),
    ('销售管理流程', '销售管理相关业务流程', 1, NULL, 1, @TotalNavID, 2, 1, 1, '销售管理', '销售,客户,订单', GETDATE(), GETDATE()),
    ('库存管理流程', '库存管理相关业务流程', 1, NULL, 1, @TotalNavID, 2, 1, 1, '库存管理', '库存,盘点,调拨', GETDATE(), GETDATE()),
    ('财务管理流程', '财务管理相关业务流程', 1, NULL, 1, @TotalNavID, 2, 1, 1, '财务管理', '财务,应收,应付', GETDATE(), GETDATE()),
    ('生产管理流程', '生产管理相关业务流程', 1, NULL, 1, @TotalNavID, 2, 1, 1, '生产管理', '生产,计划,制造', GETDATE(), GETDATE())
    
    PRINT '插入示例流程导航图数据成功'
END

-- 9. 验证创建结果
PRINT '============================================'
PRINT '流程导航图功能数据库脚本执行完成'
PRINT '============================================'

-- 10. 为现有数据更新层级深度（如果存在）
IF EXISTS (SELECT * FROM [dbo].[tb_ProcessNavigation] WHERE ProcessNavName LIKE '%ERP%')
BEGIN
    -- 更新已有数据的层级深度
    UPDATE [dbo].[tb_ProcessNavigation]
    SET HierarchyLevel = CASE 
        WHEN ParentNavigationID IS NULL THEN 1
        ELSE 2
    END
    
    PRINT '更新现有数据的层级深度成功'
END

-- 11. 为ParentNavigationID添加字段描述（兼容文档中的命名）
IF NOT EXISTS (SELECT * FROM sys.extended_properties 
               WHERE name = 'MS_Description' 
               AND major_id = OBJECT_ID('dbo.tb_ProcessNavigation') 
               AND minor_id = COLUMNPROPERTY(OBJECT_ID('dbo.tb_ProcessNavigation'), 'ParentNavigationID', 'ColumnId'))
BEGIN
    EXEC sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = '父流程导航图ID（别名：ParentProcessNavID）', 
        @level0type = N'SCHEMA', @level0name = dbo,
        @level1type = N'TABLE',  @level1name = tb_ProcessNavigation,
        @level2type = N'COLUMN', @level2name = ParentNavigationID
    PRINT '添加ParentNavigationID字段描述成功'
END
ELSE
BEGIN
    PRINT 'ParentNavigationID字段描述已存在，跳过添加'
END

IF NOT EXISTS (SELECT * FROM sys.extended_properties 
               WHERE name = 'MS_Description' 
               AND major_id = OBJECT_ID('dbo.tb_ProcessNavigation') 
               AND minor_id = COLUMNPROPERTY(OBJECT_ID('dbo.tb_ProcessNavigation'), 'NavigationLevel', 'ColumnId'))
BEGIN
    EXEC sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = '层级深度（别名：HierarchyLevel）', 
        @level0type = N'SCHEMA', @level0name = dbo,
        @level1type = N'TABLE',  @level1name = tb_ProcessNavigation,
        @level2type = N'COLUMN', @level2name = NavigationLevel
    PRINT '添加NavigationLevel字段描述成功'
END
ELSE
BEGIN
    PRINT 'NavigationLevel字段描述已存在，跳过添加'
END

PRINT '字段描述信息处理完成'

PRINT '建议使用以下命令测试新功能：'
PRINT '1. 查看层级结构：SELECT * FROM V_tb_ProcessNavigationHierarchy'
PRINT '2. 设置层级关系：EXEC SP_SetProcessNavigationHierarchy @ChildProcessNavID=1, @ParentProcessNavID=NULL'
PRINT '3. 获取子流程：EXEC SP_GetChildProcessNavigations @ParentProcessNavID=1, @IncludeAllLevels=1'
PRINT '4. 获取路径：EXEC SP_GetProcessNavigationPath @ProcessNavID=1'
PRINT '5. 获取树结构：SELECT * FROM FN_GetProcessNavigationTreeLevel(NULL)'

-- 显示创建的对象信息
SELECT 
    'Table' AS ObjectType, 
    name AS ObjectName,
    crdate AS CreateDate
FROM sysobjects 
WHERE xtype='U' AND name LIKE '%ProcessNavigation%'
UNION ALL
SELECT 
    'View' AS ObjectType, 
    name AS ObjectName,
    crdate AS CreateDate
FROM sysobjects 
WHERE xtype='V' AND name LIKE '%ProcessNavigation%'
UNION ALL
SELECT 
    'Procedure' AS ObjectType, 
    name AS ObjectName,
    crdate AS CreateDate
FROM sysobjects 
WHERE xtype='P' AND name LIKE '%ProcessNavigation%'
UNION ALL
SELECT 
    'Trigger' AS ObjectType, 
    name AS ObjectName,
    crdate AS CreateDate
FROM sysobjects 
WHERE xtype='TR' AND name LIKE '%ProcessNavigation%'
UNION ALL
SELECT 
    'Function' AS ObjectType, 
    name AS ObjectName,
    crdate AS CreateDate
FROM sysobjects 
WHERE xtype='FN' AND name LIKE '%ProcessNavigation%'
ORDER BY ObjectType, ObjectName

PRINT '数据库对象创建完成！'