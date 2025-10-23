using System;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// JSON序列化服务实现
    /// 包装现有的静态JsonCompressionSerializationService，使其可通过依赖注入使用
    /// </summary>
    public class JsonSerializationService : IJsonSerializationService
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="compress">是否压缩</param>
        /// <returns>序列化后的字节数组</returns>
        public byte[] Serialize<T>(T obj, bool compress = true)
        {
            return JsonCompressionSerializationService.Serialize(obj, compress);
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <param name="decompress">是否解压</param>
        /// <returns>反序列化后的对象</returns>
        public T Deserialize<T>(byte[] data, bool decompress = true)
        {
            return JsonCompressionSerializationService.Deserialize<T>(data, decompress);
        }

        /// <summary>
        /// 动态反序列化（不知道具体类型时使用）
        /// </summary>
        /// <param name="data">要反序列化的字节数组</param>
        /// <param name="decompress">是否解压</param>
        /// <returns>反序列化后的对象</returns>
        public object DeserializeDynamic(byte[] data, bool decompress = true)
        {
            return JsonCompressionSerializationService.DeserializeDynamic(data, decompress);
        }
    }
}