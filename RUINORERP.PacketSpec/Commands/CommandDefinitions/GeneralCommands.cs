using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 通用命令集合 - 包含各种特殊类别和较少使用的命令
    /// </summary>
    public static class GeneralCommands
    {
        #region 异常处理命令 (0x05xx)
        /// <summary>
        /// 异常处理 - 通用异常处理命令
        /// </summary>
        public static readonly CommandId ExceptionHandler = new CommandId(CommandCategory.Exception, (byte)(CommandCatalog.System_ExceptionReport & 0xFF));
        #endregion

        #region 复合型命令 (0x10xx)
        /// <summary>
        /// 复合命令执行 - 执行包含多个子命令的复合操作
        /// </summary>
        public static readonly CommandId CompositeCommandExecute = new CommandId(CommandCategory.Composite, (byte)(CommandCatalog.Composite_CompositeCommandExecute & 0xFF));

        /// <summary>
        /// 复合命令结果 - 复合命令执行结果反馈
        /// </summary>
        public static readonly CommandId CompositeCommandResult = new CommandId(CommandCategory.Composite, (byte)(CommandCatalog.Composite_CompositeCommandResult & 0xFF));
        #endregion

        #region 连接管理命令 (0x11xx)
        /// <summary>
        /// 连接状态通知 - 通知连接状态变化
        /// </summary>
        public static readonly CommandId ConnectionStatusNotification = new CommandId(CommandCategory.Connection, (byte)(CommandCatalog.Connection_ConnectionStatusNotification & 0xFF));

        /// <summary>
        /// 连接请求 - 请求建立新连接
        /// </summary>
        public static readonly CommandId ConnectionRequest = new CommandId(CommandCategory.Connection, (byte)(CommandCatalog.Connection_ConnectionRequest & 0xFF));

        /// <summary>
        /// 连接断开 - 请求断开连接
        /// </summary>
        public static readonly CommandId ConnectionDisconnect = new CommandId(CommandCategory.Connection, (byte)(CommandCatalog.Connection_ConnectionDisconnect & 0xFF));

        /// <summary>
        /// 连接池管理 - 管理连接池相关操作
        /// </summary>
        public static readonly CommandId ConnectionPoolManagement = new CommandId(CommandCategory.Connection, (byte)(CommandCatalog.Connection_ConnectionPoolManagement & 0xFF));
        #endregion

        #region 特殊功能命令 (0x90xx)
        /// <summary>
        /// 特殊操作 - 预留的特殊功能操作命令
        /// </summary>
        public static readonly CommandId SpecialOperation = new CommandId(CommandCategory.Special, (byte)(CommandCatalog.Special_SpecialOperation & 0xFF));

        /// <summary>
        /// 诊断模式 - 系统诊断模式命令
        /// </summary>
        public static readonly CommandId DiagnosticMode = new CommandId(CommandCategory.Special, (byte)(CommandCatalog.Special_DiagnosticMode & 0xFF));

        /// <summary>
        /// 调试信息 - 获取系统调试信息
        /// </summary>
        public static readonly CommandId DebugInfo = new CommandId(CommandCategory.Special, (byte)(CommandCatalog.Special_DebugInfo & 0xFF));
        #endregion

        #region 配置命令 (0x09xx)
        /// <summary>
        /// 配置更新 - 更新系统配置信息
        /// </summary>
        public static readonly CommandId ConfigUpdate = new CommandId(CommandCategory.Config, (byte)(CommandCatalog.Config_ConfigUpdate & 0xFF));

        /// <summary>
        /// 配置请求 - 请求系统配置信息
        /// </summary>
        public static readonly CommandId ConfigRequest = new CommandId(CommandCategory.Config, (byte)(CommandCatalog.Config_ConfigRequest & 0xFF));

        /// <summary>
        /// 配置同步 - 同步系统配置信息
        /// </summary>
        public static readonly CommandId ConfigSync = new CommandId(CommandCategory.Config, (byte)(CommandCatalog.Config_ConfigSync & 0xFF));
        #endregion
    }
}
