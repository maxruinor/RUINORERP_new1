using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SqlSugar;

namespace RUINORERP.Business.BNR
{
    /// <summary>
    /// 数据库序号管理服务
    /// 提供序号的生成、查询、重置等管理功能
    /// </summary>
    public class DatabaseSequenceService : IDisposable
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        // ✅ 方案B: 预分配批次缓存 - 每个序列键维护一个可用序号范围
        // Key: sequenceKey, Value: (当前值, 批次上限值)
        private readonly ConcurrentDictionary<string, SequenceBatchCache> _batchCaches = new ConcurrentDictionary<string, SequenceBatchCache>();

        // 按键分片锁,用于控制对同一序列键的并发访问
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _keyLocks = new ConcurrentDictionary<string, SemaphoreSlim>();

        // 批量获取配置
        private const int BATCH_SIZE = 10; // ✅ 优化:每次从数据库预取10个序号(并发用户少,减小批次避免重启时浪费)

        /// <summary>
        /// 构造函数 - 简化版,移除后台任务
        /// </summary>
        /// <param name="sqlSugarClient">SqlSugar客户端实例</param>
        public DatabaseSequenceService(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
            // ✅ 不再启动后台刷写任务,所有操作实时写入数据库
        }

        /// <summary>
        /// 序号批次缓存 - 线程安全的序号分配器
        /// </summary>
        private class SequenceBatchCache
        {
            private long _currentValue;    // 当前已分配的序号
            private long _batchUpperLimit; // 当前批次的上限值
            private readonly object _lock = new object();

            public SequenceBatchCache(long initialValue, long upperLimit)
            {
                _currentValue = initialValue;
                _batchUpperLimit = upperLimit;
            }

            /// <summary>
            /// 尝试从缓存中分配一个序号
            /// </summary>
            /// <param name="nextValue">输出的下一个序号</param>
            /// <returns>是否成功分配(true=有可用序号, false=需要重新从数据库获取批次)</returns>
            public bool TryAllocate(out long nextValue)
            {
                lock (_lock)
                {
                    if (_currentValue < _batchUpperLimit)
                    {
                        nextValue = ++_currentValue;
                        return true;
                    }
                    else
                    {
                        nextValue = 0;
                        return false; // 批次耗尽
                    }
                }
            }

            /// <summary>
            /// 更新批次范围
            /// </summary>
            public void UpdateBatch(long newValue, long newUpperLimit)
            {
                lock (_lock)
                {
                    _currentValue = newValue;
                    _batchUpperLimit = newUpperLimit;
                }
            }

            /// <summary>
            /// 获取当前值(用于监控)
            /// </summary>
            public long CurrentValue
            {
                get { lock (_lock) return _currentValue; }
            }
        }

        

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                // 释放所有信号量锁
                foreach (var kvp in _keyLocks)
                {
                    kvp.Value.Dispose();
                }
                _keyLocks.Clear();

