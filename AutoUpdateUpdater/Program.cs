using System;
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
            WriteLog("AutoUpdateUpdaterLog.txt", $"所有参数: {string.Join(" | ", args.Select((arg, index) => $"[{index}]={arg}"))}");
            
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
                        WriteLog("AutoUpdateUpdaterLog.txt", $"命令行参数有效，已保存到配置文件: {configFilePath}");
                    }
                }
                
                WriteLog("AutoUpdateUpdaterLog.txt", $"最终配置: {config}");
                
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
                    // 更新失败，保留配置文件以便排查问题
                    WriteLog("AutoUpdateUpdaterLog.txt", "更新失败，保留配置文件以便排查问题");
                    MessageBox.Show("AutoUpdate更新失败，请手动更新", "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"源目录不存在：{sourceDir}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"源文件不存在：{sourceFile}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
                // 如果指定了AutoUpdate.exe的直接路径，优先使用该路径
                if (!string.IsNullOrEmpty(autoUpdateExePath))
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"使用直接指定的AutoUpdate.exe路径: {autoUpdateExePath}");
                }
                
                // 目标文件路径
                string targetFile = Path.Combine(targetDir, exeName);
                
                // 如果目标文件存在，先尝试关闭正在运行的AutoUpdate进程
                if (File.Exists(targetFile))
                {
                    string processName = Path.GetFileNameWithoutExtension(exeName);
                    
                    // 等待并关闭正在运行的AutoUpdate进程
                    if (!WaitAndKillProcess(processName, 5000))
                    {
                        MessageBox.Show("无法关闭正在运行的AutoUpdate进程，请手动关闭后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    
                    // 备份原文件
                    string backupFile = targetFile + ".bak";
                    if (File.Exists(backupFile))
                    {
                        File.Delete(backupFile);
                    }
                    File.Move(targetFile, backupFile);
                }
                
                // 复制新文件
                File.Copy(sourceFile, targetFile, true);
                
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
        /// </summary>
        private static bool WaitAndKillProcess(string processName, int timeoutMs)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                
                while ((DateTime.Now - startTime).TotalMilliseconds < timeoutMs)
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    
                    // 排除当前进程
                    processes = processes.Where(p => p.Id != Process.GetCurrentProcess().Id).ToArray();
                    
                    if (processes.Length == 0)
                    {
                        return true; // 没有找到相关进程
                    }
                    
                    // 尝试关闭进程
                    foreach (Process process in processes)
                    {
                        try
                        {
                            if (!process.HasExited)
                            {
                                process.CloseMainWindow();
                                
                                if (!process.WaitForExit(1000))
                                {
                                    process.Kill();
                                    process.WaitForExit(1000);
                                }
                            }
                        }
                        catch
                        {
                            // 忽略关闭失败的情况
                        }
                    }
                    
                    Thread.Sleep(500);
                }
                
                // 检查是否还有进程在运行
                Process[] remainingProcesses = Process.GetProcessesByName(processName);
                remainingProcesses = remainingProcesses.Where(p => p.Id != Process.GetCurrentProcess().Id).ToArray();
                
                return remainingProcesses.Length == 0;
            }
            catch
            {
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
                WriteLog("AutoUpdateUpdaterLog.txt", "开始解析命令行参数...");
                
                for (int i = 0; i < args.Length; i++)
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"解析参数[{i}]: {args[i]}");
                    
                    string currentArg = args[i].Trim('"');
                    
                    // 处理格式1: --source-dir "path"
                    if (currentArg == "--source-dir" && i + 1 < args.Length)
                    {
                        config.SourceDir = args[i + 1].Trim('"');
                        i++;
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到源目录(格式1): {config.SourceDir}");
                    }
                    // 处理格式2: --source-dir=path
                    else if (currentArg.StartsWith("--source-dir="))
                    {
                        config.SourceDir = currentArg.Substring("--source-dir=".Length).Trim('"');
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到源目录(格式2): {config.SourceDir}");
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
        /// 启动ERP系统应用程序
        /// </summary>
        private static void StartERPApplication()
        {
            try
            {
                WriteLog("AutoUpdateUpdaterLog.txt", "开始启动ERP系统应用程序...");
                
                // 获取当前目录
                string currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                
                // 尝试从配置文件获取入口程序路径
                string configFile = Path.Combine(currentDir, "AutoUpdaterList.xml");
                string mainAppExe = "企业数字化集成ERP.exe";
                
                if (File.Exists(configFile))
                {
                    try
                    {
                        // 使用System.Xml命名空间读取配置文件
                        var xmlDoc = new System.Xml.XmlDocument();
                        xmlDoc.Load(configFile);
                        
                        var entryPointNode = xmlDoc.SelectSingleNode("//EntryPoint");
                        if (entryPointNode != null && !string.IsNullOrEmpty(entryPointNode.InnerText))
                        {
                            mainAppExe = entryPointNode.InnerText;
                            WriteLog("AutoUpdateUpdaterLog.txt", $"从配置文件读取主程序路径: {mainAppExe}");
                        }
                        else
                        {
                            WriteLog("AutoUpdateUpdaterLog.txt", $"使用默认主程序路径: {mainAppExe}");
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"读取配置文件失败: {ex.Message}，使用默认路径");
                        MessageBox.Show($"读取配置文件失败: {ex.Message}，使用默认路径", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                
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
                    
                    // 添加启动参数，标识从AutoUpdateUpdater启动
                    startInfo.Arguments = "--updated-from-auto-updater";
                    
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
    }
}