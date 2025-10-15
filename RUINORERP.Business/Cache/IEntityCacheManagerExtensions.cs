using RUINORERP.Business.Cache;
using System;

namespace RUINORERP.Common.Extensions
{
    /// <summary>
    /// IEntityCacheManager接口的扩展方法类
    /// 提供获取表结构信息的功能扩展
    /// </summary>
    public static class IEntityCacheManagerExtensions
    {
        /// <summary>
        /// 根据表名获取表结构信息
        /// </summary>
        /// <param name="cacheManager">IEntityCacheManager实例</param>
        /// <param name="tableName">表名</param>
        /// <returns>表结构信息，如果不存在返回null</returns>
        public static TableSchemaInfo GetTableSchema(this IEntityCacheManager cacheManager, string tableName)
        {
            if (cacheManager == null)
            {
                throw new ArgumentNullException(nameof(cacheManager));
            }
            
            // 调用TableSchemaManager单例获取表结构信息
            return TableSchemaManager.Instance.GetSchemaInfo(tableName);
        }
    }
}