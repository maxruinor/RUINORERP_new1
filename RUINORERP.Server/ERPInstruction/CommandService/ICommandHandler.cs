using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

namespace TransInstruction.CommandService
{
    
    public class CommandHandlerAttribute : Attribute { }

    public class LoginCommandParameters
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public interface ICommandHandler<TCommand> where TCommand : IServerCommand
    {
        void Handle(TCommand command);
    }

    public interface ICommandHandlerFactory
    {
        ICommandHandler CreateHandler(Type commandType);
    }



   


    /// <summary>
    /// 指令处理器接口
    /// </summary>
    public interface ICommandHandler
    {
        void HandleCommand(IServerCommand command);
        Task HandleCommandAsync(IServerCommand command, CancellationToken cancellationToken);
        bool CanHandle(IServerCommand command);
    }



}
