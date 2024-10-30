using Microsoft.Extensions.Logging;
using RUINORERP.Common.Extensions;
using RUINORERP.UI.SuperSocketClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction;

namespace RUINORERP.UI.Common
{

    /// <summary>
    /// 服务器登陆授权相关类
    /// </summary>
    internal class ServerAuthorizer
    {

        /// <summary>
        /// 3秒超时返回
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="timeOutSec"></param>
        /// <returns></returns>
        public async Task LongRunningOperationAsync(EasyClientService _ecs, string userName, string password, int timeOutSec)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                var startTime = DateTime.Now;
                // 在任务执行期间定期检查 CancellationToken，
                //發送帳號密碼
                LoginServerByEasyClient(_ecs, userName, password);
                // 如果它已取消，可以安全地退出。
                while (!_ecs.LoginStatus)
                {
                    if ((DateTime.Now - startTime) >= TimeSpan.FromSeconds(timeOutSec))
                    {
                        tokenSource.Cancel();
                    }
                    if (tokenSource.IsCancellationRequested)
                    {
                        return;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
            }
        }


        public async void LoginServerByEasyClient(EasyClientService _ecs, string userName, string password)
        {
            try
            {
                //ip
                /*
                if (!IPAddress.TryParse(txtServerIP.Text, out myip))
                {
                    IPHostEntry ipHostInfo = Dns.GetHostEntry("192.168.0.254");
                    if (myip == null)
                    {
                        //ipHostInfo = Dns.GetHostEntry(txtServerIP.Text);
                        ipHostInfo = Dns.GetHostEntry("192.168.0.254");
                    }

                    myip = ipHostInfo.AddressList[0];
                }
                else
                {
                    //主机名
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(txtServerIP.Text);
                    myip = ipHostInfo.AddressList[0];
                }
                */
                IPHostEntry ipHostInfo = null;
                if (IsIpAddress(UserGlobalConfig.Instance.ServerIP))
                {
                    ipHostInfo = Dns.GetHostEntry(UserGlobalConfig.Instance.ServerIP);
                }
                else if (IsHostname(UserGlobalConfig.Instance.ServerIP))
                {
                    //主机名
                    ipHostInfo = Dns.GetHostEntry(UserGlobalConfig.Instance.ServerIP);
                    UserGlobalConfig.Instance.ServerIP = ipHostInfo.AddressList[0].ToString();
                }
                else
                {
                    //默认一个
                    ipHostInfo = Dns.GetHostEntry("192.168.0.254");
                    UserGlobalConfig.Instance.ServerIP = "192.168.0.254";
                }

                TransPackProcess tpp = new TransPackProcess();

                if (_ecs == null)
                {
                    _ecs = new EasyClientService();
                }
                if (!_ecs.IsConnected)
                {
                    //ecs.ServerIp = "127.0.0.1";
                    _ecs.ServerIp = UserGlobalConfig.Instance.ServerIP;
                    _ecs.Port = UserGlobalConfig.Instance.ServerPort.ToInt();
                    await _ecs.Connect();

                }

                //连接上准备
                OriginalData od = ActionForClient.UserReayLogin();

                byte[] buffer = Tool4DataProcess.HexStrTobyte(tpp.ClientPackingAsHexString(od));
                //socketSession.AddSendData(od);
                _ecs.client.Send(buffer);

                OriginalData od1 = ActionForClient.UserLogin(userName, password);
                byte[] buffer1 = Tool4DataProcess.HexStrTobyte(tpp.ClientPackingAsHexString(od1));
                _ecs.client.Send(buffer1);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex.Message);
                //MainForm.Instance.PrintInfoLog(ex.Message);
            }
        }


        static bool IsIpAddress(string ipString)
        {
            return IPAddress.TryParse(ipString, out _);
        }

        static bool IsHostname(string hostname)
        {
            if (string.IsNullOrEmpty(hostname))
            {
                return false;
            }
            try
            {
                var dnsEntry = Dns.GetHostEntry(hostname);
                return true;
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is System.Net.Sockets.SocketException)
            {
                return false;
            }
        }


    }
}
