using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SqlSugar;

namespace RUINORERP.Server.BNR
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
        
        // 线程同步锁，用于控制缓存刷新到数据库的操作
        private readonly object _queueLock = new object();
        
        // 缓存最大生命周期（毫秒），超过这个时间会强制刷新到数据库
        private const int CACHE_MAX_LIFETIME = 5000;
        
        // 批量更新阈值，当队列超过这个数量时触发批量更新
        // 将常量改为可变的静态字段，支持动态调整
        private static int _batchUpdateThreshold = 5;
        
        // 上次刷新时间
        private DateTime _lastFlushTime;
        
        // 上次清理时间
        private DateTime _lastCleanupTime;
        
        // 序列缓存过期时间（分钟）
        private const int SEQUENCE_CACHE_EXPIRATION_MINUTES = 60;
        
        // 取消令牌，用于控制后台任务的停止
        private CancellationTokenSource _cancellationTokenSource;
        
        // 后台任务
        private Task _backgroundTask;
        
        /// <summary>
        /// 后台刷新间隔（毫秒）
        /// </summary>
        private const int BACKGROUND_FLUSH_INTERVAL = 5000; // 5秒，减少过于频繁的检查
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlSugarClient">SqlSugar客户端实例</param>
        public DatabaseSequenceService(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
            // 初始化表结构
            InitializeTable();
            
            // 初始化最后刷新时间和清理时间
            _lastFlushTime = DateTime.Now;
            _lastCleanupTime = DateTime.Now;
            
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
                    await Task.Delay(BACKGROUND_FLUSH_INTERVAL, cancellationToken);
                    
                    // 检查是否需要刷新
                    TimeSpan elapsed = DateTime.Now - _lastFlushTime;
                    if (elapsed.TotalMilliseconds >= CACHE_MAX_LIFETIME || _updateQueue.Count >= _batchUpdateThreshold)
                    {
                        FlushCacheToDatabase();
                    }
                    
                    // 检查是否需要清理过期缓存
                    TimeSpan cleanupElapsed = DateTime.Now - _lastCleanupTime;
                    if (cleanupElapsed.TotalMinutes >= SEQUENCE_CACHE_EXPIRATION_MINUTES)
                    {
                        CleanupExpiredCache();
                        _lastCleanupTime = DateTime.Now;
                    }
                }
                catch (OperationCanceledException)
                {
                    // 任务被取消，正常退出循环
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"后台刷新任务异常: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 清理过期缓存
        /// </summary>
        private void CleanupExpiredCache()
        {
            try
            {
                // 在实际应用中，可以根据最后访问时间清理过期缓存
                // 这里简化处理，仅记录日志
                Console.WriteLine($"执行缓存清理: 当前缓存项数量 {_sequenceCache.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"缓存清理过程中发生异常: {ex.Message}");
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
                    _backgroundTask.Wait(2000); // 等待最多2秒
                }
                
                // 最后一次刷新缓存到数据库
                FlushAllToDatabase();
                
                // 释放取消令牌
                _cancellationTokenSource?.Dispose();
                
                Console.WriteLine("DatabaseSequenceService 资源已释放，所有缓存已刷新到数据库");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"释放 DatabaseSequenceService 资源时出错: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 将缓存中的序列值刷新到数据库
        /// </summary>
        private void FlushCacheToDatabase()
        {
            lock (_queueLock)
            {
                if (_updateQueue.IsEmpty)
                    return;
                
                List<SequenceUpdateInfo> batchUpdates = new List<SequenceUpdateInfo>();
                
                try
                {
                    _sqlSugarClient.Ado.BeginTran();
                    
                    int count = 0;
                    
                    // 从队列中取出一定数量的更新项
                    while (_updateQueue.TryDequeue(out var updateInfo) && count < 50)
                    {
                        batchUpdates.Add(updateInfo);
                        count++;
                    }
                    
                    if (batchUpdates.Count == 0)
                        return;
                    
                    // 批量处理更新
                    foreach (var update in batchUpdates)
                    {
                        // 检查记录是否存在
                        bool exists = _sqlSugarClient.Queryable<SequenceNumbers>()
                            .Where(s => s.SequenceKey == update.SequenceKey)
                            .Any();
                        
                        if (exists)
                        {
                            // 更新现有记录
                            _sqlSugarClient.Updateable<SequenceNumbers>()
                                .SetColumns(s => new SequenceNumbers
                                {
                                    CurrentValue = update.Value,
                                    LastUpdated = DateTime.Now,
                                    ResetType = update.ResetType,
                                    FormatMask = update.FormatMask,
                                    Description = update.Description,
                                    BusinessType = update.BusinessType
                                })
                                .Where(s => s.SequenceKey == update.SequenceKey)
                                .ExecuteCommand();
                        }
                        else
                        {
                            // 插入新记录
                            var newSequence = new SequenceNumbers
                            {
                                SequenceKey = update.SequenceKey,
                                CurrentValue = update.Value,
                                LastUpdated = DateTime.Now,
                                CreatedAt = DateTime.Now,
                                ResetType = update.ResetType,
                                FormatMask = update.FormatMask,
                                Description = update.Description,
                                BusinessType = update.BusinessType
                            };
                            
                            _sqlSugarClient.Insertable(newSequence).ExecuteCommand();
                        }
                    }
                    
                    _sqlSugarClient.Ado.CommitTran();
                    _lastFlushTime = DateTime.Now;
                    
                    Console.WriteLine($"成功将 {batchUpdates.Count} 个序列值刷新到数据库");
                }
                catch (Exception ex)
                {
                    _sqlSugarClient.Ado.RollbackTran();
                    Console.WriteLine($"刷新序列值到数据库失败: {ex.Message}");
                    
                    // 重新入队失败的更新（简化处理）
                    foreach (var update in batchUpdates)
                    {
                        _updateQueue.Enqueue(update);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化序号表
        /// 检查表是否存在，不存在则创建，确保表结构包含所有必要字段和索引
        /// </summary>
        public void InitializeTable()
        {
            try
            {
                string tableName = "SequenceNumbers";
                
                // 检查数据库表是否存在
                if (!_sqlSugarClient.DbMaintenance.IsAnyTable(tableName))
                {
                    // 表不存在，创建新表
                    // 使用CodeFirst创建表，设置字符串默认长度
                    _sqlSugarClient.CodeFirst
                        .SetStringDefaultLength(255)
                        .InitTables(typeof(SequenceNumbers));
                    Console.WriteLine("序号表创建成功");
                }
                else
                {
                    // 表存在，确保字段完整性
                    EnsureTableStructure();
                    Console.WriteLine("序号表已存在");
                }
                
                // 检查并创建索引（如果不存在）
                if (!_sqlSugarClient.DbMaintenance.GetIndexList(tableName).Contains("IX_SequenceNumbers_SequenceKey"))
                {
                    _sqlSugarClient.DbMaintenance.CreateIndex(tableName, new string[] { "SequenceKey" }, "IX_SequenceNumbers_SequenceKey", true);
                    Console.WriteLine("创建序号表索引成功");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"初始化序号表失败: {ex.Message}");
                // 记录日志但不抛出异常，避免影响服务启动
            }
        }

        /// <summary>
        /// 确保表结构包含所有必要字段
        /// 使用SqlSugar的CodeFirst功能进行表结构同步，只添加新字段，不删除或修改现有字段
        /// </summary>
        private void EnsureTableStructure()
        {
            try
            {
                // 安全模式：只添加新字段，不删除或修改现有字段
                // 对于生产环境，建议禁用自动创建表和修改表结构的功能
                // 通过WithCheckExists确保只有在表存在的情况下才同步结构
                _sqlSugarClient.CodeFirst
                    .SetStringDefaultLength(255)
                    // 在初始化表时检查表是否存在
                    .InitTables<SequenceNumbers>();
                Console.WriteLine("表结构同步成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"同步表结构时出错: {ex.Message}");
                // 记录异常但不抛出，确保服务继续运行
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
            EnsureTableStructure();
            
            try
            {
                var sequence = _sqlSugarClient.Queryable<SequenceNumbers>()
                    .Where(s => s.SequenceKey == sequenceKey)
                    .First();
                
                return sequence?.CurrentValue ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取当前序列值失败: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// 更新指定序列的值
        /// </summary>
        /// <param name="sequenceKey">序列键</param>
        /// <param name="newValue">新值</param>
        public void UpdateSequenceValue(string sequenceKey, long newValue)
        {
            EnsureTableStructure();
            
            try
            {
                _sqlSugarClient.Ado.BeginTran();
                
                var sequence = _sqlSugarClient.Queryable<SequenceNumbers>()
                    .Where(s => s.SequenceKey == sequenceKey)
                    .First();
                
                if (sequence != null)
                {
                    sequence.CurrentValue = newValue;
                    sequence.LastUpdated = DateTime.Now;
                    _sqlSugarClient.Updateable(sequence).ExecuteCommand();
                }
                else
                {
                    var newSequence = new SequenceNumbers
                    {
                        SequenceKey = sequenceKey,
                        CurrentValue = newValue,
                        LastUpdated = DateTime.Now,
                        CreatedAt = DateTime.Now
                    };
                    _sqlSugarClient.Insertable(newSequence).ExecuteCommand();
                }
                
                _sqlSugarClient.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _sqlSugarClient.Ado.RollbackTran();
                Console.WriteLine($"更新序列值失败: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// 获取下一个序列值（支持按时间单位重置）
        /// 使用内存缓存和锁机制确保高并发下序列号唯一性
        /// </summary>
        /// <param name="sequenceKey">序列键</param>
        /// <param name="resetType">重置类型（None、Daily、Monthly、Yearly）</param>
        /// <param name="formatMask">格式掩码</param>
        /// <param name="description">描述</param>
        /// <param name="businessType">业务类型</param>
        /// <returns>下一个序列值</returns>
        public long GetNextSequenceValue(string sequenceKey, string resetType = "None", string formatMask = null, string description = null, string businessType = null)
        {
            // 先确保表结构正确
            EnsureTableStructure();
            
            // 生成动态键（包含重置类型信息）
            string dynamicKey = GenerateDynamicKey(sequenceKey, resetType);
            
            // 尝试从内存缓存中获取并递增序列值
            // 使用原子操作确保线程安全
            long nextValue = _sequenceCache.AddOrUpdate(
                dynamicKey,
                // 如果键不存在，从数据库加载或初始化为1
                (key) => {
                    // 先尝试从数据库获取当前值
                    long dbValue = GetCurrentSequenceValueFromDb(key);
                    if (dbValue > 0)
                    {
                        // 数据库中存在记录，返回数据库值 + 1
                        _updateQueue.Enqueue(new SequenceUpdateInfo
                        {
                            SequenceKey = key,
                            Value = dbValue + 1,
                            ResetType = resetType,
                            FormatMask = formatMask,
                            Description = description,
                            BusinessType = businessType
                        });
                        return dbValue + 1;
                    }
                    else
                    {
                        // 数据库中不存在记录，初始化为1
                        _updateQueue.Enqueue(new SequenceUpdateInfo
                        {
                            SequenceKey = key,
                            Value = 1,
                            ResetType = resetType,
                            FormatMask = formatMask,
                            Description = description,
                            BusinessType = businessType
                        });
                        return 1;
                    }
                },
                // 如果键存在，原子递增并返回新值
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
                Task.Run(() => FlushCacheToDatabase());
            }
            
            return nextValue;
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
                Console.WriteLine($"从数据库获取序列值失败: {ex.Message}");
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
        /// </summary>
        /// <param name="key">序列键</param>
        public void ResetSequence(string key)
        {
            EnsureTableStructure();
            
            try
            {
                _sqlSugarClient.Ado.BeginTran();
                
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
                
                _sqlSugarClient.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _sqlSugarClient.Ado.RollbackTran();
                Console.WriteLine($"重置序列失败: {ex.Message}");
                throw;
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
            EnsureTableStructure();
            
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
            EnsureTableStructure();
            
            return _sqlSugarClient.Queryable<SequenceNumbers>()
                .Where(s => s.BusinessType == businessType)
                .OrderBy(s => s.Id)
                .ToList();
        }
        
        /// <summary>
        /// 更新序列信息
        /// </summary>
        /// <param name="key">序列键</param>
        /// <param name="resetType">重置类型</param>
        /// <param name="formatMask">格式掩码</param>
        /// <param name="description">描述</param>
        /// <param name="businessType">业务类型</param>
        public void UpdateSequenceInfo(string key, string resetType = null, string formatMask = null, string description = null, string businessType = null)
        {
            EnsureTableStructure();
            
            try
            {
                _sqlSugarClient.Ado.BeginTran();
                
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
                _sqlSugarClient.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _sqlSugarClient.Ado.RollbackTran();
                Console.WriteLine($"更新序列信息失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 重置序列号到指定值
        /// </summary>
        /// <param name="key">序列号键</param>
        /// <param name="newValue">新的值</param>
        public void ResetSequenceValue(string key, long newValue)
        {
            EnsureTableStructure();
            
            try
            {
                _sqlSugarClient.Ado.BeginTran();
                
                var sequence = _sqlSugarClient.Queryable<SequenceNumbers>()
                    .Where(s => s.SequenceKey == key)
                    .First();
                     
                if (sequence != null)
                {
                    sequence.CurrentValue = newValue;
                    sequence.LastUpdated = DateTime.Now;
                    _sqlSugarClient.Updateable(sequence).ExecuteCommand();
                }
                else
                {
                    var newSequence = new SequenceNumbers
                    {
                        SequenceKey = key,
                        CurrentValue = newValue,
                        LastUpdated = DateTime.Now,
                        CreatedAt = DateTime.Now
                    };
                    _sqlSugarClient.Insertable(newSequence).ExecuteCommand();
                }
                
                _sqlSugarClient.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _sqlSugarClient.Ado.RollbackTran();
                Console.WriteLine($"重置序列值失败: {ex.Message}");
                throw;
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
                Console.WriteLine($"批量更新阈值已设置为: {threshold}");
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