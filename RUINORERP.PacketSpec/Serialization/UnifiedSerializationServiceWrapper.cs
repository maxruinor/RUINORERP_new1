using System;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 统一序列化服务包装器
    /// 为静态UnifiedSerializationService类提供可注入的实例包装
    /// </summary>
    public class UnifiedSerializationServiceWrapper : IUnifiedSerializationService
    {
        #region MessagePack序列化方法
        
        /// <summary>
        /// 序列化对象为字节数组（MessagePack）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        public byte[] SerializeWithMessagePack<T>(T obj)
        {
            return UnifiedSerializationService.SerializeWithMessagePack(obj);
        }

        /// <summary>
        /// 异步序列化对象为字节数组（MessagePack）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        public async Task<byte[]> SerializeWithMessagePackAsync<T>(T obj)
        {
            return await UnifiedSerializationService.SerializeWithMessagePackAsync(obj);
        }

        /// <summary>
        /// 反序列化字节数组为对象（MessagePack）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public T DeserializeWithMessagePack<T>(byte[] data)
        {
            return UnifiedSerializationService.DeserializeWithMessagePack<T>(data);
        }

        /// <summary>
        /// 异步反序列化字节数组为对象（MessagePack）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public async Task<T> DeserializeWithMessagePackAsync<T>(byte[] data)
        {
            return await UnifiedSerializationService.DeserializeWithMessagePackAsync<T>(data);
        }

        /// <summary>
        /// 尝试反序列化字节数组为对象（MessagePack）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <param name="result">反序列化后的对象</param>
        /// <returns>是否反序列化成功</returns>
        public bool TryDeserializeWithMessagePack<T>(byte[] data, out T result)
        {
            return UnifiedSerializationService.TryDeserializeWithMessagePack(data, out result);
        }

        /// <summary>
        /// 安全反序列化（MessagePack，捕获异常返回默认值）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public T SafeDeserializeWithMessagePack<T>(byte[] data)
        {
            return UnifiedSerializationService.SafeDeserializeWithMessagePack<T>(data);
        }

        /// <summary>
        /// 获取序列化后的数据大小（MessagePack，不实际序列化）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要测量的对象</param>
        /// <returns>估计的序列化大小</returns>
        public int GetSerializedSizeWithMessagePack<T>(T obj)
        {
            return UnifiedSerializationService.GetSerializedSizeWithMessagePack(obj);
        }
        
        #endregion

        #region JSON序列化方法
        
        /// <summary>
        /// 序列化对象到字节数组（JSON）
        /// </summary>
        public byte[] SerializeWithJson(object obj)
        {
            return UnifiedSerializationService.SerializeWithJson(obj);
        }

        /// <summary>
        /// 从字节数组反序列化对象（JSON）
        /// </summary>
        public T DeserializeWithJson<T>(byte[] data) where T : class
        {
            return UnifiedSerializationService.DeserializeWithJson<T>(data);
        }

        /// <summary>
        /// 安全反序列化（JSON，捕获异常返回默认值）
        /// </summary>
        public T SafeDeserializeWithJson<T>(byte[] data) where T : class
        {
            return UnifiedSerializationService.SafeDeserializeWithJson<T>(data);
        }

        /// <summary>
        /// 验证字节数组是否可以反序列化（JSON）
        /// </summary>
        public bool CanDeserializeWithJson(byte[] data)
        {
            return UnifiedSerializationService.CanDeserializeWithJson(data);
        }
        
        #endregion

        #region 二进制序列化方法
        
        /// <summary>
        /// 序列化数据包到二进制格式（用于网络传输）
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>二进制数据</returns>
        public byte[] SerializeToBinary<T>(T packet) where T : class
        {
            return UnifiedSerializationService.SerializeToBinary(packet);
        }

        /// <summary>
        /// 从二进制格式反序列化数据包
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>反序列化后的对象</returns>
        public T DeserializeFromBinary<T>(byte[] data) where T : class
        {
            return UnifiedSerializationService.DeserializeFromBinary<T>(data);
        }

        /// <summary>
        /// 安全反序列化（二进制，捕获异常返回默认值）
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>反序列化后的对象或null</returns>
        public T SafeDeserializeFromBinary<T>(byte[] data) where T : class
        {
            return UnifiedSerializationService.SafeDeserializeFromBinary<T>(data);
        }
        
        #endregion

        #region 压缩序列化方法
        
        /// <summary>
        /// 压缩序列化（使用GZip）
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="useMessagePack">是否使用MessagePack序列化，默认为true</param>
        /// <returns>压缩后的字节数组</returns>
        public byte[] SerializeCompressed<T>(T obj, bool useMessagePack = true)
        {
            return UnifiedSerializationService.SerializeCompressed(obj, useMessagePack);
        }

        /// <summary>
        /// 解压缩反序列化
        /// </summary>
        /// <param name="compressedData">压缩的字节数组</param>
        /// <param name="useMessagePack">是否使用MessagePack反序列化，默认为true</param>
        /// <returns>反序列化后的对象</returns>
        public T DeserializeCompressed<T>(byte[] compressedData, bool useMessagePack = true) where T : class
        {
            return UnifiedSerializationService.DeserializeCompressed<T>(compressedData, useMessagePack);
        }

        /// <summary>
        /// 安全解压缩反序列化（捕获异常返回默认值）
        /// </summary>
        /// <param name="compressedData">压缩的字节数组</param>
        /// <param name="useMessagePack">是否使用MessagePack反序列化，默认为true</param>
        /// <returns>反序列化后的对象或null</returns>
        public T SafeDeserializeCompressed<T>(byte[] compressedData, bool useMessagePack = true) where T : class
        {
            return UnifiedSerializationService.SafeDeserializeCompressed<T>(compressedData, useMessagePack);
        }
        
        #endregion

        #region 辅助方法
        
        /// <summary>
        /// 压缩数据
        /// </summary>
        public byte[] Compress(byte[] data)
        {
            return UnifiedSerializationService.Compress(data);
        }

        /// <summary>
        /// 解压缩数据
        /// </summary>
        public byte[] Decompress(byte[] compressedData)
        {
            return UnifiedSerializationService.Decompress(compressedData);
        }
        
        #endregion
    }
}