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
    public class ReceiveReminderCmdHandler : ICommandHandler
    {
        public bool CanHandle(IServerCommand command) => command is ReceiveReminderCmd;
     

        public async Task HandleCommandAsync(IServerCommand command, CancellationToken cancellationToken)
        {
            var receiveCmd = command as ReceiveReminderCmd;
            await receiveCmd.ExecuteAsync(cancellationToken);
        }
        //public void HandleCommand(IServerCommand command)
        //{
        //    var receiveCmd = command as ReceiveReminderCmd;
        //    if (receiveCmd != null)
        //    {
        //        receiveCmd.Execute();
        //    }
        //}
        //public void HandleCommand(object parameters)
        //{
        //    //var loginParams = parameters as receiveCmdParameters;
        //    // 处理登录指令
        //}
    }
}
