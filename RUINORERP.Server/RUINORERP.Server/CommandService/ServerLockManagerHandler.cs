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
    /// <summary>
    /// 锁单指令
    /// </summary>
    [CommandHandler]
    public class ServerLockManagerHandler : ICommandHandler
    {
        public bool targetUserOnlineStatus { get; set; } = true;
        public bool CanHandle(IServerCommand command, BlockingCollection<IServerCommand> queue)
        {

            //不在线不发送。在线后再发送
            bool result = command is ServerLockManagerCmd;
            if (result && command is ServerLockManagerCmd messageCmd)
            {
                if (messageCmd.ToSession != null) result = true;
            }
            targetUserOnlineStatus = true;
            return result;
        }

        public async Task HandleCommandAsync(IServerCommand command, CancellationToken cancellationToken)
        {
            var sendCmd = command as ServerLockManagerCmd;
            await sendCmd.ExecuteAsync(cancellationToken);
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
