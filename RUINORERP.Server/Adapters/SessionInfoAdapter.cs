using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RUINORERP.Model.CommonModel;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.Adapters
{
    /// <summary>
    /// SessionInfo与UserInfo之间的适配器
    /// 用于将新的SessionInfo模型转换为旧的UserInfo模型，以便与现有UI代码兼容
    /// </summary>
    public static class SessionInfoAdapter
    {
        /// <summary>
        /// 将SessionInfo转换为UserInfo
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>用户信息</returns>
        public static UserInfo ToUserInfo(SessionInfo sessionInfo)
        {
            if (sessionInfo == null)
                return null;

            var userInfo = new UserInfo
            {
                SessionId = sessionInfo.SessionID,
                用户名 = sessionInfo.UserName,
                客户端IP = sessionInfo.RemoteEndPoint is IPEndPoint ipEndPoint ? ipEndPoint.Address.ToString() : sessionInfo.RemoteEndPoint?.ToString() ?? "",
                登陆时间 = sessionInfo.ConnectedTime,
                心跳数 = sessionInfo.HeartbeatCount,
                最后心跳时间 = sessionInfo.LastHeartbeat.ToString("yyyy-MM-dd HH:mm:ss"),
                在线状态 = sessionInfo.IsConnected,
                授权状态 = sessionInfo.IsAuthenticated,
                UserID = sessionInfo.UserId.Value,
                超级用户 = sessionInfo.IsAdmin
            };

            // 如果SessionInfo中有UserInfo对象，则复制其属性
            if (sessionInfo.UserInfo != null)
            {
                userInfo.姓名 = sessionInfo.UserInfo.姓名;
                userInfo.当前模块 = sessionInfo.UserInfo.当前模块;
                userInfo.当前窗体 = sessionInfo.UserInfo.当前窗体;
                userInfo.客户端版本 = sessionInfo.UserInfo.客户端版本;
                userInfo.Employee_ID = sessionInfo.UserInfo.Employee_ID;
                
                // 如果SessionInfo中的用户名为空，则使用UserInfo中的用户名
                if (string.IsNullOrEmpty(userInfo.用户名) && !string.IsNullOrEmpty(sessionInfo.UserInfo.用户名))
                {
                    userInfo.用户名 = sessionInfo.UserInfo.用户名;
                }
            }

            return userInfo;
        }

        /// <summary>
        /// 将多个SessionInfo转换为多个UserInfo
        /// </summary>
        /// <param name="sessionInfos">会话信息列表</param>
        /// <returns>用户信息列表</returns>
        public static List<UserInfo> ToUserInfoList(IEnumerable<SessionInfo> sessionInfos)
        {
            if (sessionInfos == null)
                return new List<UserInfo>();

            return sessionInfos.Select(ToUserInfo).Where(u => u != null).ToList();
        }

        /// <summary>
        /// 从SessionInfo中提取会话ID
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>会话ID</returns>
        public static string GetSessionId(SessionInfo sessionInfo)
        {
            return sessionInfo?.SessionID;
        }

        /// <summary>
        /// 从UserInfo中提取会话ID
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>会话ID</returns>
        public static string GetSessionId(UserInfo userInfo)
        {
            return userInfo?.SessionId;
        }
    }
}