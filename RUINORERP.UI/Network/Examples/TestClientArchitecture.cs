using System;

namespace RUINORERP.UI.Network.Examples
{
    /// <summary>
    /// 测试客户端架构
    /// </summary>
    public class TestClientArchitecture
    {
        public static void RunTest()
        {
            try
            {
                Console.WriteLine("开始测试客户端架构...");
                
                // 测试命令调度器
                var dispatcher = new ClientCommandDispatcher();
                Console.WriteLine($"命令调度器创建成功，已注册 {dispatcher.GetRegisteredCommandTypes().Count} 个命令类型");
                
                // 测试命令工厂
                var factory = new ClientCommandFactory(dispatcher);
                Console.WriteLine("命令工厂创建成功");
                
                // 测试创建命令
                var loginCommand = dispatcher.CreateCommand(0x0100, "testuser", "testpass");
                Console.WriteLine($"登录命令创建成功: {loginCommand.CommandId}");
                
                var heartbeatCommand = dispatcher.CreateCommand(0x00F0, "client123");
                Console.WriteLine($"心跳命令创建成功: {heartbeatCommand.CommandId}");
                
                Console.WriteLine("客户端架构测试完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试过程中发生异常: {ex.Message}");
            }
        }
    }
}