using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 锁管理相关命令
    /// </summary>
    public static class LockCommands
    {
        #region 锁管理命令 (0x08xx)
        /// <summary>
        /// 申请锁 - 申请获取资源锁
        /// </summary>
        public static readonly CommandId LockRequest = new CommandId(CommandCategory.Lock, 0x00);
        
        /// <summary>
        /// 释放锁 - 释放已获取的资源锁
        /// </summary>
        public static readonly CommandId LockRelease = new CommandId(CommandCategory.Lock, 0x01);
        
        /// <summary>
        /// 锁状态查询 - 查询资源锁状态
        /// </summary>
        public static readonly CommandId LockStatus = new CommandId(CommandCategory.Lock, 0x02);
        
        /// <summary>
        /// 强制解锁 - 强制释放资源锁
        /// </summary>
        public static readonly CommandId ForceUnlock = new CommandId(CommandCategory.Lock, 0x03);
        
        /// <summary>
        /// 检查锁状态 - 检查资源锁是否可用
        /// </summary>
        public static readonly CommandId CheckLock = new CommandId(CommandCategory.Lock, 0x04);
        
        /// <summary>
        /// 广播 - 广播锁状态信息
        /// </summary>
        public static readonly CommandId Broadcast = new CommandId(CommandCategory.Lock, 0x05);
        
        /// <summary>
        /// 请求解锁 - 请求释放锁资源
        /// </summary>
        public static readonly CommandId RequestUnlock = new CommandId(CommandCategory.Lock, 0x06);
        
        /// <summary>
        /// 拒绝解锁 - 拒绝解锁请求
        /// </summary>
        public static readonly CommandId RefuseUnlock = new CommandId(CommandCategory.Lock, 0x07);
        
        /// <summary>
        /// 请求锁定单据 - 请求锁定业务单据
        /// </summary>
        public static readonly CommandId RequestLock = new CommandId(CommandCategory.Lock, 0x08);
        
        /// <summary>
        /// 释放锁定的单据 - 释放已锁定的业务单据
        /// </summary>
        public static readonly CommandId ReleaseLock = new CommandId(CommandCategory.Lock, 0x09);
        
        /// <summary>
        /// 强制释放锁定 - 强制释放业务单据锁
        /// </summary>
        public static readonly CommandId ForceReleaseLock = new CommandId(CommandCategory.Lock, 0x0A);
        
        /// <summary>
        /// 锁定状态查询 - 查询业务单据锁定状态
        /// </summary>
        public static readonly CommandId QueryLockStatus = new CommandId(CommandCategory.Lock, 0x0B);
        
        /// <summary>
        /// 广播锁定状态变化 - 广播业务单据锁定状态变化
        /// </summary>
        public static readonly CommandId BroadcastLockStatus = new CommandId(CommandCategory.Lock, 0x0C);
        
        /// <summary>
        /// 锁管理 - 锁资源管理操作
        /// </summary>
        public static readonly CommandId LockManagement = new CommandId(CommandCategory.Lock, 0x0D);
        
        /// <summary>
        /// 转发单据锁定 - 转发单据锁定请求
        /// </summary>
        public static readonly CommandId ForwardDocumentLock = new CommandId(CommandCategory.Lock, 0x0E);
        #endregion
    }
}