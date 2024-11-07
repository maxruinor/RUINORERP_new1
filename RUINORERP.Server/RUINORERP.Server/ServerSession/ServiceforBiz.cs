using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SuperSocket;
using SuperSocket.Connection;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Session;

namespace RUINORERP.Server.ServerSession
{
    public class ServiceforBiz<BizPackageInfo> : SuperSocketService<BizPackageInfo>
        where BizPackageInfo : class
    {
     
        private readonly List<IAppSession> _appSessions;
        private readonly CancellationTokenSource _tokenSource;
        private string ServiceName => "[ServiceforBiz]";

        public ServiceforBiz(IServiceProvider serviceProvider, IOptions<ServerOptions> serverOptions)
            : base(serviceProvider, serverOptions)
        {
            _appSessions = new List<IAppSession>();
            _tokenSource = new CancellationTokenSource();
        }

        protected override async ValueTask OnSessionConnectedAsync(IAppSession session)
        {
            Console.WriteLine($@"{DateTime.Now} {ServiceName} " +
                              $@"Session connected: {session.RemoteEndPoint}.");

            lock (_appSessions)
            {
                _appSessions.Add(session);
            }

            await base.OnSessionConnectedAsync(session);
        }

        protected override async ValueTask OnSessionClosedAsync(IAppSession session, CloseEventArgs e)
        {
            Console.WriteLine($@"{DateTime.Now} {ServiceName} " +
                              $@"Session {session.RemoteEndPoint} closed : {e.Reason}.");

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
                sb.AppendLine($@"{DateTime.Now} {ServiceName} {sessionList.Count} sessions connected:");
                foreach (var session in sessionList)
                {
                    sb.AppendLine($"\t{session}");
                }

                Console.WriteLine(sb.ToString());
            }
        }
    }
}
