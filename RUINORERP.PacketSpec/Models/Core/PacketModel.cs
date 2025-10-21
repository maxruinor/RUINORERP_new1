using System;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using MessagePack;
using RUINORERP.PacketSpec.Commands.Authentication;
using System.Net.Sockets;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 统一数据包模型 - 核心通信协议实体
    /// 
    /// 网络传输层 - 只关心网络协议格式
    /// 职责：包头、加密、压缩、序列化等网络传输相关属性
    /// 不包含任何业务逻辑或业务属性
    /// </summary>
    [MessagePackObject]
    public class PacketModel : ITimestamped
    {
        /// <summary>
        /// 保存指令实体数据
        /// </summary>
        [Key(0)]
        [Obsolete("将来去掉")]
        public byte[] CommandData { get; set; }

        // 简单的命令标识（不包含业务逻辑）
        //命令类型
        [Key(1)]
        public CommandId CommandId { get; set; }

        /// <summary>
        /// 数据包状态
        /// </summary>
        [Key(2)]
        public PacketStatus Status { get; set; }

        #region 网络传输属性

        /// <summary>
        /// 数据包唯一标识符
        /// </summary>
        // 网络标识
        [Key(3)]
        public string PacketId { get; set; }

        /// <summary>
        /// 数据包大小（字节）
        /// </summary>
        [IgnoreMember]
        public int Size => CommandData?.Length ?? 0;

        /// <summary>
        /// 获取包大小
        /// </summary>
        public int GetPackageSize()
        {
            return CommandData?.Length ?? 0;
        }

        /// <summary>
        /// 校验和
        /// </summary>
        [IgnoreMember]
        public string Checksum => ComputeHash();

        /// <summary>
        /// 数据包方向
        /// </summary>
        [Key(4)]
        public PacketDirection Direction { get; set; }

        /// <summary>
        /// 扩展属性字典（用于存储非核心但需要传输的元数据）
        /// </summary>
        [MessagePack.IgnoreMember]  // 忽略此属性，不序列化
        public System.Collections.Generic.Dictionary<string, object> Extensions { get; set; }

        /// <summary>
        /// 命令执行上下文 - 网络传输层使用
        /// 包含会话、认证、追踪等基础设施信息
        /// </summary>
        [Key(5)]
        public CommandContext ExecutionContext { get; set; }

        /// <summary>
        /// 会话ID（通过ExecutionContext获取）
        /// </summary>
        [IgnoreMember]
        public string SessionId
        {
            get => ExecutionContext?.SessionId;
            set
            {
                if (ExecutionContext == null)
                    ExecutionContext = new CommandContext();
                ExecutionContext.SessionId = value;
            }
        }



        /// <summary>
        /// 认证Token（通过ExecutionContext获取）
        /// </summary>
        [IgnoreMember]
        public string Token
        {
            get => ExecutionContext?.Token?.AccessToken;
            set
            {
                if (ExecutionContext == null)
                    ExecutionContext = new CommandContext();
                if (ExecutionContext.Token == null)
                    ExecutionContext.Token = new Commands.Authentication.TokenInfo();
                ExecutionContext.Token.AccessToken = value;
            }
        }

        #endregion

        /// <summary>
        /// 创建数据包的便捷方法 - 不再依赖ICommand接口
        /// </summary>
        /// <typeparam name="TData">数据对象类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="data">数据对象</param>
        /// <returns>创建的数据包</returns>
        public static PacketModel Create<TData>(CommandId commandId, TData data)
        {
            if (commandId == null)
                throw new ArgumentNullException(nameof(commandId), "命令标识符不能为空");

            return new PacketModel
            {
                PacketId = IdGenerator.GeneratePacketId(commandId.Name),
                CommandId = commandId,
                CommandData = data != null ? UnifiedSerializationService.SerializeWithMessagePack(data) : Array.Empty<byte>(),
            };
        }

        /// <summary>
        /// 使用请求对象创建数据包
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <param name="commandId">命令标识符</param>
        /// <param name="request">请求对象</param>
        /// <returns>创建的数据包</returns>
        public static PacketModel CreateFromRequest<TRequest>(CommandId commandId, TRequest request) where TRequest : IRequest
        {
            var packet = Create(commandId, request);
            packet.Request = request;
            packet.Direction = PacketDirection.ClientToServer;
            return packet;
        }

        #region ITimestamped 接口实现
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        [Key(6)]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// 记录对象的当前状态时间点，会随着对象状态变化而更新
        /// </summary>
        [Key(7)]
        public DateTime TimestampUtc { get; set; }


        /// <summary>
        /// 请求模型
        /// </summary>
        [Key(8)]
        public IRequest Request { get; set; }


        /// <summary>
        /// 响应模型
        /// </summary>
        [Key(9)]
        public IResponse Response { get; set; }

        [Key(10)]
        public PacketPriority PacketPriority { get; set; }
        /// <summary>
        /// 验证模型有效性（实现 ICoreEntity 接口）
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return CreatedTime <= DateTime.Now &&
                   CreatedTime >= DateTime.Now.AddYears(-1) &&
                   !string.IsNullOrEmpty(PacketId);
        }

        /// <summary>
        /// 更新时间戳（实现 ITimestamped 接口）
        /// </summary>
        public void UpdateTimestamp()
        {
            TimestampUtc = DateTime.UtcNow;
        }
        #endregion

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PacketModel()
        {
            PacketId = Guid.NewGuid().ToString();
            CreatedTime = DateTime.UtcNow;
            TimestampUtc = CreatedTime;
            Extensions = new System.Collections.Generic.Dictionary<string, object>();
            // 初始化默认扩展属性
            SetExtension("Version", "2.0");
            SetExtension("MessageType", MessageType.Request);
            // 移除CommandData的强制初始化，允许外部设置数据
            // CommandData = Array.Empty<byte>();
        }

        /// <summary>
        /// 获取JSON数据为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>反序列化后的对象</returns>
        public T GetJsonData<T>()
        {
            if (CommandData == null || CommandData.Length == 0)
                return default;

            var json = Encoding.UTF8.GetString(CommandData);
            return JsonConvert.DeserializeObject<T>(json);
        }


        /// <summary>
        /// 获取MessagePack数据为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>反序列化后的对象</returns>
        public T GetMessagePackData<T>()
        {
            if (CommandData == null || CommandData.Length == 0)
                return default;

            // 为了利用缓存，我们需要重新计算缓存键
            // 但由于我们不知道原始数据，只能从CommandData反序列化
            // 这里保持简单实现，直接反序列化
            return UnifiedSerializationService.DeserializeWithMessagePack<T>(CommandData);
        }

        #endregion

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public void ClearSensitiveData()
        {

            // 清理ExecutionContext中的敏感数据
            if (ExecutionContext != null)
            {
                ExecutionContext.SessionId = null;
                if (ExecutionContext.Token != null)
                {
                    ExecutionContext.Token.AccessToken = null;
                }
            }

            Extensions?.Clear();
            // 清理包体数据
            if (CommandData != null)
            {
                Array.Clear(CommandData, 0, CommandData.Length);
                CommandData = null;
            }
        }

        /// <summary>
        /// 设置Token
        /// </summary>
        /// <param name="token">认证Token</param>
        public void SetToken(string token)
        {
            Token = token;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns>认证Token</returns>
        public string GetToken()
        {
            return Token;
        }

        #region 扩展数据存储方法

        /// <summary>
        /// 获取扩展属性值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">属性键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>属性值或默认值</returns>
        public T GetExtension<T>(string key, T defaultValue = default)
        {
            return Extensions.TryGetValue(key, out var value) ? (T)value : defaultValue;
        }

        /// <summary>
        /// 设置扩展属性值
        /// </summary>
        /// <param name="key">属性键</param>
        /// <param name="value">属性值</param>
        public void SetExtension(string key, object value)
        {
            Extensions[key] = value;
        }

        /// <summary>
        /// 获取包标志位
        /// </summary>
        [IgnoreMember]
        public string Flag
        {
            get => GetExtension<string>("Flag");
            set => SetExtension("Flag", value);
        }

        /// <summary>
        /// 获取模型版本
        /// </summary>
        [IgnoreMember]
        public string Version
        {
            get => GetExtension<string>("Version");
            set => SetExtension("Version", value);
        }

        /// <summary>
        /// 获取消息类型
        /// </summary>
        [IgnoreMember]
        public MessageType MessageType
        {
            get => GetExtension<MessageType>("MessageType");
            set => SetExtension("MessageType", value);
        }

        #endregion

        #region 重写方法

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>数据包信息字符串</returns>
        public override string ToString()
        {
            return $"Packet[Id:{PacketId}, Size:{Size}, Flag:{Flag}, Version:{Version}, MessageType:{MessageType}]";
        }

        internal void SetData(byte[] data)
        {
            this.CommandData = data;
        }

        /// <summary>
        /// 设置JSON格式数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">要序列化的数据</param>
        /// <returns>当前数据包实例</returns>
        public PacketModel SetJsonData<T>(T data)
        {
            CommandData = UnifiedSerializationService.SerializeWithJson(data);
            UpdateTimestamp();  // 使用统一的时间戳更新方法
            return this;
        }


        /// <summary>
        /// 设置MessagePack格式数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">要序列化的数据</param>
        /// <returns>当前数据包实例</returns>
        public PacketModel SetCommandDataByMessagePack<T>(T data)
        {
            CommandData = MessagePackSerializer.Serialize<T>(data, UnifiedSerializationService.MessagePackOptions);
            UpdateTimestamp();  // 使用统一的时间戳更新方法
            return this;
        }


        /// <summary>
        /// 设置MessagePack格式数据（非泛型版本）
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <param name="data">要序列化的数据</param>
        /// <returns>当前数据包实例</returns>
        public PacketModel SetCommandDataByMessagePack(Type type, object data)
        {
            CommandData = MessagePackSerializer.Serialize(data, UnifiedSerializationService.MessagePackOptions);
            UpdateTimestamp();  // 使用统一的时间戳更新方法
            return this;
        }


        #endregion

        /// <summary>
        /// 获取数据为文本格式
        /// </summary>
        /// <param name="encoding">编码格式，默认UTF-8</param>
        /// <returns>文本数据</returns>
        public string GetDataAsText(Encoding encoding = null)
        {
            if (CommandData == null || CommandData.Length == 0)
                return string.Empty;

            encoding ??= Encoding.UTF8;
            return encoding.GetString(CommandData);
        }

        /// <summary>
        /// 提取请求数据为指定的IRequest类型
        /// </summary>
        /// <typeparam name="T">目标IRequest类型</typeparam>
        /// <returns>反序列化后的请求对象</returns>
        /// <exception cref="InvalidOperationException">提取请求数据失败时抛出</exception>
        public T ExtractRequest<T>() where T : class, IRequest
        {
            try
            {
                if (CommandData == null || CommandData.Length == 0)
                    return default(T);

                // 优先尝试使用UnifiedSerializationService进行MessagePack反序列化
                try
                {
                    return UnifiedSerializationService.DeserializeWithMessagePack<T>(CommandData);
                }
                catch
                {
                    // 回退到JSON反序列化
                    var json = Encoding.UTF8.GetString(CommandData);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"提取请求数据失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 提取Payload数据为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>反序列化后的Payload对象</returns>
        /// <exception cref="InvalidOperationException">提取Payload数据失败时抛出</exception>
        public T ExtractPayload<T>()
        {
            try
            {
                if (CommandData == null || CommandData.Length == 0)
                    return default(T);

                return UnifiedSerializationService.DeserializeWithMessagePack<T>(CommandData);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"提取Payload数据失败: {ex.Message}", ex);
            }
        }

        public PacketModel Clone()
        {
            return new PacketModel
            {
                PacketId = IdGenerator.GeneratePacketId(CommandId.Category.ToString()),
                CommandId = CommandId,
                Status = Status,
                SessionId = SessionId,
                CommandData = CommandData?.Clone() as byte[],
                CreatedTime = CreatedTime,
                Extensions = new System.Collections.Generic.Dictionary<string, object>(Extensions),
                TimestampUtc = TimestampUtc
            };
        }

        /// <summary>
        /// 计算数据包的哈希值
        /// </summary>
        /// <returns>数据包的哈希值</returns>
        private string ComputeHash()
        {
            // 这里实现具体的哈希计算逻辑
            // 例如使用SHA256或其他哈希算法
            // 为简化起见，这里返回一个占位符
            return string.Empty; // 实际实现中应该计算CommandData的哈希值
        }
    }

    /// <summary>
    /// PacketModel的扩展方法类
    /// </summary>
    public static class PacketModelExtensions
    {
        /// <summary>
        /// 从数据包中提取命令ID
        /// 供服务端Pipeline解码时直接拿到CommandId，无需再new CommandId(...)重复拼装
        /// </summary>
        /// <param name="packet">数据包模型实例</param>
        /// <returns>命令ID对象</returns>
        public static CommandId ExtractCommandId(this PacketModel packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet), "数据包不能为空");

            // 直接返回已有的Command属性，避免重新创建CommandId对象
            return packet.CommandId;
        }
    }
}
