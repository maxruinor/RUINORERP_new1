﻿using System;
using RUINORERP.Common;
using RUINORERP.Common.Seed;
using log4net;
using Microsoft.AspNetCore.Builder;

namespace RUINORERP.Extensions.Middlewares
{
    /// <summary>
    /// 生成种子数据中间件服务
    /// </summary>
    public static class SeedDataMiddleware
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SeedDataMiddleware));
        public static void UseSeedDataMiddle(this IApplicationBuilder app, MyContext myContext, string webRootPath)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (AppSettings.app("AppSettings", "SeedDBEnabled").ObjToBool() || AppSettings.app("AppSettings", "SeedDBDataEnabled").ObjToBool())
                {
                    DBSeed.SeedAsync(myContext, webRootPath).Wait();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error occured seeding the Database.\n{e.Message}");
                throw;
            }
        }
    }
}
