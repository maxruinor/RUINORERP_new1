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
        public static readonly CommandId None = new CommandId(CommandCategory.System, 0x00);
        
        /// <summary>
        /// 心跳包 - 保持连接活跃
        /// </summary>
        public static readonly CommandId Heartbeat = new CommandId(CommandCategory.System, 0x01);
        
        /// <summary>
        /// 心跳回复
        /// </summary>
        public static readonly CommandId HeartbeatResponse = new CommandId(CommandCategory.System, 0x02);
        
        /// <summary>
        /// 系统状态查询
        /// </summary>
        public static readonly CommandId SystemStatus = new CommandId(CommandCategory.System, 0x03);
        
        /// <summary>
        /// 异常报告
        /// </summary>
        public static readonly CommandId ExceptionReport = new CommandId(CommandCategory.System, 0x04);
        #endregion
    }
}