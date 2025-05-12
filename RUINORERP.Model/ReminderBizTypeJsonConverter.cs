using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RUINORERP.Global.EnumExt;
    using System;

    public class ReminderBizTypeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ReminderBizType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Integer)
            {
                return Enum.Parse(typeof(ReminderBizType), token.ToString());
            }
            if (token.Type == JTokenType.String)
            {
                return Enum.Parse(typeof(ReminderBizType), token.ToString());
            }
            throw new JsonSerializationException($"无法解析枚举类型：{token}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ReminderBizType enumValue)
            {
                writer.WriteValue((int)enumValue);
                return;
            }
            throw new JsonSerializationException("值不是有效的枚举类型");
        }
    }
}
