using System;
using System.Text;

namespace RUINORERP.Server.Comm
{
    /// <summary>
    /// 缓存键生成器
    /// 统一缓存键命名规范，便于缓存管理和排查
    /// </summary>
    public static class CacheKeyGenerator
    {
        /// <summary>
        /// 缓存键前缀
        /// </summary>
        private const string Prefix = "ruino:erp";

        /// <summary>
        /// 用户打印机配置缓存键
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>缓存键</returns>
        public static string UserPrinterConfig(long userId) 
            => $"{Prefix}:user:printer:{userId}";

        /// <summary>
        /// 系统打印配置缓存键
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <param name="bizName">业务名称</param>
        /// <returns>缓存键</returns>
        public static string PrintConfig(int bizType, string bizName) 
            => $"{Prefix}:print:config:{bizType}:{bizName}";

        /// <summary>
        /// 打印模板缓存键
        /// </summary>
        /// <param name="configId">配置ID</param>
        /// <returns>缓存键</returns>
        public static string PrintTemplate(long configId) 
            => $"{Prefix}:print:template:{configId}";

        /// <summary>
        /// 菜单个性化缓存键
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>缓存键</returns>
        public static string MenuPersonalization(long menuId, long userId) 
            => $"{Prefix}:menu:personal:{menuId}:{userId}";

        /// <summary>
        /// 通用缓存锁键
        /// </summary>
        /// <param name="key">原缓存键</param>
        /// <returns>锁缓存键</returns>
        public static string Lock(string key) 
            => $"{Prefix}:lock:{key}";

        /// <summary>
        /// 用户会话缓存键
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>缓存键</returns>
        public static string UserSession(string sessionId)
            => $"{Prefix}:session:{sessionId}";

        /// <summary>
        /// 缓存元数据键
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>缓存键</returns>
        public static string CacheMetadata(string tableName)
            => $"{Prefix}:metadata:{tableName}";
    }
}
