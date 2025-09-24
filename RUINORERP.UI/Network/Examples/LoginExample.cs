using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Examples
{
    /// <summary>
    /// 登录示例类
    /// 展示如何使用客户端通信模块进行用户登录操作
    /// </summary>
    public class LoginExample
    {
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationManager">通信管理器实例</param>
        public LoginExample(CommunicationManager communicationManager)
        {
            _communicationManager = communicationManager ?? throw new ArgumentNullException(nameof(communicationManager));

            // 注册事件处理
            _communicationManager.ConnectionStatusChanged += OnConnectionStatusChanged;
            _communicationManager.ErrorOccurred += OnErrorOccurred;
            _communicationManager.CommandReceived += OnCommandReceived;
        }

        /// <summary>
        /// 用户登录示例
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
                var connected = await _communicationManager.ConnectAsync(
                    serverUrl,
                    port,
                    CancellationToken.None);

                if (!connected)
                {
                    Console.WriteLine("连接服务器失败");
                    return false;
                }

                Console.WriteLine("服务器连接成功");

                // 2. 创建登录命令
                var loginCommand = new LoginCommand(username, password, "Windows Client");

                // 3. 发送登录命令并等待响应
                Console.WriteLine($"正在登录用户: {username}");
                var response = await _communicationManager.SendCommandAsync<LoginResult>(
                    loginCommand,
                    CancellationToken.None);

                if (response.Success)
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
        /// 使用命令ID直接发送登录请求
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果</returns>
        public async Task<bool> LoginWithCommandIdAsync(string username, string password)
        {
            try
            {
                // 准备登录数据
                var loginData = new LoginData
                {
                    Username = username,
                    Password = password,
                    ClientInfo = "Windows Client"
                };

                // 发送登录请求并等待响应
                Console.WriteLine($"正在登录用户: {username}");
                var response = await _communicationManager.SendCommandAsync<LoginData, LoginResult>(
                    AuthenticationCommands.Login, // 使用预定义的登录命令ID
                    loginData,
                    CancellationToken.None,
                    30000); // 30秒超时

                if (response.Success)
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
                var success = await _communicationManager.SendOneWayCommandAsync(
                    AuthenticationCommands.Logout, // 使用预定义的登出命令ID
                    logoutData,
                    CancellationToken.None);

                if (success)
                {
                    Console.WriteLine("用户登出请求发送成功");
                    _communicationManager.Disconnect();
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

        /// <summary>
        /// 处理连接状态变更事件
        /// </summary>
        /// <param name="isConnected">是否已连接</param>
        private void OnConnectionStatusChanged(bool isConnected)
        {
            Console.WriteLine($"连接状态变更: {(isConnected ? "已连接" : "已断开")}");
        }

        /// <summary>
        /// 处理错误发生事件
        /// </summary>
        /// <param name="ex">异常信息</param>
        private void OnErrorOccurred(Exception ex)
        {
            Console.WriteLine($"通信错误: {ex.Message}");
        }

        /// <summary>
        /// 处理接收到的服务器推送命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        private void OnCommandReceived(CommandId commandId, object data)
        {
            Console.WriteLine($"接收到服务器推送命令: {commandId}");
            // 可以在这里处理服务器主动推送的消息
        }
    }

    /// <summary>
    /// 登录结果类
    /// </summary>
    public class LoginResult
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string SessionId { get; set; }
        public string AccessToken { get; set; }
        public DateTime LoginTime { get; set; }
        public string[] Roles { get; set; }
    }
}