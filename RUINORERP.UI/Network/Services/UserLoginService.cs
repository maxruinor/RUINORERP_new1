using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
/// <summary>
    /// 用户登录服务 - 新架构业务层示例实现
    /// </summary>
    public class UserLoginService
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly ILogger<UserLoginService> _logger;

        /// <summary>
        /// 用户登录服务构造函数
        /// </summary>
        /// <param name="communicationService">客户端通信服务，通过依赖注入提供</param>
        /// <param name="logger">日志记录器，可选</param>
        /// <remarks>
        /// 新架构依赖注入示例：
        /// services.AddSingleton&lt;IClientCommunicationService, ClientCommunicationService&gt;();
        /// services.AddTransient&lt;UserLoginService&gt;();
        /// 
        /// 使用方式：
        /// var loginService = serviceProvider.GetService&lt;UserLoginService&gt;();
        /// </remarks>
        public UserLoginService(IClientCommunicationService communicationService, ILogger<UserLoginService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _logger = logger;
        }

        /// <summary>
        /// 用户登录 - 新架构完整流程
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientInfo">客户端信息（可选）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>登录响应结果</returns>
        /// <remarks>
        /// 新架构数据流：
        /// 1. 构建 LoginCommand 命令对象
        /// 2. 调用 ClientCommunicationService.SendCommandAsync
        /// 3. ClientCommunicationService → ClientNetworkManager
        /// 4. ClientNetworkManager → SuperSocketClient（网络发送）
        /// 5. 服务器处理并返回响应
        /// 6. SuperSocketClient → BizPipelineFilter（接收响应）
        /// 7. BizPipelineFilter → ClientCommunicationService（处理响应）
        /// 8. ClientCommunicationService → UserLoginService（返回结果）
        /// 9. ClientCommunicationService → UserLoginService（业务结果）
        /// 
        /// 异常处理：
        /// - 网络异常：由 CommunicationManager 处理
        /// - 序列化异常：由 BizPipelineFilter 处理
        /// - 业务异常：由服务器返回的错误信息处理
        /// - 超时异常：由 RequestResponseManager 处理
        /// </remarks>
        public Task<ApiResponse<LoginResponse>> LoginAsync(
            string username,
            string password,
            string clientInfo = null,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("开始用户登录流程，用户名: {Username}", username);
            
            var command = new LoginCommand(username, password, clientInfo);
            _logger?.LogDebug("构建登录命令对象: CommandId={CommandId}, Category={Category}", 
                command.CommandIdentifier, command.CommandIdentifier.Category);
            
            return ExecuteCommandAsync<LoginResponse>(command, cancellationToken);
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        public Task<ApiResponse<bool>> LogoutAsync(
            string sessionId,
            CancellationToken cancellationToken = default)
            => ExecuteCommandAsync<bool>(
                new { SessionId = sessionId, Timestamp = DateTime.UtcNow },
                AuthenticationCommands.Logout,
                cancellationToken);

        /// <summary>
        /// 验证Token有效性
        /// </summary>
        public Task<ApiResponse<bool>> ValidateTokenAsync(
            string token,
            CancellationToken cancellationToken = default)
            => ExecuteCommandAsync<bool>(
                new { Token = token, Timestamp = DateTime.UtcNow },
                AuthenticationCommands.ValidateToken,
                cancellationToken);

        /// <summary>
        /// 刷新访问令牌
        /// </summary>
        public Task<ApiResponse<TokenInfo>> RefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default)
            => ExecuteCommandAsync<TokenInfo>(
                new { RefreshToken = refreshToken, Timestamp = DateTime.UtcNow },
                AuthenticationCommands.RefreshToken,
                cancellationToken);

        /* -------------------- 私有公共模板 -------------------- */

        /// <summary>
        /// 统一命令执行模板 - 新架构集成
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>API响应结果</returns>
        /// <remarks>
        /// 新架构集成流程：
        /// 1. 命令验证（参数完整性、业务规则）
        /// 2. 调用 ClientCommunicationService.SendCommandAsync
        /// 3. ClientCommunicationService 构建请求数据包
        /// 4. CommunicationManager 协调网络发送
        /// 5. SuperSocketClient 执行实际网络通信
        /// 6. 等待服务器响应（通过 RequestResponseManager）
        /// 7. BizPipelineFilter 处理响应数据流
        /// 8. ClientCommunicationService 执行命令响应处理
        /// 9. 返回最终业务结果
        /// 
        /// 错误处理层次：
        /// - 验证错误：本地业务验证（400状态码）
        /// - 网络错误：CommunicationManager 处理（500状态码）
        /// - 超时错误：RequestResponseManager 处理（408状态码）
        /// - 服务器错误：业务响应中包含错误信息
        /// </remarks>
        private async Task<ApiResponse<TResponse>> ExecuteCommandAsync<TResponse>(
            ICommand command,
            CancellationToken cancellationToken)
        {
            _logger?.LogDebug("开始执行命令: {CommandId}, 类型: {CommandType}", 
                command.CommandIdentifier, command.GetType().Name);
            
            // 步骤1：命令验证
            var validationResult = command.Validate();
            if (!validationResult.IsValid)
            {
                _logger?.LogWarning("命令验证失败: {ErrorMessage}", validationResult.ErrorMessage);
                return ApiResponse<TResponse>.Failure(validationResult.ErrorMessage, 400);
            }
            
            try
            {
                // 步骤2-8：通过新架构发送命令并等待响应
                _logger?.LogDebug("通过新架构发送命令，超时时间: {Timeout}ms", command.TimeoutMs);
                var response = await _communicationService.SendCommandAsync<TResponse>(command, cancellationToken);
                
                _logger?.LogDebug("命令执行完成，响应状态: {Success}", response.Success);
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "命令执行异常: {CommandId}", command.CommandIdentifier);
                return ApiResponse<TResponse>.Failure($"命令执行失败: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 匿名请求体快速重载（用于无专用 Command 的简单场景）
        /// </summary>
        private Task<ApiResponse<TResponse>> ExecuteCommandAsync<TResponse>(
            object requestBody,
            CommandId commandId,
            CancellationToken cancellationToken)
            => _communicationService.SendCommandAsync<object, TResponse>(commandId, requestBody, cancellationToken);
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