using RUINORERP.PacketSpec.Commands.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Authentication
{
    public interface ITokenRefreshService
    {
        Task<TokenInfo> RefreshTokenAsync(CancellationToken ct = default);
        Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default);
    }
}
