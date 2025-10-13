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
    public static class CacheDataConverter
    {
        // 动态类型转换
        public static T ConvertToType<T>(object source)
        {
            if (source == null) return default;

            if (source is T typedSource)
                return typedSource;

            try
            {
                // 使用MessagePack进行转换
                var bytes = MessagePackSerializer.Serialize(source);
                return MessagePackSerializer.Deserialize<T>(bytes);
            }
            catch
            {
                // 使用JSON进行转换
                var json = JsonConvert.SerializeObject(source);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public static List<T> ConvertToList<T>(object source)
        {
            if (source == null) return new List<T>();

            if (source is List<T> typedList)
                return typedList;

            if (source is IEnumerable<T> enumerable)
                return enumerable.ToList();

            try
            {
                if (source is JArray jArray)
                    return jArray.ToObject<List<T>>();

                if (source is IList list)
                {
                    var result = new List<T>();
                    foreach (var item in list)
                    {
                        result.Add(ConvertToType<T>(item));
                    }
                    return result;
                }

                // 最后尝试序列化方式
                var json = JsonConvert.SerializeObject(source);
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"转换缓存数据到类型 {typeof(T).Name} 失败", ex);
            }
        }
    }
}
