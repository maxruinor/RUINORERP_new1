using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.ManagementServer.Network;

namespace RUINORERP.ManagementServer.ServerManagement
{
    /// <summary>
    /// 服务器管理核心类
    /// 负责管理客户端部署的各服务器实例
    /// </summary>
    public class ServerManager
    {
        // 服务器实例字典，键为实例ID
        private ConcurrentDictionary<Guid, ServerInstanceInfo> _serverInstances;
        // 会话与服务器实例映射，键为会话ID
        private ConcurrentDictionary<string, Guid> _sessionToInstanceMap;
        // 心跳检查定时器
        private System.Timers.Timer _heartbeatCheckTimer;
        // 心跳超时时间（毫秒）
        private const int HEARTBEAT_TIMEOUT = 30000;

        /// <summary>
        /// 服务器实例注册事件
        /// </summary>
        public event EventHandler<ServerInstanceRegisteredEventArgs> ServerInstanceRegistered;

        /// <summary>
        /// 服务器实例注销事件
        /// </summary>
        public event EventHandler<ServerInstanceUnregisteredEventArgs> ServerInstanceUnregistered;

        /// <summary>
        /// 服务器实例状态变化事件
        /// </summary>
        public event EventHandler<ServerInstanceStatusChangedEventArgs> ServerInstanceStatusChanged;

        /// <summary>
        /// 获取所有服务器实例
        /// </summary>
        public IEnumerable<ServerInstanceInfo> AllServerInstances => _serverInstances.Values;

        /// <summary>
        /// 获取在线服务器实例
        /// </summary>
        public IEnumerable<ServerInstanceInfo> OnlineServerInstances => _serverInstances.Values.Where(s => s.Status == ServerInstanceStatus.Online);

        /// <summary>
        /// 获取离线服务器实例
        /// </summary>
        public IEnumerable<ServerInstanceInfo> OfflineServerInstances => _serverInstances.Values.Where(s => s.Status == ServerInstanceStatus.Offline);

        /// <summary>
        /// 获取异常服务器实例
        /// </summary>
        public IEnumerable<ServerInstanceInfo> ExceptionServerInstances => _serverInstances.Values.Where(s => s.Status == ServerInstanceStatus.Exception);

        /// <summary>
        /// 构造函数
        /// </summary>
        public ServerManager()
        {
            _serverInstances = new ConcurrentDictionary<Guid, ServerInstanceInfo>();
            _sessionToInstanceMap = new ConcurrentDictionary<string, Guid>();

            // 初始化心跳检查定时器
            _heartbeatCheckTimer = new System.Timers.Timer(HEARTBEAT_TIMEOUT / 2);
            _heartbeatCheckTimer.Elapsed += OnHeartbeatCheckTimerElapsed;
            _heartbeatCheckTimer.AutoReset = true;
            _heartbeatCheckTimer.Start();
        }

        /// <summary>
        /// 注册服务器实例
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <param name="instanceInfo">服务器实例信息</param>
        /// <returns>注册结果</returns>
        public bool RegisterServerInstance(ServerSession session, ServerInstanceInfo instanceInfo)
        {
            if (instanceInfo == null || string.IsNullOrEmpty(session.SessionID))
            {
                return false;
            }

            // 生成唯一实例ID
            if (instanceInfo.InstanceId == Guid.Empty)
            {
                instanceInfo.InstanceId = Guid.NewGuid();
            }

            // 更新实例信息
            instanceInfo.RegisterTime = DateTime.Now;
            instanceInfo.LastHeartbeatTime = DateTime.Now;
            instanceInfo.Status = ServerInstanceStatus.Online;

            // 添加到实例字典
            _serverInstances.AddOrUpdate(instanceInfo.InstanceId, instanceInfo, (key, oldValue) =>
            {
                // 更新现有实例信息
                oldValue.InstanceName = instanceInfo.InstanceName;
                oldValue.IpAddress = instanceInfo.IpAddress;
                oldValue.Port = instanceInfo.Port;
                oldValue.Version = instanceInfo.Version;
                oldValue.LastHeartbeatTime = DateTime.Now;
                oldValue.Status = ServerInstanceStatus.Online;
                return oldValue;
            });

            // 建立会话与实例映射
            _sessionToInstanceMap[session.SessionID] = instanceInfo.InstanceId;

            // 关联会话与实例
            session.ServerInstance = instanceInfo;

            // 触发注册事件
            ServerInstanceRegistered?.Invoke(this, new ServerInstanceRegisteredEventArgs(instanceInfo));

            return true;
        }

        /// <summary>
        /// 注销服务器实例
        /// </summary>
        /// <param name="session">客户端会话</param>
        public void UnregisterServerInstance(ServerSession session)
        {
            if (string.IsNullOrEmpty(session.SessionID))
            {
                return;
            }

            // 获取实例ID
            if (_sessionToInstanceMap.TryRemove(session.SessionID, out Guid instanceId))
            {
                // 获取实例信息
                if (_serverInstances.TryRemove(instanceId, out ServerInstanceInfo instanceInfo))
                {
                    // 更新实例状态
                    instanceInfo.Status = ServerInstanceStatus.Offline;
                    instanceInfo.LastHeartbeatTime = DateTime.Now;

                    // 触发注销事件
                    ServerInstanceUnregistered?.Invoke(this, new ServerInstanceUnregisteredEventArgs(instanceInfo));
                }
            }
        }

