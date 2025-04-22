using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RUINORERP.Server.Comm;
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

            // 每2分钟清理一次连接记录
            _cleanupTimer = new Timer(_ =>
            {
                _connectionAttempts.Clear();
                BlacklistManager.CleanupExpiredBans();
            }, null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));
        }

        private readonly ConcurrentDictionary<string, int> _connectionAttempts = new ConcurrentDictionary<string, int>();
        private readonly Timer _cleanupTimer;

        protected override async ValueTask OnSessionConnectedAsync(IAppSession session)
        {
            Console.WriteLine($@"{DateTime.Now} {ServiceName} " +
                              $@"Session connected: {session.RemoteEndPoint}.");

            lock (_appSessions)
            {
                _appSessions.Add(session);
            }
            #region
            
            if (session.RemoteEndPoint is IPEndPoint iP)
            {
                var ip = iP.Address.ToString();
                // 记录断开次数
                _connectionAttempts.AddOrUpdate(ip, 1, (_, count) => count + 1);

                // 检查是否超过阈值（例如60秒2分钟内断开5次）
                if (_connectionAttempts.TryGetValue(ip, out var attempts) && attempts >= 5)
                {
                    BlacklistManager.BanIp(ip, TimeSpan.FromHours(1)); // 封禁1小时
                    _connectionAttempts.TryRemove(ip, out _); // 重置计数
                }

            }
            #endregion

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
