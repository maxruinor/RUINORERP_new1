using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Authentication
{
    /// <summary>
    /// Token刷新服务接口
    /// 提供Token刷新和验证功能
    /// </summary>
    public interface ITokenRefreshService
    {
        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>新的Token信息</returns>
        Task<TokenInfo> RefreshTokenAsync(CancellationToken ct = default);
        
        /// <summary>
        /// 验证Token是否有效
        /// </summary>
        /// <param name="token">要验证的Token</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>Token是否有效</returns>
        Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default);
    }

}
