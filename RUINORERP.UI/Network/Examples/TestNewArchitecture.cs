using System;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Examples
{
    /// <summary>
    /// 测试新架构的示例程序
    /// </summary>
    public class TestNewArchitecture
    {
        public static async Task RunTestAsync()
        {
            Console.WriteLine("=== 测试新客户端网络架构 ===");

            try
            {
                // 1. 创建核心组件
                var socketClient = new SuperSocketClient();
                var commandDispatcher = new ClientCommandDispatcher();
                var communicationService = new ClientCommunicationService(socketClient, commandDispatcher);

                Console.WriteLine("✓ 核心组件创建成功");

                // 2. 测试命令调度器功能
                var registeredCommands = commandDispatcher.GetRegisteredCommandTypes();
                Console.WriteLine($"✓ 已注册 {registeredCommands.Count} 个命令类型");

                // 3. 测试创建命令
                try
                {
                    var loginCommand = commandDispatcher.CreateCommand(0x0100, "testuser", "testpass");
                    Console.WriteLine($"✓ 登录命令创建成功: {loginCommand.CommandId}");

                    var heartbeatCommand = commandDispatcher.CreateCommand(0x00F0, "client123");
                    Console.WriteLine($"✓ 心跳命令创建成功: {heartbeatCommand.CommandId}");

                    var getUserDataCommand = commandDispatcher.CreateCommand(0x5001, 12345L, true);
                    Console.WriteLine($"✓ 获取用户数据命令创建成功: {getUserDataCommand.CommandId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ 创建命令时发生异常: {ex.Message}");
                }

                // 4. 测试活动命令实例管理
                var activeCommands = commandDispatcher.GetActiveCommands();
                Console.WriteLine($"✓ 当前有 {activeCommands.Count} 个活动命令实例");

                // 5. 测试通信服务接口
                Console.WriteLine($"✓ 通信服务连接状态: {communicationService.IsConnected}");

                // 6. 测试清理功能
                commandDispatcher.CleanupExpiredCommands(0); // 立即清理所有命令
                var afterCleanup = commandDispatcher.GetActiveCommands();
                Console.WriteLine($"✓ 清理后剩余 {afterCleanup.Count} 个活动命令实例");

                Console.WriteLine("=== 测试完成 ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ 测试过程中发生异常: {ex.Message}");
                Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
            }
        }
    }
}