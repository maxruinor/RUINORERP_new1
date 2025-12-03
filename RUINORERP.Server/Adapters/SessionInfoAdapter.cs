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
                UserID = sessionInfo.UserId ?? 0, // 添加空值检查，避免NullReferenceException
                超级用户 = sessionInfo.IsAdmin,
                操作系统 = sessionInfo.ClientSystemInfo?.OperatingSystem?.Version ?? "",
                机器名 = sessionInfo.ClientIp ?? "未知", // HardwareInfo没有MachineName属性
                CPU信息 = sessionInfo.ClientSystemInfo?.Hardware?.ProcessorInfo?.Name ?? "",
                内存大小 = "" // ClientSystemInfo没有MemorySize属性
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

        /// <summary>
        /// 将UserInfo转换为SessionInfo
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>会话信息</returns>
        public static SessionInfo ToSessionInfo(UserInfo userInfo)
        {
            if (userInfo == null)
                return null;

            // 创建新的SessionInfo实例
            var sessionInfo = SessionInfo.Create();
            
            // 设置基本属性 - SessionID是只读属性，不设置
            sessionInfo.UserName = userInfo.用户名;
            sessionInfo.UserId = userInfo.UserID;
            sessionInfo.IsAdmin = userInfo.超级用户;
            sessionInfo.IsAuthenticated = userInfo.授权状态;
            sessionInfo.IsConnected = userInfo.在线状态;
            sessionInfo.ConnectedTime = userInfo.登陆时间;
            sessionInfo.HeartbeatCount = userInfo.心跳数;
            
            // 尝试解析最后心跳时间
            if (!string.IsNullOrEmpty(userInfo.最后心跳时间))
            {
                if (DateTime.TryParse(userInfo.最后心跳时间, out DateTime heartbeatTime))
                {
                    sessionInfo.LastHeartbeat = heartbeatTime;
                }
            }
            
            // 设置客户端IP和端口
            if (!string.IsNullOrEmpty(userInfo.客户端IP))
            {
                sessionInfo.ClientIp = userInfo.客户端IP;
                // 简单处理，不解析端口
            }
            
            // 创建并设置UserInfo属性
            sessionInfo.UserInfo = new UserInfo
            {
                SessionId = userInfo.SessionId,
                用户名 = userInfo.用户名,
                姓名 = userInfo.姓名,
                当前模块 = userInfo.当前模块,
                当前窗体 = userInfo.当前窗体,
                登陆时间 = userInfo.登陆时间,
                心跳数 = userInfo.心跳数,
                最后心跳时间 = userInfo.最后心跳时间,
                客户端版本 = userInfo.客户端版本,
                客户端IP = userInfo.客户端IP,
                静止时间 = userInfo.静止时间,
                Employee_ID = userInfo.Employee_ID,
                UserID = userInfo.UserID,
                超级用户 = userInfo.超级用户,
                在线状态 = userInfo.在线状态,
                授权状态 = userInfo.授权状态,
                操作系统 = userInfo.操作系统,
                机器名 = userInfo.机器名,
                CPU信息 = userInfo.CPU信息,
                内存大小 = userInfo.内存大小
            };
            
            // 设置客户端系统信息
            if (!string.IsNullOrEmpty(userInfo.操作系统) || !string.IsNullOrEmpty(userInfo.机器名))
            {
                // ClientSystemInfo的属性是只读的，需要通过其他方式初始化
                sessionInfo.ClientSystemInfo = new();
                // 注意：这里不能直接设置OSVersion、MachineName、CPUInfo和MemorySize属性，因为它们是只读的
            }
            
            return sessionInfo;
        }

        /// <summary>
        /// 将数据库实体tb_UserInfo更新到SessionInfo
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="userInfo">数据库用户实体</param>
        /// <param name="isAdmin">是否为管理员</param>
        public static void UpdateSessionInfoFromUserEntity(SessionInfo sessionInfo, tb_UserInfo userInfo, bool isAdmin = false)
        {
            if (sessionInfo == null || userInfo == null)
                return;

            sessionInfo.UserId = (long?)userInfo.User_ID; // 添加类型转换
            sessionInfo.UserName = userInfo.UserName;
            sessionInfo.IsAuthenticated = true;
            sessionInfo.IsAdmin = isAdmin;
            sessionInfo.UpdateActivity();

            // 如果需要，还可以设置嵌套的UserInfo对象
            if (sessionInfo.UserInfo == null)
            {
                sessionInfo.UserInfo = new UserInfo();
            }
            sessionInfo.UserInfo.UserID = userInfo.User_ID;
            sessionInfo.UserInfo.用户名 = userInfo.UserName;
            sessionInfo.UserInfo.Employee_ID = userInfo.Employee_ID.HasValue ? (long)userInfo.Employee_ID.Value : 0; // 添加显式类型转换
        }
    }
}