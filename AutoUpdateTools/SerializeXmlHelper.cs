using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoUpdateTools
{
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
                lock (xmlFileName)
                {
                    try
                    {
                        string dir = Path.GetDirectoryName(xmlFileName);       //获取文件路径
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        string xmlContent = SerializeObject<T>(obj);
                        FileHelper.WriteFile(xmlFileName, xmlContent, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
            }

            /// <summary>
            /// 把对象序列化为xml字符串
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="obj"></param>
            /// <returns></returns>
            public static string SerializeObject<T>(T obj)
            {
                if (obj != null)
                {
                    StringWriter strWriter = new StringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(strWriter, obj);
                    return strWriter.ToString();
                }
                else
                {
                    return String.Empty;
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
                lock (xmlFileName)
                {
                    try
                    {
                        if (!File.Exists(xmlFileName))
                        {
                            Console.Write("序列化文件不存在!");
                            return default(T);
                        }
                        else
                        {
                            string xmlContent = FileHelper.ReadFile(xmlFileName, Encoding.UTF8);
                            T obj = DeserializeObject<T>(xmlContent);
                            return obj;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                        return default(T);
                    }
                }
            }

            /// <summary>
            /// 把xml字符串反序列化为对象
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="xmlString"></param>
            /// <returns></returns>
            public static T DeserializeObject<T>(string xmlString)
            {
                if (!String.IsNullOrEmpty(xmlString))
                {
                    StringReader strReader = new StringReader(xmlString);
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    T obj = (T)serializer.Deserialize(strReader);
                    return obj;
                }
                else
                {
                    return default(T);
                }
            }
            /// <summary>
            /// 向指定文件写入内容
            /// </summary>
            /// <param name="path">要写入内容的文件完整路径</param>
            /// <param name="content">要写入的内容</param>
            /// <param name="encoding">编码格式</param>
            public static void WriteFile(string path, string content, System.Text.Encoding encoding)
            {
                try
                {
                    object obj = new object();
                    if (!File.Exists(path))
                    {
                        FileStream fileStream = File.Create(path);
                        fileStream.Close();
                    }
                    lock (obj)
                    {
                        using (StreamWriter streamWriter = new StreamWriter(path, false, encoding))
                        {
                            streamWriter.WriteLine(content);
                            streamWriter.Close();
                            streamWriter.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);

                }
            }
        }
    }

