using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction.CommandService;

namespace RUINORERP.Server.CommandService
{
    /// <summary>
    /// 用户登陆
    /// </summary>
    public class LoginCommand : IServerCommand
    {

        public LoginCommand()
        {

        }


        public string Username { get; set; }
        public string Password { get; set; }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // 登录逻辑
            //await Task.Run(() => session.Login(), cancellationToken);
            // 登录逻辑，例如验证用户名和密码
            // 模拟登录逻辑，这里可以替换为实际的登录逻辑
            await Task.Run(() =>
            {
                // 登录逻辑，例如验证用户名和密码
                if (Username == "admin" && Password == "password")
                {
                    Console.WriteLine("Login successful");
                }
                else
                {
                    Console.WriteLine("Login failed");
                }
            }, cancellationToken);
        }
        public void Execute()
        {
            // 执行登录逻辑
            // 同步执行登录逻辑
            ExecuteAsync(CancellationToken.None).Wait();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
