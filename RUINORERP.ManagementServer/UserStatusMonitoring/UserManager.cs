using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.TopServer.ServerManagement;

namespace RUINORERP.TopServer.UserStatusMonitoring
{
    /// <summary>
    /// 用户管理核心类
    /// 负责收集并展示各客户服务器的登录用户信息与使用情况
    /// </summary>
    public class UserManager
    {
        // 用户信息字典，键为服务器实例ID，值为该实例下的用户信息字典
        private ConcurrentDictionary<Guid, ConcurrentDictionary<string, UserInfo>> _userInfos;
        // 用户会话信息字典，键为会话ID，值为会话信息
        private ConcurrentDictionary<string, UserSessionInfo> _sessionInfos;
        // 用户活动记录字典，键为服务器实例ID，值为该实例下的用户活动记录
        private ConcurrentDictionary<Guid, ConcurrentQueue<UserActivityInfo>> _activityRecords;
        // 活动记录最大数量
        private const int MAX_ACTIVITY_RECORDS = 1000;

        /// <summary>
        /// 用户登录事件
        /// </summary>
        public event EventHandler<UserLoggedInEventArgs> UserLoggedIn;

        /// <summary>
        /// 用户登出事件
        /// </summary>
        public event EventHandler<UserLoggedOutEventArgs> UserLoggedOut;

        /// <summary>
        /// 用户活动事件
        /// </summary>
        public event EventHandler<UserActivityEventArgs> UserActivity;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserManager()
        {
            _userInfos = new ConcurrentDictionary<Guid, ConcurrentDictionary<string, UserInfo>>();
            _sessionInfos = new ConcurrentDictionary<string, UserSessionInfo>();
            _activityRecords = new ConcurrentDictionary<Guid, ConcurrentQueue<UserActivityInfo>>();
        }

        /// <summary>
        /// 获取指定服务器实例的所有用户
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <returns>用户信息列表</returns>
        public IEnumerable<UserInfo> GetUsersByInstanceId(Guid instanceId)
        {
            if (_userInfos.TryGetValue(instanceId, out ConcurrentDictionary<string, UserInfo> users))
            {
                return users.Values;
            }
            return Enumerable.Empty<UserInfo>();
        }

        /// <summary>
        /// 获取所有服务器实例的用户总数
        /// </summary>
        public int TotalUserCount => _userInfos.Values.Sum(users => users.Count);

        /// <summary>
        /// 获取指定服务器实例的用户数量
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <returns>用户数量</returns>
        public int GetUserCountByInstanceId(Guid instanceId)
        {
            if (_userInfos.TryGetValue(instanceId, out ConcurrentDictionary<string, UserInfo> users))
            {
                return users.Count;
            }
            return 0;
        }

        /// <summary>
        /// 添加或更新用户信息
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <param name="userInfo">用户信息</param>
        public void AddOrUpdateUser(Guid instanceId, UserInfo userInfo)
        {
            if (userInfo == null || string.IsNullOrEmpty(userInfo.UserId))
            {
                return;
            }

            // 获取或创建服务器实例的用户字典
            var users = _userInfos.GetOrAdd(instanceId, new ConcurrentDictionary<string, UserInfo>());

            // 添加或更新用户信息
            var isNewUser = !users.ContainsKey(userInfo.UserId);
            users.AddOrUpdate(userInfo.UserId, userInfo, (key, oldValue) =>
            {
                // 更新现有用户信息
                oldValue.UserName = userInfo.UserName;
                oldValue.Status = userInfo.Status;
                oldValue.LastActivityTime = DateTime.Now;
                oldValue.IpAddress = userInfo.IpAddress;
                return oldValue;
            });

            // 如果是新用户，触发登录事件
            if (isNewUser)
            {
                UserLoggedIn?.Invoke(this, new UserLoggedInEventArgs(instanceId, userInfo));
            }
        }

        /// <summary>
        /// 移除用户信息
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <param name="userId">用户ID</param>
        public void RemoveUser(Guid instanceId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            // 获取服务器实例的用户字典
            if (_userInfos.TryGetValue(instanceId, out ConcurrentDictionary<string, UserInfo> users))
            {
                // 移除用户信息
                if (users.TryRemove(userId, out UserInfo userInfo))
                {
                    // 触发登出事件
                    UserLoggedOut?.Invoke(this, new UserLoggedOutEventArgs(instanceId, userInfo));
                }
            }
        }

        /// <summary>
        /// 添加用户会话信息
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        public void AddSession(UserSessionInfo sessionInfo)
        {
            if (sessionInfo == null || string.IsNullOrEmpty(sessionInfo.SessionId))
            {
                return;
            }

            // 添加会话信息
            _sessionInfos[sessionInfo.SessionId] = sessionInfo;
        }

        /// <summary>
        /// 移除用户会话信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        public void RemoveSession(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return;
            }

