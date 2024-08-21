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

    public class ApprovalWorkflow : IWorkflow<ApprovalWFData>
    {
        private readonly ILogger<ApprovalWorkflow> _logger;
        public ApprovalWorkflow(ILogger<ApprovalWorkflow> logger)
        {
            _logger = logger;
        }
        public string Id => "单据审核";
        public int Version => 1;

        public void Build(IWorkflowBuilder<ApprovalWFData> builder)
        {
            builder
             .StartWith(context =>
             {
                 Console.WriteLine("开始启动流程...");
                 _logger.LogInformation("开始启动流程...");
                 return ExecutionResult.Next();
             })
            .Then<NotifyApprovedBy>().Name("通知审批人")
            .Input(step => step.BillID, data => data.approvalEntity.BillID.ToString())
            .WaitFor("审核结果", (data, context) => context.Workflow.Id, data => DateTime.Now)
              .Output(data => data.approvalEntity, step => step.EventData)//approvalEntity 是ui上的数据，通过事件推送传过来
                                                                          //  .When(t => t.approvalEntity.ApprovalResults == true).Do(d => d.Then<ApprovedStep>().Name("同意"))
                                                                          // .When(t => t.approvalEntity.ApprovalResults == false).Do(d => d.Then<CancelStep>().Name("驳回"))
              .If(t => t.approvalEntity.ApprovalResults == true).Do(d => d.Then<ApprovedStep>().Name("同意"))
              .If(t => t.approvalEntity.ApprovalResults == false).Do(d => d.Then<CancelStep>().Name("驳回"))
              //.If(data => data.审核状态 == "完成").Do(context => _logger.LogInformation("!!完成!!!"))

              .Then<NotifyApprovedCompleted>()
              .Then(context =>
               {
                   Console.WriteLine("Workflow complete");
                   _logger.LogInformation($"Workflow complete");
                  // frmMain.Instance.workflowlist.Remove(session as SessionforBiz);

                   KeyValuePair<string, string> kv = frmMain.Instance.workflowlist.Where(k => k.Value == context.Workflow.Id).FirstOrDefault();
                   if (kv.Key != null)
                   {
                       //frmMain.Instance.workflowlist.Remove<string,string>()
                   }

                   // frmMain.Instance.workflowlist.TryRemove(context.Workflow.Id,)
                   return ExecutionResult.Next();
               })
                ;
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
            .Then<WF.WFApproval.Submit>();
            // .Input(step => step.subtext, data => data.BillId);
            // .Then<tb_Stocktake>()
            //    .Input(step => step.ApprovalStatus, data => data.ApprovalStatus);
        }
    }


}
