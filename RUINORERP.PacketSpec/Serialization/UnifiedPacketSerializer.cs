using System;
using System.IO;
using System.Text;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// ✅ [最新架构] 统一数据包序列化器
    /// 支持PacketModel和BusinessPacket的序列化/反序列化
    /// </summary>
    public static class UnifiedPacketSerializer
    {
        /// <summary>
        /// 序列化统一数据包到字节数组 (使用JSON)
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] SerializePacket(PacketModel packet)
        {
            return UnifiedSerializationService.SerializeWithJson(packet);
        }

        /// <summary>
        /// 使用MessagePack序列化统一数据包到字节数组
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] SerializePacketWithMessagePack(PacketModel packet)
        {
            return UnifiedSerializationService.SerializeWithMessagePack(packet);
        }

        /// <summary>
        /// 从字节数组反序列化统一数据包 (使用JSON)
        /// </summary>
        /// <param name="data">序列化的字节数组</param>
        /// <returns>统一数据包</returns>
        public static PacketModel DeserializeUnifiedPacket(byte[] data)
        {
            return UnifiedSerializationService.DeserializeWithJson<PacketModel>(data);
        }

        /// <summary>
        /// 使用MessagePack从字节数组反序列化统一数据包
        /// </summary>
        /// <param name="data">序列化的字节数组</param>
        /// <returns>统一数据包</returns>
        public static PacketModel DeserializeUnifiedPacketWithMessagePack(byte[] data)
        {
            return UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(data);
        }

        /// <summary>
        /// 从字节数组反序列化业务数据包
        /// </summary>
        /// <param name="data">序列化的字节数组</param>
        /// <returns>业务数据包</returns>
        public static PacketModel DeserializeBusinessPacket(byte[] data)
        {
            return UnifiedSerializationService.DeserializeWithJson<PacketModel>(data);
        }

        /// <summary>
        /// 使用MessagePack从字节数组反序列化业务数据包
        /// </summary>
        /// <param name="data">序列化的字节数组</param>
        /// <returns>业务数据包</returns>
        public static PacketModel DeserializeBusinessPacketWithMessagePack(byte[] data)
        {
            return UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(data);
        }

        /// <summary>
        /// 安全反序列化统一数据包（捕获异常返回默认值）
        /// </summary>
        /// <param name="data">序列化的字节数组</param>
        /// <returns>统一数据包或null</returns>
        public static PacketModel SafeDeserializeUnifiedPacket(byte[] data)
        {
            return UnifiedSerializationService.SafeDeserializeWithJson<PacketModel>(data);
        }

        /// <summary>
        /// 使用MessagePack安全反序列化统一数据包（捕获异常返回默认值）
        /// </summary>
        /// <param name="data">序列化的字节数组</param>
        /// <returns>统一数据包或null</returns>
        public static PacketModel SafeDeserializeUnifiedPacketWithMessagePack(byte[] data)
        {
            return UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(data);
        }

        /// <summary>
        /// 安全反序列化业务数据包（捕获异常返回默认值）
        /// </summary>
        /// <param name="data">序列化的字节数组</param>
        /// <returns>业务数据包或null</returns>
        public static PacketModel SafeDeserializeBusinessPacket(byte[] data)
        {
            return UnifiedSerializationService.SafeDeserializeWithJson<PacketModel>(data);
        }

        /// <summary>
        /// 使用MessagePack安全反序列化业务数据包（捕获异常返回默认值）
        /// </summary>
        /// <param name="data">序列化的字节数组</param>
        /// <returns>业务数据包或null</returns>
        public static PacketModel SafeDeserializeBusinessPacketWithMessagePack(byte[] data)
        {
            return UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(data);
        }

        /// <summary>
        /// 序列化数据包到二进制格式（用于网络传输）
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>二进制数据</returns>
        public static byte[] SerializeToBinary(PacketModel packet)
        {
            return UnifiedSerializationService.SerializeToBinary(packet);
        }

        /// <summary>
        /// 使用MessagePack序列化数据包到二进制格式（用于网络传输）
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>二进制数据</returns>
        public static byte[] SerializeToBinaryWithMessagePack(PacketModel packet)
        {
            return UnifiedSerializationService.SerializeWithMessagePack(packet);
        }

        /// <summary>
        /// 从二进制格式反序列化数据包
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>统一数据包</returns>
        public static PacketModel DeserializeFromBinary(byte[] data)
        {
            return UnifiedSerializationService.DeserializeFromBinary<PacketModel>(data);
        }

        /// <summary>
        /// 使用MessagePack从二进制格式反序列化数据包
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>统一数据包</returns>
        public static PacketModel DeserializeFromBinaryWithMessagePack(byte[] data)
        {
            return UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(data);
        }

        /// <summary>
        /// 验证字节数组是否可以反序列化
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>是否可以反序列化</returns>
        public static bool CanDeserialize(byte[] data)
        {
            return UnifiedSerializationService.CanDeserializeWithJson(data);
        }

        /// <summary>
        /// 使用MessagePack验证字节数组是否可以反序列化
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>是否可以反序列化</returns>
        public static bool CanDeserializeWithMessagePack(byte[] data)
        {
            return UnifiedSerializationService.TryDeserializeWithMessagePack<PacketModel>(data, out _);
        }

        /// <summary>
        /// 获取序列化后的数据大小
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>序列化后的字节数</returns>
        public static int GetSerializedSize(PacketModel packet)
        {
            // 计算二进制格式的大小
            var sessionIdBytes = string.IsNullOrEmpty(packet.SessionId) 
                ? Array.Empty<byte>() 
                : Encoding.UTF8.GetBytes(packet.SessionId);
            
            // 简化计算，实际大小会根据序列化方式有所不同
            return 16 + // 包头：4个int32
                   (packet.Body?.Length ?? 0) + 
                   (packet.Extensions.ContainsKey("DataTwo") ? ((byte[])packet.Extensions["DataTwo"]).Length : 0) + 
                   sessionIdBytes.Length;
        }

        /// <summary>
        /// 使用MessagePack获取序列化后的数据大小
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>序列化后的字节数</returns>
        public static int GetSerializedSizeWithMessagePack(PacketModel packet)
        {
            return UnifiedSerializationService.GetSerializedSizeWithMessagePack(packet);
        }

        /// <summary>
        /// 压缩序列化（使用GZip）
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>压缩后的字节数组</returns>
        public static byte[] SerializeCompressed(PacketModel packet)
        {
            return UnifiedSerializationService.SerializeCompressed(packet, false);
        }

        /// <summary>
        /// 使用MessagePack压缩序列化（使用GZip）
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>压缩后的字节数组</returns>
        public static byte[] SerializeCompressedWithMessagePack(PacketModel packet)
        {
            return UnifiedSerializationService.SerializeCompressed(packet, true);
        }

        /// <summary>
        /// 解压缩反序列化
        /// </summary>
        /// <param name="compressedData">压缩的字节数组</param>
        /// <returns>统一数据包</returns>
        public static PacketModel DeserializeCompressed(byte[] compressedData)
        {
            return UnifiedSerializationService.DeserializeCompressed<PacketModel>(compressedData, false);
        }

        /// <summary>
        /// 使用MessagePack解压缩反序列化
        /// </summary>
        /// <param name="compressedData">压缩的字节数组</param>
        /// <returns>统一数据包</returns>
        public static PacketModel DeserializeCompressedWithMessagePack(byte[] compressedData)
        {
            return UnifiedSerializationService.DeserializeCompressed<PacketModel>(compressedData, true);
        }
    }
}
