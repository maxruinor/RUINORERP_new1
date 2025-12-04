using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 登录流程服务
    /// 统一管理整个登录过程，包括重复登录处理
    /// </summary>
    public class LoginFlowService : IDisposable
    {
        private readonly UserLoginService _userLoginService;
        private readonly ConnectionManager _connectionManager;
        private readonly ILogger<LoginFlowService> _logger;
        private LoginFlowContext _currentContext;
        private bool _disposed = false;

        /// <summary>
        /// 登录流程状态变更事件
        /// </summary>
        public event EventHandler<LoginFlowEventArgs> StatusChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginFlowService(
            UserLoginService userLoginService,
            ConnectionManager connectionManager,
            ILogger<LoginFlowService> logger = null)
        {
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _logger = logger;
        }

        /// <summary>
        /// 执行完整的登录流程
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="serverIp">服务器IP</param>
        /// <param name="serverPort">服务器端口</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>登录结果</returns>
        public async Task<LoginResponse> ExecuteLoginFlowAsync(
            string username,
            string password,
            string serverIp,
            int serverPort,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 初始化登录上下文
                _currentContext = new LoginFlowContext
                {
                    Username = username,
                    Password = password,
                    ServerIp = serverIp,
                    ServerPort = serverPort,
                    CancellationToken = cancellationToken,
                    Status = LoginStatus.Connecting
                };

                NotifyStatusChanged(LoginStatus.None);

                // 1. 检查快速登录条件
                var quickLoginResult = await TryQuickLoginAsync();
                if (quickLoginResult != null)
                {
                    return quickLoginResult;
                }

                // 2. 执行标准登录流程
                return await ExecuteStandardLoginFlowAsync();
            }
            catch (OperationCanceledException)
            {
                _currentContext.Status = LoginStatus.Cancelled;
                NotifyStatusChanged(LoginStatus.Failed);
                throw;
            }
            catch (Exception ex)
            {
                _currentContext.Status = LoginStatus.Failed;
                _currentContext.LastError = ex.Message;
                NotifyStatusChanged(LoginStatus.Failed);
                
                _logger?.LogError(ex, "登录流程执行失败");
                throw;
            }
        }

        /// <summary>
        /// 尝试快速登录（基于现有Token和连接）
        /// </summary>
        private async Task<LoginResponse> TryQuickLoginAsync()
        {
            try
            {
                // 检查连接状态
                bool isConnected = _connectionManager.IsConnected &&
                                 _connectionManager.CurrentServerAddress == _currentContext.ServerIp &&
                                 _connectionManager.CurrentServerPort.ToString() == _currentContext.ServerPort.ToString();

                if (!isConnected)
                {
                    return null;
                }

                // 检查现有Token
                string currentToken = await _userLoginService.GetCurrentAccessToken();
                if (string.IsNullOrEmpty(currentToken))
                {
                    return null;
                }

                // 尝试Token验证
                _currentContext.IsQuickLogin = true;
                _currentContext.Status = LoginStatus.Validating;
                NotifyStatusChanged(LoginStatus.Connecting);

                var validationResponse = await _userLoginService.ValidateTokenAsync(currentToken);
                if (validationResponse)
                {
                    _currentContext.Status = LoginStatus.Success;
                    NotifyStatusChanged(LoginStatus.Validating);

                    // 创建成功的登录响应
                    return new LoginResponse
                    {
                        IsSuccess = true,
                        Message = "快速登录成功（基于有效Token）",
                        Username = _currentContext.Username,
                        SessionId = MainForm.Instance?.AppContext?.SessionId
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "快速登录验证失败，将执行标准登录流程");
                return null;
            }
        }

        /// <summary>
        /// 执行标准登录流程
        /// </summary>
        private async Task<LoginResponse> ExecuteStandardLoginFlowAsync()
        {
            // 1. 连接服务器
            if (!_connectionManager.IsConnected)
            {
                _currentContext.Status = LoginStatus.Connecting;
                NotifyStatusChanged(LoginStatus.Validating);

                var connected = await _connectionManager.ConnectAsync(_currentContext.ServerIp, _currentContext.ServerPort);
                if (!connected)
                {
                    throw new Exception("无法连接到服务器");
                }
            }

            // 2. 执行登录验证
            _currentContext.Status = LoginStatus.Validating;
            NotifyStatusChanged(LoginStatus.Connecting);

            var loginResponse = await _userLoginService.LoginAsync(
                _currentContext.Username,
                _currentContext.Password,
                _currentContext.CancellationToken);

            if (loginResponse == null)
            {
                throw new Exception("登录请求失败，服务器未返回响应");
            }

            // 3. 处理重复登录情况
            if (loginResponse.IsSuccess && loginResponse.HasDuplicateLogin && loginResponse.DuplicateLoginResult != null)
            {
                // 检查是否需要用户确认
                if (loginResponse.DuplicateLoginResult.RequireUserConfirmation)
                {
                    _currentContext.Status = LoginStatus.DuplicateLoginConfirming;
                    NotifyStatusChanged(LoginStatus.Validating);

                    // 存在远程重复登录，需要用户确认
                    // 用户交互不受网络请求超时影响
                    var userAction = await HandleDuplicateLoginAsync(loginResponse.DuplicateLoginResult);

                    if (userAction == DuplicateLoginAction.Cancel)
                    {
                        _currentContext.Status = LoginStatus.Cancelled;
                        NotifyStatusChanged(LoginStatus.DuplicateLoginConfirming);
                        return CreateCancelledResponse("用户取消了登录操作");
                    }
                    else if (userAction == DuplicateLoginAction.ForceOfflineOthers)
                    {
                        // 用户选择强制其他登录下线，通知服务器处理
                        // 强制下线操作使用新的短超时令牌，避免用户长时间等待
                        using var forceOfflineCts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                        await _userLoginService.HandleDuplicateLoginAsync(
                        loginResponse.SessionId, 
                        _currentContext.Username, 
                        DuplicateLoginAction.ForceOfflineOthers,
                        forceOfflineCts.Token
                    );
                        _logger?.LogInformation($"用户 {_currentContext.Username} 选择强制其他登录下线");
                    }
                    // 如果用户选择"取消当前登录"，则已经在上一步处理了
                }
                else
                {
                    // 不需要用户确认的重复登录（如本地重复登录），直接继续
                    _logger?.LogDebug($"用户 {_currentContext.Username} 本地重复登录，同一台机器允许多会话，直接继续登录流程");
                }
            }

            // 5. 登录成功处理
            if (loginResponse.IsSuccess)
            {
                _currentContext.Status = LoginStatus.Success;
                NotifyStatusChanged(LoginStatus.Failed);
            }
            else
            {
                _currentContext.Status = LoginStatus.Failed;
                _currentContext.LastError = loginResponse.ErrorMessage;
                NotifyStatusChanged(LoginStatus.Success);
            }

            return loginResponse;
        }



        /// <summary>
        /// 处理重复登录用户交互
        /// </summary>
        private async Task<DuplicateLoginAction> HandleDuplicateLoginAsync(DuplicateLoginResult duplicateResult)
        {
            return await Task.Run(() =>
            {
                // 在UI线程上显示对话框
                if (Application.OpenForms.Count > 0)
                {
                    var mainForm = Application.OpenForms[0];
                    return (DuplicateLoginAction)mainForm.Invoke(new Func<DuplicateLoginAction>(() =>
                    {
                        using var dialog = new DuplicateLoginDialog(duplicateResult);
                        var result = dialog.ShowDialog();
                        return result == DialogResult.OK ? dialog.SelectedAction : DuplicateLoginAction.Cancel;
                    }));
                }

                // 如果没有主窗体，返回默认选项
                return DuplicateLoginAction.Cancel;
            });
        }



        /// <summary>
        /// 创建取消登录的响应
        /// </summary>
        private LoginResponse CreateCancelledResponse(string message)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = message
            };
        }



        /// <summary>
        /// 通知状态变更
        /// </summary>
        private void NotifyStatusChanged(LoginStatus previousStatus)
        {
            StatusChanged?.Invoke(this, new LoginFlowEventArgs(_currentContext, previousStatus));
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _currentContext = null;
        }
    }
}