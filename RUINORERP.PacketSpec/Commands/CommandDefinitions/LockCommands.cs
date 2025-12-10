using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 锁管理相关命令 - 优化版本（保持原有CommandId定义方式）
    /// 优化目标：移除冗余指令，保留核心业务场景所需的最小指令集
    /// </summary>
    public static class LockCommands
    {
        #region 基础锁定操作 (0x0800-0x0802)

        /// <summary>
        /// 锁定单据 - 申请锁定指定单据，防止其他用户同时编辑
        /// 命令码: 0x0800
        /// </summary>
        public static readonly CommandId Lock = new CommandId(CommandCategory.Lock, (byte)(CommandCatalog.Lock_Lock & 0xFF));

        /// <summary>
        /// 解锁单据 - 释放已锁定的单据，允许其他用户编辑
        /// 命令码: 0x0801  
        /// </summary>
        public static readonly CommandId Unlock = new CommandId(CommandCategory.Lock, (byte)(CommandCatalog.Lock_Unlock & 0xFF));

        /// <summary>
        /// 检查锁定状态 - 查询单据当前的锁定状态
        /// 命令码: 0x0802
        /// </summary>
        public static readonly CommandId CheckLockStatus = new CommandId(CommandCategory.Lock, (byte)(CommandCatalog.Lock_CheckLockStatus & 0xFF));

        #endregion

        #region 锁定冲突处理 (0x0803-0x0805)

        /// <summary>
        /// 请求解锁 - 当单据被其他用户锁定时，请求当前锁定用户释放锁定
        /// 命令码: 0x0803
        /// </summary>
        public static readonly CommandId RequestUnlock = new CommandId(CommandCategory.Lock, (byte)(CommandCatalog.Lock_RequestUnlock & 0xFF));

        /// <summary>
        /// 拒绝解锁 - 当前锁定用户拒绝其他用户的解锁请求
        /// 命令码: 0x0804
        /// </summary>
        public static readonly CommandId RefuseUnlock = new CommandId(CommandCategory.Lock, (byte)(CommandCatalog.Lock_RefuseUnlock & 0xFF));

        /// <summary>
        /// 同意解锁 - 当前锁定用户同意其他用户的解锁请求并释放锁定
        /// 命令码: 0x0805
        /// </summary>
        public static readonly CommandId AgreeUnlock = new CommandId(CommandCategory.Lock, (byte)(CommandCatalog.Lock_AgreeUnlock & 0xFF));



        #endregion

        #region 系统管理操作 (0x0806-0x0807)

        /// <summary>
        /// 强制解锁 - 管理员强制释放锁定（特殊情况使用）
        /// 命令码: 0x0806
        /// </summary>
        public static readonly CommandId ForceUnlock = new CommandId(CommandCategory.Lock, (byte)(CommandCatalog.Lock_ForceUnlock & 0xFF));

        /// <summary>
        /// 广播锁定状态 - 向相关用户广播锁定状态变化
        /// 命令码: 0x0807
        /// </summary>
        public static readonly CommandId BroadcastLockStatus = new CommandId(CommandCategory.Lock, (byte)(CommandCatalog.Lock_BroadcastLockStatus & 0xFF));

        #endregion


        /// <summary>
        /// 同意解锁 - 当前锁定用户同意其他用户的解锁请求并释放锁定
        /// 命令码: 0x0805
        /// </summary>
        public static readonly CommandId GetLockStatuList = new CommandId(CommandCategory.Lock, (byte)(CommandCatalog.Lock_GetLockStatuList & 0xFF));

    }
}
