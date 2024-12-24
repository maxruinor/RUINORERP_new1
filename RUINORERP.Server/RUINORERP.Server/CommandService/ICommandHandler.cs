using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
namespace RUINORERP.Server.CommandService
{
    //直接用接口来识别了
    public class CommandHandlerAttribute : Attribute { }

    public interface ICommandHandlerFactory
    {
        ICommandHandler CreateHandler(Type commandType);
    }

    public interface ICommandHandler<TCommand> where TCommand : IServerCommand
    {
        void Handle(TCommand command);
    }

    /// <summary>
    /// 服务器指令处理器接口
    /// 接口则定义了指令处理器的基本结构和行为，它包含了 HandleCommand 和 HandleCommandAsync 方法，
    /// 用于处理指令。这个接口的主要目的是提供一个统一的方式来注册和调用不同类型的指令处理器。
    /// 用于定义指令处理器的基本结构和行为
    /// </summary>
    public interface ICommandHandler
    {
        //void HandleCommand(IServerCommand command);
        Task HandleCommandAsync(IServerCommand command, CancellationToken cancellationToken);

        /// <summary>
        /// 比较发送消息。如果对方不在线。就不能处理。要等下一次？
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool CanHandle(IServerCommand command);
    }



}
