using System;
using System.Windows.Forms;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Commands;
using SuperSocket.ClientEngine;

namespace RUINORERP.UI.Network.Examples
{
    /// <summary>
    /// WinForm通信集成示例类
    /// 提供在WinForms环境下如何使用CommunicationManager进行网络通信的完整示例
    /// </summary>
    public class WinFormCommunicationIntegration : IDisposable
    {
        private readonly CommunicationManager _communicationManager;
        private readonly Form _mainForm;
        private bool _isDisposed = false;

        /// <summary>
        /// 初始化WinFormCommunicationIntegration类的新实例
        /// </summary>
        /// <param name="communicationManager">通信管理器实例</param>
        /// <param name="mainForm">主窗体实例，用于UI线程同步</param>
        public WinFormCommunicationIntegration(CommunicationManager communicationManager, Form mainForm)
        {
            _communicationManager = communicationManager ?? throw new ArgumentNullException(nameof(communicationManager));
            _mainForm = mainForm ?? throw new ArgumentNullException(nameof(mainForm));

            // 注册事件处理程序
            RegisterEvents();
        }

        /// <summary>
        /// 注册所有需要的事件处理程序
        /// </summary>
        private void RegisterEvents()
        {
            _communicationManager.ConnectionStatusChanged += OnConnectionStatusChanged;
            _communicationManager.ErrorOccurred += OnErrorOccurred;
            _communicationManager.CommandReceived += OnCommandReceived;
        }

        /// <summary>
        /// 取消注册所有事件处理程序
        /// </summary>
        private void UnregisterEvents()
        {
            _communicationManager.ConnectionStatusChanged -= OnConnectionStatusChanged;
            _communicationManager.ErrorOccurred -= OnErrorOccurred;
            _communicationManager.CommandReceived -= OnCommandReceived;
        }

        /// <summary>
        /// 初始化通信模块
        /// </summary>
        public void Initialize()
        {
            try
            {
                // 配置自动重连参数
                _communicationManager.AutoReconnect = true;
                _communicationManager.ReconnectDelay = 5000; // 5秒后尝试重连
                _communicationManager.MaxReconnectAttempts = 10; // 最大重连尝试次数

                // 可以在这里添加其他初始化逻辑
                LogMessage("通信模块初始化成功");
            }
            catch (Exception ex)
            {
                LogError("初始化通信模块时出错: " + ex.Message);
            }
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="serverUrl">服务器地址</param>
        /// <param name="port">服务器端口</param>
        public async void Connect(string serverUrl, int port)
        {
            try
            {
                LogMessage($"正在连接到服务器: {serverUrl}:{port}");
                await _communicationManager.ConnectAsync(serverUrl, port);
            }
            catch (Exception ex)
            {
                LogError($"连接服务器失败: {ex.Message}");
                // 可以在这里添加重新连接或其他错误处理逻辑
            }
        }

        /// <summary>
        /// 断开与服务器的连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                _communicationManager.Disconnect();
                LogMessage("已断开与服务器的连接");
            }
            catch (Exception ex)
            {
                LogError($"断开连接时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送命令到服务器
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        public async void SendCommand(CommandId commandId, object data)
        {
            try
            {
                if (!_communicationManager.IsConnected)
                {
                    LogError("未连接到服务器，无法发送命令");
                    return;
                }

                LogMessage($"正在发送命令: {commandId}");
                await _communicationManager.SendCommandAsync(commandId, data);
            }
            catch (Exception ex)
            {
                LogError($"发送命令时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理连接状态变化事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e)
        {
            // 使用Invoke确保在UI线程上更新UI
            _mainForm.Invoke((MethodInvoker)delegate
            {
                string statusText = e.IsConnected ? "已连接" : "已断开连接";
                LogMessage($"连接状态已更改为: {statusText}");
                
                // 可以在这里添加UI更新逻辑，如更改连接状态指示器
                // 例如：_connectionStatusLabel.Text = statusText;
                //       _connectionStatusLabel.ForeColor = e.IsConnected ? Color.Green : Color.Red;
            });
        }

        /// <summary>
        /// 处理错误事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnErrorOccurred(object sender, ErrorEventArgs e)
        {
            // 使用Invoke确保在UI线程上更新UI
            _mainForm.Invoke((MethodInvoker)delegate
            {
                LogError($"发生错误: {e.Error.Message}");
                
                // 可以根据错误类型添加不同的处理逻辑
                // 例如：显示错误对话框或记录到日志系统
            });
        }

        /// <summary>
        /// 处理接收到的命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        private void OnCommandReceived(CommandId commandId, object data)
        {
            // 使用Invoke确保在UI线程上更新UI
            _mainForm.Invoke((MethodInvoker)delegate
            {
                LogMessage($"接收到命令: {commandId}");
                
                // 在这里添加针对不同命令的处理逻辑
                switch (commandId)
                {
                    case CommandId.RefreshData:
                        HandleRefreshDataCommand(data);
                        break;
                    case CommandId.ShowNotification:
                        HandleShowNotificationCommand(data);
                        break;
                    // 可以添加更多的命令处理逻辑
                    default:
                        LogMessage($"未处理的命令: {commandId}");
                        break;
                }
            });
        }

        /// <summary>
        /// 处理刷新数据命令
        /// </summary>
        /// <param name="data">刷新数据命令的数据</param>
        private void HandleRefreshDataCommand(object data)
        {
            try
            {
                LogMessage("处理刷新数据命令");
                // 这里添加刷新数据的逻辑
                // 例如：重新加载数据、更新UI等
            }
            catch (Exception ex)
            {
                LogError($"处理刷新数据命令时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理显示通知命令
        /// </summary>
        /// <param name="data">显示通知命令的数据</param>
        private void HandleShowNotificationCommand(object data)
        {
            try
            {
                LogMessage("处理显示通知命令");
                // 这里添加显示通知的逻辑
                string message = data?.ToString() ?? "收到通知";
                
                // 在状态栏显示通知或使用消息框
                MessageBox.Show(_mainForm, message, "系统通知", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogError($"处理显示通知命令时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 记录普通消息
        /// </summary>
        /// <param name="message">消息内容</param>
        private void LogMessage(string message)
        {
            // 可以根据实际需要修改日志记录方式
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] {message}");
            
            // 如果需要在UI上显示日志，可以添加相应的逻辑
            // 例如：_logTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
        }

        /// <summary>
        /// 记录错误消息
        /// </summary>
        /// <param name="message">错误消息内容</param>
        private void LogError(string message)
        {
            // 可以根据实际需要修改错误记录方式
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] {message}");
            
            // 如果需要在UI上显示错误日志，可以添加相应的逻辑
            // 例如：_logTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] ERROR: {message}\r\n");
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源的实际实现
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // 取消注册事件
                    UnregisterEvents();
                    
                    // 断开连接
                    try
                    {
                        _communicationManager.Disconnect();
                    }
                    catch { }
                }
                
                _isDisposed = true;
            }
        }
    }
}