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

namespace RUINORERP.WF.WFApproval
{

    public class ApprovalWorkflow : IWorkflow<tb_Stocktake>
    {
        public string Id => "盘点001";
        public int Version => 1;

        public void Build(IWorkflowBuilder<tb_Stocktake> builder)
        {
            builder
            .StartWith(context => ExecutionResult.Next())
            //.WaitFor("MyEventST", (data, context) => context.Workflow.Id, data => DateTime.Now)
           //     .Output(data => data.approvalData, step => step.EventData)
            .Then<Submit>();
            // .Input(step => step., data => data.MyName);
            // .Then<tb_Stocktake>()
            //    .Input(step => step.ApprovalStatus, data => data.ApprovalStatus);
        }
    }


    public class ApprovalWorkflowTest : IWorkflow<ApprovalWFData>
    {
        private readonly ILogger<ApprovalWorkflowTest> logger;
        public string Id => "001";
        public int Version => 1;
        public ApprovalWorkflowTest(ILogger<ApprovalWorkflowTest> _logger)
        {
            logger = _logger;
        }
        public void Build(IWorkflowBuilder<ApprovalWFData> builder)
        {
            logger.LogDebug("==asdfasd===");
            logger.LogError("rrrrr==");
            logger.LogWarning("wwww===");
            logger.LogInformation("开始启动001wf  " + System.DateTime.Now);
            Console.WriteLine("开始启动001wf" + System.DateTime.Now);
            builder
            .StartWith(context => ExecutionResult.Next())
            //.WaitFor("MyEventST", (data, context) => context.Workflow.Id, data => DateTime.Now)
           //     .Output(data => data.DocumentName, step => step.EventData)
            .Then<Submit>()
             .Input(step => step.subtext, data => data.DocumentName);
            // .Then<tb_Stocktake>()
            //    .Input(step => step.ApprovalStatus, data => data.ApprovalStatus);
        }
    }


}
