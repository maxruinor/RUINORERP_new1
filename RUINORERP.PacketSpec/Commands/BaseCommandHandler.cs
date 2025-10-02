using System;
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

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令处理器基类 - 提供命令处理器的通用实现
    /// </summary>
    public abstract class BaseCommandHandler :  ICommandHandler
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

        #region 上下文的支持

        protected CommandExecutionContext GetCommandContext(ICommand command)
        {
            if (command is BaseCommand baseCommand)
            {
                return baseCommand.ExecutionContext;
            }
            return new CommandExecutionContext();
        }

        protected string GetSessionId(ICommand command)
            => GetCommandContext(command)?.SessionId;

        protected string GetClientId(ICommand command)
            => GetCommandContext(command)?.ClientId;

        protected T GetExtension<T>(ICommand command, string key)
        {
            var context = GetCommandContext(command);
            if (context?.Extensions != null && context.Extensions.ContainsKey(key))
            {
                try
                {
                    return (T)context.Extensions[key];
                }
                catch
                {
                    return default(T);
                }
            }
            return default(T);
        }

        #endregion

        #region ICoreEntity 接口实现
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        public DateTime CreatedTimeUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 验证模型有效性（实现 ICoreEntity 接口）
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return CreatedTimeUtc <= DateTime.UtcNow &&
                   CreatedTimeUtc >= DateTime.UtcNow.AddYears(-1); // 创建时间在1年内
        }

        /// <summary>
        /// 更新时间戳（实现 ICoreEntity 接口）
        /// </summary>
        public void UpdateTimestamp()
        {
            TimestampUtc = DateTime.UtcNow;
            LastUpdatedTime = TimestampUtc;
        }
        #endregion

        /// <summary>
        /// 处理器名称
        /// </summary>
        public virtual string Name => this.GetType().Name;

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
            // 初始化ICoreEntity属性
            CreatedTimeUtc = DateTime.UtcNow;
            TimestampUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// 异步处理命令
        /// </summary>
        public async Task<ResponseBase> HandleAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var startTime = DateTime.UtcNow;
            bool success = true; // 默认认为成功
            
            try
            {
                // 记录开始处理时间
                var logStartTime = DateTime.UtcNow;
                
                // 安全获取命令ID（通过反射）
                string commandId = null;
                try 
                {
                    var commandWithId = command as dynamic;
                    commandId = commandWithId.CommandId ?? "N/A";
                }
                catch
                {
                    commandId = "N/A";
                }
                
                Logger.LogInformation($"开始处理命令: {command.GetType().Name}, CommandId: {commandId}");

                // 验证命令
                var validationResult = await command.ValidateAsync(cancellationToken);
                if (!validationResult.IsValid)
                {
                    Logger.LogWarning($"命令验证失败: {validationResult.Errors[0].ErrorMessage}");
                    return ResponseFactory.Fail(UnifiedErrorCodes.Command_ValidationFailed, $"命令验证失败: {validationResult.Errors[0].ErrorMessage}");
                }

                // 检查命令是否过期（如果命令有实现ExpirationTimeUtc属性）
                try 
                {
                    var commandWithExpiration = command as dynamic;
                    if (commandWithExpiration.ExpirationTimeUtc != null && 
                        commandWithExpiration.ExpirationTimeUtc < DateTime.UtcNow)
                    {
                        Logger.LogWarning($"命令已过期: {commandWithExpiration.ExpirationTimeUtc}");
                        return ResponseFactory.Fail(UnifiedErrorCodes.Command_Timeout, "命令已过期");
                    }
                }
                catch
                {
                    // 如果命令没有ExpirationTimeUtc属性，则跳过检查
                }

                // 命令前置处理
                var beforeResult = await OnBeforeHandleAsync(command, cancellationToken);
                if (beforeResult != null)
                    return beforeResult;

                // 执行命令处理
                var result = await OnHandleAsync(command, cancellationToken);

                // 命令后置处理
                var afterResult = await OnAfterHandleAsync(command, result, cancellationToken);
                if (afterResult != null)
                    return afterResult;

                // 记录处理完成时间
                var endTime = DateTime.UtcNow;
                Logger.LogInformation($"命令处理完成: {command.GetType().Name}, CommandId: {command.CommandIdentifier}, 结果: {result.IsSuccess}, 执行时间: {(endTime - logStartTime).TotalMilliseconds} ms");

                return result;
            }
            catch (OperationCanceledException)
            {
                success = false;
                return ResponseFactory.Fail(UnifiedErrorCodes.Command_ProcessCancelled);
            }
            catch (Exception ex)
            {
                success = false;
                return ResponseFactory.Except(ex, UnifiedErrorCodes.Command_ExecuteFailed);
            }
            finally
            {
                var elapsed = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
                
                // 记录结构化日志
                _logHandled(Logger, command.ToString(), elapsed, success, null);
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
        [MustOverride]
        protected abstract Task<ResponseBase> OnHandleAsync(ICommand command, CancellationToken cancellationToken);

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
        protected virtual Task<ResponseBase> OnBeforeHandleAsync(ICommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult<ResponseBase>(null);
        }

        /// <summary>
        /// 执行后置处理
        /// </summary>
        protected virtual Task<ResponseBase> OnAfterHandleAsync(ICommand command, ResponseBase result, CancellationToken cancellationToken)
        {
            return Task.FromResult<ResponseBase>(null);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 创建成功响应结果
        /// </summary>
        /// <typeparam name="T">响应数据类型</typeparam>
        /// <param name="command">命令ID</param>
        /// <param name="data">响应数据</param>
        /// <param name="msg">响应消息</param>
        /// <returns>API响应结果</returns>
        protected ResponseBase Success<T>(uint command, T data, string msg = null)
        {
            var responseData = CreateMessageResponse(command, data);
            
            // 创建非泛型ResponseBase实例
            var result = new ResponseBase
            {
                IsSuccess = true,
                Message = msg ?? "操作成功",
                Code = 200,
                TimestampUtc = DateTime.UtcNow
            };
            
            // 添加元数据 - 修复WithMetadata返回ResponseBase的问题
            result = (ResponseBase)result.WithMetadata("ResponseData", responseData);
            
            // 确保数据可以正确序列化和存储
            if (data != null)
            {
                try
                {
                    // 序列化数据为JSON字符串以避免类型转换问题
                    string serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    result = (ResponseBase)result.WithMetadata("Data", serializedData);
                }
                catch (Exception ex)
                {
                    LogWarning($"无法序列化响应数据: {ex.Message}");
                    // 如果序列化失败，仍然添加原始数据，但记录警告
                    result = (ResponseBase)result.WithMetadata("Data", data);
                }
            }
            
            return result;
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

        ///// <summary>
        ///// 从原始数据解析对象 - 使用系统默认的JSON序列化器
        ///// </summary>
        //protected T ParseData<T>(OriginalData originalData) where T : class
        //{
        //    if (originalData.One == null || originalData.One.Length == 0)
        //        return null;

        //    try
        //    {
        //        var json = Encoding.UTF8.GetString(originalData.One);
        //        return JsonSerializer.Deserialize<T>(json);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError($"解析数据失败: {ex.Message}", ex);
        //        return null;
        //    }
        //}

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
        /// 创建响应数据
        /// </summary>
        //protected OriginalData CreateResponseData(uint command, object data)
        //{
        //    try
        //    {
        //        var json = System.Text.Json.JsonSerializer.Serialize(data);
        //        var dataBytes = System.Text.Encoding.UTF8.GetBytes(json);
                
        //        // 将完整的CommandId正确分解为Category和OperationCode
        //        byte category = (byte)(command & 0xFF); // 取低8位作为Category
        //        byte operationCode = (byte)((command >> 8) & 0xFF); // 取次低8位作为OperationCode
                
        //        return new OriginalData(category, new byte[] { operationCode }, dataBytes);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError($"创建响应数据失败: {ex.Message}", ex);
                
        //        // 将完整的CommandId正确分解为Category和OperationCode
        //        byte category = (byte)(command & 0xFF); // 取低8位作为Category
        //        byte operationCode = (byte)((command >> 8) & 0xFF); // 取次低8位作为OperationCode
                
        //        return new OriginalData(category, new byte[] { operationCode }, null);
        //    }
        //}

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
                Code = baseResponse.Code,
                TimestampUtc = baseResponse.TimestampUtc,
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
