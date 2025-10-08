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
    public class BaseCommand<TRequest, TResponse> : BaseCommand where TRequest : class, IRequest where TResponse : class, IResponse
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseCommand() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="direction">命令方向</param>
        protected BaseCommand(PacketDirection direction) : base(direction)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="direction">命令方向</param>
        /// <param name="logger">日志记录器</param>
        protected BaseCommand(PacketDirection direction, ILogger<BaseCommand> logger) : base(direction, logger)
        {
        }

        /// <summary>
        /// 构造函数 - 支持依赖注入
        /// </summary>
        /// <param name="tokenManager">Token管理器</param>
        /// <param name="request">请求数据</param>
        /// <param name="direction">命令方向</param>
        /// <param name="logger">日志记录器</param>
        protected BaseCommand(TokenManager tokenManager, IRequest request = null, PacketDirection direction = PacketDirection.Unknown, ILogger<BaseCommand> logger = null)
            : base(tokenManager, direction, logger)
        {
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

        private CommandDataContainer<TRequest> _requestContainer;
        private CommandDataContainer<TResponse> _responseContainer;

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
    public class BaseCommand : ICommand
    {
        public CommandExecutionContext ExecutionContext { get; set; }

        public RequestBase Request { get; set; }
        protected virtual object GetSerializableDataCore() { return null; }
        // 新增智能访问方法
        public byte[] GetBinaryData()
        {
            var data = GetSerializableDataCore();
            return data != null ? MessagePackSerializer.Serialize(data) : Array.Empty<byte>();
        }
        public T GetObjectData<T>() where T : class
        {
            var data = GetSerializableDataCore();
            return data as T;
        }
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected ILogger<BaseCommand> Logger { get; set; }

        /// <summary>
        /// Token管理器 - 通过依赖注入获取
        /// </summary>
        protected TokenManager TokenManager { get; set; }

        /// <summary>
        /// 认证令牌
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// 令牌类型
        /// </summary>
        public string TokenType { get; set; } = "Bearer";


        /// <summary>
        /// 命令标识符（类型安全命令系统）
        /// </summary>
        public CommandId CommandIdentifier { get; set; }

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


        #region  
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
        /// 构造函数 - 支持依赖注入
        /// </summary>
        /// <param name="tokenManager">Token管理器</param>
        /// <param name="direction">命令方向</param>
        /// <param name="logger">日志记录器</param>
        protected BaseCommand(TokenManager tokenManager, PacketDirection direction = PacketDirection.Unknown, ILogger<BaseCommand> logger = null)
            : this(direction, logger)
        {
            TokenManager = tokenManager;
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
                var commandData = new
                {
                    CommandIdentifier,
                    Direction,
                    Priority,
                    Status,
                    CreatedTimeUtc,
                    TimeoutMs,
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
                    Request.RequestId,
                    CommandIdentifier,
                    Direction,
                    Priority,
                    Status,
                    CreatedTimeUtc,
                    TimeoutMs,
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

        private MemoryTokenStorage memoryTokenStorage;

        #region 虚方法 - 子类可以重写

        /// <summary>
        /// 自动附加认证Token - 优化版
        /// 增强功能：确保Token的完整性、类型设置、ExecutionContext绑定和异常处理
        /// </summary>
        protected virtual async void AutoAttachToken()
        {
            try
            {
                // 检查TokenManager是否可用
                if (TokenManager == null)
                {
                    Logger?.LogDebug("TokenManager未初始化，跳过自动附加");
                    return;
                }

                // 简化版：使用依赖注入的TokenManager
                var tokenInfo = await TokenManager.TokenStorage.GetTokenAsync();
                if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.AccessToken))
                {
                    AuthToken = tokenInfo.AccessToken;
                    TokenType = "Bearer";

                    // 自动设置到ExecutionContext，确保服务器端也能获取
                    if (ExecutionContext == null)
                        ExecutionContext = new CommandExecutionContext();

                    ExecutionContext.Token = tokenInfo.AccessToken;
                    ExecutionContext.Extensions["RefreshToken"] = tokenInfo.RefreshToken;

                    Logger?.LogDebug("自动附加Token成功: {TokenLength} 字符", tokenInfo.AccessToken?.Length ?? 0);
                }
                else
                {
                    Logger?.LogDebug("未找到有效Token，跳过自动附加");
                }
            }
            catch (Exception ex)
            {
                Logger?.LogWarning(ex, "自动附加Token失败");
            }
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
        /// 包体数据（业务请求响应数据序列化后的字节）
        /// </summary>
        public byte[] BizData { get; set; }

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
        public void SetJsonData<T>(T data)
        {
            var json = JsonConvert.SerializeObject(data);
            SetData(Encoding.UTF8.GetBytes(json));
        }

        #region 核心方法

        /// <summary>
        /// 获取包大小
        /// </summary>
        public int GetPackageSize()
        {
            return BizData?.Length ?? 0;
        }

        /// <summary>
        /// 设置数据内容
        /// </summary>
        /// <param name="data">数据字节数组</param>
        /// <returns>当前实例</returns>
        public void SetData(byte[] data)
        {
            BizData = data;
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
            var json = JsonConvert.SerializeObject(data);
            var jsonBytes = Encoding.UTF8.GetBytes(json);

            // 计算JSON的哈希值作为缓存键的一部分
            string hash;
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(jsonBytes);
                hash = Convert.ToBase64String(hashBytes);
            }

            var cacheKey = $"{type.FullName}:{hash}";
            // 尝试从缓存获取或添加
            BizData = _jsonCache.GetOrAdd(cacheKey, jsonBytes);
            LastUpdatedTime = DateTime.UtcNow;
        }



        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public void ClearSensitiveData()
        {
            // Extensions?.Clear();
            // 清理包体数据
            if (BizData != null)
            {
                Array.Clear(BizData, 0, BizData.Length);
                BizData = null;
            }
        }

        /// <summary>
        /// 创建数据包克隆
        /// </summary>
        /// <returns>克隆实例</returns>
        //public PacketModel Clone()
        //{
        //    return new PacketModel
        //    {
        //        PacketId = Guid.NewGuid().ToString(),
        //        Data = BizData?.Clone() as byte[],
        //        Size = Size,
        //        Checksum = Checksum,
        //        IsEncrypted = IsEncrypted,
        //        IsCompressed = IsCompressed,
        //        Direction = Direction,
        //        Version = Version,
        //        MessageType = MessageType,
        //        CreatedTimeUtc = CreatedTimeUtc,
        //        LastUpdatedTime = LastUpdatedTime,
        //        Extensions = new System.Collections.Generic.Dictionary<string, object>(Extensions),
        //        Flag = Flag,
        //        TimestampUtc = TimestampUtc
        //    };
        //}

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
            if (BizData == null || BizData.Length == 0)
                return string.Empty;

            encoding ??= Encoding.UTF8;
            return encoding.GetString(BizData);
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
        /// 检查是否超时
        /// </summary>
        protected bool IsTimeout()
        {
            return (DateTime.UtcNow - CreatedTimeUtc).TotalMilliseconds > TimeoutMs;
        }

        #endregion
    }
}
