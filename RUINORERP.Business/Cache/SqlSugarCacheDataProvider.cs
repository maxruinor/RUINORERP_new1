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
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <returns>实体列表</returns>
        public List<T> GetEntityListFromSource<T>(string tableName) where T : class
        {
            try
            {
                _logger?.LogDebug($"从数据库加载表 {tableName} 的实体列表数据");
                
                // 移除using块，让SqlSugar自己管理连接生命周期
                var db = _unitOfWorkManage.GetDbClient();
                // 直接使用SqlSugar查询表数据
                return db.Queryable<T>(tableName).ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从数据库加载表 {tableName} 的实体列表数据时发生错误");
                return new List<T>();
            }
        }

        /// <summary>
        /// 从数据源根据ID获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="idValue">主键值</param>
        /// <returns>实体对象</returns>
        public T GetEntityFromSource<T>(string tableName, object idValue) where T : class
        {
            try
            {
                _logger?.LogDebug($"从数据库加载表 {tableName} ID为 {idValue} 的实体数据");
                
                // 获取表的主键字段信息
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                if (schemaInfo == null)
                {
                    _logger?.LogWarning($"未找到表 {tableName} 的结构信息");
                    return null;
                }

                // 移除using块，让SqlSugar自己管理连接生命周期
                var db = _unitOfWorkManage.GetDbClient();
                // 根据主键字段查询实体
                return db.Queryable<T>(tableName)
                    .Where($"{schemaInfo.PrimaryKeyField} = @id", new { id = idValue })
                    .First();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从数据库加载表 {tableName} ID为 {idValue} 的实体数据时发生错误");
                return null;
            }
        }

        /// <summary>
        /// 从数据源获取显示值
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="idValue">主键值</param>
        /// <returns>显示值</returns>
        public string GetDisplayValueFromSource(string tableName, object idValue)
        {
            try
            {
                _logger?.LogDebug($"从数据库加载表 {tableName} ID为 {idValue} 的显示值");
                
                // 获取表的主键和显示字段信息
                var schemaInfo = _tableSchemaManager.GetSchemaInfo(tableName);
                if (schemaInfo == null)
                {
                    return null;
                }

                // 移除using块，让SqlSugar自己管理连接生命周期
                var db = _unitOfWorkManage.GetDbClient();
                // 只查询显示字段
                var result = db.Ado.GetDataTable($"SELECT {schemaInfo.DisplayField} FROM {tableName} WHERE {schemaInfo.PrimaryKeyField} = @id", 
                    new { id = idValue });
                
                if (result != null && result.Rows.Count > 0 && result.Rows[0][0] != DBNull.Value)
                {
                    return result.Rows[0][0].ToString();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从数据库加载表 {tableName} ID为 {idValue} 的显示值时发生错误");
                return null;
            }
        }
    }
}