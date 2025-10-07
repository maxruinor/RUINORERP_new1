using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Core;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token刷新命令 - 简化版
    /// 服务端自动管理Token状态，客户端无需传递Token信息
    /// </summary>
    [PacketCommand("RefreshToken", CommandCategory.Authentication)]
    public class RefreshTokenCommand : BaseCommand<TokenRefreshRequest, LoginResponse>
    {
        /// <summary>
        /// 刷新请求数据（简化版，无需传递Token）
        /// </summary>
        public TokenRefreshRequest RefreshRequest
        {
            get => Request;
            set => Request = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RefreshTokenCommand() : base(PacketDirection.ClientToServer)
        {
            CommandIdentifier = AuthenticationCommands.RefreshToken;
            Priority = CommandPriority.High;
            TimeoutMs = 30000;
            // 简化版：创建空的刷新请求
            Request = new TokenRefreshRequest();
        }

        /// <summary>
        /// 自动附加认证Token
        /// 刷新命令不需要自动附加Token，因为服务端会自动管理
        /// </summary>
        protected override void AutoAttachToken()
        {
            // 刷新命令不需要自动附加Token，因为服务端会自动管理
            // 重写基类方法，空实现
        }

        /// <summary>
        /// 验证命令数据
        /// 简化版：无需验证Token信息
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            // 简化版：直接返回成功，服务端负责Token验证
            return await base.ValidateAsync(cancellationToken);
        }
    }
}
