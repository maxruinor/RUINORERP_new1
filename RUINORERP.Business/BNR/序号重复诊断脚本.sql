-- ============================================
-- 业务编号重复问题诊断脚本
-- 用于检查和修复SequenceNumbers表中的序列数据
-- ============================================

-- 1. 查看所有销售出库单相关的序列记录
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
WHERE SequenceKey LIKE '%销售出库单%' 
   OR SequenceKey LIKE '%SOD%'
   OR BusinessType = '销售出库单'
ORDER BY LastUpdated DESC;

-- 2. 检查是否有重复的序列键（不同重置类型可能导致）
SELECT 
    SequenceKey,
    COUNT(*) as RecordCount,
    STRING_AGG(CAST(Id AS VARCHAR) + '(' + CAST(CurrentValue AS VARCHAR) + ')', ', ') as Records
FROM SequenceNumbers
GROUP BY SequenceKey
HAVING COUNT(*) > 1
ORDER BY RecordCount DESC;

-- 3. 查看最近生成的所有序列（按更新时间排序）
SELECT TOP 50
    SequenceKey,
    CurrentValue,
    ResetType,
    BusinessType,
    LastUpdated
FROM SequenceNumbers
ORDER BY LastUpdated DESC;

-- 4. 检查销售出库单规则配置
SELECT 
    BillNoRuleID,
    BizType,
    RuleType,
    RulePattern,
    ResetType,
    EncryptionMethod,
    DisplayMode,
    IsActive,
    LastUpdated
FROM tb_sys_BillNoRule
WHERE BizType = 202 -- 假设202是销售出库单的BizType枚举值
   OR RulePattern LIKE '%销售出库单%';

-- 5. 查找可能产生重复编号的序列记录
-- 检查同一业务类型下是否有多个序列键
SELECT 
    BusinessType,
    COUNT(DISTINCT SequenceKey) as DistinctKeys,
    STRING_AGG(SequenceKey + '=' + CAST(CurrentValue AS VARCHAR), '; ') as KeyValues
FROM SequenceNumbers
WHERE BusinessType IS NOT NULL
GROUP BY BusinessType
HAVING COUNT(DISTINCT SequenceKey) > 1
ORDER BY DistinctKeys DESC;

-- 6. 诊断特定日期的序列生成情况
DECLARE @TargetDate DATE = GETDATE(); -- 修改为需要检查的日期

SELECT 
    SequenceKey,
    CurrentValue,
    ResetType,
    FORMAT(LastUpdated, 'yyyy-MM-dd HH:mm:ss') as LastUpdateTime
FROM SequenceNumbers
WHERE CAST(LastUpdated AS DATE) = @TargetDate
  AND (SequenceKey LIKE '%销售出库单%' OR BusinessType = '销售出库单')
ORDER BY LastUpdated;

-- 7. 清理测试数据（谨慎使用！）
-- 仅删除测试或异常的序列记录
/*
DELETE FROM SequenceNumbers
WHERE SequenceKey LIKE '%TEST%'
   OR SequenceKey LIKE '%test%'
   OR (CurrentValue < 0); -- 异常值
*/

-- 8. 重置特定序列到指定值（谨慎使用！）
/*
UPDATE SequenceNumbers
SET CurrentValue = 1,
    LastUpdated = GETDATE()
WHERE SequenceKey = 'SEQ_销售出库单_DAILY'; -- 替换为实际的序列键
*/

-- 9. 查看序列键的完整分布情况
SELECT 
    LEFT(SequenceKey, CHARINDEX('_', SequenceKey + '_') - 1) as Prefix,
    COUNT(*) as Count,
    MIN(CurrentValue) as MinValue,
    MAX(CurrentValue) as MaxValue,
    AVG(CurrentValue) as AvgValue
FROM SequenceNumbers
GROUP BY LEFT(SequenceKey, CHARINDEX('_', SequenceKey + '_') - 1)
ORDER BY Count DESC;

-- 10. 检查是否存在跨天/跨月的序列键不一致问题
SELECT 
    SequenceKey,
    CurrentValue,
    ResetType,
    FORMAT(LastUpdated, 'yyyy-MM-dd') as UpdateDate,
    FORMAT(LastUpdated, 'yyyy-MM') as UpdateMonth
FROM SequenceNumbers
WHERE ResetType IN ('DAILY', 'MONTHLY')
  AND (SequenceKey LIKE '%销售出库单%' OR BusinessType = '销售出库单')
ORDER BY SequenceKey, LastUpdated DESC;
