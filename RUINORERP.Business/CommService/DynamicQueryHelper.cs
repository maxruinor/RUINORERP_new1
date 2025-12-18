using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.CommService
{
    using CacheManager.Core;
    using log4net; // 或其他日志框架，根据你的实际使用
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using RUINORERP.Common.Helper;
    using SqlSugar;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 动态表名强类型查询帮助类
    /// </summary>
    public class DynamicQueryHelper
    {
            // 依赖注入的服务
            private readonly ISqlSugarClient _sqlSugarClient;
            private readonly ICacheManager<object> _cacheManager;
            private readonly ILogger<DynamicQueryHelper> _logger;
        
            // 反射获取的方法缓存
            private static MethodInfo _queryableMethod;
     
        
            // 静态构造函数 - 初始化反射方法
            static DynamicQueryHelper()
            {
                InitializeReflectionMethods();
            }
        
            /// <summary>
            /// 初始化反射方法
            /// </summary>
            private static void InitializeReflectionMethods()
            {
                try
                {
                    // 1. 初始化Queryable方法
                    InitializeQueryableMethod();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ERROR] DynamicQueryHelper: 静态构造函数初始化失败 - {ex.Message}\n{ex.StackTrace}");
                }
            }
        
            /// <summary>
            /// 初始化Queryable方法
            /// </summary>
            private static void InitializeQueryableMethod()
            {
                try
                {
                    // 从ISqlSugarClient接口获取Queryable方法，而不是从SqlSugarClient类获取
                    // 这样可以确保方法调用时目标对象类型匹配
                    var sqlSugarClientType = typeof(ISqlSugarClient);
                    _queryableMethod = sqlSugarClientType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(m => m.Name == "Queryable" && m.IsGenericMethodDefinition && m.GetParameters().Length == 0)
                        .FirstOrDefault();

                    if (_queryableMethod != null)
                    {
                        System.Diagnostics.Debug.WriteLine("[INFO] DynamicQueryHelper: 成功初始化Queryable方法");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("[ERROR] DynamicQueryHelper: 无法初始化Queryable方法");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ERROR] DynamicQueryHelper: 初始化Queryable方法失败 - {ex.Message}");
                }
            }
        
            /// <summary>
            /// 获取 ISugarQueryable<T> 的 AS 实例方法（指定表名）
            /// </summary>
            /// <summary>
            /// 从实际查询对象获取AS方法
            /// </summary>
            /// <param name="queryableInstance">查询对象实例</param>
            private MethodInfo GetAsMethod(object queryableInstance)
            {
                if (queryableInstance == null)
                {
                    _logger.LogWarning("查询对象实例为null");
                    return null;
                }
                
                try
                {
                    // 从实际的查询对象实例获取方法，这样方法的泛型参数已经被解析
                    var asMethods = queryableInstance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(m => m.Name == "AS")
                        .ToList();
                    
                    if (asMethods.Count == 0)
                    {
                        _logger.LogWarning("未找到AS方法");
                        return null;
                    }
                    
                    // 精确查找：选择参数为单个string的方法
                    var exactMethod = asMethods.FirstOrDefault(m => 
                        m.GetParameters().Length == 1 && 
                        m.GetParameters()[0].ParameterType == typeof(string) &&
                        !m.ContainsGenericParameters // 确保方法不包含未解析的泛型参数
                    );
                    
                    if (exactMethod != null)
                    {
                        _logger.LogInformation("找到精确匹配的AS方法");
                        return exactMethod;
                    }
                    
                    // 备选方案：返回第一个非泛型方法
                    var nonGenericMethod = asMethods.FirstOrDefault(m => !m.IsGenericMethod && !m.ContainsGenericParameters);
                    if (nonGenericMethod != null)
                    {
                        _logger.LogInformation("使用非泛型AS方法");
                        return nonGenericMethod;
                    }
                    
                    _logger.LogWarning("未找到合适的AS方法重载");
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "获取AS方法失败");
                    return null;
                }
            }

            /// <summary>
        /// 获取字符串条件的 Where 扩展方法
        /// </summary>
        private MethodInfo GetWhereStringMethod()
        {
            try
            {
                // 遍历 SqlSugar 程序集中所有静态类的静态方法
                var sqlSugarAssembly = typeof(ISqlSugarClient).Assembly;
                return sqlSugarAssembly.GetTypes()
                    .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    .FirstOrDefault(m =>
                        m.Name == "Where"
                        && m.IsGenericMethod
                        && m.GetParameters().Length == 3
                        && m.GetParameters()[0].ParameterType.IsGenericType
                        && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(ISugarQueryable<>) // 第一个参数是 ISugarQueryable<T>
                        && m.GetParameters()[1].ParameterType == typeof(string) // 第二个参数是 string 条件
                        && m.GetParameters()[2].ParameterType == typeof(object) // 第三个参数是参数对象
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取Where方法失败");
                return null;
            }
        }

            /// <summary>
        /// 获取 ToList 扩展方法
        /// </summary>
        private MethodInfo GetToListMethod()
        {
            try
            {
                var sqlSugarAssembly = typeof(ISqlSugarClient).Assembly;
                return sqlSugarAssembly.GetTypes()
                    .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    .FirstOrDefault(m =>
                        m.Name == "ToList"
                        && m.IsGenericMethod
                        && m.GetParameters().Length == 1
                        && m.GetParameters()[0].ParameterType.IsGenericType
                        && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(ISugarQueryable<>) // 参数是 ISugarQueryable<T>
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取ToList方法失败");
                return null;
            }
        }
        
            /// <summary>
            /// 构造函数 - 依赖注入
            /// </summary>
            /// <param name="sqlSugarClient">SqlSugar客户端</param>
            /// <param name="cacheManager">缓存管理器</param>
            /// <param name="logger">日志记录器</param>
            public DynamicQueryHelper(
                ISqlSugarClient sqlSugarClient,
                ICacheManager<object> cacheManager,
                ILogger<DynamicQueryHelper> logger)
            {
                _sqlSugarClient = sqlSugarClient ?? throw new ArgumentNullException(nameof(sqlSugarClient));
                _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }
        
            /// <summary>
            /// 根据表名查询所有数据（强类型List）
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="entityType">实体类型</param>
            /// <param name="cacheExpiration">缓存过期时间（null则使用默认配置）</param>
            /// <returns>强类型List（实际类型为List<TEntity>）</returns>
            public object QueryAll(string tableName, Type entityType, TimeSpan? cacheExpiration = null)
            {
                // 防御性编程：参数验证
                if (string.IsNullOrEmpty(tableName))
                {
                    _logger.LogError("表名不能为空");
                    return Activator.CreateInstance(typeof(List<>).MakeGenericType(entityType ?? typeof(object)));
                }
                
                if (entityType == null)
                {
                    _logger.LogError("实体类型不能为空");
                    return new List<object>();
                }

                string cacheKey = $"DynamicQuery_{tableName}_All";

                // 尝试从缓存获取
                if (_cacheManager.Exists(cacheKey))
                {
                    try
                    {
                        return _cacheManager.Get(cacheKey);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"从缓存获取{tableName}数据失败");
                    }
                }

                // 缓存未命中，执行数据库查询
                var result = ExecuteQuery(entityType, tableName);

                // 更新缓存
                try
                {
                    if (cacheExpiration.HasValue)
                    {
                        _cacheManager.Put(cacheKey, result, cacheExpiration.Value.ToString());
                    }
                    else
                    {
                        _cacheManager.Put(cacheKey, result);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"更新{tableName}缓存失败");
                }

                return result;
            }
        
            /// <summary>
            /// 根据表名和条件查询数据（强类型List）
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="entityType">实体类型</param>
            /// <param name="whereCondition">Where条件表达式</param>
            /// <param name="parameters">参数对象</param>
            /// <param name="cacheExpiration">缓存过期时间（null则使用默认配置）</param>
            /// <returns>强类型List（实际类型为List<TEntity>）</returns>
            public object QueryByCondition(string tableName, Type entityType, string whereCondition, object parameters = null, TimeSpan? cacheExpiration = null)
            {
                // 防御性编程：参数验证
                if (string.IsNullOrEmpty(tableName))
                {
                    _logger.LogError("表名不能为空");
                    return Activator.CreateInstance(typeof(List<>).MakeGenericType(entityType ?? typeof(object)));
                }
                
                if (entityType == null)
                {
                    _logger.LogError("实体类型不能为空");
                    return new List<object>();
                }

                // 为条件查询生成缓存键
                string conditionHash = string.IsNullOrEmpty(whereCondition) ? "None" : 
                    (parameters != null ? $"{whereCondition}_{JsonConvert.SerializeObject(parameters).GetHashCode()}" : whereCondition.GetHashCode().ToString());
                string cacheKey = $"DynamicQuery_{tableName}_{conditionHash}";

                // 尝试从缓存获取
                if (_cacheManager.Exists(cacheKey))
                {
                    try
                    {
                        return _cacheManager.Get(cacheKey);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"从缓存获取{tableName}条件查询数据失败");
                    }
                }

                // 缓存未命中，执行数据库查询
                var result = ExecuteQuery(entityType, tableName, whereCondition, parameters);

                // 更新缓存
                try
                {
                    if (cacheExpiration.HasValue)
                    {
                        _cacheManager.Put(cacheKey, result, cacheExpiration.Value.ToString());
                    }
                    else
                    {
                        _cacheManager.Put(cacheKey, result);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"更新{tableName}条件查询缓存失败");
                }

                return result;
            }
        
            /// <summary>
            /// 执行动态查询（内部方法）
            /// </summary>
            /// <param name="entityType">实体类型</param>
            /// <param name="tableName">表名</param>
            /// <param name="whereCondition">条件表达式</param>
            /// <param name="parameters">参数对象</param>
            /// <returns>查询结果</returns>
            private object ExecuteQuery(Type entityType, string tableName, string whereCondition = null, object parameters = null)
            {
                try
                {
                    // 1. 创建 Queryable<T>：db.Queryable<T>()
                    MethodInfo queryableMethod = _queryableMethod ?? typeof(ISqlSugarClient).GetMethod("Queryable", Type.EmptyTypes);
                    if (queryableMethod == null)
                    {
                        _logger.LogError("无法获取Queryable方法");
                        return Activator.CreateInstance(typeof(List<>).MakeGenericType(entityType));
                    }
                    
                    var genericQueryableMethod = queryableMethod.MakeGenericMethod(entityType);
                    object queryable = genericQueryableMethod.Invoke(_sqlSugarClient, null);

                    // 2. 调用 AS 实例方法指定表名
                    MethodInfo asMethod = GetAsMethod(queryable);
                    if (asMethod != null && !asMethod.ContainsGenericParameters)
                    {
                        try
                        {
                            _logger.Debug($"尝试调用AS方法，表名: {tableName}");
                            queryable = asMethod.Invoke(queryable, new[] { tableName });
                            _logger.Debug($"AS方法调用成功，表名: {tableName}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"调用AS方法指定表名{tableName}失败，将使用默认表名继续查询");
                            // 即使AS方法调用失败，也继续执行查询，使用默认表名
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"无法获取有效的AS方法或方法包含未解析的泛型参数，将使用默认表名查询{entityType.Name}");
                    }

                    // 3. 调用 Where 扩展方法（如有条件）
                    if (!string.IsNullOrEmpty(whereCondition))
                    {
                        try
                        {
                            MethodInfo whereMethod = GetWhereStringMethod();
                            if (whereMethod != null)
                            {
                                var genericWhereMethod = whereMethod.MakeGenericMethod(entityType);
                                queryable = genericWhereMethod.Invoke(null, new[] { queryable, whereCondition, parameters });
                                _logger.LogInformation($"成功应用条件查询: {whereCondition}");
                            }
                            else
                            {
                                _logger.LogWarning("无法获取Where方法，将跳过条件过滤");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"应用条件查询失败: {whereCondition}，将使用无条件查询继续");
                            // 即使Where方法调用失败，也继续执行查询
                        }
                    }

                    // 4. 调用 ToList 扩展方法
                    try
                    {
                        MethodInfo toListMethod = GetToListMethod();
                        if (toListMethod != null)
                        {
                            var genericToListMethod = toListMethod.MakeGenericMethod(entityType);
                            return genericToListMethod.Invoke(null, new[] { queryable });
                        }
                        
                        // 备选方案：尝试实例方法
                        _logger.Debug("尝试使用ToList实例方法");
                        var instanceToListMethod = queryable.GetType().GetMethod("ToList", Type.EmptyTypes);
                        if (instanceToListMethod != null)
                        {
                            return instanceToListMethod.Invoke(queryable, null);
                        }
                        
                        _logger.LogError("无法获取ToList方法，将返回空列表");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"执行ToList操作失败");
                    }
                    
                    // 最终备选方案：返回空的强类型列表
                    return Activator.CreateInstance(typeof(List<>).MakeGenericType(entityType));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"执行{tableName}查询失败");
                    return Activator.CreateInstance(typeof(List<>).MakeGenericType(entityType));
                }
            }
        }
    
}