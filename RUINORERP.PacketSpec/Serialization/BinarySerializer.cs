using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 二进制序列化工具（使用System.Text.Json）
    /// </summary>
    public static class BinarySerializer
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

        /// <summary>
        /// 序列化对象到字节数组
        /// </summary>
        public static byte[] Serialize(object obj)
        {
            if (obj == null)
                return Array.Empty<byte>();

            try
            {
                return JsonSerializer.SerializeToUtf8Bytes(obj, _jsonOptions);
            }
            catch (Exception ex)
            {
                throw new SerializationException("序列化失败", ex);
            }
        }

        /// <summary>
        /// 从字节数组反序列化对象
        /// </summary>
        public static T Deserialize<T>(byte[] data) where T : class
        {
            if (data == null || data.Length == 0)
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(data, _jsonOptions);
            }
            catch (Exception ex)
            {
                throw new SerializationException("反序列化失败", ex);
            }
        }

        /// <summary>
        /// 安全反序列化（捕获异常返回默认值）
        /// </summary>
        public static T SafeDeserialize<T>(byte[] data) where T : class
        {
            try
            {
                return Deserialize<T>(data);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 验证字节数组是否可以反序列化
        /// </summary>
        public static bool CanDeserialize(byte[] data)
        {
            if (data == null || data.Length < 10) // 最小长度检查
                return false;

            try
            {
                JsonDocument.Parse(data);
                return true;
            }
            catch
            {
                return false;
            }
        }
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