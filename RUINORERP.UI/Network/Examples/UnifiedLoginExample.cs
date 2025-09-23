using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Communication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Examples
{
    /// <summary>
    /// 统一登录示例类
    /// 展示如何使用通用请求处理框架进行用户登录操作
    /// </summary>
    public class UnifiedLoginExample
    {
        private readonly IClientCommunicationService _communicationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">客户端通信服务</param>
        public UnifiedLoginExample(IClientCommunicationService communicationService)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
        }

        /// <summary>
        /// 用户登录示例（使用通用请求处理框架）
        /// </summary>
        /// <param name="serverUrl">服务器地址</param>
        /// <param name="port">端口号</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果</returns>
        public async Task<bool> LoginAsync(string serverUrl, int port, string username, string password)
        {
            try
            {
                Console.WriteLine($"正在连接到服务器: {serverUrl}:{port}");

                // 1. 连接到服务器
                var connected = await _communicationService.ConnectAsync(
                    serverUrl,
                    port,
                    CancellationToken.None);

                if (!connected)
                {
                    Console.WriteLine("连接服务器失败");
                    return false;
                }

                Console.WriteLine("服务器连接成功");

                // 2. 创建登录请求数据
                var loginRequest = LoginRequest.Create(username, password, "Windows Client");

                // 3. 发送登录请求并等待响应（使用通用请求处理框架）
                Console.WriteLine($"正在登录用户: {username}");
                var response = await _communicationService.SendCommandAsync<LoginRequest, LoginResult>(
                    AuthenticationCommands.Login, // 使用预定义的登录命令ID
                    loginRequest,
                    CancellationToken.None,
                    30000); // 30秒超时

                if (response.IsSuccess())
                {
                    Console.WriteLine("用户登录成功");
                    Console.WriteLine($"用户ID: {response.Data.UserId}");
                    Console.WriteLine($"用户名: {response.Data.Username}");
                    Console.WriteLine($"会话ID: {response.Data.SessionId}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"用户登录失败: {response.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"登录过程中发生异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 使用命令对象发送登录请求
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果</returns>
        public async Task<bool> LoginWithCommandAsync(string username, string password)
        {
            try
            {
                // 创建登录命令
                var loginCommand = new LoginCommand(username, password, "Windows Client");

                // 发送登录命令并等待响应
                Console.WriteLine($"正在登录用户: {username}");
                var response = await _communicationService.SendCommandAsync<LoginResult>(
                    loginCommand,
                    CancellationToken.None);

                if (response.IsSuccess())
                {
                    Console.WriteLine("用户登录成功");
                    Console.WriteLine($"用户ID: {response.Data.UserId}");
                    Console.WriteLine($"用户名: {response.Data.Username}");
                    Console.WriteLine($"会话ID: {response.Data.SessionId}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"用户登录失败: {response.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"登录过程中发生异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 用户登出示例
        /// </summary>
        public async Task<bool> LogoutAsync()
        {
            try
            {
                // 准备登出数据
                var logoutData = new { Timestamp = DateTime.Now };

                // 发送登出请求
                var success = await _communicationService.SendOneWayCommandAsync(
                    AuthenticationCommands.Logout, // 使用预定义的登出命令ID
                    logoutData,
                    CancellationToken.None);

                if (success)
                {
                    Console.WriteLine("用户登出请求发送成功");
                    _communicationService.Disconnect();
                    return true;
                }
                else
                {
                    Console.WriteLine("用户登出请求发送失败");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"登出过程中发生异常: {ex.Message}");
                return false;
            }
        }
    }
}