using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using Microsoft.Extensions.Logging;
using System.Data;
using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// ✅ [最新架构] 命令工厂接口 - 定义从各种数据包格式创建统一命令对象的契约
    /// 支持依赖注入和灵活的扩展机制
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// 从统一数据包创建命令
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <returns>命令对象</returns>
        ICommand CreateCommand(PacketModel packet);


        /// <summary>
        /// 注册命令创建器
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="creator">创建器函数</param>
        void RegisterCommandCreator(uint commandCode, Func<PacketModel, ICommand> creator);
        
    }

    /// <summary>
    /// 异步命令工厂接口
    /// </summary>
    public interface ICommandFactoryAsync : ICommandFactory
    {
        /// <summary>
        /// 异步从统一数据包创建命令
        /// </summary>
        /// <param name="packet">统一数据包</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令对象</returns>
        Task<ICommand> CreateCommandAsync(PacketModel packet, CancellationToken cancellationToken = default);

    }


}
