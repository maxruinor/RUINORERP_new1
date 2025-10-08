using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MessagePack;
using MessagePack.Resolvers;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 统一序列化服务 - 整合所有序列化功能
    /// 支持JSON、MessagePack、二进制和压缩序列化
    /// </summary>
    public static class UnifiedSerializationService
    {
        #region MessagePack配置
        
        // 配置MessagePack序列化选项
        private static readonly MessagePackSerializerOptions _messagePackOptions;
        
        /// <summary>
        /// 获取MessagePack序列化选项（供外部使用）
        /// </summary>
        public static MessagePackSerializerOptions MessagePackOptions => _messagePackOptions;
        
        // 静态构造函数，用于初始化MessagePack选项
        static UnifiedSerializationService()
        {
            try
            {
                _messagePackOptions = MessagePackSerializerOptions.Standard
                    .WithResolver(ContractlessStandardResolver.Instance)
                    .WithCompression(MessagePackCompression.Lz4Block);
            }
            catch (Exception ex)
            {
                // 记录初始化异常，但不抛出，避免类型初始化器异常
                System.Diagnostics.Debug.WriteLine($"UnifiedSerializationService初始化失败: {ex}");
                throw new InvalidOperationException("MessagePack序列化服务初始化失败", ex);
            }
        }
        
        #endregion
        
        #region JSON配置
        
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            DefaultValueHandling = DefaultValueHandling.Include,
            TypeNameHandling = TypeNameHandling.Auto
        };
        
        #endregion

        #region 基础MessagePack序列化方法
        
        /// <summary>
        /// 序列化对象为字节数组（MessagePack）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] SerializeWithMessagePack<T>(T obj)
        {
            try
            {
                return MessagePackSerializer.Serialize(obj, _messagePackOptions);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"MessagePack序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 异步序列化对象为字节数组（MessagePack）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        public static async Task<byte[]> SerializeWithMessagePackAsync<T>(T obj)
        {
            try
            {
                using var stream = new MemoryStream();
                await MessagePackSerializer.SerializeAsync(stream, obj, _messagePackOptions);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new SerializationException($"MessagePack异步序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 反序列化字节数组为对象（MessagePack）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public static T DeserializeWithMessagePack<T>(byte[] data)
        {
            if (data == null || data.Length == 0)
                return default(T);

            try
            {
                return MessagePackSerializer.Deserialize<T>(data, _messagePackOptions);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"MessagePack反序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 异步反序列化字节数组为对象（MessagePack）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public static async Task<T> DeserializeWithMessagePackAsync<T>(byte[] data)
        {
            if (data == null || data.Length == 0)
                return default(T);

            try
            {
                using var stream = new MemoryStream(data);
                return await MessagePackSerializer.DeserializeAsync<T>(stream, _messagePackOptions);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"MessagePack异步反序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 尝试反序列化字节数组为对象（MessagePack）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <param name="result">反序列化后的对象</param>
        /// <returns>是否反序列化成功</returns>
        public static bool TryDeserializeWithMessagePack<T>(byte[] data, out T result)
        {
            try
            {
                result = DeserializeWithMessagePack<T>(data);
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }

        /// <summary>
        /// 安全反序列化（MessagePack，捕获异常返回默认值）
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public static T SafeDeserializeWithMessagePack<T>(byte[] data)
        {
            try
            {
                return DeserializeWithMessagePack<T>(data);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取序列化后的数据大小（MessagePack，不实际序列化）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要测量的对象</param>
        /// <returns>估计的序列化大小</returns>
        public static int GetSerializedSizeWithMessagePack<T>(T obj)
        {
            try
            {
                // 序列化一次来获取大小
                var data = SerializeWithMessagePack(obj);
                return data?.Length ?? 0;
            }
            catch
            {
                return 0;
            }
        }
        
        #endregion

        #region JSON序列化方法
        
        /// <summary>
        /// 序列化对象到字节数组（JSON）
        /// </summary>
        public static byte[] SerializeWithJson(object obj)
        {
            if (obj == null)
                return Array.Empty<byte>();

            try
            {
                string json = JsonConvert.SerializeObject(obj, _jsonSettings);
                return Encoding.UTF8.GetBytes(json);
            }
            catch (Exception ex)
            {
                throw new SerializationException("JSON序列化失败", ex);
            }
        }

        /// <summary>
        /// 从字节数组反序列化对象（JSON）
        /// </summary>
        public static T DeserializeWithJson<T>(byte[] data) where T : class
        {
            if (data == null || data.Length == 0)
                return default;

            try
            {
                string json = Encoding.UTF8.GetString(data);
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (Exception ex)
            {
                throw new SerializationException("JSON反序列化失败", ex);
            }
        }

        /// <summary>
        /// 安全反序列化（JSON，捕获异常返回默认值）
        /// </summary>
        public static T SafeDeserializeWithJson<T>(byte[] data) where T : class
        {
            try
            {
                return DeserializeWithJson<T>(data);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 验证字节数组是否可以反序列化（JSON）
        /// </summary>
        public static bool CanDeserializeWithJson(byte[] data)
        {
            if (data == null || data.Length < 10) // 最小长度检查
                return false;

            try
            {
                string json = Encoding.UTF8.GetString(data);
                JsonConvert.DeserializeObject(json);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        #endregion

        #region 二进制序列化方法
        
        /// <summary>
        /// 序列化数据包到二进制格式（用于网络传输）
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>二进制数据</returns>
        public static byte[] SerializeToBinary<T>(T packet) where T : class
        {
            if (packet == null)
                return Array.Empty<byte>();

            try
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    // 使用MessagePack序列化对象到二进制
                    var data = SerializeWithMessagePack(packet);
                    writer.Write(data.Length);
                    writer.Write(data);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException($"二进制序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从二进制格式反序列化数据包
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>反序列化后的对象</returns>
        public static T DeserializeFromBinary<T>(byte[] data) where T : class
        {
            if (data == null || data.Length < 4) // 最小长度：4字节长度信息
                return null;

            try
            {
                using (var stream = new MemoryStream(data))
                using (var reader = new BinaryReader(stream))
                {
                    // 读取数据长度
                    var length = reader.ReadInt32();
                    
                    // 验证数据长度
                    if (data.Length < 4 + length)
                        return null;

                    // 读取实际数据
                    var packetData = reader.ReadBytes(length);
                    
                    // 使用MessagePack反序列化
                    return DeserializeWithMessagePack<T>(packetData);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException($"二进制反序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 安全反序列化（二进制，捕获异常返回默认值）
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>反序列化后的对象或null</returns>
        public static T SafeDeserializeFromBinary<T>(byte[] data) where T : class
        {
            try
            {
                return DeserializeFromBinary<T>(data);
            }
            catch
            {
                return null;
            }
        }
        
        #endregion

        #region 压缩序列化方法
        
        /// <summary>
        /// 压缩序列化（使用GZip）
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="useMessagePack">是否使用MessagePack序列化，默认为true</param>
        /// <returns>压缩后的字节数组</returns>
        public static byte[] SerializeCompressed<T>(T obj, bool useMessagePack = true)
        {
            if (obj == null)
                return Array.Empty<byte>();

            try
            {
                byte[] data = useMessagePack ? 
                    SerializeWithMessagePack(obj) : 
                    SerializeWithJson(obj);
                return Compress(data);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"压缩序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 解压缩反序列化
        /// </summary>
        /// <param name="compressedData">压缩的字节数组</param>
        /// <param name="useMessagePack">是否使用MessagePack反序列化，默认为true</param>
        /// <returns>反序列化后的对象</returns>
        public static T DeserializeCompressed<T>(byte[] compressedData, bool useMessagePack = true) where T : class
        {
            if (compressedData == null || compressedData.Length == 0)
                return null;

            try
            {
                var data = Decompress(compressedData);
                return useMessagePack ? 
                    DeserializeWithMessagePack<T>(data) : 
                    DeserializeWithJson<T>(data);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"解压缩反序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 安全解压缩反序列化（捕获异常返回默认值）
        /// </summary>
        /// <param name="compressedData">压缩的字节数组</param>
        /// <param name="useMessagePack">是否使用MessagePack反序列化，默认为true</param>
        /// <returns>反序列化后的对象或null</returns>
        public static T SafeDeserializeCompressed<T>(byte[] compressedData, bool useMessagePack = true) where T : class
        {
            try
            {
                return DeserializeCompressed<T>(compressedData, useMessagePack);
            }
            catch
            {
                return null;
            }
        }
        
        #endregion

        #region 辅助方法
        
        /// <summary>
        /// 压缩数据
        /// </summary>
        public static byte[] Compress(byte[] data)
        {
            if (data == null || data.Length == 0)
                return Array.Empty<byte>();

            using (var output = new MemoryStream())
            {
                using (var gzip = new System.IO.Compression.GZipStream(output, System.IO.Compression.CompressionMode.Compress))
                {
                    gzip.Write(data, 0, data.Length);
                }
                return output.ToArray();
            }
        }

        /// <summary>
        /// 解压缩数据
        /// </summary>
        public static byte[] Decompress(byte[] compressedData)
        {
            if (compressedData == null || compressedData.Length == 0)
                return Array.Empty<byte>();

            using (var input = new MemoryStream(compressedData))
            using (var gzip = new System.IO.Compression.GZipStream(input, System.IO.Compression.CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gzip.CopyTo(output);
                return output.ToArray();
            }
        }
        
        #endregion
    }

    /// <summary>
    /// 序列化异常
    /// </summary>
    public class SerializationException : Exception
    {
        public SerializationException(string message) : base(message)
        {
        }

        public SerializationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}