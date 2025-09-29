﻿﻿﻿﻿﻿﻿using System;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 统一数据包模型 - 核心通信协议实体
    /// 替代原有的PacketInfo和PacketModel
    /// 直接支持SuperSocket
    /// </summary>
    [Serializable]
    public class PacketModel : ITraceable, IValidatable, IPacketData
    {
        #region 公共属性

        /// <summary>
        /// 包标志位
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 包体数据
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 数据包方向
        /// </summary>
        public PacketDirection Direction { get; set; }

        /// <summary>
        /// 数据包优先级
        /// </summary>
        public PacketPriority Priority { get; set; } = PacketPriority.Normal;

        /// <summary>
        /// 模型版本
        /// </summary>
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; } = MessageType.Request;

        /// <summary>
        /// 获取包大小
        /// </summary>
        public int GetPackageSize()
        {
            return Body?.Length ?? 0;
        }

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public virtual void ClearSensitiveData()
        {
            // 清理包体数据
            if (Body != null)
            {
                Array.Clear(Body, 0, Body.Length);
                Body = null;
            }
        }

        #endregion

        #region 属性定义

        /// <summary>
        /// JSON序列化缓存，用于缓存高频序列化操作（如心跳包）
        /// </summary>
        private static readonly ConcurrentDictionary<string, byte[]> _jsonCache = new ConcurrentDictionary<string, byte[]>();
        
        /// <summary>
        /// 数据包唯一标识符
        /// </summary>
        public string PacketId { get; set; }

        /// <summary>
        /// 命令类型
        /// </summary>
        public CommandId Command { get; set; }



        /// <summary>
        /// 数据包状态
        /// </summary>
        public PacketStatus Status { get; set; }


        /// <summary>
        /// 客户端标识
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 数据包大小（字节）
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 校验和
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// 扩展属性字典
        /// 包含：RequestId，
        /// </summary>
        public System.Collections.Generic.Dictionary<string, object> Extensions { get; set; }

        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// 是否压缩
        /// </summary>
        public bool IsCompressed { get; set; }

        #endregion

        #region ITraceable 接口实现

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
        /// 更新时间戳
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
            PacketId = GeneratePacketId();
            CreatedTimeUtc = DateTime.UtcNow;
            TimestampUtc = CreatedTimeUtc;
            Status = PacketStatus.Created;
            Extensions = new System.Collections.Generic.Dictionary<string, object>();
        }

        /// <summary>
        /// 从原始数据创建数据包
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <param name="command">命令标识符</param>
        public PacketModel(byte[] originalData, CommandId command = default(CommandId))
            : this()
        {
            Body = originalData;
            Command = command;
            Size = originalData?.Length ?? 0;
        }

        /// <summary>
        /// 从原始数据创建指定类型的数据包
        /// </summary>
        /// <param name="originalData">原始数据字节数组</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="command">命令标识符</param>
        /// <returns>PacketModel实例</returns>
        public static PacketModel FromOriginalData(byte[] originalData, RUINORERP.PacketSpec.Enums.Core.MessageType messageType, CommandId command = default(CommandId))
        {
            var packet = new PacketModel(originalData, command);
            packet.MessageType = messageType;
            return packet;
        }

        #endregion

        #region 核心方法

        /// <summary>
        /// 从原始数据创建数据包
        /// </summary>
        /// <param name="originalData">原始数据字节数组</param>
        /// <param name="command">命令标识符</param>
        /// <returns>PacketModel实例</returns>
        public static PacketModel FromOriginalData(byte[] originalData, CommandId command = default(CommandId))
        {
            return new PacketModel(originalData, command);
        }



        /// <summary>
        /// 设置会话信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns>当前实例</returns>
        public PacketModel SetSessionInfo(string sessionId, string clientId = null)
        {
            SessionId = sessionId;
            ClientId = clientId;
            return this;
        }

        /// <summary>
        /// 设置数据内容
        /// </summary>
        /// <param name="data">数据字节数组</param>
        /// <returns>当前实例</returns>
        public PacketModel SetData(byte[] data)
        {
            Body = data;
            Size = data?.Length ?? 0;
            LastUpdatedTime = DateTime.UtcNow;
            return this;
        }

        /// <summary>
        /// 设置JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据对象</param>
        /// <returns>当前实例</returns>
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
            Body = _jsonCache.GetOrAdd(cacheKey, jsonBytes);
            Size = Body.Length;
            LastUpdatedTime = DateTime.UtcNow;
            return this;
        }

        /// <summary>
        /// 获取数据为文本格式
        /// </summary>
        /// <param name="encoding">编码格式，默认UTF-8</param>
        /// <returns>文本数据</returns>
        public string GetDataAsText(Encoding encoding = null)
        {
            if (Body == null || Body.Length == 0)
                return string.Empty;

            encoding ??= Encoding.UTF8;
            return encoding.GetString(Body);
        }

        /// <summary>
        /// 获取JSON数据为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>反序列化后的对象</returns>
        public T GetJsonData<T>()
        {
            if (Body == null || Body.Length == 0)
                return default;

            var json = Encoding.UTF8.GetString(Body);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 验证数据包有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return CreatedTimeUtc <= DateTime.UtcNow &&
                   CreatedTimeUtc >= DateTime.UtcNow.AddYears(-1) &&
                   !string.IsNullOrEmpty(PacketId) &&
                   Body != null &&
                   Body.Length > 0 &&
                   Size == Body.Length;
        }

        /// <summary>
        /// 验证数据包有效性 - IValidatable接口实现
        /// </summary>
        /// <returns>是否有效</returns>
        bool IValidatable.IsValid()
        {
            return IsValid();
        }

        /// <summary>
        /// 清理敏感数据
        /// </summary>
        public override void ClearSensitiveData()
        {
            base.ClearSensitiveData();
            SessionId = null;
            ClientId = null;
            Extensions?.Clear();
        }

        /// <summary>
        /// 创建数据包克隆
        /// </summary>
        /// <returns>克隆实例</returns>
        public PacketModel Clone()
        {
            return new PacketModel
            {
                PacketId = GeneratePacketId(),
                Command = Command,
                Status = Status,
                SessionId = SessionId,
                ClientId = ClientId,
                Body = Body?.Clone() as byte[],
                Size = Size,
                Checksum = Checksum,
                CreatedTimeUtc = CreatedTimeUtc,
                LastUpdatedTime = LastUpdatedTime,
                Extensions = new System.Collections.Generic.Dictionary<string, object>(Extensions),
                Flag = Flag,
                TimestampUtc = TimestampUtc
            };
        }

        /// <summary>
        /// 从二进制数据创建PacketModel实例
        /// </summary>
        /// <param name="binaryData">二进制数据</param>
        /// <returns>PacketModel实例</returns>
        public static PacketModel FromBinary(byte[] binaryData)
        {
            if (binaryData == null || binaryData.Length == 0)
                throw new ArgumentException("二进制数据不能为空", nameof(binaryData));

            // 这里应该使用实际的反序列化逻辑
            // 暂时返回一个基本实现
            var packet = new PacketModel();
            packet.Body = binaryData;
            packet.Size = binaryData.Length;
            return packet;
        }

        /// <summary>
        /// 转换为二进制数据
        /// </summary>
        /// <returns>二进制数据</returns>
        public byte[] ToBinary()
        {
            return Body?.Clone() as byte[] ?? new byte[0];
        }

        #endregion

        #region 私有辅助方法

        /// <summary>
        /// 生成数据包ID
        /// </summary>
        /// <returns>唯一数据包ID</returns>
        private string GeneratePacketId()
        {
            return $"PKT_{Command.Category.ToString()}_{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}";
        }

        /// <summary>
        /// 计算数据包大小
        /// </summary>
        /// <returns>数据包大小</returns>
        private int CalculateSize()
        {
            return Body?.Length ?? 0;
        }

        #endregion

        #region 重写方法

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>数据包信息字符串</returns>
        public override string ToString()
        {
            return $"Packet[Id:{PacketId}, Command:{Command}, Size:{Size}, Status:{Status}]";
        }

        #endregion
    }
}
