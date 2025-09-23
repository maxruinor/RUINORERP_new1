using RUINORERP.PacketSpec.Communication;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Communication.Handlers
{
    /// <summary>
    /// 登录请求处理器
    /// 处理用户登录请求的通用实现
    /// </summary>
    public class LoginRequestHandler : RequestHandlerBase<LoginRequest, LoginResult>
    {
        /// <summary>
        /// 验证登录请求
        /// </summary>
        protected override async Task<RequestValidationResult> ValidateRequestAsync(LoginRequest request, CancellationToken cancellationToken)
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
        /// 处理登录请求的核心逻辑
        /// </summary>
        protected override async Task<RequestProcessResult<LoginResult>> ProcessRequestAsync(LoginRequest request, CancellationToken cancellationToken)
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
            var loginResult = new LoginResult
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

            return RequestProcessResult<LoginResult>.Create(loginResult, "登录成功");
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

        /// <summary>
        /// 处理异常
        /// </summary>
        protected override ApiResponse<LoginResult> HandleException(Exception ex)
        {
            LogException(ex);

            if (ex is UnauthorizedAccessException)
            {
                return ApiResponse<LoginResult>.Failure(ex.Message, 401);
            }

            return ApiResponse<LoginResult>.Failure($"登录处理时发生错误: {ex.Message}", 500);
        }
    }

    /// <summary>
    /// 登录结果
    /// </summary>
    public class LoginResult
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
}