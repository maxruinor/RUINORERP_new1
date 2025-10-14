using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using MessagePack;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 缓存序列化助手类，支持多种序列化方式
    /// </summary>
    public static class CacheSerializationHelper
    {
        /// <summary>
        /// 序列化方式枚举
        /// </summary>
        public enum SerializationType
        {
            Json,
            MessagePack,
            Xml
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="type">序列化方式</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] Serialize<T>(T obj, SerializationType type = SerializationType.Json)
        {
            switch (type)
            {
                case SerializationType.Json:
                    return SerializeJson(obj);
                case SerializationType.MessagePack:
                    return SerializeMessagePack(obj);
                case SerializationType.Xml:
                    return SerializeXml(obj);
                default:
                    return SerializeJson(obj);
            }
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="data">序列化后的字节数组</param>
        /// <param name="type">序列化方式</param>
        /// <returns>反序列化后的对象</returns>
        public static T Deserialize<T>(byte[] data, SerializationType type = SerializationType.Json)
        {
            switch (type)
            {
                case SerializationType.Json:
                    return DeserializeJson<T>(data);
                case SerializationType.MessagePack:
                    return DeserializeMessagePack<T>(data);
                case SerializationType.Xml:
                    return DeserializeXml<T>(data);
                default:
                    return DeserializeJson<T>(data);
            }
        }

        #region JSON序列化
        private static byte[] SerializeJson<T>(T obj)
        {
            var jsonString = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        private static T DeserializeJson<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            
            var jsonString = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        #endregion

        #region MessagePack序列化
        private static byte[] SerializeMessagePack<T>(T obj)
        {
            return MessagePackSerializer.Serialize(obj);
        }

        private static T DeserializeMessagePack<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            
            return MessagePackSerializer.Deserialize<T>(data);
        }
        #endregion

        #region XML序列化
        private static byte[] SerializeXml<T>(T obj)
        {
            // 简化实现，实际项目中可能需要更完善的XML序列化
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        private static T DeserializeXml<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var stream = new MemoryStream(data))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
        #endregion
    }
}