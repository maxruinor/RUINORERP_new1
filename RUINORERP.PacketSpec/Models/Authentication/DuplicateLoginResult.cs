using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Authentication
{
    /// <summary>
    /// 重复登录分析结果
    /// </summary>
    [Serializable]
    public class DuplicateLoginResult
    {
        /// <summary>
        /// 是否存在重复登录
        /// </summary>
        public bool HasDuplicateLogin { get; set; }

        /// <summary>
        /// 现有会话信息列表
        /// </summary>
        public List<ExistingSessionInfo> ExistingSessions { get; set; } = new List<ExistingSessionInfo>();

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否需要用户确认
        /// </summary>
        public bool RequireUserConfirmation { get; set; } = true;

        /// <summary>
        /// 是否允许多个本地会话同时存在
        /// </summary>
        public bool AllowMultipleLocalSessions { get; set; } = false;

        /// <summary>
        /// 重复登录类型
        /// </summary>
        public DuplicateLoginType Type => DetermineDuplicateLoginType();

        /// <summary>
        /// 确定重复登录类型
        /// </summary>
        private DuplicateLoginType DetermineDuplicateLoginType()
        {
            if (!HasDuplicateLogin)
                return DuplicateLoginType.None;

            var localSessions = ExistingSessions.FindAll(s => s.IsLocal);
            var remoteSessions = ExistingSessions.FindAll(s => !s.IsLocal);

            if (localSessions.Any() && !remoteSessions.Any())
                return DuplicateLoginType.LocalOnly;
            if (!localSessions.Any() && remoteSessions.Any())
                return DuplicateLoginType.RemoteOnly;
            if (localSessions.Any() && remoteSessions.Any())
                return DuplicateLoginType.Both;

            return DuplicateLoginType.None;
        }
    }

    /// <summary>
    /// 重复登录类型枚举
    /// </summary>
    public enum DuplicateLoginType
    {
        /// <summary>
        /// 无重复登录
        /// </summary>
        None,
        /// <summary>
        /// 仅本地重复登录
        /// </summary>
        LocalOnly,
        /// <summary>
        /// 仅远程重复登录
        /// </summary>
        RemoteOnly,
        /// <summary>
        /// 本地和远程都有重复登录
        /// </summary>
        Both
    }
}
