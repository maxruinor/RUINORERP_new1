using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 服务器命令接口
    /// </summary>
    public interface IServerCommand
    {
        CommandOperation OperationType { get; set; }
        object DataPacket { get; set; }
        Task ExecuteAsync(CancellationToken cancellationToken);
        bool AnalyzeDataPacket(object dataPacket, object session);
        void BuildDataPacket(object request = null);
        bool Validate();
        string GetDescription();
    }

    /// <summary>
    /// 命令处理器接口
    /// </summary>
    public interface ICommandHandler<in TCommand> where TCommand : IServerCommand
    {
        Task<CommandResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
        bool CanHandle(TCommand command);
        int Priority { get; }
    }

    /// <summary>
    /// 命令操作类型
    /// </summary>
    public enum CommandOperation
    {
        Unknown = 0,
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4,
        Execute = 5,
        Query = 6,
        Validate = 7,
        Notify = 8,
        Broadcast = 9,
        Response = 10
    }
}