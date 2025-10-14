using RUINORERP.PacketSpec.Commands;
using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Core;
using System.Collections.Generic;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 客户端事件管理器
    /// </summary>
    public class ClientEventManager
    {
        // 用于事件触发的同步锁
        private readonly object _lock = new object();
        // 日志记录器（可选）
        private readonly ILogger _logger;

        /// <summary>
        /// 当接收到服务器命令时触发的事件
        /// </summary>
        public event Action<PacketModel, object> CommandReceived;

        /// <summary>
        /// 当连接状态改变时触发的事件
        /// 参数isConnected为true表示连接成功，为false表示连接断开
        /// </summary>
        public event Action<bool> ConnectionStatusChanged;

        /// <summary>
        /// 当发生错误时触发的事件
        /// 参数ex包含错误的详细信息
        /// </summary>
        public event Action<Exception> ErrorOccurred;

        /// <summary>
        /// 当连接关闭时触发的事件
        /// </summary>
        public event Action ConnectionClosed;

        /// <summary>
        /// 当重连失败时触发的事件
        /// </summary>
        public event Action ReconnectFailed;

        public event Action<string, TimeSpan> RequestCompleted; // 新增：请求完成事件
        
        // 专门用于服务器推送命令的事件
        public event Action<PacketModel, object> ServerPushCommandReceived;
        
        // 专门用于特定命令的事件
        private readonly Dictionary<CommandId, Action<PacketModel, object>> _specificCommandHandlers = 
            new Dictionary<CommandId, Action<PacketModel, object>>();

        public void OnRequestCompleted(string requestId, TimeSpan duration)
        {
            RequestCompleted?.Invoke(requestId, duration);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ClientEventManager() : this(null)
        {
        }

        /// <summary>
        /// 构造函数（带日志记录器）
        /// </summary>
        /// <param name="logger">日志记录器，用于记录事件处理过程中的异常和信息</param>
        public ClientEventManager(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 订阅特定命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="handler">处理函数</param>
        public void SubscribeCommand(CommandId commandId, Action<PacketModel, object> handler)
        {
            lock (_lock)
            {
                if (_specificCommandHandlers.ContainsKey(commandId))
                {
                    _specificCommandHandlers[commandId] += handler;
                }
                else
                {
                    _specificCommandHandlers[commandId] = handler;
                }
            }
        }
        
        /// <summary>
        /// 取消订阅特定命令
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="handler">处理函数</param>
        public void UnsubscribeCommand(CommandId commandId, Action<PacketModel, object> handler)
        {
            lock (_lock)
            {
                if (_specificCommandHandlers.ContainsKey(commandId))
                {
                    _specificCommandHandlers[commandId] -= handler;
                    if (_specificCommandHandlers[commandId] == null)
                    {
                        _specificCommandHandlers.Remove(commandId);
                    }
                }
            }
        }
        
        /// <summary>
        /// 触发服务器推送命令事件
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <param name="data">数据</param>
        public void OnServerPushCommandReceived(PacketModel packet, object data)
        {
            ServerPushCommandReceived?.Invoke(packet, data);
            
            // 触发特定命令处理
            lock (_lock)
            {
                if (_specificCommandHandlers.ContainsKey(packet.CommandId))
                {
                    _specificCommandHandlers[packet.CommandId]?.Invoke(packet, data);
                }
            }
        }

        /// <summary>
        /// 触发命令接收事件
        /// </summary>
        /// <param name="commandId">命令ID，标识接收到的命令类型</param>
        /// <param name="data">命令数据，包含命令的具体内容</param>
        public void OnCommandReceived(PacketModel packetModel, object data)
        {
            // 获取事件处理程序的快照，避免在多线程环境下触发时可能发生的问题
            Action<PacketModel, object> handler;
            lock (_lock)
            {
                handler = CommandReceived;
            }

            if (handler == null)
                return;

            try
            {
                // 触发事件
                handler.Invoke(packetModel, data);
            }
            catch (Exception ex)
            {
                // 记录异常并触发错误事件
                LogException(ex, "处理命令接收事件时出错");
                OnErrorOccurred(new Exception($"处理命令接收事件时出错: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 触发连接状态改变事件
        /// </summary>
        /// <param name="isConnected">连接状态，true表示已连接，false表示未连接</param>
        public void OnConnectionStatusChanged(bool isConnected)
        {
            // 获取事件处理程序的快照
            Action<bool> handler;
            lock (_lock)
            {
                handler = ConnectionStatusChanged;
            }

            if (handler == null)
                return;

            try
            {
                // 触发事件
                handler.Invoke(isConnected);
            }
            catch (Exception ex)
            {
                // 记录异常并触发错误事件
                LogException(ex, "处理连接状态改变事件时出错");
                OnErrorOccurred(new Exception($"处理连接状态改变事件时出错: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 触发错误事件
        /// </summary>
        /// <param name="ex">异常对象，包含错误的详细信息</param>
        public void OnErrorOccurred(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex), "异常对象不能为空");

            // 获取事件处理程序的快照
            Action<Exception> handler;
            lock (_lock)
            {
                handler = ErrorOccurred;
            }

            if (handler == null)
            {
                // 如果没有注册错误处理程序，则记录到日志
                LogException(ex, "未处理的异常");
                return;
            }

            try
            {
                // 触发事件
                handler.Invoke(ex);
            }
            catch (Exception innerEx)
            {
                // 忽略错误处理中的异常，避免无限循环
                // 但记录到日志以便排查问题
                LogException(innerEx, "处理错误事件时发生的异常");
            }
        }

        /// <summary>
        /// 触发连接关闭事件
        /// </summary>
        public void OnConnectionClosed()
        {
            // 获取事件处理程序的快照
            Action handler;
            lock (_lock)
            {
                handler = ConnectionClosed;
            }

            if (handler == null)
                return;

            try
            {
                // 触发事件
                handler.Invoke();
            }
            catch (Exception ex)
            {
                // 记录异常并触发错误事件
                LogException(ex, "处理连接关闭事件时出错");
                OnErrorOccurred(new Exception($"处理连接关闭事件时出错: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 触发重连失败事件
        /// </summary>
        public void OnReconnectFailed()
        {
            // 获取事件处理程序的快照
            Action handler;
            lock (_lock)
            {
                handler = ReconnectFailed;
            }

            if (handler == null)
                return;

            try
            {
                // 触发事件
                handler.Invoke();
            }
            catch (Exception ex)
            {
                // 记录异常并触发错误事件
                LogException(ex, "处理重连失败事件时出错");
                OnErrorOccurred(new Exception($"处理重连失败事件时出错: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// 移除所有已注册的事件处理程序
        /// 重置事件管理器到初始状态
        /// </summary>
        public void ClearAllHandlers()
        {
            lock (_lock)
            {
                CommandReceived = null;
                ConnectionStatusChanged = null;
                ErrorOccurred = null;
                ConnectionClosed = null;
                ReconnectFailed = null;
                ServerPushCommandReceived = null;
                _specificCommandHandlers.Clear();
            }
        }

        /// <summary>
        /// 检查是否有命令订阅者
        /// </summary>
        /// <param name="packetModel">数据包模型</param>
        /// <returns>是否有订阅者</returns>
        public bool HasCommandSubscribers(PacketModel packetModel)
        {
            lock (_lock)
            {
                return CommandReceived != null || ServerPushCommandReceived != null ||
                       _specificCommandHandlers.ContainsKey(packetModel.CommandId);
            }
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="message">自定义消息</param>
        private void LogException(Exception ex, string message)
        {
            try
            {
                if (_logger != null)
                {
                    _logger.LogError(ex, message);
                }
            }
            catch
            {
                // 忽略日志记录过程中的异常
            }
        }
    }
}