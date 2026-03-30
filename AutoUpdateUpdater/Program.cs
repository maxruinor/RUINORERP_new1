using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AutoUpdateUpdater
{
    /// <summary>
    /// AutoUpdateUpdater - 专门用于更新AutoUpdate.exe的程序
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // 记录启动日志
            WriteLog("AutoUpdateUpdaterLog.txt", $"AutoUpdateUpdater启动，参数数量: {args.Length}");
            
            try
            {
                // 获取当前目录
                string currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                string configFilePath = Path.Combine(currentDir, "AutoUpdateUpdaterConfig.json");
                
                // 优先使用配置文件方式
                UpdateConfig config = LoadConfigFromFile(configFilePath);
                
                if (!config.IsValid() && args.Length > 0)
                {
                    // 如果配置文件无效，尝试解析命令行参数作为备用方案
                    WriteLog("AutoUpdateUpdaterLog.txt", "配置文件无效，尝试解析命令行参数...");
                    config = ParseCommandLineArguments(args);
                    
                    // 如果命令行参数有效，保存到配置文件
                    if (config.IsValid())
                    {
                        config.SaveToFile(configFilePath);
                    }
                }
                

                
                // 参数验证
                if (!config.IsValid())
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", "错误: 更新配置无效");
                    MessageBox.Show("更新参数无效，无法执行更新，将尝试启动ERP主程序", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StartERPApplication();
                    return;
                }
                
                string sourceDir = config.SourceDir;
                string targetDir = config.TargetDir;
                string exeName = config.ExeName;
                
                // 执行更新
                bool success = ExecuteUpdate(sourceDir, targetDir, exeName, config.AutoUpdateExePath);
                
                if (success)
                {
                    // 更新成功，清理配置文件
                    CleanupConfigFile(configFilePath);
                    WriteLog("AutoUpdateUpdaterLog.txt", "AutoUpdate更新成功，准备启动ERP主程序");
                    
                    // 更新成功后直接启动ERP主程序，而不是AutoUpdate.exe
                    StartERPApplication();
                    
                    Application.Exit();
                }
                else
                {
                    // 更新失败或不需要更新，清理配置文件
                    CleanupConfigFile(configFilePath);
                    WriteLog("AutoUpdateUpdaterLog.txt", "更新失败或不需要更新，清理配置文件");
                    // 确保ERP主程序被启动
                    StartERPApplication();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新过程中发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 执行更新操作
        /// </summary>
        private static bool ExecuteUpdate(string sourceDir, string targetDir, string exeName, string autoUpdateExePath = "")
        {
            try
            {
                // 检查源目录是否存在
                if (!Directory.Exists(sourceDir))
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"源目录不存在：{sourceDir}，跳过更新");
                    // 如果源目录不存在，可能是不需要更新，直接启动ERP主程序
                    StartERPApplication();
                    return false;
                }
                
                // 检查目标目录是否存在
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
                
                // 查找源文件（支持版本子目录）
                string sourceFile = FindSourceFile(sourceDir, exeName);
                if (!File.Exists(sourceFile))
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"源文件不存在：{sourceFile}，跳过更新");
                    // 如果源文件不存在，可能是不需要更新，直接启动ERP主程序
                    StartERPApplication();
                    return false;
                }
                
                // 计算源文件和目标文件的校验和，判断是否真的需要更新
                string targetFile = Path.Combine(targetDir, exeName);
                if (File.Exists(targetFile))
                {
                    // 比较文件大小和修改时间，如果相同则跳过更新
                    var sourceInfo = new FileInfo(sourceFile);
                    var targetInfo = new FileInfo(targetFile);
                    
                    if (sourceInfo.Length == targetInfo.Length && 
                        sourceInfo.LastWriteTime <= targetInfo.LastWriteTime)
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"目标文件已是最新版本，跳过更新: {targetFile}");
                        // 更新完成，启动ERP主程序
                        StartERPApplication();
                        return true; // 返回true表示操作成功（即使没有实际更新）
                    }
                }
                
                // 如果指定了AutoUpdate.exe的直接路径，优先使用该路径
                if (!string.IsNullOrEmpty(autoUpdateExePath))
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"使用直接指定的AutoUpdate.exe路径: {autoUpdateExePath}");
                }
                
                // 如果目标文件存在，先尝试关闭正在运行的AutoUpdate进程
                if (File.Exists(targetFile))
                {
                    string processName = Path.GetFileNameWithoutExtension(exeName);
                    
                    // 【增强】增加等待时间到15秒，确保进程完全退出
                    WriteLog("AutoUpdateUpdaterLog.txt", $"[文件更新] 开始等待AutoUpdate进程完全退出...");
                    if (!WaitAndKillProcess(processName, 15000))
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"[文件更新] 警告: 进程关闭超时，但将继续尝试更新");
                        // 不再直接返回false，而是继续尝试更新
                    }
                    
                    // 【新增】额外等待，确保文件句柄完全释放
                    WriteLog("AutoUpdateUpdaterLog.txt", $"[文件更新] 额外等待3秒，确保文件句柄释放...");
                    Thread.Sleep(3000);
                    
                    // 备份原文件（带重试机制）
                    string backupFile = targetFile + ".bak";
                    bool backupSuccess = SafeFileOperation(() =>
                    {
                        if (File.Exists(backupFile))
                        {
                            File.Delete(backupFile);
                        }
                        File.Move(targetFile, backupFile);
                    }, "备份文件", 5);
                    
                    if (!backupSuccess)
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"[文件更新] 备份文件失败，尝试直接删除目标文件");
                        // 尝试删除目标文件
                        SafeFileOperation(() => File.Delete(targetFile), "删除目标文件", 5);
                    }
                }
                
                // 【增强】复制新文件，带重试机制
                WriteLog("AutoUpdateUpdaterLog.txt", $"[文件更新] 开始复制新文件: {sourceFile} -> {targetFile}");
                bool copySuccess = SafeFileOperation(() => File.Copy(sourceFile, targetFile, true), "复制新文件", 10);
                
                if (!copySuccess)
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"[文件更新] 直接复制失败，尝试使用临时文件方式");
                    
                    // 使用临时文件方式复制
                    string tempFile = targetFile + ".temp";
                    SafeFileOperation(() => File.Copy(sourceFile, tempFile, true), "复制到临时文件", 5);
                    
                    // 删除目标文件
                    SafeFileOperation(() => File.Delete(targetFile), "删除目标文件", 5);
                    
                    // 重命名临时文件
                    SafeFileOperation(() => File.Move(tempFile, targetFile), "重命名临时文件", 5);
                }
                
                // 验证文件复制成功
                if (File.Exists(targetFile))
                {
                    // 清理备份文件
                    string backupFile = targetFile + ".bak";
                    if (File.Exists(backupFile))
                    {
                        File.Delete(backupFile);
                    }
                    
                    return true;
                }
                else
                {
                    // 恢复备份
                    string backupFile = targetFile + ".bak";
                    if (File.Exists(backupFile))
                    {
                        File.Move(backupFile, targetFile);
                    }
                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新操作失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        /// <summary>
        /// 等待并关闭指定进程
        /// 【修复】增加更激进的进程关闭策略，确保AutoUpdate完全退出
        /// </summary>
        private static bool WaitAndKillProcess(string processName, int timeoutMs)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                int checkCount = 0;
                
                WriteLog("AutoUpdateUpdaterLog.txt", $"[进程管理] 开始等待进程退出: {processName}, 超时时间: {timeoutMs}ms");
                
                while ((DateTime.Now - startTime).TotalMilliseconds < timeoutMs)
                {
                    checkCount++;
                    Process[] processes = Process.GetProcessesByName(processName);
                    
                    // 排除当前进程
                    int currentProcessId = Process.GetCurrentProcess().Id;
                    processes = processes.Where(p => p.Id != currentProcessId).ToArray();
                    
                    if (processes.Length == 0)
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"[进程管理] 进程已完全退出: {processName}, 检查次数: {checkCount}");
                        // 额外等待确保资源完全释放
                        Thread.Sleep(1000);
                        return true;
                    }
                    
                    WriteLog("AutoUpdateUpdaterLog.txt", $"[进程管理] 发现 {processes.Length} 个进程仍在运行, 检查次数: {checkCount}");
                    
                    // 尝试关闭进程 - 使用更激进的策略
                    foreach (Process process in processes)
                    {
                        try
                        {
                            if (!process.HasExited)
                            {
                                WriteLog("AutoUpdateUpdaterLog.txt", $"[进程管理] 尝试关闭进程: {process.ProcessName} (ID: {process.Id})");
                                
                                // 首先尝试优雅关闭
                                process.CloseMainWindow();
                                
                                // 等待进程响应
                                if (process.WaitForExit(500))
                                {
                                    WriteLog("AutoUpdateUpdaterLog.txt", $"[进程管理] 进程正常退出: {process.ProcessName}");
                                    continue;
                                }
                                
                                // 如果优雅关闭失败，强制终止
                                WriteLog("AutoUpdateUpdaterLog.txt", $"[进程管理] 强制终止进程: {process.ProcessName}");
                                process.Kill();
                                process.WaitForExit(1000);
                                WriteLog("AutoUpdateUpdaterLog.txt", $"[进程管理] 进程已强制终止: {process.ProcessName}");
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog("AutoUpdateUpdaterLog.txt", $"[进程管理] 关闭进程时出错: {ex.Message}");
                        }
                    }
                    
                    Thread.Sleep(300);
                }
                
                // 最终检查
                Process[] finalCheck = Process.GetProcessesByName(processName);
                finalCheck = finalCheck.Where(p => p.Id != Process.GetCurrentProcess().Id).ToArray();
                
                if (finalCheck.Length == 0)
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", "[进程管理] 最终检查：进程已完全退出");
                    Thread.Sleep(1000);
                    return true;
                }
                else
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"[进程管理] 警告: 等待超时，仍有 {finalCheck.Length} 个进程在运行");
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteLog("AutoUpdateUpdaterLog.txt", $"[进程管理] 异常: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 从配置文件加载配置
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <returns>配置对象</returns>
        private static UpdateConfig LoadConfigFromFile(string configFilePath)
        {
            try
            {
                if (File.Exists(configFilePath))
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"从配置文件加载配置: {configFilePath}");
                    return UpdateConfig.LoadFromFile(configFilePath);
                }
                else
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"配置文件不存在: {configFilePath}");
                }
            }
            catch (Exception ex)
            {
                WriteLog("AutoUpdateUpdaterLog.txt", $"加载配置文件失败: {ex.Message}");
            }
            
            return new UpdateConfig();
        }

        /// <summary>
        /// 解析命令行参数
        /// </summary>
        /// <param name="args">命令行参数</param>
        /// <returns>配置对象</returns>
        private static UpdateConfig ParseCommandLineArguments(string[] args)
        {
            UpdateConfig config = new UpdateConfig();
            
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string currentArg = args[i].Trim('"');
                    
                    // 处理格式1: --source-dir "path"
                    if (currentArg == "--source-dir" && i + 1 < args.Length)
                    {
                        config.SourceDir = args[i + 1].Trim('"');
                        i++;
                    }
                    // 处理格式2: --source-dir=path
                    else if (currentArg.StartsWith("--source-dir="))
                    {
                        config.SourceDir = currentArg.Substring("--source-dir=".Length).Trim('"');
                    }
                    // 处理格式1: --target-dir "path"
                    else if (currentArg == "--target-dir" && i + 1 < args.Length)
                    {
                        config.TargetDir = args[i + 1].Trim('"');
                        i++;
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到目标目录(格式1): {config.TargetDir}");
                    }
                    // 处理格式2: --target-dir=path
                    else if (currentArg.StartsWith("--target-dir="))
                    {
                        config.TargetDir = currentArg.Substring("--target-dir=".Length).Trim('"');
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到目标目录(格式2): {config.TargetDir}");
                    }
                    // 处理格式1: --exe-name "name"
                    else if (currentArg == "--exe-name" && i + 1 < args.Length)
                    {
                        config.ExeName = args[i + 1].Trim('"');
                        i++;
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到可执行文件名(格式1): {config.ExeName}");
                    }
                    // 处理格式2: --exe-name=name
                    else if (currentArg.StartsWith("--exe-name="))
                    {
                        config.ExeName = currentArg.Substring("--exe-name=".Length).Trim('"');
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到可执行文件名(格式2): {config.ExeName}");
                    }
                    else
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"未识别的参数格式: {currentArg}");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("AutoUpdateUpdaterLog.txt", $"解析命令行参数失败: {ex.Message}");
            }
            
            return config;
        }

        /// <summary>
        /// 清理配置文件
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        private static void CleanupConfigFile(string configFilePath)
        {
            try
            {
                if (File.Exists(configFilePath))
                {
                    File.Delete(configFilePath);
                    WriteLog("AutoUpdateUpdaterLog.txt", $"已清理配置文件: {configFilePath}");
                }
            }
            catch (Exception ex)
            {
                WriteLog("AutoUpdateUpdaterLog.txt", $"清理配置文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 写入日志文件
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
                    logFilePath = "AutoUpdateUpdaterLog.txt";
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
                
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}\r\n";
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
        /// 查找源文件（支持版本子目录，优先按创建时间选择最新的文件）
        /// </summary>
        /// <param name="sourceDir">源目录</param>
        /// <param name="exeName">可执行文件名</param>
        /// <returns>源文件路径</returns>
        private static string FindSourceFile(string sourceDir, string exeName)
        {
            try
            {
                // 首先尝试直接在源目录中查找
                string directPath = Path.Combine(sourceDir, exeName);
                if (File.Exists(directPath))
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"找到源文件（直接路径）: {directPath}");
                    return directPath;
                }
                
                // 查找子目录中的文件（处理版本目录）
                string[] subDirectories = Directory.GetDirectories(sourceDir);
                
                // 收集所有找到的版本文件路径及其信息
                List<FileInfoWithTime> foundFiles = new List<FileInfoWithTime>();
                
                foreach (string subDir in subDirectories)
                {
                    string versionPath = Path.Combine(subDir, exeName);
                    if (File.Exists(versionPath))
                    {
                        var fileInfo = new FileInfo(versionPath);
                        foundFiles.Add(new FileInfoWithTime
                        {
                            FilePath = versionPath,
                            CreationTime = fileInfo.CreationTime,
                            LastWriteTime = fileInfo.LastWriteTime,
                            Version = ExtractVersionFromPath(versionPath)
                        });
                        WriteLog("AutoUpdateUpdaterLog.txt", $"找到源文件（版本目录）: {versionPath}");
                    }
                }
                
                // 如果找到多个版本文件，选择最新的
                if (foundFiles.Count > 1)
                {
                    // 优先按最后修改时间排序，其次按创建时间，最后按版本号
                    var sortedFiles = foundFiles
                        .OrderByDescending(f => f.LastWriteTime)
                        .ThenByDescending(f => f.CreationTime)
                        .ThenByDescending(f => f.Version)
                        .ToList();
                    
                    string latestPath = sortedFiles.First().FilePath;
                    var latestFile = sortedFiles.First();
                    WriteLog("AutoUpdateUpdaterLog.txt", $"找到 {foundFiles.Count} 个版本文件，选择最新的: {latestPath}");
                    WriteLog("AutoUpdateUpdaterLog.txt", $"最新文件信息 - 最后修改时间: {latestFile.LastWriteTime}, 创建时间: {latestFile.CreationTime}, 版本号: {latestFile.Version}");
                    
                    return latestPath;
                }
                else if (foundFiles.Count == 1)
                {
                    // 只有一个版本文件，直接返回
                    var fileInfo = foundFiles.First();
                    WriteLog("AutoUpdateUpdaterLog.txt", $"找到唯一版本文件: {fileInfo.FilePath}");
                    WriteLog("AutoUpdateUpdaterLog.txt", $"文件信息 - 最后修改时间: {fileInfo.LastWriteTime}, 创建时间: {fileInfo.CreationTime}, 版本号: {fileInfo.Version}");
                    return fileInfo.FilePath;
                }
                
                // 如果都找不到，返回原始路径
                WriteLog("AutoUpdateUpdaterLog.txt", $"未找到源文件，使用默认路径: {directPath}");
                return directPath;
            }
            catch (Exception ex)
            {
                WriteLog("AutoUpdateUpdaterLog.txt", $"查找源文件失败: {ex.Message}");
                return Path.Combine(sourceDir, exeName);
            }
        }
        
        /// <summary>
        /// 文件信息类，包含路径和时间信息
        /// </summary>
        private class FileInfoWithTime
        {
            public string FilePath { get; set; }
            public DateTime CreationTime { get; set; }
            public DateTime LastWriteTime { get; set; }
            public string Version { get; set; }
        }
        
        /// <summary>
        /// 从路径中提取版本号
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>版本号字符串</returns>
        private static string ExtractVersionFromPath(string filePath)
        {
            try
            {
                // 获取文件所在目录的名称，通常版本号在目录名中
                string directoryName = Path.GetFileName(Path.GetDirectoryName(filePath));
                
                if (string.IsNullOrEmpty(directoryName))
                {
                    return "0";
                }
                
                // 尝试从目录名中提取版本号
                // 版本号格式通常为：1.0.0.0 或 1.0.0.0_20240101
                string versionPattern = @"\d+(\.\d+)*";
                var match = System.Text.RegularExpressions.Regex.Match(directoryName, versionPattern);
                
                if (match.Success)
                {
                    return match.Value;
                }
                
                // 如果没有匹配到标准版本号格式，返回目录名本身
                return directoryName;
            }
            catch (Exception ex)
            {
                WriteLog("AutoUpdateUpdaterLog.txt", $"提取版本号失败: {ex.Message}");
                return "0";
            }
        }
        
        /// <summary>
        /// 比较两个版本号的大小
        /// </summary>
        /// <param name="version1">版本号1</param>
        /// <param name="version2">版本号2</param>
        /// <returns>比较结果（-1：version1 < version2，0：相等，1：version1 > version2）</returns>
        private static int CompareVersions(string version1, string version2)
        {
            try
            {
                // 将版本号拆分为数字数组
                var v1Parts = version1.Split('.').Select(p => int.Parse(p)).ToArray();
                var v2Parts = version2.Split('.').Select(p => int.Parse(p)).ToArray();
                
                // 比较每个部分
                int maxLength = Math.Max(v1Parts.Length, v2Parts.Length);
                
                for (int i = 0; i < maxLength; i++)
                {
                    int v1Part = i < v1Parts.Length ? v1Parts[i] : 0;
                    int v2Part = i < v2Parts.Length ? v2Parts[i] : 0;
                    
                    if (v1Part != v2Part)
                    {
                        return v1Part.CompareTo(v2Part);
                    }
                }
                
                return 0; // 版本号相等
            }
            catch (Exception ex)
            {
                WriteLog("AutoUpdateUpdaterLog.txt", $"比较版本号失败: {ex.Message}");
                
                // 如果无法解析版本号，使用字符串比较
                return string.Compare(version1, version2, StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// 【修复】读取配置文件获取主程序路径
        /// 增加重试机制和错误处理
        /// </summary>
        private static string GetMainAppPathFromConfig(string currentDir)
        {
            string configFile = Path.Combine(currentDir, "AutoUpdaterList.xml");
            string defaultAppExe = "企业数字化集成ERP.exe";

            // 重试3次
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (File.Exists(configFile))
                    {
                        var xmlDoc = new System.Xml.XmlDocument();
                        xmlDoc.Load(configFile);

                        var entryPointNode = xmlDoc.SelectSingleNode("//EntryPoint");
                        if (entryPointNode != null && !string.IsNullOrEmpty(entryPointNode.InnerText))
                        {
                            string mainAppExe = entryPointNode.InnerText;
                            WriteLog("AutoUpdateUpdaterLog.txt", $"[配置读取] 成功获取主程序路径: {mainAppExe} (尝试{i + 1})");
                            return mainAppExe;
                        }
                        else
                        {
                            WriteLog("AutoUpdateUpdaterLog.txt", $"[配置读取] EntryPoint节点为空，使用默认值");
                        }
                    }
                    else
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"[配置读取] 配置文件不存在，等待后重试... (尝试{i + 1})");
                        Thread.Sleep(500); // 等待500ms后重试
                    }
                }
                catch (Exception ex)
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"[配置读取] 异常: {ex.Message} (尝试{i + 1})");
                    Thread.Sleep(500);
                }
            }

            WriteLog("AutoUpdateUpdaterLog.txt", $"[配置读取] 使用默认主程序路径: {defaultAppExe}");
            return defaultAppExe;
        }

        /// <summary>
        /// 启动ERP系统应用程序
        /// 【修复】增加等待AutoUpdate进程退出的逻辑，确保资源完全释放
        /// </summary>
        private static void StartERPApplication()
        {
            try
            {
                WriteLog("AutoUpdateUpdaterLog.txt", "开始启动ERP系统应用程序...");

                // 【新增】等待AutoUpdate进程完全退出
                WaitForAutoUpdateExit();

                // 获取当前目录
                string currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

                // 【修复】使用新的配置读取方法，增加重试机制
                string mainAppExe = GetMainAppPathFromConfig(currentDir);

                // 确保路径是完整的绝对路径
                if (!Path.IsPathRooted(mainAppExe))
                {
                    mainAppExe = Path.Combine(currentDir, mainAppExe);
                    WriteLog("AutoUpdateUpdaterLog.txt", $"转换为绝对路径: {mainAppExe}");
                }
                
                // 检查程序是否存在
                if (File.Exists(mainAppExe))
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"找到主程序文件，准备启动: {mainAppExe}");
                    
                    // 启动进程
                    ProcessStartInfo startInfo = new ProcessStartInfo(mainAppExe);
                    startInfo.WorkingDirectory = Path.GetDirectoryName(mainAppExe);
                    startInfo.UseShellExecute = true;
                    
                    // 【修复】统一使用--updated参数
                    startInfo.Arguments = "--updated";
                    
                    WriteLog("AutoUpdateUpdaterLog.txt", $"工作目录: {startInfo.WorkingDirectory}");
                    WriteLog("AutoUpdateUpdaterLog.txt", $"启动参数: {startInfo.Arguments}");
                    
                    Process process = Process.Start(startInfo);
                    
                    if (process != null)
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"ERP系统启动成功，进程ID: {process.Id}");
                    }
                    else
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", "警告: 启动ERP系统进程返回null");
                    }
                }
                else
                {
                    string errorMsg = $"ERP主程序文件不存在: {mainAppExe}";
                    WriteLog("AutoUpdateUpdaterLog.txt", errorMsg);
                    MessageBox.Show(errorMsg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"启动ERP系统失败: {ex.Message}";
                WriteLog("AutoUpdateUpdaterLog.txt", errorMsg);
                WriteLog("AutoUpdateUpdaterLog.txt", $"异常详情: {ex.StackTrace}");
                MessageBox.Show(errorMsg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 【修复】等待AutoUpdate进程退出
        /// 确保AutoUpdate完全退出后再启动主程序，避免"程序已经在运行中"错误
        /// 【关键修改】增加更长的等待时间和更激进的进程关闭策略
        /// </summary>
        private static void WaitForAutoUpdateExit()
        {
            try
            {
                string[] autoUpdateProcessNames = new[] { "AutoUpdate", "AutoUpdate.vshost" };
                int maxWaitTime = 20000; // 最大等待20秒（增加等待时间）
                int waitInterval = 300;
                int elapsedTime = 0;
                int checkCount = 0;
                
                WriteLog("AutoUpdateUpdaterLog.txt", "[等待退出] 开始等待AutoUpdate进程退出...");
                
                while (elapsedTime < maxWaitTime)
                {
                    checkCount++;
                    bool hasAutoUpdateRunning = false;
                    Process[] runningProcesses = null;
                    
                    foreach (var processName in autoUpdateProcessNames)
                    {
                        Process[] processes = Process.GetProcessesByName(processName);
                        // 排除当前进程
                        int currentProcessId = Process.GetCurrentProcess().Id;
                        processes = processes.Where(p => p.Id != currentProcessId).ToArray();
                        
                        if (processes.Length > 0)
                        {
                            hasAutoUpdateRunning = true;
                            runningProcesses = processes;
                            WriteLog("AutoUpdateUpdaterLog.txt", $"[等待退出] 检测到进程仍在运行: {processName} (数量: {processes.Length}), 检查次数: {checkCount}");
                            
                            // 【新增】主动尝试关闭进程
                            foreach (var process in processes)
                            {
                                try
                                {
                                    if (!process.HasExited)
                                    {
                                        WriteLog("AutoUpdateUpdaterLog.txt", $"[等待退出] 尝试关闭进程: {process.ProcessName} (ID: {process.Id})");
                                        process.CloseMainWindow();
                                        if (!process.WaitForExit(200))
                                        {
                                            process.Kill();
                                            process.WaitForExit(500);
                                            WriteLog("AutoUpdateUpdaterLog.txt", $"[等待退出] 进程已强制终止: {process.ProcessName}");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLog("AutoUpdateUpdaterLog.txt", $"[等待退出] 关闭进程时出错: {ex.Message}");
                                }
                            }
                            break;
                        }
                    }
                    
                    if (!hasAutoUpdateRunning)
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"[等待退出] AutoUpdate进程已完全退出，检查次数: {checkCount}");
                        // 额外等待确保Mutex等资源完全释放
                        Thread.Sleep(2000);
                        WriteLog("AutoUpdateUpdaterLog.txt", "[等待退出] 继续启动主程序");
                        return;
                    }
                    
                    Thread.Sleep(waitInterval);
                    elapsedTime += waitInterval;
                }
                
                WriteLog("AutoUpdateUpdaterLog.txt", "[等待退出] 警告: 等待AutoUpdate进程退出超时，将继续启动主程序");
                // 即使超时也等待一段时间，确保资源释放
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                WriteLog("AutoUpdateUpdaterLog.txt", $"[等待退出] 等待AutoUpdate退出时发生异常: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 安全的文件操作方法，带重试机制
        /// </summary>
        /// <param name="fileOperation">文件操作委托</param>
        /// <param name="operationName">操作名称（用于日志）</param>
        /// <param name="maxRetries">最大重试次数</param>
        /// <returns>操作是否成功</returns>
        private static bool SafeFileOperation(Action fileOperation, string operationName, int maxRetries = 5)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"[文件操作] {operationName} - 尝试 {attempt}/{maxRetries}");
                    fileOperation();
                    WriteLog("AutoUpdateUpdaterLog.txt", $"[文件操作] {operationName} - 成功");
                    return true;
                }
                catch (IOException ioEx)
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"[文件操作] {operationName} - IO异常（尝试 {attempt}/{maxRetries}）: {ioEx.Message}");
                    
                    if (attempt < maxRetries)
                    {
                        // 指数退避策略：1秒、2秒、4秒、8秒、16秒
                        int waitTime = (int)Math.Pow(2, attempt - 1) * 1000;
                        WriteLog("AutoUpdateUpdaterLog.txt", $"[文件操作] 等待 {waitTime}ms 后重试...");
                        Thread.Sleep(waitTime);
                    }
                    else
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"[文件操作] {operationName} - 达到最大重试次数，操作失败");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"[文件操作] {operationName} - 异常: {ex.Message}");
                    return false;
                }
            }
            
            return false;
        }
    }
}