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

namespace RUINORERP.Server.Workflow.WFApproval
{
    /// <summary>
    /// 定时任务的工作流
    /// </summary>
    public class ScheduledlWorkflow : IWorkflow<ApprovalWFData>
    {
        private readonly ILogger<ScheduledlWorkflow> _logger;
        public ScheduledlWorkflow(ILogger<ScheduledlWorkflow> logger)
        {
            _logger = logger;
        }
        public string Id => "定时任务的工作流";
        public int Version => 1;

        //计划任务：比如在工作流步骤中设置一个延迟5分钟执行的计划任务
        public void Build(IWorkflowBuilder<ApprovalWFData> builder)
        {
            builder
             .StartWith(context => Console.WriteLine("Hello")).Schedule(data => TimeSpan.FromSeconds(5)).Do(schedule => schedule
            .StartWith(
                 context => Console.WriteLine("Doing scheduled tasks"))
            )
             .Then(
                context => Console.WriteLine("Doing normal tasks")
             );
        }
    }

    //（2）循环任务：比如在工作流步骤中设置一个延迟5分钟进行的循环任务，知道Counter > 5才结束
    //    builder
    //.StartWith(context=> Console.WriteLine("Hello")).Recur(data=> TimeSpan.FromSeconds(5),data=> data.Counter >5).Do(recur=> recur
    //    .StartWith(context=> Console.WriteLine("Doing recurring task"))).Then(context=> Console.WriteLine("Carry on"));



}
