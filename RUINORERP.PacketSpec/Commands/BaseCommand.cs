using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Protocol;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Handlers;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Enums.Core;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令基类 - 提供命令的通用实现
    /// </summary>
    public abstract class BaseCommand : ITraceable, IValidatable, ICommand
    {    /// <summary>
         /// 日志记录器
         /// </summary>
        protected ILogger<BaseCommand> Logger { get; set; }
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
        public PacketDirection Direction { get; set; }

        /// <summary>
        /// 命令优先级
        /// </summary>
        public CommandPriority Priority { get; set; }

        /// <summary>
        /// 命令状态
        /// </summary>
        public CommandStatus Status { get; set; }

        /// <summary>
        /// 数据包模型 - 包含完整的数据包信息和业务数据
        /// </summary>
        public PacketModel Packet { get; set; }


        /// <summary>
        /// 命令创建时间（UTC时间）
        /// </summary>
        public DateTime CreatedAtUtc { get; private set; }

        #region ITraceable 接口实现
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
        /// 更新时间戳
        /// </summary>
        public void UpdateTimestamp()
        {
            TimestampUtc = DateTime.UtcNow;
            LastUpdatedTime = TimestampUtc;
        }
        #endregion



        public int TimeoutMs { get; set; }
        public string SessionId { get; set; }
        public string ClientId { get; set; }
        public string RequestId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseCommand(PacketDirection direction = PacketDirection.Unknown, ILogger<BaseCommand> logger = null)
        {
            CommandId = GenerateCommandId();
            Direction = direction;
            Priority = CommandPriority.Normal;
            Status = CommandStatus.Created;
            CreatedAtUtc = DateTime.UtcNow;

            // 初始化ITraceable属性
            CreatedTimeUtc = DateTime.UtcNow;
            TimestampUtc = DateTime.UtcNow;
            Logger = logger ?? NullLogger<BaseCommand>.Instance;
        }

        #region IValidatable 接口实现
        /// <summary>
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return CreatedTimeUtc <= DateTime.UtcNow &&
                   CreatedTimeUtc >= DateTime.UtcNow.AddYears(-1); // 创建时间在1年内
        }
        #endregion


        /// <summary>
        /// 执行命令 - 模板方法模式
        /// </summary>
        public async Task<ResponseBase> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                LogInfo($"开始执行命令: {GetType().Name} [ID: {CommandId}]");

                // 验证命令
                var validationResult = Validate();
                if (!validationResult.IsValid)
                {
                    LogWarning($"命令验证失败: {validationResult.ErrorMessage}");
                    ResponseBase errorResponse = ResponseBase.CreateError(validationResult.ErrorMessage, 400).WithMetadata("ErrorCode", validationResult.ErrorCode ?? "VALIDATION_FAILED");
                    return errorResponse;
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
                ResponseBase errorResponse = ResponseBase.CreateError("命令执行被取消", 503).WithMetadata("ErrorCode", "PROCESS_CANCELLED");
                return errorResponse;
            }
            catch (Exception ex)
            {
                LogError($"执行命令 {GetType().Name} [ID: {CommandId}] 异常: {ex.Message}", ex);
                ResponseBase errorResponse = ResponseBase.CreateError($"执行异常: {ex.Message}", 500)
                        .WithMetadata("ErrorCode", "PROCESS_ERROR")
                        .WithMetadata("Exception", ex.Message)
                        .WithMetadata("StackTrace", ex.StackTrace);
                return errorResponse;
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
            if (RequiresSession() && Packet == null)
            {
                return CommandValidationResult.Failure("该命令需要有效的会话信息", ErrorCodes.SessionRequired);
            }

            // 数据验证（如果需要）
            if (RequiresData() && !Packet.IsValid())
            {
                return CommandValidationResult.Failure("该命令需要有效的数据包", ErrorCodes.DataRequired);
            }
            // 添加 SessionId 验证（如果需要）
            if (RequiresSession() && string.IsNullOrEmpty(SessionId))
            {
                return CommandValidationResult.Failure("该命令需要有效的会话ID", ErrorCodes.SessionRequired);
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
                    CreatedAtUtc,
                    TimeoutMs,
                    SessionId,
                    ClientId,
                    RequestId,
                    Packet,
                    Data = GetSerializableData()
                };

                var json = JsonConvert.SerializeObject(commandData);
                return Encoding.UTF8.GetBytes(json);
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
                    CreatedAtUtc,
                    TimeoutMs,
                    SessionId,
                    ClientId,
                    RequestId,
                    Packet,
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

                var json = Encoding.UTF8.GetString(data);
                var commandData = JsonConvert.DeserializeObject<dynamic>(json);

                // 恢复基本属性
                if (commandData.Direction != null)
                    Direction = (PacketDirection)commandData.Direction;

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
                    Direction = (PacketDirection)commandData.Direction;

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
        protected abstract Task<ResponseBase> OnExecuteAsync(CancellationToken cancellationToken);

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
        public virtual object GetSerializableData()
        {
            return null;
        }

        /// <summary>
        /// 获取强类型的数据载荷
        /// </summary>
        /// <typeparam name="T">数据载荷类型</typeparam>
        /// <returns>指定类型的载荷数据</returns>
        public virtual T GetPayload<T>()
        {
            return (T)GetSerializableData();
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

        /// <summary>
        /// 创建响应数据包
        /// </summary>
        /// <param name="responseCommand">完整的响应命令ID</param>
        /// <param name="data1">第一部分数据</param>
        /// <param name="data2">第二部分数据</param>
        /// <returns>原始数据包</returns>
        protected OriginalData CreateResponseData(uint responseCommand, byte[] data1 = null, byte[] data2 = null)
        {
            // 将uint类型的命令ID转换为字节数组
            byte[] commandBytes = BitConverter.GetBytes(responseCommand);

            // 构造OriginalData: Cmd使用命令ID的低8位(Category)，One使用命令ID的次低8位(OperationCode)
            byte cmd = commandBytes[0]; // 命令类别
            byte[] one = commandBytes.Length > 1 ? new byte[] { commandBytes[1] } : Array.Empty<byte>(); // 操作码

            return new OriginalData(cmd, one, data2);
        }

        /// <summary>
        /// 检查是否超时
        /// </summary>
        protected bool IsTimeout()
        {
            return (DateTime.UtcNow - CreatedAtUtc).TotalMilliseconds > TimeoutMs;
        }

        #endregion
    }
}
