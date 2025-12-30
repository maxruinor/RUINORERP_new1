using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Cryptography;

namespace AutoUpdate
{
    /// <summary>
    /// 自身更新助手类
    /// 采用双进程机制实现可靠的自身更新
    /// </summary>
    public class SelfUpdateHelper
    {
        /// <summary>
        /// 启动AutoUpdateUpdater来更新AutoUpdate程序自身
        /// </summary>
        /// <param name="updaterExePath">当前更新程序的路径</param>
        /// <param name="newFilesPath">新文件所在的临时目录</param>
        /// <returns>是否成功启动更新器</returns>
        public static bool StartAutoUpdateUpdater(string updaterExePath, string newFilesPath)
        {
            try
            {
                // 查找AutoUpdateUpdater.exe
                string targetDir = Path.GetDirectoryName(updaterExePath);
                string autoUpdateUpdaterPath = Path.Combine(targetDir, "AutoUpdateUpdater.exe");
                
                if (!File.Exists(autoUpdateUpdaterPath))
                {
                    WriteLog("AutoUpdateLog.txt", $"AutoUpdateUpdater.exe不存在：{autoUpdateUpdaterPath}");
                    return false;
                }

                // 准备更新命令 - 使用等号格式确保参数解析一致性
                string arguments = $"--source-dir=\"{newFilesPath}\" --target-dir=\"{targetDir}\" --exe-name=\"{Path.GetFileName(updaterExePath)}\"";

                // 启动AutoUpdateUpdater
                WriteLog("AutoUpdateLog.txt", $"启动AutoUpdateUpdater，参数: {arguments}");
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = autoUpdateUpdaterPath,
                    Arguments = arguments,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                };

                Process updateProcess = Process.Start(startInfo);
                if (updateProcess != null)
                {
                    WriteLog("AutoUpdateLog.txt", "AutoUpdateUpdater已成功启动");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // 记录错误日志
                string logFilePath = Path.Combine(Path.GetDirectoryName(updaterExePath), "AutoUpdateLog.txt");
                try
                {
                    string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] 启动AutoUpdateUpdater失败: {ex.Message}\r\n堆栈跟踪: {ex.StackTrace}\r\n";
                    File.AppendAllText(logFilePath, logEntry);
                }
                catch { }
                return false;
            }
        }

