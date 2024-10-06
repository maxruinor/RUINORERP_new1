using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Nodes;
using System;

namespace RUINORERP.IIS.Comm
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,                                                               //启用缩进设置
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,    //中文编码
            Converters = { new JsonDatetime() }                                       //日期格式化
        };

        /// <summary>
        /// 对象转Json字符串
        /// </summary>   
        /// <param name="Obj">对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToJson(object Obj)
        {
            try
            {
                return JsonSerializer.Serialize(Obj, options);
            }
            catch (Exception e)
            {
                throw new Exception("JsonHelper: " + e.Message);
            }
        }

        /// <summary>
        /// Json字符串转对象
        /// </summary>
        /// <typeparam name="T">要转换的对象</typeparam>
        /// <param name="Json">Json字符串</param>
        /// <returns>T</returns>
        public static T ToModel<T>(string Json) where T : new()
        {
            try
            {
                return JsonSerializer.Deserialize<T>(Json, options);
            }
            catch (Exception e)
            {
                throw new Exception("JsonHelper: " + e.Message);
            }
        }

        /// <summary>
        /// Json字符串解析，取值方法 var value = data["alipay_trade_refund_response"]["code"]?.ToString()
        /// </summary>
        /// <param name="Json">Json字符串</param>
        /// <returns>JsonNode</returns>
        public static JsonNode Parse(string Json)
        {
            try
            {
                return JsonNode.Parse(Json);
            }
            catch (Exception e)
            {
                throw new Exception("JsonHelper: " + e.Message);
            }
        }

    }
}