using System;
using System.Collections.Concurrent;
using System.Threading;
using RUINORERP.Global.EnumExt;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using System.Threading.Tasks;

namespace RUINORERP.UI.BusinessService
{
    /// <summary>
    /// 防重复操作服务类 - 负责处理所有客户端的防重复操作判断
    /// </summary>
    public class RepeatOperationGuardService
    {
        /// <summary>
        /// 操作记录项 - 存储操作的相关信息
        /// </summary>
        private class OperationRecord
        {
            /// <summary>
            /// 操作类型（用于MenuItemEnums类型的操作）
            /// </summary>
            public MenuItemEnums? OperationType { get; set; }
            
            /// <summary>
            /// 操作名称（用于自定义string类型的操作）
            /// </summary>
            public string OperationName { get; set; }
            
            /// <summary>
            /// 操作时间
            /// </summary>
            public DateTime OperationTime { get; set; }
            
            /// <summary>
            /// 操作源标识（例如：表单名称、控件名称等）
            /// </summary>
            public string OperationSource { get; set; }
            
            /// <summary>
            /// 实体ID - 用于实体级别的防重复检查
            /// </summary>
            public long EntityId { get; set; }
        }
        
        /// <summary>
        /// 操作记录字典 - 存储最近的操作记录
        /// 使用ConcurrentDictionary确保线程安全
        /// </summary>
        private readonly ConcurrentDictionary<string, OperationRecord> _operationRecords = new ConcurrentDictionary<string, OperationRecord>();
        
        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<RepeatOperationGuardService> _logger;
        
        /// <summary>
        /// 默认防抖时间间隔（毫秒）- 与现有实现保持一致
        /// </summary>
        private const int DEFAULT_DEBOUNCE_INTERVAL_MS = 1500;
        
        /// <summary>
        /// 操作记录清理间隔（毫秒）
        /// </summary>
        private const int OPERATION_RECORD_CLEANUP_INTERVAL_MS = 60000; // 1分钟
        
        /// <summary>
        /// 操作记录最大保留时间（毫秒）
        /// </summary>
        private const int OPERATION_RECORD_MAX_AGE_MS = 30000; // 30秒
        
        /// <summary>
        /// 清理定时器
        /// </summary>
        private readonly Timer _cleanupTimer;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public RepeatOperationGuardService(ILogger<RepeatOperationGuardService> logger = null)
        {
            _logger = logger;
            
            // 初始化清理定时器
            _cleanupTimer = new Timer(CleanupOperationRecords, null, 
                OPERATION_RECORD_CLEANUP_INTERVAL_MS, 
                OPERATION_RECORD_CLEANUP_INTERVAL_MS);
        }
        
