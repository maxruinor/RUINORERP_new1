using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Helpers;
using WorkflowCore.Interface;
using System.Timers;
using System.Threading.Tasks;
using System;

namespace RUINORERP.Server.Workflow
{
    /// <summary>
    /// 注册信息更新工作流配置
    /// </summary>
    public static class RegistrationInfoUpdateWorkflowConfig
    {
        private static System.Timers.Timer _timer;

        /// <summary>
        /// 注册工作流
        /// </summary>
        public static void RegisterWorkflow(IServiceCollection services)
        {
            // 注册工作流和步骤
            services.AddTransient<RegistrationInfoUpdateWorkflow>();
            services.AddTransient<RegistrationInfoUpdateStep>();
        }

        /// <summary>
        /// 启动注册信息更新工作流
        /// </summary>
        public static async Task<bool> ScheduleRegistrationInfoUpdate(IWorkflowHost host)
        {
            try
            {
                // 注册工作流
                host.RegisterWorkflow<RegistrationInfoUpdateWorkflow>();

                // 计算下次执行时间（每日晚上11点）
                var nextRunTime = DateTime.Today.AddHours(23);
                if (nextRunTime <= DateTime.Now)
                {
                    nextRunTime = nextRunTime.AddDays(1);
                }

                var interval = nextRunTime - DateTime.Now;

                // 首次延迟执行
                _timer = new System.Timers.Timer(interval.TotalMilliseconds);
                _timer.Elapsed += async (sender, e) =>
                {
                    frmMainNew.Instance?.PrintInfoLog($"开始执行注册信息更新任务: {System.DateTime.Now.ToString()}");
                    try
                    {
                        // 执行工作流
                        await host.StartWorkflow("RegistrationInfoUpdateWorkflow");

                        // 计算下次执行时间
                        var nextExecutionTime = DateTime.Today.AddHours(23).AddDays(1);
                        var nextInterval = nextExecutionTime - DateTime.Now;
                        _timer.Interval = nextInterval.TotalMilliseconds;
                    }
                    catch (Exception ex)
                    {
                        frmMainNew.Instance?.PrintInfoLog($"注册信息更新任务执行失败: {ex.Message}");
                    }
                };
                _timer.AutoReset = false;
                _timer.Start();

                frmMainNew.Instance?.PrintInfoLog($"注册信息更新工作流已启动，首次执行时间: {nextRunTime:yyyy-MM-dd HH:mm:ss}");
                return true;
            }
            catch (Exception ex)
            {
                frmMainNew.Instance?.PrintInfoLog($"启动注册信息更新工作流失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 停止工作流
        /// </summary>
        public static void StopWorkflow()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
        }
    }
}