                System.Diagnostics.Debug.WriteLine("DatabaseSequenceService 资源已释放");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"释放 DatabaseSequenceService 资源时出错: {ex.Message}");
                LogError("资源释放失败", ex);
            }
        }

        




        /// <summary>
        /// 生成动态键
        /// 优化后：保持键的规范性，同时确保按时间重置功能正常工作
        /// </summary>
        /// <param name="key">基础键</param>
        /// <param name="resetType">重置类型</param>
        /// <returns>动态键</returns>
        private string GenerateDynamicKey(string key, string resetType)
        {
            string dynamicKey = key;

            if (!string.IsNullOrEmpty(resetType))
            {
                switch (resetType.ToUpper())
                {
                    case "DAILY":
                        dynamicKey = $"{key}_{DateTime.Now.ToString("yyyyMMdd")}";
                        break;
                    case "MONTHLY":
                        dynamicKey = $"{key}_{DateTime.Now.ToString("yyyyMM")}";
                        break;
                    case "YEARLY":
                        dynamicKey = $"{key}_{DateTime.Now.ToString("yyyy")}";
                        break;
                    case "NONE":
                        // 对于NONE类型，保持原始键不变
                        break;
                }
            }

            // 确保键格式规范，避免特殊字符
            dynamicKey = System.Text.RegularExpressions.Regex.Replace(dynamicKey, "[^a-zA-Z0-9_一-龥]", "_");

            return dynamicKey;
        }

        /// <summary>
        /// 获取当前序列值
        /// </summary>
        /// <param name="sequenceKey">序列键</param>
        /// <returns>当前序列值，如果不存在返回0</returns>
        public long GetCurrentSequenceValue(string sequenceKey)
        {
            try
            {
                var sequence = _sqlSugarClient.Queryable<SequenceNumbers>()
                    .Where(s => s.SequenceKey == sequenceKey)
                    .First();

                return sequence?.CurrentValue ?? 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取当前序列值失败: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// 更新指定序列的值
        /// 修复：移除事务锁，使用重试机制处理并发冲突
        /// </summary>
        /// <param name="sequenceKey">序列键</param>
        /// <param name="newValue">新值</param>
        /// <param name="businessType">业务类型</param>
        public void UpdateSequenceValue(string sequenceKey, long newValue, string businessType = null)
        {
            // 使用重试机制处理并发冲突
            int retryCount = 0;
            int maxRetries = 3;
            bool success = false;

            while (!success && retryCount < maxRetries)
            {
                try
                {
                    using (var tran = _sqlSugarClient.Ado.UseTran())
                    {
                        var sequence = _sqlSugarClient.Queryable<SequenceNumbers>()
                            .Where(s => s.SequenceKey == sequenceKey)
                            .First();

                        if (sequence != null)
                        {
                            sequence.CurrentValue = newValue;
                            sequence.LastUpdated = DateTime.Now;
                            // 如果业务类型不为空且原记录没有业务类型，则更新业务类型
                            if (!string.IsNullOrEmpty(businessType) && string.IsNullOrEmpty(sequence.BusinessType))
                            {
                                sequence.BusinessType = businessType;
                            }
                            _sqlSugarClient.Updateable(sequence).ExecuteCommand();
                        }
                        else
                        {
                            var newSequence = new SequenceNumbers
                            {
                                SequenceKey = sequenceKey,
                                CurrentValue = newValue,
                                LastUpdated = DateTime.Now,
                                CreatedAt = DateTime.Now,
                                BusinessType = businessType
                            };
                            _sqlSugarClient.Insertable(newSequence).ExecuteCommand();
                        }

                        // 提交事务
                        tran.CommitTran();
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    // 如果是并发事务冲突，尝试重试
                    if (ex.Message.Contains("不允许启动新事务") || ex.Message.Contains("already in progress"))
                    {
                        retryCount++;
                        if (retryCount < maxRetries)
                        {
                            System.Diagnostics.Debug.WriteLine($"UpdateSequenceValue 检测到并发事务冲突，等待后重试 ({retryCount}/{maxRetries})");
                            Thread.Sleep(50 * retryCount); // 递增等待时间
                            continue;
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"更新序列值失败: {ex.Message}");
                    throw;
                }
            }

            if (!success)
            {
                throw new Exception($"更新序列值失败，已达到最大重试次数 ({maxRetries})");
            }
        }

        /// <summary>
        /// 获取下一个序列值(支持按时间单位重置)
        /// ✅ 方案B: 使用预分配批次缓存,减少数据库访问频率
        /// </summary>
        /// <param name="sequenceKey">序列键</param>
        /// <param name="resetType">重置类型(None、Daily、Monthly、Yearly)</param>
        /// <param name="formatMask">格式掩码</param>
        /// <param name="description">描述</param>
        /// <param name="businessType">业务类型</param>
        /// <returns>下一个序列值</returns>
        public async Task<long> GetNextSequenceValueAsync(string sequenceKey, string resetType = "None", string formatMask = null, string description = null, string businessType = null)
        {
            if (string.IsNullOrEmpty(sequenceKey))
            {
                throw new ArgumentNullException(nameof(sequenceKey), "序列键不能为空");
            }

            string dynamicKey = GenerateDynamicKey(sequenceKey, resetType);

            // ✅ 优化：第一次快速检查，避免不必要的锁获取
            if (_batchCaches.TryGetValue(dynamicKey, out var fastCache))
            {
                if (fastCache.TryAllocate(out long fastNextValue))
                {
                    System.Diagnostics.Debug.WriteLine($"[缓存快速命中] 键: {dynamicKey}, 值: {fastNextValue}");
                    return fastNextValue;
                }
            }

            // 获取或创建该键的信号量锁(异步友好)
            var keyLock = _keyLocks.GetOrAdd(dynamicKey, _ => new SemaphoreSlim(1, 1));

            // ✅ 修复: 添加超时机制防止永久阻塞（编号生成应在毫秒级完成）
            const int LOCK_TIMEOUT_SECONDS = 5;
            bool lockAcquired = false;
            try
            {
                lockAcquired = await keyLock.WaitAsync(TimeSpan.FromSeconds(LOCK_TIMEOUT_SECONDS));
                if (!lockAcquired)
                {
                    throw new TimeoutException(
                        $"获取序列键锁超时 ({LOCK_TIMEOUT_SECONDS}秒): {dynamicKey}。 " +
                        $"可能原因: 1) 极高并发竞争 2) 持有锁的线程异常卡死 3) 数据库死锁。 " +
                        $"建议: 检查系统负载、数据库状态及是否有长时间运行的事务。");
                }

                // ✅ 双重检查锁定：再次检查缓存，防止其他线程已经重新分配了批次
                if (_batchCaches.TryGetValue(dynamicKey, out var cache))
                {
                    if (cache.TryAllocate(out long nextValue))
                    {
                        // 缓存命中,直接返回
                        System.Diagnostics.Debug.WriteLine($"[缓存命中] 键: {dynamicKey}, 值: {nextValue}");
                        return nextValue;
                    }
                }

                // 缓存未命中或已耗尽,从数据库获取新批次
                return await AllocateNewBatchAsync(dynamicKey, resetType, formatMask, description, businessType);
            }
            finally
            {
                // ✅ 只有成功获取锁时才释放
                if (lockAcquired)
                {
                    keyLock.Release();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[警告] 锁获取超时,无需释放: {dynamicKey}");
                }
            }
        }

        /// <summary>
        /// 同步版本(兼容旧代码)
        /// </summary>
        public long GetNextSequenceValue(string sequenceKey, string resetType = "None", string formatMask = null, string description = null, string businessType = null)
        {
            return GetNextSequenceValueAsync(sequenceKey, resetType, formatMask, description, businessType).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 从数据库分配新的序号批次
        /// ✅ 核心逻辑:一次性获取BATCH_SIZE个序号,缓存在内存中
        /// ✅ 修复:增强序列键一致性检查,防止因键不一致导致的重复
        /// </summary>
        private async Task<long> AllocateNewBatchAsync(string sequenceKey, string resetType,
            string formatMask, string description, string businessType)
        {
            if (description == null)
            {
                description = string.Empty;
            }

            int retryCount = 0;
            const int maxRetries = 5;

            while (retryCount < maxRetries)
            {
                try
                {
                    // ✅ 关键修复:在事务开始前记录序列键,用于调试和验证
                    System.Diagnostics.Debug.WriteLine($"[分配批次] 开始处理序列键: '{sequenceKey}', 重置类型: '{resetType}'");
                    
                    // 使用事务确保原子性
                    using (var tran = _sqlSugarClient.Ado.UseTran())
                    {
                        // 1. 查询或创建序列记录
                        var sequence = _sqlSugarClient.Ado.SqlQuery<SequenceNumbers>(
                            "SELECT * FROM SequenceNumbers WITH(UPDLOCK, ROWLOCK, HOLDLOCK) " +
                            "WHERE SequenceKey = @SequenceKey",
                            new { SequenceKey = sequenceKey })
                            .FirstOrDefault();

                        long batchStartValue;
                        long batchEndValue;

                        if (sequence != null)
                        {
                            // ✅ 记录存在,计算新批次范围
                            batchStartValue = sequence.CurrentValue;
                            batchEndValue = batchStartValue + BATCH_SIZE;
                            
                            System.Diagnostics.Debug.WriteLine(
                                $"[分配批次] 找到现有记录: 键='{sequenceKey}', 当前值={batchStartValue}, 新批次上限={batchEndValue}");

                            // 更新数据库中的当前值到批次上限
                            int affectedRows = _sqlSugarClient.Updateable<SequenceNumbers>()
                                .SetColumns(s => new SequenceNumbers
                                {
                                    CurrentValue = batchEndValue,
                                    LastUpdated = DateTime.Now
                                })
                                .Where(s => s.SequenceKey == sequenceKey)
                                .ExecuteCommand();

                            if (affectedRows == 0)
                            {
                                throw new Exception($"更新序列失败: {sequenceKey}");
                            }
                        }
                        else
                        {
                            // ✅ 记录不存在,创建新记录
                            batchStartValue = 0;
                            batchEndValue = BATCH_SIZE;
                            
                            System.Diagnostics.Debug.WriteLine(
                                $"[分配批次] 创建新记录: 键='{sequenceKey}', 初始批次范围 1-{batchEndValue}");

                            try
                            {
                                _sqlSugarClient.Insertable(new SequenceNumbers
                                {
                                    SequenceKey = sequenceKey,
                                    CurrentValue = batchEndValue,
                                    LastUpdated = DateTime.Now,
                                    CreatedAt = DateTime.Now,
                                    ResetType = resetType,
                                    FormatMask = formatMask,
                                    Description = description,
                                    BusinessType = businessType
                                }).ExecuteCommand();
                            }
                            catch (Exception ex) when (IsUniqueConstraintViolation(ex))
                            {
                                // ✅ 并发插入,回滚后重试
                                tran.RollbackTran();
                                retryCount++;
                                System.Diagnostics.Debug.WriteLine(
                                    $"[分配批次] 并发插入检测到重复键,重试中 ({retryCount}/{maxRetries}),键: {sequenceKey}");
                                await Task.Delay(CalculateBackoffDelay(retryCount));
                                continue;
                            }
                        }

                        // 2. 提交事务
                        tran.CommitTran();

                        // 3. ✅ 关键修复:原子性更新或创建缓存
                        //    注意:服务器重启后,_batchCaches为空,这里会创建新的缓存实例
                        //    但因为数据库中的CurrentValue已经更新到batchEndValue,所以不会重复
                        var newCache = new SequenceBatchCache(batchStartValue, batchEndValue);
                        var existingCache = _batchCaches.AddOrUpdate(
                            sequenceKey,
                            newCache,  // 键不存在时插入
                            (key, oldCache) => 
                            {
                                // ✅ 如果缓存已存在,说明有其他线程已经分配了批次
                                // 不应该替换,应该抛出异常或警告
                                System.Diagnostics.Debug.WriteLine(
                                    $"[警告] 序列键 '{sequenceKey}' 的缓存已存在,旧批次: ({oldCache.CurrentValue}), 新批次: ({batchStartValue})");
                                return newCache; // 仍然使用新批次,因为数据库已更新
                            }
                        );

                        // 4. 返回批次中的第一个序号
                        long nextValue = batchStartValue + 1;
                        System.Diagnostics.Debug.WriteLine(
                            $"[新批次] 键: {sequenceKey}, 范围: {batchStartValue + 1}-{batchEndValue}, 返回: {nextValue}");

                        return nextValue;
                    }
                }
                catch (System.Data.SqlClient.SqlException ex) when (ex.Number == 1205) // 死锁
                {
                    retryCount++;
                    System.Diagnostics.Debug.WriteLine(
                        $"[分配批次] 检测到死锁,重试中 ({retryCount}/{maxRetries}),键: {sequenceKey}");
                    await Task.Delay(CalculateBackoffDelay(retryCount));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[分配批次] 发生异常: {ex.Message},键: {sequenceKey}");
                    LogError($"分配批次失败: {sequenceKey}", ex);

                    if (retryCount >= maxRetries - 1)
                    {
                        throw;
                    }

                    retryCount++;
                    await Task.Delay(CalculateBackoffDelay(retryCount));
                }
            }

            throw new Exception($"分配批次失败,已达到最大重试次数 {maxRetries},键: {sequenceKey}");
        }

        /// <summary>
        /// 从数据库获取当前序列值
        /// </summary>
        /// <param name="sequenceKey">序列键</param>
        /// <returns>当前序列值，如果不存在返回0</returns>
        private long GetCurrentSequenceValueFromDb(string sequenceKey)
        {
            try
            {
                var sequence = _sqlSugarClient.Queryable<SequenceNumbers>()
                    .Where(s => s.SequenceKey == sequenceKey)
                    .First();

                return sequence?.CurrentValue ?? 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从数据库获取序列值失败: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// 获取下一个序列值（旧版本，兼容使用）
        /// </summary>
        /// <param name="sequenceKey">序列键</param>
        /// <returns>下一个序列值</returns>
        public long GetNextSequenceValue(string sequenceKey)
        {
            return GetNextSequenceValue(sequenceKey, "None", null, null, null);
        }

        /// <summary>
        /// 重置指定序列的值为1
        /// 修复：移除事务锁，使用重试机制处理并发冲突
        /// </summary>
        /// <param name="key">序列键</param>
        public void ResetSequence(string key)
        {
            // 使用重试机制处理并发冲突
            int retryCount = 0;
            int maxRetries = 3;
            bool success = false;

            while (!success && retryCount < maxRetries)
            {
                try
                {
                    using (var tran = _sqlSugarClient.Ado.UseTran())
                    {
                        // 查找所有匹配的序列记录（包括动态键）
                        var sequences = _sqlSugarClient.Queryable<SequenceNumbers>()
                            .Where(s => s.SequenceKey == key || s.SequenceKey.StartsWith($"{key}_"))
                            .ToList();

                        if (sequences.Count == 0)
                        {
                            throw new Exception($"未找到序列键 '{key}' 相关的记录");
                        }

                        foreach (var sequence in sequences)
                        {
                            sequence.CurrentValue = 1;
                            sequence.LastUpdated = DateTime.Now;
                            _sqlSugarClient.Updateable(sequence).ExecuteCommand();
                        }

                        // 提交事务
                        tran.CommitTran();
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    // 如果是并发事务冲突，尝试重试
                    if (ex.Message.Contains("不允许启动新事务") || ex.Message.Contains("already in progress"))
                    {
                        retryCount++;
                        if (retryCount < maxRetries)
                        {
                            System.Diagnostics.Debug.WriteLine($"ResetSequence 检测到并发事务冲突，等待后重试 ({retryCount}/{maxRetries})");
                            Thread.Sleep(50 * retryCount); // 递增等待时间
                            continue;
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"重置序列失败: {ex.Message}");
                    throw;
                }
            }

            if (!success)
            {
                throw new Exception($"重置序列失败，已达到最大重试次数 ({maxRetries})");
            }
        }

        /// <summary>
        /// 获取当前序号值（不增加）
        /// </summary>
        /// <param name="key">序号键</param>
        /// <param name="resetType">重置类型</param>
        /// <returns>当前序号值</returns>
        public long GetCurrentSequenceValue(string key, string resetType = "None")
        {
            string dynamicKey = GenerateDynamicKey(key, resetType);

            var sequence = _sqlSugarClient.Queryable<SequenceNumbers>()
                .Where(s => s.SequenceKey == dynamicKey)
                .First();

            return sequence?.CurrentValue ?? 0;
        }

        /// <summary>
        /// 获取所有序列
        /// </summary>
        /// <returns>序列列表</returns>
        public List<SequenceNumbers> GetAllSequences()
        {
            return _sqlSugarClient.Queryable<SequenceNumbers>()
                .OrderBy(s => s.Id)
                .ToList();
        }

        /// <summary>
        /// 根据业务类型获取序列
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <returns>序列列表</returns>
        public List<SequenceNumbers> GetSequencesByBusinessType(string businessType)
        {
            return _sqlSugarClient.Queryable<SequenceNumbers>()
                .Where(s => s.BusinessType == businessType)
                .OrderBy(s => s.Id)
                .ToList();
        }

        /// <summary>
        /// 更新序列信息
        /// 修复：移除事务锁，使用重试机制处理并发冲突
        /// </summary>
        /// <param name="key">序列键</param>
        /// <param name="resetType">重置类型</param>
        /// <param name="formatMask">格式掩码</param>
        /// <param name="description">描述</param>
        /// <param name="businessType">业务类型</param>
        public void UpdateSequenceInfo(string key, string resetType = null, string formatMask = null, string description = null, string businessType = null)
        {
            // 使用重试机制处理并发冲突
            int retryCount = 0;
            int maxRetries = 3;
            bool success = false;

            while (!success && retryCount < maxRetries)
            {
                try
                {
                    using (var tran = _sqlSugarClient.Ado.UseTran())
                    {
                        var sequence = _sqlSugarClient.Queryable<SequenceNumbers>()
                            .Where(s => s.SequenceKey == key)
                            .First();

                        if (sequence == null)
                        {
                            throw new Exception($"未找到序列键 '{key}' 的记录");
                        }

                        // 更新非空字段
                        if (!string.IsNullOrEmpty(resetType))
                            sequence.ResetType = resetType;

                        if (!string.IsNullOrEmpty(formatMask))
                            sequence.FormatMask = formatMask;

                        if (!string.IsNullOrEmpty(description))
                            sequence.Description = description;

                        if (!string.IsNullOrEmpty(businessType))
                            sequence.BusinessType = businessType;

                        sequence.LastUpdated = DateTime.Now;

                        _sqlSugarClient.Updateable(sequence).ExecuteCommand();

                        // 提交事务
                        tran.CommitTran();
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    // 如果是并发事务冲突，尝试重试
                    if (ex.Message.Contains("不允许启动新事务") || ex.Message.Contains("already in progress"))
                    {
                        retryCount++;
                        if (retryCount < maxRetries)
                        {
                            System.Diagnostics.Debug.WriteLine($"UpdateSequenceInfo 检测到并发事务冲突，等待后重试 ({retryCount}/{maxRetries})");
                            Thread.Sleep(50 * retryCount); // 递增等待时间
                            continue;
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"更新序列信息失败: {ex.Message}");
                    throw;
                }
            }

            if (!success)
            {
                throw new Exception($"更新序列信息失败，已达到最大重试次数 ({maxRetries})");
            }
        }

        /// <summary>
        /// 重置序列号到指定值
        /// 修复：移除事务锁，使用重试机制处理并发冲突
        /// </summary>
        /// <param name="key">序列号键</param>
        /// <param name="newValue">新的值</param>
        /// <param name="businessType">业务类型</param>
        public void ResetSequenceValue(string key, long newValue, string businessType = null)
        {
            // 使用重试机制处理并发冲突
            int retryCount = 0;
            int maxRetries = 3;
            bool success = false;

            while (!success && retryCount < maxRetries)
            {
                try
                {
                    using (var tran = _sqlSugarClient.Ado.UseTran())
                    {
                        var sequence = _sqlSugarClient.Queryable<SequenceNumbers>()
                            .Where(s => s.SequenceKey == key)
                            .First();

                        if (sequence != null)
                        {
                            sequence.CurrentValue = newValue;
                            sequence.LastUpdated = DateTime.Now;
                            // 如果业务类型不为空且原记录没有业务类型，则更新业务类型
                            if (!string.IsNullOrEmpty(businessType) && string.IsNullOrEmpty(sequence.BusinessType))
                            {
                                sequence.BusinessType = businessType;
                            }
                            _sqlSugarClient.Updateable(sequence).ExecuteCommand();
                        }
                        else
                        {
                            var newSequence = new SequenceNumbers
                            {
                                SequenceKey = key,
                                CurrentValue = newValue,
                                LastUpdated = DateTime.Now,
                                CreatedAt = DateTime.Now,
                                BusinessType = businessType
                            };
                            _sqlSugarClient.Insertable(newSequence).ExecuteCommand();
                        }

                        // 提交事务
                        tran.CommitTran();
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    // 如果是并发事务冲突，尝试重试
                    if (ex.Message.Contains("不允许启动新事务") || ex.Message.Contains("already in progress"))
                    {
                        retryCount++;
                        if (retryCount < maxRetries)
                        {
                            System.Diagnostics.Debug.WriteLine($"ResetSequenceValue 检测到并发事务冲突，等待后重试 ({retryCount}/{maxRetries})");
                            Thread.Sleep(50 * retryCount); // 递增等待时间
                            continue;
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"重置序列值失败: {ex.Message}");
                    throw;
                }
            }

            if (!success)
            {
                throw new Exception($"重置序列值失败，已达到最大重试次数 ({maxRetries})");
            }
        }

        /// <summary>
        /// 删除序号记录
        /// </summary>
        /// <param name="key">序号键</param>
        public void DeleteSequence(string key)
        {
            _sqlSugarClient.Ado.ExecuteCommand(
                "DELETE FROM SequenceNumbers WHERE SequenceKey = @SequenceKey",
                new { SequenceKey = key });
        }

        /// <summary>
        /// 检查序号是否存在
        /// </summary>
        /// <param name="key">序号键</param>
        /// <returns>是否存在</returns>
        public bool SequenceExists(string key)
        {
            var countValue = _sqlSugarClient.Ado.GetScalar(
                "SELECT COUNT(1) FROM SequenceNumbers WHERE SequenceKey = @SequenceKey",
                new { SequenceKey = key });
            int count = Convert.ToInt32(countValue);

            return count > 0;
        }

        /// <summary>
        /// 测试序号表功能
        /// 用于验证表结构是否正确创建及序列生成是否正常
        /// </summary>
        /// <returns>测试结果信息</returns>
        public string TestSequenceTable()
        {
            try
            {
                StringBuilder result = new StringBuilder();

                // 检查表是否存在
                if (_sqlSugarClient.DbMaintenance.IsAnyTable("SequenceNumbers"))
                {
                    result.AppendLine("序号表存在");
                }
                else
                {
                    return "错误：序号表不存在！";
                }

                // 测试生成序号
                string testKey = "TEST_KEY_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                long sequenceValue = GetNextSequenceValue(testKey);
                result.AppendLine($"生成测试序号成功：{testKey} = {sequenceValue}");

                // 验证序号已保存
                if (SequenceExists(testKey))
                {
                    result.AppendLine("序号记录已正确保存到数据库");

                    // 清理测试数据
                    DeleteSequence(testKey);
                    result.AppendLine("测试数据已清理");
                }
                else
                {
                    result.AppendLine("警告：序号记录未保存到数据库");
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                return $"测试失败：{ex.Message}";
            }
        }

        /// <summary>
        /// ✅ 已废弃: 批次缓存模式下不再需要批量更新阈值
        /// </summary>
        [Obsolete("批次缓存模式下不再使用此方法", false)]
        public static void SetBatchUpdateThreshold(int threshold)
        {
            // 不再使用,保留方法签名以兼容旧代码
            System.Diagnostics.Debug.WriteLine($"警告: SetBatchUpdateThreshold 已废弃");
        }

        /// <summary>
        /// ✅ 已废弃: 批次缓存模式下不再需要批量更新阈值
        /// </summary>
        [Obsolete("批次缓存模式下不再使用此方法", false)]
        public static int GetBatchUpdateThreshold()
        {
            return 50; // 返回固定的BATCH_SIZE
        }

        /// <summary>
        /// 获取服务健康状态(✅ 适配批次缓存模式)
        /// </summary>
        /// <returns>健康检查结果</returns>
        public SequenceServiceHealthInfo GetHealthInfo()
        {
            return new SequenceServiceHealthInfo
            {
                IsHealthy = true,
                CacheSize = _batchCaches.Count, // ✅ 改为批次缓存数量
                QueueSize = 0, // ✅ 不再有更新队列
                IsFlushing = false, // ✅ 不再有刷写状态
                LastFlushTime = DateTime.Now,
                BatchThreshold = BATCH_SIZE // ✅ 返回固定的批次大小
            };
        }

        /// <summary>
        /// ✅ 已废弃: 批次缓存模式下不需要手动刷写
        /// </summary>
        [Obsolete("批次缓存模式下数据实时写入数据库,无需手动刷写", false)]
        public void ForceFlushCacheValue(string key, long value, string reason = "ManualFlush")
        {
            // 不再使用,所有数据已实时写入数据库
            System.Diagnostics.Debug.WriteLine($"警告: ForceFlushCacheValue 已废弃");
        }

        /// <summary>
        /// 诊断序列键冲突问题(✅ 适配批次缓存模式)
        /// </summary>
        /// <param name="sequenceKey">要诊断的序列键</param>
        /// <returns>诊断结果</returns>
        public SequenceConflictDiagnosis DiagnoseSequenceConflict(string sequenceKey)
        {
            var diagnosis = new SequenceConflictDiagnosis
            {
                SequenceKey = sequenceKey,
                Timestamp = DateTime.Now
            };

            try
            {
                // 检查数据库中是否存在该键
                var dbRecord = _sqlSugarClient.Queryable<SequenceNumbers>()
                    .Where(s => s.SequenceKey == sequenceKey)
                    .First();

                diagnosis.ExistsInDatabase = dbRecord != null;
                if (dbRecord != null)
                {
                    diagnosis.DatabaseValue = dbRecord.CurrentValue;
                    diagnosis.LastUpdated = dbRecord.LastUpdated;
                }

                // ✅ 检查批次缓存中是否存在
                diagnosis.ExistsInCache = _batchCaches.ContainsKey(sequenceKey);
                if (diagnosis.ExistsInCache && _batchCaches.TryGetValue(sequenceKey, out var cache))
                {
                    diagnosis.CacheValue = cache.CurrentValue;
                }

                // ✅ 批次缓存模式下没有待处理更新
                diagnosis.PendingUpdates = 0;

                // 分析冲突原因
                if (diagnosis.ExistsInDatabase && diagnosis.ExistsInCache)
                {
                    if (diagnosis.DatabaseValue >= diagnosis.CacheValue)
                    {
                        diagnosis.ConflictReason = "数据库值大于等于缓存值，可能是正常并发更新";
                    }
                    else
                    {
                        diagnosis.ConflictReason = "缓存值大于数据库值，可能存在数据不一致";
                    }
                }
                else if (diagnosis.ExistsInDatabase)
                {
                    diagnosis.ConflictReason = "仅存在于数据库中";
                }
                else if (diagnosis.ExistsInCache)
                {
                    diagnosis.ConflictReason = "仅存在于缓存中，等待刷写";
                }
                else
                {
                    diagnosis.ConflictReason = "键不存在于任何存储中";
                }

                diagnosis.IsHealthy = true;
            }
            catch (Exception ex)
            {
                diagnosis.IsHealthy = false;
                diagnosis.ConflictReason = $"诊断过程中发生异常: {ex.Message}";
                LogError($"诊断序列冲突失败: {sequenceKey}", ex);
            }

            return diagnosis;
        }

        #region 日志辅助方法

        /// <summary>
        /// 记录普通错误日志
        /// </summary>
        private void LogError(string message, Exception ex = null)
        {
            var logMessage = ex != null ? $"{message}: {ex.Message}" : message;
            System.Diagnostics.Debug.WriteLine($"[ERROR] {logMessage}");
            
            // 在实际项目中，这里应该调用正式的日志系统
            // Logger.Error(logMessage, ex);
        }

        /// <summary>
        /// 记录严重错误日志
        /// </summary>
        private void LogCriticalError(string message, Exception ex = null)
        {
            var logMessage = ex != null ? $"{message}: {ex.Message}" : message;
            System.Diagnostics.Debug.WriteLine($"[CRITICAL] {logMessage}");
            
            // 在实际项目中，这里应该调用正式的日志系统并触发告警
            // Logger.Critical(logMessage, ex);
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        private void LogInfo(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[INFO] {message}");
            
            // 在实际项目中，这里应该调用正式的日志系统
            // Logger.Info(message);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 判断是否为唯一约束违反异常
        /// </summary>
        private bool IsUniqueConstraintViolation(Exception ex)
        {
            if (ex == null) return false;
            
            var message = ex.Message.ToLower();
            return message.Contains("primary key") ||
                   message.Contains("unique constraint") ||
                   message.Contains("违反了 primary key") ||
                   message.Contains("duplicate key");
        }

        /// <summary>
        /// 计算退避延迟时间（指数退避加随机化）
        /// </summary>
        private int CalculateBackoffDelay(int retryCount)
        {
            var random = new Random();
            int baseDelay = Math.Min(50 * (int)Math.Pow(2, retryCount), 1000);
            int jitter = random.Next(0, Math.Max(50 * retryCount, 100));
            return baseDelay + jitter;
        }

        #endregion
    }

    /// <summary>
    /// 序号表实体类
    /// 用于SqlSugar自动创建表结构
    /// </summary>
    [SugarTable("SequenceNumbers")]
    public class SequenceNumbers
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "Id", DecimalDigits = 0, IsNullable = false, ColumnDescription = "", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 序号键，唯一标识一个序号序列
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType = "String", ColumnName = "SequenceKey", Length = 255, IsNullable = false, ColumnDescription = "序号键，唯一标识一个序号序列")]
        public string SequenceKey { get; set; }

        /// <summary>
        /// 当前序号值
        /// </summary>
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "CurrentValue", DecimalDigits = 0, IsNullable = false, ColumnDescription = "当前序号值")]
        public long CurrentValue { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime2", SqlParameterDbType = "DateTime", ColumnName = "LastUpdated", IsNullable = false, ColumnDescription = "最后更新时间")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime2", SqlParameterDbType = "DateTime", ColumnName = "CreatedAt", IsNullable = false, ColumnDescription = "创建时间")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 重置类型: None, Daily, Monthly, Yearly
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType = "String", ColumnName = "ResetType", Length = 20, IsNullable = true, ColumnDescription = "重置类型: None, Daily, Monthly, Yearly")]
        public string ResetType { get; set; }

        /// <summary>
        /// 格式化掩码，如 000
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType = "String", ColumnName = "FormatMask", Length = 50, IsNullable = true, ColumnDescription = "格式化掩码，如 000")]
        public string FormatMask { get; set; }

        /// <summary>
        /// 序列描述
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType = "String", ColumnName = "Description", Length = 255, IsNullable = true, ColumnDescription = "序列描述")]
        public string Description { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType = "String", ColumnName = "BusinessType", Length = 100, IsNullable = true, ColumnDescription = "业务类型")]
        public string BusinessType { get; set; }
    }

    /// <summary>
    /// 序列冲突诊断结果
    /// </summary>
    public class SequenceConflictDiagnosis
    {
        /// <summary>
        /// 序列键
        /// </summary>
        public string SequenceKey { get; set; }

        /// <summary>
        /// 诊断时间
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 是否存在于数据库中
        /// </summary>
        public bool ExistsInDatabase { get; set; }

        /// <summary>
        /// 是否存在于缓存中
        /// </summary>
        public bool ExistsInCache { get; set; }

        /// <summary>
        /// 数据库中的值
        /// </summary>
        public long? DatabaseValue { get; set; }

        /// <summary>
        /// 缓存中的值
        /// </summary>
        public long? CacheValue { get; set; }

        /// <summary>
        /// 待处理的更新数量
        /// </summary>
        public int PendingUpdates { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// 冲突原因分析
        /// </summary>
        public string ConflictReason { get; set; }

        /// <summary>
        /// 是否健康
        /// </summary>
        public bool IsHealthy { get; set; }

        /// <summary>
        /// 格式化输出诊断信息
        /// </summary>
        public override string ToString()
        {
            return $@"序列键: {SequenceKey}
诊断时间: {Timestamp:yyyy-MM-dd HH:mm:ss}
数据库存在: {ExistsInDatabase}
缓存存在: {ExistsInCache}
数据库值: {DatabaseValue?.ToString() ?? "N/A"}
缓存值: {CacheValue?.ToString() ?? "N/A"}
待处理更新: {PendingUpdates}
最后更新: {LastUpdated?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"}
冲突原因: {ConflictReason}
健康状态: {IsHealthy}";
        }
    }

    /// <summary>
    /// 序号服务健康检查信息
    /// </summary>
    public class SequenceServiceHealthInfo
    {
        /// <summary>
        /// 服务是否健康
        /// </summary>
        public bool IsHealthy { get; set; }

        /// <summary>
        /// 缓存大小
        /// </summary>
        public int CacheSize { get; set; }

        /// <summary>
        /// 队列大小
        /// </summary>
        public int QueueSize { get; set; }

        /// <summary>
        /// 是否正在刷写
        /// </summary>
        public bool IsFlushing { get; set; }

        /// <summary>
        /// 最后刷写时间
        /// </summary>
        public DateTime LastFlushTime { get; set; }

        /// <summary>
        /// 批量更新阈值
        /// </summary>
        public int BatchThreshold { get; set; }
    }

    /// <summary>
    /// 序列键状态信息
    /// </summary>
    public class SequenceKeyStatus
    {
        /// <summary>
        /// 序列键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 是否存在于缓存中
        /// </summary>
        public bool ExistsInCache { get; set; }

        /// <summary>
        /// 是否存在于数据库中
        /// </summary>
        public bool ExistsInDatabase { get; set; }

        /// <summary>
        /// 缓存中的值
        /// </summary>
        public long? CacheValue { get; set; }

        /// <summary>
        /// 数据库中的值
        /// </summary>
        public long? DatabaseValue { get; set; }

        /// <summary>
        /// 数据是否一致
        /// </summary>
        public bool IsConsistent => 
            (!ExistsInCache && !ExistsInDatabase) || 
            (ExistsInCache && ExistsInDatabase && CacheValue == DatabaseValue);
    }
}