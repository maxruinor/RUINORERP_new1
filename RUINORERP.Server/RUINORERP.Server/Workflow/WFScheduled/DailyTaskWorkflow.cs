using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
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

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
            .StartWith(context => frmMain.Instance.PrintInfoLog("任务开始时间: " + DateTime.Now))

              .Schedule(data => DateTime.Today.AddMinutes(60) - DateTime.Now)
              .Do(x => x.StartWith<DailyTaskStep>(context =>


              //frmMain.Instance.PrintInfoLog("定时任务执行: " + DateTime.Now)

              //MessageBox.Show("定时任务执行:  " + DateTime.Now)
              // 模拟耗时任务，例如等待 1 秒
              Task.Delay(1000)
               
              )




              )
              // 计算从现在到今天凌晨1点的时间间隔
              // .Then<DailyTaskStep>()  
              .Then(context => frmMain.Instance.PrintInfoLog("任务完成" + DateTime.Now));
        }
    }


    public class DailyTaskStep : StepBodyAsync
    {
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            try
            {
                // 在这里编写你的耗时任务逻辑
                frmMain.Instance.PrintInfoLog($"开始每日任务~~~。");
                // 模拟耗时任务，例如等待 5 秒
                await Task.Delay(5000);
                frmMain.Instance.PrintInfoLog($"结束每日任务~~~。");
            }
            catch (Exception ex)
            {

                
            }
           
            return ExecutionResult.Next();
        }
    }
}



