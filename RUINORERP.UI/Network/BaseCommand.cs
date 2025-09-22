using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端命令基类
    /// 实现ICommand接口的基本功能
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionID { get; set; }

        /// <summary>
        /// 命令唯一标识（字符串形式）
        /// </summary>
        public virtual string CommandId => CommandIdentifier.ToString();

        /// <summary>
        /// 命令标识符（类型安全命令系统）
        /// </summary>
        public abstract CommandId CommandIdentifier { get; }

        /// <summary>
        /// 命令方向
        /// 客户端命令默认是客户端到服务器
        /// </summary>
        public CommandDirection Direction { get; set; } = CommandDirection.Send;

        /// <summary>
        /// 命令优先级
        /// 默认普通优先级
        /// </summary>
        public CommandPriority Priority { get; set; } = CommandPriority.Normal;

        /// <summary>
        /// 命令状态
        /// </summary>
        public CommandStatus Status { get; set; } = CommandStatus.Created;

        /// <summary>
        /// 原始数据包
        /// </summary>
        public OriginalData OriginalData { get; set; }

        /// <summary>
        /// 命令创建时间
        /// </summary>
        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        /// <summary>
        /// 命令超时时间（毫秒）
        /// 默认30秒
        /// </summary>
        public int TimeoutMs { get; set; } = 30000;

        /// <summary>
        /// 执行命令
        /// 客户端命令通常只构建命令，不执行逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                Status = CommandStatus.Processing;
                var result = await OnExecuteAsync(cancellationToken);
                Status = CommandStatus.Completed;
                return result;
            }
            catch (Exception ex)
            {
                Status = CommandStatus.Failed;
                return CommandResult.Failure(ex.Message);
            }
        }

        /// <summary>
        /// 验证命令是否可以执行
        /// </summary>
        /// <returns>验证结果</returns>
        public virtual bool Validate()
        {
            // 默认实现：检查命令是否已经处理完成
            return Status != CommandStatus.Completed && Status != CommandStatus.Failed;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// 子类需要实现此方法来提供命令的序列化数据
        /// </summary>
        /// <returns>可序列化的对象</returns>
        protected abstract object GetSerializableData();

        /// <summary>
        /// 命令执行的具体逻辑
        /// 子类需要实现此方法来提供命令的执行逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        protected abstract Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken);
    }
}