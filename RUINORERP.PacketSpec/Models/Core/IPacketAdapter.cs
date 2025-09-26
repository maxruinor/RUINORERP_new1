using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 数据包适配器接口 - 用于在业务DTO和传输层PacketModel之间进行转换
    /// 实现业务代码对传输层细节的无感知
    /// </summary>
    /// <typeparam name="TReq">请求DTO类型</typeparam>
    /// <typeparam name="TResp">响应DTO类型</typeparam>
    public interface IPacketAdapter<TReq, TResp>
    {
        /// <summary>
        /// 将请求DTO转换为PacketModel
        /// </summary>
        /// <param name="request">请求DTO对象</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>转换后的PacketModel实例</returns>
        PacketModel Pack(TReq request, string clientId, string sessionId);
        
        /// <summary>
        /// 将PacketModel转换为响应DTO
        /// </summary>
        /// <param name="packet">接收到的PacketModel实例</param>
        /// <returns>转换后的响应DTO对象</returns>
        TResp Unpack(PacketModel packet);
    }
}