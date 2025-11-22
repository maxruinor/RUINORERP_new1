using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AutoUpdateTools
{
    /// <summary>
    /// XML序列化和反序列化辅助类
    /// </summary>
    class SerializeXmlHelper
    {
        /// <summary>
        /// 序列化指定类型的对象到指定的Xml文件
        /// </summary>
        /// <typeparam name="T">要序列化的对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="xmlFileName">保存对象数据的完整文件名</param>
        public static void SerializeXml<T>(T obj, string xmlFileName)
        {
            if (obj == null || string.IsNullOrEmpty(xmlFileName)) return;
            
            lock (xmlFileName)
            {
                try
                {
                    string dir = Path.GetDirectoryName(xmlFileName);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string xmlContent = SerializeObject<T>(obj);
                    FileHelper.WriteFile(xmlFileName, xmlContent, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"序列化对象到XML文件失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 把对象序列化为xml字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>XML字符串</returns>
        public static string SerializeObject<T>(T obj)
        {
            if (obj == null) return string.Empty;

            try
            {
                using (var strWriter = new StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(strWriter, obj);
                    return strWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"序列化对象失败: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 从指定的Xml文件中反序列化指定类型的对象
        /// </summary>
        /// <typeparam name="T">反序列化的对象类型</typeparam>
        /// <param name="xmlFileName">保存对象数据的文件名</param>
        /// <returns>返回反序列化出的对象实例</returns>
        public static T DeserializeXml<T>(string xmlFileName)
        {
            if (string.IsNullOrEmpty(xmlFileName)) return default(T);
            
            lock (xmlFileName)
            {
                try
                {
                    if (!File.Exists(xmlFileName))
                    {
                        Console.WriteLine("序列化文件不存在!");
                        return default(T);
                    }
                    else
                    {
                        string xmlContent = FileHelper.ReadFile(xmlFileName, Encoding.UTF8);
                        return DeserializeObject<T>(xmlContent);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"反序列化XML文件失败: {ex.Message}");
                    return default(T);
                }
            }
        }

        /// <summary>
        /// 把xml字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xmlString">XML字符串</param>
        /// <returns>反序列化的对象</returns>
        public static T DeserializeObject<T>(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString)) return default(T);

            try
            {
                using (var strReader = new StringReader(xmlString))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(strReader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"反序列化对象失败: {ex.Message}");
                return default(T);
            }
        }
    }
}