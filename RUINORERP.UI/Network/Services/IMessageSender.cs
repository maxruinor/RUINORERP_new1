using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands;
using System.Threading;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 消息发送器接口
    /// 提供发送各种类型消息的抽象方法
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// 发送带响应的命令
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="command">命令标识符</param>
        /// <param name="request">请求对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>响应对象</returns>
        Task<TResponse> SendCommandWithResponseAsync<TResponse>(CommandId command, MessageRequest request, CancellationToken cancellationToken = default) where TResponse : class, RUINORERP.PacketSpec.Models.Responses.IResponse;
    }
}