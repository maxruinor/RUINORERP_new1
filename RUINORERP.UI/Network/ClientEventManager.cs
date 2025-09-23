using RUINORERP.PacketSpec.Commands;
using System;
using System.Threading;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端事件管理器
    /// 管理客户端的各种事件
    /// </summary>
    public class ClientEventManager
    {
        /// <summary>
        /// 当接收到服务器命令时触发的事件
        /// </summary>
        public event Action<CommandId, object> CommandReceived;

        /// <summary>
        /// 当连接状态改变时触发的事件
        /// </summary>
        public event Action<bool> ConnectionStatusChanged;

        /// <summary>
        /// 当发生错误时触发的事件
        /// </summary>
        public event Action<Exception> ErrorOccurred;

        /// <summary>
        /// 当连接关闭时触发的事件
        /// </summary>
        public event Action ConnectionClosed;

        /// <summary>
        /// 触发命令接收事件
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        public void OnCommandReceived(CommandId commandId, object data)
        {
            try
            {
                CommandReceived?.Invoke(commandId, data);
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new Exception($"处理命令接收事件时出错: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 触发连接状态改变事件
        /// </summary>
        /// <param name="isConnected">是否连接</param>
        public void OnConnectionStatusChanged(bool isConnected)
        {
            try
            {
                ConnectionStatusChanged?.Invoke(isConnected);
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new Exception($"处理连接状态改变事件时出错: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 触发错误事件
        /// </summary>
        /// <param name="ex">异常</param>
        public void OnErrorOccurred(Exception ex)
        {
            try
            {
                ErrorOccurred?.Invoke(ex);
            }
            catch
            {
                // 忽略错误处理中的异常，避免无限循环
            }
        }

        /// <summary>
        /// 触发连接关闭事件
        /// </summary>
        public void OnConnectionClosed()
        {
            try
            {
                ConnectionClosed?.Invoke();
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new Exception($"处理连接关闭事件时出错: {ex.Message}", ex));
            }
        }
    }
}