            // 移除会话信息
            _sessionInfos.TryRemove(sessionId, out _);
        }

        /// <summary>
        /// 更新用户活动信息
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <param name="activityInfo">活动信息</param>
        public void UpdateUserActivity(Guid instanceId, UserActivityInfo activityInfo)
        {
            if (activityInfo == null || string.IsNullOrEmpty(activityInfo.UserId))
            {
                return;
            }

            // 获取或创建服务器实例的活动记录队列
            var activityQueue = _activityRecords.GetOrAdd(instanceId, new ConcurrentQueue<UserActivityInfo>());

            // 添加活动记录
            activityQueue.Enqueue(activityInfo);

            // 限制活动记录数量
            while (activityQueue.Count > MAX_ACTIVITY_RECORDS)
            {
                activityQueue.TryDequeue(out _);
            }

            // 更新用户的最后活动时间
            if (_userInfos.TryGetValue(instanceId, out ConcurrentDictionary<string, UserInfo> users))
            {
                if (users.TryGetValue(activityInfo.UserId, out UserInfo userInfo))
                {
                    userInfo.LastActivityTime = DateTime.Now;
                }
            }

            // 触发用户活动事件
            UserActivity?.Invoke(this, new UserActivityEventArgs(instanceId, activityInfo));
        }

        /// <summary>
        /// 获取指定服务器实例的用户活动记录
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <param name="limit">返回记录数量限制</param>
        /// <returns>用户活动记录列表</returns>
        public IEnumerable<UserActivityInfo> GetUserActivityRecords(Guid instanceId, int limit = 100)
        {
            if (_activityRecords.TryGetValue(instanceId, out ConcurrentQueue<UserActivityInfo> activityQueue))
            {
                return activityQueue.TakeLast(limit);
            }
            return Enumerable.Empty<UserActivityInfo>();
        }

        /// <summary>
        /// 清除指定服务器实例的所有用户信息
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        public void ClearUsersByInstanceId(Guid instanceId)
        {
            // 移除服务器实例的用户字典
            _userInfos.TryRemove(instanceId, out _);
        }
    }

    /// <summary>
    /// 用户信息类
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatus Status { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActivityTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserInfo()
        {
            LoginTime = DateTime.Now;
            LastActivityTime = DateTime.Now;
            Status = UserStatus.Online;
        }
    }

    /// <summary>
    /// 用户会话信息类
    /// </summary>
    public class UserSessionInfo
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 服务器实例ID
        /// </summary>
        public Guid InstanceId { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActivityTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserSessionInfo()
        {
            LoginTime = DateTime.Now;
            LastActivityTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 用户活动信息类
    /// </summary>
    public class UserActivityInfo
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public UserActivityType ActivityType { get; set; }

        /// <summary>
        /// 活动描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 活动时间
        /// </summary>
        public DateTime ActivityTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserActivityInfo()
        {
            ActivityId = Guid.NewGuid();
            ActivityTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 用户状态枚举
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 在线
        /// </summary>
        Online = 0,
        /// <summary>
        /// 离线
        /// </summary>
        Offline = 1,
        /// <summary>
        /// 忙碌
        /// </summary>
        Busy = 2,
        /// <summary>
        /// 离开
        /// </summary>
        Away = 3
    }

    /// <summary>
    /// 用户活动类型枚举
    /// </summary>
    public enum UserActivityType
    {
        /// <summary>
        /// 登录
        /// </summary>
        Login = 0,
        /// <summary>
        /// 登出
        /// </summary>
        Logout = 1,
        /// <summary>
        /// 操作
        /// </summary>
        Operation = 2,
        /// <summary>
        /// 查询
        /// </summary>
        Query = 3,
        /// <summary>
        /// 报错
        /// </summary>
        Error = 4
    }

    /// <summary>
    /// 用户登录事件参数
    /// </summary>
    public class UserLoggedInEventArgs : EventArgs
    {
        /// <summary>
        /// 服务器实例ID
        /// </summary>
        public Guid InstanceId { get; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <param name="userInfo">用户信息</param>
        public UserLoggedInEventArgs(Guid instanceId, UserInfo userInfo)
        {
            InstanceId = instanceId;
            UserInfo = userInfo;
        }
    }

    /// <summary>
    /// 用户登出事件参数
    /// </summary>
    public class UserLoggedOutEventArgs : EventArgs
    {
        /// <summary>
        /// 服务器实例ID
        /// </summary>
        public Guid InstanceId { get; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <param name="userInfo">用户信息</param>
        public UserLoggedOutEventArgs(Guid instanceId, UserInfo userInfo)
        {
            InstanceId = instanceId;
            UserInfo = userInfo;
        }
    }

    /// <summary>
    /// 用户活动事件参数
    /// </summary>
    public class UserActivityEventArgs : EventArgs
    {
        /// <summary>
        /// 服务器实例ID
        /// </summary>
        public Guid InstanceId { get; }

        /// <summary>
        /// 用户活动信息
        /// </summary>
        public UserActivityInfo ActivityInfo { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <param name="activityInfo">用户活动信息</param>
        public UserActivityEventArgs(Guid instanceId, UserActivityInfo activityInfo)
        {
            InstanceId = instanceId;
            ActivityInfo = activityInfo;
        }
    }
}