using RUINORERP.Server.Workflow.WFApproval;
using RUINORERP.Server.Workflow.WFApproval.Steps;
using RUINORERP.WF.WFApproval.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace RUINORERP.WF.WFApproval
{
        public class BillApprovalWorkflow : IWorkflow<ApprovalWFData>
        {
            public string Id => nameof(BillApprovalWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<ApprovalWFData> builder)
            {
                builder
//重点：initialStep 发邮件的内容是从外部输入的，因此这里的Input方法就是说明step是怎么接收输入参数的，Output表示输出把这个step执行的结果输出到外部
//每一个Step就相当于是一个独立的执行单元，参数和结果的输入 / 输出,全靠input / output两个方法
                       .StartWith<InitialStep>()
                            .Input(step => step.To, data => data.Approver)
                            .Input(step => step.DocumentId, data => data.BillId)
                            .Output(data => data.WorkflowId, step => step.WorkId)
                     //UserTask / WithOption 这是 HumanWorkflow 扩展的方法用户接收外部事件，这里.就是人工提交审批结果
                     .UserTask("Do you approve", data => data.Approver)
                         .WithOption("Approved", "I approve").Do(then => then
                         //ApprovedStep 发邮件通知申请人
                             .StartWith<ApprovedStep>()
                             .Input(step => step.DocumentName, data => data.DocumentName)
                             .Input(step => step.To, data => data.Applicant)
                         )//RejectedStep / CancelStep 功能和ApprovedStep 一致
                         .WithOption("Rejected", "I do not approve").Do(then => then
                             .StartWith<RejectedStep>()
                             .Input(step => step.DocumentName, data => data.DocumentName)
                             .Input(step => step.To, data => data.Applicant)
                         )
                         .WithEscalation(x => TimeSpan.FromMinutes(1), x => x.Applicant, action => action
                             .StartWith<CancelStep>()
                             .Input(step => step.To, data => data.Applicant)
                             .Input(step => step.DocumentName, data => data.DocumentName)
                             )

                     .Then(context => Console.WriteLine("end"));
            }
        }

    } 

