using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;

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
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>命令对象</returns>
        ICommand CreateCommand(PacketModel packet);

        /// <summary>
        /// 从OriginalData创建命令
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>命令对象</returns>
        ICommand CreateCommand(OriginalData originalData);

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
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令对象</returns>
        Task<ICommand> CreateCommandAsync(PacketModel packet,  CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步从OriginalData创建命令
        /// </summary>
        /// <param name="originalData">原始数据</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令对象</returns>
        Task<ICommand> CreateCommandAsync(OriginalData originalData, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 默认命令工厂实现
    /// </summary>
    public class DefaultCommandFactory : ICommandFactoryAsync
    {
        /// <summary>
        /// 从统一数据包创建命令
        /// </summary>
        public ICommand CreateCommand(PacketModel packet)
        {
            // 默认实现，返回null，由具体实现类重写
            return null;
        }
     

        /// <summary>
        /// 从OriginalData创建命令
        /// </summary>
        public ICommand CreateCommand(OriginalData originalData)
        {
            // 默认实现，返回null，由具体实现类重写
            return null;
        }

        /// <summary>
        /// 异步从统一数据包创建命令
        /// </summary>
        public async Task<ICommand> CreateCommandAsync(PacketModel packet, CancellationToken cancellationToken = default)
        {
            // 异步包装同步方法
            return await Task.Run(() => CreateCommand(packet), cancellationToken);
        }

        /// <summary>
        /// 异步从OriginalData创建命令
        /// </summary>
        public async Task<ICommand> CreateCommandAsync(OriginalData originalData, CancellationToken cancellationToken = default)
        {
            // 异步包装同步方法
            return await Task.Run(() => CreateCommand(originalData), cancellationToken);
        }

        /// <summary>
        /// 注册命令创建器
        /// </summary>
        public void RegisterCommandCreator(uint commandCode, Func<PacketModel,  ICommand> creator)
        {
            // 默认实现为空，由具体实现类重写
        }
    }
}
