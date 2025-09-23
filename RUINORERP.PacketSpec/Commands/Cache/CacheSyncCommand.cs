using RUINORERP.PacketSpec.Models.Core;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Cache
{
    /// <summary>
    /// 缓存同步命令 - 用于同步客户端与服务器之间的缓存数据
    /// </summary>
    [PacketCommand("CacheSync", CommandCategory.Cache)]
    public class CacheSyncCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => CacheCommands.CacheSync;

        /// <summary>
        /// 需要同步的缓存键列表
        /// </summary>
        public List<string> CacheKeys { get; set; }

        /// <summary>
        /// 同步模式
        /// </summary>
        public string SyncMode { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheSyncCommand()
        {
            CacheKeys = new List<string>();
            SyncMode = "FULL"; // 默认全量同步
            Direction = CommandDirection.Send;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheKeys">缓存键列表</param>
        /// <param name="syncMode">同步模式</param>
        public CacheSyncCommand(List<string> cacheKeys, string syncMode = "FULL")
        {
            CacheKeys = cacheKeys ?? new List<string>();
            SyncMode = syncMode;
            Direction = CommandDirection.Send;
        }

        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            var result = base.Validate();
            if (!result.IsValid)
            {
                return result;
            }

            // 验证同步模式
            if (string.IsNullOrWhiteSpace(SyncMode))
            {
                return CommandValidationResult.Failure("同步模式不能为空", "INVALID_SYNC_MODE");
            }

            // 验证缓存键列表
            if (CacheKeys == null)
            {
                return CommandValidationResult.Failure("缓存键列表不能为空", "INVALID_CACHE_KEYS");
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 执行命令的核心逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        protected override Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 缓存同步命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            var result = CommandResult.Success(
                data: new { CacheKeys = CacheKeys, SyncMode = SyncMode },
                message: "缓存同步命令构建成功"
            );
            return Task.FromResult(result);
        }
    }
}