        /// <summary>
        /// 更新服务器实例心跳
        /// </summary>
        /// <param name="instanceId">实例ID</param>
        public void UpdateHeartbeat(Guid instanceId)
        {
            if (_serverInstances.TryGetValue(instanceId, out ServerInstanceInfo instanceInfo))
            {
                // 保存旧状态
                var oldStatus = instanceInfo.Status;

                // 更新心跳时间和状态
                instanceInfo.LastHeartbeatTime = DateTime.Now;
                instanceInfo.Status = ServerInstanceStatus.Online;

                // 如果状态发生变化，触发状态变化事件
                if (oldStatus != instanceInfo.Status)
                {
                    ServerInstanceStatusChanged?.Invoke(this, new ServerInstanceStatusChangedEventArgs(instanceInfo, oldStatus));
                }
            }
        }

        /// <summary>
        /// 更新服务器实例心跳
        /// </summary>
        /// <param name="session">客户端会话</param>
        public void UpdateHeartbeat(ServerSession session)
        {
            if (string.IsNullOrEmpty(session.SessionID))
            {
                return;
            }

            // 获取实例ID
            if (_sessionToInstanceMap.TryGetValue(session.SessionID, out Guid instanceId))
            {
                UpdateHeartbeat(instanceId);
            }
        }

        /// <summary>
        /// 获取服务器实例信息
        /// </summary>
        /// <param name="instanceId">实例ID</param>
        /// <returns>服务器实例信息</returns>
        public ServerInstanceInfo GetServerInstance(Guid instanceId)
        {
            _serverInstances.TryGetValue(instanceId, out ServerInstanceInfo instanceInfo);
            return instanceInfo;
        }

        /// <summary>
        /// 获取服务器实例信息
        /// </summary>
        /// <param name="session">客户端会话</param>
        /// <returns>服务器实例信息</returns>
        public ServerInstanceInfo GetServerInstance(ServerSession session)
        {
            if (string.IsNullOrEmpty(session.SessionID))
            {
                return null;
            }

            // 获取实例ID
            if (_sessionToInstanceMap.TryGetValue(session.SessionID, out Guid instanceId))
            {
                return GetServerInstance(instanceId);
            }

            return null;
        }

        /// <summary>
        /// 更新服务器实例状态
        /// </summary>
        /// <param name="instanceId">实例ID</param>
        /// <param name="status">新状态</param>
        public void UpdateServerInstanceStatus(Guid instanceId, ServerInstanceStatus status)
        {
            if (_serverInstances.TryGetValue(instanceId, out ServerInstanceInfo instanceInfo))
            {
                // 保存旧状态
                var oldStatus = instanceInfo.Status;

                // 更新状态
                instanceInfo.Status = status;

                // 如果状态发生变化，触发状态变化事件
                if (oldStatus != status)
                {
                    ServerInstanceStatusChanged?.Invoke(this, new ServerInstanceStatusChangedEventArgs(instanceInfo, oldStatus));
                }
            }
        }

        /// <summary>
        /// 心跳检查定时器事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnHeartbeatCheckTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckHeartbeatTimeout();
        }

        /// <summary>
        /// 检查心跳超时
        /// </summary>
        private void CheckHeartbeatTimeout()
        {
            var now = DateTime.Now;
            foreach (var instance in _serverInstances.Values.ToList())
            {
                // 检查心跳是否超时
                if ((now - instance.LastHeartbeatTime).TotalMilliseconds > HEARTBEAT_TIMEOUT)
                {
                    // 保存旧状态
                    var oldStatus = instance.Status;

                    // 更新状态为离线
                    instance.Status = ServerInstanceStatus.Offline;

                    // 如果状态发生变化，触发状态变化事件
                    if (oldStatus != instance.Status)
                    {
                        ServerInstanceStatusChanged?.Invoke(this, new ServerInstanceStatusChangedEventArgs(instance, oldStatus));
                    }
                }
            }
        }

        /// <summary>
        /// 分组获取服务器实例
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <returns>该分组下的服务器实例</returns>
        public IEnumerable<ServerInstanceInfo> GetServerInstancesByGroup(string groupName)
        {
            // 目前简单返回所有实例，后续可以扩展分组功能
            return _serverInstances.Values;
        }
    }

    /// <summary>
    /// 服务器实例注册事件参数
    /// </summary>
    public class ServerInstanceRegisteredEventArgs : EventArgs
    {
        /// <summary>
        /// 注册的服务器实例
        /// </summary>
        public ServerInstanceInfo ServerInstance { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverInstance">注册的服务器实例</param>
        public ServerInstanceRegisteredEventArgs(ServerInstanceInfo serverInstance)
        {
            ServerInstance = serverInstance;
        }
    }

    /// <summary>
    /// 服务器实例注销事件参数
    /// </summary>
    public class ServerInstanceUnregisteredEventArgs : EventArgs
    {
        /// <summary>
        /// 注销的服务器实例
        /// </summary>
        public ServerInstanceInfo ServerInstance { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverInstance">注销的服务器实例</param>
        public ServerInstanceUnregisteredEventArgs(ServerInstanceInfo serverInstance)
        {
            ServerInstance = serverInstance;
        }
    }

    /// <summary>
    /// 服务器实例状态变化事件参数
    /// </summary>
    public class ServerInstanceStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 服务器实例
        /// </summary>
        public ServerInstanceInfo ServerInstance { get; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public ServerInstanceStatus OldStatus { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serverInstance">服务器实例</param>
        /// <param name="oldStatus">旧状态</param>
        public ServerInstanceStatusChangedEventArgs(ServerInstanceInfo serverInstance, ServerInstanceStatus oldStatus)
        {
            ServerInstance = serverInstance;
            OldStatus = oldStatus;
        }
    }
}