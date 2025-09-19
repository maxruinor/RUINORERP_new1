using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Protocol;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Handlers;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Enums.Exception;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令基类 - 提供命令的通用实现
    /// </summary>
    public abstract class BaseCommand : BaseModel, ICommand
    {
        /// <summary>
        /// 命令唯一标识
        /// </summary>
        public string CommandId { get; private set; }

        /// <summary>
        /// 命令标识符（类型安全命令系统）
        /// </summary>
        public abstract CommandId CommandIdentifier { get; }

        /// <summary>
        /// 命令方向
        /// </summary>
        public CommandDirection Direction { get; set; }

        /// <summary>
        /// 命令优先级
        /// </summary>
        public CommandPriority Priority { get; set; }

        /// <summary>
        /// 命令状态
        /// </summary>
        public CommandStatus Status { get; set; }

        /// <summary>
        /// 会话信息中的数据包
        /// </summary>
        public PacketModel packetModel { get; set; }

        /// <summary>
        /// 原始数据包
        /// </summary>
        public OriginalData OriginalData { get; set; }

        /// <summary>
        /// 命令创建时间
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger Logger { get; set; }

        public int TimeoutMs { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseCommand(CommandDirection direction = CommandDirection.Receive)
        {
            CommandId = GenerateCommandId();
            Direction = direction;
            Priority = CommandPriority.Normal;
            Status = CommandStatus.Created;
            CreatedAt = DateTime.Now;

            // 不再初始化默认的日志记录器，而是延迟初始化
        }

        /// <summary>
        /// 设置日志记录器
        /// </summary>
        public void SetLogger(ILogger logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// 确保日志记录器已初始化
        /// </summary>
        private void EnsureLoggerInitialized()
        {
            if (Logger == null)
            {
                // 使用控制台日志器作为后备方案
                Logger = new Utilities.ConsoleLogger(GetType().Name);
            }
        }

        /// <summary>
        /// 执行命令 - 模板方法模式
        /// </summary>
        public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                EnsureLoggerInitialized();
                LogInfo($"开始执行命令: {GetType().Name} [ID: {CommandId}]");

                // 验证命令
                var validationResult = Validate();
                if (!validationResult.IsValid)
                {
                    LogWarning($"命令验证失败: {validationResult.ErrorMessage}");
                    return CommandResult.Failure(
                        validationResult.ErrorMessage,
                        validationResult.ErrorCode ?? ErrorCodes.CommandValidationFailed);
                }

                // 执行前处理
                await OnBeforeExecuteAsync(cancellationToken);

                // 执行核心逻辑
                var result = await OnExecuteAsync(cancellationToken);

                // 执行后处理
                await OnAfterExecuteAsync(cancellationToken);

                LogInfo($"命令执行完成: {GetType().Name} [ID: {CommandId}]");
                return result;
            }
            catch (OperationCanceledException)
            {
                LogWarning($"命令执行被取消: {GetType().Name} [ID: {CommandId}]");
                return CommandResult.Failure("命令执行被取消", ErrorCodes.ProcessCancelled);
            }
            catch (Exception ex)
            {
                LogError($"执行命令 {GetType().Name} [ID: {CommandId}] 异常: {ex.Message}", ex);
                return CommandResult.Failure($"执行异常: {ex.Message}", ErrorCodes.ProcessError, ex);
            }
        }

        /// <summary>
        /// 验证命令
        /// </summary>
        public virtual CommandValidationResult Validate()
        {
            // 基本验证
            if (string.IsNullOrEmpty(CommandId))
            {
                return CommandValidationResult.Failure("命令ID不能为空", ErrorCodes.InvalidCommandId);
            }

            if (CommandIdentifier.FullCode == 0)
            {
                return CommandValidationResult.Failure("命令标识符不能为0", ErrorCodes.InvalidCommandIdentifier);
            }

            // 超时验证
            if (TimeoutMs <= 0)
            {
                return CommandValidationResult.Failure("超时时间必须大于0", ErrorCodes.InvalidTimeout);
            }

            // 会话验证（如果需要）
            if (RequiresSession() && packetModel == null)
            {
                return CommandValidationResult.Failure("该命令需要有效的会话信息", ErrorCodes.SessionRequired);
            }

            // 数据验证（如果需要）
            if (RequiresData() && OriginalData.IsValid)
            {
                return CommandValidationResult.Failure("该命令需要有效的数据包", ErrorCodes.DataRequired);
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 序列化命令数据
        /// </summary>
        public virtual byte[] Serialize()
        {
            try
            {
                var commandData = new
                {
                    CommandId,
                    CommandIdentifier,
                    Direction,
                    Priority,
                    Status,
                    CreatedAt,
                    TimeoutMs,
                    packetModel,
                    Data = GetSerializableData()
                };

                var json = JsonConvert.SerializeObject(commandData);
                return System.Text.Encoding.UTF8.GetBytes(json);
            }
            catch (Exception ex)
            {
                LogError($"序列化命令失败: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 使用MessagePack序列化命令数据
        /// </summary>
        public virtual byte[] SerializeWithMessagePack()
        {
            try
            {
                var commandData = new
                {
                    CommandId,
                    CommandIdentifier,
                    Direction,
                    Priority,
                    Status,
                    CreatedAt,
                    TimeoutMs,
                    packetModel,
                    Data = GetSerializableData()
                };

                return MessagePackService.Serialize(commandData);
            }
            catch (Exception ex)
            {
                LogError($"MessagePack序列化命令失败: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 反序列化命令数据
        /// </summary>
        public virtual bool Deserialize(byte[] data)
        {
            try
            {
                if (data == null || data.Length == 0)
                    return false;

                var json = System.Text.Encoding.UTF8.GetString(data);
                var commandData = JsonConvert.DeserializeObject<dynamic>(json);

                // 恢复基本属性
                if (commandData.Direction != null)
                    Direction = (CommandDirection)commandData.Direction;

                if (commandData.Priority != null)
                    Priority = (CommandPriority)commandData.Priority;

                if (commandData.TimeoutMs != null)
                    TimeoutMs = commandData.TimeoutMs;

                // 恢复自定义数据
                return DeserializeCustomData(commandData.Data);
            }
            catch (Exception ex)
            {
                LogError($"反序列化命令失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 使用MessagePack反序列化命令数据
        /// </summary>
        public virtual bool DeserializeWithMessagePack(byte[] data)
        {
            try
            {
                if (data == null || data.Length == 0)
                    return false;

                var commandData = MessagePackService.Deserialize<dynamic>(data);

                // 恢复基本属性
                if (commandData.Direction != null)
                    Direction = (CommandDirection)commandData.Direction;

                if (commandData.Priority != null)
                    Priority = (CommandPriority)commandData.Priority;

                if (commandData.TimeoutMs != null)
                    TimeoutMs = commandData.TimeoutMs;

                // 恢复自定义数据
                return DeserializeCustomData(commandData.Data);
            }
            catch (Exception ex)
            {
                LogError($"MessagePack反序列化命令失败: {ex.Message}", ex);
                return false;
            }
        }

        #region 抽象方法 - 子类必须实现

        /// <summary>
        /// 执行核心逻辑
        /// </summary>
        protected abstract Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken);

        #endregion

        #region 虚方法 - 子类可以重写

        /// <summary>
        /// 执行前处理
        /// </summary>
        protected virtual Task OnBeforeExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 执行后处理
        /// </summary>
        protected virtual Task OnAfterExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 是否需要会话信息
        /// </summary>
        protected virtual bool RequiresSession()
        {
            return false;
        }

        /// <summary>
        /// 是否需要数据包
        /// </summary>
        protected virtual bool RequiresData()
        {
            return false;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        protected virtual object GetSerializableData()
        {
            return null;
        }

        /// <summary>
        /// 反序列化自定义数据
        /// </summary>
        protected virtual bool DeserializeCustomData(dynamic data)
        {
            return true;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 生成命令ID
        /// </summary>
        private string GenerateCommandId()
        {
            return $"{GetType().Name}_{Guid.NewGuid():N}";
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        protected void LogDebug(string message)
        {
            EnsureLoggerInitialized();
            Logger.LogDebug(message);
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        protected void LogInfo(string message)
        {
            EnsureLoggerInitialized();
            Logger.LogInformation(message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        protected void LogWarning(string message)
        {
            EnsureLoggerInitialized();
            Logger.LogWarning(message);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        protected void LogError(string message, Exception ex = null)
        {
            EnsureLoggerInitialized();
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
        /// 创建响应数据包
        /// </summary>
        protected OriginalData CreateResponseData(uint responseCommand, byte[] data1 = null, byte[] data2 = null)
        {
            return new OriginalData((byte)Math.Min(responseCommand, 255), data1, data2);
        }

        /// <summary>
        /// 检查是否超时
        /// </summary>
        protected bool IsTimeout()
        {
            return (DateTime.UtcNow - CreatedAt).TotalMilliseconds > TimeoutMs;
        }

        #endregion
    }
}
