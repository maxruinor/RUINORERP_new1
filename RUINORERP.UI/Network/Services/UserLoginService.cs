using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 用户登录服务
    /// 处理用户身份验证和会话管理相关功能
    /// </summary>
    public class UserLoginService
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly ClientCommandDispatcher _commandDispatcher;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="commandDispatcher">命令调度器</param>
        public UserLoginService(
            IClientCommunicationService communicationService,
            ClientCommandDispatcher commandDispatcher)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientInfo">客户端信息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>登录响应</returns>
        public async Task<ApiResponse<LoginResponse>> LoginAsync(
            string username, 
            string password, 
            string clientInfo = null,
            CancellationToken cancellationToken = default)
        {
            // 创建登录命令
            var loginCommand = new LoginCommand(username, password, clientInfo);
            
            // 验证命令
            var validationResult = loginCommand.Validate();
            if (!validationResult.IsValid)
            {
                return ApiResponse<LoginResponse>.Failure(validationResult.ErrorMessage, 400);
            }

            // 发送命令并等待响应
            var response = await _communicationService.SendCommandAsync<LoginResponse>(
                loginCommand, 
                cancellationToken);

            return response;
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>登出结果</returns>
        public async Task<ApiResponse<bool>> LogoutAsync(
            string sessionId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备登出数据
                var logoutData = new 
                { 
                    SessionId = sessionId,
                    Timestamp = DateTime.UtcNow 
                };

                // 发送登出请求
                var success = await _communicationService.SendOneWayCommandAsync(
                    AuthenticationCommands.Logout,
                    logoutData,
                    cancellationToken);

                if (success)
                {
                    return ApiResponse<bool>.CreateSuccess(true, "用户登出成功");
                }
                else
                {
                    return ApiResponse<bool>.Failure("用户登出失败", 500);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"登出过程中发生异常: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 验证Token有效性
        /// </summary>
        /// <param name="token">访问令牌</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        public async Task<ApiResponse<bool>> ValidateTokenAsync(
            string token,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备验证数据
                var validateData = new 
                { 
                    Token = token,
                    Timestamp = DateTime.UtcNow 
                };

                // 发送验证请求并等待响应
                var response = await _communicationService.SendCommandAsync<object, bool>(
                    AuthenticationCommands.ValidateToken,
                    validateData,
                    cancellationToken);

                // 直接返回响应，因为 SendCommandAsync 已经返回了 ApiResponse<bool>
                return response;
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure($"Token验证过程中发生异常: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 刷新访问令牌
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>新的访问令牌信息</returns>
        public async Task<ApiResponse<TokenInfo>> RefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 准备刷新数据
                var refreshData = new 
                { 
                    RefreshToken = refreshToken,
                    Timestamp = DateTime.UtcNow 
                };

                // 发送刷新请求并等待响应
                var response = await _communicationService.SendCommandAsync<object, TokenInfo>(
                    AuthenticationCommands.RefreshToken,
                    refreshData,
                    cancellationToken);

                // 直接返回响应，因为 SendCommandAsync 已经返回了 ApiResponse<TokenInfo>
                return response;
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenInfo>.Failure($"令牌刷新过程中发生异常: {ex.Message}", 500);
            }
        }
    }

    /// <summary>
    /// 登录响应模型
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 令牌类型
        /// </summary>
        public string TokenType { get; set; }
    }

    /// <summary>
    /// Token信息模型
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 令牌类型
        /// </summary>
        public string TokenType { get; set; }
    }
}