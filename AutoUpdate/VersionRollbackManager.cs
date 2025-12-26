using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Linq;
using System.IO.Compression;
using System.Text;

namespace AutoUpdate
{
    /// <summary>
    /// 版本回滚管理器
    /// 负责处理应用程序的版本回滚操作
    /// </summary>
    public class VersionRollbackManager
    {
        /// <summary>
        /// 版本历史管理器
        /// </summary>
        private VersionHistoryManager versionHistoryManager;

        /// <summary>
        /// 更新服务器URL
        /// </summary>
        private string UpdateServerUrl { get; set; }

        /// <summary>
        /// 应用程序ID
        /// </summary>
        private string AppId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="updateServerUrl">更新服务器URL</param>
        /// <param name="appId">应用程序ID</param>
        public VersionRollbackManager(string updateServerUrl = null, string appId = null)
        {
            versionHistoryManager = new VersionHistoryManager();
            UpdateServerUrl = updateServerUrl;
            AppId = appId;
        }

        /// <summary>
        /// 获取可回滚的版本列表
        /// </summary>
        /// <returns>可回滚的版本列表（排除当前版本）</returns>
        public List<VersionEntry> GetRollbackVersions()
        {
            try
            {
                var allVersions = versionHistoryManager.GetAllVersions();
                if (allVersions.Count <= 1)
                {
                    return new List<VersionEntry>();
                }

                // 返回除当前版本外的所有版本（按安装时间倒序）
                return allVersions.Skip(1).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"获取回滚版本列表失败: {ex.Message}");
                return new List<VersionEntry>();
            }
        }

        /// <summary>
        /// 执行版本回滚
        /// </summary>
        /// <param name="targetVersion">目标版本号</param>
        /// <returns>回滚是否成功</returns>
        public bool RollbackToVersion(string targetVersion)
        {
            try
            {
                Debug.WriteLine($"开始回滚到版本: {targetVersion}");

                // 1. 验证目标版本是否存在于历史记录中
                var targetVersionEntry = versionHistoryManager.GetAllVersions().FirstOrDefault(v => v.Version == targetVersion);
                if (targetVersionEntry == null)
                {
                    Debug.WriteLine($"目标版本不存在于历史记录中: {targetVersion}");
                    return false;
                }

                // 2. 检查目标版本是否为当前版本
                var currentVersion = versionHistoryManager.GetCurrentVersion();
                if (currentVersion != null && currentVersion.Version == targetVersion)
                {
                    Debug.WriteLine("目标版本已是当前版本，无需回滚");
                    return false;
                }

                // 3. 获取目标版本的文件夹路径
                string targetVersionFolder = versionHistoryManager.GetVersionFolderPath(targetVersion);
                bool rollbackSuccess = false;
                
                if (!string.IsNullOrEmpty(targetVersionFolder) && Directory.Exists(targetVersionFolder))
                {
                    // 3.1 从本地版本目录回滚
                    Debug.WriteLine($"从本地版本目录回滚: {targetVersionFolder}");
                    rollbackSuccess = RollbackFromLocalVersion(targetVersion, targetVersionEntry);
                }
                else if (!string.IsNullOrEmpty(UpdateServerUrl))
                {
                    // 3.2 从服务器下载目标版本进行回滚
                    Debug.WriteLine($"从服务器下载目标版本进行回滚: {targetVersion}");
                    rollbackSuccess = RollbackFromServer(targetVersion);
                }
                else
                {
                    Debug.WriteLine("无法回滚，本地版本目录不存在且未配置更新服务器URL");
                    return false;
                }

                if (rollbackSuccess)
                {
                    // 4. 更新当前版本配置
                    UpdateCurrentVersionConfig(targetVersion);
                    
                    // 5. 更新版本历史记录
                    versionHistoryManager.RecordNewVersion(targetVersion, targetVersionEntry.FolderName, targetVersionEntry.Files, targetVersionEntry.Checksum);
                    Debug.WriteLine($"版本回滚成功，当前版本: {targetVersion}");
                    
                    return true;
                }
                else
                {
                    Debug.WriteLine("版本回滚失败");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"执行版本回滚失败: {ex.Message}\r\n堆栈跟踪: {ex.StackTrace}");
                return false;
            }
        }
        
