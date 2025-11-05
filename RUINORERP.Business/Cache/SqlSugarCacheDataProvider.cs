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
                // 生成查询缓存键，包含泛型类型信息
                string queryCacheKey = $"SqlSugarCache:GetEntity_{typeof(T).Name}_{tableName}_{idValue}";
                
                _logger?.LogDebug($"尝试从缓存获取实体：表 {tableName} ID为 {idValue}");
                
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
                    .Where($"{schemaInfo.PrimaryKeyField} = @id", new { id = idValue })
                    .WithCache(queryCacheKey, 60)
                    .First();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从数据库加载表 {tableName} ID为 {idValue} 的实体数据时发生错误");
                return null;
            }
        }

    
    }
}