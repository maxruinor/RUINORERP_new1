using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{
    public class CustomCollectionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            // 这个转换器只适用于集合
            return typeof(IEnumerable).IsAssignableFrom(objectType);
          
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartArray(); // 开始写入数组

            foreach (var item in (IEnumerable)value)
            {
                writer.WriteStartObject(); // 开始写入对象

                var properties = item.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var attribute = property.GetCustomAttribute<SugarColumn>();
                    if (attribute == null || !attribute.IsIgnore)
                    {
                        // 序列化属性
                        try
                        {
                            // 检查属性值是否为 DateTime 类型
                            if (property.PropertyType == typeof(DateTime))
                            {
                                // 格式化 DateTime 为字符串
                                writer.WritePropertyName(property.Name);
                                writer.WriteValue(((DateTime)property.GetValue(item)).ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            else if (property.PropertyType == typeof(DateTime?)) // 处理可空 DateTime
                            {
                                var dateTime = (DateTime?)property.GetValue(item);
                                writer.WritePropertyName(property.Name);
                                writer.WriteValue(dateTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)null);
                            }
                            else
                            {
                                // 序列化属性
                                writer.WritePropertyName(property.Name);
                                writer.WriteValue(property.GetValue(item));
                            }
                        }
                        catch (Exception ex)
                        {
                            // 如果属性值无法序列化，写入 null 或默认值
                            writer.WritePropertyName(property.Name);
                            writer.WriteNull();
                        }
                    }
                }

                writer.WriteEndObject(); // 结束写入对象
            }

            writer.WriteEndArray(); // 结束写入数组
        }
    }

    public class CustomJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            // 返回true表示这个转换器适用于所有类型
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // 这里不需要实现反序列化逻辑，因为我们只关心序列化
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // 序列化对象开始
            writer.WriteStartObject();

            // 获取对象的类型
            Type type = value.GetType();

            // 遍历对象的所有属性
            foreach (PropertyInfo property in type.GetProperties())
            {
                // 检查属性是否有我们的自定义特性，并且是否标记为忽略
                var attribute = property.GetCustomAttribute<SugarColumn>();
                if (attribute != null && attribute.IsIgnore)
                {
                    // 如果标记为忽略，则跳过这个属性
                    continue;
                }

                // 序列化属性
                serializer.Serialize(writer, property.GetValue(value));
            }

            // 序列化对象结束
            writer.WriteEndObject();
        }
    }
}
