using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 系统相关命令
    /// </summary>
    public static class SystemCommands
    {
        #region 系统命令 (0x00xx)
        /// <summary>
        /// 空命令/心跳包
        /// </summary>
        public static readonly CommandId None = new CommandId(CommandCategory.System, (byte)(CommandCatalog.System_None & 0xFF));

        /// <summary>
        /// 心跳包 - 保持连接活跃
        /// </summary>
        public static readonly CommandId Heartbeat = new CommandId(CommandCategory.System, (byte)(CommandCatalog.System_Heartbeat & 0xFF));

        /// <summary>
        /// 心跳回复
        /// </summary>
        public static readonly CommandId HeartbeatResponse = new CommandId(CommandCategory.System, (byte)(CommandCatalog.System_HeartbeatResponse & 0xFF));

        /// <summary>
        /// 电脑状态查询
        /// </summary>
        public static readonly CommandId ComputerStatus = new CommandId(CommandCategory.System, (byte)(CommandCatalog.System_ComputerStatus & 0xFF));

        /// <summary>
        /// 异常报告
        /// </summary>
        public static readonly CommandId ExceptionReport = new CommandId(CommandCategory.System, (byte)(CommandCatalog.System_ExceptionReport & 0xFF));

        /// <summary>
        /// 关闭电脑
        /// </summary>
        public static readonly CommandId ShutdownComputer = new CommandId(CommandCategory.System, (byte)(CommandCatalog.System_ShutdownComputer & 0xFF));

        /// <summary>
        /// 退出系统
        /// </summary>
        public static readonly CommandId ExitSystem = new CommandId(CommandCategory.System, (byte)(CommandCatalog.System_ExitSystem & 0xFF));
        
        /// <summary>
        /// 推送版本更新 - 服务器主动推送最新版本信息，要求客户端更新
        /// </summary>
        public static readonly CommandId PushVersionUpdate = new CommandId(CommandCategory.System, (byte)(CommandCatalog.System_PushVersionUpdate & 0xFF));
        #endregion
    }
}
