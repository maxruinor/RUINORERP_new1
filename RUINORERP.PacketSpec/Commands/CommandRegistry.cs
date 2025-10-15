using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 文件级别注释：
    /// 命令注册表 - 企业级命令和处理器注册管理中心
    /// 
    /// 职责：
    /// 1. 管理命令类型与命令ID的映射关系
    /// 2. 管理命令处理器注册和生命周期
    /// 3. 提供延迟注册机制
    /// 4. 支持注册验证和冲突检测
    /// 5. 提供注册事件和通知机制
    /// 
    /// 设计目标：
    /// - 分离CommandScanner的扫描和注册职责
    /// - 提供线程安全的注册机制
    /// - 支持运行时动态注册和注销
    /// - 提供详细的注册统计和监控
    /// 
    /// 工作流程：
    /// 1. 接收扫描器发现的类型信息
    /// 2. 验证类型有效性和冲突
    /// 3. 执行注册操作并更新缓存
    /// 4. 触发注册事件通知
    /// 5. 维护注册状态和历史记录
    /// </summary>
    public class CommandRegistry
    {
        private readonly ILogger<CommandRegistry> _logger;
        private readonly CommandCacheManager _cacheManager;
        
        // 注册状态管理
        private readonly ConcurrentDictionary<CommandId, RegistrationInfo> _registrationStatus;
        private readonly ConcurrentDictionary<string, RegistrationInfo> _handlerRegistrationStatus;
        
        // 延迟注册队列
        private readonly ConcurrentQueue<PendingRegistration> _pendingRegistrations;
        private readonly SemaphoreSlim _registrationSemaphore;
        
        // 冲突检测
        private readonly ConcurrentDictionary<CommandId, List<Type>> _commandConflicts;
        private readonly ConcurrentDictionary<string, List<Type>> _handlerConflicts;
        
        // 注册事件
        public event EventHandler<RegistrationEventArgs> CommandRegistered;
        public event EventHandler<RegistrationEventArgs> CommandUnregistered;
        public event EventHandler<RegistrationEventArgs> HandlerRegistered;
        public event EventHandler<RegistrationEventArgs> HandlerUnregistered;
        public event EventHandler<RegistrationConflictEventArgs> RegistrationConflict;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="logger">日志记录器</param>
        public CommandRegistry(CommandCacheManager cacheManager, ILogger<CommandRegistry> logger = null)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _logger = logger;
            
            _registrationStatus = new ConcurrentDictionary<CommandId, RegistrationInfo>();
            _handlerRegistrationStatus = new ConcurrentDictionary<string, RegistrationInfo>();
            _pendingRegistrations = new ConcurrentQueue<PendingRegistration>();
            _registrationSemaphore = new SemaphoreSlim(1, 1);
            
            _commandConflicts = new ConcurrentDictionary<CommandId, List<Type>>();
            _handlerConflicts = new ConcurrentDictionary<string, List<Type>>();
        }

        #region 注册信息类

        /// <summary>
        /// 注册信息
        /// </summary>
        public class RegistrationInfo
        {
            /// <summary>
            /// 注册类型
            /// </summary>
            public Type RegisteredType { get; set; }
            
            /// <summary>
            /// 注册时间
            /// </summary>
            public DateTime RegistrationTime { get; set; }
            
            /// <summary>
            /// 注册状态
            /// </summary>
            public RegistrationStatus Status { get; set; }
            
            /// <summary>
            /// 元数据信息
            /// </summary>
            public Dictionary<string, object> Metadata { get; set; }
            
            /// <summary>
            /// 来源描述
            /// </summary>
            public string Source { get; set; }
        }

        /// <summary>
        /// 注册状态枚举
        /// </summary>
        public enum RegistrationStatus
        {
            /// <summary>
            /// 待注册
            /// </summary>
            Pending,
            
            /// <summary>
            /// 已注册
            /// </summary>
            Registered,
            
            /// <summary>
            /// 注册失败
            /// </summary>
            Failed,
            
            /// <summary>
            /// 已注销
            /// </summary>
            Unregistered
        }

        /// <summary>
        /// 待注册项
        /// </summary>
        public class PendingRegistration
        {
            /// <summary>
            /// 注册类型
            /// </summary>
            public RegistrationType Type { get; set; }
            
            /// <summary>
            /// 要注册的类型
            /// </summary>
            public Type TargetType { get; set; }
            
            /// <summary>
            /// 关联的命令ID
            /// </summary>
            public CommandId CommandId { get; set; }
            
            /// <summary>
            /// 优先级
            /// </summary>
            public int Priority { get; set; }
            
            /// <summary>
            /// 元数据
            /// </summary>
            public Dictionary<string, object> Metadata { get; set; }
            
            /// <summary>
            /// 注册回调
            /// </summary>
            public Action<RegistrationResult> Callback { get; set; }
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        public enum RegistrationType
        {
            /// <summary>
            /// 命令类型
            /// </summary>
            Command,
            
            /// <summary>
            /// 处理器类型
            /// </summary>
            Handler
        }

        /// <summary>
        /// 注册结果
        /// </summary>
        public class RegistrationResult
        {
            /// <summary>
            /// 是否成功
            /// </summary>
            public bool Success { get; set; }
            
            /// <summary>
            /// 注册信息
            /// </summary>
            public RegistrationInfo Info { get; set; }
            
            /// <summary>
            /// 错误信息
            /// </summary>
            public string ErrorMessage { get; set; }
            
            /// <summary>
            /// 冲突信息
            /// </summary>
            public List<string> Conflicts { get; set; }
        }

        #endregion

        #region 事件参数类

        /// <summary>
        /// 注册事件参数
        /// </summary>
        public class RegistrationEventArgs : EventArgs
        {
            /// <summary>
            /// 注册类型
            /// </summary>
            public RegistrationType RegistrationType { get; set; }
            
            /// <summary>
            /// 注册信息
            /// </summary>
            public RegistrationInfo Info { get; set; }
            
            /// <summary>
            /// 时间戳
            /// </summary>
            public DateTime Timestamp { get; set; }
        }

        /// <summary>
        /// 注册冲突事件参数
        /// </summary>
        public class RegistrationConflictEventArgs : EventArgs
        {
            /// <summary>
            /// 冲突类型
            /// </summary>
            public RegistrationType ConflictType { get; set; }
            
            /// <summary>
            /// 冲突的命令ID或处理器名称
            /// </summary>
            public string ConflictKey { get; set; }
            
            /// <summary>
            /// 已存在的类型
            /// </summary>
            public Type ExistingType { get; set; }
            
            /// <summary>
            /// 新类型
            /// </summary>
            public Type NewType { get; set; }
            
            /// <summary>
            /// 冲突描述
            /// </summary>
            public string ConflictDescription { get; set; }
            
            /// <summary>
            /// 解决建议
            /// </summary>
            public List<string> Suggestions { get; set; }
        }

        #endregion

        #region 命令类型注册

        /// <summary>
        /// 注册命令类型
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="metadata">元数据</param>
        /// <returns>注册结果</returns>
        public async Task<RegistrationResult> RegisterCommandTypeAsync(CommandId commandId, Type commandType, Dictionary<string, object> metadata = null)
        {
            if (commandId == null)
                return new RegistrationResult { Success = false, ErrorMessage = "命令ID不能为空" };
            
            if (commandType == null)
                return new RegistrationResult { Success = false, ErrorMessage = "命令类型不能为空" };
            
            if (!typeof(ICommand).IsAssignableFrom(commandType))
                return new RegistrationResult { Success = false, ErrorMessage = "类型必须实现ICommand接口" };

            await _registrationSemaphore.WaitAsync();
            try
            {
                // 检查冲突
                var conflicts = CheckCommandConflicts(commandId, commandType);
                if (conflicts.Any())
                {
                    var conflictArgs = new RegistrationConflictEventArgs
                    {
                        ConflictType = RegistrationType.Command,
                        ConflictKey = commandId.ToString(),
                        ExistingType = _registrationStatus.ContainsKey(commandId) ? _registrationStatus[commandId].RegisteredType : null,
                        NewType = commandType,
                        ConflictDescription = $"命令ID {commandId} 存在冲突",
                        Suggestions = conflicts
                    };
                    
                    RegistrationConflict?.Invoke(this, conflictArgs);
                    
                    return new RegistrationResult 
                    { 
                        Success = false, 
                        ErrorMessage = $"命令ID {commandId} 注册冲突",
                        Conflicts = conflicts
                    };
                }

                // 执行注册
                var registrationInfo = new RegistrationInfo
                {
                    RegisteredType = commandType,
                    RegistrationTime = DateTime.Now,
                    Status = RegistrationStatus.Registered,
                    Metadata = metadata ?? new Dictionary<string, object>(),
                    Source = "DirectRegistration"
                };

                // 更新缓存
                _cacheManager.CacheCommandType(commandId, commandType);
                
                // 更新注册状态
                _registrationStatus.AddOrUpdate(commandId, registrationInfo, (key, old) => registrationInfo);

                // 触发事件
                CommandRegistered?.Invoke(this, new RegistrationEventArgs
                {
                    RegistrationType = RegistrationType.Command,
                    Info = registrationInfo,
                    Timestamp = DateTime.Now
                });

                _logger?.LogInformation("命令类型注册成功: {CommandId} -> {TypeName}", commandId, commandType.Name);

                return new RegistrationResult { Success = true, Info = registrationInfo };
            }
            finally
            {
                _registrationSemaphore.Release();
            }
        }

        /// <summary>
        /// 批量注册命令类型
        /// </summary>
        /// <param name="commandTypes">命令类型字典</param>
        /// <param name="metadata">元数据</param>
        /// <returns>注册结果列表</returns>
        public async Task<List<RegistrationResult>> RegisterCommandTypesAsync(Dictionary<CommandId, Type> commandTypes, Dictionary<string, object> metadata = null)
        {
            var results = new List<RegistrationResult>();
            
            foreach (var kvp in commandTypes)
            {
                var result = await RegisterCommandTypeAsync(kvp.Key, kvp.Value, metadata);
                results.Add(result);
            }
            
            return results;
        }

        /// <summary>
        /// 注销命令类型
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>是否成功</returns>
        public async Task<bool> UnregisterCommandTypeAsync(CommandId commandId)
        {
            if (commandId == null) return false;

            await _registrationSemaphore.WaitAsync();
            try
            {
                if (_registrationStatus.TryRemove(commandId, out var registrationInfo))
                {
                    registrationInfo.Status = RegistrationStatus.Unregistered;
                    
                    // 触发事件
                    CommandUnregistered?.Invoke(this, new RegistrationEventArgs
                    {
                        RegistrationType = RegistrationType.Command,
                        Info = registrationInfo,
                        Timestamp = DateTime.Now
                    });

                    _logger?.LogInformation("命令类型注销成功: {CommandId}", commandId);
                    return true;
                }
                
                return false;
            }
            finally
            {
                _registrationSemaphore.Release();
            }
        }

        #endregion

        #region 处理器注册

        /// <summary>
        /// 注册命令处理器
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <param name="metadata">元数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>注册结果</returns>
        public async Task<RegistrationResult> RegisterHandlerTypeAsync(Type handlerType, Dictionary<string, object> metadata = null, CancellationToken cancellationToken = default)
        {
            if (handlerType == null)
                return new RegistrationResult { Success = false, ErrorMessage = "处理器类型不能为空" };
            
            if (!typeof(ICommandHandler).IsAssignableFrom(handlerType))
                return new RegistrationResult { Success = false, ErrorMessage = "类型必须实现ICommandHandler接口" };

            await _registrationSemaphore.WaitAsync();
            try
            {
                var handlerName = handlerType.FullName;
                
                // 检查冲突
                var conflicts = CheckHandlerConflicts(handlerName, handlerType);
                if (conflicts.Any())
                {
                    var conflictArgs = new RegistrationConflictEventArgs
                    {
                        ConflictType = RegistrationType.Handler,
                        ConflictKey = handlerName,
                        ExistingType = _handlerRegistrationStatus.ContainsKey(handlerName) ? _handlerRegistrationStatus[handlerName].RegisteredType : null,
                        NewType = handlerType,
                        ConflictDescription = $"处理器 {handlerName} 存在冲突",
                        Suggestions = conflicts
                    };
                    
                    RegistrationConflict?.Invoke(this, conflictArgs);
                    
                    return new RegistrationResult 
                    { 
                        Success = false, 
                        ErrorMessage = $"处理器 {handlerName} 注册冲突",
                        Conflicts = conflicts
                    };
                }

                // 执行注册
                var registrationInfo = new RegistrationInfo
                {
                    RegisteredType = handlerType,
                    RegistrationTime = DateTime.Now,
                    Status = RegistrationStatus.Registered,
                    Metadata = metadata ?? new Dictionary<string, object>(),
                    Source = "DirectRegistration"
                };

                // 更新缓存
                var handlerAttribute = handlerType.GetCustomAttribute<CommandHandlerAttribute>();
                if (handlerAttribute?.Priority > 0)
                {
                    metadata = metadata ?? new Dictionary<string, object>();
                    metadata["Priority"] = handlerAttribute.Priority;
                }
                
                // 更新注册状态
                _handlerRegistrationStatus.AddOrUpdate(handlerName, registrationInfo, (key, old) => registrationInfo);

                // 更新缓存
                await _cacheManager.CacheCommandHandlerAsync(handlerType, cancellationToken);

                // 触发事件
                HandlerRegistered?.Invoke(this, new RegistrationEventArgs
                {
                    RegistrationType = RegistrationType.Handler,
                    Info = registrationInfo,
                    Timestamp = DateTime.Now
                });

                _logger?.LogInformation("处理器类型注册成功: {TypeName}", handlerType.Name);

                return new RegistrationResult { Success = true, Info = registrationInfo };
            }
            finally
            {
                _registrationSemaphore.Release();
            }
        }

        /// <summary>
        /// 批量注册处理器类型
        /// </summary>
        /// <param name="handlerTypes">处理器类型列表</param>
        /// <param name="metadata">元数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>注册结果列表</returns>
        public async Task<List<RegistrationResult>> RegisterHandlerTypesAsync(List<Type> handlerTypes, Dictionary<string, object> metadata = null, CancellationToken cancellationToken = default)
        {
            var results = new List<RegistrationResult>();
            
            foreach (var handlerType in handlerTypes)
            {
                var result = await RegisterHandlerTypeAsync(handlerType, metadata, cancellationToken);
                results.Add(result);
            }
            
            return results;
        }

        /// <summary>
        /// 批量注册命令类型（兼容CommandScanner的调用）
        /// </summary>
        /// <param name="commandTypes">命令类型字典</param>
        /// <param name="metadata">元数据</param>
        /// <returns>注册结果列表</returns>
        public async Task<List<RegistrationResult>> RegisterCommandsAsync(Dictionary<CommandId, Type> commandTypes, Dictionary<string, object> metadata = null)
        {
            var results = new List<RegistrationResult>();
            
            foreach (var kvp in commandTypes)
            {
                var result = await RegisterCommandTypeAsync(kvp.Key, kvp.Value, metadata);
                results.Add(result);
            }
            
            return results;
        }

        /// <summary>
        /// 批量注册命令类型（兼容CommandScanner的调用）
        /// </summary>
        /// <param name="commandTypes">命令类型列表</param>
        /// <param name="metadata">元数据</param>
        /// <returns>注册结果列表</returns>
        public async Task<List<RegistrationResult>> RegisterCommandsAsync(List<Type> commandTypes, Dictionary<string, object> metadata = null)
        {
            var results = new List<RegistrationResult>();
            
            foreach (var commandType in commandTypes)
            {
                // 提取命令ID
                var commandId = ExtractCommandIdFromType(commandType);
                if (commandId != null)
                {
                    var result = await RegisterCommandTypeAsync(commandId.Value, commandType, metadata);
                    results.Add(result);
                }
                else
                {
                    results.Add(new RegistrationResult 
                    { 
                        Success = false, 
                        ErrorMessage = $"无法从类型 {commandType.FullName} 提取命令ID" 
                    });
                }
            }
            
            return results;
        }

        /// <summary>
        /// 批量注册处理器类型（兼容CommandScanner的调用）
        /// </summary>
        /// <param name="handlerTypes">处理器类型列表</param>
        /// <param name="metadata">元数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>注册结果列表</returns>
        public async Task<List<RegistrationResult>> RegisterHandlersAsync(List<Type> handlerTypes, Dictionary<string, object> metadata = null, CancellationToken cancellationToken = default)
        {
            return await RegisterHandlerTypesAsync(handlerTypes, metadata, cancellationToken);
        }

        /// <summary>
        /// 从类型中提取命令ID
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>命令ID，如果提取失败则返回null</returns>
        private CommandId? ExtractCommandIdFromType(Type type)
        {
            if (type == null) return null;

            try
            {
                // 尝试从类型名解析（主要方式）
                var typeName = type.Name;
                if (typeName.EndsWith("Command"))
                {
                    var commandName = typeName.Substring(0, typeName.Length - 7);
                    if (Enum.TryParse<CommandId>(commandName, out var commandId))
                    {
                        return commandId;
                    }
                }

                // 尝试获取PacketCommandAttribute中的名称
                var commandAttr = type.GetCustomAttribute<PacketCommandAttribute>();
                if (commandAttr != null && !string.IsNullOrEmpty(commandAttr.Name))
                {
                    if (Enum.TryParse<CommandId>(commandAttr.Name, out var commandId))
                    {
                        return commandId;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "从类型 {TypeName} 提取命令ID失败", type.FullName);
                return null;
            }
        }

        /// <summary>
        /// 注销处理器类型
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <returns>是否成功</returns>
        public async Task<bool> UnregisterHandlerTypeAsync(Type handlerType)
        {
            if (handlerType == null) return false;

            var handlerName = handlerType.FullName;
            return await UnregisterHandlerByNameAsync(handlerName);
        }

        /// <summary>
        /// 根据名称注销处理器
        /// </summary>
        /// <param name="handlerName">处理器名称</param>
        /// <returns>是否成功</returns>
        public async Task<bool> UnregisterHandlerByNameAsync(string handlerName)
        {
            if (string.IsNullOrEmpty(handlerName)) return false;

            await _registrationSemaphore.WaitAsync();
            try
            {
                if (_handlerRegistrationStatus.TryRemove(handlerName, out var registrationInfo))
                {
                    registrationInfo.Status = RegistrationStatus.Unregistered;
                    
                    // 触发事件
                    HandlerUnregistered?.Invoke(this, new RegistrationEventArgs
                    {
                        RegistrationType = RegistrationType.Handler,
                        Info = registrationInfo,
                        Timestamp = DateTime.Now
                    });

                    _logger?.LogInformation("处理器类型注销成功: {TypeName}", handlerName);
                    return true;
                }
                
                return false;
            }
            finally
            {
                _registrationSemaphore.Release();
            }
        }

        #endregion

        #region 延迟注册机制

        /// <summary>
        /// 添加延迟注册项
        /// </summary>
        /// <param name="registration">待注册项</param>
        public void AddPendingRegistration(PendingRegistration registration)
        {
            if (registration == null) return;
            
            _pendingRegistrations.Enqueue(registration);
            _logger?.LogDebug("添加延迟注册项: {Type} - {TargetType}", registration.Type, registration.TargetType?.Name);
        }

        /// <summary>
        /// 处理延迟注册队列
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        public async Task<Dictionary<string, RegistrationResult>> ProcessPendingRegistrationsAsync(CancellationToken cancellationToken = default)
        {
            var results = new Dictionary<string, RegistrationResult>();
            var processedCount = 0;
            var failedCount = 0;

            while (_pendingRegistrations.TryDequeue(out var pendingRegistration))
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                try
                {
                    RegistrationResult result = null;
                    var key = $"{pendingRegistration.Type}_{pendingRegistration.TargetType?.FullName}";

                    switch (pendingRegistration.Type)
                    {
                        case RegistrationType.Command:
                            result = await RegisterCommandTypeAsync(
                                pendingRegistration.CommandId, 
                                pendingRegistration.TargetType, 
                                pendingRegistration.Metadata);
                            break;
                            
                        case RegistrationType.Handler:
                            result = await RegisterHandlerTypeAsync(
                                pendingRegistration.TargetType, 
                                pendingRegistration.Metadata);
                            break;
                    }

                    if (result != null)
                    {
                        results[key] = result;
                        if (result.Success)
                            processedCount++;
                        else
                            failedCount++;
                    }

                    // 执行回调
                    pendingRegistration.Callback?.Invoke(result);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "处理延迟注册项失败: {Type} - {TargetType}", 
                        pendingRegistration.Type, pendingRegistration.TargetType?.Name);
                    failedCount++;
                }
            }

            _logger?.LogInformation("延迟注册处理完成: 成功 {SuccessCount}, 失败 {FailedCount}", processedCount, failedCount);
            return results;
        }

        /// <summary>
        /// 获取待注册队列数量
        /// </summary>
        /// <returns>待注册数量</returns>
        public int GetPendingRegistrationCount()
        {
            return _pendingRegistrations.Count;
        }

        /// <summary>
        /// 清空待注册队列
        /// </summary>
        public void ClearPendingRegistrations()
        {
            while (_pendingRegistrations.TryDequeue(out _)) { }
            _logger?.LogInformation("已清空延迟注册队列");
        }

        #endregion

        #region 冲突检测

        /// <summary>
        /// 检查命令类型冲突
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="newType">新类型</param>
        /// <returns>冲突信息列表</returns>
        private List<string> CheckCommandConflicts(CommandId commandId, Type newType)
        {
            var conflicts = new List<string>();
            
            if (_registrationStatus.TryGetValue(commandId, out var existingInfo))
            {
                var existingType = existingInfo.RegisteredType;
                
                if (existingType != newType)
                {
                    conflicts.Add($"命令ID {commandId} 已注册到类型 {existingType.FullName}");
                    conflicts.Add($"建议: 检查命令ID定义或考虑使用不同的ID");
                    
                    // 记录冲突
                    _commandConflicts.AddOrUpdate(commandId, 
                        new List<Type> { existingType, newType }, 
                        (key, list) => 
                        {
                            if (!list.Contains(newType))
                                list.Add(newType);
                            return list;
                        });
                }
            }
            
            return conflicts;
        }

        /// <summary>
        /// 检查处理器类型冲突
        /// </summary>
        /// <param name="handlerName">处理器名称</param>
        /// <param name="newType">新类型</param>
        /// <returns>冲突信息列表</returns>
        private List<string> CheckHandlerConflicts(string handlerName, Type newType)
        {
            var conflicts = new List<string>();
            
            if (_handlerRegistrationStatus.TryGetValue(handlerName, out var existingInfo))
            {
                var existingType = existingInfo.RegisteredType;
                
                if (existingType != newType)
                {
                    conflicts.Add($"处理器名称 {handlerName} 已注册到类型 {existingType.FullName}");
                    conflicts.Add($"建议: 检查处理器命名或考虑合并功能");
                    
                    // 记录冲突
                    _handlerConflicts.AddOrUpdate(handlerName, 
                        new List<Type> { existingType, newType }, 
                        (key, list) => 
                        {
                            if (!list.Contains(newType))
                                list.Add(newType);
                            return list;
                        });
                }
            }
            
            return conflicts;
        }

        /// <summary>
        /// 获取命令冲突信息
        /// </summary>
        /// <returns>冲突字典</returns>
        public IReadOnlyDictionary<CommandId, List<Type>> GetCommandConflicts()
        {
            return new Dictionary<CommandId, List<Type>>(_commandConflicts);
        }

        /// <summary>
        /// 获取处理器冲突信息
        /// </summary>
        /// <returns>冲突字典</returns>
        public IReadOnlyDictionary<string, List<Type>> GetHandlerConflicts()
        {
            return new Dictionary<string, List<Type>>(_handlerConflicts);
        }

        /// <summary>
        /// 清理冲突记录
        /// </summary>
        public void ClearConflicts()
        {
            _commandConflicts.Clear();
            _handlerConflicts.Clear();
            _logger?.LogInformation("已清理冲突记录");
        }

        #endregion

        #region 查询和统计

        /// <summary>
        /// 获取注册统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public RegistrationStatistics GetRegistrationStatistics()
        {
            var stats = new RegistrationStatistics
            {
                TotalCommands = _registrationStatus.Count,
                TotalHandlers = _handlerRegistrationStatus.Count,
                PendingRegistrations = _pendingRegistrations.Count,
                CommandConflicts = _commandConflicts.Count,
                HandlerConflicts = _handlerConflicts.Count,
                LastUpdateTime = DateTime.Now
            };

            // 统计状态分布
            foreach (var info in _registrationStatus.Values)
            {
                switch (info.Status)
                {
                    case RegistrationStatus.Registered:
                        stats.RegisteredCommands++;
                        break;
                    case RegistrationStatus.Failed:
                        stats.FailedCommands++;
                        break;
                    case RegistrationStatus.Unregistered:
                        stats.UnregisteredCommands++;
                        break;
                }
            }

            foreach (var info in _handlerRegistrationStatus.Values)
            {
                switch (info.Status)
                {
                    case RegistrationStatus.Registered:
                        stats.RegisteredHandlers++;
                        break;
                    case RegistrationStatus.Failed:
                        stats.FailedHandlers++;
                        break;
                    case RegistrationStatus.Unregistered:
                        stats.UnregisteredHandlers++;
                        break;
                }
            }

            return stats;
        }

        /// <summary>
        /// 注册统计信息类
        /// </summary>
        public class RegistrationStatistics
        {
            /// <summary>
            /// 总命令数
            /// </summary>
            public int TotalCommands { get; set; }
            
            /// <summary>
            /// 总处理器数
            /// </summary>
            public int TotalHandlers { get; set; }
            
            /// <summary>
            /// 已注册命令数
            /// </summary>
            public int RegisteredCommands { get; set; }
            
            /// <summary>
            /// 已注册处理器数
            /// </summary>
            public int RegisteredHandlers { get; set; }
            
            /// <summary>
            /// 注册失败命令数
            /// </summary>
            public int FailedCommands { get; set; }
            
            /// <summary>
            /// 注册失败处理器数
            /// </summary>
            public int FailedHandlers { get; set; }
            
            /// <summary>
            /// 已注销命令数
            /// </summary>
            public int UnregisteredCommands { get; set; }
            
            /// <summary>
            /// 已注销处理器数
            /// </summary>
            public int UnregisteredHandlers { get; set; }
            
            /// <summary>
            /// 待注册数
            /// </summary>
            public int PendingRegistrations { get; set; }
            
            /// <summary>
            /// 命令冲突数
            /// </summary>
            public int CommandConflicts { get; set; }
            
            /// <summary>
            /// 处理器冲突数
            /// </summary>
            public int HandlerConflicts { get; set; }
            
            /// <summary>
            /// 最后更新时间
            /// </summary>
            public DateTime LastUpdateTime { get; set; }
        }

        #endregion

        #region 清理和重置

        /// <summary>
        /// 清理所有注册信息
        /// </summary>
        public async Task ClearAllAsync()
        {
            await _registrationSemaphore.WaitAsync();
            try
            {
                _registrationStatus.Clear();
                _handlerRegistrationStatus.Clear();
                
                // ConcurrentQueue 没有 Clear 方法，需要逐个出队
                while (_pendingRegistrations.TryDequeue(out _))
                {
                }
                
                _commandConflicts.Clear();
                _handlerConflicts.Clear();
                
                _logger?.LogInformation("已清理所有注册信息");
            }
            finally
            {
                _registrationSemaphore.Release();
            }
        }

        /// <summary>
        /// 清理命令注册
        /// </summary>
        public void ClearCommandRegistrations()
        {
            _registrationStatus.Clear();
            _commandConflicts.Clear();
            _logger?.LogInformation("已清理命令注册信息");
        }

        /// <summary>
        /// 清理处理器注册
        /// </summary>
        public void ClearHandlerRegistrations()
        {
            _handlerRegistrationStatus.Clear();
            _handlerConflicts.Clear();
            _logger?.LogInformation("已清理处理器注册信息");
        }

        #endregion
    }
}
