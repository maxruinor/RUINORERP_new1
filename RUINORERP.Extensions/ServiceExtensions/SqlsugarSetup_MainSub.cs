﻿using RUINORERP.Common.Helper;
using RUINORERP.Common.DB;
using RUINORERP.Common.LogHelper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace RUINORERP.Extensions
{
    /// <summary>
    /// SqlSugar 启动服务
    /// </summary>
    [Obsolete]
    public static class SqlsugarSetup_MainSub
    {
        private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

       
        public static void AddSqlsugarSetup_MainSub(this IServiceCollection services,IConfiguration configuration,
            string dbName = "ConnectString")
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 默认添加主数据库连接
            MainDb.CurrentDbConnId = AppSettings.app(new string[] { "MainDB" });

            // SqlSugarScope是线程安全，可使用单例注入
            // 参考：https://www.donet5.com/Home/Doc?typeId=1181
            services.AddSingleton<ISqlSugarClient>(o =>
            {
                var memoryCache = o.GetRequiredService<IMemoryCache>();

                // 连接字符串
                var listConfig = new List<ConnectionConfig>();
                // 从库
                var listConfig_Slave = new List<SlaveConnectionConfig>();
                BaseDBConfig.MutiConnectionString.slaveDbs.ForEach(s =>
                {
                    listConfig_Slave.Add(new SlaveConnectionConfig()
                    {
                        HitRate = s.HitRate,
                        ConnectionString = s.Connection
                    });
                });

                BaseDBConfig.MutiConnectionString.allDbs.ForEach(m =>
                {
                    listConfig.Add(new ConnectionConfig()
                    {
                        ConfigId = m.ConnId.ObjToString().ToLower(),
                        ConnectionString = m.Connection,
                        DbType = (DbType)m.DbType,
                        IsAutoCloseConnection = true,
                        // Check out more information: https://github.com/anjoy8/RUINORERP/issues/122
                        //IsShardSameThread = false,
                        AopEvents = new AopEvents
                        {
                            OnLogExecuting = (sql, p) =>
                            {
                                if (AppSettings.app(new string[] { "AppSettings", "SqlAOP", "Enabled" }).ObjToBool())
                                {
                                    if (AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToFile", "Enabled" }).ObjToBool())
                                    {
                                        Parallel.For(0, 1, e =>
                                        {
                                            //MiniProfiler.Current.CustomTiming("SQL：", GetParas(p) + "【SQL语句】：" + sql);
                                            //LogLock.OutSql2Log("SqlLog", new string[] { GetParas(p), "【SQL语句】：" + sql });
                                            LogLock.OutLogAOP("SqlLog", "", new string[] { sql.GetType().ToString(), GetParas(p), "【SQL语句】：" + sql });

                                        });
                                    }
                                    if (AppSettings.app(new string[] { "AppSettings", "SqlAOP", "LogToConsole", "Enabled" }).ObjToBool())
                                    {
                                        Common.ConsoleHelper.WriteColorLine(string.Join("\r\n", new string[] { "--------", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} ：" + GetWholeSql(p, sql) }), ConsoleColor.DarkCyan);
                                    }
                                }
                            },
                        },
                        MoreSettings = new ConnMoreSettings()
                        {
                            //IsWithNoLockQuery = true,
                            IsAutoRemoveDataCache = true
                        },
                        // 从库
                        SlaveConnectionConfigs = listConfig_Slave,
                        // 自定义特性
                        ConfigureExternalServices = new ConfigureExternalServices()
                        {
                            DataInfoCacheService = new SqlSugarMemoryCacheService(memoryCache),
                            EntityService = (property, column) =>
                            {
                                if (column.IsPrimarykey && property.PropertyType == typeof(int))
                                {
                                    column.IsIdentity = true;
                                }
                            }
                        },
                        InitKeyType = InitKeyType.Attribute
                    }
                   );
                });
                return new SqlSugarScope(listConfig);
            });
        }

        private static string GetWholeSql(SugarParameter[] paramArr, string sql)
        {
            foreach (var param in paramArr)
            {
                sql.Replace(param.ParameterName, param.Value.ObjToString());
            }

            return sql;
        }

        private static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }

            return key;
        }
    }
}