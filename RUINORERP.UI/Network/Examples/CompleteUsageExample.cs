using RUINORERP.PacketSpec.Commands;
using System;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Examples
{
    /// <summary>
    /// 完整使用示例
    /// 展示如何初始化和使用整个客户端通信系统
    /// </summary>
    public class CompleteUsageExample
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("客户端通信系统使用示例");

            try
            {
                // 1. 创建Socket客户端（使用SuperSocket实现）
                var socketClient = new SuperSocketClient();

                // 2. 创建客户端命令调度器
                var commandDispatcher = new ClientCommandDispatcher();

                // 3. 创建客户端通信服务
                var communicationService = new ClientCommunicationService(socketClient, commandDispatcher);

                // 4. 创建心跳管理器
                var heartbeatManager = new HeartbeatManager(communicationService);

                // 5. 创建通信管理器
                var communicationManager = new CommunicationManager(communicationService, heartbeatManager);

                // 6. 初始化通信管理器
                communicationManager.Initialize();

                // 7. 创建登录示例
                var loginExample = new LoginExample(communicationManager);

                // 8. 执行登录操作
                var loginSuccess = await loginExample.LoginAsync(
                    "127.0.0.1", 
                    8080, 
                    "testuser", 
                    "testpassword");

                if (loginSuccess)
                {
                    Console.WriteLine("登录成功，系统准备就绪");

                    // 模拟一些操作
                    await Task.Delay(5000);

                    // 9. 执行登出操作
                    await loginExample.LogoutAsync();
                }
                else
                {
                    Console.WriteLine("登录失败");
                }

                // 10. 释放资源
                communicationManager.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"程序执行过程中发生异常: {ex.Message}");
                Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
            }

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}