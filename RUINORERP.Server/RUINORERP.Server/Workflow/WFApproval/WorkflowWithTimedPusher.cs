using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using Microsoft.Extensions.Logging;
using WorkflowCore.Models;

namespace RUINORERP.WF.WFApproval
{
    /// <summary>
    /// 定时推送工作流
    /// </summary>
    public class WorkflowWithTimedPusher : IWorkflow<PushData>
    {
        private readonly ILogger<WorkflowWithTimedPusher> _logger;
        public WorkflowWithTimedPusher(ILogger<WorkflowWithTimedPusher> logger)
        {
            _logger = logger;
        }

        public string Id => nameof(WorkflowWithTimedPusher);

        public int Version => 1;

        public void Build(IWorkflowBuilder<PushData> builder)
        {
            builder
                 .StartWith(context =>
                 {
                     Console.WriteLine("Starting workflow...");
                     return ExecutionResult.Next();
                 })
                 .WaitFor("审核状态", (data, context) => context.Workflow.Id, data => DateTime.Now)//WaitFor第一个参数是 事件名称，第二个是事件Key，事件Key可以任意取名，在这里用的是工作流的Id，data=>"1"这样也是可以的。
                 .Output(data => data.审核状态, step => step.EventData)//waitfor是一个事件，他可以输入一个参数给下节点
                 .If(data => data.审核状态 == "完成").Do(context => _logger.LogInformation("!!完成!!!"))
                 .If(data => data.Counter < 3).Do(then => then
                    .StartWith<PrintMessage>()
                        .Input(step => step.Message, data => "Value is less than 3")
                )
                .If(data => data.Counter < 5).Do(then => then
                    .StartWith<PrintMessage>()
                        .Input(step => step.Message, data => "Value is less than 5")
                )
                .Then<SayGoodbye>();
        }
    }


    /// <summary>
    /// 推送数据
    /// </summary>
    public class PushData
    {
        public string WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public string Status { get; set; }
        public string Content { get; set; }
        public string Comments { get; set; }

    }
}
