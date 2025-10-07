using System;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Core;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using MessagePack;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 统一数据包模型 - 核心通信协议实体
    /// 
    /// 网络传输层 - 只关心网络协议格式
    /// 职责：包头、加密、压缩、序列化等网络传输相关属性
    /// 不包含任何业务逻辑或业务属性
    /// </summary>
    [Serializable]
    public class PacketModel 
    {
        // 传输的指令数据里面有请求数据或响应数据
        public byte[] CommandData { get; set; }

        // 简单的命令标识（不包含业务逻辑）
        //命令类型
        public CommandId CommandId { get; set; }

        public ICommand Command { get; set; }
        /// <summary>
        /// 数据包状态
        /// </summary>
        public PacketStatus Status { get; set; }

        #region 网络传输属性

        public string SessionId { get; set; } // 属性声明


        /// <summary>
        /// 包标志位
        /// </summary>
        public string Flag { get; set; }

        public string ClientId { get; set; }

        /// <summary>
        /// 认证Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 数据包唯一标识符
        /// </summary>
        // 网络标识
        public string PacketId { get; set; }


        /// <summary>
        /// 数据包大小（字节）
        /// </summary>
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
        public string Checksum { get; set; }

        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// 是否压缩
        /// </summary>
        public bool IsCompressed { get; set; }

        /// <summary>
        /// 数据包方向
        /// </summary>
        public PacketDirection Direction { get; set; }

        /// <summary>
        /// 模型版本
        /// </summary>
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; } = MessageType.Request;

        /// <summary>
        /// 扩展属性字典（用于存储非核心但需要传输的元数据）
        /// </summary>
        public System.Collections.Generic.Dictionary<string, object> Extensions { get; set; }

        #endregion

        // 便捷创建方法
        public static PacketModel Create<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            return new PacketModel
            {
                PacketId = IdGenerator.GeneratePacketId(command.CommandIdentifier.Name),
                CommandId = command.CommandIdentifier,
                CommandData = MessagePackSerializer.Serialize(command),
                CreatedTimeUtc = command.CreatedTimeUtc
            };
        }


        #region ICoreEntity 接口实现

        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        public DateTime CreatedTimeUtc { get; set; }

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
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
        /// 更新时间戳（实现 ICoreEntity 接口）
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
        /// 从原始数据创建数据包
        /// </summary>
        /// <param name="originalData">原始数据</param>
        public PacketModel(byte[] originalData)
            : this()
        {
            //Data = originalData;
            Size = originalData?.Length ?? 0;
        }

        #endregion


        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public void ClearSensitiveData()
        {
            SessionId = null;
            ClientId = null;
            Token = null; // 清理Token
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

        /// <summary>
        /// 检查是否包含有效Token
        /// </summary>
        /// <returns>是否包含Token</returns>
        public bool HasToken()
        {
            return !string.IsNullOrEmpty(Token);
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
        /// JSON序列化缓存，用于缓存高频序列化操作（如心跳包）
        /// </summary>
        private static readonly ConcurrentDictionary<string, byte[]> _jsonCache = new ConcurrentDictionary<string, byte[]>();
        public PacketModel SetJsonData<T>(T data)
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
            CommandData = _jsonCache.GetOrAdd(cacheKey, jsonBytes);
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

                // 优先尝试MessagePack反序列化
                try
                {
                    return MessagePack.MessagePackSerializer.Deserialize<T>(CommandData);
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

                return MessagePack.MessagePackSerializer.Deserialize<T>(CommandData);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"提取Payload数据失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从数据包提取执行上下文
        /// </summary>
        /// <returns>命令执行上下文对象</returns>
        public CommandExecutionContext ExtractExecutionContext()
        {
            var context = CommandExecutionContext.CreateFromPacket(this);

            // 从Extensions中提取额外上下文信息
            if (Extensions != null)
            {
                foreach (var extension in Extensions)
                {
                    context.Extensions[extension.Key] = extension.Value;
                }
            }

            return context;
        }
        public PacketModel Clone()
        {
            return new PacketModel
            {
                PacketId = IdGenerator.GeneratePacketId(CommandId.Category.ToString()),
                CommandId = CommandId,
                Status = Status,
                SessionId = SessionId,
                ClientId = ClientId,
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
