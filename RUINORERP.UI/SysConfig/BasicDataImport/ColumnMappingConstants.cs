using System;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 列映射配置常量类
    /// 定义列映射配置相关的常量值
    /// </summary>
    public static class ColumnMappingConstants
    {
        /// <summary>
        /// 列映射配置文件保存的相对路径（相对于应用程序根目录）
        /// </summary>
        public const string ConfigFolderName = "SysConfigFiles\\ColumnMappings";

        /// <summary>
        /// 列映射配置文件扩展名
        /// </summary>
        public const string ConfigFileExtension = ".xml";

        /// <summary>
        /// 获取列映射配置文件的完整保存路径
        /// </summary>
        /// <returns>配置文件保存路径</returns>
        public static string GetConfigFilePath()
        {
            return System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                ConfigFolderName);
        }
    }
}
