using RUINORERP.Model.ConfigModel;
using System;
using System.IO;
using System.Windows.Forms;

namespace RUINORERP.Server.Helpers
{
    /// <summary>
    /// 文件存储帮助类
    /// 用于处理文件存储路径的初始化、验证和管理
    /// </summary>
    public static class FileStorageHelper
    {
        /// <summary>
        /// 初始化文件存储路径
        /// 确保配置的存储目录存在，如果不存在则创建
        /// </summary>
        /// <param name="config">服务器配置对象</param>
        /// <returns>是否初始化成功</returns>
        public static bool InitializeStoragePath(ServerConfig config)
        {
            try
            {
                // 验证配置对象
                if (config == null || string.IsNullOrEmpty(config.FileStoragePath))
                {
                    MessageBox.Show("文件存储路径未配置，请在全局配置中设置", "配置错误", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // 检查是否为程序运行目录或其子目录
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string storagePath = Path.GetFullPath(config.FileStoragePath);
                
                if (storagePath.StartsWith(appDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    DialogResult result = MessageBox.Show(
                        "警告：您正在使用程序运行目录作为文件存储位置，这可能导致程序文件和数据文件混淆。\n" +
                        "建议使用单独的目录存储文件。\n\n是否继续使用此路径？",
                        "路径警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.No)
                    {
                        return false;
                    }
                }

                // 确保目录存在
                if (!Directory.Exists(storagePath))
                {
                    Directory.CreateDirectory(storagePath);
                    MessageBox.Show($"已创建文件存储目录: {storagePath}", "目录创建", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // 创建子目录结构
                CreateSubDirectories(storagePath);
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化文件存储路径时出错: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 创建必要的子目录结构
        /// </summary>
        /// <param name="basePath">基础存储路径</param>
        private static void CreateSubDirectories(string basePath)
        {
            string[] subDirectories = {
                "Products",      // 产品图片
                "Documents",     // 文档
                "Certificates",  // 凭证
                "Temp",          // 临时文件
                "Backups"        // 备份
            };

            foreach (string dir in subDirectories)
            {
                string subDirPath = Path.Combine(basePath, dir);
                if (!Directory.Exists(subDirPath))
                {
                    Directory.CreateDirectory(subDirPath);
                }
            }
        }

        /// <summary>
        /// 验证文件存储路径的有效性
        /// </summary>
        /// <param name="path">要验证的路径</param>
        /// <returns>路径是否有效</returns>
        public static bool ValidateStoragePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            try
            {
                // 检查路径格式是否有效
                Path.GetFullPath(path);
                
                // 检查是否为系统保留路径
                string root = Path.GetPathRoot(path);
                if (string.IsNullOrEmpty(root))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取指定类型的文件存储子目录
        /// </summary>
        /// <param name="basePath">基础存储路径</param>
        /// <param name="fileType">文件类型</param>
        /// <returns>完整的子目录路径</returns>
        public static string GetFileTypeDirectory(string basePath, FileType fileType)
        {
            string subDirName = fileType switch
            {
                FileType.Product => "Products",
                FileType.Document => "Documents",
                FileType.Certificate => "Certificates",
                FileType.Temp => "Temp",
                FileType.Backup => "Backups",
                _ => "Other"
            };

            string subDirPath = Path.Combine(basePath, subDirName);
            if (!Directory.Exists(subDirPath))
            {
                Directory.CreateDirectory(subDirPath);
            }
            return subDirPath;
        }
    }

    /// <summary>
    /// 文件类型枚举
    /// </summary>
    public enum FileType
    {
        Product,      // 产品图片
        Document,     // 文档
        Certificate,  // 凭证
        Temp,         // 临时文件
        Backup,       // 备份
        Other         // 其他文件
    }
}