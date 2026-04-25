1. 获取指定表的所有外键关系（最实用）
sql
-- 获取表的所有外键（本表 → 引用其他表）
SELECT 
    fk.name AS ForeignKeyName,          -- 外键名称
    sch_parent.name AS ParentSchema,    -- 本表架构
    tp.name AS ParentTableName,         -- 本表（外键所在表）
    cp.name AS ParentColumnName,        -- 外键列
    sch_ref.name AS ReferencedSchema,   -- 引用表架构
    tr.name AS ReferencedTableName,     -- 引用表（主键所在表）
    cr.name AS ReferencedColumnName     -- 引用列（主键列）
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fkc 
    ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.tables AS tp 
    ON fkc.parent_object_id = tp.object_id
INNER JOIN sys.schemas AS sch_parent 
    ON tp.schema_id = sch_parent.schema_id
INNER JOIN sys.columns AS cp 
    ON fkc.parent_object_id = cp.object_id 
    AND fkc.parent_column_id = cp.column_id
INNER JOIN sys.tables AS tr 
    ON fkc.referenced_object_id = tr.object_id
INNER JOIN sys.schemas AS sch_ref 
    ON tr.schema_id = sch_ref.schema_id
INNER JOIN sys.columns AS cr 
    ON fkc.referenced_object_id = cr.object_id 
    AND fkc.referenced_column_id = cr.column_id
WHERE tp.name = '你的表名'  
-- 可选：如果需要精确匹配架构，加上 AND sch_parent.name = 'dbo'
ORDER BY tp.name, fk.name;
2. 获取指定表的所有主键列（支持多列主键）
sql
-- 获取表主键（单列/多列都会正确显示）
SELECT 
    SCHEMA_NAME(t.schema_id) AS TableSchema,
    t.name AS TableName,
    c.name AS PrimaryKeyColumn,
    kc.name AS PrimaryKeyName
FROM sys.key_constraints AS kc
INNER JOIN sys.index_columns AS ic 
    ON kc.parent_object_id = ic.object_id 
    AND kc.unique_index_id = ic.index_id
INNER JOIN sys.columns AS c 
    ON ic.object_id = c.object_id 
    AND ic.column_id = c.column_id
INNER JOIN sys.tables AS t 
    ON kc.parent_object_id = t.object_id
WHERE kc.type = 'PK' 
  AND t.name = '你的表名'
ORDER BY t.name, ic.index_column_id;
3. 查询【本表被哪些表引用】（上游依赖）
sql
-- 谁引用了我？（其他表 → 本表）
SELECT 
    sch_parent.name AS ReferencingSchema,
    parent.name AS ReferencingTableName,  -- 引用我的表
    fc.name AS ReferencingColumnName,     -- 对方外键列
    fk.name AS ForeignKeyName             -- 外键名称
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc 
    ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.tables parent 
    ON fkc.parent_object_id = parent.object_id
INNER JOIN sys.schemas sch_parent 
    ON parent.schema_id = sch_parent.schema_id
INNER JOIN sys.columns fc 
    ON fkc.parent_object_id = fc.object_id 
    AND fkc.parent_column_id = fc.column_id
WHERE fk.referenced_object_id = OBJECT_ID('你的表名')
ORDER BY parent.name;
4. 查询【本表引用了哪些表】（下游依赖）
sql
-- 我引用了谁？（本表 → 其他表）
SELECT 
    sch_ref.name AS ReferencedSchema,
    referenced.name AS ReferencedTableName,  -- 我引用的表
    rc.name AS ReferencedColumnName,        -- 对方主键列
    fk.name AS ForeignKeyName               -- 外键名称
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc 
    ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.tables referenced 
    ON fkc.referenced_object_id = referenced.object_id
INNER JOIN sys.schemas sch_ref 
    ON referenced.schema_id = sch_ref.schema_id
INNER JOIN sys.columns rc 
    ON fkc.referenced_object_id = rc.object_id 
    AND fkc.referenced_column_id = rc.column_id
WHERE fk.parent_object_id = OBJECT_ID('你的表名')
ORDER BY referenced.name;
使用说明
把语句里的 '你的表名' 替换成真实表名即可直接运行。
示例：
sql
WHERE tp.name = 'UserInfo'