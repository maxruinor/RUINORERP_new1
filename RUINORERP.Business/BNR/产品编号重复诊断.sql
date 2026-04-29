-- ============================================
-- 产品编号重复问题诊断 - 快速检查
-- ============================================

-- 1. 查看所有产品类目相关的序列记录
SELECT 
    Id,
    SequenceKey,
    CurrentValue,
    ResetType,
    FormatMask,
    BusinessType,
    LastUpdated,
    CreatedAt
FROM SequenceNumbers
WHERE SequenceKey LIKE 'SEQ_%'
  AND (BusinessType = 'ProductNo' OR SequenceKey LIKE '%CZ%' OR SequenceKey LIKE '%HC%')
ORDER BY LastUpdated DESC;

-- 2. 检查是否有多个相同类目的序列键
SELECT 
    LEFT(SequenceKey, CHARINDEX('_', SequenceKey + '_') - 1) as Prefix,
    SUBSTRING(SequenceKey, 5, LEN(SequenceKey)) as CategoryCode,
    COUNT(*) as RecordCount,
    STRING_AGG(SequenceKey + '(CurrentValue=' + CAST(CurrentValue AS VARCHAR) + ')', ', ') as Records
FROM SequenceNumbers
WHERE SequenceKey LIKE 'SEQ_%'
GROUP BY LEFT(SequenceKey, CHARINDEX('_', SequenceKey + '_') - 1), 
         SUBSTRING(SequenceKey, 5, LEN(SequenceKey))
HAVING COUNT(*) > 1
ORDER BY RecordCount DESC;

-- 3. 查看最近生成的产品编号对应的序列
SELECT TOP 20
    SequenceKey,
    CurrentValue,
    ResetType,
    FORMAT(LastUpdated, 'yyyy-MM-dd HH:mm:ss') as LastUpdateTime
FROM SequenceNumbers
WHERE SequenceKey LIKE 'SEQ_%'
ORDER BY LastUpdated DESC;

-- 4. 检查产品表中是否有重复的产品编号
SELECT 
    ProductNo,
    COUNT(*) as DuplicateCount,
    STRING_AGG(CAST(ProdBaseID AS VARCHAR), ', ') as ProductIDs
FROM tb_Prod
WHERE ProductNo IS NOT NULL
GROUP BY ProductNo
HAVING COUNT(*) > 1
ORDER BY DuplicateCount DESC;

-- 5. 诊断特定类目的序列状态(以车载CZ为例)
DECLARE @CategoryCode NVARCHAR(10) = 'CZ'; -- 修改为需要检查的类目代码

SELECT 
    SequenceKey,
    CurrentValue,
    ResetType,
    FORMAT(LastUpdated, 'yyyy-MM-dd HH:mm:ss') as LastUpdateTime,
    DATEDIFF(SECOND, LastUpdated, GETDATE()) as SecondsAgo
FROM SequenceNumbers
WHERE SequenceKey = 'SEQ_' + @CategoryCode
   OR SequenceKey LIKE 'SEQ_' + @CategoryCode + '_%'
ORDER BY LastUpdated DESC;

-- 6. 检查是否存在序列键不一致的情况
-- (例如: SEQ_CZ 和 SEQ_CZ_NONE 同时存在)
SELECT 
    s1.SequenceKey as Key1,
    s1.CurrentValue as Value1,
    s2.SequenceKey as Key2,
    s2.CurrentValue as Value2,
    s1.LastUpdated as UpdateTime1,
    s2.LastUpdated as UpdateTime2
FROM SequenceNumbers s1
CROSS JOIN SequenceNumbers s2
WHERE s1.Id < s2.Id
  AND s1.SequenceKey LIKE 'SEQ_CZ%'
  AND s2.SequenceKey LIKE 'SEQ_CZ%'
  AND s1.SequenceKey != s2.SequenceKey;
