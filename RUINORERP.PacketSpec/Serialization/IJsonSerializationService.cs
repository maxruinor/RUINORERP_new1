using System;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// JSON序列化服务接口
    /// 定义JSON序列化和反序列化的核心方法
    /// </summary>
    public interface IJsonSerializationService
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="compress">是否压缩</param>
        /// <returns>序列化后的字节数组</returns>
        byte[] Serialize<T>(T obj, bool compress = true);

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <param name="decompress">是否解压</param>
        /// <returns>反序列化后的对象</returns>
        T Deserialize<T>(byte[] data, bool decompress = true);

        /// <summary>
        /// 动态反序列化（不知道具体类型时使用）
        /// </summary>
        /// <param name="data">要反序列化的字节数组</param>
        /// <param name="decompress">是否解压</param>
        /// <returns>反序列化后的对象</returns>
        object DeserializeDynamic(byte[] data, bool decompress = true);
    }
}