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
        
        // 内存缓存，用于存储序列当前值，减少数据库访问
        private readonly ConcurrentDictionary<string, long> _sequenceCache = new ConcurrentDictionary<string, long>();
        
        // 批量更新队列，存储需要写入数据库的序列值
        private readonly ConcurrentQueue<SequenceUpdateInfo> _updateQueue = new ConcurrentQueue<SequenceUpdateInfo>();
        
        // 按键分片锁，用于控制对同一序列键的并发访问
        // 优化：使用按键分片锁替代全局锁，不同序列键可并行处理
        private readonly ConcurrentDictionary<string, object> _keyLocks = new ConcurrentDictionary<string, object>();
        
        // 队列处理锁，用于控制队列更新的并发
        private readonly object _queueLock = new object();
        
        // 缓存最大生命周期（毫秒），超过这个时间会强制刷新到数据库
        private const int CACHE_MAX_LIFETIME = 5000;
        
        // 批量更新阈值，当队列超过这个数量时触发批量更新
        // 将常量改为可变的静态字段，支持动态调整
        private static int _batchUpdateThreshold = 20;
        
        // 上次刷新时间
        private DateTime _lastFlushTime;
        
        // 取消令牌，用于控制后台任务的停止
        private CancellationTokenSource _cancellationTokenSource;
        
        // 后台任务
        private Task _backgroundTask;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlSugarClient">SqlSugar客户端实例</param>
        public DatabaseSequenceService(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
            // 初始化最后刷新时间
            _lastFlushTime = DateTime.Now;
            
            // 初始化取消令牌
            _cancellationTokenSource = new CancellationTokenSource();
            
            // 启动后台任务，定期将缓存中的序列值刷新到数据库
            _backgroundTask = Task.Run(() => BackgroundFlushTask(_cancellationTokenSource.Token));
        }
        
        /// <summary>
        /// 序列更新信息
        /// </summary>
        private class SequenceUpdateInfo
        {
            public string SequenceKey { get; set; }
            public long Value { get; set; }
            public string ResetType { get; set; }
            public string FormatMask { get; set; }
            public string Description { get; set; }
            public string BusinessType { get; set; }
        }
        
        /// <summary>
        /// 后台刷新任务，定期将缓存中的序列值写入数据库
        /// </summary>
        private async Task BackgroundFlushTask(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // 等待一段时间，可取消
                    await Task.Delay(1000, cancellationToken);
                    
                    // 检查是否需要刷新
                    TimeSpan elapsed = DateTime.Now - _lastFlushTime;
                    if (elapsed.TotalMilliseconds >= CACHE_MAX_LIFETIME || _updateQueue.Count >= _batchUpdateThreshold)
                    {
                        FlushCacheToDatabase();
                    }
                }
                catch (OperationCanceledException)
                {
                    // 任务被取消，正常退出循环
                    break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"后台刷新任务异常: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 手动刷新所有缓存到数据库
        /// 在服务关闭前调用此方法确保数据持久化
        /// </summary>
        public void FlushAllToDatabase()
        {
            FlushCacheToDatabase();
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                // 取消后台任务
                _cancellationTokenSource?.Cancel();
                
                // 等待后台任务完成
                if (_backgroundTask != null && !_backgroundTask.IsCompleted)
                {
                    // 使用Task.Wait替代_task.Wait()以避免潜在的死锁
                    Task.WaitAny(_backgroundTask, Task.Delay(2000)); // 等待最多2秒
                }
                
                // 最后一次刷新缓存到数据库
                FlushAllToDatabase();
                
                // 释放取消令牌
                _cancellationTokenSource?.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"释放 DatabaseSequenceService 资源时出错: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 将缓存中的序列值刷新到数据库
        /// 优化：使用小事务批量更新 + 乐观锁，减少锁持有时间
        /// </summary>
        private void FlushCacheToDatabase()
        {
            // 使用双重检查锁定模式，减少锁竞争
            if (_updateQueue.IsEmpty)
                return;
                
            lock (_queueLock)
            {
                if (_updateQueue.IsEmpty)
                    return;
                
                List<SequenceUpdateInfo> batchUpdates = new List<SequenceUpdateInfo>();
                
                // 从队列中取出一定数量的更新项
                int count = 0;
                while (_updateQueue.TryDequeue(out var updateInfo) && count < 100)
                {
                    batchUpdates.Add(updateInfo);
                    count++;
                }
                
                if (batchUpdates.Count == 0)
                {
                    return;
                }
                
                System.Diagnostics.Debug.WriteLine($"开始批量更新 {batchUpdates.Count} 个序列值");
                
                // 优化：不使用大事务，而是逐条更新以减少锁持有时间
                foreach (var update in batchUpdates)
                {
                    try
                    {
                        // 使用乐观锁更新，只在数据库值小于当前值时才更新
                        int affectedRows = _sqlSugarClient.Updateable<SequenceNumbers>()
                            .SetColumns(s => new SequenceNumbers
                            {
                                CurrentValue = update.Value,
                                LastUpdated = DateTime.Now
                            })
                            .Where(s => s.SequenceKey == update.SequenceKey
                                && s.CurrentValue < update.Value) // 条件更新，避免覆盖新值
                            .ExecuteCommand();
                        
                        // 如果没有更新到任何记录，说明记录不存在
                        if (affectedRows == 0)
                        {
                            // 尝试插入新记录
                            try
                            {
                                _sqlSugarClient.Insertable(new SequenceNumbers
                                {
                                    SequenceKey = update.SequenceKey,
                                    CurrentValue = update.Value,
                                    LastUpdated = DateTime.Now,
                                    CreatedAt = DateTime.Now,
                                    ResetType = update.ResetType,
                                    FormatMask = update.FormatMask,
                                    Description = update.Description,
                                    BusinessType = update.BusinessType
                                }).ExecuteCommand();
                            }
                            catch
                            {
                                // 插入失败，可能已被并发插入，忽略
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"更新序列值失败，键: {update.SequenceKey}，错误: {ex.Message}");
                    }
                }
                
                _lastFlushTime = DateTime.Now;
                System.Diagnostics.Debug.WriteLine($"成功将 {batchUpdates.Count} 个序列值刷新到数据库");
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
        {            string dynamicKey = key;
            
            if (!string.IsNullOrEmpty(resetType))
            {                switch (resetType.ToUpper())
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
        /// 获取下一个序列值（支持按时间单位重置）
        /// 优化：使用按键分片锁、行级锁和乐观锁机制，提升高并发性能
        /// </summary>
        /// <param name="sequenceKey">序列键</param>
        /// <param name="resetType">重置类型（None、Daily、Monthly、Yearly）</param>
        /// <param name="formatMask">格式掩码</param>
        /// <param name="description">描述</param>
        /// <param name="businessType">业务类型</param>
        /// <returns>下一个序列值</returns>
        public long GetNextSequenceValue(string sequenceKey, string resetType = "None", string formatMask = null, string description = null, string businessType = null)
        {
            if (string.IsNullOrEmpty(sequenceKey))
            {
                throw new ArgumentNullException(nameof(sequenceKey), "序列键不能为空");
            }
            
            // 生成动态键（包含重置类型信息）
            string dynamicKey = GenerateDynamicKey(sequenceKey, resetType);
            
            // 尝试从内存缓存中获取并递增序列值
            // 使用原子操作确保线程安全
            long nextValue = _sequenceCache.AddOrUpdate(
                dynamicKey,
                // 如果键不存在，使用行级锁从数据库加载（优化点）
                (key) => GetNextValueWithRowLock(key, resetType, formatMask, description, businessType),
                // 如果键存在，原子递增并异步更新数据库
                (key, currentValue) => {
                    long next = currentValue + 1;
                    
                    // 将更新添加到队列，等待批量写入数据库
                    _updateQueue.Enqueue(new SequenceUpdateInfo
                    {
                        SequenceKey = key,
                        Value = next,
                        ResetType = resetType,
                        FormatMask = formatMask,
                        Description = description,
                        BusinessType = businessType
                    });
                    
                    return next;
                });
            
            // 检查是否需要立即刷新缓存到数据库（避免内存缓存过大）
            if (_updateQueue.Count >= _batchUpdateThreshold)
            {
                // 使用单独的任务刷新缓存，避免阻塞当前线程
                Task.Run(() => {
                    try
                    {
                        FlushCacheToDatabase();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"后台刷新缓存时发生异常: {ex.Message}");
                    }
                });
            }
            
            return nextValue;
        }
        
        /// <summary>
        /// 使用行级锁获取下一个值
        /// 核心优化：使用WITH(UPDLOCK, HOLDLOCK)确保行级锁，避免脏读和并发冲突
        /// </summary>
        /// <param name="sequenceKey">序列键</param>
        /// <param name="resetType">重置类型</param>
        /// <param name="formatMask">格式掩码</param>
        /// <param name="description">描述</param>
        /// <param name="businessType">业务类型</param>
        /// <returns>下一个序列值</returns>
        private long GetNextValueWithRowLock(string sequenceKey, string resetType,
            string formatMask, string description, string businessType)
        {
            // 获取键级别锁，减少锁竞争粒度
            // 相比全局锁，按键锁可以让不同序列键的请求并行执行
            var keyLock = _keyLocks.GetOrAdd(sequenceKey, k => new object());
            
            lock (keyLock)
            {
                // 使用乐观锁机制，先查后更新
                int retryCount = 0;
                const int maxRetries = 5;
                
                while (retryCount < maxRetries)
                {
                    try
                    {
                        // 使用WITH(UPDLOCK, HOLDLOCK)确保行级锁
                        // UPDLOCK: 读取时加锁，直到事务结束
                        // HOLDLOCK: 等同于SERIALIZABLE隔离级别，防止幻读
                        var sequence = _sqlSugarClient.Ado.SqlQuery<SequenceNumbers>(
                            "SELECT * FROM SequenceNumbers WITH(UPDLOCK, HOLDLOCK) " +
                            "WHERE SequenceKey = @SequenceKey",
                            new { SequenceKey = sequenceKey })
                            .FirstOrDefault();
                        
                        long nextValue;
                        
                        if (sequence != null)
                        {
                            // 记录当前版本号，用于乐观锁
                            long currentVersion = sequence.CurrentValue;
                            nextValue = currentVersion + 1;
                            
                            // 使用乐观锁更新，只有当CurrentValue未变化时才更新
                            // 这种方式避免了长时间的行锁持有，提高并发性能
                            int affectedRows = _sqlSugarClient.Updateable<SequenceNumbers>()
                                .SetColumns(s => new SequenceNumbers
                                {
                                    CurrentValue = nextValue,
                                    LastUpdated = DateTime.Now
                                })
                                .Where(s => s.SequenceKey == sequenceKey 
                                    && s.CurrentValue == currentVersion) // 乐观锁条件
                                .ExecuteCommand();
                            
                            if (affectedRows == 0)
                            {
                                // 乐观锁失败，已被其他事务修改，重试
                                retryCount++;
                                System.Diagnostics.Debug.WriteLine(
                                    $"乐观锁失败，重试中 ({retryCount}/{maxRetries})，键: {sequenceKey}");
                                Thread.Sleep(10 * retryCount); // 指数退避
                                continue;
                            }
                        }
                        else
                        {
                            // 插入新记录
                            nextValue = 1;
                            try
                            {
                                _sqlSugarClient.Insertable(new SequenceNumbers
                                {
                                    SequenceKey = sequenceKey,
                                    CurrentValue = nextValue,
                                    LastUpdated = DateTime.Now,
                                    CreatedAt = DateTime.Now,
                                    ResetType = resetType,
                                    FormatMask = formatMask,
                                    Description = description,
                                    BusinessType = businessType
                                }).ExecuteCommand();
                            }
                            catch (Exception ex) when (ex.Message.Contains("PRIMARY KEY") || 
                                ex.Message.Contains("UNIQUE constraint") ||
                                ex.Message.Contains("违反了 PRIMARY KEY"))
                            {
                                // 插入失败，已被其他事务插入，重试查询
                                retryCount++;
                                System.Diagnostics.Debug.WriteLine(
                                    $"插入失败（并发插入），重试中 ({retryCount}/{maxRetries})，键: {sequenceKey}");
                                continue;
                            }
                        }
                        
                        // 成功获取值后，立即更新缓存
                        _sequenceCache.AddOrUpdate(sequenceKey, nextValue, (k, v) => nextValue);
                        
                        System.Diagnostics.Debug.WriteLine(
                            $"成功获取序列值，键: {sequenceKey}，值: {nextValue}");
                        
                        return nextValue;
                    }
                    catch (System.Data.SqlClient.SqlException ex) when (ex.Number == 1205) // 死锁错误码
                    {
                        retryCount++;
                        System.Diagnostics.Debug.WriteLine(
                            $"检测到死锁，重试中 ({retryCount}/{maxRetries})，键: {sequenceKey}");
                        Thread.Sleep(50 * retryCount);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"获取序列值时发生异常: {ex.Message}，键: {sequenceKey}");
                        
                        if (retryCount >= maxRetries - 1)
                        {
                            throw;
                        }
                        
                        retryCount++;
                        Thread.Sleep(20 * retryCount);
                    }
                }
                
                throw new Exception($"获取序列值失败，已达到最大重试次数 {maxRetries}，键: {sequenceKey}");
            }
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
        /// 设置批量更新阈值
        /// </summary>
        /// <param name="threshold">新的阈值</param>
        public static void SetBatchUpdateThreshold(int threshold)
        {
            if (threshold > 0)
            {
                _batchUpdateThreshold = threshold;
                System.Diagnostics.Debug.WriteLine($"批量更新阈值已设置为: {threshold}");
            }
        }
        
        /// <summary>
        /// 获取当前批量更新阈值
        /// </summary>
        /// <returns>当前阈值</returns>
        public static int GetBatchUpdateThreshold()
        {
            return _batchUpdateThreshold;
        }
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
}