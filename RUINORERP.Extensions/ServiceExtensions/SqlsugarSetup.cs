using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Common.DB;
using RUINORERP.Common.Log4Net;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using SqlSugar;

namespace RUINORERP.Extensions
{
    /// <summary>
    /// SqlSugar ORM 配置扩展类，使用中
    /// 提供数据库连接和AOP配置的静态方法
    /// </summary>
    public static class SqlsugarSetup
    {
        /// <summary>
        /// SQL日志检查委托
        /// </summary>
        /// <param name="sql">执行的SQL语句</param>
        public delegate void CheckHandler(string sql);

        /// <summary>
        /// SQL执行检查事件
        /// </summary>
        [Browsable(true), Description("SQL执行检查事件")]
        public static event CheckHandler CheckEvent;

        /// <summary>
        /// SQL错误提醒委托
        /// </summary>
        /// <param name="sugarException">SqlSugar异常</param>
        /// <returns>是否已处理</returns>
        public delegate bool RemindHandler(SqlSugarException sugarException);

        /// <summary>
        /// SQL错误提醒事件
        /// </summary>
        [Browsable(true), Description("SQL错误提醒事件")]
        public static event RemindHandler RemindEvent;

        /// <summary>
        /// 添加SqlSugar配置，使用ApplicationContext
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="appContextData">应用上下文数据</param>
        /// <param name="connectString">数据库连接字符串</param>
        public static void AddSqlsugarSetup(this IServiceCollection services, ApplicationContext appContextData, string connectString, IConfiguration configuration = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (appContextData == null) throw new ArgumentNullException(nameof(appContextData));
            if (string.IsNullOrEmpty(connectString)) throw new ArgumentNullException(nameof(connectString));

            var logProvider = new Log4NetProvider();
            var logger = logProvider.CreateLogger(typeof(SqlsugarSetup).FullName);

            StaticConfig.DynamicExpressionParserType = typeof(DynamicExpressionParser);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            // 检查连接字符串是否已包含连接池配置，如果没有则添加默认值
            string finalConnectionString = EnsureConnectionPoolSettings(connectString);

            SqlSugarScope sqlSugar = new SqlSugarScope(
                new ConnectionConfig
                {
                    DbType = SqlSugar.DbType.SqlServer,
                    ConnectionString = finalConnectionString,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    ConfigureExternalServices = new ConfigureExternalServices
                    {
                        DataInfoCacheService = new SqlSugarMemoryCacheService(memoryCache),
                    },
                    MoreSettings = new ConnMoreSettings
                    {
                        IsWithNoLockQuery = true, // 查询时使用 WITH(NOLOCK)
                        DisableNvarchar = true,
                        IsAutoRemoveDataCache = true,
                    }
                },
                db => ConfigureDbAop(db, logger, appContextData, configuration)
            );

            services.AddSingleton<ISqlSugarClient>(sqlSugar); // SqlSugarScope 应当使用单例模式
            appContextData.Db = sqlSugar;
        }

