using System;
using System.IO;
using System.Xml.Serialization;

namespace HLH.Lib.Xml
{
    //测试代码：
    /*
 
public class Student
{
    public string Name { set; get; }
    public int Age { set; get; }
}
 
Student stu1 = new Student() { Name = "okbase", Age = 10 };
string xml = XmlUtil.Serializer(typeof(Student), stu1);
Console.Write(xml);
2. Xml转换到实体对象
 
Student stu2 = XmlUtil.Deserialize(typeof(Student), xml) as Student;
Console.Write(string.Format("名字:{0},年龄:{1}", stu2.Name, stu2.Age));
   
     */



    /// <summary>
    /// Xml序列化与反序列化
    /// </summary>
    public class XmlUtil
    {
        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {

            MemoryStream Stream = new MemoryStream();
            try
            {
                XmlSerializer xml = new XmlSerializer(type);

                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion
    }
}

