using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Lock
{
    /// <summary>
    /// 锁管理命令枚举
    /// </summary>
    public enum LockCommand : uint
    {
        /// <summary>
        /// 申请锁
        /// </summary>
        [Description("申请锁")]
        LockRequest = 0x0800,

        /// <summary>
        /// 释放锁
        /// </summary>
        [Description("释放锁")]
        LockRelease = 0x0801,

        /// <summary>
        /// 锁状态查询
        /// </summary>
        [Description("锁状态查询")]
        LockStatus = 0x0802,

        /// <summary>
        /// 强制解锁
        /// </summary>
        [Description("强制解锁")]
        ForceUnlock = 0x0803,

        /// <summary>
        /// 检查锁状态
        /// </summary>
        [Description("检查锁状态")]
        CheckLock = 0x0804,

        /// <summary>
        /// 广播
        /// </summary>
        [Description("广播")]
        Broadcast = 0x0805,

        /// <summary>
        /// 请求解锁
        /// </summary>
        [Description("请求解锁")]
        RequestUnlock = 0x0806,

        /// <summary>
        /// 拒绝解锁
        /// </summary>
        [Description("拒绝解锁")]
        RefuseUnlock = 0x0807,

        /// <summary>
        /// 请求锁定单据
        /// </summary>
        [Description("请求锁定单据")]
        RequestLock = 0x0808,

        /// <summary>
        /// 释放锁定的单据
        /// </summary>
        [Description("释放锁定的单据")]
        ReleaseLock = 0x0809,

        /// <summary>
        /// 强制释放锁定
        /// </summary>
        [Description("强制释放锁定")]
        ForceReleaseLock = 0x080A,

        /// <summary>
        /// 锁定状态查询
        /// </summary>
        [Description("锁定状态查询")]
        QueryLockStatus = 0x080B,

        /// <summary>
        /// 广播锁定状态变化
        /// </summary>
        [Description("广播锁定状态变化")]
        BroadcastLockStatus = 0x080C,

        /// <summary>
        /// 锁管理
        /// </summary>
        [Description("锁管理")]
        LockManagement = 0x080D,

        /// <summary>
        /// 转发单据锁定
        /// </summary>
        [Description("转发单据锁定")]
        ForwardDocumentLock = 0x080E
    }
}