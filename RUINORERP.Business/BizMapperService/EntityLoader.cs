using Microsoft.Extensions.Logging;
using RUINORERP.Business;
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

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 统一实体加载服务
    /// </summary>
    public class EntityLoader
    {
        private readonly EntityBizMappingService _mappingService;
        private readonly ApplicationContext _context;
        private readonly ILogger<EntityLoader> _logger;

        public EntityLoader(
            EntityBizMappingService mappingService,
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

            var config = _mappingService.GetFieldConfig(entityType);
            if (config == null)
            {
                _logger.LogError($"找不到实体类型的字段配置: {entityType.Name}");
                return null;
            }

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
                    queryable = queryable.Where($"{config.IdField} = @id", new { id });
                }
                else if (billNo is string no)
                {
                    queryable = queryable.Where($"{config.NoField} = @no", new { no });
                }

                return queryable.Single();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"加载实体数据失败: {entityType.Name}, BillNo: {billNo}");
                return null;
            }
        }


        ///// <summary>
        ///// 加载实体数据（返回动态类型）
        ///// </summary>
        //public dynamic LoadEntity(string tableName, object billNo)
        //{
        //    var entityType = _mappingService.GetEntityTypeByTableName(tableName);
        //    if (entityType == null)
        //    {
        //        _logger.LogError($"找不到表名对应的实体类型: {tableName}");
        //        return null;
        //    }

        //    return LoadEntityInternal(entityType, billNo);
        //}

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

        ///// <summary>
        ///// 内部加载方法（核心实现）
        ///// </summary>
        //private object LoadEntityInternal(Type entityType, object billNo)
        //{
        //    var config = _mappingService.GetFieldConfig(entityType);
        //    if (config == null)
        //    {
        //        _logger.LogError($"找不到实体类型的字段配置: {entityType.Name}");
        //        return null;
        //    }

        //    try
        //    {
        //        // 使用泛型方法创建查询
        //        var method = typeof(EntityLoader).GetMethod("CreateQueryable",
        //            BindingFlags.NonPublic | BindingFlags.Instance)
        //            .MakeGenericMethod(entityType);

        //        var queryable = method.Invoke(this, new object[] { config }) as ISugarQueryable<T>;

        //        // 添加查询条件
        //        if (billNo is long id)
        //        {
        //            queryable = queryable.Where($"{config.IdField} = @id", new { id });
        //        }
        //        else if (billNo is string no)
        //        {
        //            queryable = queryable.Where($"{config.NoField} = @no", new { no });
        //        }

        //        // 执行查询
        //        var result = queryable.Single();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"加载实体数据失败: {entityType.Name}, BillNo: {billNo}");
        //        return null;
        //    }
        //}


        public object LoadEntityInternal(Type entityType, object billNo)
        {
            var cfg = _mappingService.GetFieldConfig(entityType);
            if (cfg == null)
            {
                _logger.LogError($"找不到实体类型的字段配置: {entityType.Name}");
                return null;
            }

            try
            {
                // 1) 拿到 CreateQueryable<T>
                var createMethod = GetType()
                    .GetMethod(nameof(CreateQueryable), BindingFlags.NonPublic | BindingFlags.Instance)
                    .MakeGenericMethod(entityType);

                // 2) 调 CreateQueryable<T>() 得到 ISugarQueryable<T>
                var queryable = createMethod.Invoke(this, new object[] { cfg });

                // 3) 给 ISugarQueryable<T>.Where(...) 拼接动态条件
                var sugarType = queryable.GetType();          // 运行时 T 已确定
                var whereMethod = sugarType.GetMethods()
                    .First(m => m.Name == "Where" && m.GetParameters().Length == 2 &&
                                m.GetParameters()[0].ParameterType == typeof(string) &&
                                m.GetParameters()[1].ParameterType == typeof(object))
                    .MakeGenericMethod(entityType);           // 其实这里不需要再 MakeGeneric，因为 sugarType 已含 T


         


                // 根据主键/编号字段构造 where
                string fieldName;
                object param;
                if (billNo is long id)
                {
                    fieldName = cfg.IdField;
                    param = new { id };
                }
                else if (billNo is string no)
                {
                    fieldName = cfg.NoField;
                    param = new { no };
                }
                else
                {
                    throw new ArgumentException("billNo 必须是 long 或 string");
                }

                string paramName = fieldName;                    // RepairOrderNo
                object paramObj = new Dictionary<string, object>
                {
                    [paramName] = billNo                         // Key 必须与 @RepairOrderNo 同名
                };


                // 动态调用 Where
                queryable = whereMethod.Invoke(queryable, new object[] { $"{fieldName} = @{fieldName}", paramObj });

        

                // 4) 取 Single()
                var singleMethod = sugarType.GetMethod("Single", Type.EmptyTypes);
                return singleMethod.Invoke(queryable, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"加载实体数据失败: {entityType.Name}, BillNo: {billNo}");
                return null;
            }
        }

        // 把反射逻辑封装到泛型方法里
        private object LoadEntityCore<T>(EntityFieldConfig cfg, object billNo) where T : class, new()
        {
            var q = CreateQueryable<T>(cfg);

            if (billNo is long id)
                q = q.Where($"{cfg.IdField} = @id", new { id });
            else if (billNo is string no)
                q = q.Where($"{cfg.NoField} = @no", new { no });

            return q.Single();
        }

        /// <summary>
        /// 创建泛型查询对象
        /// </summary>
        private ISugarQueryable<T> CreateQueryable<T>(EntityFieldConfig config) where T : class
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
