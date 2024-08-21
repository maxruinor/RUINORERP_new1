﻿using System;
using RUINORERP.Common;
using RUINORERP.IServices;
using RUINORERP.Tasks;
using log4net;
using Microsoft.AspNetCore.Builder;

namespace RUINORERP.Extensions.Middlewares
{
    /// <summary>
    /// Quartz 启动服务
    /// </summary>
    public static class QuartzJobMiddleware
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(QuartzJobMiddleware));
        public static void UseQuartzJobMiddleware(this IApplicationBuilder app, ITasksQzServices tasksQzServices, ISchedulerCenter schedulerCenter)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (AppSettings.app("Middleware", "QuartzNetJob", "Enabled").ObjToBool())
                {

                    var allQzServices = tasksQzServices.Query().Result;
                    foreach (var item in allQzServices)
                    {
                        if (item.IsStart)
                        {
                            var result = schedulerCenter.AddScheduleJobAsync(item).Result;
                            if (result.success)
                            {
                                Console.WriteLine($"QuartzNetJob{item.Name}启动成功！");
                            }
                            else
                            {
                                Console.WriteLine($"QuartzNetJob{item.Name}启动失败！错误信息：{result.msg}");
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Log.Error($"An error was reported when starting the job service.\n{e.Message}");
                throw;
            }
        }
    }
}
