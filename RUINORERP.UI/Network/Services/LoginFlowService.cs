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
            if (!loginResponse.IsSuccess && IsDuplicateLoginResponse(loginResponse))
            {
                _currentContext.Status = LoginStatus.DuplicateLoginConfirming;
                NotifyStatusChanged(LoginStatus.Validating);

                var duplicateLoginInfo = ExtractDuplicateLoginInfo(loginResponse);
                
                // 检查是否为本地重复登录（同一台机器）
                if (duplicateLoginInfo.IsLocalDuplicate && !duplicateLoginInfo.ExistingSessions.Any(s => !s.IsLocal))
                {
                    // 纯本地重复登录：同一台机器，直接允许登录，不需要用户确认和重新登录
                    _logger?.LogDebug($"用户 {_currentContext.Username} 本地重复登录，同一台机器允许多会话，直接继续登录流程");
                    
                    // 创建成功的登录响应
                    var successResponse = new LoginResponse
                    {
                        IsSuccess = true,
                        Message = "本地重复登录成功",
                        Username = _currentContext.Username
                    };
                    
                    return successResponse;
                }

                // 存在远程重复登录，需要用户确认
                var userAction = await HandleDuplicateLoginAsync(duplicateLoginInfo);

                if (userAction == DuplicateLoginAction.Cancel)
                {
                    _currentContext.Status = LoginStatus.Cancelled;
                    NotifyStatusChanged(LoginStatus.DuplicateLoginConfirming);
                    return CreateCancelledResponse("用户取消了登录操作");
                }

                // 4. 重新执行登录（包含用户选择）
                loginResponse = await ExecuteLoginWithUserActionAsync(userAction);
            }

            // 5. 登录成功处理
            if (loginResponse.IsSuccess)
            {
                _currentContext.Status = LoginStatus.Success;
                NotifyStatusChanged(LoginStatus.Failed);

                await PostLoginSuccessAsync(loginResponse);
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
        /// 检查是否为重复登录响应
        /// </summary>
        private bool IsDuplicateLoginResponse(LoginResponse response)
        {
            return !response.IsSuccess &&
                   (response.Message?.Contains("其他地方登录") == true ||
                    response.Message?.Contains("重复登录") == true) &&
                   response.Metadata?.ContainsKey("ExistingSessions") == true;
        }

        /// <summary>
        /// 从登录响应中提取重复登录信息
        /// </summary>
        private DuplicateLoginInfo ExtractDuplicateLoginInfo(LoginResponse response)
        {
            var duplicateInfo = new DuplicateLoginInfo
            {
                Message = response.Message,
                HasDuplicateLogin = true,
                CurrentSessionId = response.SessionId
            };

            // 解析现有会话信息
            if (response.Metadata?.TryGetValue("ExistingSessions", out var sessionsObj) == true &&
                sessionsObj is List<object> sessionsList)
            {
                duplicateInfo.ExistingSessions = sessionsList
                    .OfType<Dictionary<string, object>>()
                    .Select(sessionDict => new ExistingSessionInfo
                    {
                        SessionId = sessionDict.ContainsKey("SessionId") ? sessionDict["SessionId"]?.ToString() : null,
                        ClientIp = sessionDict.ContainsKey("ClientIp") ? sessionDict["ClientIp"]?.ToString() : null,
                        DeviceInfo = sessionDict.ContainsKey("DeviceInfo") ? sessionDict["DeviceInfo"]?.ToString() : null,
                        LoginTime = sessionDict.ContainsKey("LoginTime") && sessionDict["LoginTime"] != null ? Convert.ToDateTime(sessionDict["LoginTime"]) : DateTime.Now,
                        IsLocal = IsLocalSession(sessionDict.ContainsKey("ClientIp") ? sessionDict["ClientIp"]?.ToString() : null, 
                                sessionDict.ContainsKey("IsLocal") ? Convert.ToBoolean(sessionDict["IsLocal"]) : false)
                    })
                    .ToList();
            }

            // 检查是否需要用户确认
            if (response.Metadata?.TryGetValue("RequireUserConfirmation", out var requireConfirmObj) == true)
            {
                bool requireUserConfirmation = Convert.ToBoolean(requireConfirmObj);
                
                // 如果不需要用户确认，且所有会话都是本地的，则认为是本地重复登录
                if (!requireUserConfirmation)
                {
                    duplicateInfo.IsLocalDuplicate = duplicateInfo.ExistingSessions.All(s => s.IsLocal);
                }
                else
                {
                    duplicateInfo.IsLocalDuplicate = false;
                }
            }
            else
            {
                // 兼容旧版本：根据会话信息判断
                duplicateInfo.IsLocalDuplicate = duplicateInfo.ExistingSessions.All(s => s.IsLocal);
            }

            return duplicateInfo;
        }

        /// <summary>
        /// 处理重复登录用户交互
        /// </summary>
        private async Task<DuplicateLoginAction> HandleDuplicateLoginAsync(DuplicateLoginInfo duplicateInfo)
        {
            // 检查是否需要用户确认
            bool requireUserConfirmation = false;
            if (duplicateInfo.HasDuplicateLogin)
            {
                // 如果存在远程会话，需要用户确认
                // 如果只有本地会话，则不需要确认，直接允许登录
                requireUserConfirmation = duplicateInfo.ExistingSessions.Any(s => !s.IsLocal);
            }

            if (!requireUserConfirmation)
            {
                // 本地重复登录，直接允许
                _logger?.LogDebug("本地重复登录，同一台机器允许多会话，直接继续登录");
                return DuplicateLoginAction.ForceOfflineOthers; // 或者直接返回成功
            }

            return await Task.Run(() =>
            {
                // 在UI线程上显示对话框
                if (Application.OpenForms.Count > 0)
                {
                    var mainForm = Application.OpenForms[0];
                    return (DuplicateLoginAction)mainForm.Invoke(new Func<DuplicateLoginAction>(() =>
                    {
                        // 将DuplicateLoginInfo转换为DuplicateLoginResult
                        var duplicateResult = new DuplicateLoginResult
                        {
                            HasDuplicateLogin = duplicateInfo.HasDuplicateLogin,
                            ExistingSessions = duplicateInfo.ExistingSessions,
                            Message = duplicateInfo.Message,
                            // 根据IsLocalDuplicate设置适当的RequireUserConfirmation值
                            RequireUserConfirmation = !duplicateInfo.IsLocalDuplicate
                        };
                        
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
        /// 根据用户选择重新执行登录
        /// </summary>
        private async Task<LoginResponse> ExecuteLoginWithUserActionAsync(DuplicateLoginAction userAction)
        {
            // 创建包含用户选择的登录请求
            var loginRequest = LoginRequest.Create(_currentContext.Username, _currentContext.Password);
            loginRequest.AdditionalData = new Dictionary<string, object>
            {
                ["DuplicateLoginConfirmed"] = true,
                ["DuplicateLoginAction"] = userAction.ToString()
            };

            // 重新发送登录请求
            var response = await _userLoginService.LoginAsync(
                _currentContext.Username,
                _currentContext.Password,
                _currentContext.CancellationToken);

            return response;
        }

        /// <summary>
        /// 登录成功后的后续处理
        /// </summary>
        private async Task PostLoginSuccessAsync(LoginResponse response)
        {
            try
            {
                // 这里可以添加登录成功后的处理逻辑
                // 例如：缓存同步、权限加载等
                _logger?.LogInformation($"用户 {_currentContext.Username} 登录成功");

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "登录成功后处理过程中发生异常，但不影响登录结果");
            }
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
        /// 判断是否为本地会话
        /// </summary>
        private bool IsLocalSession(string clientIp, object isLocalFlag = null)
        {
            // 优先使用服务器提供的IsLocal标志
            if (isLocalFlag != null && isLocalFlag is bool isLocal)
            {
                return isLocal;
            }

            if (string.IsNullOrEmpty(clientIp))
                return false;

            // 检查是否为本地IP地址
            var localIps = new[] { "127.0.0.1", "localhost", "::1" };
            return localIps.Contains(clientIp.ToLower()) || clientIp.StartsWith("192.168.") || clientIp.StartsWith("10.");
        }

        /// <summary>
        /// 通知状态变更
        /// </summary>
        private void NotifyStatusChanged(LoginStatus previousStatus)
        {
            StatusChanged?.Invoke(this, new LoginFlowEventArgs(_currentContext, previousStatus));
        }

        /// <summary>
        /// 获取当前登录上下文
        /// </summary>
        public LoginFlowContext GetCurrentContext() => _currentContext;

        /// <summary>
        /// 取消当前登录流程
        /// </summary>
        public void CancelLogin()
        {
            try
            {
                if (_currentContext?.CancellationToken.CanBeCanceled == true)
                {
                    // 这里需要配合外部的CancellationTokenSource
                    _currentContext.Status = LoginStatus.Cancelled;
                    NotifyStatusChanged(_currentContext.Status);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "取消登录流程时发生异常");
            }
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