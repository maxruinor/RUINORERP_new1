using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateTools
{
    class FileHelper
    {
        /// <summary>
        /// 向指定文件写入内容
        /// </summary>
        /// <param name="path">要写入内容的文件完整路径</param>
        /// <param name="content">要写入的内容</param>
        public static void WriteFile(string path, string content)
        {
            try
            {
                object obj = new object();
                if (!System.IO.File.Exists(path))
                {
                    System.IO.FileStream fileStream = System.IO.File.Create(path);
                    fileStream.Close();
                }
                lock (obj)
                {
                    using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(path, false, System.Text.Encoding.Default))
                    {
                        streamWriter.WriteLine(content);
                        streamWriter.Close();
                        streamWriter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                if (!System.IO.File.Exists(path))
                {
                    System.IO.FileStream fileStream = System.IO.File.Create(path);
                    fileStream.Close();
                }
                lock (obj)
                {
                    using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(path, false, encoding))
                    {
                        streamWriter.WriteLine(content);
                        streamWriter.Close();
                        streamWriter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">要读取的文件路径</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>返回文件内容</returns>
        public static string ReadFile(string path, System.Text.Encoding encoding)
        {
            string result;
            if (!System.IO.File.Exists(path))
            {
                result = "不存在相应的目录";
            }
            else
            {
                System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, encoding);
                result = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
            }
            return result;
        }

    }
}


