using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RUINORERP.Common.Helper
{
    public class XmlHelper
    {

        public void Serialize<T>(T entity,string fileName)
        {
            string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\FormProperty\\Data\\", fileName);
            System.IO.FileInfo fi = new System.IO.FileInfo(PathwithFileName);
            //判断目录是否存在
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }
            //SerializationHelper.Serialize(entity, PathwithFileName, false);
            string json = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
            });
            File.WriteAllText(PathwithFileName, json);
        }

        public T Deserialize<T>(string fileName) where T : class
        {
            object Entity = null;
            string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\FormProperty\\Data\\", fileName);
            System.IO.FileInfo fi = new System.IO.FileInfo(PathwithFileName);
            //判断目录是否存在
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }
            if (System.IO.File.Exists(PathwithFileName))
            {
                //  Entity = SerializationHelper.Deserialize(PathwithFileName, false) as T;
                string json = File.ReadAllText(PathwithFileName);
                Entity = JsonConvert.DeserializeObject<T>(json) as T;
                
            }
            return Entity as T;
        }

        public object Deserialize(string fileName) 
        {
            object Entity = null;
            string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\FormProperty\\Data\\", fileName);
            System.IO.FileInfo fi = new System.IO.FileInfo(PathwithFileName);
            //判断目录是否存在
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }
            if (System.IO.File.Exists(PathwithFileName))
            {
                //  Entity = SerializationHelper.Deserialize(PathwithFileName, false) as T;
                string json = File.ReadAllText(PathwithFileName);
                Entity = JsonConvert.DeserializeObject(json);

            }
            return Entity  ;
        }

        /// <summary>
        /// 转换对象为JSON格式数据
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>字符格式的JSON数据</returns>
        public static string GetXML<T>(object obj)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));

                using (TextWriter tw = new StringWriter())
                {
                    xs.Serialize(tw, obj);
                    return tw.ToString();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Xml格式字符转换为T类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T ParseFormByXml<T>(string xml,string rootName="root")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));
            StringReader reader = new StringReader(xml);

            T res = (T)serializer.Deserialize(reader);
            reader.Close();
            reader.Dispose();
            return res; 
        }



        /// <summary>
        /// serialize object to xml file.
        /// </summary>
        /// <param name="path">the path to save the xml file</param>
        /// <param name="obj">the object you want to serialize</param>
        public void serialize_to_xml(string path, object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            string content = string.Empty;
            //serialize
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                content = writer.ToString();
            }
            //save to file
            using (StreamWriter stream_writer = new StreamWriter(path))
            {
                stream_writer.Write(content);
            }
        }

        /// <summary>
        /// deserialize xml file to object
        /// </summary>
        /// <param name="path">the path of the xml file</param>
        /// <param name="object_type">the object type you want to deserialize</param>
        public object deserialize_from_xml(string path, Type object_type)
        {
            XmlSerializer serializer = new XmlSerializer(object_type);
            using (StreamReader reader = new StreamReader(path))
            {
                return serializer.Deserialize(reader);
            }
        }
    }
}
