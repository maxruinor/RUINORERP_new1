using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using RUINORERP.Model;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Workflow.WFApproval;
using RUINORERP.Server.Workflow.WFApproval.Steps;
using RUINORERP.Global;
using System.Windows.Forms;

namespace RUINORERP.Server.Workflow.WFReminder
{

    /// <summary>
    /// 提示功能：比方跟踪计划，工作流是从服务器的一个缓存集合中去找到计划的启动时间与当前时间比较。按配置规则或默认提前1天开始提醒（启动工作流）。间隔多久提示一次。直接收到通知。（已经收到提醒或稍后提醒，或延长时间)
    /// 
    /// </summary>
    public class ReminderWorkflow : IWorkflow<ReminderBizData>
    {
        private readonly ILogger<ReminderWorkflow> _logger;

        public ReminderWorkflow(ILogger<ReminderWorkflow> logger)
        {
            _logger = logger;
        }

        public string Id => "ReminderWorkflow";

        public int Version => 1;

        public DateTime StartTime { get; set; } = System.DateTime.Now;


        //循环任务：比如在工作流步骤中设置一个延迟5秒进行的循环任务，知道Counter > 5才结束
        public void Build(IWorkflowBuilder<ReminderBizData> builder)
        {
            builder
            .StartWith<ReminderStart>(
                    context =>
                    {
                        Console.WriteLine("Hello");
                        // MessageBox.Show("开始提示前先提示一下");
                    }
                )
            .Input(step => step.Description, data => data.BizKey)

                //   .Input(step => step.Password, data => data.Password)
                //   .Output(data => data.UserId, step => step.UserId)

                .Recur(data => TimeSpan.FromSeconds(5), data => data.StopRemind == false)

                  .Do(recur => recur

                  .StartWith<ReminderTask>
                  (
                  context =>
                  {
                      Console.WriteLine("执行提醒");
                      // MessageBox.Show("执行提醒" + System.DateTime.Now);

                  }
                  ).Input(step => step.RecipientName, data => data.Receiver)
                  .Output(step => step.StopRemind, data => data.StopRemind)
                  // .WaitFor("提醒结果", (data, context) => context.Workflow.Id, data => DateTime.Now)
                  //如果取消提醒停止
                  //.If(t => t.StopRemind == true).Do(d => d.Then<ApprovedStep>().Name("同意"))
                  //.If(t => t.StopRemind == false).Do(d => d.Then<CancelStep>().Name("驳回"))
                  .If(data => data.StopRemind == true).Do(context => _logger.LogInformation("提醒!!完成!!!"))
              )
              .Then(
                context =>
            {
                if (frmMain.Instance.IsDebug)
                {
                    frmMain.Instance.PrintInfoLog($"结束提醒工作流。" + System.DateTime.Now);
                }
                return ExecutionResult.Next();
            }
          );
        }
    }
}
