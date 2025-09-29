﻿﻿﻿using System;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 统一数据包模型 - 核心通信协议实体
    /// 只关注网络传输层相关属性，不包含业务逻辑
    /// </summary>
    [Serializable]
    public class PacketModel : ICoreEntity, IPacketData
    {
        #region 网络传输属性

        /// <summary>
        /// 包标志位
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 包体数据
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// 数据包唯一标识符
        /// </summary>
        public string PacketId { get; set; }

        /// <summary>
        /// 实体唯一标识（实现 ICoreEntity 接口）
        /// </summary>
        public string Id 
        { 
            get => PacketId; 
            set => PacketId = value; 
        }

        /// <summary>
        /// 数据包大小（字节）
        /// </summary>
        public int Size { get; set; }

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

        #endregion

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
        /// 时间戳（用于验证请求有效性）
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
                   !string.IsNullOrEmpty(PacketId) &&
                   Body != null &&
                   Body.Length > 0 &&
                   Size == Body.Length;
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
            PacketId = IdGenerator.GeneratePacketId("DEFAULT");
            CreatedTimeUtc = DateTime.UtcNow;
            TimestampUtc = CreatedTimeUtc;
            Extensions = new Dictionary<string, object>();
        }

        /// <summary>
        /// 从原始数据创建数据包
        /// </summary>
        /// <param name="originalData">原始数据</param>
        public PacketModel(byte[] originalData)
            : this()
        {
            Body = originalData;
            Size = originalData?.Length ?? 0;
        }

        #endregion

        #region 核心方法

        /// <summary>
        /// 从原始数据创建数据包
        /// </summary>
        /// <param name="originalData">原始数据字节数组</param>
        /// <returns>PacketModel实例</returns>
        public static PacketModel FromOriginalData(byte[] originalData)
        {
            return new PacketModel(originalData);
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
        /// 安全清理敏感数据
        /// </summary>
        public void ClearSensitiveData()
        {
            // 清理包体数据
            if (Body != null)
            {
                Array.Clear(Body, 0, Body.Length);
                Body = null;
            }
        }

        /// <summary>
        /// 创建数据包克隆
        /// </summary>
        /// <returns>克隆实例</returns>
        public PacketModel Clone()
        {
            return new PacketModel
            {
                PacketId = IdGenerator.GeneratePacketId("CLONE"),
                Body = Body?.Clone() as byte[],
                Size = Size,
                Checksum = Checksum,
                CreatedTimeUtc = CreatedTimeUtc,
                LastUpdatedTime = LastUpdatedTime,
                Flag = Flag,
                TimestampUtc = TimestampUtc,
                IsEncrypted = IsEncrypted,
                IsCompressed = IsCompressed
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

        #region 扩展属性（用于传输层以上信息传递）

        /// <summary>
        /// 扩展属性字典
        /// 用于传输层以上信息传递，不参与序列化
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, object> Extensions { get; set; }

        #endregion

        #region 重写方法

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>数据包信息字符串</returns>
        public override string ToString()
        {
            return $"Packet[Id:{PacketId}, Size:{Size}]";
        }

        #endregion
    }
}
