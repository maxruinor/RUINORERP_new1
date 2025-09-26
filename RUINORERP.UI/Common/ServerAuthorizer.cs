using Microsoft.Extensions.Logging;
using Netron.Neon;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RUINORERP.UI.Common
{

    /// <summary>
    /// 服务器登陆授权相关类
    /// </summary>
    internal class ServerAuthorizer
    {


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
        [Obsolete]
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


        public async Task<bool> LoginToServer(
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
                            RUINORERP.Common.Log4Net.Logger.Warn($"连接 {address} 失败");
                        }
                    }
                    if (!_ecs.IsConnected)
                    {
                        throw new Exception($"无法连接到服务器（尝试了 {serverAddresses.Length} 个地址）");
                    }
                }


                // 创建登录请求数据包
                var loginPacket = PacketBuilder.Create()
                    .AsLoginRequest("admin", "password123", "1.0.0")
                    .WithSession("session_abc")
                    .WithRequestId("req_001")
                    .Build();

                // 临时代码：标记需要完善的部分
#warning TODO: 这里需要完善具体逻辑，当前仅为占位
                LoginRequest request = new LoginRequest();
                // 发送登录请求
                //var request = new LoginRequest(CommandDirection.Send)
                //{
                //    OperationType = CommandDirection.Send,
                //    requestType = LoginProcessType.用户登陆,
                //    Username = userName,
                //    Password = password
                //};

                // 创建自定义命令数据包
                var customPacket = PacketBuilder.Create()
                    .WithCommand(MessageCommands.SendPopupMessage)  // 使用预定义的命令
                    .WithPriority(PacketPriority.High)
                    .WithDirection(PacketDirection.ClientToServer)
                    .WithJsonData(new { Title = "通知", Content = "这是一条测试消息" })
                    .Build();

                // 临时代码：标记需要完善的部分
#warning TODO: 这里需要完善具体逻辑，当前仅为占位
                // MainForm.Instance.dispatcher.DispatchAsync(request, cancellationToken);

                // 持久化配置
                UserGlobalConfig.Instance.Serialize();

                return _ecs.client.IsConnected;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                MainForm.Instance.ShowStatusText(GetUserFriendlyError(ex));
                MainForm.Instance.logger.Debug(ex, "登录失败" + ex.Message);
                return false;
            }
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
                            RUINORERP.Common.Log4Net.Logger.Warn($"连接 {address} 失败");
                        }
                    }
                    if (!_ecs.IsConnected)
                    {
                        throw new Exception($"无法连接到服务器（尝试了 {serverAddresses.Length} 个地址）");
                    }
                }
                // 临时代码：标记需要完善的部分
#warning TODO: 这里需要完善具体逻辑，当前仅为占位
                // var request = new RequestLoginCommand(CommandDirection.Send);
                // 发送登录请求
                //var request = new RequestLoginCommand(CommandDirection.Send)
                //{
                //    OperationType = CommandDirection.Send,
                //    requestType = LoginProcessType.用户登陆,
                //    Username = userName,
                //    Password = password
                //};

                //   MainForm.Instance.dispatcher.DispatchAsync(request, cancellationToken);

                // 持久化配置
                UserGlobalConfig.Instance.Serialize();

                return _ecs.client.IsConnected;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                MainForm.Instance.ShowStatusText(GetUserFriendlyError(ex));
                MainForm.Instance.logger.Debug(ex, "登录失败" + ex.Message);
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
