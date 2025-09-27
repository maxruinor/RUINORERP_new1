using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 广播锁状态命令 - 服务器向所有客户端广播锁状态变化
    /// </summary>
    [PacketCommand("BroadcastLockStatus", CommandCategory.Lock)]
    public class BroadcastLockStatusCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => LockCommands.BroadcastLockStatus;

        /// <summary>
        /// 锁定的单据信息列表
        /// </summary>
        public List<LockedInfo> LockedDocuments { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BroadcastLockStatusCommand()
        {
            Direction = CommandDirection.Send;
            TimeoutMs = 30000; // 默认超时时间30秒
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lockedDocuments">锁定的单据信息列表</param>
        public BroadcastLockStatusCommand(List<LockedInfo> lockedDocuments)
        {
            LockedDocuments = lockedDocuments;
            Direction = CommandDirection.Send;
            TimeoutMs = 30000; // 默认超时时间30秒
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的广播数据</returns>
        public override object GetSerializableData()
        {
            return this.LockedDocuments;
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

            // 锁定文档列表可以为空，但不能为null
            if (LockedDocuments == null)
            {
                LockedDocuments = new List<LockedInfo>();
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 执行命令的核心逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        protected override Task<ResponseBase> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 广播锁状态命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            // 创建并返回成功响应
            return Task.FromResult(ResponseBase.CreateSuccess("广播锁状态操作成功"));
        }
    }
}