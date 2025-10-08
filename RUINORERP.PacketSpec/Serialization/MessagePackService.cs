using MessagePack;
using System;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// MessagePack序列化服务
    /// 提供对UnifiedSerializationService中MessagePack相关功能的简便访问
    /// </summary>
    public static class MessagePackService
    {
        /// <summary>
        /// 序列化对象为字节数组
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] Serialize<T>(T obj) => UnifiedSerializationService.SerializeWithMessagePack(obj);

        /// <summary>
        /// 异步序列化对象为字节数组
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        public static Task<byte[]> SerializeAsync<T>(T obj) => UnifiedSerializationService.SerializeWithMessagePackAsync(obj);

        /// <summary>
        /// 反序列化字节数组为对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public static T Deserialize<T>(byte[] data) => UnifiedSerializationService.DeserializeWithMessagePack<T>(data);

        /// <summary>
        /// 反序列化字节数组为对象（带配置选项）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <param name="options">MessagePack序列化选项</param>
        /// <returns>反序列化后的对象</returns>
        public static T Deserialize<T>(byte[] data, MessagePackSerializerOptions options) => UnifiedSerializationService.DeserializeWithMessagePack<T>(data);

        /// <summary>
        /// 异步反序列化字节数组为对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public static Task<T> DeserializeAsync<T>(byte[] data) => UnifiedSerializationService.DeserializeWithMessagePackAsync<T>(data);

        /// <summary>
        /// 尝试反序列化字节数组为对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <param name="result">反序列化后的对象</param>
        /// <returns>是否反序列化成功</returns>
        public static bool TryDeserialize<T>(byte[] data, out T result) => UnifiedSerializationService.TryDeserializeWithMessagePack(data, out result);

        /// <summary>
        /// 获取序列化后的数据大小
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要测量的对象</param>
        /// <returns>估计的序列化大小</returns>
        public static int GetSerializedSize<T>(T obj) => UnifiedSerializationService.GetSerializedSizeWithMessagePack(obj);
    }
}
