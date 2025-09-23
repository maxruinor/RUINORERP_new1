using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Utilities;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using System.Text;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令处理器基类 - 提供命令处理器的通用实现
    /// </summary>
    public abstract class BaseCommandHandler : ITraceable, IValidatable, ICommandHandler
    {
        private readonly object _lockObject = new object();
        private bool _disposed = false;
        private readonly HandlerStatistics _statistics;

        /// <summary>
        /// 处理器唯一标识
        /// </summary>
        public string HandlerId { get; private set; }

        #region ITraceable 接口实现
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 模型版本
        /// </summary>
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 更新时间戳
        /// </summary>
        public void UpdateTimestamp()
        {
            Timestamp = DateTime.UtcNow;
            LastUpdatedTime = Timestamp;
        }
        #endregion

        /// <summary>
        /// 处理器名称
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public virtual int Priority => 0;

        /// <summary>
        /// 是否已初始化
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public abstract IReadOnlyList<uint> SupportedCommands { get; }

        /// <summary>
        /// 处理器状态
        /// </summary>
        public HandlerStatus Status { get; private set; } = HandlerStatus.Uninitialized;

        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger Logger { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseCommandHandler()
        {
            HandlerId = GenerateHandlerId();
            _statistics = new HandlerStatistics();
            
            // 初始化ITraceable属性
            CreatedTime = DateTime.UtcNow;
            Timestamp = DateTime.UtcNow;
            Version = "2.0";
            
            // 不再初始化默认的日志记录器，而是延迟初始化
        }

        #region IValidatable 接口实现
        /// <summary>
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return CreatedTime <= DateTime.UtcNow &&
                   CreatedTime >= DateTime.UtcNow.AddYears(-1); // 创建时间在1年内
        }
        #endregion



 

        /// <summary>
        /// 异步处理命令
        /// </summary>
        public async Task<CommandResult> HandleAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            if (_disposed)
            {
                return CommandResult.Failure("处理器已释放", ErrorCodes.HandlerDisposed);
            }

            if (Status != HandlerStatus.Running)
            {
                return CommandResult.Failure("处理器未运行", ErrorCodes.HandlerNotInitialized);
            }

            var startTime = DateTime.UtcNow;
            
            lock (_lockObject)
            {
                _statistics.CurrentProcessingCount++;
                _statistics.TotalCommandsProcessed++;
            }

            try
            {
               
                LogDebug($"开始处理命令: {command.CommandIdentifier} [ID: {command.CommandId}]");

                // 验证命令
                if (!CanHandle(command))
                {
                    return CommandResult.Failure("处理器无法处理该命令", ErrorCodes.UnsupportedCommand);
                }

                // 执行前置处理
                var preResult = await OnBeforeHandleAsync(command, cancellationToken);
                if (preResult != null && !preResult.IsSuccess)
                {
                    return preResult;
                }

                // 执行核心处理逻辑
                var result = await OnHandleAsync(command, cancellationToken);

                // 执行后置处理
                var postResult = await OnAfterHandleAsync(command, result, cancellationToken);
                if (postResult != null)
                {
                    result = postResult;
                }

                // 更新统计信息
                var processingTime = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
                UpdateStatistics(result.IsSuccess, processingTime);

               
                LogDebug($"命令处理完成: {command.CommandIdentifier} [ID: {command.CommandId}] - {processingTime}ms");
                return result;
            }
            catch (OperationCanceledException)
            {
               
                LogWarning($"命令处理被取消: {command.CommandIdentifier} [ID: {command.CommandId}]");
                UpdateStatistics(false, (long)(DateTime.UtcNow - startTime).TotalMilliseconds);
                return CommandResult.Failure("命令处理被取消", ErrorCodes.CommandCancelled);
            }
            catch (Exception ex)
            {
                var processingTime = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
               
                LogError($"命令处理异常: {command.CommandIdentifier} [ID: {command.CommandId}]", ex);
                UpdateStatistics(false, processingTime);
                return CommandResult.Failure($"处理异常: {ex.Message}", ErrorCodes.ProcessError, ex);
            }
            finally
            {
                lock (_lockObject)
                {
                    _statistics.CurrentProcessingCount--;
                }
            }
        }

        /// <summary>
        /// 判断是否可以处理该命令
        /// </summary>
        public virtual bool CanHandle(ICommand command)
        {
            if (command == null || _disposed || Status != HandlerStatus.Running)
                return false;

            return SupportedCommands.Contains(command.CommandIdentifier.FullCode);
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
                    _statistics.StartTime = DateTime.UtcNow;
                   
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
                    StartTime = _statistics.StartTime,
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
                _statistics.StartTime = DateTime.UtcNow;
            }
        }

        #region 抽象方法 - 子类必须实现

        /// <summary>
        /// 执行核心处理逻辑
        /// </summary>
        protected abstract Task<CommandResult> OnHandleAsync(ICommand command, CancellationToken cancellationToken);

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
        protected virtual Task<CommandResult> OnBeforeHandleAsync(ICommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult<CommandResult>(null);
        }

        /// <summary>
        /// 执行后置处理
        /// </summary>
        protected virtual Task<CommandResult> OnAfterHandleAsync(ICommand command, CommandResult result, CancellationToken cancellationToken)
        {
            return Task.FromResult<CommandResult>(null);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 生成处理器ID
        /// </summary>
        private string GenerateHandlerId()
        {
            return $"{GetType().Name}_{DateTime.UtcNow.Ticks}_{Guid.NewGuid():N}";
        }

        /// <summary>
        /// 创建成功响应结果
        /// </summary>
        /// <typeparam name="T">响应数据类型</typeparam>
        /// <param name="command">命令ID</param>
        /// <param name="data">响应数据</param>
        /// <param name="msg">响应消息</param>
        /// <returns>命令处理结果</returns>
        protected CommandResult Success<T>(uint command, T data, string msg = null)
        {
            var responseData = CreateMessageResponse(command, data);
            return CommandResult.SuccessWithResponse(responseData, data, msg ?? "操作成功");
        }

        /// <summary>
        /// 创建失败响应结果
        /// </summary>
        /// <param name="msg">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="ex">异常信息</param>
        /// <returns>命令处理结果</returns>
        protected CommandResult Fail(string msg, string code = null, Exception ex = null)
        {
            return CommandResult.Failure(msg, code ?? "GENERAL_ERROR", ex);
        }

        /// <summary>
        /// 创建消息响应数据包
        /// </summary>
        /// <typeparam name="T">响应数据类型</typeparam>
        /// <param name="command">命令ID</param>
        /// <param name="data">响应数据</param>
        /// <returns>原始数据包</returns>
        protected virtual OriginalData CreateMessageResponse<T>(uint command, T data)
        {
            try
            {
                // 将命令ID转换为字节数组
                byte[] commandBytes = BitConverter.GetBytes(command);
                
                // 序列化数据部分
                byte[] dataBytes = Array.Empty<byte>();
                if (data != null)
                {
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    dataBytes = Encoding.UTF8.GetBytes(json);
                }
                
                // 构造OriginalData: Cmd使用命令ID的低8位，One使用命令ID的高8位，Two使用序列化后的数据
                byte cmd = commandBytes[0]; // 命令类别
                byte[] one = commandBytes.Length > 1 ? new byte[] { commandBytes[1] } : Array.Empty<byte>(); // 操作码
                
                return new OriginalData(cmd, one, dataBytes);
            }
            catch (Exception ex)
            {
                LogError("创建消息响应失败: " + ex.Message, ex);
                // 返回空的响应数据包
                return OriginalData.Empty;
            }
        }

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
           
            Logger.LogInformation(message);
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
