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
    /// 自定义JSON序列化契约解析器
    /// 用于排除服务端特有且可能导致并发问题的属性
    /// </summary>
    internal class ExcludingPropertiesContractResolver : DefaultContractResolver
    {
        private readonly HashSet<string> _excludedProperties = new HashSet<string>
        {
            "UserModList",
            "UserButtonList",
            "UserFieldList",
            "UserMenuList",
            "UserInfo",
            "tb_Employee",
            "DataQueue"
        };

        private readonly HashSet<string> _excludedTypes = new HashSet<string>
        {
            "CurrentUserInfo",
            "SessionInfo",
            "tb_UserInfo",
            "tb_Employee"
        };

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            
            string typeName = type.Name;
            
            if (_excludedTypes.Contains(typeName))
            {
                properties.Clear();
            }
            
            foreach (var prop in properties)
            {
                if (_excludedProperties.Contains(prop.PropertyName))
                {
                    prop.Ignored = true;
                }
            }
            
            return properties;
        }
    }

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
                // 特殊处理匿名类型：当检测到匿名类型时，返回字典类型作为替代
                if (typeName.Contains("<>f__AnonymousType"))
                {
                    // 使用Dictionary<string, object>作为匿名类型的替代类型
                    // 这样可以确保即使客户端无法识别服务器端的匿名类型，也能正常反序列化数据
                    return typeof(Dictionary<string, object>);
                }

                // 处理各种集合类型 - 解决.NET 8和.NET 4.8之间的序列化差异
                var collectionTypeNames = new[]
                {
                    "List`1", "Array", "ICollection`1", "IList`1", "IEnumerable`1",
                    "ReadOnlyCollection`1", "ObservableCollection`1", "HashSet`1",
                    "LinkedList`1", "Queue`1", "Stack`1", "SortedSet`1"
                };

                foreach (var collectionTypeName in collectionTypeNames)
                {
                    if (typeName.Contains(collectionTypeName))
                    {
                        // 对于所有集合类型，统一返回List<object>作为安全的默认实现
                        // 这样可以处理包含$values字段的.NET 8集合序列化格式
                        return typeof(List<>).MakeGenericType(typeof(object));
                    }
                }

                // 先尝试直接加载类型
                var type = Type.GetType($"{typeName}, {assemblyName}");
                if (type != null)
                    return type;

                // 如果直接加载失败，尝试加载常用类型（如字典）
                if (typeName.Contains("Dictionary`2"))
                {
                    // 创建通用字典类型
                    // 注意：不能使用typeof(dynamic)，因为dynamic是编译时概念而不是运行时类型
                    // PacketModel.Extensions字段已定义为JObject类型，Newtonsoft.Json会自动处理类型转换
                    return typeof(Dictionary<,>).MakeGenericType(
                        typeof(string), typeof(object));
                }

                // 对于其他无法识别的类型，返回字典类型以确保程序不会崩溃
                return typeof(Dictionary<string, object>);
            }
            catch (Exception ex)
            {
                // 记录异常但不抛出，返回字典类型作为安全回退
                // 这样即使类型解析失败，程序也能继续运行
                return typeof(Dictionary<string, object>);
            }
        }
    }

    public static class JsonCompressionSerializationService
    {
        // P1-2优化: 压缩阈值常量，小于1KB不压缩
        private const int COMPRESSION_THRESHOLD_BYTES = 1024;

        private static readonly ExcludingPropertiesContractResolver _contractResolver = new ExcludingPropertiesContractResolver();

        /// <summary>
        /// 创建线程安全的JSON序列化设置
        /// 每次调用都创建新实例以避免多线程并发问题
        /// </summary>
        private static JsonSerializerSettings CreateThreadSafeJsonSettings()
        {
            return new JsonSerializerSettings
            {
                // 空值处理：序列化时忽略对象中值为null的属性，减少数据传输量
                NullValueHandling = NullValueHandling.Ignore,
                
                // 格式化设置：设置为None表示不进行美化，生成紧凑的JSON字符串，节省空间
                Formatting = Formatting.None,
                
                // 类型名称处理：自动处理多态类型，在需要区分继承类时自动添加类型信息
                TypeNameHandling = TypeNameHandling.Auto, 
                
                // 启用自定义序列化绑定器，解决跨平台程序集名称差异问题
                SerializationBinder = new CrossPlatformSerializationBinder(),
                
                // 使用自定义契约解析器，排除服务端特有属性
                ContractResolver = _contractResolver,
                
                // 日期格式处理：使用ISO标准格式处理日期时间字符串，确保跨平台兼容性
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                
                // 日期时间时区处理：设置为RoundtripKind以确保保持时间的原始状态
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                
                // 引用循环处理：忽略对象之间的循环引用，避免序列化时出现StackOverflow异常
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        /// <summary>
        /// 序列化并压缩（智能压缩策略）
        /// P1-2优化: 小于1KB的数据包不压缩，避免压缩开销
        /// </summary>
        public static byte[] Serialize<T>(T obj, bool compress = true)
        {
            if (obj == null)
                return Array.Empty<byte>();

            try
            {
                // 使用线程安全的JSON设置实例
                var jsonSettings = CreateThreadSafeJsonSettings();
                string json = JsonConvert.SerializeObject(obj, jsonSettings);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

                // P1-2优化: 只有超过1KB且启用压缩时才执行GZip
                if (compress && jsonBytes.Length > COMPRESSION_THRESHOLD_BYTES)
                {
                    return GZipCompress(jsonBytes);
                }

                // 小包直接返回，避免压缩开销
                return jsonBytes;
            }
            catch (Exception ex)
            {
                throw new SerializationException($"JSON序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 解压并反序列化（主要方法）
        /// P1-2优化: 添加GZip魔数自动检测，智能判断是否需要解压
        /// </summary>
        public static T Deserialize<T>(byte[] data, bool decompress = true)
        {
            if (data == null || data.Length == 0)
                return default(T);

            try
            {
                // P1-2优化: 自动检测GZip压缩格式（魔数 0x1F 0x8B）
                byte[] jsonBytes = ShouldDecompress(data, decompress) ? GZipDecompress(data) : data;
                string json = Encoding.UTF8.GetString(jsonBytes);

                // 使用线程安全的JSON设置实例
                var jsonSettings = CreateThreadSafeJsonSettings();
                return JsonConvert.DeserializeObject<T>(json, jsonSettings);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"JSON反序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 动态反序列化（不知道具体类型时使用）
        /// P1-2优化: 添加GZip魔数自动检测
        /// </summary>
        public static object DeserializeDynamic(byte[] data, bool decompress = true)
        {
            if (data == null || data.Length == 0)
                return null;

            try
            {
                // P1-2优化: 自动检测GZip压缩格式
                byte[] jsonBytes = ShouldDecompress(data, decompress) ? GZipDecompress(data) : data;
                string json = Encoding.UTF8.GetString(jsonBytes);

                // 使用线程安全的JSON设置实例
                var jsonSettings = CreateThreadSafeJsonSettings();
                return JsonConvert.DeserializeObject(json, jsonSettings);
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

        /// <summary>
        /// P1-2优化: 判断是否需要解压（检测GZip魔数）
        /// GZip格式以 0x1F 0x8B 开头
        /// </summary>
        private static bool ShouldDecompress(byte[] data, bool decompressFlag)
        {
            // 如果调用方明确指定不解压，直接返回
            if (!decompressFlag)
                return false;

            // 检查GZip魔数（至少需要2个字节）
            if (data.Length < 2)
                return false;

            // GZip魔数: 0x1F 0x8B
            return data[0] == 0x1F && data[1] == 0x8B;
        }
        #endregion
    }
}
