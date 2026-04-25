// **********************************************
// 文件名: PermissionConfig.cs
// 创建时间: 2026-04-25
// 作者: RUINORERP
// 功能描述: 权限系统配置常量统一管理
// **********************************************

namespace RUINORERP.UI.SysConfig
{
    /// <summary>
    /// 【P2优化】权限系统配置常量统一管理，消除魔法数字
    /// </summary>
    public static class PermissionConfig
    {
        #region 缓存配置

        /// <summary>
        /// 角色权限缓存过期时间（分钟）
        /// </summary>
        public const int RolePermissionCacheExpirationMinutes = 60;

        /// <summary>
        /// 窗体实例缓存过期时间（分钟）
        /// </summary>
        public const int FormInstanceCacheExpirationMinutes = 30;

        /// <summary>
        /// 窗体实例缓存最大数量，防止内存泄漏
        /// </summary>
        public const int MaxFormInstanceCacheSize = 100;

        /// <summary>
        /// 缓存清理定时器间隔（分钟）
        /// </summary>
        public const int CacheCleanupIntervalMinutes = 60;

        #endregion

        #region 批量操作配置

        /// <summary>
        /// 批量操作超时时间（秒）
        /// </summary>
        public const int BatchOperationTimeoutSeconds = 30;

        /// <summary>
        /// 数据库命令超时时间（秒）
        /// </summary>
        public const int DatabaseCommandTimeoutSeconds = 30;

        #endregion

        #region 性能优化配置

        /// <summary>
        /// 并行加载最大并发数
        /// </summary>
        public const int MaxParallelLoadingDegree = 4;

        /// <summary>
        /// 日志记录级别阈值（Debug/Information/Warning/Error）
        /// </summary>
        public const string DefaultLogLevel = "Information";

        #endregion

        #region 防重机制配置

        /// <summary>
        /// 会话级防重缓存最大容量
        /// </summary>
        public const int MaxProcessedPermissionsCapacity = 10000;

        #endregion
    }
}
