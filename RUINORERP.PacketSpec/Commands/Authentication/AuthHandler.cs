using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RUINORERP.PacketSpec.Commands.Authentication
{


   //应该是服务器端代码   一个类一个处理类统一处理。合并处理
    [CommandHandler("AuthHandler", priority: 0,
     AuthenticationCommands.Login,
     AuthenticationCommands.Logout,
     AuthenticationCommands.RefreshToken)]
    public sealed class AuthHandler : BaseCommandHandler
    {
        protected override Task<CommandResult> OnHandleAsync(ICommand cmd, CancellationToken ct)
        {
            // 统一转基类
            var generic = (ICommand)cmd;
            var payload = generic.GetSerializableData();   // 就是 LoginPayLoad / LogoutPayLoad …

            return cmd.CommandIdentifier switch
            {
                var id when id == AuthenticationCommands.Login =>
                    HandleLogin((LoginPayLoad)payload),

                var id when id == AuthenticationCommands.Logout =>
                    HandleLogout((LogoutPayLoad)payload),

                _ => Task.FromResult(CommandResult.Failure("未实现"))
            };
        }
    }
}
