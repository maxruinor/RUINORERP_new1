using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 默认命令工厂 - 现已重构为 CommandCreationService 的代理
    /// 保留此类以保持向后兼容性，所有功能已迁移到 CommandCreationService
    /// </summary>
    public class DefaultCommandFactory : ICommandFactoryAsync, ICommandFactory
    {
        private readonly CommandCreationService _creationService;
        private readonly ILogger<DefaultCommandFactory> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="creationService">命令创建服务</param>
        /// <param name="logger">日志记录器</param>
        public DefaultCommandFactory(CommandCreationService creationService, ILogger<DefaultCommandFactory> logger = null)
        {
            _creationService = creationService ?? throw new ArgumentNullException(nameof(creationService));
            _logger = logger;
            _logger?.LogInformation("DefaultCommandFactory 已初始化为 CommandCreationService 的代理");
        }

        /// <summary>
        /// 创建命令（同步）
        /// </summary>
        /// <param name="packet">数据包模型</param>
        /// <returns>命令实例</returns>
        public ICommand CreateCommand(PacketModel packet)
        {
            _logger?.LogDebug("DefaultCommandFactory.CreateCommand 委托给 CommandCreationService");
            return _creationService.CreateCommand(packet);
        }

        /// <summary>
        /// 异步创建命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>命令实例</returns>
        public async Task<ICommand> CreateCommandAsync(string commandId, Dictionary<string, object> parameters = null)
        {
            _logger?.LogDebug("DefaultCommandFactory.CreateCommandAsync(string) 委托给 CommandCreationService");
            return await _creationService.CreateCommandAsync(commandId, parameters);
        }

        /// <summary>
        /// 异步创建命令
        /// </summary>
        /// <param name="packet">数据包模型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令实例</returns>
        public async Task<ICommand> CreateCommandAsync(PacketModel packet, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("DefaultCommandFactory.CreateCommandAsync(PacketModel) 委托给 CommandCreationService");
            return await _creationService.CreateCommandAsync(packet, cancellationToken);
        }

        /// <summary>
        /// 注册命令创建器
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="creator">创建器函数</param>
        public void RegisterCommandCreator(CommandId commandCode, Func<PacketModel, ICommand> creator)
        {
            _logger?.LogDebug("DefaultCommandFactory.RegisterCommandCreator 委托给 CommandCreationService");
            _creationService.RegisterCommandCreator(commandCode, creator);
        }

        /// <summary>
        /// 获取缓存统计信息（新增功能）
        /// </summary>
        /// <returns>缓存统计信息</returns>
        public Dictionary<string, object> GetCacheStatistics()
        {
            return _creationService.GetCacheStatistics();
        }

        /// <summary>
        /// 清理缓存（新增功能）
        /// </summary>
        public void ClearCache()
        {
            _logger?.LogInformation("DefaultCommandFactory.ClearCache 委托给 CommandCreationService");
            _creationService.ClearCache();
        }

        /// <summary>
        /// 清理过期缓存（新增功能）
        /// </summary>
        public void ClearExpiredCache()
        {
            _logger?.LogInformation("DefaultCommandFactory.ClearExpiredCache 委托给 CommandCreationService");
            _creationService.ClearExpiredCache();
        }
    }
}
