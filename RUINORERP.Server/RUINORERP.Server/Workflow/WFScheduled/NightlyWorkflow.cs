using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Server.Workflow.WFReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Services;

namespace RUINORERP.Server.Workflow.WFScheduled
{

    /// <summary>
    /// 定义一个工作流每天晚上2点运行
    /// 库存提醒，客户状态 ，客户的金额这些都可以在这里来处理？
    /// </summary>
    public class NightlyWorkflow : IWorkflow<GlobalScheduledData>
    {
        public string Id => "NightlyWorkflow";
        public int Version => 1;




        public void Build(IWorkflowBuilder<GlobalScheduledData> builder)
        {
            builder
                .StartWith(context => Console.WriteLine("任务开始时间: " + DateTime.Now))
              .Recur(data => TimeSpan.FromMinutes(3), data => System.DateTime.Now.Date.Hour > 18 && data.ExecutionFrequency > 1)
                  .Do(recur => recur
                  .StartWith<NightlyTaskStep>
                  (
                  context =>
                  {
                      Console.WriteLine("执行提醒");
                      //MessageBox.Show("执行提醒" + System.DateTime.Now);

                  }
                  )
                  )
                .Then(context => Console.WriteLine("任务结束时间: " + DateTime.Now));
        }



        public class NightlyTaskStep : StepBodyAsync
        {
            private readonly ILogger<NightlyTaskStep> logger;

            public string subtext;

            public NightlyTaskStep(ILogger<NightlyTaskStep> _logger)
            {
                logger = _logger;
            }

            public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
            {
                try
                {

                    logger.LogInformation("开始每日任务" + subtext + System.DateTime.Now);
                    // 在这里编写你的耗时任务逻辑
                    frmMain.Instance.PrintInfoLog($"开始每日任务~~~。DailyTaskStep");
                    var data = context.Workflow.Data as GlobalScheduledData;
                    data.ExecutionFrequency++;
                    // 模拟耗时任务，例如等待 5 秒
                    await Task.Delay(5000);
                    frmMain.Instance.PrintInfoLog($"结束每日任务~~~。DailyTaskStep");
                }
                catch (Exception ex)
                {
                    logger.LogError("开始每日任务出错", ex.Message);
                }

                return ExecutionResult.Next();
            }
        }


        // 这是一个模拟的长时间运行的任务
        private async Task<string> LongRunningTask(string parameter)
        {
            // 这里执行耗时的操作，例如大量计算或IO操作
            await Task.Delay(10000); // 假设的耗时操作
            return "任务完成";
        }
        //public void Build(IWorkflowBuilder<GlobalScheduledData> builder)
        //{
        //    builder
        //      .StartWith(context =>
        //      {

        //          Console.WriteLine("流程在凌晨 2 点开始执行！");
        //          return ExecutionResult.Next();
        //      });
        //}
    }


    /// <summary>
    /// 系统级的数据 会自动运行
    /// </summary>
    public class GlobalScheduledData
    {
        // 您的数据定义
        public DateTime FlagTime { get; set; }

        public int ExecutionFrequency { get; set; } = 1;

    }
}
