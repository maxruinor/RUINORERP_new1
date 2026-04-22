using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.Adapters
{
    /// <summary>
    /// ✅ SessionInfo与UserInfo之间的适配器 - 重构版
    /// 
    /// 【适配新结构】
    /// - SessionInfo.UserId/UserName 等改为从 UserInfo 获取的只读属性
    /// - 通过设置 SessionInfo.UserInfo 来间接设置这些值
    /// - 提供双向转换的兼容方法
    /// </summary>
    public static class SessionInfoAdapter
    {
        #region SessionInfo -> CurrentUserInfo 转换

        /// <summary>
        /// 将SessionInfo转换为CurrentUserInfo
        /// </summary>
        public static CurrentUserInfo ToUserInfo(SessionInfo sessionInfo)
        {
            if (sessionInfo == null)
                return null;

            // 如果SessionInfo已有UserInfo，则以此为基础
            var baseUserInfo = sessionInfo.UserInfo ?? new CurrentUserInfo();

            var userInfo = new CurrentUserInfo
            {
                UserID = sessionInfo.UserId ?? baseUserInfo.UserID,
                UserName = sessionInfo.UserName ?? baseUserInfo.UserName,
                DisplayName = baseUserInfo.DisplayName,
                EmployeeId = baseUserInfo.EmployeeId,
                IsSuperUser = sessionInfo.IsAdmin,
                IsAuthorized = sessionInfo.IsAuthenticated,
                IsOnline = sessionInfo.IsConnected,
                LoginTime = sessionInfo.LoginTime ?? sessionInfo.ConnectedTime,
                HeartbeatCount = sessionInfo.HeartbeatCount,
                LastHeartbeatTime = sessionInfo.LastHeartbeat,
                CurrentModule = baseUserInfo.CurrentModule,
                CurrentForm = baseUserInfo.CurrentForm,
                ClientVersion = baseUserInfo.ClientVersion,
                ClientIp = GetClientIp(sessionInfo),
                IdleTime = baseUserInfo.IdleTime,
                OperatingSystem = sessionInfo.ClientSystemInfo?.OperatingSystem?.Version ?? baseUserInfo.OperatingSystem,
                MachineName = baseUserInfo.MachineName,
                CpuInfo = baseUserInfo.CpuInfo,
                MemorySize = baseUserInfo.MemorySize
            };

            return userInfo;
        }

        /// <summary>
        /// 将多个SessionInfo转换为多个CurrentUserInfo
        /// </summary>
        public static List<CurrentUserInfo> ToUserInfoList(IEnumerable<SessionInfo> sessionInfos)
        {
            if (sessionInfos == null)
                return new List<CurrentUserInfo>();

            return sessionInfos.Select(ToUserInfo).Where(u => u != null).ToList();
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        private static string GetClientIp(SessionInfo sessionInfo)
        {
            if (sessionInfo.RemoteEndPoint is IPEndPoint ipEndPoint)
                return ipEndPoint.Address.ToString();
            if (!string.IsNullOrEmpty(sessionInfo.ClientIp))
                return sessionInfo.ClientIp;
            return sessionInfo.RemoteEndPoint?.ToString() ?? "";
        }

        #endregion

        #region CurrentUserInfo -> SessionInfo 转换

        /// <summary>
        /// 将CurrentUserInfo转换为SessionInfo
        /// </summary>
        public static SessionInfo ToSessionInfo(CurrentUserInfo userInfo)
        {
            if (userInfo == null)
                return null;

            var sessionInfo = SessionInfo.Create();
            UpdateSessionInfoFromUserInfo(sessionInfo, userInfo);
            return sessionInfo;
        }

        /// <summary>
        /// 从CurrentUserInfo更新SessionInfo
        /// </summary>
        public static void UpdateSessionInfoFromUserInfo(SessionInfo sessionInfo, CurrentUserInfo userInfo)
        {
            if (sessionInfo == null || userInfo == null)
                return;

            // 确保UserInfo存在
            if (sessionInfo.UserInfo == null)
                sessionInfo.UserInfo = new CurrentUserInfo();

            // 更新UserInfo（这是唯一的数据源）
            sessionInfo.UserInfo.UserID = userInfo.UserID;
            sessionInfo.UserInfo.UserName = userInfo.UserName;
            sessionInfo.UserInfo.DisplayName = userInfo.DisplayName;
            sessionInfo.UserInfo.EmployeeId = userInfo.EmployeeId;
            sessionInfo.UserInfo.IsSuperUser = userInfo.IsSuperUser;
            sessionInfo.UserInfo.IsAuthorized = userInfo.IsAuthorized;
            sessionInfo.UserInfo.IsOnline = userInfo.IsOnline;
            sessionInfo.UserInfo.LoginTime = userInfo.LoginTime;
            sessionInfo.UserInfo.HeartbeatCount = userInfo.HeartbeatCount;
            sessionInfo.UserInfo.LastHeartbeatTime = userInfo.LastHeartbeatTime;
            sessionInfo.UserInfo.CurrentModule = userInfo.CurrentModule;
            sessionInfo.UserInfo.CurrentForm = userInfo.CurrentForm;
            sessionInfo.UserInfo.ClientVersion = userInfo.ClientVersion;
            sessionInfo.UserInfo.ClientIp = userInfo.ClientIp;
            sessionInfo.UserInfo.IdleTime = userInfo.IdleTime;
            sessionInfo.UserInfo.OperatingSystem = userInfo.OperatingSystem;
            sessionInfo.UserInfo.MachineName = userInfo.MachineName;
            sessionInfo.UserInfo.CpuInfo = userInfo.CpuInfo;
            sessionInfo.UserInfo.MemorySize = userInfo.MemorySize;

            // 通过便捷属性更新会话状态
            sessionInfo.IsAdmin = userInfo.IsSuperUser;
            sessionInfo.IsAuthenticated = userInfo.IsAuthorized;
            sessionInfo.IsConnected = userInfo.IsOnline;
            sessionInfo.HeartbeatCount = userInfo.HeartbeatCount;
            sessionInfo.LastHeartbeat = userInfo.LastHeartbeatTime;

            // 设置客户端IP
            if (!string.IsNullOrEmpty(userInfo.ClientIp))
                sessionInfo.ClientIp = userInfo.ClientIp;
        }

        #endregion

        #region 数据库实体 -> SessionInfo 转换

        /// <summary>
        /// 从数据库用户实体更新SessionInfo
        /// </summary>
        public static void UpdateSessionInfoFromUserEntity(SessionInfo sessionInfo, tb_UserInfo userEntity, bool isAdmin = false)
        {
            if (sessionInfo == null || userEntity == null)
                return;

            // 确保UserInfo存在
            if (sessionInfo.UserInfo == null)
                sessionInfo.UserInfo = new CurrentUserInfo();

            // 更新UserInfo（核心数据源）
            sessionInfo.UserInfo.UserID = userEntity.User_ID;
            sessionInfo.UserInfo.UserName = userEntity.UserName;
            sessionInfo.UserInfo.EmployeeId = userEntity.Employee_ID.HasValue ? (long)userEntity.Employee_ID.Value : 0;
            sessionInfo.UserInfo.IsAuthorized = true; // 登录成功后授权

            // 更新会话状态
            sessionInfo.IsAuthenticated = true;
            sessionInfo.IsAdmin = isAdmin;
            sessionInfo.LoginTime = DateTime.Now;
            sessionInfo.UpdateActivity();
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 从SessionInfo中提取会话ID
        /// </summary>
        public static string GetSessionId(SessionInfo sessionInfo)
        {
            return sessionInfo?.SessionID;
        }

 

        #endregion
    }
}
