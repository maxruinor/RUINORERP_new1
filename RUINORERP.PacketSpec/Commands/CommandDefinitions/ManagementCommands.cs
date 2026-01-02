using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 顶级服务器管理相关命令
    /// 用于RUINORERP.ManagementServer与其他下级服务器之间的通信
    /// 使用独立的Management命令类别(0x12xx)，与系统命令完全分离
    /// </summary>
    public static class ManagementCommands
    {
        #region 服务器管理命令 (使用独立的Management命令类别)
        
        /// <summary>
        /// 服务器注册 - 下级服务器向顶级服务器注册
        /// </summary>
        public static readonly CommandId RegisterServer = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_RegisterServer & 0xFF));

        /// <summary>
        /// 心跳包 - 保持连接活跃
        /// </summary>
        public static readonly CommandId Heartbeat = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_Heartbeat & 0xFF));

        /// <summary>
        /// 状态上报 - 下级服务器向上级服务器上报状态
        /// </summary>
        public static readonly CommandId ReportStatus = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_ReportStatus & 0xFF));

        /// <summary>
        /// 用户信息上报 - 下级服务器向上级服务器上报用户信息
        /// </summary>
        public static readonly CommandId ReportUsers = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_ReportUsers & 0xFF));

        /// <summary>
        /// 配置上报 - 下级服务器向上级服务器上报配置信息
        /// </summary>
        public static readonly CommandId ReportConfiguration = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_ReportConfiguration & 0xFF));

        /// <summary>
        /// 配置更新 - 上级服务器向下级服务器推送配置更新
        /// </summary>
        public static readonly CommandId UpdateConfiguration = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_UpdateConfiguration & 0xFF));

        /// <summary>
        /// 获取配置 - 下级服务器向上级服务器请求配置信息
        /// </summary>
        public static readonly CommandId GetConfiguration = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_GetConfiguration & 0xFF));

        /// <summary>
        /// 用户活动上报 - 下级服务器向上级服务器上报用户活动
        /// </summary>
        public static readonly CommandId ReportUserActivity = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_ReportUserActivity & 0xFF));

        /// <summary>
        /// 服务器授权查询 - 查询服务器授权状态
        /// </summary>
        public static readonly CommandId QueryAuthorization = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_QueryAuthorization & 0xFF));

        /// <summary>
        /// 服务器授权更新 - 更新服务器授权信息
        /// </summary>
        public static readonly CommandId UpdateAuthorization = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_UpdateAuthorization & 0xFF));

        /// <summary>
        /// 服务器状态广播 - 广播服务器集群状态
        /// </summary>
        public static readonly CommandId BroadcastServerStatus = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_BroadcastServerStatus & 0xFF));

        /// <summary>
        /// 服务器集群配置同步 - 同步服务器集群配置
        /// </summary>
        public static readonly CommandId SyncClusterConfig = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_SyncClusterConfig & 0xFF));

        /// <summary>
        /// 服务器实例列表查询 - 查询所有注册的服务器实例
        /// </summary>
        public static readonly CommandId QueryServerInstances = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_QueryServerInstances & 0xFF));

        /// <summary>
        /// 服务器实例统计 - 获取服务器实例统计信息
        /// </summary>
        public static readonly CommandId ServerStatistics = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_ServerStatistics & 0xFF));

        /// <summary>
        /// 服务器重启指令 - 指令下级服务器重启
        /// </summary>
        public static readonly CommandId RestartServer = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_RestartServer & 0xFF));

        /// <summary>
        /// 服务器停止指令 - 指令下级服务器停止
        /// </summary>
        public static readonly CommandId StopServer = new CommandId(CommandCategory.Management, (byte)(CommandCatalog.Management_StopServer & 0xFF));

        #endregion

        /// <summary>
        /// 服务器管理命令类型枚举
        /// 用于标识不同的服务器管理操作
        /// </summary>
        public enum ManagementCommandType
        {
            /// <summary>
            /// 服务器注册
            /// </summary>
            ServerRegistration = 1,

            /// <summary>
            /// 心跳检测
            /// </summary>
            HeartbeatCheck = 2,

            /// <summary>
            /// 状态监控
            /// </summary>
            StatusMonitoring = 3,

            /// <summary>
            /// 用户管理
            /// </summary>
            UserManagement = 4,

            /// <summary>
            /// 配置管理
            /// </summary>
            ConfigurationManagement = 5,

            /// <summary>
            /// 授权管理
            /// </summary>
            AuthorizationManagement = 6,

            /// <summary>
            /// 集群管理
            /// </summary>
            ClusterManagement = 7
        }

        /// <summary>
        /// 服务器状态枚举
        /// </summary>
        public enum ServerStatus
        {
            /// <summary>
            /// 未知状态
            /// </summary>
            [Description("未知")]
            Unknown = 0,

            /// <summary>
            /// 正常运行
            /// </summary>
            [Description("正常运行")]
            Running = 1,

            /// <summary>
            /// 停止运行
            /// </summary>
            [Description("停止运行")]
            Stopped = 2,

            /// <summary>
            /// 维护中
            /// </summary>
            [Description("维护中")]
            Maintenance = 3,

            /// <summary>
            /// 异常状态
            /// </summary>
            [Description("异常状态")]
            Error = 4,

            /// <summary>
            /// 启动中
            /// </summary>
            [Description("启动中")]
            Starting = 5,

            /// <summary>
            /// 停止中
            /// </summary>
            [Description("停止中")]
            Stopping = 6
        }

        /// <summary>
        /// 授权类型枚举
        /// </summary>
        public enum AuthorizationType
        {
            /// <summary>
            /// 试用版
            /// </summary>
            [Description("试用版")]
            Trial = 0,

            /// <summary>
            /// 标准版
            /// </summary>
            [Description("标准版")]
            Standard = 1,

            /// <summary>
            /// 专业版
            /// </summary>
            [Description("专业版")]
            Professional = 2,

            /// <summary>
            /// 企业版
            /// </summary>
            [Description("企业版")]
            Enterprise = 3
        }
    }
}