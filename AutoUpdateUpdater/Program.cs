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
            
            try
            {
                // 解析命令行参数
                string sourceDir = "";
                string targetDir = "";
                string exeName = "";
                
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--source-dir" && i + 1 < args.Length)
                    {
                        sourceDir = args[i + 1].Trim('"');
                        i++;
                    }
                    else if (args[i] == "--target-dir" && i + 1 < args.Length)
                    {
                        targetDir = args[i + 1].Trim('"');
                        i++;
                    }
                    else if (args[i] == "--exe-name" && i + 1 < args.Length)
                    {
                        exeName = args[i + 1].Trim('"');
                        i++;
                    }
                    else if (args[i].StartsWith("--source-dir="))
                    {
                        sourceDir = args[i].Substring("--source-dir=".Length).Trim('"');
                    }
                    else if (args[i].StartsWith("--target-dir="))
                    {
                        targetDir = args[i].Substring("--target-dir=".Length).Trim('"');
                    }
                    else if (args[i].StartsWith("--exe-name="))
                    {
                        exeName = args[i].Substring("--exe-name=".Length).Trim('"');
                    }
                }
                
                // 参数验证
                if (string.IsNullOrEmpty(sourceDir) || string.IsNullOrEmpty(targetDir) || string.IsNullOrEmpty(exeName))
                {
                    MessageBox.Show("更新参数无效，无法执行更新", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}