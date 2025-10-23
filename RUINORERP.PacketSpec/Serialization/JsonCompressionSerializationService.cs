using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Text;

namespace RUINORERP.PacketSpec.Serialization
{
    public static class JsonCompressionSerializationService
    {
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            TypeNameHandling = TypeNameHandling.Auto, // 自动处理多态类型
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
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
