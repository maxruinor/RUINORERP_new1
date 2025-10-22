using MessagePack;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 统一缓存数据转换器 - 支持多种数据格式的转换
    /// 为统一的缓存请求响应模型提供数据转换支持
    /// </summary>
    public static class CacheDataConverter
    {
        /// <summary>
        /// 将对象转换为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>转换后的对象</returns>
        public static T ConvertToType<T>(object source)
        {
            if (source == null) return default;

            // 如果源对象已经是目标类型，直接返回
            if (source is T typedSource)
                return typedSource;

            try
            {
                // 优先使用MessagePack进行转换（性能更好）
                var bytes = MessagePackSerializer.Serialize(source);
                return MessagePackSerializer.Deserialize<T>(bytes);
            }
            catch
            {
                try
                {
                    // 使用JSON进行转换（兼容性更好）
                    var json = JsonConvert.SerializeObject(source);
                    return JsonConvert.DeserializeObject<T>(json);
                }
                catch (Exception jsonEx)
                {
                    throw new InvalidOperationException($"无法将对象转换为类型 {typeof(T).Name}", jsonEx);
                }
            }
        }

        /// <summary>
        /// 将对象转换为指定类型的列表
        /// </summary>
        /// <typeparam name="T">目标元素类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>转换后的列表</returns>
        public static List<T> ConvertToList<T>(object source)
        {
            if (source == null) return new List<T>();

            // 如果源对象已经是目标类型列表，直接返回
            if (source is List<T> typedList)
                return typedList;

            // 如果源对象是可枚举的，转换为列表
            if (source is IEnumerable<T> enumerable)
                return enumerable.ToList();

            try
            {
                // 处理JArray类型
                if (source is JArray jArray)
                    return jArray.ToObject<List<T>>();

                // 处理IList类型
                if (source is IList list)
                {
                    var result = new List<T>();
                    foreach (var item in list)
                    {
                        result.Add(ConvertToType<T>(item));
                    }
                    return result;
                }

                // 最后尝试JSON序列化方式
                var json = JsonConvert.SerializeObject(source);
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"转换缓存数据到类型 List<{typeof(T).Name}> 失败", ex);
            }
        }

        /// <summary>
        /// 将对象转换为字典
        /// </summary>
        /// <param name="source">源对象</param>
        /// <returns>转换后的字典</returns>
        public static Dictionary<string, object> ConvertToDictionary(object source)
        {
            if (source == null) return new Dictionary<string, object>();

            // 如果源对象已经是字典类型，直接返回
            if (source is Dictionary<string, object> dict)
                return dict;

            try
            {
                // 使用JSON序列化转换
                var json = JsonConvert.SerializeObject(source);
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("无法将对象转换为字典类型", ex);
            }
        }

        /// <summary>
        /// 序列化对象为字节数组
        /// </summary>
        /// <param name="source">源对象</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] SerializeToBytes(object source)
        {
            if (source == null) return null;

            try
            {
                return MessagePackSerializer.Serialize(source);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("无法序列化对象为字节数组", ex);
            }
        }

        /// <summary>
        /// 从字节数组反序列化对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="bytes">字节数组</param>
        /// <returns>反序列化后的对象</returns>
        public static T DeserializeFromBytes<T>(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return default;

            try
            {
                return MessagePackSerializer.Deserialize<T>(bytes);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("无法从字节数组反序列化对象", ex);
            }
        }

        /// <summary>
        /// 克隆对象（深拷贝）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>克隆后的对象</returns>
        public static T Clone<T>(T source)
        {
            if (source == null) return default;

            try
            {
                // 使用MessagePack进行深拷贝
                var bytes = MessagePackSerializer.Serialize(source);
                return MessagePackSerializer.Deserialize<T>(bytes);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"无法克隆对象类型 {typeof(T).Name}", ex);
            }
        }
    }
}
