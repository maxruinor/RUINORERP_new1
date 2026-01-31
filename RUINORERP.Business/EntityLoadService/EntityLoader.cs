using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Common.Extensions;
using RUINORERP.Extensions.ServiceExtensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.EntityLoadService
{
    /// <summary>
    /// 统一实体加载服务
    /// </summary>
    public class EntityLoader
    {
        private readonly IEntityMappingService _mappingService;
        private readonly ApplicationContext _context;
        private readonly ILogger<EntityLoader> _logger;

        public EntityLoader(
            IEntityMappingService mappingService,
            ApplicationContext context,
            ILogger<EntityLoader> logger)
        {
            _mappingService = mappingService;
            _context = context;
            _logger = logger;
        }

        public object LoadEntity(string tableName, object billNo)
        {
            var entityType = _mappingService.GetEntityTypeByTableName(tableName);
            if (entityType == null)
            {
                _logger.LogError($"找不到表名对应的实体类型: {tableName}");
                return null;
            }

            var config = _mappingService.GetEntityInfo(entityType);
            if (config == null)
            {
                _logger.LogError($"找不到实体类型的字段配置: {entityType.Name}");
                return null;
            }

            // 在try外面定义临时变量保存查询相关信息
            string debugInfo = $"开始加载实体: {entityType.Name}, 表名: {tableName}, BillNo: {billNo} (类型: {billNo?.GetType().Name ?? "null"})";
            string entityConfigInfo = $"实体配置信息 - IdField: {config.IdField}, NoField: {config.NoField}, DetailProperty: {config.DetailProperty}";
            string fieldName = "";
            string sqlStatement = "";
            
            try
            {
                // 创建查询对象
                var queryable = _context.Db.Queryable(entityType.Name, tableName);

                // 包含明细
                if (!string.IsNullOrEmpty(config.DetailProperty))
                {
                    queryable = queryable.IncludesByNameString(config.DetailProperty);
                }

                // 添加查询条件
                if (billNo is long id)
                {
                    fieldName = config.IdField;
                    queryable = queryable.Where($"{config.IdField} = @id", new { id });
                }
                else if (billNo is string no)
                {
                    fieldName = config.NoField;
                    queryable = queryable.Where($"{config.NoField} = @no", new { no });
                }

                // 获取查询的SQL语句以便调试
                var sqlResult = queryable.ToSql();
                // 检查返回类型并相应处理
                var resultType = sqlResult.GetType();
                if (resultType == typeof(KeyValuePair<string, List<SugarParameter>>))
                {
                    var sqlPair = (KeyValuePair<string, List<SugarParameter>>)sqlResult;
                    sqlStatement = sqlPair.Key; // 获取SQL语句部分
                }
                else
                {
                    sqlStatement = sqlResult.ToString() ?? "未知SQL格式";
                }

                var result = queryable.Single();
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"加载实体数据失败 - {debugInfo}");
                _logger.LogError($"{entityConfigInfo}");
                
                if (!string.IsNullOrEmpty(fieldName))
                    _logger.LogError($"查询字段: {fieldName}, 值: {billNo}");
                
                if (!string.IsNullOrEmpty(sqlStatement))
                    _logger.LogError($"执行的SQL语句: {sqlStatement}");

                _logger.LogError(ex, $"加载实体数据失败: {entityType.Name}, BillNo: {billNo}, 类型: {billNo?.GetType().Name ?? "null"}");
                
                return null;
            }
        }


        

        /// <summary>
        /// 加载实体数据（返回指定类型）
        /// </summary>
        public T LoadEntity<T>(object billNo) where T : class
        {
            return LoadEntityInternal(typeof(T), billNo) as T;
        }

        /// <summary>
        /// 根据业务类型加载实体
        /// </summary>
        public T LoadEntityByBizType<T>(BizType bizType, object billNo) where T : class
        {
            var entityType = _mappingService.GetEntityType(bizType);
            return LoadEntityInternal(entityType, billNo) as T;
        }
 

        public object LoadEntityInternal(Type entityType, object billNo)
        {
            if (entityType==null)
            {
                return null;
            }
            var bizEntityInfo = _mappingService.GetEntityInfo(entityType);
            if (bizEntityInfo == null)
            {
                _logger.LogError($"找不到实体类型的字段配置: {entityType.Name}");
                return null;
            }

            try
            {
                // 创建EntityFieldConfigBuilder实例并从BizEntityInfo复制所需字段
                var fieldConfig = EntityFieldConfigBuilder.Create()
                    .WithIdField(bizEntityInfo.IdField)
                    .WithNoField(bizEntityInfo.NoField)
                    .WithDetailProperty(bizEntityInfo.DetailProperty)
                    .WithDescriptionField(bizEntityInfo.DescriptionField)
                    .WithDiscriminatorField(bizEntityInfo.DiscriminatorField);

                // 简化反射调用，直接调用泛型LoadEntityCore方法
                var loadCoreMethod = GetType()
                    .GetMethod(nameof(LoadEntityCore), BindingFlags.NonPublic | BindingFlags.Instance)
                    .MakeGenericMethod(entityType);

                // 调用LoadEntityCore执行查询
                return loadCoreMethod.Invoke(this, new object[] { fieldConfig, billNo });
            }
            catch (Exception ex)
            {
                // 只在错误时记录详细日志，此时才构建调试信息
                string debugInfo = $"EntityLoader加载实体数据失败 - 实体类型: {entityType.Name}, BillNo: {billNo} (类型: {billNo?.GetType().Name ?? "null"})";
                string entityConfigInfo = $"实体配置信息 - IdField: {bizEntityInfo.IdField}, NoField: {bizEntityInfo.NoField}, DetailProperty: {bizEntityInfo.DetailProperty}";
                
                _logger.LogError(ex, $"{debugInfo}");
                _logger.LogError($"{entityConfigInfo}");
                _logger.LogError(ex, $"加载实体数据失败: {entityType.Name}, BillNo: {billNo}");
                
                return null;
            }
        }

        // 把反射逻辑封装到泛型方法里
        private object LoadEntityCore<T>(EntityFieldConfigBuilder cfg, object billNo) where T : class, new()
        {
            var q = CreateQueryable<T>(cfg);

            if (billNo is long id)
                q = q.Where($"{cfg.IdField} = @id", new { id });
            else if (billNo is string no)
                q = q.Where($"{cfg.NoField} = @no", new { no });

            return q.First();
        }

        /// <summary>
        /// 创建泛型查询对象
        /// </summary>
        private ISugarQueryable<T> CreateQueryable<T>(EntityFieldConfigBuilder config) where T : class
        {
            var queryable = _context.Db.Queryable<T>();

            // 包含明细
            if (!string.IsNullOrEmpty(config.DetailProperty))
            {
                // 使用反射动态包含子表
                var property = typeof(T).GetProperty(config.DetailProperty);
                if (property != null)
                {
                    queryable = queryable.IncludesByNameString(config.DetailProperty);
                }
            }

            return queryable;
        }

       



    }


}