        /// <summary>
        /// 执行自身更新逻辑
        /// </summary>
        /// <param name="args">命令行参数</param>
        public static void ExecuteSelfUpdate(string[] args)
        {
            // 解析命令行参数
            string sourceDir = string.Empty;
            string targetDir = string.Empty;
            string exeName = string.Empty;
            string checksum = string.Empty;

            WriteLog("AutoUpdateLog.txt", $"开始解析命令行参数，参数数量: {args.Length}");
            
            // 调试输出所有参数
            string allArgs = string.Join(" ", args.Select((arg, index) => $"[{index}]={arg}"));
            WriteLog("AutoUpdateLog.txt", $"所有参数: {allArgs}");
            
            // 简化参数解析逻辑，直接处理所有可能的格式
            for (int i = 0; i < args.Length; i++)
            {
                WriteLog("AutoUpdateLog.txt", $"解析参数[{i}]: {args[i]}");
                
                if (args[i] == "--self-update")
                {
                    WriteLog("AutoUpdateLog.txt", "检测到自我更新标志");
                    continue;
                }
                
                // 处理所有可能的参数格式
                string arg = args[i];
                if (arg.StartsWith("--source-dir"))
                {
                    if (arg == "--source-dir" && i + 1 < args.Length)
                    {
                        sourceDir = args[i + 1].Trim('"');
                        i++;
                        WriteLog("AutoUpdateLog.txt", $"解析到源目录(格式1): {sourceDir}");
                    }
                    else if (arg.StartsWith("--source-dir="))
                    {
                        sourceDir = arg.Substring("--source-dir=".Length).Trim('"');
                        WriteLog("AutoUpdateLog.txt", $"解析到源目录(格式2): {sourceDir}");
                    }
                }
                else if (arg.StartsWith("--target-dir"))
                {
                    if (arg == "--target-dir" && i + 1 < args.Length)
                    {
                        targetDir = args[i + 1].Trim('"');
                        i++;
                        WriteLog("AutoUpdateLog.txt", $"解析到目标目录(格式1): {targetDir}");
                    }
                    else if (arg.StartsWith("--target-dir="))
                    {
                        targetDir = arg.Substring("--target-dir=".Length).Trim('"');
                        WriteLog("AutoUpdateLog.txt", $"解析到目标目录(格式2): {targetDir}");
                    }
                }
                else if (arg.StartsWith("--exe-name"))
                {
                    if (arg == "--exe-name" && i + 1 < args.Length)
                    {
                        exeName = args[i + 1].Trim('"');
                        i++;
                        WriteLog("AutoUpdateLog.txt", $"解析到可执行文件名(格式1): {exeName}");
                    }
                    else if (arg.StartsWith("--exe-name="))
                    {
                        exeName = arg.Substring("--exe-name=".Length).Trim('"');
                        WriteLog("AutoUpdateLog.txt", $"解析到可执行文件名(格式2): {exeName}");
                    }
                }
                else if (arg.StartsWith("--checksum"))
                {
                    if (arg == "--checksum" && i + 1 < args.Length)
                    {
                        checksum = args[i + 1].Trim('"');
                        i++;
                        WriteLog("AutoUpdateLog.txt", $"解析到校验和(格式1): {checksum}");
                    }
                    else if (arg.StartsWith("--checksum="))
                    {
                        checksum = arg.Substring("--checksum=".Length).Trim('"');
                        WriteLog("AutoUpdateLog.txt", $"解析到校验和(格式2): {checksum}");
                    }
                }
            }

            // 验证参数
            WriteLog("AutoUpdateLog.txt", $"参数验证 - 源目录: {sourceDir}, 目标目录: {targetDir}, 可执行文件名: {exeName}");
            
            // 如果目标目录为空，尝试使用当前目录
            if (string.IsNullOrEmpty(targetDir))
            {
                targetDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                WriteLog("AutoUpdateLog.txt", $"使用当前目录作为目标目录: {targetDir}");
            }
            
            // 如果可执行文件名为空，尝试使用当前程序名
            if (string.IsNullOrEmpty(exeName))
            {
                exeName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
                WriteLog("AutoUpdateLog.txt", $"使用当前程序名作为可执行文件名: {exeName}");
            }
            
            if (string.IsNullOrEmpty(sourceDir) || string.IsNullOrEmpty(targetDir) || string.IsNullOrEmpty(exeName))
            {
                WriteLog("AutoUpdateLog.txt", "更新参数无效，无法执行自身更新");
                WriteLog("AutoUpdateLog.txt", $"最终参数 - 源目录: {sourceDir}, 目标目录: {targetDir}, 可执行文件名: {exeName}");
                
                // 更新失败时，确保启动主程序
                StartERPApplication(targetDir);
                return;
            }
            
            // 验证目录是否存在
            if (!Directory.Exists(sourceDir))
            {
                WriteLog("AutoUpdateLog.txt", $"源目录不存在: {sourceDir}");
                
                // 更新失败时，确保启动主程序
                StartERPApplication(targetDir);
                return;
            }
            
            if (!Directory.Exists(targetDir))
            {
                WriteLog("AutoUpdateLog.txt", $"目标目录不存在: {targetDir}");
                
                // 更新失败时，确保启动主程序
                StartERPApplication(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
                return;
            }

            // 日志文件路径
            string logFilePath = Path.Combine(targetDir, "AutoUpdateLog.txt");
            WriteLog(logFilePath, "开始执行自身更新...");

            // 等待主进程退出，采用逐步等待策略
            WriteLog(logFilePath, "等待主进程退出...");
            
            // 尝试等待多次，每次间隔检查进程状态
            int maxWaitAttempts = 10;
            int waitInterval = 500; // 500毫秒
            
            for (int i = 0; i < maxWaitAttempts; i++)
            {
                Thread.Sleep(waitInterval);
                
                // 检查主进程是否已经退出
                string currentProcessName = Path.GetFileNameWithoutExtension(exeName);
                Process[] runningProcesses = Process.GetProcessesByName(currentProcessName);
                
                // 排除当前进程（自我更新进程）
                runningProcesses = runningProcesses.Where(p => p.Id != Process.GetCurrentProcess().Id).ToArray();
                
                if (runningProcesses.Length == 0)
                {
                    WriteLog(logFilePath, $"主进程已退出，等待时间: {(i + 1) * waitInterval}ms");
                    break;
                }
                else
                {
                    WriteLog(logFilePath, $"等待主进程退出... 剩余进程数: {runningProcesses.Length}, 尝试次数: {i + 1}");
                }
                
                if (i == maxWaitAttempts - 1)
                {
                    WriteLog(logFilePath, "警告: 主进程未完全退出，但将继续执行更新");
                }
            }

            try
            {
                // 1. 验证更新包完整性
                if (!string.IsNullOrEmpty(checksum))
                {
                    WriteLog(logFilePath, $"验证更新包完整性，预期校验和: {checksum}");
                    string actualChecksum = CalculateDirectoryChecksum(sourceDir);
                    if (actualChecksum != checksum)
                    {
                        WriteLog(logFilePath, $"更新包完整性验证失败，实际校验和: {actualChecksum}");
                        return;
                    }
                    WriteLog(logFilePath, "更新包完整性验证通过");
                }

                // 2. 备份当前版本
                string backupDir = Path.Combine(targetDir, "Backup_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                WriteLog(logFilePath, $"开始备份当前版本到: {backupDir}");
                BackupCurrentVersion(targetDir, backupDir, exeName);
                WriteLog(logFilePath, "当前版本备份完成");

                try
                {
                    // 3. 执行文件替换
                    ReplaceFiles(sourceDir, targetDir);
                    WriteLog(logFilePath, "文件替换完成");

                    // 4. 验证更新结果
                    string mainExePath = Path.Combine(targetDir, exeName);
                    if (File.Exists(mainExePath))
                    {
                        WriteLog(logFilePath, $"更新完成，准备重启: {mainExePath}");
                        
                        // 5. 更新版本记录
                        UpdateVersionRecord(targetDir, sourceDir);
                        
                        // 6. 启动ERP系统（如果存在）
                        StartERPApplication(targetDir);
                        
                        // 7. 重启主进程
                        Process.Start(mainExePath);
                    }
                    else
                    {
                        throw new FileNotFoundException($"更新失败，主程序文件不存在: {mainExePath}");
                    }
                }
                catch (Exception ex)
                {
                    // 7. 更新失败，执行回滚
                    WriteLog(logFilePath, $"更新失败，开始回滚到备份版本: {ex.Message}\r\n堆栈跟踪: {ex.StackTrace}");
                    RollbackToBackup(targetDir, backupDir);
                    WriteLog(logFilePath, "回滚完成，重启应用程序");
                    
                    // 重启主进程
                    string mainExePath = Path.Combine(targetDir, exeName);
                    if (File.Exists(mainExePath))
                    {
                        Process.Start(mainExePath);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(logFilePath, $"执行更新失败: {ex.Message}\r\n堆栈跟踪: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 替换文件
        /// </summary>
        /// <param name="sourceDir">源目录</param>
        /// <param name="targetDir">目标目录</param>
        private static void ReplaceFiles(string sourceDir, string targetDir)
        {
            string logFilePath = Path.Combine(targetDir, "AutoUpdateLog.txt");
            WriteLog(logFilePath, $"开始替换文件，源目录: {sourceDir}, 目标目录: {targetDir}");

            // 获取所有文件
            string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
            WriteLog(logFilePath, $"找到 {files.Length} 个文件需要替换");

            foreach (string file in files)
            {
                try
                {
                    // 计算相对路径
                    string relativePath = file.Substring(sourceDir.Length).Trim(Path.DirectorySeparatorChar);
                    string targetFilePath = Path.Combine(targetDir, relativePath);

                    // 确保目标目录存在
                    string targetFileDir = Path.GetDirectoryName(targetFilePath);
                    if (!Directory.Exists(targetFileDir))
                    {
                        Directory.CreateDirectory(targetFileDir);
                        WriteLog(logFilePath, $"创建目录: {targetFileDir}");
                    }

                    // 如果目标文件存在，先尝试删除
                    if (File.Exists(targetFilePath))
                    {
                        try
                        {
                            File.Delete(targetFilePath);
                            WriteLog(logFilePath, $"删除旧文件: {targetFilePath}");
                        }
                        catch (IOException)
                        {
                            // 文件可能被锁定，尝试重命名后删除
                            string tempPath = targetFilePath + ".old";
                            if (File.Exists(tempPath))
                            {
                                File.Delete(tempPath);
                            }
                            File.Move(targetFilePath, tempPath);
                            WriteLog(logFilePath, $"重命名旧文件: {targetFilePath} -> {tempPath}");
                        }
                    }

                    // 复制新文件
                    File.Copy(file, targetFilePath, true);
                    WriteLog(logFilePath, $"复制新文件: {file} -> {targetFilePath}");
                }
                catch (Exception ex)
                {
                    WriteLog(logFilePath, $"替换文件失败: {file}\r\n错误信息: {ex.Message}");
                }
            }

            WriteLog(logFilePath, "文件替换完成");
        }

        /// <summary>
        /// 写入更新日志
        /// </summary>
        /// <param name="logFilePath">日志文件路径</param>
        /// <param name="message">日志内容</param>
        private static void WriteLog(string logFilePath, string message)
        {
            try
            {
                // 确保日志文件路径有效
                if (string.IsNullOrEmpty(logFilePath))
                {
                    logFilePath = "AutoUpdateLog.txt";
                }
                
                // 如果 logFilePath 不包含路径，则使用当前目录
                if (!Path.IsPathRooted(logFilePath))
                {
                    string currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                    logFilePath = Path.Combine(currentDir, logFilePath);
                }
                
                // 确保日志目录存在
                string logDir = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [UPDATE] {message}\r\n";
                File.AppendAllText(logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                // 如果写入日志失败，尝试在控制台输出
                Console.WriteLine($"[ERROR] 写入日志失败: {ex.Message}");
                Console.WriteLine($"[DEBUG] 消息内容: {message}");
            }
        }

        /// <summary>
        /// 启动ERP系统应用程序
        /// </summary>
        /// <param name="targetDir">目标目录</param>
        private static void StartERPApplication(string targetDir)
        {
            try
            {
                WriteLog("AutoUpdateLog.txt", $"[ERP启动] 开始启动ERP系统应用程序...");
                
                // 尝试从配置文件获取入口程序路径
                string configFile = Path.Combine(targetDir, "AutoUpdaterList.xml");
                string mainAppExe = "企业数字化集成ERP.exe";
                
                if (File.Exists(configFile))
                {
                    try
                    {
                        var xmlFiles = new XmlFiles(configFile);
                        string entryPoint = xmlFiles.GetNodeValue("//EntryPoint");
                        if (!string.IsNullOrEmpty(entryPoint))
                        {
                            mainAppExe = entryPoint;
                            WriteLog("AutoUpdateLog.txt", $"[ERP启动] 从配置文件读取主程序路径: {mainAppExe}");
                        }
                        else
                        {
                            WriteLog("AutoUpdateLog.txt", $"[ERP启动] 使用默认主程序路径: {mainAppExe}");
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog("AutoUpdateLog.txt", $"[ERP启动] 读取配置文件失败: {ex.Message}，使用默认路径");
                    }
                }
                
                // 确保路径是完整的绝对路径
                if (!Path.IsPathRooted(mainAppExe))
                {
                    mainAppExe = Path.Combine(targetDir, mainAppExe);
                    WriteLog("AutoUpdateLog.txt", $"[ERP启动] 转换为绝对路径: {mainAppExe}");
                }
                
                // 检查程序是否存在
                if (File.Exists(mainAppExe))
                {
                    WriteLog("AutoUpdateLog.txt", $"[ERP启动] 找到主程序文件，准备启动...");
                    
                    // 启动进程
                    ProcessStartInfo startInfo = new ProcessStartInfo(mainAppExe);
                    startInfo.WorkingDirectory = Path.GetDirectoryName(mainAppExe);
                    startInfo.UseShellExecute = true;
                    
                    // 添加启动参数
                    string arguments = $"--updated-from-auto-update";
                    startInfo.Arguments = arguments;
                    
                    WriteLog("AutoUpdateLog.txt", $"[ERP启动] 工作目录: {startInfo.WorkingDirectory}");
                    WriteLog("AutoUpdateLog.txt", $"[ERP启动] 启动参数: {arguments}");
                    
                    Process process = Process.Start(startInfo);
                    
                    if (process != null && !process.HasExited)
                    {
                        WriteLog("AutoUpdateLog.txt", $"[ERP启动] ERP系统启动成功，进程ID: {process.Id}");
                        WriteLog("AutoUpdateLog.txt", $"[ERP启动] 进程名称: {process.ProcessName}");
                        
                        // 等待进程启动完成
                        Thread.Sleep(2000);
                        
                        // 检查进程是否仍在运行
                        if (!process.HasExited)
                        {
                            WriteLog("AutoUpdateLog.txt", "[ERP启动] ERP系统已成功启动并正在运行");
                        }
                        else
                        {
                            WriteLog("AutoUpdateLog.txt", "[ERP启动] 警告: ERP系统启动后立即退出");
                        }
                    }
                    else
                    {
                        WriteLog("AutoUpdateLog.txt", "[ERP启动] 错误: 无法启动ERP系统进程");
                    }
                }
                else
                {
                    WriteLog("AutoUpdateLog.txt", $"[ERP启动] 错误: 主程序文件不存在: {mainAppExe}");
                }
            }
            catch (Exception ex)
            {
                WriteLog("AutoUpdateLog.txt", $"[ERP启动] ERP系统启动失败: {ex.Message}");
                WriteLog("AutoUpdateLog.txt", $"[ERP启动] 异常详情: {ex.StackTrace}");
            }
        }
        
        /// <summary>
        /// 检查是否需要执行自身更新
        /// </summary>
        /// <param name="args">命令行参数</param>
        /// <returns>是否需要执行自身更新</returns>
        public static bool IsSelfUpdateRequested(string[] args)
        {
            return args != null && args.Contains("--self-update");
        }
        
        /// <summary>
        /// 计算目录的校验和
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>目录校验和</returns>
        private static string CalculateDirectoryChecksum(string directoryPath)
        {
            try
            {
                // 获取目录中的所有文件，按路径排序
                string[] files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
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
                Debug.WriteLine($"计算目录校验和失败: {directoryPath}, 错误: {ex.Message}");
                return string.Empty;
            }
        }
        
        /// <summary>
        /// 备份当前版本
        /// </summary>
        /// <param name="currentDir">当前版本目录</param>
        /// <param name="backupDir">备份目录</param>
        /// <param name="exeName">主程序名称</param>
        private static void BackupCurrentVersion(string currentDir, string backupDir, string exeName)
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
        /// 回滚到备份版本
        /// </summary>
        /// <param name="targetDir">目标目录</param>
        /// <param name="backupDir">备份目录</param>
        private static void RollbackToBackup(string targetDir, string backupDir)
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
                
                // 清理备份目录
                Directory.Delete(backupDir, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"回滚到备份版本失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 更新版本记录
        /// </summary>
        /// <param name="targetDir">目标目录</param>
        /// <param name="sourceDir">源目录</param>
        private static void UpdateVersionRecord(string targetDir, string sourceDir)
        {
            try
            {
                // 获取当前版本信息
                string currentVersion = File.ReadAllText(Path.Combine(targetDir, "CurrentVersion.txt"), Encoding.UTF8);
                
                // 创建版本文件夹管理器
                VersionFolderManager folderManager = new VersionFolderManager();
                VersionHistoryManager historyManager = new VersionHistoryManager();
                
                // 创建版本文件夹
                string folderName = folderManager.CreateVersionFolder(currentVersion);
                
                // 复制更新后的核心文件到版本文件夹
                string[] coreFiles = Directory.GetFiles(targetDir, "AutoUpdate.*")
                    .Where(file => Path.GetExtension(file) == ".exe" || Path.GetExtension(file) == ".dll" || Path.GetExtension(file) == ".config")
                    .ToArray();
                
                foreach (string file in coreFiles)
                {
                    folderManager.CopyFileToVersionFolder(file, folderName);
                }
                
                // 获取版本文件列表和校验和
                List<string> files = folderManager.GetVersionFiles(folderName);
                string checksum = folderManager.CalculateVersionChecksum(folderName);
                
                // 记录版本信息
                historyManager.RecordNewVersion(currentVersion, folderName, files, checksum);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"更新版本记录失败: {ex.Message}");
            }
        }
    }
}