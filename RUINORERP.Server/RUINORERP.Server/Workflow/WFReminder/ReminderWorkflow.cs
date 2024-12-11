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
    public class ReminderWorkflow : IWorkflow<ReminderData>
    {
        private readonly ILogger<ReminderWorkflow> _logger;

        public ReminderWorkflow(ILogger<ReminderWorkflow> logger)
        {
            _logger = logger;
        }

        public string Id => "ReminderWorkflow";

        public int Version => 1;

        //循环任务：比如在工作流步骤中设置一个延迟5秒进行的循环任务，知道Counter > 5才结束
        public void Build(IWorkflowBuilder<ReminderData> builder)
        {
            builder
            .StartWith<ReminderStart>(
                    context =>
                    {
                        Console.WriteLine("Hello");
                        MessageBox.Show("开始提示前先提示一下");
                    }
                )
            .Input(step => step.Description, data => data.BizKey)

              //   .Input(step => step.Password, data => data.Password)
              //   .Output(data => data.UserId, step => step.UserId)

              .Recur(data => TimeSpan.FromSeconds(5), data => data.RemindCount == 5)
                
                  .Do(recur => recur
                 
                  .StartWith<ReminderTask>
                  (
                  context =>
                  {
                      Console.WriteLine("执行提醒");
                      MessageBox.Show("执行提醒" + System.DateTime.Now);
                   
                  }
                  ).Input(step => step.RecipientID, data => data.RecipientID)
                  .Output(step => step.RemindCount, data => data.RecipientID)

              )
              .Then(
                context =>
            {
                Console.WriteLine("结束");
                MessageBox.Show("结束" + System.DateTime.Now);
                return ExecutionResult.Next();
            }

                            );
        }
    }
}
