using Microsoft.Extensions.Logging;
using Netron.Neon;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.ClientCmdService;
using RUINORERP.UI.SuperSocketClient;
using SourceGrid2.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.CommandService;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
        /// 验证是否已经登陆并等待响应，最多等待指定的超时时间。
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="timeOutSec">超时时间（秒）</param>
        /// <returns>如果登录成功，返回true；否则返回false。</returns>
        public async Task<bool> AlreadyloggedinAsync(EasyClientService _ecs, string userName, int timeOutSec)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                var startTime = DateTime.Now;

                // 等待服务器响应，直到超时或收到登录状态
                while (!_ecs.LoginStatus && Program.AppContextData.AlreadyLogged)
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

                return _ecs.LoginStatus && Program.AppContextData.AlreadyLogged; // 登录状态已更新，返回结果
            }
        }



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
                bool rs = await LoginServerByEasyClient(_ecs, userName, password, tokenSource.Token);

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

        [Obsolete]
        public async Task<bool> LoginServerByEasyClient11(EasyClientService _ecs, string userName, string password, CancellationToken cancellationToken)
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



                //OriginalData od1 = ActionForClient.UserLogin(userName, password);
                //byte[] buffer1 = CryptoProtocol.EncryptClientPackToServer(od1);
                //_ecs.client.Send(buffer1);

                RequestLoginCommand request = new RequestLoginCommand(CmdOperation.Send);
                request.requestType = LoginProcessType.用户登陆;
                request.Username = userName;
                request.Password = password;
                MainForm.Instance.dispatcher.DispatchAsync(request, cancellationToken);
                rs = _ecs.client.IsConnected;
                UserGlobalConfig.Instance.Serialize();
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex.Message);
                MainForm.Instance.ShowStatusText(ex.Message);
                rs = false;
            }
            return rs;
        }


        public async Task<bool> LoginServerByEasyClient(
            EasyClientService _ecs,
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            const int DnsTimeoutSeconds = 5;
            const int ConnectTimeoutSeconds = 10;

            try
            {
                // 参数校验
                if (string.IsNullOrWhiteSpace(UserGlobalConfig.Instance.ServerIP))
                {
                    throw new ArgumentException("服务器地址不能为空");
                }

                // 新建客户端实例（如果需要）
                _ecs ??= new EasyClientService();
                if (!_ecs.IsConnected)
                {

                    // 智能解析服务器地址
                    IPAddress[] serverAddresses;
                    if (IPAddress.TryParse(UserGlobalConfig.Instance.ServerIP, out var ip))
                    {
                        // 直接使用IP地址
                        serverAddresses = new[] { ip };
                    }
                    else
                    {
                        // 域名解析（带超时和取消）
                        var dnsTask = Dns.GetHostAddressesAsync(UserGlobalConfig.Instance.ServerIP);
                        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(DnsTimeoutSeconds), cancellationToken);

                        var completedTask = await Task.WhenAny(dnsTask, timeoutTask);

                        if (completedTask == timeoutTask)
                        {
                            throw new TimeoutException($"DNS解析超时（{DnsTimeoutSeconds}秒）");
                        }

                        serverAddresses = (await dnsTask)
                            .Where(addr => addr.AddressFamily == AddressFamily.InterNetwork ||
                                            addr.AddressFamily == AddressFamily.InterNetworkV6)
                            .ToArray();

                        if (serverAddresses.Length == 0)
                        {
                            throw new SocketException((int)SocketError.HostNotFound);
                        }
                    }

                    // 尝试连接所有解析到的地址
                    foreach (var address in serverAddresses)
                    {
                        try
                        {
                            _ecs.ServerIp = address.ToString();
                            _ecs.Port = UserGlobalConfig.Instance.ServerPort.ToInt();

                            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ConnectTimeoutSeconds)))
                            {
                                var connectTask = _ecs.Connect();
                                await connectTask.WaitAsync(cts.Token); // 使用自定义WaitAsync
                            }

                            if (_ecs.IsConnected) break;
                        }
                        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                        {
                            throw; // 用户取消
                        }
                        catch
                        {
                            // 记录失败日志，继续尝试下一个地址
                            MainForm.Instance.logger.LogWarning($"连接 {address} 失败");
                        }
                    }
                    if (!_ecs.IsConnected)
                    {
                        throw new Exception($"无法连接到服务器（尝试了 {serverAddresses.Length} 个地址）");
                    }
                }

                // 发送登录请求
                var request = new RequestLoginCommand(CmdOperation.Send)
                {
                    OperationType = CmdOperation.Send,
                    requestType = LoginProcessType.用户登陆,
                    Username = userName,
                    Password = password
                };

                MainForm.Instance.dispatcher.DispatchAsync(request, cancellationToken);

                // 持久化配置
                UserGlobalConfig.Instance.Serialize();

                return _ecs.client.IsConnected;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                MainForm.Instance.ShowStatusText(GetUserFriendlyError(ex));
                MainForm.Instance.logger.LogError(ex, "登录失败");
                return false;
            }
        }

        // 辅助方法：生成友好错误信息
        private string GetUserFriendlyError(Exception ex)
        {
            return ex switch
            {
                SocketException { SocketErrorCode: SocketError.HostNotFound } =>
                    $"找不到主机：{UserGlobalConfig.Instance.ServerIP}",
                SocketException { SocketErrorCode: SocketError.ConnectionRefused } =>
                    "连接被服务器拒绝",
                TimeoutException =>
                    $"操作超时，请检查网络连接",
                ArgumentException =>
                    ex.Message,
                _ =>
                    $"登录失败：{ex.Message}"
            };
        }


        // 辅助方法：生成友好错误信息
        private static string GetFriendlyErrorMessage(Exception ex)
        {
            return ex switch
            {
                SocketException { SocketErrorCode: SocketError.HostNotFound } => "服务器地址无法解析",
                SocketException { SocketErrorCode: SocketError.ConnectionRefused } => "连接被服务器拒绝",
                TimeoutException => "连接超时，请检查网络",
                _ => ex.Message
            };
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
