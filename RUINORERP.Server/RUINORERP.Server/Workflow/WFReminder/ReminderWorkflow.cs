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
using RUINORERP.Model.TransModel;

namespace RUINORERP.Server.Workflow.WFReminder
{

    /// <summary>
    /// 提示功能：比方跟踪计划，工作流是从服务器的一个缓存集合中去找到计划的启动时间与当前时间比较。按配置规则或默认提前1天开始提醒（启动工作流）。间隔多久提示一次。直接收到通知。（已经收到提醒或稍后提醒，或延长时间)
    /// 
    /// </summary>
    public class ReminderWorkflow : IWorkflow<ServerReminderData>
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
        public void Build(IWorkflowBuilder<ServerReminderData> builder)
        {
            builder
            .StartWith<ReminderStart>(
                    context =>
                    {
                        Console.WriteLine("Hello");
                        // MessageBox.Show("开始提示前先提示一下");
                    }
                )
            .Input(step => step.Description, data => data.RemindSubject)

                //   .Input(step => step.Password, data => data.Password)
                //   .Output(data => data.UserId, step => step.UserId)
                //默认30秒提醒一次，只要到期 或人为取消提醒（取消计划）才停止
                  .Recur(data => TimeSpan.FromSeconds(60), data => data.Status == MessageStatus.Cancel || data.EndTime <= System.DateTime.Now)
                  .Do(recur => recur
                  .StartWith<ReminderTask>
                  (
                  context =>
                  {
                      Console.WriteLine("执行提醒");
                      //MessageBox.Show("执行提醒" + System.DateTime.Now);

                  }
                  )
                  .Input(step => step.BizData, data => data)//直接把整个ReminderBizData数据传到步骤中去——OK
                                                            // .Input(step => step.RemindInterval, data => data.RemindInterval)//直接把整个ReminderBizData数据传到步骤中去——OK
                                                            //.Input(step => step.RecipientName, data => data.ReceiverName)
                  .Output(step => step.Status, data => data.Status)
                  // .WaitFor("提醒结果", (data, context) => context.Workflow.Id, data => DateTime.Now)
                  //如果取消提醒停止
                  //.If(t => t.StopRemind == true).Do(d => d.Then<ApprovedStep>().Name("同意"))
                  //.If(t => t.StopRemind == false).Do(d => d.Then<CancelStep>().Name("驳回"))
                  .If(data => data.Status == MessageStatus.Cancel).Do(context => _logger.LogInformation("提醒!!完成!!!不再提醒了。"))
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