        /// <summary>
        /// 添加SqlSugar配置，使用IConfiguration
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">配置对象</param>
        /// <param name="dbName">数据库配置键名</param>
        public static void AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration, string dbName = "ConnectString")
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrEmpty(dbName)) throw new ArgumentNullException(nameof(dbName));

            string connectString = configuration[dbName];
            if (string.IsNullOrEmpty(connectString))
            {
                throw new ArgumentException($"配置键 '{dbName}' 未找到或连接字符串为空", nameof(dbName));
            }

            // 检查连接字符串是否已包含连接池配置，如果没有则添加默认值
            string finalConnectionString = EnsureConnectionPoolSettings(connectString);
            
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            
            SqlSugarScope sqlSugar = new SqlSugarScope(
                new ConnectionConfig
                {
                    DbType = SqlSugar.DbType.SqlServer,
                    ConnectionString = finalConnectionString,
                    IsAutoCloseConnection = true,
                    ConfigureExternalServices = new ConfigureExternalServices
                    {
                        DataInfoCacheService = new SqlSugarMemoryCacheService(memoryCache)
                    }
                },
                db => ConfigureDbAop(db, null, null, configuration) // 没有logger和appContextData
            );

            services.AddSingleton<ISqlSugarClient>(sqlSugar);
        }

        /// <summary>
        /// 配置数据库 AOP 事件
        /// </summary>
        /// <param name="db">SqlSugar 数据库对象</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="appContextData">应用上下文数据</param>
        private static void ConfigureDbAop(SqlSugarClient db, ILogger logger, ApplicationContext appContextData, IConfiguration configuration = null)
        {
            ApplyAopConfiguration(db, logger, appContextData, configuration);
        }
                
        /// <summary>
        /// 为 SqlSugarClient 应用 AOP 配置（可重复调用）
        /// </summary>
        /// <param name="db">SqlSugar 客户端</param>
        /// <param name="logger">日志记录器（可选）</param>
        /// <param name="appContextData">应用上下文数据（可选）</param>
        /// <param name="configuration">配置信息（可选）</param>
        public static void ApplyAopConfiguration(SqlSugarClient db, ILogger logger = null, ApplicationContext appContextData = null, IConfiguration configuration = null)
        {
            // 从配置读取是否启用调用方法名记录
            bool enableCallerMethod = configuration?.GetValue<bool>("SqlSugar:EnableCallerMethod", false) ?? false;
            
            // 配置 SQL 执行前事件
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                string callerMethod = string.Empty;
                
                if (enableCallerMethod)
                {
                    callerMethod = GetCallerMethodName();
                }

                string nativeSql = UtilMethods.GetNativeSql(sql, pars);
                string sqlWithCaller = string.IsNullOrEmpty(callerMethod) ? nativeSql : $"{callerMethod}:{nativeSql}";

                if (CheckEvent != null)
                {
                    string formattedSql = Common.DB.SqlProfiler.FormatParam(sql, pars);
                    string formattedSqlWithCaller = string.IsNullOrEmpty(callerMethod) ? formattedSql : $"{callerMethod}:     {formattedSql}";
                    CheckEvent(formattedSqlWithCaller);
                }
            };

            // 配置 SQL 执行错误事件
            db.Aop.OnError = (e) =>
            {
                try
                {
                    string errorsql = SqlProfiler.FormatParam(e.Sql, e.Parametres as SugarParameter[]);
                    Exception exception = e.GetBaseException();
                    
                    if (logger != null)
                    {
                        logger.LogError("SQL 执行错误：{ErrorMessage}, SQL: {Sql}", exception.Message, errorsql, e);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"SQL 执行错误：{exception.Message}, SQL: {errorsql}");
                    }
                    
                    // ✅ 死锁检测与性能监控
                    if (e.InnerException != null && e.InnerException is SqlException sqlEx && sqlEx.Number == 1205)
                    {
                        var deadlockInfo = new
                        {
                            Time = DateTime.Now,
                            StackTrace = e.StackTrace,
                            Sql = errorsql,
                            Parameters = (e.Parametres as System.Collections.IEnumerable)?
                                .Cast<SugarParameter>()?
                                .Select(p => $"{p.ParameterName}={p.Value}")?.ToArray() ?? Array.Empty<string>()
                        };
                                            
                        if (logger != null)
                        {
                            logger.LogError("检测到数据库死锁：{DeadlockInfo}", JsonConvert.SerializeObject(deadlockInfo), e);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"检测到数据库死锁：{JsonConvert.SerializeObject(deadlockInfo)}");
                        }
                        
                        // ✅ 记录到性能监控系统
                        try
                        {
                            var tableName = ExtractTableNameFromSql(errorsql);
                            var operation = GetOperationTypeFromSql(errorsql);
                            
                            RUINORERP.Repository.UnitOfWorks.TransactionMetrics.RecordDeadlock(
                                tableName,
                                operation,
                                TimeSpan.FromSeconds(30), // 估算等待时间
                                errorsql,
                                "Unknown"
                            );
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"记录死锁统计失败：{ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"日志记录失败：{ex.Message}\n原始错误：{e.Message}");
                    if (logger != null)
                    {
                        logger.LogError("记录 SQL 错误日志时出错", ex);
                    }
                }
                
                if (RemindEvent != null)
                {
                    RemindEvent(e);
                }
            };

            // 配置 SQL 执行后事件
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                if (appContextData != null && appContextData.IsDebug)
                {
                    // 调试模式下可启用详细日志记录
                }
            };

            // 配置差异日志事件
            db.Aop.OnDiffLogEvent = async u =>
            {
                await Task.Delay(0);
            };
        }

        /// <summary>
        /// 获取调用方法名称
        /// </summary>
        /// <returns>调用方法的名称</returns>
        // 使用缓存减少堆栈跟踪计算
        private static readonly ConcurrentDictionary<string, string> _methodNameCache = new();
        private const int MaxCacheSize = 10000; // 最大缓存条目数，防止内存泄漏
                
        private static string GetCallerMethodName()
        {
            string callerMethod = "未知方法";
            try
            {
                // 创建 StackTrace 对象，跳过 3 个堆栈帧（当前方法、OnLogExecuting 调用和匿名方法）
                StackTrace stackTrace = new StackTrace(3, false);
                if (stackTrace.FrameCount > 0)
                {
                    // 获取第一个非框架方法
                    for (int i = 0; i < Math.Min(stackTrace.FrameCount, 10); i++) // 限制最多检查 10 层
                    {
                        StackFrame frame = stackTrace.GetFrame(i);
                        MethodBase method = frame.GetMethod();
                        // 跳过系统框架方法
                        if (method != null && !method.DeclaringType.FullName.StartsWith("SqlSugar") && 
                            !method.DeclaringType.FullName.StartsWith("System") &&
                            !method.DeclaringType.FullName.StartsWith("Microsoft"))
                        {
                            string key = $"{method.DeclaringType.FullName}.{method.Name}";
                                    
                            // 使用缓存
                            if (_methodNameCache.TryGetValue(key, out var cachedName))
                            {
                                callerMethod = cachedName;
                            }
                            else
                            {
                                callerMethod = $"{method.DeclaringType.Name}.{method.Name}";
                                
                                // 防止缓存无限增长：如果超过最大容量，清理部分旧缓存
                                if (_methodNameCache.Count >= MaxCacheSize)
                                {
                                    // 简单策略：清除一半的缓存项
                                    var keysToRemove = _methodNameCache.Keys.Take(MaxCacheSize / 2).ToList();
                                    foreach (var k in keysToRemove)
                                    {
                                        _methodNameCache.TryRemove(k, out _);
                                    }
                                }
                                
                                _methodNameCache.TryAdd(key, callerMethod);
                            }
                            break;
                        }
                    }
                }
            }
            catch { /* 捕获异常以避免影响正常 SQL 执行 */ }
            return callerMethod;
        }
                
        /// <summary>
        /// 从 SQL 中提取表名（简化版本）
        /// </summary>
        private static string ExtractTableNameFromSql(string sql)
        {
            try
            {
                if (string.IsNullOrEmpty(sql)) return "Unknown";
                        
                sql = sql.Trim().ToUpper();
                        
                // INSERT INTO [TableName]
                if (sql.StartsWith("INSERT INTO"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(sql, @"INSERT\s+INTO\s+\[?(\w+)\]?");
                    if (match.Success) return match.Groups[1].Value;
                }
                        
                // UPDATE [TableName]
                if (sql.StartsWith("UPDATE"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(sql, @"UPDATE\s+\[?(\w+)\]?");
                    if (match.Success) return match.Groups[1].Value;
                }
                        
                // DELETE FROM [TableName]
                if (sql.StartsWith("DELETE FROM"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(sql, @"DELETE\s+FROM\s+\[?(\w+)\]?");
                    if (match.Success) return match.Groups[1].Value;
                }
                        
                // SELECT ... FROM [TableName]
                if (sql.StartsWith("SELECT"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(sql, @"FROM\s+\[?(\w+)\]?");
                    if (match.Success) return match.Groups[1].Value;
                }
            }
            catch { }
                    
            return "Unknown";
        }
                
        /// <summary>
        /// 从 SQL 中获取操作类型
        /// </summary>
        private static string GetOperationTypeFromSql(string sql)
        {
            if (string.IsNullOrEmpty(sql)) return "Unknown";
                    
            sql = sql.Trim().ToUpper();
                    
            if (sql.StartsWith("INSERT")) return "INSERT";
            if (sql.StartsWith("UPDATE")) return "UPDATE";
            if (sql.StartsWith("DELETE")) return "DELETE";
            if (sql.StartsWith("SELECT")) return "SELECT";
                    
            return "Unknown";
        }

        /// <summary>
        /// 确保连接字符串包含连接池配置
        /// 针对高并发场景（50-100客户端），优化SQL Server连接池配置
        /// 超时参数说明：
        /// - Connect Timeout: 连接超时（建立连接等待时间），默认15秒
        /// - Connection Lifetime: 连接最大生命周期（秒）
        /// 注意：Command Timeout 不是连接字符串参数，应通过 SqlSugar API 设置
        /// </summary>
        /// <param name="connectionString">原始连接字符串</param>
        /// <returns>包含连接池配置的连接字符串</returns>
        private static string EnsureConnectionPoolSettings(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return connectionString;

            // 检查是否已包含连接池配置
            bool hasPooling = connectionString.IndexOf("Pooling", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasMaxPoolSize = connectionString.IndexOf("Max Pool Size", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasMinPoolSize = connectionString.IndexOf("Min Pool Size", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasConnectTimeout = connectionString.IndexOf("Connect Timeout", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    connectionString.IndexOf("Connection Timeout", StringComparison.OrdinalIgnoreCase) >= 0;
            bool hasConnectionLifetime = connectionString.IndexOf("Connection Lifetime", StringComparison.OrdinalIgnoreCase) >= 0;

            if (!hasPooling && !hasMaxPoolSize)
            {
                // 默认启用连接池
                connectionString += ";Pooling=true";
            }

            // 为高并发场景设置合理的连接池大小
            // 默认最大连接数为100，最小为10，可根据实际情况调整
            if (!hasMaxPoolSize)
            {
                connectionString += ";Max Pool Size=100";
            }

            if (!hasMinPoolSize)
            {
                connectionString += ";Min Pool Size=10";
            }

            // 设置连接超时（默认30秒）- 连接建立等待时间
            if (!hasConnectTimeout)
            {
                connectionString += ";Connect Timeout=30";
            }

            // 设置连接生命周期（默认300秒 = 5分钟）
            // 避免长时间占用连接，促进连接复用
            if (!hasConnectionLifetime)
            {
                connectionString += ";Connection Lifetime=300";
            }

            return connectionString;
        }



    }


    public class SqlSugarTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        public SqlSugarTypeProvider(ParsingConfig config, bool cacheCustomTypes = true) : base(config, cacheCustomTypes)
        {
        }

        public override HashSet<Type> GetCustomTypes()
        {
            var customTypes = base.GetCustomTypes();
            customTypes.Add(typeof(SqlFunc));
            return customTypes;
        }
    }

}

