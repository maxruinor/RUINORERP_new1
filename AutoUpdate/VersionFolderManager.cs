using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace AutoUpdate
{
    /// <summary>
    /// 版本文件夹管理器
    /// 负责管理版本文件夹的创建、删除和访问
    /// </summary>
    public class VersionFolderManager
    {
        /// <summary>
        /// 版本管理主目录
        /// </summary>
        public string VersionsRootDir { get; private set; }
        
        /// <summary>
        /// 当前版本配置文件路径
        /// </summary>
        public string CurrentVersionFilePath { get; private set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public VersionFolderManager()
        {
            // 初始化版本管理主目录
            VersionsRootDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Versions");
            
            // 初始化当前版本配置文件路径
            CurrentVersionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CurrentVersion.txt");
            
            // 确保版本管理主目录存在
            EnsureVersionsDirExists();
        }
        
        /// <summary>
        /// 确保版本管理主目录存在
        /// </summary>
        private void EnsureVersionsDirExists()
        {
            if (!Directory.Exists(VersionsRootDir))
            {
                Directory.CreateDirectory(VersionsRootDir);
                Debug.WriteLine($"创建版本管理主目录: {VersionsRootDir}");
            }
        }
        
        /// <summary>
        /// 创建版本文件夹
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>版本文件夹名称</returns>
        public string CreateVersionFolder(string version)
        {
            try
            {
                // 生成版本文件夹名称：{版本号}_{时间戳}
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string folderName = $"{version}_{timestamp}";
                string folderPath = Path.Combine(VersionsRootDir, folderName);
                
                // 创建版本文件夹
                Directory.CreateDirectory(folderPath);
                Debug.WriteLine($"创建版本文件夹: {folderPath}");
                
                return folderName;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"创建版本文件夹失败: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 删除版本文件夹
        /// </summary>
        /// <param name="folderName">版本文件夹名称</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteVersionFolder(string folderName)
        {
            try
            {
                string folderPath = Path.Combine(VersionsRootDir, folderName);
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                    Debug.WriteLine($"删除版本文件夹成功: {folderPath}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"删除版本文件夹失败: {folderName}, 错误: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 获取版本文件夹路径
        /// </summary>
        /// <param name="folderName">版本文件夹名称</param>
        /// <returns>版本文件夹路径</returns>
        public string GetVersionFolderPath(string folderName)
        {
            return Path.Combine(VersionsRootDir, folderName);
        }
        
        /// <summary>
        /// 获取指定版本的所有文件
        /// </summary>
        /// <param name="folderName">版本文件夹名称</param>
        /// <returns>文件路径列表</returns>
        public List<string> GetVersionFiles(string folderName)
        {
            try
            {
                string folderPath = GetVersionFolderPath(folderName);
                if (!Directory.Exists(folderPath))
                {
                    return new List<string>();
                }
                
                // 获取文件夹中的所有文件
                List<string> files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                    .Select(file => file.Substring(folderPath.Length + 1))
                    .ToList();
                
                return files;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"获取版本文件列表失败: {folderName}, 错误: {ex.Message}");
                return new List<string>();
            }
        }
        
        /// <summary>
        /// 计算版本的完整性校验和
        /// </summary>
        /// <param name="folderName">版本文件夹名称</param>
        /// <returns>版本完整性校验和</returns>
        public string CalculateVersionChecksum(string folderName)
        {
            try
            {
                string folderPath = GetVersionFolderPath(folderName);
                if (!Directory.Exists(folderPath))
                {
                    return null;
                }
                
                // 获取文件夹中的所有文件，按路径排序
                string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                    .OrderBy(file => file)
                    .ToArray();
                
                // 创建SHA256哈希计算器
                using (SHA256 sha256 = SHA256.Create())
                {
                    // 计算所有文件的哈希值
                    foreach (string file in files)
                    {
                        using (FileStream stream = File.OpenRead(file))
                        {
                            byte[] fileHash = sha256.ComputeHash(stream);
                            sha256.TransformBlock(fileHash, 0, fileHash.Length, fileHash, 0);
                        }
                    }
                    
                    // 完成哈希计算
                    sha256.TransformFinalBlock(new byte[0], 0, 0);
                    
                    // 将哈希值转换为十六进制字符串
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in sha256.Hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"计算版本校验和失败: {folderName}, 错误: {ex.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// 验证版本文件夹的完整性
        /// </summary>
        /// <param name="folderName">版本文件夹名称</param>
        /// <param name="expectedChecksum">预期的校验和</param>
        /// <returns>是否验证通过</returns>
        public bool ValidateVersionFolder(string folderName, string expectedChecksum)
        {
            try
            {
                // 计算实际校验和
                string actualChecksum = CalculateVersionChecksum(folderName);
                
                // 比较校验和
                return actualChecksum == expectedChecksum;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"验证版本文件夹完整性失败: {folderName}, 错误: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 获取当前版本
        /// </summary>
        /// <returns>当前版本号</returns>
        public string GetCurrentVersion()
        {
            try
            {
                if (File.Exists(CurrentVersionFilePath))
                {
                    return File.ReadAllText(CurrentVersionFilePath).Trim();
                }
                return "1.0.0.0"; // 默认版本
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"获取当前版本失败: {ex.Message}");
                return "1.0.0.0"; // 默认版本
            }
        }
        
        /// <summary>
        /// 设置当前版本
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>是否设置成功</returns>
        public bool SetCurrentVersion(string version)
        {
            try
            {
                File.WriteAllText(CurrentVersionFilePath, version);
                Debug.WriteLine($"设置当前版本成功: {version}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"设置当前版本失败: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 获取所有版本文件夹名称
        /// </summary>
        /// <returns>版本文件夹名称列表，按创建时间倒序排列</returns>
        public List<string> GetAllVersionFolders()
        {
            try
            {
                if (!Directory.Exists(VersionsRootDir))
                {
                    return new List<string>();
                }
                
                // 获取所有版本文件夹，按创建时间倒序排列
                return Directory.GetDirectories(VersionsRootDir)
                    .Select(dir => new {
                        Path = dir,
                        CreationTime = Directory.GetCreationTime(dir)
                    })
                    .OrderByDescending(dir => dir.CreationTime)
                    .Select(dir => Path.GetFileName(dir.Path))
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"获取所有版本文件夹失败: {ex.Message}");
                return new List<string>();
            }
        }
        
        /// <summary>
        /// 复制文件到版本文件夹
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="versionFolderName">版本文件夹名称</param>
        /// <param name="relativePath">相对路径，默认为空</param>
        /// <returns>是否复制成功</returns>
        public bool CopyFileToVersionFolder(string sourceFilePath, string versionFolderName, string relativePath = "")
        {
            try
            {
                string folderPath = GetVersionFolderPath(versionFolderName);
                string destFilePath = Path.Combine(folderPath, relativePath, Path.GetFileName(sourceFilePath));
                
                // 确保目标目录存在
                string destDir = Path.GetDirectoryName(destFilePath);
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
                
                // 复制文件
                File.Copy(sourceFilePath, destFilePath, true);
                Debug.WriteLine($"复制文件到版本文件夹成功: {sourceFilePath} -> {destFilePath}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"复制文件到版本文件夹失败: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 从版本文件夹复制文件到目标位置
        /// </summary>
        /// <param name="versionFolderName">版本文件夹名称</param>
        /// <param name="relativeFilePath">相对文件路径</param>
        /// <param name="destFilePath">目标文件路径</param>
        /// <returns>是否复制成功</returns>
        public bool CopyFileFromVersionFolder(string versionFolderName, string relativeFilePath, string destFilePath)
        {
            try
            {
                string folderPath = GetVersionFolderPath(versionFolderName);
                string sourceFilePath = Path.Combine(folderPath, relativeFilePath);
                
                // 确保源文件存在
                if (!File.Exists(sourceFilePath))
                {
                    Debug.WriteLine($"源文件不存在: {sourceFilePath}");
                    return false;
                }
                
                // 确保目标目录存在
                string destDir = Path.GetDirectoryName(destFilePath);
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
                
                // 复制文件
                File.Copy(sourceFilePath, destFilePath, true);
                Debug.WriteLine($"从版本文件夹复制文件成功: {sourceFilePath} -> {destFilePath}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"从版本文件夹复制文件失败: {ex.Message}");
                return false;
            }
        }
    }
}