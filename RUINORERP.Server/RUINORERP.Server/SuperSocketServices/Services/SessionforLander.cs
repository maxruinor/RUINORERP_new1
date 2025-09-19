using System;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SuperSocket;
using SuperSocket.Server.Abstractions.Session;
using SuperSocket.Server;
using SuperSocket.Connection;

namespace RUINORERP.Server.ServerSession
{
    /// <summary>
    /// ⚠️ [已过时] 登录会话处理类 - 旧架构实现
    /// 已由新架构 Network/Services/UnifiedSessionManager.cs 替代
    /// 
    /// 迁移说明:
    /// - 会话管理已迁移到: RUINORERP.Server.Network.Services.UnifiedSessionManager
    /// - 认证会话已迁移到: RUINORERP.Server.Network.Models.AuthSessionState
    /// - 会话存储已迁移到: RUINORERP.Server.Network.Services.SessionStorageService
    /// 
    /// 建议使用新架构进行开发，此会话类将在未来版本中移除
    /// </summary>
    [Obsolete("此会话类已过时，请使用 Network/Services/UnifiedSessionManager.cs 替代", false)]
    public class SessionforLander : AppSession
    {
        private string SessionName => "[SessionforLander]";

        //private readonly ILog _log;
        //{
        //    _log = LogManager.GetLogger(typeof(SessionforLander));
        //}

        private const String Ver = "V5.211028";
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        protected override async ValueTask OnSessionConnectedAsync()
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        {
            Console.WriteLine($@"{DateTime.Now} {SessionName} New Session connected: {RemoteEndPoint}.");
            // string rs = "true";
            //发送消息给客户端
            string sends = "true\n";
            sends += Ver + " 启动时间:" + DateTime.Now + "\n";
            sends += "在线人数:";
            int usercnt = 1110;
            sends += usercnt.ToString();
            sends += "\n";
            //if (System.IO.File.Exists("kx2.exe") == true)
            //{
            //    sends += "[有新程序等待更新]\n";
            //}
            sends += "服务器介绍";
            // await (this as IAppSession).SendAsync(Tools.StrToBytes(sends));
            // await (this as IAppSession).SendAsync(Encoding.UTF8.GetBytes(msg + "\n"));
            //await (this as IAppSession).SendAsync(Encoding.UTF8.GetBytes(msg + "\r\n"));

            //Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} " +
            //              $@"New Session connected: {RemoteEndPoint}.");
            //await Task.Delay(0);

        }

        protected override async ValueTask OnSessionClosedAsync(CloseEventArgs e)
        {
            Console.WriteLine($@"{DateTime.Now} {SessionName} Session {RemoteEndPoint} closed: {e.Reason}.");
            await Task.Delay(0);
        }

        public void ReceiveData(float temperature, short humidity)
        {
            Console.WriteLine($@"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} Receive data: " +
                              $@"temperature = {temperature}℃, " +
                              $@"humidity = {humidity}%.");
        }

    }
}
