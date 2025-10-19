﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Utilities;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Text;
using RUINORERP.Global.CustomAttribute;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models.Core;
using MessagePack;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令处理器基类 - 提供命令处理器的通用实现
    /// </summary>
    public abstract class BaseCommandHandler : ICommandHandler, ITimestamped
    {
        // 定义结构化日志消息
        private static readonly Action<ILogger, string, long, bool, Exception> _logHandled =
            LoggerMessage.Define<string, long, bool>(LogLevel.Information, new EventId(1001, "Handled"),
                "Command {CommandId} handled in {Elapsed}ms, Success: {Success}");

        private readonly object _lockObject = new object();
        private bool _disposed = false;
        private readonly HandlerStatistics _statistics;

        /// <summary>
        /// 处理器唯一标识
        /// </summary>
        public string HandlerId { get; private set; }

        public int Priority { get; set; }

        #region ITimestamped 接口实现
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 时间戳（UTC时间）
        /// 记录对象的当前状态时间点，会随着对象状态变化而更新
        /// </summary>
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;


        /// <summary>
        /// 更新时间戳（实现 ITimestamped 接口）
        /// </summary>
        public void UpdateTimestamp()
        {
            TimestampUtc = DateTime.UtcNow;
        }
        #endregion

        /// <summary>
        /// 验证模型有效性（实现 ICoreEntity 接口）
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return CreatedTime <= DateTime.Now &&
                   CreatedTime >= DateTime.Now.AddYears(-1); // 创建时间在1年内
        }

        /// <summary>
        /// 处理器名称
        /// </summary>
        public virtual string Name => this.GetType().Name;

        /// <summary>
        /// 是否已初始化
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 支持的命令类型 - 使用CommandId类型提供更好的类型安全性和可读性
        /// 提供默认实现，避免空引用异常
        /// </summary>
        public virtual IReadOnlyList<CommandId> SupportedCommands { get; protected set; } = Array.Empty<CommandId>();
        

        /// <summary>
        /// 安全设置支持的命令列表 - CommandId版本
        /// </summary>
        /// <param name="commands">CommandId命令对象数组</param>
        protected void SetSupportedCommands(params CommandId[] commands)
        {
            if (commands == null || commands.Length == 0)
            {
                SupportedCommands = Array.Empty<CommandId>();
                Logger?.LogDebug($"处理器 {Name} 设置支持 0 个命令");
                return;
            }
            
            SupportedCommands = commands.ToList();
            // 减少调试日志，在生产环境中不记录
            #if DEBUG
            Logger?.LogDebug($"处理器 {Name} 设置支持 {commands.Length} 个命令: {string.Join(", ", commands.Select(c => $"{c.Name}(0x{c.FullCode:X4})"))}");
            #endif
        }

        /// <summary>
        /// 安全设置支持的命令列表 - CommandId集合版本
        /// </summary>
        /// <param name="commands">CommandId命令对象集合</param>
        protected void SetSupportedCommands(IEnumerable<CommandId> commands)
        {
            if (commands == null)
            {
                SupportedCommands = Array.Empty<CommandId>();
                Logger?.LogDebug($"处理器 {Name} 设置支持 0 个命令");
                return;
            }
            
            var commandList = commands.ToList();
            if (commandList.Count == 0)
            {
                SupportedCommands = Array.Empty<CommandId>();
                Logger?.LogDebug($"处理器 {Name} 设置支持 0 个命令");
                return;
            }
            
            SupportedCommands = commandList;
            Logger?.LogDebug($"处理器 {Name} 设置支持 {commandList.Count} 个命令: {string.Join(", ", commandList.Select(c => $"{c.Name}(0x{c.FullCode:X4})"))}");
        }

       
        /// <summary>
        /// 处理器状态
        /// </summary>
        public HandlerStatus Status { get; private set; } = HandlerStatus.Uninitialized;

        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger<BaseCommandHandler> Logger { get; set; }

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        protected BaseCommandHandler() : this(new LoggerFactory().CreateLogger<BaseCommandHandler>())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseCommandHandler(ILogger<BaseCommandHandler> _logger)
        {
            HandlerId = IdGenerator.GenerateHandlerId(this.GetType().Name);
            _statistics = new HandlerStatistics();
            Logger = _logger;
            // 初始化ITimestamped属性
            CreatedTime = DateTime.Now;
            TimestampUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// 异步处理命令 - 统一命令处理流程
        /// </summary>
        public async Task<BaseCommand<IRequest, IResponse>> HandleAsync(QueuedCommand cmd, CancellationToken cancellationToken = default)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            // 获取执行超时时间
            var executionTimeoutMs = GetExecutionTimeoutMs(cmd);
            
            using var executionCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(executionTimeoutMs));
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, executionCts.Token);
            
            var startTime = DateTime.UtcNow;
            bool success = true;

            try
            {
                // 执行统一的命令预处理流程
                var preprocessResult = await ExecuteCommandPreprocessingAsync(cmd, linkedCts.Token);
                if (preprocessResult != null)
                    return preprocessResult;

                // 执行命令前置处理
                var beforeResult = await OnBeforeHandleAsync(cmd, linkedCts.Token);
                if (beforeResult != null)
                    return beforeResult;

                // 执行核心命令处理
                var result = await OnHandleAsync(cmd, linkedCts.Token);

                // 执行命令后置处理
                var afterResult = await OnAfterHandleAsync(cmd, result, linkedCts.Token);
                if (afterResult != null)
                    return afterResult;

                // 检查是否接近超时
                var executionTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
                if (executionTime > executionTimeoutMs * 0.8) // 超过80%时间发出警告
                {
                    Logger.LogWarning($"命令 {cmd.Command.CommandIdentifier} 执行接近超时: {executionTime}ms");
                }

                // 更新统计信息
                UpdateStatistics(result.Response?.IsSuccess ?? false, (long)(DateTime.UtcNow - startTime).TotalMilliseconds);

                return result;
            }
            catch (OperationCanceledException) when (executionCts.IsCancellationRequested)
            {
                var executionTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
                Logger.LogWarning($"命令执行超时: {cmd.Command.CommandIdentifier}, 设置: {executionTimeoutMs}ms, 实际: {executionTime}ms");
                
                // 更新超时统计信息
                UpdateTimeoutStatistics();
                
                success = false;
                return BaseCommand<IRequest, IResponse>.CreateError($"业务处理超时: {executionTimeoutMs}ms", 408);
            }
            catch (OperationCanceledException)
            {
                success = false;
                return BaseCommand<IRequest, IResponse>.CreateError(UnifiedErrorCodes.Command_ProcessCancelled.Message, UnifiedErrorCodes.Command_ProcessCancelled.Code);
            }
            catch (Exception ex)
            {
                success = false;
                return HandleCommandException(ex);
            }
            finally
            {
                var elapsed = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
                _logHandled(Logger, cmd.Command.ToString(), elapsed, success, null);
            }
        }

        /// <summary>
        /// 统一的命令预处理流程 - 提取为独立方法以便复用
        /// </summary>
        protected async Task<BaseCommand<IRequest, IResponse>> ExecuteCommandPreprocessingAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            // 验证命令
            var validationResult = await cmd.Command.ValidateAsync(cancellationToken);
            if (!validationResult.IsValid)
            {
                Logger.LogDebug($"命令验证失败: {validationResult.Errors[0].ErrorMessage}");
                return BaseCommand<IRequest, IResponse>.CreateValidationError(validationResult);
            }

            // 验证会话
            if (!ValidateSession(cmd.Packet?.ExecutionContext?.SessionId))
            {
                Logger.LogDebug($"会话验证失败: {cmd.Packet?.ExecutionContext?.SessionId}");
                return BaseCommand<IRequest, IResponse>.CreateError("会话无效或未认证", UnifiedErrorCodes.Auth_SessionExpired.Code)
                    .WithMetadata("ErrorCode", "INVALID_SESSION");
            }

            // 注意：命令超时检查已移除，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理

            return null; // 预处理通过
        }

        /// <summary>
        /// 验证会话是否有效
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>会话是否有效</returns>
        protected virtual bool ValidateSession(string sessionId)
        {
            // 默认实现返回true，具体子类可重写
            return true;
        }

 

        /// <summary>
        /// 统一的异常处理
        /// </summary>
        protected BaseCommand<IRequest, IResponse> HandleCommandException(Exception ex)
        {
            var errorCode = UnifiedErrorCodes.Command_ExecuteFailed.Code == 0 ? UnifiedErrorCodes.System_InternalError : UnifiedErrorCodes.Command_ExecuteFailed;
            Logger.LogError(ex, $"命令处理异常: {ex.Message}");
            return BaseCommand<IRequest, IResponse>.CreateError($"[{ex.GetType().Name}] {ex.Message}", errorCode.Code)
                .WithMetadata("StackTrace", ex.StackTrace);
        }

        /// <summary>
        /// 判断是否可以处理该命令 - 使用CommandId进行判断
        /// </summary>
        public virtual bool CanHandle(QueuedCommand cmd)
        {
            if (cmd.Command == null || _disposed || Status != HandlerStatus.Running)
                return false;

            // 安全处理SupportedCommands为null或空的情况
            var supportedCommands = SupportedCommands ?? Array.Empty<CommandId>();
            return supportedCommands.Contains(cmd.Command.CommandIdentifier);
        }

        /// <summary>
        /// 判断是否可以处理该命令 - uint版本（向后兼容）
        /// </summary>
        /// <param name="commandCode">命令代码（uint格式）</param>
        /// <returns>是否可以处理</returns>
        public virtual bool CanHandle(uint commandCode)
        {
            if (_disposed || Status != HandlerStatus.Running)
                return false;

            // 安全处理SupportedCommands为null或空的情况
            var supportedCommands = SupportedCommands ?? Array.Empty<CommandId>();
            return supportedCommands.Any(cmd => cmd.FullCode == commandCode);
        }

        /// <summary>
        /// 处理器初始化
        /// </summary>
        public virtual async Task<bool> InitializeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (IsInitialized)
                    return true;


                LogInfo($"初始化处理器: {Name}");

                var result = await OnInitializeAsync(cancellationToken);
                if (result)
                {
                    IsInitialized = true;
                    Status = HandlerStatus.Initialized;

                    LogInfo($"处理器初始化成功: {Name}");
                }
                else
                {
                    Status = HandlerStatus.Error;

                    LogError($"处理器初始化失败: {Name}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Status = HandlerStatus.Error;

                LogError($"处理器初始化异常: {Name}", ex);
                return false;
            }
        }

        /// <summary>
        /// 处理器启动
        /// </summary>
        public virtual async Task<bool> StartAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (!IsInitialized)
                {
                    var initResult = await InitializeAsync(cancellationToken);
                    if (!initResult)
                        return false;
                }

                if (Status == HandlerStatus.Running)
                    return true;


                LogInfo($"启动处理器: {Name}");

                var result = await OnStartAsync(cancellationToken);
                if (result)
                {
                    Status = HandlerStatus.Running;
                    _statistics.StartTimeUtc = DateTime.UtcNow;

                    LogInfo($"处理器启动成功: {Name}");
                }
                else
                {
                    Status = HandlerStatus.Error;

                    LogError($"处理器启动失败: {Name}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Status = HandlerStatus.Error;

                LogError($"处理器启动异常: {Name}", ex);
                return false;
            }
        }

        /// <summary>
        /// 处理器停止
        /// </summary>
        public virtual async Task<bool> StopAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (Status == HandlerStatus.Stopped || Status == HandlerStatus.Disposed)
                    return true;


                LogInfo($"停止处理器: {Name}");

                var result = await OnStopAsync(cancellationToken);
                if (result)
                {
                    Status = HandlerStatus.Stopped;

                    LogInfo($"处理器停止成功: {Name}");
                }
                else
                {

                    LogError($"处理器停止失败: {Name}");
                }

                return result;
            }
            catch (Exception ex)
            {

                LogError($"处理器停止异常: {Name}", ex);
                return false;
            }
        }




        /// <summary>
        /// 获取处理器统计信息
        /// </summary>
        public HandlerStatistics GetStatistics()
        {
            lock (_lockObject)
            {
                return new HandlerStatistics
                {
                    StartTimeUtc = _statistics.StartTimeUtc,
                    TotalCommandsProcessed = _statistics.TotalCommandsProcessed,
                    SuccessfulCommands = _statistics.SuccessfulCommands,
                    FailedCommands = _statistics.FailedCommands,
                    AverageProcessingTimeMs = _statistics.AverageProcessingTimeMs,
                    MaxProcessingTimeMs = _statistics.MaxProcessingTimeMs,
                    MinProcessingTimeMs = _statistics.MinProcessingTimeMs,
                    LastProcessTime = _statistics.LastProcessTime,
                    CurrentProcessingCount = _statistics.CurrentProcessingCount
                };
            }
        }

        /// <summary>
        /// 重置处理器统计信息
        /// </summary>
        public void ResetStatistics()
        {
            lock (_lockObject)
            {
                _statistics.TotalCommandsProcessed = 0;
                _statistics.SuccessfulCommands = 0;
                _statistics.FailedCommands = 0;
                _statistics.AverageProcessingTimeMs = 0;
                _statistics.MaxProcessingTimeMs = 0;
                _statistics.MinProcessingTimeMs = 0;
                _statistics.CurrentProcessingCount = 0;
                _statistics.StartTimeUtc = DateTime.UtcNow;
            }
        }

        #region 抽象方法 - 子类必须实现

        /// <summary>
        /// 执行核心处理逻辑
        /// </summary>
        protected abstract Task<BaseCommand<IRequest, IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken);

        /// <summary>
        /// 基于命令类型确定处理超时
        /// </summary>
        /// <param name="cmd">排队命令</param>
        /// <returns>执行超时毫秒数</returns>
        protected virtual int GetExecutionTimeoutMs(QueuedCommand cmd)
        {
            var baseTimeout = cmd.Command.CommandIdentifier.Category switch
            {
                CommandCategory.Authentication => 10000,    // 认证: 10秒
                CommandCategory.Cache => 5000,              // 缓存: 5秒  
                CommandCategory.File => 30000,              // 文件: 30秒
                CommandCategory.DataSync => 60000,          // 数据同步: 60秒
                _ => 15000                                  // 默认: 15秒
            };
            
            // 优先级已移到数据包中，不再根据命令优先级调整超时
            return baseTimeout;
        }

 

        #endregion

        #region 虚方法 - 子类可以重写

        /// <summary>
        /// 初始化处理器
        /// </summary>
        protected virtual Task<bool> OnInitializeAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 启动处理器
        /// </summary>
        protected virtual Task<bool> OnStartAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 停止处理器
        /// </summary>
        protected virtual Task<bool> OnStopAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 执行前置处理
        /// </summary>
        protected virtual Task<BaseCommand<IRequest, IResponse>> OnBeforeHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            return Task.FromResult<BaseCommand<IRequest, IResponse>>(null);
        }

        /// <summary>
        /// 执行后置处理
        /// </summary>
        protected virtual Task<BaseCommand<IRequest, IResponse>> OnAfterHandleAsync(QueuedCommand cmd, BaseCommand<IRequest, IResponse> result, CancellationToken cancellationToken)
        {
            return Task.FromResult<BaseCommand<IRequest, IResponse>>(null);
        }

        #endregion

        #region 辅助方法

        
        /// <summary>
        /// 更新统计信息
        /// </summary>
        private void UpdateStatistics(bool isSuccess, long processingTimeMs)
        {
            lock (_lockObject)
            {
                if (isSuccess)
                    _statistics.SuccessfulCommands++;
                else
                    _statistics.FailedCommands++;

                _statistics.LastProcessTime = DateTime.UtcNow;
                _statistics.TotalCommandsProcessed++;

                // 更新处理时间统计
                if (_statistics.MinProcessingTimeMs == 0 || processingTimeMs < _statistics.MinProcessingTimeMs)
                    _statistics.MinProcessingTimeMs = processingTimeMs;

                if (processingTimeMs > _statistics.MaxProcessingTimeMs)
                    _statistics.MaxProcessingTimeMs = processingTimeMs;

                // 计算平均处理时间
                _statistics.AverageProcessingTimeMs =
                    (_statistics.AverageProcessingTimeMs * (_statistics.TotalCommandsProcessed - 1) + processingTimeMs)
                    / _statistics.TotalCommandsProcessed;
            }
        }

        /// <summary>
        /// 更新超时统计信息
        /// </summary>
        private void UpdateTimeoutStatistics()
        {
            lock (_lockObject)
            {
                _statistics.TimeoutCount++;
                _statistics.LastProcessTime = DateTime.UtcNow;
                _statistics.TotalCommandsProcessed++;
            }
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        protected void LogDebug(string message)
        {
            Logger.LogDebug(message);
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        protected void LogInfo(string message)
        {
            Logger.LogDebug(message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        protected void LogWarning(string message)
        {
            Logger.LogWarning(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        protected void LogError(string message, Exception ex = null)
        {
            if (ex != null)
            {
                Logger.LogError(ex, message);
            }
            else
            {
                Logger.LogError(message);
            }
        }


        /// <summary>
        /// 使用GetJsonData方法从PacketModel中解析业务数据
        /// 推荐使用此方法以确保数据解析的统一性
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <returns>解析后的数据对象，如果解析失败则返回null</returns>
        protected T ParseBusinessData<T>(ICommand command) where T : class
        {
            if (command is BaseCommand baseCmd)
            {
                return baseCmd.GetObjectData<T>();
            }
            return null;
        }



        /// <summary>
        /// 将ResponseBase转换为ResponseBase
        /// </summary>
        /// <param name="baseResponse">基础响应对象</param>
        /// <returns>ResponseBase对象</returns>
        protected ResponseBase ConvertToResponseBase(ResponseBase baseResponse)
        {
            var response = new ResponseBase
            {
                IsSuccess = baseResponse.IsSuccess,
                Message = baseResponse.Message,
                RequestId = baseResponse.RequestId,
                Metadata = baseResponse.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                ExecutionTimeMs = baseResponse.ExecutionTimeMs
            };
            return response;
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    StopAsync(CancellationToken.None).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {

                    LogError($"释放处理器时停止失败: {Name}", ex);
                }

                Status = HandlerStatus.Disposed;
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
