using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.Server.Network.Models;
using System;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Interfaces.Services
{
    /// <summary>
    /// 客户端响应处理器接口
    /// </summary>
    public interface IClientResponseHandler
    {
        /// <summary>
        /// 处理客户端响应
        /// </summary>
        /// <param name="packet">响应数据包</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>处理结果</returns>
        Task<ResponseProcessingResult> HandleResponseAsync(PacketModel packet, SessionInfo sessionInfo);

        /// <summary>
        /// 注册待处理请求
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <param name="taskCompletionSource">任务完成源</param>
        /// <returns>是否注册成功</returns>
        bool RegisterPendingRequest(string requestId, TaskCompletionSource<PacketModel> taskCompletionSource);

        /// <summary>
        /// 移除待处理请求
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>是否移除成功</returns>
        bool RemovePendingRequest(string requestId);

        /// <summary>
        /// 清理过期的待处理请求
        /// </summary>
        /// <param name="timeoutMinutes">超时分钟数，默认5分钟</param>
        /// <returns>清理的请求数量</returns>
        int CleanupExpiredPendingRequests(int timeoutMinutes = 5);

        /// <summary>
        /// 注册自定义响应处理器
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="handler">处理委托</param>
        void RegisterResponseHandler(CommandId commandId, Func<PacketModel, SessionInfo, Task<ResponseProcessingResult>> handler);

        /// <summary>
        /// 取消注册响应处理器
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>是否取消成功</returns>
        bool UnregisterResponseHandler(CommandId commandId);

        /// <summary>
        /// 获取响应统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        ResponseStatistics GetStatistics();

        /// <summary>
        /// 重置统计信息
        /// </summary>
        void ResetStatistics();
    }
}
