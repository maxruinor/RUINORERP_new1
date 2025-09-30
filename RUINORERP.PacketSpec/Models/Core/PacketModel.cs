﻿﻿﻿﻿﻿using System;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Core;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using RUINORERP.PacketSpec.Commands;
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
    public class PacketModel :  IPacketData
    {
        // 传输数据
        public byte[] Data { get; set; }


        // 简单的命令标识（不包含业务逻辑）
        //命令类型
        public CommandId Command { get; set; }

        /// <summary>
        /// 数据包状态
        /// </summary>
        public PacketStatus Status { get; set; }

        #region 网络传输属性

        public string SessionId { get; set; } // 属性声明

        public string RequestId { get; set; }

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
            return Data?.Length ?? 0;
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
                Command = command.CommandIdentifier,
                //SessionId = command.SessionId,
                //ClientId = command.ClientId,
                Data = SerializeCommand(command)
            };
        }

        private static byte[] SerializeCommand<T>(T command)
    => MessagePackSerializer.Serialize(command);


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
            if (Data == null || Data.Length == 0)
                return default;

            var json = Encoding.UTF8.GetString(Data);
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
            if (Data != null)
            {
                Array.Clear(Data, 0, Data.Length);
                Data = null;
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
            this.Data = data;
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
            Data = _jsonCache.GetOrAdd(cacheKey, jsonBytes);
            Size = Data.Length;
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
            if (Data == null || Data.Length == 0)
                return string.Empty;

            encoding ??= Encoding.UTF8;
            return encoding.GetString(Data);
        }
        public PacketModel Clone()
        {
            return new PacketModel
            {
                PacketId = IdGenerator.GeneratePacketId(Command.Category.ToString()),
                Command = Command,
                Status = Status,
                SessionId = SessionId,
                ClientId = ClientId,
                Data = Data?.Clone() as byte[],
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
}
