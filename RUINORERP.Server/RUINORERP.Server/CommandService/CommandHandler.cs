using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction.CommandService;

namespace RUINORERP.Server.CommandService
{


    [CommandHandler]
    public class LoginCommandHandler : ICommandHandler
    {

        public bool CanHandle(IServerCommand command) => command is LoginCommand;


        public async Task HandleCommandAsync(IServerCommand command, CancellationToken cancellationToken)
        {
            var loginCommand = command as LoginCommand;
            await loginCommand.ExecuteAsync(cancellationToken);
        }
        public void HandleCommand(IServerCommand command)
        {
            var loginCommand = command as LoginCommand;
            if (loginCommand != null)
            {
                loginCommand.Execute();
            }
        }
        public void HandleCommand(object parameters)
        {
            var loginParams = parameters as LoginCommandParameters;
            // 处理登录指令
        }
    }

    [CommandHandler]
    public class AddProductCommandHandler : ICommandHandler
    {
        public async Task HandleCommandAsync(IServerCommand command, CancellationToken cancellationToken)
        {
            var addProductCommand = command as AddProductCommand;
            await addProductCommand.ExecuteAsync(cancellationToken);
        }
        public void HandleCommand(IServerCommand command)
        {

            var addProductCommand = command as AddProductCommand;
            if (addProductCommand != null)
            {
                addProductCommand.Execute();
            }
        }

        public bool CanHandle(IServerCommand command)
        {
            throw new NotImplementedException();
        }

        public void Handle(IServerCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
