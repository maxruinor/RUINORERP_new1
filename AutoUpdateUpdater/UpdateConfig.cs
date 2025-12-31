using System;
using System.IO;
using Newtonsoft.Json;

namespace AutoUpdateUpdater
{
    /// <summary>
    /// 更新配置类，用于解决命令行参数解析问题
    /// </summary>
    public class UpdateConfig
    {
        /// <summary>
        /// 源目录路径
        /// </summary>
        public string SourceDir { get; set; } = string.Empty;

        /// <summary>
        /// 目标目录路径
        /// </summary>
        public string TargetDir { get; set; } = string.Empty;

        /// <summary>
        /// 可执行文件名
        /// </summary>
        public string ExeName { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// AutoUpdate.exe的完整路径（直接指定，避免搜索版本目录）
        /// </summary>
        public string AutoUpdateExePath { get; set; } = string.Empty;

        /// <summary>
        /// 从JSON文件加载配置（带重试机制）
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        /// <returns>配置对象</returns>
        public static UpdateConfig LoadFromFile(string configPath)
        {
            try
            {
                if (File.Exists(configPath))
                {
                    // 使用安全的文件读取方法
                    string jsonContent = SafeReadFile(configPath, 3, 500);
                    
                    if (!string.IsNullOrEmpty(jsonContent))
                    {
                        return JsonConvert.DeserializeObject<UpdateConfig>(jsonContent) ?? new UpdateConfig();
                    }
                    else
                    {
                        Console.WriteLine("安全文件读取返回空内容");
                    }
                }
                else
                {
                    Console.WriteLine($"配置文件不存在: {configPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载配置文件失败: {ex.Message}");
            }
            
            return new UpdateConfig();
        }

        /// <summary>
        /// 安全的文件读取方法（带重试机制）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="maxRetries">最大重试次数</param>
        /// <param name="retryDelay">重试延迟（毫秒）</param>
        /// <returns>文件内容，读取失败返回空字符串</returns>
        private static string SafeReadFile(string filePath, int maxRetries = 3, int retryDelay = 500)
        {
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                catch (IOException ex) when (attempt < maxRetries - 1)
                {
                    Console.WriteLine($"文件读取失败（尝试 {attempt + 1}/{maxRetries}）: {ex.Message}");
                    System.Threading.Thread.Sleep(retryDelay);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"文件读取失败（最终）: {ex.Message}");
                    return string.Empty;
                }
            }
            
            return string.Empty;
        }

        /// <summary>
        /// 保存配置到JSON文件
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        public void SaveToFile(string configPath)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(configPath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存配置文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证配置是否完整
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(SourceDir) && 
                   !string.IsNullOrEmpty(TargetDir) && 
                   !string.IsNullOrEmpty(ExeName);
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>配置信息字符串</returns>
        public override string ToString()
        {
            return $"SourceDir: {SourceDir}, TargetDir: {TargetDir}, ExeName: {ExeName}";
        }
    }
}