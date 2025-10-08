using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
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
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>初始化结果</returns>
        Task<bool> InitializeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步分发命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        Task<ResponseBase> DispatchAsync(PacketModel Packet, ICommand command, CancellationToken cancellationToken = default);

        /// <summary>
        /// 注册命令类型
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="commandType">命令类型</param>
        void RegisterCommandType(uint commandCode, Type commandType);

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令类型，如果找不到则返回null</returns>
        Type GetCommandType(uint commandCode);

        /// <summary>
        /// 创建命令实例
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令实例，如果找不到类型或创建失败则返回null</returns>
        ICommand CreateCommand(uint commandCode);
 

        /// <summary>
        /// 清理注册的命令类型
        /// </summary>
        void ClearCommandTypes();
    }
}
