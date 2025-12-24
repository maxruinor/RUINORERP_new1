using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace AutoUpdate
{
    /// <summary>
    /// 自身更新助手类
    /// 采用双进程机制实现可靠的自身更新
    /// </summary>
    public class SelfUpdateHelper
    {
        /// <summary>
        /// 启动辅助进程进行自身更新
        /// </summary>
        /// <param name="updaterExePath">当前更新程序的路径</param>
        /// <param name="newFilesPath">新文件所在的临时目录</param>
        /// <returns>是否成功启动辅助进程</returns>
        public static bool StartUpdateHelper(string updaterExePath, string newFilesPath)
        {
            try
            {
                // 准备更新命令
                string arguments = $"--self-update --source-dir \"{newFilesPath}\" --target-dir \"{Path.GetDirectoryName(updaterExePath)}\" --exe-name \"{Path.GetFileName(updaterExePath)}\"";

                // 启动辅助进程
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = updaterExePath,
                    Arguments = arguments,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                };

                Process updateProcess = Process.Start(startInfo);
                if (updateProcess != null)
                {
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
                    string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] 启动更新辅助进程失败: {ex.Message}\r\n堆栈跟踪: {ex.StackTrace}\r\n";
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

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--self-update")
                {
                    continue;
                }
                if (args[i] == "--source-dir" && i + 1 < args.Length)
                {
                    sourceDir = args[i + 1].Trim('"');
                    i++;
                }
                if (args[i] == "--target-dir" && i + 1 < args.Length)
                {
                    targetDir = args[i + 1].Trim('"');
                    i++;
                }
                if (args[i] == "--exe-name" && i + 1 < args.Length)
                {
                    exeName = args[i + 1].Trim('"');
                    i++;
                }
            }

            // 验证参数
            if (string.IsNullOrEmpty(sourceDir) || string.IsNullOrEmpty(targetDir) || string.IsNullOrEmpty(exeName))
            {
                WriteLog(Path.Combine(targetDir, "AutoUpdateLog.txt"), "更新参数无效，无法执行自身更新");
                return;
            }

            // 等待主进程退出
            WriteLog(Path.Combine(targetDir, "AutoUpdateLog.txt"), "等待主进程退出...");
            Thread.Sleep(2000); // 等待2秒，确保主进程已退出

            try
            {
                // 执行文件替换
                ReplaceFiles(sourceDir, targetDir);

                // 重启主进程
                string mainExePath = Path.Combine(targetDir, exeName);
                WriteLog(Path.Combine(targetDir, "AutoUpdateLog.txt"), $"更新完成，准备重启: {mainExePath}");
                
                Process.Start(mainExePath);
            }
            catch (Exception ex)
            {
                WriteLog(Path.Combine(targetDir, "AutoUpdateLog.txt"), $"执行更新失败: {ex.Message}\r\n堆栈跟踪: {ex.StackTrace}");
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
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [UPDATE] {message}\r\n";
                File.AppendAllText(logFilePath, logEntry);
            }
            catch { }
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
    }
}