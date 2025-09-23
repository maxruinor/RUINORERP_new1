using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 完整的登录指令实现示例
    /// 展示如何在优化后的架构中实现登录功能
    /// </summary>
    
    // 1. 登录命令定义（在UI项目中）
    [Command("Login", CommandCategory.Authentication, Description = "用户登录命令")]
    public class LoginCommand : RUINORERP.PacketSpec.Commands.BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => AuthenticationCommands.Login;

        /// <summary>
        /// 登录请求数据
        /// </summary>
        public LoginRequest LoginRequest { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginCommand()
        {
            Priority = CommandPriority.High; // 登录命令优先级高
            TimeoutMs = 30000; // 登录超时时间30秒
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientInfo">客户端信息</param>
        public LoginCommand(string username, string password, string clientInfo = null)
        {
            LoginRequest = LoginRequest.Create(username, password, clientInfo);
            Priority = CommandPriority.High; // 登录命令优先级高
            TimeoutMs = 30000; // 登录超时时间30秒
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的登录数据</returns>
        public override object GetSerializableData()
        {
            return LoginRequest;
        }

        /// <summary>
        /// 命令执行的具体逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        protected override async Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                // 客户端登录命令通常只构建数据，实际的发送由通信服务处理
                // 这里可以添加一些自定义逻辑，例如验证输入数据等
                
                // 验证登录请求数据
                if (LoginRequest == null)
                {
                    return CommandResult.Failure("登录请求数据不能为空", "EMPTY_LOGIN_REQUEST");
                }

                if (!LoginRequest.IsValid())
                {
                    return CommandResult.Failure("登录请求数据无效", "INVALID_LOGIN_REQUEST");
                }

                // 构建登录数据
                var loginData = GetSerializableData();
                
                // 返回成功结果，实际的网络请求由通信服务处理
                return CommandResult.Success(loginData, "登录命令构建成功");
            }
            catch (Exception ex)
            {
                return CommandResult.Failure($"登录命令执行异常: {ex.Message}", "LOGIN_EXCEPTION", ex);
            }
        }

        /// <summary>
        /// 验证命令数据
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            // 调用基类验证
            var baseResult = base.Validate();
            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            // 验证登录请求数据
            if (LoginRequest == null)
            {
                return CommandValidationResult.Failure("登录请求数据不能为空", "EMPTY_LOGIN_REQUEST");
            }

            if (!LoginRequest.IsValid())
            {
                return CommandValidationResult.Failure("登录请求数据无效", "INVALID_LOGIN_REQUEST");
            }

            return CommandValidationResult.Success();
        }
    }

    /// <summary>
    /// 登录响应模型（如果PacketSpec中没有定义）
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
        /// 访问Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public string[] Roles { get; set; }
    }

    /// <summary>
    /// 客户端登录服务
    /// </summary>
    public class ClientLoginService
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly ClientCommandDispatcher _commandDispatcher;

        public ClientLoginService(
            IClientCommunicationService communicationService,
            ClientCommandDispatcher commandDispatcher)
        {
            _communicationService = communicationService;
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// 执行登录操作
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录响应</returns>
        public async Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password)
        {
            // 创建登录命令
            var loginCommand = new LoginCommand(username, password);
            
            // 验证命令
            var validationResult = loginCommand.Validate();
            if (!validationResult.IsValid)
            {
                return ApiResponse<LoginResponse>.Failure(validationResult.ErrorMessage, 400);
            }

            // 发送命令并等待响应
            var response = await _communicationService.SendCommandAsync<LoginResponse>(
                loginCommand, 
                CancellationToken.None);

            return response;
        }

        /// <summary>
        /// 使用命令调度器创建命令并执行登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录响应</returns>
        public async Task<ApiResponse<LoginResponse>> LoginWithDispatcherAsync(string username, string password)
        {
            // 使用命令调度器创建命令
            var loginCommand = _commandDispatcher.CreateCommand(0x0100, username, password) as LoginCommand;

            // 发送命令并等待响应
            var response = await _communicationService.SendCommandAsync<LoginResponse>(
                loginCommand, 
                CancellationToken.None);

            return response;
        }
    }

    /// <summary>
    /// 服务端登录命令处理器示例
    /// </summary>
    public class LoginCommandHandler : RUINORERP.PacketSpec.Commands.BaseCommandHandler
    {
        // 支持的命令类型
        public override IReadOnlyList<uint> SupportedCommands => new List<uint> { AuthenticationCommands.Login.FullCode };

        /// <summary>
        /// 执行核心处理逻辑
        /// </summary>
        protected override async Task<CommandResult> OnHandleAsync(ICommand command, CancellationToken cancellationToken)
        {
            // 确保是登录命令
            if (!(command is LoginCommand loginCommand))
            {
                return CommandResult.Failure("不支持的命令类型", ErrorCodes.UnsupportedCommand);
            }

            try
            {
                // 验证登录请求
                var validationResult = await ValidateLoginRequestAsync(loginCommand.LoginRequest, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return CommandResult.Failure(validationResult.ErrorMessage, ErrorCodes.InvalidRequest);
                }

                // 处理登录逻辑
                var loginResult = await ProcessLoginAsync(loginCommand.LoginRequest, cancellationToken);
                
                // 返回成功结果
                return CommandResult.Success(loginResult, "登录成功");
            }
            catch (Exception ex)
            {
                // 记录异常
                LogError($"处理登录命令时发生异常: {ex.Message}", ex);
                return CommandResult.Failure($"登录处理失败: {ex.Message}", ErrorCodes.ProcessError, ex);
            }
        }

        /// <summary>
        /// 验证登录请求
        /// </summary>
        private async Task<RequestValidationResult> ValidateLoginRequestAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask; // 模拟异步操作

            if (request == null)
            {
                return RequestValidationResult.Failure("登录请求不能为空");
            }

            if (string.IsNullOrEmpty(request.Username))
            {
                return RequestValidationResult.Failure("用户名不能为空");
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                return RequestValidationResult.Failure("密码不能为空");
            }

            if (request.Username.Length < 3)
            {
                return RequestValidationResult.Failure("用户名长度不能少于3个字符");
            }

            if (request.Password.Length < 6)
            {
                return RequestValidationResult.Failure("密码长度不能少于6个字符");
            }

            return RequestValidationResult.Success();
        }

        /// <summary>
        /// 处理登录逻辑
        /// </summary>
        private async Task<LoginResponse> ProcessLoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            // 模拟用户验证过程
            await Task.Delay(100, cancellationToken); // 模拟数据库查询延迟

            // 这里应该调用实际的用户验证服务
            var userInfo = await ValidateUserCredentialsAsync(request.Username, request.Password, cancellationToken);
            
            if (userInfo == null)
            {
                throw new UnauthorizedAccessException("用户名或密码错误");
            }

            // 生成Token信息
            var tokenInfo = GenerateTokenInfo(userInfo);

            // 创建登录结果
            var loginResult = new LoginResponse
            {
                UserId = userInfo.UserId,
                Username = userInfo.Username,
                DisplayName = userInfo.DisplayName,
                SessionId = Guid.NewGuid().ToString(),
                AccessToken = tokenInfo.AccessToken,
                RefreshToken = tokenInfo.RefreshToken,
                ExpiresIn = tokenInfo.ExpiresIn,
                LoginTime = DateTime.UtcNow,
                Roles = new string[] { "User" } // 实际应用中应从用户信息中获取
            };

            return loginResult;
        }

        /// <summary>
        /// 验证用户凭据
        /// </summary>
        private async Task<UserInfo> ValidateUserCredentialsAsync(string username, string password, CancellationToken cancellationToken)
        {
            await Task.CompletedTask; // 模拟异步操作

            // 这里应该调用实际的用户服务来验证凭据
            // 模拟验证逻辑
            if (username == "admin" && password == "password123")
            {
                return new UserInfo
                {
                    UserId = 1,
                    Username = "admin",
                    DisplayName = "系统管理员"
                };
            }

            return null;
        }

        /// <summary>
        /// 生成Token信息
        /// </summary>
        private TokenInfo GenerateTokenInfo(UserInfo userInfo)
        {
            return new TokenInfo
            {
                AccessToken = $"access_token_{userInfo.UserId}_{Guid.NewGuid()}",
                RefreshToken = $"refresh_token_{userInfo.UserId}_{Guid.NewGuid()}",
                ExpiresIn = 3600,
                TokenType = "Bearer"
            };
        }
    }

    /// <summary>
    /// 用户信息模型
    /// </summary>
    public class UserInfo
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
    }

    /// <summary>
    /// Token信息模型
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// 访问Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Token类型
        /// </summary>
        public string TokenType { get; set; }
    }
    
    /// <summary>
    /// 请求验证结果
    /// </summary>
    public class RequestValidationResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static RequestValidationResult Success()
        {
            return new RequestValidationResult { IsValid = true };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static RequestValidationResult Failure(string message)
        {
            return new RequestValidationResult
            {
                IsValid = false,
                ErrorMessage = message
            };
        }
    }
}