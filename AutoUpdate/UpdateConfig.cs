using System;
using Newtonsoft.Json;

namespace AutoUpdate
{
    /// <summary>
    /// 更新配置类，用于解决命令行参数解析问题（.NET Framework兼容版本）
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