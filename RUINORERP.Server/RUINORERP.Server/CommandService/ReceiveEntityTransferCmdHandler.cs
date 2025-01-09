using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction.CommandService;

namespace RUINORERP.Server.CommandService
{
    [CommandHandler]
    public class ReceiveEntityTransferCmdHandler : ICommandHandler
    {

        public bool CanHandle(IServerCommand command) => command is ReceiveEntityTransferCmd;
        public bool CanHandle(IServerCommand command, BlockingCollection<IServerCommand> queue = null)
        {
            //发送消息
            //不在线不发送。在线后再发送
            bool result = CanHandle(command);
            //if (result && command is ReceiveEntityTransferCmd messageCmd)
            //{
            //    if (messageCmd.ToSession != null) result = true;//messageCmd.ToSession
            //}
            return result;
        }

        public async Task HandleCommandAsync(IServerCommand command, CancellationToken cancellationToken)
        {
            var receiveCmd = command as ReceiveEntityTransferCmd;
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
