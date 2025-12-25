using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AutoUpdateTools
{
    /// <summary>
    /// XML序列化和反序列化辅助类
    /// </summary>
    public static class SerializeXmlHelper
    {
        /// <summary>
        /// 序列化指定类型的对象到指定的Xml文件
        /// </summary>
        /// <typeparam name="T">要序列化的对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="xmlFileName">保存对象数据的完整文件名</param>
        /// <exception cref="ArgumentNullException">当obj为null或xmlFileName为空时抛出</exception>
        /// <exception cref="IOException">当文件操作失败时抛出</exception>
        public static void SerializeXml<T>(T obj, string xmlFileName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "要序列化的对象不能为空");
            }
            
            if (string.IsNullOrEmpty(xmlFileName))
            {
                throw new ArgumentNullException(nameof(xmlFileName), "XML文件名不能为空");
            }
            
            lock (xmlFileName)
            {
                try
                {
                    // 确保目录存在
                    string directory = Path.GetDirectoryName(xmlFileName);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                        System.Diagnostics.Debug.WriteLine($"创建目录: {directory}");
                    }
                    
                    // 序列化对象到XML字符串
                    string xmlContent = SerializeObject<T>(obj);
                    
                    // 写入文件
                    FileHelper.WriteFile(xmlFileName, xmlContent, Encoding.UTF8);
                    System.Diagnostics.Debug.WriteLine($"成功序列化对象到XML文件: {xmlFileName}");
                }
                catch (Exception ex)
                {
                    string errorMessage = $"序列化对象到XML文件失败: {xmlFileName}, 错误: {ex.Message}";
                    System.Diagnostics.Debug.WriteLine(errorMessage);
                    throw new IOException(errorMessage, ex);
                }
            }
        }

        /// <summary>
        /// 把对象序列化为xml字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>XML字符串</returns>
        /// <exception cref="ArgumentNullException">当obj为null时抛出</exception>
        public static string SerializeObject<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "要序列化的对象不能为空");
            }

            try
            {
                using (var stringWriter = new StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(stringWriter, obj);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"序列化对象失败, 类型: {typeof(T).FullName}, 错误: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(errorMessage);
                throw;
            }
        }

        /// <summary>
        /// 从指定的Xml文件中反序列化指定类型的对象
        /// </summary>
        /// <typeparam name="T">反序列化的对象类型</typeparam>
        /// <param name="xmlFileName">保存对象数据的文件名</param>
        /// <returns>返回反序列化出的对象实例</returns>
        /// <exception cref="ArgumentNullException">当xmlFileName为空时抛出</exception>
        /// <exception cref="FileNotFoundException">当xmlFileName指定的文件不存在时抛出</exception>
        /// <exception cref="IOException">当文件操作失败时抛出</exception>
        public static T DeserializeXml<T>(string xmlFileName)
        {
            if (string.IsNullOrEmpty(xmlFileName))
            {
                throw new ArgumentNullException(nameof(xmlFileName), "XML文件名不能为空");
            }
            
            lock (xmlFileName)
            {
                try
                {
                    if (!File.Exists(xmlFileName))
                    {
                        string errorMessage = $"序列化文件不存在: {xmlFileName}";
                        System.Diagnostics.Debug.WriteLine(errorMessage);
                        throw new FileNotFoundException(errorMessage, xmlFileName);
                    }
                    
                    // 读取文件内容
                    string xmlContent = FileHelper.ReadFile(xmlFileName, Encoding.UTF8);
                    
                    // 反序列化为对象
                    return DeserializeObject<T>(xmlContent);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"反序列化XML文件失败: {xmlFileName}, 错误: {ex.Message}";
                    System.Diagnostics.Debug.WriteLine(errorMessage);
                    throw;
                }
            }
        }

        /// <summary>
        /// 把xml字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xmlString">XML字符串</param>
        /// <returns>反序列化的对象</returns>
        /// <exception cref="ArgumentNullException">当xmlString为空时抛出</exception>
        public static T DeserializeObject<T>(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString))
            {
                throw new ArgumentNullException(nameof(xmlString), "XML字符串不能为空");
            }

            try
            {
                using (var stringReader = new StringReader(xmlString))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"反序列化对象失败, 类型: {typeof(T).FullName}, 错误: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(errorMessage);
                throw;
            }
        }
    }
}