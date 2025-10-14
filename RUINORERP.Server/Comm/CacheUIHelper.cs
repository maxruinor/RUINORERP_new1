using RUINORERP.Business.CommService;
using RUINORERP.Model;
using RUINORERP.Server.Network.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{
    /// <summary>
    /// 缓存UI帮助类，用于支持缓存管理界面的显示
    /// </summary>
    public static class CacheUIHelper
    {
        /// <summary>
        /// 获取指定表的缓存项数量
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="tableName">表名</param>
        /// <returns>缓存项数量</returns>
        public static int GetCacheItemCount(IEntityCacheManager cacheManager, string tableName)
        {
            try
            {
                // 直接通过ICacheManager接口获取实体列表，然后返回数量
                // 首先获取实体类型
                var tableSchemaManager = TableSchemaManager.Instance;
                var schemaInfo = tableSchemaManager.GetSchemaInfo(tableName);

                if (schemaInfo != null)
                {
                    // 使用反射调用GetEntityList<T>(string tableName)方法
                    // 修复：使用BindingFlags.Public | BindingFlags.Instance来确保能找到方法
                    var method = typeof(IEntityCacheManager).GetMethod("GetEntityList", 
                        BindingFlags.Public | BindingFlags.Instance, 
                        null, 
                        new[] { typeof(string) }, 
                        null);
                    
                    if (method != null)
                    {
                        var genericMethod = method.MakeGenericMethod(schemaInfo.EntityType);
                        var entityList = genericMethod.Invoke(cacheManager, new object[] { tableName });

                        // 获取列表数量
                        if (entityList is System.Collections.IList list)
                        {
                            return list.Count;
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                // 如果出现任何错误，返回0
                return 0;
            }
        }

        /// <summary>
        /// 获取所有可缓存的表名列表
        /// </summary>
        /// <returns>表名列表</returns>
        public static List<string> GetCacheableTableNames()
        {
            try
            {
                var tableSchemaManager = TableSchemaManager.Instance;
                return tableSchemaManager.GetCacheableTableNamesList();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}