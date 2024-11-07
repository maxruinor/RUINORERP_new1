using Microsoft.Extensions.Logging;
using RUINORERP.Common.Extensions;
using RUINORERP.UI.SuperSocketClient;
using SourceGrid2.Win32;
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

        ///// <summary>
        ///// 3秒超时返回
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="password"></param>
        ///// <param name="timeOutSec"></param>
        ///// <returns></returns>
        //public async Task LongRunningOperationAsync(EasyClientService _ecs, string userName, string password, int timeOutSec)
        //{
        //    using (var tokenSource = new CancellationTokenSource())
        //    {
        //        var startTime = DateTime.Now;
        //        // 在任务执行期间定期检查 CancellationToken，
        //        //發送帳號密碼
        //        LoginServerByEasyClient(_ecs, userName, password);

        //        //等待授权成功后需要1秒钟
        //        await Task.Delay(TimeSpan.FromSeconds(1));

        //        // 如果它已取消，可以安全地退出。
        //        while (!_ecs.LoginStatus)
        //        {
        //            if ((DateTime.Now - startTime) >= TimeSpan.FromSeconds(timeOutSec))
        //            {
        //                tokenSource.Cancel();
        //            }
        //            if (tokenSource.IsCancellationRequested)
        //            {
        //                return;
        //            }
        //            await Task.Delay(TimeSpan.FromSeconds(2));
        //        }
        //    }
        //}

        /// <summary>
        /// 登录服务器并等待响应，最多等待指定的超时时间。
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="timeOutSec">超时时间（秒）</param>
        /// <returns>如果登录成功，返回true；否则返回false。</returns>
        public async Task<bool> loginRunningOperationAsync(EasyClientService _ecs, string userName, string password, int timeOutSec)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                var startTime = DateTime.Now;

                // 发送用户名和密码到服务器
                bool rs = await LoginServerByEasyClient(_ecs, userName, password);

                // 等待服务器响应，直到超时或收到登录状态
                while (!_ecs.LoginStatus && rs)
                {
                    if ((DateTime.Now - startTime) >= TimeSpan.FromSeconds(timeOutSec))
                    {
                        tokenSource.Cancel();
                    }

                    if (tokenSource.IsCancellationRequested)
                    {
                        return false; // 登录超时，返回false
                    }

                    await Task.Delay(TimeSpan.FromSeconds(2), tokenSource.Token); // 等待2秒再次检查
                }

                return _ecs.LoginStatus && rs; // 登录状态已更新，返回结果
            }
        }

        public async Task<bool> LoginServerByEasyClient(EasyClientService _ecs, string userName, string password)
        {
            bool rs = false;
            try
            {

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

                //TransPackProcess tpp = new TransPackProcess();

                if (_ecs == null)
                {
                    _ecs = new EasyClientService();
                }
                if (!_ecs.IsConnected)
                {
                    //ecs.ServerIp = "127.0.0.1";
                    _ecs.ServerIp = UserGlobalConfig.Instance.ServerIP;
                    _ecs.Port = UserGlobalConfig.Instance.ServerPort.ToInt();
                    rs = await _ecs.Connect();
                }

                //连接上准备
                //OriginalData od = ActionForClient.UserReayLogin();
                //byte[] buffer = CryptoProtocol.EncryptClientPackToServer(od);
                //_ecs.client.Send(buffer);

                OriginalData od1 = ActionForClient.UserLogin(userName, password);
                byte[] buffer1 = CryptoProtocol.EncryptClientPackToServer(od1);
                _ecs.client.Send(buffer1);

                rs = _ecs.client.IsConnected;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex.Message);
                MainForm.Instance.ShowStatusText(ex.Message);
                rs = false;
            }
            return rs;
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