        /// <summary>
        /// 判断是否应该阻止操作（防重复操作和防抖机制）- 支持MenuItemEnums类型
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="operationSource">操作源标识（例如：表单名称、控件名称等）</param>
        /// <param name="entityId">实体ID - 用于实体级别的防重复检查</param>
        /// <param name="debounceIntervalMs">防抖时间间隔（毫秒），默认使用DEFAULT_DEBOUNCE_INTERVAL_MS</param>
        /// <param name="showStatusMessage">是否显示状态消息</param>
        /// <returns>是否应该阻止操作</returns>
        public bool ShouldBlockOperation(MenuItemEnums operationType, string operationSource, long entityId = 0, int debounceIntervalMs = DEFAULT_DEBOUNCE_INTERVAL_MS, bool showStatusMessage = true)
        {
            if (string.IsNullOrEmpty(operationSource))
            {
                throw new ArgumentException("操作源标识不能为空", nameof(operationSource));
            }
            
            // 生成操作唯一标识（包含实体ID，支持实体级别的防重复检查）
            string operationKey = $"{operationSource}_{operationType}_{entityId}";
            
            // 获取当前时间
            DateTime currentTime = DateTime.Now;
            
            // 尝试获取现有的操作记录
            if (_operationRecords.TryGetValue(operationKey, out OperationRecord existingRecord))
            {
                // 计算距离上次操作的时间间隔
                TimeSpan timeSinceLastOperation = currentTime - existingRecord.OperationTime;
                
                // 如果时间间隔小于防抖时间间隔，则阻止操作
                if (timeSinceLastOperation.TotalMilliseconds < debounceIntervalMs)
                {
                    _logger?.LogDebug("防重复操作：{OperationType} 操作被阻止（距离上次操作 {Time}ms）", 
                        operationType, (int)timeSinceLastOperation.TotalMilliseconds);
                    if (showStatusMessage)
                    {
                        MainForm.Instance?.ShowStatusText($"操作过快，请稍候...");
                    }
                    return true;
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// 判断是否应该阻止操作（防重复操作和防抖机制）- 支持string类型
        /// </summary>
        /// <param name="operationName">操作名称</param>
        /// <param name="operationSource">操作源标识（例如：表单名称、控件名称等）</param>
        /// <param name="entityId">实体ID - 用于实体级别的防重复检查</param>
        /// <param name="debounceIntervalMs">防抖时间间隔（毫秒），默认使用DEFAULT_DEBOUNCE_INTERVAL_MS</param>
        /// <param name="showStatusMessage">是否显示状态消息</param>
        /// <returns>是否应该阻止操作</returns>
        public bool ShouldBlockOperation(string operationName, string operationSource, long entityId = 0, int debounceIntervalMs = DEFAULT_DEBOUNCE_INTERVAL_MS, bool showStatusMessage = true)
        {
            if (string.IsNullOrEmpty(operationSource))
            {
                throw new ArgumentException("操作源标识不能为空", nameof(operationSource));
            }
            
            if (string.IsNullOrEmpty(operationName))
            {
                throw new ArgumentException("操作名称不能为空", nameof(operationName));
            }
            
            // 生成操作唯一标识（包含实体ID，支持实体级别的防重复检查）
            string operationKey = $"{operationSource}_{operationName}_{entityId}";
            
            // 获取当前时间
            DateTime currentTime = DateTime.Now;
            
            // 尝试获取现有的操作记录
            if (_operationRecords.TryGetValue(operationKey, out OperationRecord existingRecord))
            {
                // 计算距离上次操作的时间间隔
                TimeSpan timeSinceLastOperation = currentTime - existingRecord.OperationTime;
                
                // 如果时间间隔小于防抖时间间隔，则阻止操作
                if (timeSinceLastOperation.TotalMilliseconds < debounceIntervalMs)
                {
                    _logger?.LogDebug("防重复操作：{OperationName} 操作被阻止（距离上次操作 {Time}ms）", 
                        operationName, (int)timeSinceLastOperation.TotalMilliseconds);
                    if (showStatusMessage)
                    {
                        MainForm.Instance?.ShowStatusText($"操作过快，请稍候...");
                    }
                    return true;
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// 记录操作（用于手动记录操作，例如在操作完成后）- 支持MenuItemEnums类型
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="operationSource">操作源标识（例如：表单名称、控件名称等）</param>
        /// <param name="entityId">实体ID</param>
        public void RecordOperation(MenuItemEnums operationType, string operationSource, long entityId = 0)
        {
            if (string.IsNullOrEmpty(operationSource))
            {
                throw new ArgumentException("操作源标识不能为空", nameof(operationSource));
            }
            
            // 生成操作唯一标识
            string operationKey = $"{operationSource}_{operationType}_{entityId}";
            
            // 更新操作记录
            _operationRecords[operationKey] = new OperationRecord
            {
                OperationType = operationType,
                OperationTime = DateTime.Now,
                OperationSource = operationSource,
                EntityId = entityId
            };
        }
        
        /// <summary>
        /// 记录操作（用于手动记录操作，例如在操作完成后）- 支持string类型
        /// </summary>
        /// <param name="operationName">操作名称</param>
        /// <param name="operationSource">操作源标识（例如：表单名称、控件名称等）</param>
        /// <param name="entityId">实体ID</param>
        public void RecordOperation(string operationName, string operationSource, long entityId = 0)
        {
            if (string.IsNullOrEmpty(operationSource))
            {
                throw new ArgumentException("操作源标识不能为空", nameof(operationSource));
            }
            
            if (string.IsNullOrEmpty(operationName))
            {
                throw new ArgumentException("操作名称不能为空", nameof(operationName));
            }
            
            // 生成操作唯一标识
            string operationKey = $"{operationSource}_{operationName}_{entityId}";
            
            // 更新操作记录
            _operationRecords[operationKey] = new OperationRecord
            {
                OperationName = operationName,
                OperationTime = DateTime.Now,
                OperationSource = operationSource,
                EntityId = entityId
            };
        }
        
        /// <summary>
        /// 判断是否应该阻止方法调用（防重复调用机制）
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="operationSource">操作源标识（例如：类名、对象实例标识等）</param>
        /// <param name="debounceIntervalMs">防抖时间间隔（毫秒），默认使用DEFAULT_DEBOUNCE_INTERVAL_MS</param>
        /// <param name="showStatusMessage">是否显示状态消息</param>
        /// <returns>是否应该阻止方法调用</returns>
        public bool ShouldBlockMethod(string methodName, string operationSource, int debounceIntervalMs = DEFAULT_DEBOUNCE_INTERVAL_MS, bool showStatusMessage = false)
        {
            // 直接使用字符串类型的操作名称，将方法名作为操作名
            return ShouldBlockOperation(methodName, operationSource, 0, debounceIntervalMs, showStatusMessage);
        }
        
        /// <summary>
        /// 记录方法调用（用于手动记录方法调用，例如在方法执行完成后）
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="operationSource">操作源标识（例如：类名、对象实例标识等）</param>
        public void RecordMethodCall(string methodName, string operationSource)
        {
            // 直接使用字符串类型的操作记录，将方法名作为操作名
            RecordOperation(methodName, operationSource, 0);
        }
        
        /// <summary>
        /// 执行方法并进行防重复检查 - 提供一个便捷的方式来执行方法，自动处理防重复逻辑
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="operationSource">操作源标识</param>
        /// <param name="action">要执行的方法</param>
        /// <param name="debounceIntervalMs">防抖时间间隔（毫秒）</param>
        /// <param name="showStatusMessage">是否显示状态消息</param>
        /// <returns>是否成功执行了方法</returns>
        public bool ExecuteWithGuard(string methodName, string operationSource, Action action, int debounceIntervalMs = DEFAULT_DEBOUNCE_INTERVAL_MS, bool showStatusMessage = false)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            
            // 检查是否应该阻止方法调用
            if (ShouldBlockMethod(methodName, operationSource, debounceIntervalMs, showStatusMessage))
            {
                return false;
            }
            
            try
            {
                // 记录方法调用
                RecordMethodCall(methodName, operationSource);
                
                // 执行方法
                action.Invoke();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "执行受保护方法 {MethodName} 时发生错误", methodName);
                throw;
            }
        }
        
        /// <summary>
        /// 异步执行方法并进行防重复检查
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="operationSource">操作源标识</param>
        /// <param name="func">要执行的异步方法</param>
        /// <param name="debounceIntervalMs">防抖时间间隔（毫秒）</param>
        /// <param name="showStatusMessage">是否显示状态消息</param>
        /// <returns>是否成功执行了方法</returns>
        public async Task<bool> ExecuteWithGuardAsync(string methodName, string operationSource, Func<Task> func, int debounceIntervalMs = DEFAULT_DEBOUNCE_INTERVAL_MS, bool showStatusMessage = false)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            
            // 检查是否应该阻止方法调用
            if (ShouldBlockMethod(methodName, operationSource, debounceIntervalMs, showStatusMessage))
            {
                return false;
            }
            
            try
            {
                // 记录方法调用
                RecordMethodCall(methodName, operationSource);
                
                // 执行异步方法
                await func.Invoke().ConfigureAwait(false);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "执行受保护的异步方法 {MethodName} 时发生错误", methodName);
                throw;
            }
        }
        
        /// <summary>
        /// 清理过期的操作记录
        /// </summary>
        /// <param name="state">定时器状态</param>
        private void CleanupOperationRecords(object state)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                int removedCount = 0;
                
                // 遍历所有操作记录，删除过期的记录
                foreach (var operationKey in _operationRecords.Keys)
                {
                    if (_operationRecords.TryGetValue(operationKey, out OperationRecord record))
                    {
                        TimeSpan timeSinceOperation = currentTime - record.OperationTime;
                        
                        if (timeSinceOperation.TotalMilliseconds > OPERATION_RECORD_MAX_AGE_MS)
                        {
                            if (_operationRecords.TryRemove(operationKey, out _))
                            {
                                removedCount++;
                            }
                        }
                    }
                }
                
                if (removedCount > 0)
                {
                    _logger?.LogDebug("清理了 {RemovedCount} 条过期操作记录", removedCount);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "清理操作记录时发生错误");
            }
        }
        
