using System;
using System.Threading;
using System.Threading.Tasks;

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
using System.Linq;
using RUINORERP.PacketSpec.Validation;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Models.Requests;
using System.Security.Cryptography;
using MessagePack;
using System.Collections.Concurrent;
using RUINORERP.PacketSpec.Commands.Authentication;

namespace RUINORERP.PacketSpec.Commands
{
    [MessagePackObject(AllowPrivate = true)]
    public class BaseCommand<TRequest, TResponse> : BaseCommand where TRequest : class, IRequest where TResponse : class, IResponse
    {


        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseCommand() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="direction">命令方向</param>
        public BaseCommand(PacketDirection direction) : base(direction)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="direction">命令方向</param>
        /// <param name="logger">日志记录器</param>
        public BaseCommand(PacketDirection direction, ILogger<BaseCommand> logger) : base(direction, logger)
        {
        }

        /// <summary>
        /// 构造函数 - 支持依赖注入
        /// </summary>
        /// <param name="tokenManager">Token管理器</param>
        /// <param name="request">请求数据</param>
        /// <param name="direction">命令方向</param>
        /// <param name="logger">日志记录器</param>
        public BaseCommand(IRequest request = null, PacketDirection direction = PacketDirection.Unknown, ILogger<BaseCommand> logger = null)
            : base(direction, logger)
        {
            if (request != null)
                Request = request as TRequest;
        }

        /// <summary>
        /// 构造函数 - 用于快速创建命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <param name="direction">命令方向</param>
        public BaseCommand(CommandId commandId, TRequest request, PacketDirection direction = PacketDirection.ClientToServer)
            : base(direction)
        {
            CommandIdentifier = commandId;
            Request = request;
        }

        [IgnoreMember]
        private CommandDataContainer<TRequest> _requestContainer;

        [IgnoreMember]
        private CommandDataContainer<TResponse> _responseContainer;

        [Key(50)]
        public TRequest Request
        {
            get => _requestContainer?.ObjectData;
            set
            {
                if (_requestContainer == null)
                    _requestContainer = new CommandDataContainer<TRequest>();
                _requestContainer.ObjectData = value;
            }
        }

        [Key(51)]
        public TResponse Response
        {
            get => _responseContainer?.ObjectData;
            set
            {
                if (_responseContainer == null)
                    _responseContainer = new CommandDataContainer<TResponse>();
                _responseContainer.ObjectData = value;
            }
        }

        // 重写基类方法，提供智能数据访问
        protected override object GetSerializableDataCore()
        {
            // 根据命令方向返回请求或响应数据
            return Direction == PacketDirection.ClientToServer ? Request : Response;
        }

        // 新增高效二进制访问方法
        public byte[] GetRequestBinary() => _requestContainer?.BinaryData;
        public byte[] GetResponseBinary() => _responseContainer?.BinaryData;

        // 从二进制数据初始化请求,有被使用放射，用字符搜索
        public void SetRequestFromBinary(byte[] data)
        {
            if (_requestContainer == null)
                _requestContainer = new CommandDataContainer<TRequest>();
            _requestContainer.BinaryData = data;
        }

    }

    /// <summary>
    /// 命令基类 - 提供命令的通用实现
    /// </summary>
    [Serializable]
    [MessagePackObject(AllowPrivate = true)]
    public class BaseCommand : ICommand
    {
        public BaseCommand()
        {

        }


        protected virtual object GetSerializableDataCore() { return null; }
        // 新增智能访问方法
        public byte[] GetBinaryData()
        {
            var data = GetSerializableDataCore();
            return data != null ? UnifiedSerializationService.SerializeWithMessagePack(data) : Array.Empty<byte>();
        }
        public T GetObjectData<T>() where T : class
        {
            var data = GetSerializableDataCore();
            return data as T;
        }
        /// <summary>
        /// 日志记录器
        /// </summary>
        [IgnoreMember]
        protected ILogger<BaseCommand> Logger { get; set; }

