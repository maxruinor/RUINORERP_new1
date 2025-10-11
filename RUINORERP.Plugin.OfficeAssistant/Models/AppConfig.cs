using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RUINORERP.Plugin.OfficeAssistant.Models
{
    /// <summary>
    /// 应用程序配置类
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 旧文件路径
        /// </summary>
        public string OldFilePath { get; set; }
        
        /// <summary>
        /// 新文件路径
        /// </summary>
        public string NewFilePath { get; set; }
        
        /// <summary>
        /// 是否自动加载文件
        /// </summary>
        public bool AutoLoadFiles { get; set; }
        
        /// <summary>
        /// 区分大小写设置
        /// </summary>
        public bool CaseSensitive { get; set; }
        
        /// <summary>
        /// 忽略空格设置
        /// </summary>
        public bool IgnoreSpaces { get; set; }
        
        /// <summary>
        /// 对比模式
        /// </summary>
        public string ComparisonMode { get; set; }
        
        /// <summary>
        /// 键列映射
        /// </summary>
        public List<string> KeyColumns { get; set; }
        
        /// <summary>
        /// 比较列映射
        /// </summary>
        public List<string> CompareColumns { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppConfig()
        {
            KeyColumns = new List<string>();
            CompareColumns = new List<string>();
            AutoLoadFiles = false;
            CaseSensitive = false;
            IgnoreSpaces = false;
            ComparisonMode = "数据差异";
        }
        
        /// <summary>
        /// 保存配置到文件
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        public void Save(string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"保存配置文件时发生错误: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 从文件加载配置
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>应用程序配置</returns>
        public static AppConfig Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new AppConfig();
                }
                
                string json = File.ReadAllText(filePath, Encoding.UTF8);
                return JsonConvert.DeserializeObject<AppConfig>(json) ?? new AppConfig();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"加载配置文件时发生错误: {ex.Message}", ex);
            }
        }
    }
}