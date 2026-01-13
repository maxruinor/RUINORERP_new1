using System;
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
        public static void AddSqlsugarSetup(this IServiceCollection services, ApplicationContext appContextData, string connectString)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (appContextData == null) throw new ArgumentNullException(nameof(appContextData));
            if (string.IsNullOrEmpty(connectString)) throw new ArgumentNullException(nameof(connectString));

            var logProvider = new Log4NetProviderByCustomeDb("Log4net_db.config", connectString, appContextData);
            var logger = logProvider.CreateLogger(typeof(SqlsugarSetup).FullName);

            StaticConfig.DynamicExpressionParserType = typeof(DynamicExpressionParser);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            SqlSugarScope sqlSugar = new SqlSugarScope(
                new ConnectionConfig
                {
                    DbType = SqlSugar.DbType.SqlServer,
                    ConnectionString = connectString,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    ConfigureExternalServices = new ConfigureExternalServices
                    {
                        DataInfoCacheService = new SqlSugarMemoryCacheService(memoryCache),
                    },
                    MoreSettings = new ConnMoreSettings
                    {
                        IsWithNoLockQuery = true,
                        DisableNvarchar = true,
                        IsAutoRemoveDataCache = true,
                    }
                },
                db => ConfigureDbAop(db, logger, appContextData)
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

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            
            SqlSugarScope sqlSugar = new SqlSugarScope(
                new ConnectionConfig
                {
                    DbType = SqlSugar.DbType.SqlServer,
                    ConnectionString = connectString,
                    IsAutoCloseConnection = true,
                    ConfigureExternalServices = new ConfigureExternalServices
                    {
                        DataInfoCacheService = new SqlSugarMemoryCacheService(memoryCache)
                    }
                },
                db => ConfigureDbAop(db, null, null) // 没有logger和appContextData
            );

            services.AddSingleton<ISqlSugarClient>(sqlSugar);
        }

        /// <summary>
        /// 配置数据库AOP事件
        /// </summary>
        /// <param name="db">SqlSugar数据库对象</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="appContextData">应用上下文数据</param>
        private static void ConfigureDbAop(SqlSugarClient db, ILogger logger, ApplicationContext appContextData)
        {
            db.Ado.CommandTimeOut = 30; // 单位秒

            // 配置SQL执行前事件
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                // 获取调用堆栈以识别调用方法
                string callerMethod = GetCallerMethodName();

                // 获取原生SQL并添加调用方法信息
                string nativeSql = UtilMethods.GetNativeSql(sql, pars);
                string sqlWithCaller = $"{callerMethod}:{nativeSql}";
                System.Diagnostics.Debug.WriteLine(sqlWithCaller);

                // 触发自定义检查事件
                if (CheckEvent != null)
                {
                    string formattedSql = Common.DB.SqlProfiler.FormatParam(sql, pars);
                    string formattedSqlWithCaller = $"{callerMethod}:     {formattedSql}";
                    System.Diagnostics.Debug.WriteLine(formattedSqlWithCaller);
                    CheckEvent(formattedSqlWithCaller);
                }
            };

            // 配置SQL执行错误事件
            db.Aop.OnError = (e) =>
            {
                try
                {
                    string errorsql = SqlProfiler.FormatParam(e.Sql, e.Parametres as SugarParameter[]);
                    Exception exception = e.GetBaseException();
                    
                    // 记录错误日志
                    if (logger != null)
                    {
                        logger.LogError("SQL执行错误: {ErrorMessage}, SQL: {Sql}", exception.Message, errorsql, e);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"SQL执行错误: {exception.Message}, SQL: {errorsql}");
                    }
                    
                    // 检测死锁异常
                    if (e.InnerException != null && e.InnerException is SqlException sqlEx && sqlEx.Number == 1205)
                    {
                        var deadlockInfo = new
                        {
                            Time = DateTime.Now,
                            StackTrace = e.StackTrace,
                            Sql = errorsql
                        };
                        
                        if (logger != null)
                        {
                            logger.LogError("检测到数据库死锁: {DeadlockInfo}", JsonConvert.SerializeObject(deadlockInfo), e);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"检测到数据库死锁: {JsonConvert.SerializeObject(deadlockInfo)}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"日志记录失败: {ex.Message}\n原始错误: {e.Message}");
                    if (logger != null)
                    {
                        logger.LogError("记录SQL错误日志时出错", ex);
                    }
                }
                
                // 触发提醒事件
                if (RemindEvent != null)
                {
                    RemindEvent(e);
                }
            };

            // 配置SQL执行后事件
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                // 在调试模式下记录详细信息
                if (appContextData != null && appContextData.IsDebug)
                {
                    // 调试模式下可启用详细日志记录
                }
            };

            // 配置差异日志事件（当前为空实现）
            db.Aop.OnDiffLogEvent = async u =>
            {
                // 异步延迟以避免阻塞
                await Task.Delay(0);
                // 差异日志功能暂未实现，可以根据需要启用
            };
        }

        /// <summary>
        /// 获取调用方法名称
        /// </summary>
        /// <returns>调用方法的名称</returns>
        private static string GetCallerMethodName()
        {
            string callerMethod = "未知方法";
            try
            {
                // 创建StackTrace对象，跳过3个堆栈帧（当前方法、OnLogExecuting调用和匿名方法）
                StackTrace stackTrace = new StackTrace(3, false);
                if (stackTrace.FrameCount > 0)
                {
                    // 获取第一个非框架方法
                    for (int i = 0; i < stackTrace.FrameCount; i++)
                    {
                        StackFrame frame = stackTrace.GetFrame(i);
                        MethodBase method = frame.GetMethod();
                        // 跳过系统框架方法
                        if (method != null && !method.DeclaringType.FullName.StartsWith("SqlSugar") && 
                            !method.DeclaringType.FullName.StartsWith("System") &&
                            !method.DeclaringType.FullName.StartsWith("Microsoft"))
                        {
                            callerMethod = $"{method.DeclaringType.Name}.{method.Name}";
                            break;
                        }
                    }
                }
            }
            catch { /* 捕获异常以避免影响正常SQL执行 */ }
            return callerMethod;
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

