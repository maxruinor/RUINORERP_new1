using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Commands;
using System;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Examples
{
    /// <summary>
    /// 客户端架构使用示例
    /// 展示如何使用新的客户端网络架构
    /// </summary>
    public class ClientArchitectureExample
    {
        /// <summary>
        /// 基本使用示例
        /// </summary>
        public static async Task BasicUsageExample()
        {
            Console.WriteLine("=== 客户端架构基本使用示例 ===");

            // 1. 创建客户端组件
            var socketClient = new SuperSocketClient();
            var commandDispatcher = new ClientCommandDispatcher();
            var communicationService = new ClientCommunicationService(socketClient, commandDispatcher);

            try
            {
                // 2. 连接服务器
                bool connected = await communicationService.ConnectAsync("127.0.0.1", 8080);
                if (!connected)
                {
                    Console.WriteLine("连接服务器失败");
                    return;
                }

                Console.WriteLine("成功连接到服务器");

                // 3. 发送登录命令
                var loginCommand = new LoginCommand("testuser", "testpassword", "TestClient");
                var loginResponse = await communicationService.SendCommandAsync<LoginResult>(loginCommand);

                if (loginResponse.Success)
                {
                    Console.WriteLine($"登录成功: {loginResponse.Data?.ToString()}");
                    
                    // 4. 发送获取用户数据命令
                    var getUserDataCommand = new GetUserDataCommand(12345, true);
                    var userDataResponse = await communicationService.SendCommandAsync<object>(getUserDataCommand);
                    
                    if (userDataResponse.Success)
                    {
                        Console.WriteLine($"获取用户数据成功: {userDataResponse.Data?.ToString()}");
                    }
                    else
                    {
                        Console.WriteLine($"获取用户数据失败: {userDataResponse.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"登录失败: {loginResponse.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常: {ex.Message}");
            }
            finally
            {
                // 5. 清理资源
                communicationService.Dispose();
                socketClient.Dispose();
            }
        }

        /// <summary>
        /// 心跳管理示例
        /// </summary>
        public static async Task HeartbeatExample()
        {
            Console.WriteLine("\n=== 心跳管理示例 ===");

            // 创建客户端组件
            var socketClient = new SuperSocketClient();
            var commandDispatcher = new ClientCommandDispatcher();
            var communicationService = new ClientCommunicationService(socketClient, commandDispatcher);
            var heartbeatManager = new HeartbeatManager(communicationService, 10000); // 10秒心跳间隔

            try
            {
                // 连接服务器
                bool connected = await communicationService.ConnectAsync("127.0.0.1", 8080);
                if (!connected)
                {
                    Console.WriteLine("连接服务器失败");
                    return;
                }

                Console.WriteLine("成功连接到服务器");

                // 注册心跳事件
                heartbeatManager.OnHeartbeatSuccess += () => Console.WriteLine("心跳发送成功");
                heartbeatManager.OnHeartbeatFailed += (message) => Console.WriteLine($"心跳发送失败: {message}");
                heartbeatManager.OnHeartbeatException += (ex) => Console.WriteLine($"心跳异常: {ex.Message}");

                // 开始心跳
                heartbeatManager.Start();
                Console.WriteLine("心跳管理器已启动");

                // 保持运行30秒
                await Task.Delay(30000);

                // 停止心跳
                heartbeatManager.Stop();
                Console.WriteLine("心跳管理器已停止");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常: {ex.Message}");
            }
            finally
            {
                // 清理资源
                heartbeatManager.Dispose();
                communicationService.Dispose();
                socketClient.Dispose();
            }
        }

        /// <summary>
        /// 事件处理示例
        /// </summary>
        public static async Task EventHandlingExample()
        {
            Console.WriteLine("\n=== 事件处理示例 ===");

            // 创建客户端组件
            var socketClient = new SuperSocketClient();
            var commandDispatcher = new ClientCommandDispatcher();
            var communicationService = new ClientCommunicationService(socketClient, commandDispatcher);

            try
            {
                // 注册事件处理
                communicationService.CommandReceived += (commandId, data) =>
                {
                    Console.WriteLine($"接收到服务器命令: {commandId} - 数据: {data?.ToString()}");
                };

                // 连接服务器
                bool connected = await communicationService.ConnectAsync("127.0.0.1", 8080);
                if (!connected)
                {
                    Console.WriteLine("连接服务器失败");
                    return;
                }

                Console.WriteLine("成功连接到服务器");

                // 保持运行一段时间以接收服务器推送
                await Task.Delay(10000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常: {ex.Message}");
            }
            finally
            {
                // 清理资源
                communicationService.Dispose();
                socketClient.Dispose();
            }
        }

        /// <summary>
        /// 命令调度器使用示例
        /// </summary>
        public static void CommandDispatcherExample()
        {
            Console.WriteLine("\n=== 命令调度器使用示例 ===");

            // 创建命令调度器
            var commandDispatcher = new ClientCommandDispatcher();

            // 查看已注册的命令类型
            var registeredCommands = commandDispatcher.GetRegisteredCommandTypes();
            Console.WriteLine($"已注册 {registeredCommands.Count} 个命令类型:");
            foreach (var kvp in registeredCommands)
            {
                Console.WriteLine($"  命令代码: {kvp.Key:X4} -> 类型: {kvp.Value.Name}");
            }

            // 创建命令实例
            try
            {
                var loginCommand = commandDispatcher.CreateCommand(0x0100, "testuser", "testpass");
                Console.WriteLine($"创建登录命令成功: {loginCommand.CommandId}");

                var heartbeatCommand = commandDispatcher.CreateCommand(0x00F0, "client123");
                Console.WriteLine($"创建心跳命令成功: {heartbeatCommand.CommandId}");

                var getUserDataCommand = commandDispatcher.CreateCommand(0x5001, 12345L, true);
                Console.WriteLine($"创建获取用户数据命令成功: {getUserDataCommand.CommandId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建命令时发生异常: {ex.Message}");
            }

            // 查看活动的命令实例
            var activeCommands = commandDispatcher.GetActiveCommands();
            Console.WriteLine($"当前有 {activeCommands.Count} 个活动命令实例");
        }
    }
}