using MessagePack;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Authentication;
using SourceLibrary.Security;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 用户登录服务类
    /// 提供用户认证、Token管理等功能
    /// 使用简化版TokenManager体系，提供核心的Token管理功能
    /// </summary>
    public sealed class UserLoginService : IDisposable
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ICommandCreationService _commandCreationService;
        private readonly ILogger<UserLoginService> _log;
        private readonly SilentTokenRefresher _silentTokenRefresher;
        private readonly TokenManager _tokenManager;

        /// <summary>
        /// 构造函数 - 使用依赖注入的TokenManager
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="commandCreationService">命令创建服务</param>
        /// <param name="tokenManager">Token管理器（依赖注入）</param>
        /// <param name="logger">日志记录器</param>
        public UserLoginService(
            ClientCommunicationService communicationService,
            ICommandCreationService commandCreationService,
            TokenManager tokenManager,
            ILogger<UserLoginService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _commandCreationService = commandCreationService ?? throw new ArgumentNullException(nameof(commandCreationService));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _log = logger;

            _silentTokenRefresher = new SilentTokenRefresher(new TokenRefreshService(communicationService, _tokenManager));

            // 订阅静默刷新事件
            _silentTokenRefresher.RefreshSucceeded += OnRefreshSucceeded;
            _silentTokenRefresher.RefreshFailed += OnTokenRefreshFailed;
        }

        /* -------------------- 唯一对外入口 -------------------- */

        /// <summary>
        /// 用户登录 - 使用简化版TokenManager
        /// 返回包含指令信息的响应数据
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>包含指令信息的登录响应</returns>
        public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default)
        {
            try
            {

                var loginRequest = LoginRequest.Create(username, password);
                BaseCommand<IRequest, LoginResponse> baseCommand = new BaseCommand<IRequest, LoginResponse>(AuthenticationCommands.Login, loginRequest);

                // 使用新的方法发送命令并获取包含指令信息的响应
                var command = await _communicationService.SendCommandWithResponseAsync<IRequest, LoginResponse>(baseCommand, ct);

                // 检查响应数据是否为空
                if (command.Response == null)
                {
                    _log?.LogError("登录失败：服务器返回了空的响应数据");
                    return BaseCommand<IRequest, LoginResponse>.CreateError("服务器返回了空的响应数据").Response;
                }

                // 检查响应是否成功
                if (!command.Response.IsSuccess)
                {
                    // 检查是否是时间错误
                    var errorCode = command.Response.ErrorMessage;
                    if (errorCode == "TIME_MISMATCH")
                    {
                        // 时间不匹配错误，提示用户校准系统时间
                        return BaseCommand<IRequest, LoginResponse>.CreateError(
                            "客户端时间与服务器时间差异过大，请校准系统时间后重试",
                            UnifiedErrorCodes.Command_ValidationFailed.Code)
                            .WithMetadata("ErrorCode", "TIME_MISMATCH").Response;
                    }

                    return BaseCommand<IRequest, IResponse>.CreateError("登陆失败").Response as LoginResponse;
                }

                // 登录成功后处理Token - 使用简化版TokenManager
                if (command.Response.IsSuccess)
                {
                    var loginResponse = command.Response;

                    // 保存Token信息
                    if (loginResponse.Token != null && !string.IsNullOrEmpty(loginResponse.Token.AccessToken))
                    {
                        await _tokenManager.TokenStorage.SetTokenAsync(loginResponse.Token);
                        MainForm.Instance.AppContext.SessionId = command.Response.SessionId;

                        // 登录成功后启动心跳
                        try
                        {
                            await _communicationService.StartHeartbeatAfterLoginAsync(ct);
                        }
                        catch (Exception heartbeatEx)
                        {
                            _log?.LogWarning(heartbeatEx, "登录成功后启动心跳失败 - 用户: {Username}", username);
                            // 心跳启动失败不影响登录流程，只记录警告
                        }
                    }
                    else
                    {
                        _log?.LogWarning("登录响应中未包含有效的Token信息 - 用户: {Username}, 请求ID: {RequestId}",
                            username, command.Request.RequestId);
                    }
                }
                else
                {
                    _log?.LogWarning("登录失败 - 用户: {Username}, 错误: {ErrorMessage}",
                        username, command.Response.ErrorMessage);
                }

                return command.Response;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "登录过程中发生异常 - 用户: {Username}", username);
                throw new Exception($"登录失败: {ex.Message}", ex);
            }
        }



        /// <summary>
        /// 用户登出 - 使用简化版TokenManager
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>登出结果</returns>
        public async Task<bool> LogoutAsync(CancellationToken ct = default)
        {
            try
            {
                // 获取当前令牌信息
                var tokenInfo = await _tokenManager.TokenStorage.GetTokenAsync();
                if (tokenInfo == null)
                {
                    _log?.LogWarning("登出失败：未找到有效的令牌信息");
                    return false;
                }

                // 创建登出请求
                var request = LoginRequest.CreateLogoutRequest();
                var baseCommand = new BaseCommand<LoginRequest, LoginResponse>(AuthenticationCommands.Logout, request);
                var response = await _communicationService.SendCommandAsync<LoginRequest, LoginResponse>(baseCommand, ct);

                // 检查响应是否成功 TODO  要完善
                bool isSuccess = true;// response != null && response.CommandData.Request != null && response.Request.IsSuccess;

                if (isSuccess)
                {
                    // 登出成功后清除令牌并停止静默刷新 - 使用简化版TokenManager
                    await _tokenManager.TokenStorage.ClearTokenAsync();
                    _log?.LogDebug("用户登出成功");
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "登出过程中发生异常");
                throw new Exception($"登出失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取当前访问令牌 - 使用简化版TokenManager
        /// </summary>
        /// <returns>访问令牌</returns>
        public async Task<string> GetCurrentAccessToken()
        {
            var tokenInfo = await _tokenManager.TokenStorage.GetTokenAsync();
            return tokenInfo?.AccessToken;
        }

        /// <summary>
        /// 手动触发Token刷新（通常由静默刷新器调用）
        /// </summary>
        /// <returns>刷新是否成功</returns>
        public Task<bool> TrySilentRefreshAsync()
        {
            return _silentTokenRefresher.TriggerRefreshAsync();
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ValidateTokenAsync()
        {
            try
            {
                var currentToken = await _tokenManager.TokenStorage.GetTokenAsync();
                if (currentToken == null)
                {
                    return false;
                }

                var validateRequest = SimpleRequest.CreateString(currentToken.AccessToken);
                var baseCommand = new BaseCommand<IRequest, IResponse>();

                var response = await _communicationService.SendCommandAsync<IRequest, IResponse>(baseCommand, CancellationToken.None);
                return response != null;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "Token验证过程中发生异常");
                return false;
            }
        }





        /// <summary>
        /// 处理Token刷新成功事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnRefreshSucceeded(object sender, SilentTokenRefresher.RefreshSucceededEventArgs e)
        {
            _log?.LogDebug("静默刷新Token成功");
        }

        /// <summary>
        /// 处理Token刷新失败事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnTokenRefreshFailed(object sender, SilentTokenRefresher.RefreshFailedEventArgs e)
        {
            _log?.LogWarning("静默刷新Token失败: {Message}", e.Exception.Message);
            // 可以在这里添加通知用户重新登录的逻辑
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _silentTokenRefresher.RefreshFailed -= OnTokenRefreshFailed;
            _silentTokenRefresher.RefreshSucceeded -= OnRefreshSucceeded;
            _silentTokenRefresher.Dispose();
        }
    }
}