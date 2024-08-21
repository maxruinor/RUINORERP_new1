using System;
using System.Linq;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
 
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using RUINORERP.Server.ServerSession;

namespace RUINORERP.Server.Commands
{
    /// <summary>
    /// getmsg命令
    /// </summary>
    [Command(Key = "getmsg")]
    [AsyncKeyUpperCommandFilter]
    public class getmsgCommand : IAsyncCommand<LanderPackageInfo>
    {
        public async ValueTask ExecuteAsync(IAppSession session, LanderPackageInfo package)
        {
            
            await Task.Delay(0);
            var result = GetResult(package);
            //发送消息给客户端 处理命令
            string Ver = "test2021cmd";
            string sends = "true\n";
            sends += Ver + " 启动时间:" + DateTime.Now + "\n";
            sends += "在线人数:";
            int usercnt = 100;
            sends += usercnt.ToString();
            sends += "\n";
            //if (System.IO.File.Exists("kx2.exe") == true)
            //{
            //    sends += "[有新程序等待更新]\n";
            //}
            sends += "服务器介绍";
            /// await (this as IAppSession).SendAsync(Tools.StrToBytes(sends));
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            await session.SendAsync(System.Text.Encoding.GetEncoding("GB2312").GetBytes(sends + "\r\n"));
        }

        protected string GetResult(LanderPackageInfo package)
        {
            string result;
            try
            {
                result = package.Key;
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }
    }
}
