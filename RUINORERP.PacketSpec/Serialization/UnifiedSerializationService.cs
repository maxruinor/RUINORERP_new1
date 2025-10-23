using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MessagePack;
using MessagePack.Resolvers;
using System.Runtime.Serialization;
using System.Reflection;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 统一序列化服务 - 简化版本，专注于核心功能
    /// 支持JSON、MessagePack序列化，自动忽略null值
    /// </summary>
    public static class UnifiedSerializationService
    {
        #region MessagePack配置

        private static readonly MessagePackSerializerOptions _messagePackOptions;

        static UnifiedSerializationService()
        {
            try
            {
                // 增强的解析器配置，支持[Key]属性和接口序列化
                var resolver = CompositeResolver.Create(

                    

                                                           // 原生解析器，支持[Key]特性
                    NativeDateTimeResolver.Instance,
                    // 优先使用基于属性的解析器，支持[Key]属性
                    AttributeFormatterResolver.Instance, // 支持[MessagePack.Key]和[DataMember]属性
                                                         // 支持接口类型的序列化
                                                         //TypelessObjectResolver.Instance, // 保留完整类型信息，支持接口和抽象类
                     ContractlessStandardResolver.Instance, // 支持无属性标记的类

                    // 标准解析器作为后备
                    StandardResolver.Instance
                                        // 动态泛型解析器，支持接口和抽象类
                                        //  DynamicGenericResolver.Instance
                                        // 类型less解析器，支持动态类型
                                        //TypelessObjectResolver.Instance

                );

                // 配置选项，确保支持接口和[Key]属性
                _messagePackOptions = MessagePackSerializerOptions.Standard
                    .WithResolver(resolver)
                    .WithCompression(MessagePackCompression.Lz4Block)
                    .WithSecurity(MessagePackSecurity.UntrustedData)
                    .WithAllowAssemblyVersionMismatch(true) // 重新添加此选项以支持不同版本程序集间的序列化
                    .WithOmitAssemblyVersion(true); // 允许跨版本序列化
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UnifiedSerializationService初始化失败: {ex}");
                throw new InvalidOperationException("MessagePack序列化服务初始化失败", ex);
            }
        }

        #endregion

        #region JSON配置

        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore, // 这里已经配置了忽略null值
            Formatting = Formatting.None,
            DefaultValueHandling = DefaultValueHandling.Include,
            TypeNameHandling = TypeNameHandling.None,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        #endregion

        #region 核心序列化方法（推荐使用）

        /// <summary>
        /// 【核心方法】序列化对象为字节数组（MessagePack）- 支持[Key]属性和接口类型
        /// 这是最常用的序列化方法
        /// </summary>
        /// <typeparam name="T">对象类型，支持接口类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组</returns>
        /// <remarks>
        /// 支持带有[MessagePack.Key]属性标记的类，允许自定义字段顺序和名称映射。
        /// 也支持接口类型的对象序列化，会保留实际类型信息。
        /// </remarks>
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
        /// 【核心方法】反序列化字节数组为对象（MessagePack）- 支持[Key]属性和接口类型
        /// 这是最常用的反序列化方法
        /// </summary>
        /// <typeparam name="T">目标对象类型，支持接口类型</typeparam>
        /// <param name="data">要反序列化的字节数组</param>
        /// <returns>反序列化后的对象</returns>
        /// <remarks>
        /// 支持带有[MessagePack.Key]属性标记的类，能正确映射自定义字段顺序和名称。
        /// 也支持接口类型的对象反序列化，会创建正确的实现类实例。
        /// </remarks>
        public static T DeserializeWithMessagePack<T>(byte[] data)
        {
            if (data == null || data.Length == 0)
                return default(T);

            try
            {
                // 简化反序列化调用，确保与.NET Framework兼容
                return MessagePackSerializer.Deserialize<T>(data, _messagePackOptions);
            }
            catch (Exception ex)
            {
                // 添加更详细的错误信息，方便调试
                string detailedError = $"MessagePack反序列化失败: 类型={typeof(T).FullName}, 数据长度={data.Length}, 错误={ex.Message}";
                throw new SerializationException(detailedError, ex);
            }
        }



        #endregion

        #region 带类型信息的序列化（用于动态类型）

        /// <summary>
        /// 安全动态序列化 - 专门处理动态类型和接口类型
        /// </summary>
        public static byte[] SafeSerializeDynamic(object obj)
        {
            if (obj == null)
                return Array.Empty<byte>();

            try
            {
                // 使用 Contractless 模式进行序列化
                var options = MessagePackSerializerOptions.Standard
                    .WithResolver(ContractlessStandardResolver.Instance)
                    .WithCompression(MessagePackCompression.Lz4Block);

                // 获取实际类型
                var type = obj.GetType();

                // 使用反射调用序列化方法
                var method = typeof(MessagePackSerializer).GetMethod("Serialize", new[] { typeof(object), typeof(MessagePackSerializerOptions) });
                return (byte[])method.Invoke(null, new object[] { obj, options });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"动态序列化失败，降级到JSON: {ex.Message}");

                // 降级到 JSON
                try
                {
                    return SerializeWithJson(obj);
                }
                catch
                {
                    return Array.Empty<byte>();
                }
            }
        }

        /// <summary>
        /// 专门处理列表类型的序列化
        /// </summary>
        public static byte[] SafeSerializeList(IEnumerable<object> list)
        {
            if (list == null || !list.Any())
                return Array.Empty<byte>();

            try
            {
                // 转换为具体类型的列表
                var elementType = GetCommonElementType(list);
                if (elementType != null)
                {
                    // 创建具体类型的列表
                    var concreteListType = typeof(List<>).MakeGenericType(elementType);
                    var concreteList = Activator.CreateInstance(concreteListType);

                    var addMethod = concreteListType.GetMethod("Add");
                    foreach (var item in list)
                    {
                        addMethod.Invoke(concreteList, new[] { item });
                    }

                    return SafeSerializeDynamic(concreteList);
                }

                // 如果无法确定具体类型，直接序列化
                return SafeSerializeDynamic(list.ToList());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"列表序列化失败: {ex.Message}");
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// 获取列表中元素的公共类型
        /// </summary>
        private static Type GetCommonElementType(IEnumerable<object> list)
        {
            if (list == null || !list.Any())
                return null;

            var firstType = list.First().GetType();

            // 检查所有元素是否都是相同类型
            if (list.All(item => item.GetType() == firstType))
                return firstType;

            // 如果类型不同，查找公共基类
            var types = list.Select(item => item.GetType()).Distinct().ToList();
            if (types.Count == 1)
                return types[0];

            // 查找公共基类或接口
            var commonType = types[0];
            foreach (var type in types.Skip(1))
            {
                while (commonType != null && !commonType.IsAssignableFrom(type))
                {
                    commonType = commonType.BaseType;
                }

                if (commonType == null || commonType == typeof(object))
                    break;
            }

            return commonType != typeof(object) ? commonType : null;
        }
        #endregion



        #region JSON序列化方法

        /// <summary>
        /// 序列化对象为JSON字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>JSON字符串</returns>
        public static string SerializeWithJson<T>(T obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, _jsonSettings);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"JSON序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 反序列化JSON字符串为对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T DeserializeWithJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);

            try
            {
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (Exception ex)
            {
                throw new SerializationException($"JSON反序列化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 序列化对象到字节数组（JSON）- 自动忽略null值
        /// 使用场景：需要人类可读格式或与其他系统交互
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

        #endregion



        #region 安全版本（不抛出异常）

        /// <summary>
        /// 安全序列化（捕获异常返回空数组）
        /// </summary>
        public static byte[] SafeSerializeWithMessagePack<T>(T obj)
        {
            try
            {
                return SerializeWithMessagePack(obj);
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// 安全反序列化（捕获异常返回默认值）
        /// </summary>
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

        #endregion

        #region 辅助方法

        /// <summary>
        /// 验证数据是否可以反序列化为指定类型
        /// </summary>
        public static bool CanDeserialize<T>(byte[] data)
        {
            try
            {
                var result = DeserializeWithMessagePack<T>(data);
                return result != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取序列化后的大小估计
        /// </summary>
        public static int GetSerializedSize<T>(T obj)
        {
            try
            {
                var data = SerializeWithMessagePack(obj);
                return data?.Length ?? 0;
            }
            catch
            {
                return 0;
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
