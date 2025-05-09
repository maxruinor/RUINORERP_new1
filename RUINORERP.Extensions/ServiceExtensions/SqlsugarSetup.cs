using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.DB;
using RUINORERP.Common.Log4Net;
using RUINORERP.Model.Context;
using SqlSugar;

namespace RUINORERP.Extensions
{

    /// <summary>
    /// 2023-9-18 前使用
    /// 但是2024-7-30调试时 发现在使用第一个方法
    /// </summary>
    public static class SqlsugarSetup
    {

        public delegate void CheckHandler(string sql);

        [Browsable(true), Description("引发外部事件")]
        public static event CheckHandler CheckEvent;
      
        public static void AddSqlsugarSetup(this IServiceCollection services,
        ApplicationContext AppContextData, string connectString,
            string dbName = "ConnectString")
        {
            
            var logProvider = new Log4NetProviderByCustomeDb("Log4net_db.config", connectString, AppContextData);
            var logger = logProvider.CreateLogger("SqlsugarSetup");

            StaticConfig.DynamicExpressionParserType = typeof(DynamicExpressionParser);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = SqlSugar.DbType.SqlServer,
                ConnectionString = connectString,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,//就改这一行

                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    DataInfoCacheService = new SqlSugarMemoryCacheService(memoryCache),
                },
                MoreSettings = new ConnMoreSettings() { IsWithNoLockQuery = true, DisableNvarchar = true }
            },
                db =>
                {
                    //单例参数配置，所有上下文生效       
                    db.Aop.OnLogExecuting = (sql, pars) =>
                        {
                            //获取原生SQL推荐 5.1.4.63  性能OK
//                            Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));

                            if (CheckEvent != null)
                            {
                                //kimi查询到说这个性能更好
                                CheckEvent(Common.DB.SqlProfiler.FormatParam(sql, pars));
                            }
                        };


                    //技巧：拿到非ORM注入对象
                    //services.GetService<注入对象>();

                    // 差异日志

                    db.Aop.OnDiffLogEvent = async u =>
                    {
                        await Task.Delay(0);
                        /*
                        if (!db.EnableDiffLog) return;

                        var LogDiff = new SysLogDiff

                        {

                            // 操作后记录（字段描述、列名、值、表名、表描述）

                            AfterData = JsonConvert.SerializeObject(u.AfterData),

                            // 操作前记录（字段描述、列名、值、表名、表描述）

                            BeforeData = JsonConvert.SerializeObject(u.BeforeData),

                            // 传进来的对象

                            BusinessData = JsonConvert.SerializeObject(u.BusinessData),

                            // 枚举（insert、update、delete）

                            DiffType = u.DiffType.ToString(),

                            Sql = UtilMethods.GetSqlString(config.DbType, u.Sql, u.Parameters),

                            Parameters = JsonConvert.SerializeObject(u.Parameters),

                            Duration = u.Time == null ? 0 : (long)u.Time.Value.TotalMilliseconds

                        };

                        await db.GetConnectionScope(SqlSugarConst.ConfigId).Insertable(LogDiff).ExecuteCommandAsync();

                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine(DateTime.Now + $"\r\n**********差异日志开始**********\r\n{Environment.NewLine}{JsonConvert.SerializeObject(LogDiff)}{Environment.NewLine}**********差异日志结束**********\r\n");
                        */
                    };

                    db.Aop.OnError = (e) =>
                    {
                        try
                        {
                            logger.LogError(e.Message + SqlProfiler.FormatParam(e.Sql, e.Parametres as SugarParameter[]));
                        }
                        catch (Exception exx)
                        {
                            var aa = (e.Parametres as SugarParameter[]).ToDictionary(it => it.ParameterName, it => it.Value);
                        }

                    };

                    db.Aop.OnLogExecuted = (sql, pars) =>
                    {
                        //Console.WriteLine("执行成功：" + sql);//输出sql
                        if (AppContextData.IsDebug)
                        {
                            Console.WriteLine(Common.DB.SqlProfiler.FormatParam(sql, pars));
                            logger.LogInformation(SqlProfiler.FormatParam(sql, pars as SugarParameter[]));
                        }

                    };

                });

            services.AddSingleton<ISqlSugarClient>(sqlSugar);//这边是SqlSugarScope用AddSingleton
            AppContextData.Db = sqlSugar;
        }


        public static void AddSqlsugarSetup(this IServiceCollection services,
            IConfiguration configuration, string dbName = "ConnectString")
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = SqlSugar.DbType.SqlServer,
                ConnectionString = configuration[dbName],
                IsAutoCloseConnection = true,

                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    DataInfoCacheService = new SqlSugarMemoryCacheService(memoryCache)
                }

            },
                db =>
                {
                    //单例参数配置，所有上下文生效       
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        Console.WriteLine(sql);//输出sql

                        if (CheckEvent != null)
                        {
                            CheckEvent(Common.DB.SqlProfiler.FormatParam(sql, pars));
                        }
                    };


                    //技巧：拿到非ORM注入对象
                    //services.GetService<注入对象>();

                    // 差异日志

                    db.Aop.OnDiffLogEvent = async u =>
                    {
                        await Task.Delay(0);
                        /*
                        if (!db.EnableDiffLog) return;

                        var LogDiff = new SysLogDiff

                        {

                            // 操作后记录（字段描述、列名、值、表名、表描述）

                            AfterData = JsonConvert.SerializeObject(u.AfterData),

                            // 操作前记录（字段描述、列名、值、表名、表描述）

                            BeforeData = JsonConvert.SerializeObject(u.BeforeData),

                            // 传进来的对象

                            BusinessData = JsonConvert.SerializeObject(u.BusinessData),

                            // 枚举（insert、update、delete）

                            DiffType = u.DiffType.ToString(),

                            Sql = UtilMethods.GetSqlString(config.DbType, u.Sql, u.Parameters),

                            Parameters = JsonConvert.SerializeObject(u.Parameters),

                            Duration = u.Time == null ? 0 : (long)u.Time.Value.TotalMilliseconds

                        };

                        await db.GetConnectionScope(SqlSugarConst.ConfigId).Insertable(LogDiff).ExecuteCommandAsync();

                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine(DateTime.Now + $"\r\n**********差异日志开始**********\r\n{Environment.NewLine}{JsonConvert.SerializeObject(LogDiff)}{Environment.NewLine}**********差异日志结束**********\r\n");
                        */
                    };


                });

            services.AddSingleton<ISqlSugarClient>(sqlSugar);//这边是SqlSugarScope用AddSingleton
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

