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
                // 解析命令行参数
                string sourceDir = "";
                string targetDir = "";
                string exeName = "";
                
                WriteLog("AutoUpdateUpdaterLog.txt", "开始解析命令行参数...");
                
                for (int i = 0; i < args.Length; i++)
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", $"解析参数[{i}]: {args[i]}");
                    
                    if (args[i] == "--source-dir" && i + 1 < args.Length)
                    {
                        sourceDir = args[i + 1].Trim('"');
                        i++;
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到源目录(格式1): {sourceDir}");
                    }
                    else if (args[i] == "--target-dir" && i + 1 < args.Length)
                    {
                        targetDir = args[i + 1].Trim('"');
                        i++;
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到目标目录(格式1): {targetDir}");
                    }
                    else if (args[i] == "--exe-name" && i + 1 < args.Length)
                    {
                        exeName = args[i + 1].Trim('"');
                        i++;
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到可执行文件名(格式1): {exeName}");
                    }
                    else if (args[i].StartsWith("--source-dir="))
                    {
                        sourceDir = args[i].Substring("--source-dir=".Length).Trim('"');
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到源目录(格式2): {sourceDir}");
                    }
                    else if (args[i].StartsWith("--target-dir="))
                    {
                        targetDir = args[i].Substring("--target-dir=".Length).Trim('"');
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到目标目录(格式2): {targetDir}");
                    }
                    else if (args[i].StartsWith("--exe-name="))
                    {
                        exeName = args[i].Substring("--exe-name=".Length).Trim('"');
                        WriteLog("AutoUpdateUpdaterLog.txt", $"解析到可执行文件名(格式2): {exeName}");
                    }
                    else
                    {
                        WriteLog("AutoUpdateUpdaterLog.txt", $"未识别的参数格式: {args[i]}");
                    }
                }
                
                WriteLog("AutoUpdateUpdaterLog.txt", $"参数解析完成 - 源目录: {sourceDir}, 目标目录: {targetDir}, 可执行文件名: {exeName}");
                
                // 参数验证
                if (string.IsNullOrEmpty(sourceDir))
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", "错误: 源目录参数为空");
                    MessageBox.Show("更新参数无效: 源目录为空，无法执行更新，将尝试启动ERP主程序", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StartERPApplication();
                    return;
                }
                
                if (string.IsNullOrEmpty(targetDir))
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", "错误: 目标目录参数为空");
                    MessageBox.Show("更新参数无效: 目标目录为空，无法执行更新，将尝试启动ERP主程序", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StartERPApplication();
                    return;
                }
                
                if (string.IsNullOrEmpty(exeName))
                {
                    WriteLog("AutoUpdateUpdaterLog.txt", "错误: 可执行文件名参数为空");
                    MessageBox.Show("更新参数无效: 可执行文件名为空，无法执行更新，将尝试启动ERP主程序", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StartERPApplication();
                    return;
                }
                
                // 执行更新
                bool success = ExecuteUpdate(sourceDir, targetDir, exeName);
                
                if (success)
                {
                    // 启动更新后的AutoUpdate.exe
                    string newAutoUpdatePath = Path.Combine(targetDir, exeName);
                    if (File.Exists(newAutoUpdatePath))
                    {
                        Process.Start(newAutoUpdatePath);
                    }
                    
                    Application.Exit();
                }
                else
                {
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
        private static bool ExecuteUpdate(string sourceDir, string targetDir, string exeName)
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
                
                // 源文件路径
                string sourceFile = Path.Combine(sourceDir, exeName);
                if (!File.Exists(sourceFile))
                {
                    MessageBox.Show($"源文件不存在：{sourceFile}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
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