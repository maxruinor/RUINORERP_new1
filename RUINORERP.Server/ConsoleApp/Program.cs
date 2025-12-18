using System;
using System.Threading.Tasks;
using RUINORERP.Server.Tests;

namespace RUINORERP.Server.ConsoleApp
{
    /// <summary>
    /// 端口检查控制台应用程序
    /// </summary>
    class Program
    {
        /// <summary>
        /// 应用程序入口点
        /// </summary>
        /// <param name="args">命令行参数</param>
        static async Task Main(string[] args)
        {
            System.Diagnostics.Debug.WriteLine("RUINORERP 端口检查工具");
            System.Diagnostics.Debug.WriteLine("======================\n");
            
            // 测试端口检查功能
            PortCheckTest.TestPortCheck();
            
            System.Diagnostics.Debug.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}