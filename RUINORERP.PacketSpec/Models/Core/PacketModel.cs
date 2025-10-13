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
        /// 包标志位
        /// </summary>
        [Key(3)]
        public string Flag { get; set; }

        /// <summary>
        /// 数据包唯一标识符
        /// </summary>
        // 网络标识
        [Key(4)]
        public string PacketId { get; set; }

        /// <summary>
        /// 数据包大小（字节）
        /// </summary>
        [Key(5)]
        public int Size { get; set; }

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
        [Key(6)]
        public string Checksum { get; set; }

        /// <summary>
        /// 是否加密
        /// </summary>
        [Key(7)]
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// 是否压缩
        /// </summary>
        [Key(8)]
        public bool IsCompressed { get; set; }

        /// <summary>
        /// 数据包方向
        /// </summary>
        [Key(9)]
        public PacketDirection Direction { get; set; }

        /// <summary>
        /// 模型版本
        /// </summary>
        [Key(10)]
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 消息类型
        /// </summary>
        [Key(11)]
        public MessageType MessageType { get; set; } = MessageType.Request;

        /// <summary>
        /// 扩展属性字典（用于存储非核心但需要传输的元数据）
        /// </summary>
        [MessagePack.IgnoreMember]  // 忽略此属性，不序列化
        public System.Collections.Generic.Dictionary<string, object> Extensions { get; set; }

        /// <summary>
        /// 命令执行上下文 - 网络传输层使用
        /// 包含会话、认证、追踪等基础设施信息
        /// </summary>
        [Key(12)]
        public CmdContext  ExecutionContext { get; set; }

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
                    ExecutionContext = new CmdContext ();
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
                    ExecutionContext = new CmdContext ();
                if (ExecutionContext.Token == null)
                    ExecutionContext.Token = new Commands.Authentication.TokenInfo();
                ExecutionContext.Token.AccessToken = value;
            }
        }

        #endregion

        // 便捷创建方法
        public static PacketModel Create<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            return new PacketModel
            {
                PacketId = IdGenerator.GeneratePacketId(command.CommandIdentifier.Name),
                CommandId = command.CommandIdentifier,
                CommandData = UnifiedSerializationService.SerializeWithMessagePack(command),
                CreatedTimeUtc = command.CreatedTimeUtc
            };
        }

        #region ITimestamped 接口实现

        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        [Key(13)]
        public DateTime CreatedTimeUtc { get; set; }

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        [Key(14)]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        [Key(15)]
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// 验证模型有效性（实现 ICoreEntity 接口）
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return CreatedTimeUtc <= DateTime.UtcNow &&
                   CreatedTimeUtc >= DateTime.UtcNow.AddYears(-1) &&
                   !string.IsNullOrEmpty(PacketId);
        }

        /// <summary>
        /// 更新时间戳（实现 ITimestamped 接口）
        /// </summary>
        public void UpdateTimestamp()
        {
            TimestampUtc = DateTime.UtcNow;
            LastUpdatedTime = TimestampUtc;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PacketModel()
        {
            PacketId = Guid.NewGuid().ToString();
            CreatedTimeUtc = DateTime.UtcNow;
            TimestampUtc = CreatedTimeUtc;
            Extensions = new System.Collections.Generic.Dictionary<string, object>();
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

        #region 重写方法

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>数据包信息字符串</returns>
        public override string ToString()
        {
            return $"Packet[Id:{PacketId}, Size:{Size}, Encrypted:{IsEncrypted}, Compressed:{IsCompressed}]";
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
            Size = CommandData.Length;
            LastUpdatedTime = DateTime.UtcNow;
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
            Size = CommandData.Length;
            LastUpdatedTime = DateTime.UtcNow;
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
            Size = CommandData.Length;
            LastUpdatedTime = DateTime.UtcNow;
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
                Size = Size,
                Checksum = Checksum,
                CreatedTimeUtc = CreatedTimeUtc,
                LastUpdatedTime = LastUpdatedTime,
                Extensions = new System.Collections.Generic.Dictionary<string, object>(Extensions),
                Flag = Flag,
                TimestampUtc = TimestampUtc
            };
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
