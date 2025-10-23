using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 自定义序列化绑定器，用于解决.NET Framework和.NET Core之间的程序集名称差异
    /// 特别是处理System.Private.CoreLib和mscorlib之间的映射问题
    /// </summary>
    internal class CrossPlatformSerializationBinder : ISerializationBinder
    {
        /// <summary>
        /// 将类型映射到类型名称
        /// </summary>
        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.FullName;
            typeName = serializedType.FullName;
        }

        /// <summary>
        /// 将类型名称映射到类型，处理跨平台程序集差异
        /// </summary>
        public Type BindToType(string assemblyName, string typeName)
        {
            // 处理System.Private.CoreLib和mscorlib之间的映射
            if (assemblyName.Contains("System.Private.CoreLib"))
            {
                // 尝试使用当前运行时可用的程序集
                assemblyName = assemblyName.Replace("System.Private.CoreLib", "mscorlib");
            }

            try
            {
                // 先尝试直接加载类型
                var type = Type.GetType($"{typeName}, {assemblyName}");
                if (type != null)
                    return type;

                // 如果直接加载失败，尝试加载常用类型（如字典）
                if (typeName.Contains("Dictionary`2"))
                {
                    // 创建通用字典类型
                    return typeof(Dictionary<,>).MakeGenericType(
                        typeof(string), typeof(object));
                }

                // 对于其他类型，可以根据需要添加更多特殊处理
                return null;
            }
            catch (Exception)
            {
                // 如果加载失败，尝试返回默认类型或null
                return null;
            }
        }
    }

    public static class JsonCompressionSerializationService
    {
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            // 空值处理：序列化时忽略对象中值为null的属性，减少数据传输量
            NullValueHandling = NullValueHandling.Ignore,
            
            // 格式化设置：设置为None表示不进行美化，生成紧凑的JSON字符串，节省空间
            Formatting = Formatting.None,
            
            // 类型名称处理：自动处理多态类型，在需要区分继承类时自动添加类型信息
            TypeNameHandling = TypeNameHandling.Auto, 
            
            // 添加自定义序列化绑定器，解决跨平台程序集名称不匹配问题
            SerializationBinder = new CrossPlatformSerializationBinder(),
            
            // 日期格式处理：使用ISO标准格式处理日期时间字符串，确保跨平台兼容性
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            
            // 日期时间时区处理：设置为RoundtripKind以确保保持时间的原始状态
            // 重要：此设置确保DateTime对象在序列化和反序列化过程中保持其原始时区信息
            // - 本地时间(L)将保持为本地时间
            // - UTC时间(U)将保持为UTC时间
            // - 未指定时间(K)将保持为未指定
            // 这样可以避免不必要的时区转换，完全按照原始数据格式处理
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            
            // 引用循环处理：忽略对象之间的循环引用，避免序列化时出现StackOverflow异常
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        /// <summary>
        /// 序列化并压缩（主要方法）
        /// </summary>
        public static byte[] Serialize<T>(T obj, bool compress = true)
        {
            if (obj == null)
                return Array.Empty<byte>();

            try
            {
                string json = JsonConvert.SerializeObject(obj, _jsonSettings);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

                return compress ? GZipCompress(jsonBytes) : jsonBytes;
            }
            catch (Exception ex)
            {
                throw new SerializationException($"JSON序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 解压并反序列化（主要方法）
        /// </summary>
        public static T Deserialize<T>(byte[] data, bool decompress = true)
        {
            if (data == null || data.Length == 0)
                return default(T);

            try
            {
                byte[] jsonBytes = decompress ? GZipDecompress(data) : data;
                string json = Encoding.UTF8.GetString(jsonBytes);

                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"JSON反序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 动态反序列化（不知道具体类型时使用）
        /// </summary>
        public static object DeserializeDynamic(byte[] data, bool decompress = true)
        {
            if (data == null || data.Length == 0)
                return null;

            try
            {
                byte[] jsonBytes = decompress ? GZipDecompress(data) : data;
                string json = Encoding.UTF8.GetString(jsonBytes);

                return JsonConvert.DeserializeObject(json, _jsonSettings);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"动态JSON反序列化失败: {ex.Message}", ex);
            }
        }

        #region 压缩方法
        private static byte[] GZipCompress(byte[] data)
        {
            using (var output = new MemoryStream())
            {
                using (var gzip = new GZipStream(output, CompressionMode.Compress))
                {
                    gzip.Write(data, 0, data.Length);
                }
                return output.ToArray();
            }
        }

        private static byte[] GZipDecompress(byte[] compressedData)
        {
            using (var input = new MemoryStream(compressedData))
            using (var gzip = new GZipStream(input, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gzip.CopyTo(output);
                return output.ToArray();
            }
        }
        #endregion
    }
}
