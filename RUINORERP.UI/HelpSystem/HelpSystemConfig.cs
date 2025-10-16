using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助系统配置类
    /// </summary>
    [Serializable]
    public class HelpSystemConfig
    {
        /// <summary>
        /// 帮助文件路径
        /// </summary>
        public string HelpFilePath { get; set; } = FindHelpFile();

        /// <summary>
        /// 是否启用帮助系统
        /// </summary>
        public bool IsHelpSystemEnabled { get; set; } = true;

        /// <summary>
        /// 是否记录帮助查看历史
        /// </summary>
        public bool IsHistoryTrackingEnabled { get; set; } = true;

        /// <summary>
        /// 历史记录最大条数
        /// </summary>
        public int MaxHistoryCount { get; set; } = 50;

        /// <summary>
        /// 是否启用帮助内容搜索
        /// </summary>
        public bool IsSearchEnabled { get; set; } = true;

        /// <summary>
        /// 是否启用帮助内容推荐
        /// </summary>
        public bool IsRecommendationEnabled { get; set; } = true;

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static readonly string ConfigFilePath = Path.Combine(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "HelpSystemConfig.xml");

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns>帮助系统配置</returns>
        public static HelpSystemConfig Load()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(HelpSystemConfig));
                    using (FileStream fs = new FileStream(ConfigFilePath, FileMode.Open))
                    {
                        return (HelpSystemConfig)serializer.Deserialize(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"加载帮助系统配置时出错: {ex.Message}");
            }

            // 返回默认配置
            return new HelpSystemConfig();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(HelpSystemConfig));
                using (FileStream fs = new FileStream(ConfigFilePath, FileMode.Create))
                {
                    serializer.Serialize(fs, this);
                }
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"保存帮助系统配置时出错: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 查找帮助文件
        /// </summary>
        /// <returns>帮助文件路径</returns>
        private static string FindHelpFile()
        {
            // 可能的帮助文件路径
            string[] possiblePaths = {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "help.chm"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\RUINORERP.Helper\\help.chm"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\RUINORERP.Helper\\help.chm"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RUINORERP\\help.chm"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RUINORERP.Helper.chm")
            };
            
            foreach (string path in possiblePaths)
            {
                string fullPath = Path.GetFullPath(path);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            
            // 如果找不到CHM文件，返回默认路径
            return "RUINORERP.Helper.chm";
        }
    }
}