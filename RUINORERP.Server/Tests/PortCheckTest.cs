using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.Core;

namespace RUINORERP.Server.Tests
{
    /// <summary>
    /// 端口检查测试类
    /// </summary>
    public class PortCheckTest
    {
        /// <summary>
        /// 测试端口检查功能
        /// </summary>
        public static void TestPortCheck()
        {
            System.Diagnostics.Debug.WriteLine("开始测试端口检查功能...\n");
            
            // 测试一些常见端口
            var testPorts = new List<int> { 80, 443, 3009, 3006, 6666, 8080, 2020 };
            
            foreach (var port in testPorts)
            {
                bool isPortInUse = IsPortInUse(port);
                System.Diagnostics.Debug.WriteLine($"端口 {port}: {(isPortInUse ? "已被占用" : "可用")}");
                
                if (isPortInUse)
                {
                    ShowPortUsageDetails(port);
                }
            }
            
            System.Diagnostics.Debug.WriteLine("\n端口检查测试完成。");
        }
        
        /// <summary>
        /// 检查指定端口是否已被占用
        /// </summary>
        /// <param name="port">要检查的端口号</param>
        /// <returns>端口是否已被占用</returns>
        private static bool IsPortInUse(int port)
        {
            try
            {
                // 使用TcpClient尝试连接端口，如果连接成功说明端口被占用
                using (var tcpClient = new System.Net.Sockets.TcpClient())
                {
                    // 尝试连接本地端口，设置超时时间
                    var connectTask = tcpClient.ConnectAsync(System.Net.IPAddress.Loopback, port);
                    var completed = connectTask.Wait(TimeSpan.FromMilliseconds(500));
                    
                    if (completed && tcpClient.Connected)
                    {
                        return true; // 端口已被占用
                    }
                }
                
                // 如果本地连接失败，再检查所有网络接口
                var ipGlobalProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
                var tcpConnections = ipGlobalProperties.GetActiveTcpConnections();
                
                foreach (var connection in tcpConnections)
                {
                    if (connection.LocalEndPoint.Port == port)
                    {
                        return true; // 端口已被占用
                    }
                }
                
                return false; // 端口未被占用
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"检查端口 {port} 时发生异常: {ex.Message}");
                return false; // 发生异常时假设端口未被占用
            }
        }
        
        /// <summary>
        /// 显示端口使用详情
        /// </summary>
        /// <param name="port">端口号</param>
        private static void ShowPortUsageDetails(int port)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"  检查端口 {port} 占用详情:");
                System.Diagnostics.Debug.WriteLine($"  命令: netstat -ano | findstr :{port}");
                
                // 执行命令获取详细信息
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c netstat -ano | findstr :{port}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                
                using (var process = System.Diagnostics.Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                        
                        if (!string.IsNullOrWhiteSpace(output))
                        {
                            System.Diagnostics.Debug.WriteLine("  输出:");
                            foreach (var line in output.Split('\n'))
                            {
                                if (!string.IsNullOrWhiteSpace(line))
                                {
                                    System.Diagnostics.Debug.WriteLine($"    {line.Trim()}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"  获取端口 {port} 详情时发生错误: {ex.Message}");
            }
        }
    }
}