        /// <summary>
        /// 命令标识符（类型安全命令系统）
        /// </summary>
        [Key(30)]
        public CommandId CommandIdentifier { get; set; }

        /// <summary>
        /// 命令方向
        /// </summary>
        [Key(31)]
        public PacketDirection Direction { get; set; }

        /// <summary>
        /// 命令优先级
        /// </summary>
        [Key(32)]
        public CommandPriority Priority { get; set; }

        /// <summary>
        /// 命令状态
        /// </summary>
        [Key(33)]
        public CommandStatus Status { get; set; }

        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        [Key(34)]
        public DateTime CreatedTimeUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        [Key(35)]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        [Key(36)]
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
        /// 更新时间戳（实现 ITimestamped 接口）
        /// </summary>
        public void UpdateTimestamp()
        {
            TimestampUtc = DateTime.UtcNow;
            LastUpdatedTime = TimestampUtc;
        }

        [Key(37)]
        public int TimeoutMs { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseCommand(PacketDirection direction = PacketDirection.Unknown, ILogger<BaseCommand> logger = null)
        {
            Direction = direction;
            Priority = CommandPriority.Normal;
            Status = CommandStatus.Created;
            CreatedTimeUtc = DateTime.UtcNow;
            TimestampUtc = DateTime.UtcNow;
            Logger = logger ?? NullLogger<BaseCommand>.Instance;
        }

        /// <summary>
        /// 验证命令
        /// </summary>
        public virtual async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            // 使用验证服务进行验证
            var validationService = ValidationService.Instance;
            var result = await validationService.ValidateAsync(this);
            return result;
        }

        /// <summary>
        /// 序列化命令数据
        /// </summary>
        public virtual byte[] Serialize()
        {
            try
            {
                // 使用UnifiedSerializationService进行序列化
                return UnifiedSerializationService.SerializeWithMessagePack(this);
            }
            catch (Exception ex)
            {
                LogError($"序列化命令失败: {ex.Message}", ex);
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

                // 使用UnifiedSerializationService进行反序列化
                var deserializedCommand = UnifiedSerializationService.DeserializeWithMessagePack<BaseCommand>(data);

                // 恢复基本属性
                if (deserializedCommand != null)
                {
                    CommandIdentifier = deserializedCommand.CommandIdentifier;
                    Direction = deserializedCommand.Direction;
                    Priority = deserializedCommand.Priority;
                    Status = deserializedCommand.Status;
                    CreatedTimeUtc = deserializedCommand.CreatedTimeUtc;
                    TimeoutMs = deserializedCommand.TimeoutMs;

                    // 恢复自定义数据
                    return DeserializeCustomData(deserializedCommand.GetSerializableData());
                }

                return false;
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

                // 使用UnifiedSerializationService进行MessagePack反序列化
                var deserializedCommand = UnifiedSerializationService.DeserializeWithMessagePack<BaseCommand>(data);

                // 恢复基本属性
                if (deserializedCommand != null)
                {
                    CommandIdentifier = deserializedCommand.CommandIdentifier;
                    Direction = deserializedCommand.Direction;
                    Priority = deserializedCommand.Priority;
                    Status = deserializedCommand.Status;
                    CreatedTimeUtc = deserializedCommand.CreatedTimeUtc;
                    TimeoutMs = deserializedCommand.TimeoutMs;

                    // 恢复自定义数据
                    return DeserializeCustomData(deserializedCommand.GetSerializableData());
                }

                return false;
            }
            catch (Exception ex)
            {
                LogError($"MessagePack反序列化命令失败: {ex.Message}", ex);
                return false;
            }
        }


        #region 虚方法 - 子类可以重写

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
        /// 包体数据（业务请求响应数据序列化后的字节）
        /// </summary>
        [Key(40)]
        public byte[] JsonRequestData { get; set; }

        /// <summary>
        /// 响应数据（业务响应数据序列化后的字节）
        /// </summary>
        [Key(41)]
        public byte[] JsonResponseData { get; set; }

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
        /// 设置JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据对象</param>
        public void SetRequestData<T>(T data)
        {
            var jsonBytes = UnifiedSerializationService.SerializeWithJson(data);
            SetRequestData(jsonBytes);
        }

