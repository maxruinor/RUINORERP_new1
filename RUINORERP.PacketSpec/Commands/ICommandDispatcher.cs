using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令调度器接口
    /// 定义命令调度器的核心功能契约
    /// 作为客户端和服务器端命令调度器的统一抽象
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// 初始化调度器
        /// 扫描并注册命令处理器
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>初始化结果</returns>
        Task<bool> InitializeAsync(CancellationToken cancellationToken = default, params Assembly[] assemblies);

        /// <summary>
        /// 异步分发数据包
        /// </summary>
        /// <param name="Packet">数据包对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        Task<IResponse> DispatchAsync(PacketModel Packet, CancellationToken cancellationToken = default);



        /// <summary>
        /// 清理注册的命令类型
        /// </summary>
        void ClearCommandTypes();
    }
}
