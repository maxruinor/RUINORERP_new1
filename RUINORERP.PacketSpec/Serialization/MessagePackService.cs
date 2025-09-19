using System;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// MessagePack序列化服务
    /// 用于替代当前手动解析字节数据的逻辑，提供高性能的二进制序列化
    /// </summary>
    public static class MessagePackService
    {
        /// <summary>
        /// 序列化对象为字节数组
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] Serialize<T>(T obj)
        {
            return UnifiedSerializationService.SerializeWithMessagePack(obj);
        }

        /// <summary>
        /// 异步序列化对象为字节数组
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        public static async Task<byte[]> SerializeAsync<T>(T obj)
        {
            return await UnifiedSerializationService.SerializeWithMessagePackAsync(obj);
        }

        /// <summary>
        /// 反序列化字节数组为对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public static T Deserialize<T>(byte[] data)
        {
            return UnifiedSerializationService.DeserializeWithMessagePack<T>(data);
        }

        /// <summary>
        /// 异步反序列化字节数组为对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public static async Task<T> DeserializeAsync<T>(byte[] data)
        {
            return await UnifiedSerializationService.DeserializeWithMessagePackAsync<T>(data);
        }

        /// <summary>
        /// 尝试反序列化字节数组为对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <param name="result">反序列化后的对象</param>
        /// <returns>是否反序列化成功</returns>
        public static bool TryDeserialize<T>(byte[] data, out T result)
        {
            return UnifiedSerializationService.TryDeserializeWithMessagePack(data, out result);
        }

        /// <summary>
        /// 获取序列化后的数据大小（不实际序列化）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要测量的对象</param>
        /// <returns>估计的序列化大小</returns>
        public static int GetSerializedSize<T>(T obj)
        {
            return UnifiedSerializationService.GetSerializedSizeWithMessagePack(obj);
        }
    }
}