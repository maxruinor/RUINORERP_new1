using RUINORERP.Model;
using RUINORERP.Server.BizService;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataPortal;

namespace RUINORERP.Server.CommandService
{


    [CommandHandler]
    public class LoginCommandHandler : ICommandHandler
    {
        public bool CanHandle(IServerCommand command, BlockingCollection<IServerCommand> queue = null) => command is LoginCommand;

        public async Task HandleCommandAsync(IServerCommand command, CancellationToken cancellationToken)
        {
            var loginCommand = command as LoginCommand;
            loginCommand.OnLoginSuccess += LoginCommand_OnLoginSuccess;
            loginCommand.OnLoginFailure += LoginCommand_OnLoginFailure;
            await loginCommand.ExecuteAsync(cancellationToken);

        }

        /// <summary>
        /// 登陆成功后。给客户端一个回复
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void LoginCommand_OnLoginSuccess(bool arg1, LoginCommand arg2)
        {
            try
            {
                ByteBuff tx = new ByteBuff(100);
                if (arg2.user != null)
                {
                    tx.PushBool(arg1);
                    tx.PushString(arg2.RequestSession.SessionID);
                    tx.PushInt64(arg2.user.User_ID);
                    tx.PushString(arg2.user.UserName);
                    tx.PushString(arg2.user.tb_employee.Employee_Name);
                }
                else
                {
                    tx.PushBool(arg1);
                }
                arg2.RequestSession.AddSendData((byte)ServerCmdEnum.用户登陆回复, null, tx.toByte());
                UserService.发送在线列表(arg2.RequestSession);
                UserService.发送缓存信息列表(arg2.RequestSession);
            }
            catch (Exception ex)
            {

            }
        }

        private void LoginCommand_OnLoginFailure()
        {

        }




        //public void HandleCommand(IServerCommand command)
        //{
        //    var loginCommand = command as LoginCommand;
        //    if (loginCommand != null)
        //    {
        //        loginCommand.Execute();
        //    }
        //}
        //public void HandleCommand(object parameters)
        //{

        //    // 处理登录指令
        //}
    }

}
