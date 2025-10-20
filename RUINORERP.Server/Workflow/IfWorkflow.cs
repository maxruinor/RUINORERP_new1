using Microsoft.Extensions.Logging;
using RUINORERP.Server.Workflow.Steps;
using System;
using WorkflowCore.Interface;

namespace RUINORERP.Server.Workflow
{
    public class IfWorkflow : IWorkflow<MyData>
    {
        private readonly ILogger<IfWorkflow> _logger;
        public IfWorkflow(ILogger<IfWorkflow> logger)
        {
            // 设置日志级别为 Error
            //LoggerFilterRules rules = new LoggerFilterRules();
            //rules.Add(new LoggerFilterRule(LogLevel.Error, null, null));
            //logger.BeginScope(rules);
            _logger = logger;
        }

        public string Id => "if-sample";
        public int Version => 1;

        public void Build(IWorkflowBuilder<MyData> builder)
        {

            builder
                .StartWith<SayHello>()
                // .WaitFor("审核状态", (data, context) => context.Workflow.Id, data => DateTime.Now)//WaitFor第一个参数是 事件名称，第二个是事件Key，事件Key可以任意取名，在这里用的是工作流的Id，data=>"1"这样也是可以的。
                 //.Output(data => data.审核状态, step => step.EventData)//waitfor是一个事件，他可以输入一个参数给下节点
                 //.If(data => data.审核状态 == "完成").Do(context => _logger.LogInformation("!!完成!!!"))
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

    public class MyData
    {
        public int Counter { get; set; }
        public string 审核状态 { get; set; }
    }
}
