using System;
using System.IO;
using System.Text;

namespace AutoUpdateTools
{
    /// <summary>
    /// 文件操作辅助类
    /// </summary>
    class FileHelper
    {
        /// <summary>
        /// 向指定文件写入内容
        /// </summary>
        /// <param name="path">要写入内容的文件完整路径</param>
        /// <param name="content">要写入的内容</param>
        public static void WriteFile(string path, string content)
        {
            WriteFile(path, content, Encoding.Default);
        }

        /// <summary>
        /// 向指定文件写入内容
        /// </summary>
        /// <param name="path">要写入内容的文件完整路径</param>
        /// <param name="content">要写入的内容</param>
        /// <param name="encoding">编码格式</param>
        public static void WriteFile(string path, string content, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path)) return;
            
            try
            {
                // 确保目录存在
                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 使用using语句确保资源正确释放
                using (var writer = new StreamWriter(path, false, encoding))
                {
                    writer.Write(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"写入文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">要读取的文件路径</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>返回文件内容</returns>
        public static string ReadFile(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            
            try
            {
                if (!File.Exists(path))
                {
                    return "文件不存在: " + path;
                }

                // 使用using语句确保资源正确释放
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return $"读取文件失败: {ex.Message}";
            }
        }
    }
}