using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.Workflow.WFReminder;
using RUINORERP.WF.WFApproval;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;
using WorkflowCore.Services;

namespace RUINORERP.Server.Workflow.WFScheduled
{
    public class DailyTaskWorkflow : IWorkflow
    {
        public string Id => "DailyTaskWorkflow";
        public int Version => 1;


        static DateTime executionTime = DateTime.Today.AddHours(10).AddMinutes(50);

        TimeSpan NextTime = executionTime - DateTime.Now;
        public void Build(IWorkflowBuilder<object> builder)
        {

            builder
            .StartWith(context => frmMain.Instance.PrintInfoLog("任务开始时间: " + DateTime.Now))
             .Recur(data => NextTime, data => data.ToString() != "")
                  .Do(recur => recur
                  .StartWith<DailyTaskStep>(
                          context =>
                          {
                              NextTime = executionTime - DateTime.Now;
                          }
                      )
                 .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromMinutes(20))
                  ).Then(context => frmMain.Instance.PrintInfoLog("任务结束时间: " + DateTime.Now));
        }
    }


    public class DailyTaskStep : StepBodyAsync
    {
        private readonly ILogger<DailyTaskStep> logger;

        public string subtext;


 
        public DailyTaskStep(ILogger<DailyTaskStep> _logger)
        {
            logger = _logger;
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            // 设置日志级别为 Error
            //LoggerFilterRules rules = new LoggerFilterRules();
            //rules.Add(new LoggerFilterRule(LogLevel.Error, null, null));
            //logger.BeginScope(rules);

            try
            {
                
                logger.LogInformation("开始每日任务" + subtext + System.DateTime.Now);
                // 在这里编写你的耗时任务逻辑
                frmMain.Instance.PrintInfoLog($"开始每日任务~~~。DailyTaskStep");
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
}



