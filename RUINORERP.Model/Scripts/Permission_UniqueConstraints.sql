-- =============================================
-- RUINORERP 权限体系唯一约束添加脚本
-- 创建日期: 2025-03-27
-- 用途: 防止权限数据重复
-- =============================================

-- 注意：执行前请备份数据库
-- 如果约束已存在, 请先删除再重新创建

-- =============================================
-- 1. tb_P4Button 按钮权限表唯一约束
-- =============================================
-- 检查约束是否存在
IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'UK_tb_P4Button_Role_Button_Menu' 
    AND object_id = OBJECT_ID('tb_P4Button')
)
BEGIN
    -- 清理已存在的重复数据（保留ID最小的）
    DELETE FROM tb_P4Button
    WHERE P4Btn_ID NOT IN (
        SELECT MIN(P4Btn_ID)
        FROM tb_P4Button
        GROUP BY RoleID, ButtonInfo_ID, MenuID
    );

    -- 创建唯一索引
    CREATE UNIQUE INDEX UK_tb_P4Button_Role_Button_Menu 
    ON tb_P4Button(RoleID, ButtonInfo_ID, MenuID);
    
    PRINT 'tb_P4Button 唯一约束创建成功';
END
ELSE
BEGIN
    PRINT 'tb_P4Button 唯一约束已存在';
END

-- =============================================
-- 2. tb_P4Field 字段权限表唯一约束
-- =============================================
IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'UK_tb_P4Field_Role_Field_Menu' 
    AND object_id = OBJECT_ID('tb_P4Field')
)
BEGIN
    -- 清理已存在的重复数据
    DELETE FROM tb_P4Field
    WHERE P4Field_ID NOT IN (
        SELECT MIN(P4Field_ID)
        FROM tb_P4Field
        GROUP BY RoleID, FieldInfo_ID, MenuID
    );

    -- 创建唯一索引
    CREATE UNIQUE INDEX UK_tb_P4Field_Role_Field_Menu 
    ON tb_P4Field(RoleID, FieldInfo_ID, MenuID);
    
    PRINT 'tb_P4Field 唯一约束创建成功';
END
ELSE
BEGIN
    PRINT 'tb_P4Field 唯一约束已存在';
END

-- =============================================
-- 3. tb_P4Menu 菜单权限表唯一约束
-- =============================================
IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'UK_tb_P4Menu_Role_Menu' 
    AND object_id = OBJECT_ID('tb_P4Menu')
)
BEGIN
    -- 清理已存在的重复数据
    DELETE FROM tb_P4Menu
    WHERE P4Menu_ID NOT IN (
        SELECT MIN(P4Menu_ID)
        FROM tb_P4Menu
        GROUP BY RoleID, MenuID
    );

    -- 创建唯一索引
    CREATE UNIQUE INDEX UK_tb_P4Menu_Role_Menu 
    ON tb_P4Menu(RoleID, MenuID);
    
    PRINT 'tb_P4Menu 唯一约束创建成功';
END
ELSE
BEGIN
    PRINT 'tb_P4Menu 唯一约束已存在';
END

-- =============================================
-- 4. tb_P4RowAuthPolicyByRole 行级权限-角色关联表唯一约束
-- =============================================
IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'UK_tb_P4RowAuthPolicyByRole_Role_Policy_Menu' 
    AND object_id = OBJECT_ID('tb_P4RowAuthPolicyByRole')
)
BEGIN
    -- 清理已存在的重复数据
    DELETE FROM tb_P4RowAuthPolicyByRole
    WHERE Policy_Role_RID NOT IN (
        SELECT MIN(Policy_Role_RID)
        FROM tb_P4RowAuthPolicyByRole
        GROUP BY RoleID, PolicyId, MenuID
    );

    -- 创建唯一索引
    CREATE UNIQUE INDEX UK_tb_P4RowAuthPolicyByRole_Role_Policy_Menu 
    ON tb_P4RowAuthPolicyByRole(RoleID, PolicyId, MenuID);
    
    PRINT 'tb_P4RowAuthPolicyByRole 唯一约束创建成功';
END
ELSE
BEGIN
    PRINT 'tb_P4RowAuthPolicyByRole 唯一约束已存在';
END

-- =============================================
-- 5. 验证约束创建结果
-- =============================================
SELECT 
    t.name AS TableName,
    i.name AS IndexName,
    i.is_unique AS IsUnique,
    i.type_desc AS IndexType
FROM sys.indexes i
INNER JOIN sys.tables t ON i.object_id = t.object_id
WHERE i.name LIKE 'UK_tb_P4%'
ORDER BY t.name;

PRINT '权限体系唯一约束添加完成';

-- =============================================
-- 回滚脚本（如需要删除约束）
-- =============================================
/*
-- 删除 tb_P4Button 唯一约束
IF EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'UK_tb_P4Button_Role_Button_Menu' 
    AND object_id = OBJECT_ID('tb_P4Button')
)
BEGIN
    DROP INDEX UK_tb_P4Button_Role_Button_Menu ON tb_P4Button;
    PRINT 'tb_P4Button 唯一约束已删除';
END

-- 删除 tb_P4Field 唯一约束
IF EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'UK_tb_P4Field_Role_Field_Menu' 
    AND object_id = OBJECT_ID('tb_P4Field')
)
BEGIN
    DROP INDEX UK_tb_P4Field_Role_Field_Menu ON tb_P4Field;
    PRINT 'tb_P4Field 唯一约束已删除';
END

-- 删除 tb_P4Menu 唯一约束
IF EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'UK_tb_P4Menu_Role_Menu' 
    AND object_id = OBJECT_ID('tb_P4Menu')
)
BEGIN
    DROP INDEX UK_tb_P4Menu_Role_Menu ON tb_P4Menu;
    PRINT 'tb_P4Menu 唯一约束已删除';
END

-- 删除 tb_P4RowAuthPolicyByRole 唯一约束
IF EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'UK_tb_P4RowAuthPolicyByRole_Role_Policy_Menu' 
    AND object_id = OBJECT_ID('tb_P4RowAuthPolicyByRole')
)
BEGIN
    DROP INDEX UK_tb_P4RowAuthPolicyByRole_Role_Policy_Menu ON tb_P4RowAuthPolicyByRole;
    PRINT 'tb_P4RowAuthPolicyByRole 唯一约束已删除';
END
*/