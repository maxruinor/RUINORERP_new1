using RUINORERP.PacketSpec.Commands;
using RUINORERP.UI.Network.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Examples
{
    /// <summary>
    /// 通信示例类
    /// 展示如何使用重构后的客户端通信模块
    /// </summary>
    public class CommunicationExample
    {
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationManager">通信管理器实例</param>
        public CommunicationExample(CommunicationManager communicationManager)
        {
            _communicationManager = communicationManager ?? throw new ArgumentNullException(nameof(communicationManager));

            // 注册事件处理
            _communicationManager.ConnectionStatusChanged += OnConnectionStatusChanged;
            _communicationManager.ErrorOccurred += OnErrorOccurred;
        }

        /// <summary>
        /// 连接到服务器示例
        /// </summary>
        /// <param name="serverUrl">服务器地址</param>
        /// <param name="port">端口号</param>
        /// <returns>连接是否成功</returns>
        public async Task<bool> ConnectToServerAsync(string serverUrl, int port)
        {
            try
            {
                Console.WriteLine($"正在连接到服务器: {serverUrl}:{port}");
                
                // 设置自动重连
                _communicationManager.AutoReconnect = true;
                
                // 连接服务器
                var connected = await _communicationManager.ConnectAsync(
                    serverUrl, 
                    port, 
                    CancellationToken.None);
                
                if (connected)
                {
                    Console.WriteLine("服务器连接成功");
                }
                else
                {
                    Console.WriteLine("服务器连接失败");
                }
                
                return connected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接服务器时发生异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 发送用户数据请求示例
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="includeProfile">是否包含个人资料</param>
        /// <returns>用户数据响应</returns>
        public async Task<ApiResponse<UserDataResponse>> GetUserDataAsync(int userId, bool includeProfile = true)
        {
            try
            {
                Console.WriteLine($"请求用户数据: UserId={userId}, IncludeProfile={includeProfile}");
                
                // 准备请求数据
                var requestData = new { UserId = userId, IncludeProfile = includeProfile };
                
                // 发送请求并等待响应
                // 注意：这里使用的是假设的命令ID，实际使用时需要替换为真实的命令ID
                var commandId = new CommandId(CommandCategory.DataSync, 0x01);
                var response = await _communicationManager.SendCommandAsync<object, UserDataResponse>(
                    commandId, 
                    requestData,
                    CancellationToken.None,
                    10000); // 10秒超时
                
                if (response.Success)
                {
                    Console.WriteLine("获取用户数据成功");
                }
                else
                {
                    Console.WriteLine($"获取用户数据失败: {response.Message}");
                }
                
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取用户数据时发生异常: {ex.Message}");
                return ApiResponse<UserDataResponse>.Failure(ex.Message);
            }
        }

        /// <summary>
        /// 发送单向命令示例
        /// </summary>
        /// <param name="logMessage">日志消息</param>
        /// <returns>发送是否成功</returns>
        public async Task<bool> LogActivityAsync(string logMessage)
        {
            try
            {
                Console.WriteLine($"发送活动日志: {logMessage}");
                
                // 准备日志数据
                var logData = new { Message = logMessage, Timestamp = DateTime.Now };
                
                // 发送单向命令（不等待响应）
                // 注意：这里使用的是假设的命令ID，实际使用时需要替换为真实的命令ID
                var commandId = new CommandId(CommandCategory.Logging, 0x01);
                return await _communicationManager.SendOneWayCommandAsync(
                    commandId, 
                    logData,
                    CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送活动日志时发生异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 断开连接示例
        /// </summary>
        public void DisconnectFromServer()
        {
            try
            {
                Console.WriteLine("正在断开服务器连接...");
                _communicationManager.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"断开连接时发生异常: {ex.Message}");
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
            // 可以在这里添加错误日志记录或用户提示
        }

        /// <summary>
        /// 示例用户数据响应类
        /// 实际使用时应该使用服务器端定义的响应模型
        /// </summary>
        public class UserDataResponse
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string FullName { get; set; }
            public DateTime LastLogin { get; set; }
            public UserProfile Profile { get; set; }
        }

        /// <summary>
        /// 示例用户个人资料类
        /// </summary>
        public class UserProfile
        {
            public string Avatar { get; set; }
            public string Department { get; set; }
            public string Position { get; set; }
            public string Phone { get; set; }
        }
    }
}