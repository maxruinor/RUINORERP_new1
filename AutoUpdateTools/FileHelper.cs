using System;
using System.IO;
using System.Text;

namespace AutoUpdateTools
{
    /// <summary>
    /// 文件操作辅助类，提供文件读写等常用操作
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 向指定文件写入内容，使用默认编码
        /// </summary>
        /// <param name="path">要写入内容的文件完整路径</param>
        /// <param name="content">要写入的内容</param>
        /// <exception cref="ArgumentNullException">当path为null或为空时抛出</exception>
        /// <exception cref="IOException">当文件写入失败时抛出</exception>
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
        /// <exception cref="ArgumentNullException">当path为null或为空时抛出</exception>
        /// <exception cref="IOException">当文件写入失败时抛出</exception>
        public static void WriteFile(string path, string content, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "文件路径不能为空");
            }
            
            try
            {
                // 确保目录存在
                string directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    System.Diagnostics.Debug.WriteLine($"创建目录: {directory}");
                }

                // 使用using语句确保资源正确释放
                using (var writer = new StreamWriter(path, false, encoding))
                {
                    writer.Write(content ?? string.Empty);
                }
                
                System.Diagnostics.Debug.WriteLine($"成功写入文件: {path}");
            }
            catch (Exception ex)
            {
                string errorMessage = $"写入文件失败: {path}, 错误: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(errorMessage);
                throw new IOException(errorMessage, ex);
            }
        }

        /// <summary>
        /// 读取文件内容，使用默认编码
        /// </summary>
        /// <param name="path">要读取的文件路径</param>
        /// <returns>返回文件内容</returns>
        /// <exception cref="ArgumentNullException">当path为null或为空时抛出</exception>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        /// <exception cref="IOException">当文件读取失败时抛出</exception>
        public static string ReadFile(string path)
        {
            return ReadFile(path, Encoding.Default);
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">要读取的文件路径</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>返回文件内容</returns>
        /// <exception cref="ArgumentNullException">当path为null或为空时抛出</exception>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        /// <exception cref="IOException">当文件读取失败时抛出</exception>
        public static string ReadFile(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "文件路径不能为空");
            }
            
            try
            {
                if (!File.Exists(path))
                {
                    string errorMessage = $"文件不存在: {path}";
                    System.Diagnostics.Debug.WriteLine(errorMessage);
                    throw new FileNotFoundException(errorMessage, path);
                }

                // 使用using语句确保资源正确释放
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream, encoding))
                {
                    string content = reader.ReadToEnd();
                    System.Diagnostics.Debug.WriteLine($"成功读取文件: {path}, 内容长度: {content.Length}");
                    return content;
                }
            }
            catch (FileNotFoundException)
            {
                throw; // 重新抛出文件不存在异常，不添加额外信息
            }
            catch (Exception ex)
            {
                string errorMessage = $"读取文件失败: {path}, 错误: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(errorMessage);
                throw new IOException(errorMessage, ex);
            }
        }
    }
}