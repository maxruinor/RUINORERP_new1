using System;

namespace RUINORERP.PacketSpec.Models.Authentication
{
    /// <summary>
    /// 注册状态枚举
    /// </summary>
    [Serializable]
    public enum RegistrationStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 即将到期
        /// </summary>
        ExpiringSoon = 1,

        /// <summary>
        /// 已过期
        /// </summary>
        Expired = 2
    }
}
