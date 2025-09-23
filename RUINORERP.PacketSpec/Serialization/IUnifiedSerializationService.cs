using System;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 统一序列化服务接口
    /// 为UnifiedSerializationService提供可注入的抽象
    /// </summary>
    public interface IUnifiedSerializationService
    {
        #region MessagePack序列化方法
        
        /// <summary>
        /// 序列化对象为字节数组（MessagePack）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        byte[] SerializeWithMessagePack<T>(T obj);

        /// <summary>
        /// 异步序列化对象为字节数组（MessagePack）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        Task<byte[]> SerializeWithMessagePackAsync<T>(T obj);

        /// <summary>
        /// 反序列化字节数组为对象（MessagePack）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        T DeserializeWithMessagePack<T>(byte[] data);

        /// <summary>
        /// 异步反序列化字节数组为对象（MessagePack）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        Task<T> DeserializeWithMessagePackAsync<T>(byte[] data);

        /// <summary>
        /// 尝试反序列化字节数组为对象（MessagePack）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <param name="result">反序列化后的对象</param>
        /// <returns>是否反序列化成功</returns>
        bool TryDeserializeWithMessagePack<T>(byte[] data, out T result);

        /// <summary>
        /// 安全反序列化（MessagePack，捕获异常返回默认值）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        T SafeDeserializeWithMessagePack<T>(byte[] data);

        /// <summary>
        /// 获取序列化后的数据大小（MessagePack，不实际序列化）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要测量的对象</param>
        /// <returns>估计的序列化大小</returns>
        int GetSerializedSizeWithMessagePack<T>(T obj);
        
        #endregion

        #region JSON序列化方法
        
        /// <summary>
        /// 序列化对象到字节数组（JSON）
        /// </summary>
        byte[] SerializeWithJson(object obj);

        /// <summary>
        /// 从字节数组反序列化对象（JSON）
        /// </summary>
        T DeserializeWithJson<T>(byte[] data) where T : class;

        /// <summary>
        /// 安全反序列化（JSON，捕获异常返回默认值）
        /// </summary>
        T SafeDeserializeWithJson<T>(byte[] data) where T : class;

        /// <summary>
        /// 验证字节数组是否可以反序列化（JSON）
        /// </summary>
        bool CanDeserializeWithJson(byte[] data);
        
        #endregion

        #region 二进制序列化方法
        
        /// <summary>
        /// 序列化数据包到二进制格式（用于网络传输）
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>二进制数据</returns>
        byte[] SerializeToBinary<T>(T packet) where T : class;

        /// <summary>
        /// 从二进制格式反序列化数据包
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>反序列化后的对象</returns>
        T DeserializeFromBinary<T>(byte[] data) where T : class;

        /// <summary>
        /// 安全反序列化（二进制，捕获异常返回默认值）
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>反序列化后的对象或null</returns>
        T SafeDeserializeFromBinary<T>(byte[] data) where T : class;
        
        #endregion

        #region 压缩序列化方法
        
        /// <summary>
        /// 压缩序列化（使用GZip）
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="useMessagePack">是否使用MessagePack序列化，默认为true</param>
        /// <returns>压缩后的字节数组</returns>
        byte[] SerializeCompressed<T>(T obj, bool useMessagePack = true);

        /// <summary>
        /// 解压缩反序列化
        /// </summary>
        /// <param name="compressedData">压缩的字节数组</param>
        /// <param name="useMessagePack">是否使用MessagePack反序列化，默认为true</param>
        /// <returns>反序列化后的对象</returns>
        T DeserializeCompressed<T>(byte[] compressedData, bool useMessagePack = true) where T : class;

        /// <summary>
        /// 安全解压缩反序列化（捕获异常返回默认值）
        /// </summary>
        /// <param name="compressedData">压缩的字节数组</param>
        /// <param name="useMessagePack">是否使用MessagePack反序列化，默认为true</param>
        /// <returns>反序列化后的对象或null</returns>
        T SafeDeserializeCompressed<T>(byte[] compressedData, bool useMessagePack = true) where T : class;
        
        #endregion

        #region 辅助方法
        
        /// <summary>
        /// 压缩数据
        /// </summary>
        byte[] Compress(byte[] data);

        /// <summary>
        /// 解压缩数据
        /// </summary>
        byte[] Decompress(byte[] compressedData);
        
        #endregion
    }
}