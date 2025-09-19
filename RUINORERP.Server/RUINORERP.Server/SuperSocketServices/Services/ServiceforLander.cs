using System;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.Server.SuperSocketServices.Services
{
    /// <summary>
    /// ⚠️ [已过时] 登录服务处理类 - 旧架构实现
    /// 已由新架构 Network/Commands/LoginCommandHandler.cs 替代
    /// 
    /// 迁移说明:
    /// - 认证服务已迁移到: LoginCommandHandler.cs 中的认证逻辑
    /// - 会话管理已迁移到: RUINORERP.Server.Network.Services.UnifiedSessionManager
    /// - 登录逻辑已迁移到: RUINORERP.Server.Network.Commands.LoginCommandHandler
    /// 
    /// 建议使用新架构进行开发，此服务将在未来版本中移除
    /// </summary>
    [Obsolete("此服务已过时，请使用 Network/Commands/LoginCommandHandler.cs 替代", false)]
    public class ServiceforLander
        where TReceivePackageInfo : class
    {
        private readonly List<IAppSession> _appSessions;
        private readonly CancellationTokenSource _tokenSource;
        private string ServiceName => "[ServiceforLander]";

        
        
        public ServiceforLander(IServiceProvider serviceProvider, IOptions<ServerOptions> serverOptions)
            : base(serviceProvider, serverOptions)
        {
            _appSessions = new List<IAppSession>();
            _tokenSource = new CancellationTokenSource();
        }

        protected override async ValueTask OnSessionConnectedAsync(IAppSession session)
        {
            Console.WriteLine($@"{DateTime.Now} {ServiceName} " +
                              $@"登陆器Session connected: {session.RemoteEndPoint}.");

            lock (_appSessions)
            {
                _appSessions.Add(session);
            }

            await base.OnSessionConnectedAsync(session);
        }

        protected override async ValueTask OnSessionClosedAsync(IAppSession session, CloseEventArgs e)
        {
            Console.WriteLine($@"{DateTime.Now} {ServiceName} " +
                              $@"登陆器Session {session.RemoteEndPoint} closed : {e.Reason}.");

            lock (_appSessions)
            {
                _appSessions.Remove(session);
            }

            await base.OnSessionClosedAsync(session, e);
        }

        protected override async ValueTask OnStartedAsync()
        {
            Console.WriteLine($@"{DateTime.Now} {ServiceName} started.");
            StatisticAsync(_tokenSource.Token, 5000).GetAwaiter();
            await Task.Delay(0);
        }

        protected override async ValueTask OnStopAsync()
        {
            _tokenSource.Cancel();
            Console.WriteLine($@"{DateTime.Now} {ServiceName} stop.");
            await Task.Delay(0);
        }

        private async Task StatisticAsync(CancellationToken token, int interval)
        {
            while (true)
            {
                try
                {
                    //超时等待
                    await Task.Delay(interval, token);
                }
                catch (Exception)
                {
                    break;
                }

                if (token.IsCancellationRequested)
                {
                    break;
                }

                //统计数据
                var sessionList = new List<string>();
                lock (_appSessions)
                {
                    foreach (var session in _appSessions)
                    {
                        sessionList.Add(session.RemoteEndPoint.ToString());
                    }
                }

                if (sessionList.Count == 0)
                {
                    Console.WriteLine($@"{DateTime.Now} {ServiceName} No session connected.");
                    continue;
                }

                var sb = new StringBuilder();
                sb.AppendLine($@"{DateTime.Now} {ServiceName} {sessionList.Count} 登陆器sessions connected:");
                foreach (var session in sessionList)
                {
                    sb.AppendLine($"\t{session}");
                }

                Console.WriteLine(sb.ToString());
            }
        }
    }
}