        /// <summary>
        /// 清理指定操作源的所有操作记录
        /// </summary>
        /// <param name="operationSource">操作源标识</param>
        public void CleanupOperationRecords(string operationSource)
        {
            if (string.IsNullOrEmpty(operationSource))
            {
                throw new ArgumentException("操作源标识不能为空", nameof(operationSource));
            }
            
            int removedCount = 0;
            
            // 遍历所有操作记录，删除指定操作源的记录
            foreach (var operationKey in _operationRecords.Keys)
            {
                if (operationKey.StartsWith($"{operationSource}_"))
                {
                    if (_operationRecords.TryRemove(operationKey, out _))
                    {
                        removedCount++;
                    }
                }
            }
            
            if (removedCount > 0)
            {
                _logger?.LogDebug("清理了 {RemovedCount} 条操作记录，操作源：{OperationSource}", removedCount, operationSource);
            }
        }
        
        /// <summary>
        /// 清理指定实体的所有操作记录
        /// </summary>
        /// <param name="operationSource">操作源标识</param>
        /// <param name="entityId">实体ID</param>
        public void CleanupEntityOperationRecords(string operationSource, long entityId)
        {
            if (string.IsNullOrEmpty(operationSource))
            {
                throw new ArgumentException("操作源标识不能为空", nameof(operationSource));
            }
            
            int removedCount = 0;
            string entityKeyPrefix = $"{operationSource}_*_{entityId}";
            
            // 遍历所有操作记录，删除指定实体的记录
            foreach (var operationKey in _operationRecords.Keys)
            {
                // 检查操作键是否匹配指定实体
                var parts = operationKey.Split('_');
                if (parts.Length >= 3 && parts[0] == operationSource && long.TryParse(parts[2], out long keyEntityId) && keyEntityId == entityId)
                {
                    if (_operationRecords.TryRemove(operationKey, out _))
                    {
                        removedCount++;
                    }
                }
            }
            
            if (removedCount > 0)
            {
                _logger?.LogDebug("清理了 {RemovedCount} 条操作记录，操作源：{OperationSource}，实体ID：{EntityId}", removedCount, operationSource, entityId);
            }
        }
        
        /// <summary>
        /// 清理所有操作记录
        /// </summary>
        public void CleanupAllOperationRecords()
        {
            int removedCount = _operationRecords.Count;
            _operationRecords.Clear();
            
            if (removedCount > 0)
            {
                _logger?.LogDebug("清理了所有 {RemovedCount} 条操作记录", removedCount);
            }
        }
        
        /// <summary>
        /// 获取当前操作记录数量
        /// </summary>
        public int OperationRecordCount => _operationRecords.Count;
    }
}