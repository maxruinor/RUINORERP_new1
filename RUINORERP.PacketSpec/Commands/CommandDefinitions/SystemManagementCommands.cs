using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands.System
{
    /// <summary>
    /// 系统管理相关命令
    /// </summary>
    public static class SystemManagementCommands
    {
        #region 系统管理命令 (0x09xx)
        /// <summary>
        /// 推送版本更新 - 向客户端推送系统版本更新
        /// </summary>
        public static readonly CommandId PushVersionUpdate = new CommandId(CommandCategory.SystemManagement, 0x00);

        /// <summary>
        /// 切换服务器 - 切换到备用服务器
        /// </summary>
        public static readonly CommandId SwitchServer = new CommandId(CommandCategory.SystemManagement, 0x01);

        /// <summary>
        /// 关机 - 关闭服务器系统
        /// </summary>
        public static readonly CommandId Shutdown = new CommandId(CommandCategory.SystemManagement, 0x02);

        /// <summary>
        /// 删除列配置文件 - 删除系统列配置文件
        /// </summary>
        public static readonly CommandId DeleteColumnConfig = new CommandId(CommandCategory.SystemManagement, 0x03);
        #endregion
    }
}