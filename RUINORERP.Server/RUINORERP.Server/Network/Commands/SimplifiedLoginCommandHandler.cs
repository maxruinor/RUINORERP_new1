using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 简化登录命令处理器示例
    /// 展示如何使用SimplifiedCommandHandler简化命令处理
    /// </summary>
    // 使用常量表达式替换运行时计算的值
    [CommandHandler("SimplifiedLoginCommandHandler", 0, false, 16777217u)] // AuthenticationCommands.LoginRequest.FullCode的值
    public class SimplifiedLoginCommandHandler : SimplifiedCommandHandler<LoginRequest, LoginResponse>
    {
        /// <summary>
        /// 处理登录请求的核心逻辑
        /// </summary>
        /// <param name="request">登录请求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>登录响应</returns>
        protected override async Task<LoginResponse> ProcessRequestAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            // 验证请求
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                throw new System.Exception("用户名和密码不能为空");
            }

            // 模拟登录处理逻辑
            // 在实际应用中，这里会包含用户验证、令牌生成等逻辑
            await Task.Delay(100, cancellationToken); // 模拟处理时间

            // 创建登录响应
            var response = new LoginResponse
            {
                UserId = 12345,
                Username = request.Username,
                DisplayName = $"用户 {request.Username}",
                SessionId = $"SESSION_{System.Guid.NewGuid():N}",
                AccessToken = $"ACCESS_TOKEN_{System.Guid.NewGuid():N}",
                RefreshToken = $"REFRESH_TOKEN_{System.Guid.NewGuid():N}",
                ExpiresIn = 3600,
                TokenType = "Bearer"
            };

            return response;
        }

        /// <summary>
        /// 获取支持的命令列表
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new List<uint> { 16777217u }; // AuthenticationCommands.LoginRequest.FullCode的值
    }
}