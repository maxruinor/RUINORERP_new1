using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// SqlSugar实现的缓存数据提供者，用于在缓存未命中时从数据库加载数据
    /// 使用SqlSugar自带的缓存机制
    /// </summary>
    public class SqlSugarCacheDataProvider : ICacheDataProvider
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly TableSchemaManager _tableSchemaManager;
        private readonly ILogger<SqlSugarCacheDataProvider> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManage">工作单元管理器，用于获取数据库连接</param>
        /// <param name="logger">日志记录器</param>
        public SqlSugarCacheDataProvider(
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<SqlSugarCacheDataProvider> logger)
        {
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
            _tableSchemaManager = TableSchemaManager.Instance;
            _logger = logger;
        }

        /// <summary>
        /// 从数据源获取实体列表
        /// 使用SqlSugar自带的缓存机制避免重复查询相同的实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <returns>实体列表</returns>
        public List<T> GetEntityListFromSource<T>(string tableName) where T : class
        {
            try
            {
                // 生成查询缓存键，包含泛型类型信息
                string queryCacheKey = $"SqlSugarCache:GetEntityList_{typeof(T).Name}_{tableName}";
                
                _logger?.LogDebug($"尝试从缓存获取实体列表：表 {tableName}");
                
                // 获取数据库客户端
                var db = _unitOfWorkManage.GetDbClient();
                // 使用SqlSugar自带的缓存机制，缓存30秒
                return db.Queryable<T>(tableName)
                    .WithCache(queryCacheKey, 30)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从数据库加载表 {tableName} 的实体列表数据时发生错误");
                return new List<T>();
            }
        }

        /// <summary>
        /// 从数据源根据ID获取实体
        /// 使用SqlSugar自带的缓存机制避免重复查询相同的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="idValue">主键值</param>
        /// <returns>实体对象</returns>
        public T GetEntityFromSource<T>(string tableName, object idValue) where T : class
        {
            try
            {
                // 首先验证主键值：必须大于0且可以转换为long类型
                long longIdValue;
                if (!ValidatePrimaryKeyValue(idValue, out longIdValue))
                {
                    return null;
                }

                // 生成查询缓存键，包含泛型类型信息
                string queryCacheKey = $"SqlSugarCache:GetEntity_{typeof(T).Name}_{tableName}_{longIdValue}";
                
                _logger?.LogDebug($"尝试从缓存获取实体：表 {tableName} ID为 {longIdValue}");
                
                // 获取表的主键字段信息
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                if (schemaInfo == null)
                {
                    _logger?.LogWarning($"未找到表 {tableName} 的结构信息");
                    return null;
                }

                // 获取数据库客户端
                var db = _unitOfWorkManage.GetDbClient();
                // 使用SqlSugar自带的缓存机制，缓存60秒
                return db.Queryable<T>(tableName)
                    .Where($"{schemaInfo.PrimaryKeyField} = @id", new { id = longIdValue })
                    .WithCache(queryCacheKey, 60)
                    .First();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从数据库加载表 {tableName} ID为 {idValue} 的实体数据时发生错误");
                return null;
            }
        }

        /// <summary>
        /// 验证主键值是否有效
        /// </summary>
        /// <param name="idValue">主键值</param>
        /// <param name="longIdValue">转换后的long类型主键值</param>
        /// <returns>主键值是否有效</returns>
        private bool ValidatePrimaryKeyValue(object idValue, out long longIdValue)
        {
            longIdValue = 0;
            
            // 检查是否为null
            if (idValue == null)
            {
                return false;
            }
            
            // 直接检查是否为long类型
            if (idValue is long longValue)
            {
                longIdValue = longValue;
                return longValue > 0;
            }
            
            // 检查是否为可以安全转换为long的数值类型
            if (idValue is int intValue)
            {
                longIdValue = intValue;
                return intValue > 0;
            }
            
            if (idValue is decimal decimalValue)
            {
                // 检查是否为整数
                if (decimal.Truncate(decimalValue) != decimalValue)
                {
                    return false;
                }
                
                // 检查是否在long范围内
                if (decimalValue > long.MaxValue || decimalValue < long.MinValue)
                {
                    return false;
                }
                
                longIdValue = (long)decimalValue;
                return longIdValue > 0;
            }
            
            // 尝试从字符串转换
            if (idValue is string stringValue)
            {
                if (long.TryParse(stringValue, out long parsedValue))
                {
                    longIdValue = parsedValue;
                    return parsedValue > 0;
                }
                return false;
            }
            
            // 对于其他类型，尝试转换为字符串再解析
            try
            {
                string stringRepresentation = Convert.ToString(idValue);
                if (long.TryParse(stringRepresentation, out long finalParsedValue))
                {
                    longIdValue = finalParsedValue;
                    return finalParsedValue > 0;
                }
            }
            catch
            {
                // 转换失败，视为无效主键值
            }
            
            return false;
        }

    
    }
}