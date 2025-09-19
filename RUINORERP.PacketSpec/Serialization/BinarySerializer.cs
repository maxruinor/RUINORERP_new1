using Newtonsoft.Json;
using System;
using RUINORERP.PacketSpec.Serialization;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 二进制序列化工具（支持Newtonsoft.Json和MessagePack）
    /// </summary>
    public static class BinarySerializer
    {
        /// <summary>
        /// 序列化对象到字节数组（使用JSON）
        /// </summary>
        public static byte[] Serialize(object obj)
        {
            return UnifiedSerializationService.SerializeWithJson(obj);
        }

        /// <summary>
        /// 使用MessagePack序列化对象到字节数组
        /// </summary>
        public static byte[] SerializeWithMessagePack(object obj)
        {
            return UnifiedSerializationService.SerializeWithMessagePack(obj);
        }

        /// <summary>
        /// 从字节数组反序列化对象（使用JSON）
        /// </summary>
        public static T Deserialize<T>(byte[] data) where T : class
        {
            return UnifiedSerializationService.DeserializeWithJson<T>(data);
        }

        /// <summary>
        /// 使用MessagePack从字节数组反序列化对象
        /// </summary>
        public static T DeserializeWithMessagePack<T>(byte[] data) where T : class
        {
            return UnifiedSerializationService.DeserializeWithMessagePack<T>(data);
        }

        /// <summary>
        /// 安全反序列化（捕获异常返回默认值）
        /// </summary>
        public static T SafeDeserialize<T>(byte[] data) where T : class
        {
            return UnifiedSerializationService.SafeDeserializeWithJson<T>(data);
        }

        /// <summary>
        /// 使用MessagePack安全反序列化（捕获异常返回默认值）
        /// </summary>
        public static T SafeDeserializeWithMessagePack<T>(byte[] data) where T : class
        {
            return UnifiedSerializationService.DeserializeWithMessagePack<T>(data);
        }

        /// <summary>
        /// 验证字节数组是否可以反序列化（使用JSON）
        /// </summary>
        public static bool CanDeserialize(byte[] data)
        {
            return UnifiedSerializationService.CanDeserializeWithJson(data);
        }

        /// <summary>
        /// 使用MessagePack验证字节数组是否可以反序列化
        /// </summary>
        public static bool CanDeserializeWithMessagePack(byte[] data)
        {
            return UnifiedSerializationService.TryDeserializeWithMessagePack<object>(data, out _);
        }
    }
}
