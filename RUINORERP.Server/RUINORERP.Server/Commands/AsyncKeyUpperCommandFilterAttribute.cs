using System;
using SuperSocket.Command;
using System.Threading.Tasks;
using SuperSocket.ProtoBase;

namespace RUINORERP.Server.Commands
{
    public class AsyncKeyUpperCommandFilterAttribute : AsyncCommandFilterAttribute
    {
        public override async ValueTask<bool> OnCommandExecutingAsync(CommandExecutingContext commandContext)
        {
            if (commandContext.Package is StringPackageInfo package)
            {
                Console.WriteLine($"Session ip: {commandContext.Session.RemoteEndPoint}, " +
                                  $"Command: {package.Key}");
            }

            await Task.Delay(0);
            return true;
        }

        public override async ValueTask OnCommandExecutedAsync(CommandExecutingContext commandContext)
        {
            await Task.Delay(0);
        }
    }
}
