using RUINORERP.WF.BizOperation.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.WF.BizOperation
{
    /// <summary>
    /// 销售订单处理流程（另处再实现自定义的）
    /// </summary>
    public class WFSO : IWorkflow<BizOperationData>, IWorkflowMarker
    {
        public string Id => "WFSO";
        public int Version => 3;


        //销售订单流程：用户提交过来开始流程，
        //判断：如果金额》500，指定人审核，小于=500自己审核。
        //审核通过时提醒仓库，结束流程
        //审核不通过时提醒用户。结束流程。
        public void Build(IWorkflowBuilder<BizOperationData> builder)
        {
            builder
            .StartWith<SubmitStep>()
           .Recur(data => TimeSpan.FromSeconds(5), data => data.BizID > 10)
           .Do(recur => recur
           .StartWith(context =>  MessageBox.Show("Doing recurring task"))
           .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromMinutes(2)))
            .Then(context => MessageBox.Show("Carry on"))
           ;

            /*
             * 
             *  builder
            .StartWith<SubmitStep>()
           .Recur(data => TimeSpan.FromSeconds(5), data => data.BizID > 10)
           .Do(recur => recur
           .StartWith(context => MessageBox.Show("Doing recurring task"))
           .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromMinutes(2)))
            .Then(context => MessageBox.Show("Carry on"))
           ;
            

            builder
                .StartWith(context => ExecutionResult.Next())
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
               */

        }
    }
}
