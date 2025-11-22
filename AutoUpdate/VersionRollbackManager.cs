using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Linq;

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

                // 3. 从服务器获取目标版本的更新包
                string updatePackagePath = DownloadUpdatePackage(targetVersion);
                if (string.IsNullOrEmpty(updatePackagePath))
                {
                    Debug.WriteLine("下载目标版本更新包失败");
                    return false;
                }

                // 4. 解压并安装目标版本
                bool installSuccess = InstallVersion(updatePackagePath);
                if (!installSuccess)
                {
                    Debug.WriteLine("安装目标版本失败");
                    return false;
                }

                // 5. 更新版本历史记录
                versionHistoryManager.RecordNewVersion(targetVersion);
                Debug.WriteLine($"版本回滚成功，当前版本: {targetVersion}");

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"执行版本回滚失败: {ex.Message}");
                return false;
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
                    // 使用系统命令解压（简单实现，实际项目中可能需要使用专业解压库）
                    Debug.WriteLine($"开始解压更新包: {packagePath}");
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-Command \"Expand-Archive -Path '{packagePath}' -DestinationPath '{tempExtractDir}' -Force\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };

                    using (Process process = Process.Start(psi))
                    {
                        process.WaitForExit();
                        if (process.ExitCode != 0)
                        {
                            Debug.WriteLine("解压更新包失败");
                            return false;
                        }
                    }

                    // 复制文件到应用程序目录（简单实现，实际项目中可能需要更复杂的文件替换逻辑）
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