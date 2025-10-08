using MessagePack;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Text;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands.Authentication;

namespace TestSerialization
{
    /// <summary>
    /// 测试用的PacketModel类，结构接近原始PacketModel但使用连续Key编号
    /// </summary>
    [MessagePackObject]
    public class PacketModelTest
    {
        /// <summary>
        /// 保存指令实体数据
        /// </summary>
        [Key(100)]
        public byte[] CommandData { get; set; }

        /// <summary>
        /// 命令类型 - 简化为string类型以避免复杂结构体序列化问题
        /// </summary>
        [Key(101)]
        public string CommandId { get; set; } = string.Empty;

        /// <summary>
        /// 数据包状态
        /// </summary>
        [Key(102)]
        public PacketStatus Status { get; set; }

        /// <summary>
        /// 包标志位
        /// </summary>
        [Key(103)]
        public string Flag { get; set; } = "";

        /// <summary>
        /// 数据包唯一标识符
        /// </summary>
        [Key(104)]
        public string PacketId { get; set; } = "";

        /// <summary>
        /// 数据包大小（字节）
        /// </summary>
        [Key(105)]
        public int Size { get; set; }

        /// <summary>
        /// 校验和
        /// </summary>
        [Key(106)]
        public string Checksum { get; set; } = "";

        /// <summary>
        /// 是否加密
        /// </summary>
        [Key(107)]
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// 是否压缩
        /// </summary>
        [Key(108)]
        public bool IsCompressed { get; set; }

        /// <summary>
        /// 数据包方向
        /// </summary>
        [Key(109)]
        public PacketDirection Direction { get; set; }

        /// <summary>
        /// 模型版本
        /// </summary>
        [Key(110)]
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 消息类型
        /// </summary>
        [Key(111)]
        public MessageType MessageType { get; set; } = MessageType.Request;

        /// <summary>
        /// 会话ID - 简化版本，直接存储
        /// </summary>
        [Key(112)]
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// 客户端ID - 简化版本，直接存储  
        /// </summary>
        [Key(113)]
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 认证Token - 简化版本，直接存储
        /// </summary>
        [Key(114)]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        [Key(115)]
        public DateTime CreatedTimeUtc { get; set; }

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        [Key(116)]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        [Key(117)]
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// 获取包大小
        /// </summary>
        /// <returns>包大小（字节）</returns>
        public int GetPackageSize()
        {
            return CommandData?.Length ?? 0;
        }

        /// <summary>
        /// 更新时间戳
        /// </summary>
        public void UpdateTimestamp()
        {
            TimestampUtc = DateTime.UtcNow;
            LastUpdatedTime = TimestampUtc;
        }

        /// <summary>
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return CreatedTimeUtc <= DateTime.UtcNow &&
                   CreatedTimeUtc >= DateTime.UtcNow.AddYears(-1) &&
                   !string.IsNullOrEmpty(PacketId);
        }

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PacketModelTest()
        {
            PacketId = Guid.NewGuid().ToString();
            CreatedTimeUtc = DateTime.UtcNow;
            TimestampUtc = CreatedTimeUtc;
            CommandData = Array.Empty<byte>();
        }

        /// <summary>
        /// 从原始数据创建数据包
        /// </summary>
        /// <param name="originalData">原始数据</param>
        public PacketModelTest(byte[] originalData)
            : this()
        {
            Size = originalData?.Length ?? 0;
        }

        #endregion

        /// <summary>
        /// 获取JSON数据为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>反序列化后的对象</returns>
        public T? GetJsonData<T>()
        {
            if (CommandData == null || CommandData.Length == 0)
                return default(T);

            var json = Encoding.UTF8.GetString(CommandData);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public void ClearSensitiveData()
        {
            // 暂时注释掉，与原始PacketModel保持一致
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
        public T? ExtractRequest<T>() where T : class
        {
            try
            {
                if (CommandData == null || CommandData.Length == 0)
                    return default(T);

                // 简化版本，直接使用JSON反序列化
                var json = Encoding.UTF8.GetString(CommandData);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
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
        public T? ExtractPayload<T>()
        {
            try
            {
                if (CommandData == null || CommandData.Length == 0)
                    return default(T);

                // 简化版本，直接使用JSON反序列化
                var json = Encoding.UTF8.GetString(CommandData);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"提取Payload数据失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 克隆当前实例
        /// </summary>
        /// <returns>克隆的PacketModelTest实例</returns>
        public PacketModelTest Clone()
        {
            return new PacketModelTest
            {
                PacketId = Guid.NewGuid().ToString(),
                CommandId = CommandId,
                Status = Status,

                CommandData = CommandData?.Clone() as byte[],
                Size = Size,
                Checksum = Checksum ?? string.Empty,
                CreatedTimeUtc = CreatedTimeUtc,
                LastUpdatedTime = LastUpdatedTime,

                Flag = Flag ?? string.Empty,
                TimestampUtc = TimestampUtc,
                IsEncrypted = IsEncrypted,
                IsCompressed = IsCompressed,
                Direction = Direction,
                Version = Version ?? "2.0",
                MessageType = MessageType,

            };
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>数据包信息字符串</returns>
        public override string ToString()
        {
            return $"Packet[Id:{PacketId}, Size:{Size}, Encrypted:{IsEncrypted}, Compressed:{IsCompressed}]";
        }

        /// <summary>
        /// 创建测试实例
        /// </summary>
        /// <param name="testData">测试数据</param>
        /// <returns>测试实例</returns>
        public static PacketModelTest CreateTestInstance(string testData)
        {
            return new PacketModelTest
            {
                CommandData = System.Text.Encoding.UTF8.GetBytes(testData),
                CommandId = "SYSTEM_TEST_0x0100",
                Status = PacketStatus.Created,
                Flag = "TEST_FLAG",
                PacketId = Guid.NewGuid().ToString(),
                Size = System.Text.Encoding.UTF8.GetByteCount(testData),
                Checksum = "CHECKSUM123",
                IsEncrypted = false,
                IsCompressed = false,
                Direction = PacketDirection.ClientToServer,
                Version = "2.0",
                MessageType = MessageType.Request,
                CreatedTimeUtc = DateTime.UtcNow,
                TimestampUtc = DateTime.UtcNow,
                SessionId = "TEST_SESSION_123",
                ClientId = "TEST_CLIENT_456",
                Token = "TEST_TOKEN_789"
            };
        }
    }
}