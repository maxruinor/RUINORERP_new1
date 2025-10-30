using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Interfaces.Services
{
    /// <summary>
    /// 通用广播服务接口
    /// 负责向客户端广播通用请求数据
    /// </summary>
    public interface IGeneralBroadcastService
    {
        /// <summary>
        /// 向所有客户端广播请求数据（无需等待响应）
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        Task BroadcastToAllClients(ushort commandId, GeneralRequest request);

        /// <summary>
        /// 向特定会话广播请求数据（无需等待响应）
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        Task BroadcastToSession(string sessionId, ushort commandId, GeneralRequest request);

        /// <summary>
        /// 向特定用户组广播请求数据（无需等待响应）
        /// </summary>
        /// <param name="userGroup">用户组</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        Task BroadcastToUserGroup(string userGroup, ushort commandId, GeneralRequest request);
        
        /// <summary>
        /// 向所有客户端发送请求并等待响应
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>响应数据列表</returns>
        Task<GeneralResponse[]> SendRequestToAllClients(ushort commandId, GeneralRequest request);

        /// <summary>
        /// 向特定会话发送请求并等待响应
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>响应数据</returns>
        Task<GeneralResponse> SendRequestToSession(string sessionId, ushort commandId, GeneralRequest request);

        /// <summary>
        /// 向特定用户组发送请求并等待响应
        /// </summary>
        /// <param name="userGroup">用户组</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求数据</param>
        /// <returns>响应数据列表</returns>
        Task<GeneralResponse[]> SendRequestToUserGroup(string userGroup, ushort commandId, GeneralRequest request);
    }
}