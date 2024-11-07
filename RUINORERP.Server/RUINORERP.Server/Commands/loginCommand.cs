using System;
using System.Linq;
using SuperSocket.Command;
using SuperSocket.ProtoBase;

using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using RUINORERP.Server.ServerSession;
using TransInstruction;
using System.Threading;
using SuperSocket.Server.Abstractions.Session;


namespace RUINORERP.Server.Commands
{
    /// <summary>
    /// getmsg命令
    /// </summary>
    [Command(Key = "login")]
    [AsyncKeyUpperCommandFilter]
    public class loginCommand : IAsyncCommand<LanderPackageInfo>
    {
        public async ValueTask ExecuteAsync(IAppSession session, LanderPackageInfo package, CancellationToken cancellationToken)
        {
            //  "login|gm|123123";
            await Task.Delay(0);
            var result = GetResult(package);
            //发送消息给客户端 处理命令
            if (result.Length <= 3)
            {
                await session.SendAsync(TransInstruction.Tool4DataProcess.StrToBytes("false\n用户名或密码错误，太短了吧。"));
            }

            string[] ss = result.Split('|',StringSplitOptions.None);
            //在游戏中查找这个账号
           // S游戏玩家 player = new S游戏玩家();
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            string 加密串;
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            string msg = string.Empty;
          
            //if (UserService.Login(ss[1].Trim(), ss[2].Trim(), out 加密串, out msg) == false)
            //{
            //    await session.SendAsync(Tools.StrToBytes("false\n用户名或密码错"));
            //}
            //else
            //{
            //    await session.SendAsync(Tools.StrToBytes("true\n" + 加密串));
            //}
            ////if (System.IO.File.Exists("kx2.exe") == true)
            ////{
            ////    sends += "[有新程序等待更新]\n";
            ////}
            //sends += "服务器介绍";
            /// await (this as IAppSession).SendAsync(Tools.StrToBytes(sends));
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // await session.SendAsync(System.Text.Encoding.GetEncoding("GB2312").GetBytes(sends + "\r\n"));
        }

        protected string GetResult(LanderPackageInfo package)
        {
            string result;
            try
            {
                result = package.Body;
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }
    }
}