        /// <summary>
        /// 从本地版本目录回滚
        /// </summary>
        /// <param name="targetVersion">目标版本号</param>
        /// <param name="targetVersionEntry">目标版本条目</param>
        /// <returns>回滚是否成功</returns>
        private bool RollbackFromLocalVersion(string targetVersion, VersionEntry targetVersionEntry)
        {
            try
            {
                string targetVersionFolder = versionHistoryManager.GetVersionFolderPath(targetVersion);
                if (string.IsNullOrEmpty(targetVersionFolder) || !Directory.Exists(targetVersionFolder))
                {
                    Debug.WriteLine($"本地版本目录不存在: {targetVersionFolder}");
                    return false;
                }
                
                // 1. 获取应用程序根目录
                string appRootDir = AppDomain.CurrentDomain.BaseDirectory;
                
                // 2. 备份当前版本
                string backupDir = Path.Combine(appRootDir, "Backup_Rollback_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                Debug.WriteLine($"开始备份当前版本到: {backupDir}");
                BackupCurrentVersion(appRootDir, backupDir);
                Debug.WriteLine("当前版本备份完成");
                
                try
                {
                    // 3. 从版本目录复制文件到应用程序目录
                    Debug.WriteLine($"开始从版本目录复制文件: {targetVersionFolder} -> {appRootDir}");
                    
                    // 获取版本目录中的所有文件
                    string[] versionFiles = Directory.GetFiles(targetVersionFolder, "*.*");
                    
                    foreach (string file in versionFiles)
                    {
                        string fileName = Path.GetFileName(file);
                        string destFile = Path.Combine(appRootDir, fileName);
                        
                        // 如果目标文件存在，先尝试删除
                        if (File.Exists(destFile))
                        {
                            try
                            {
                                File.Delete(destFile);
                            }
                            catch (IOException)
                            {
                                // 文件可能被锁定，尝试重命名后删除
                                string tempPath = destFile + ".old";
                                if (File.Exists(tempPath))
                                {
                                    File.Delete(tempPath);
                                }
                                File.Move(destFile, tempPath);
                            }
                        }
                        
                        // 复制文件
                        File.Copy(file, destFile, true);
                        Debug.WriteLine($"复制文件: {file} -> {destFile}");
                    }
                    
                    Debug.WriteLine("从本地版本目录复制文件完成");
                    
                    return true;
                }
                catch (Exception ex)
                {
                    // 回滚失败，恢复备份
                    Debug.WriteLine($"从本地版本目录回滚失败，开始恢复备份: {ex.Message}");
                    RestoreFromBackup(appRootDir, backupDir);
                    return false;
                }
                finally
                {
                    // 清理备份目录
                    if (Directory.Exists(backupDir))
                    {
                        Directory.Delete(backupDir, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"从本地版本目录回滚失败: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 从服务器下载目标版本进行回滚
        /// </summary>
        /// <param name="targetVersion">目标版本号</param>
        /// <returns>回滚是否成功</returns>
        private bool RollbackFromServer(string targetVersion)
        {
            try
            {
                // 1. 从服务器获取目标版本的更新包
                string updatePackagePath = DownloadUpdatePackage(targetVersion);
                if (string.IsNullOrEmpty(updatePackagePath))
                {
                    Debug.WriteLine("下载目标版本更新包失败");
                    return false;
                }

                // 2. 安装目标版本
                bool installSuccess = InstallVersion(updatePackagePath);
                if (!installSuccess)
                {
                    Debug.WriteLine("安装目标版本失败");
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"从服务器回滚失败: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 备份当前版本
        /// </summary>
        /// <param name="currentDir">当前版本目录</param>
        /// <param name="backupDir">备份目录</param>
        private void BackupCurrentVersion(string currentDir, string backupDir)
        {
            try
            {
                // 确保备份目录存在
                Directory.CreateDirectory(backupDir);
                
                // 备份核心文件
                string[] filesToBackup = Directory.GetFiles(currentDir, "AutoUpdate.*")
                    .Where(file => Path.GetExtension(file) == ".exe" || Path.GetExtension(file) == ".dll" || Path.GetExtension(file) == ".config")
                    .ToArray();
                
                foreach (string file in filesToBackup)
                {
                    string destFile = Path.Combine(backupDir, Path.GetFileName(file));
                    File.Copy(file, destFile, true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"备份当前版本失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 从备份恢复
        /// </summary>
        /// <param name="targetDir">目标目录</param>
        /// <param name="backupDir">备份目录</param>
        private void RestoreFromBackup(string targetDir, string backupDir)
        {
            try
            {
                // 复制备份文件到目标目录
                string[] backupFiles = Directory.GetFiles(backupDir, "*.*");
                foreach (string backupFile in backupFiles)
                {
                    string destFile = Path.Combine(targetDir, Path.GetFileName(backupFile));
                    
                    // 如果目标文件存在，先删除
                    if (File.Exists(destFile))
                    {
                        File.Delete(destFile);
                    }
                    
                    // 复制备份文件
                    File.Copy(backupFile, destFile, true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"从备份恢复失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 更新当前版本配置
        /// </summary>
        /// <param name="version">版本号</param>
        private void UpdateCurrentVersionConfig(string version)
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CurrentVersion.txt");
                File.WriteAllText(configPath, version);
                Debug.WriteLine($"更新当前版本配置成功: {version}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"更新当前版本配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 下载更新包
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>下载的更新包路径，如果失败则返回null</returns>
        private string DownloadUpdatePackage(string version)
        {
            try
            {
                if (string.IsNullOrEmpty(UpdateServerUrl))
                {
                    Debug.WriteLine("更新服务器URL未配置");
                    return null;
                }

                string packageUrl = $"{UpdateServerUrl}/updates/{AppId}_{version}.zip";
                string tempPath = Path.Combine(Path.GetTempPath(), $"update_{AppId}_{version}.zip");

                using (WebClient webClient = new WebClient())
                {
                    Debug.WriteLine($"开始下载更新包: {packageUrl}");
                    webClient.DownloadFile(packageUrl, tempPath);
                    Debug.WriteLine($"更新包下载完成: {tempPath}");
                }

                return tempPath;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"下载更新包失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 安装版本
        /// </summary>
        /// <param name="packagePath">更新包路径</param>
        /// <returns>安装是否成功</returns>
        private bool InstallVersion(string packagePath)
        {
            try
            {
                if (!File.Exists(packagePath))
                {
                    Debug.WriteLine("更新包文件不存在");
                    return false;
                }

                // 获取应用程序根目录
                string appRootDir = AppDomain.CurrentDomain.BaseDirectory;

                // 创建临时目录用于解压
                string tempExtractDir = Path.Combine(Path.GetTempPath(), $"update_extract_{Guid.NewGuid().ToString()}");
                Directory.CreateDirectory(tempExtractDir);

                try
                {
                    // 使用ZipArchive解压更新包
                    Debug.WriteLine($"开始解压更新包: {packagePath}");
                    using (FileStream zipFileStream = new FileStream(packagePath, FileMode.Open, FileAccess.Read))
                    using (ZipArchive archive = new ZipArchive(zipFileStream, ZipArchiveMode.Read))
                    {
                        archive.ExtractToDirectory(tempExtractDir);
                    }
                    Debug.WriteLine("更新包解压完成");

                    // 复制文件到应用程序目录
                    Debug.WriteLine("开始安装更新文件");
                    CopyDirectory(tempExtractDir, appRootDir);

                    return true;
                }
                finally
                {
                    // 清理临时目录
                    if (Directory.Exists(tempExtractDir))
                    {
                        Directory.Delete(tempExtractDir, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"安装版本失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="sourceDir">源目录</param>
        /// <param name="destinationDir">目标目录</param>
        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            Directory.CreateDirectory(destinationDir);

            // 复制文件
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                // 如果目标文件正在使用，尝试重命名旧文件后再复制
                if (File.Exists(destFile))
                {
                    try
                    {
                        File.Delete(destFile);
                    }
                    catch
                    {
                        // 如果删除失败，先重命名
                        string backupFile = destFile + ".bak";
                        if (File.Exists(backupFile))
                        {
                            File.Delete(backupFile);
                        }
                        File.Move(destFile, backupFile);
                    }
                }
                File.Copy(file, destFile, true);
            }

            // 递归复制子目录
            foreach (string dir in Directory.GetDirectories(sourceDir))
            {
                string destDir = Path.Combine(destinationDir, Path.GetFileName(dir));
                CopyDirectory(dir, destDir);
            }
        }

        /// <summary>
        /// 验证是否可以回滚
        /// </summary>
        /// <returns>是否可以回滚</returns>
        public bool CanRollback()
        {
            return versionHistoryManager.HasRollbackVersions();
        }
    }
}