        #region 核心方法

        /// <summary>
        /// 设置数据内容
        /// </summary>
        /// <param name="data">数据字节数组</param>
        /// <returns>当前实例</returns>
        public void SetRequestData(byte[] data)
        {
            JsonRequestData = data;
            LastUpdatedTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 设置数据内容
        /// </summary>
        /// <param name="data">数据字节数组</param>
        /// <returns>当前实例</returns>
        public void SetResponseData(byte[] data)
        {
            JsonResponseData = data;
            LastUpdatedTime = DateTime.UtcNow;
        }
        /// <summary>
        /// JSON序列化缓存，用于缓存高频序列化操作（如心跳包）
        /// </summary>
        private static readonly ConcurrentDictionary<string, byte[]> _jsonCache = new ConcurrentDictionary<string, byte[]>();

        /// <summary>
        /// 设置JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据对象</param>
        /// <returns>当前实例</returns>
        public void SetJsonBizData<T>(T data)
        {
            // 生成缓存键：类型名 + 数据哈希
            var type = typeof(T);
            var jsonBytes = UnifiedSerializationService.SerializeWithJson(data);

            // 计算JSON的哈希值作为缓存键的一部分
            string hash;
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(jsonBytes);
                hash = Convert.ToBase64String(hashBytes);
            }

            var cacheKey = $"{type.FullName}:{hash}";
            // 尝试从缓存获取或添加
            JsonRequestData = _jsonCache.GetOrAdd(cacheKey, jsonBytes);
            LastUpdatedTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public void ClearSensitiveData()
        {
            // Extensions?.Clear();
            // 清理包体数据
            if (JsonRequestData != null)
            {
                Array.Clear(JsonRequestData, 0, JsonRequestData.Length);
                JsonRequestData = null;
            }

            // 清理响应数据
            if (JsonResponseData != null)
            {
                Array.Clear(JsonResponseData, 0, JsonResponseData.Length);
                JsonResponseData = null;
            }
        }

        #endregion

        /// <summary>
        /// 获取JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>数据对象</returns>
        public T GetJsonData<T>()
        {
            var json = GetDataAsText();
            return string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 获取数据为文本格式
        /// </summary>
        /// <param name="encoding">编码格式，默认UTF-8</param>
        /// <returns>文本数据</returns>
        public string GetDataAsText(Encoding encoding = null)
        {
            if (JsonRequestData == null || JsonRequestData.Length == 0)
                return string.Empty;

            encoding ??= Encoding.UTF8;
            return encoding.GetString(JsonRequestData);
        }

        /// <summary>
        /// 设置响应JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">响应数据对象</param>
        public void SetJsonResponseData<T>(T data)
        {
            var jsonBytes = UnifiedSerializationService.SerializeWithJson(data);
            JsonResponseData = jsonBytes;
            LastUpdatedTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 获取响应JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>响应数据对象</returns>
        public T GetJsonResponseData<T>()
        {
            if (JsonResponseData == null || JsonResponseData.Length == 0)
                return default(T);

            var json = Encoding.UTF8.GetString(JsonResponseData);
            return string.IsNullOrEmpty(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 获取响应数据为文本格式
        /// </summary>
        /// <param name="encoding">编码格式，默认UTF-8</param>
        /// <returns>响应文本数据</returns>
        public string GetResponseDataAsText(Encoding encoding = null)
        {
            if (JsonResponseData == null || JsonResponseData.Length == 0)
                return string.Empty;

            encoding ??= Encoding.UTF8;
            return encoding.GetString(JsonResponseData);
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
        /// 检查是否超时（基于TimeoutMs和创建时间）
        /// </summary>
        public bool IsTimeout()
        {
            return TimeoutMs > 0 && (DateTime.UtcNow - CreatedTimeUtc).TotalMilliseconds > TimeoutMs;
        }

        #endregion
    }
}
