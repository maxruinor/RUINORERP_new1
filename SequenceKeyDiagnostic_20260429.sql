-- ============================================
-- 序列键一致性诊断脚本
-- 执行时间：2026-04-29
-- 目的：检查编号生成系统的序列键状态
-- ============================================

-- 1. 查看所有序列记录
SELECT 
    '1. 所有序列记录' as 诊断项目,
    Id,
    SequenceKey,
    CurrentValue,
    ResetType,
    FormatMask,
    BusinessType,
    FORMAT(LastUpdated, 'yyyy-MM-dd HH:mm:ss') as LastUpdateTime,
    FORMAT(CreatedAt, 'yyyy-MM-dd HH:mm:ss') as CreatedTime,
    DATEDIFF(SECOND, LastUpdated, GETDATE()) as 秒前更新
FROM SequenceNumbers
ORDER BY LastUpdated DESC;

-- 2. 检查是否有重复的序列键
SELECT 
    '2. 重复的序列键' as 诊断项目,
    SequenceKey, 
    COUNT(*) as RecordCount,
    STRING_AGG(CAST(Id AS VARCHAR) + '(CurrentValue=' + CAST(CurrentValue AS VARCHAR) + ')', ', ') as RecordDetails
FROM SequenceNumbers
GROUP BY SequenceKey
HAVING COUNT(*) > 1
ORDER BY RecordCount DESC;

-- 3. 检查BusinessType为空的记录
SELECT 
    '3. 业务类型为空的序列记录' as 诊断项目,
    Id,
    SequenceKey,
    CurrentValue,
    ResetType,
    BusinessType,
    FORMAT(LastUpdated, 'yyyy-MM-dd HH:mm:ss') as LastUpdateTime
FROM SequenceNumbers
WHERE BusinessType IS NULL OR BusinessType = ''
ORDER BY LastUpdated DESC;

-- 4. 检查异常大的序列值（可能表示问题）
SELECT 
    '4. 异常大的序列值' as 诊断项目,
    SequenceKey,
    CurrentValue,
    ResetType,
    BusinessType,
    FORMAT(LastUpdated, 'yyyy-MM-dd HH:mm:mm') as LastUpdateTime
FROM SequenceNumbers
WHERE CurrentValue > 10000
ORDER BY CurrentValue DESC;

-- 5. 统计各种重置类型的分布
SELECT 
    '5. 重置类型分布统计' as 诊断项目,
    ISNULL(ResetType, 'None') as ResetType,
    COUNT(*) as RecordCount,
    MIN(CurrentValue) as MinValue,
    MAX(CurrentValue) as MaxValue,
    AVG(CAST(CurrentValue AS FLOAT)) as AvgValue
FROM SequenceNumbers
GROUP BY ISNULL(ResetType, 'None')
ORDER BY RecordCount DESC;

-- 6. 检查最近更新的序列记录（最近24小时）
SELECT 
    '6. 最近24小时更新的序列记录' as 诊断项目,
    SequenceKey,
    CurrentValue,
    ResetType,
    BusinessType,
    FORMAT(LastUpdated, 'yyyy-MM-dd HH:mm:ss') as LastUpdateTime,
    DATEDIFF(HOUR, LastUpdated, GETDATE()) as 小时前更新
FROM SequenceNumbers
WHERE LastUpdated > DATEADD(HOUR, -24, GETDATE())
ORDER BY LastUpdated DESC;

-- 7. 检查可能存在序列键不一致的情况
-- （例如：SEQ_CZ 和 SEQ_CZ_NONE 同时存在）
SELECT 
    '7. 可能的序列键不一致' as 诊断项目,
    s1.SequenceKey as Key1,
    s1.CurrentValue as Value1,
    s1.ResetType as ResetType1,
    s2.SequenceKey as Key2,
    s2.CurrentValue as Value2,
    s2.ResetType as ResetType2,
    DATEDIFF(SECOND, s1.LastUpdated, s2.LastUpdated) as 更新时间差秒
FROM SequenceNumbers s1
INNER JOIN SequenceNumbers s2 ON 
    s1.Id < s2.Id AND 
    s1.SequenceKey != s2.SequenceKey AND
    (
        -- 检查相似的序列键（移除后缀部分比较）
        LEFT(s1.SequenceKey, CHARINDEX('_', s1.SequenceKey + '_') - 1) = 
        LEFT(s2.SequenceKey, CHARINDEX('_', s2.SequenceKey + '_') - 1)
        OR
        -- 检查BusinessType相同但SequenceKey不同
        (s1.BusinessType IS NOT NULL AND s2.BusinessType IS NOT NULL AND s1.BusinessType = s2.BusinessType)
    )
ORDER BY s1.SequenceKey, s2.SequenceKey;

-- 8. 统计每个BusinessType的序列键数量
SELECT 
    '8. 按业务类型统计序列键数量' as 诊断项目,
    ISNULL(BusinessType, 'NULL') as BusinessType,
    COUNT(*) as SequenceKeyCount,
    STRING_AGG(SequenceKey, ', ') as SequenceKeys
FROM SequenceNumbers
GROUP BY ISNULL(BusinessType, 'NULL')
HAVING COUNT(*) > 1
ORDER BY COUNT(*) DESC;

-- ============================================
-- 诊断建议：
-- 1. 如果查询2有结果，说明存在重复的序列键，需要清理
-- 2. 如果查询3有结果，说明有序列记录缺少业务类型，修复后的代码应该能解决这个问题
-- 3. 如果查询7有结果，说明可能存在序列键不一致，需要进一步调查
-- 4. 观察查询1中的序列键格式，确保符合预期的命名规范
-- ============================================