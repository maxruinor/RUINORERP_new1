using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.WF.BizOperation.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    public class WFPlanProcess : IWorkflow<SOProcessData>, IWorkflowMarker
    {
        public string Id => "WFPlanProcess";
        public int Version => 1;


        //销售订单流程：用户提交过来开始流程，
        //判断：如果金额》500，指定人审核，小于=500自己审核。
        //审核通过时提醒仓库，结束流程
        //审核不通过时提醒用户。结束流程。
        public void Build(IWorkflowBuilder<SOProcessData> builder)
        {
            builder
            .StartWith<ApproveSO>()
            .WaitFor("ApproveResultEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)//等待事件传入处理的结果后再下一步
            .Output(data => data.ApprovalOpinions, step => (step.EventData as ApproveResultEventData).ApprovalOpinions)
            .Output(data => data.ApprovalStatus, step => (step.EventData as ApproveResultEventData).ApprovalStatus)
            .CancelCondition(data => data.ApprovalStatus == 0)//如果审核不通过，取消流程？
            .Then<NotificationSO>()
                    .Input(step => step.ApprovalStatus, data => data.ApprovalStatus)
                    .Input(step => step.ApprovalOpinions, data => data.ApprovalOpinions)
            .Then(context => MessageBox.Show("end"))
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


    public class ApproveResultEventData
    {
        public int ApprovalStatus { get; set; }

        public string ApprovalOpinions { get; set; }
    }

    /// <summary>
    /// 订单处理数据
    /// </summary>
    public class SOProcessData
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderID { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public long EmploryeeID { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int ApprovalStatus { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string ApprovalOpinions { get; set; }

    }

    /// <summary>
    /// 审核
    /// </summary>
    public class ApproveSO : StepBody
    {
        private ILogger logger;

        public ApproveSO(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ApproveSO>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Workflow, Goodbye");
            logger.LogInformation("Goodbye workflow");

            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 取消
    /// </summary>
    public class CancelSO : StepBody
    {
        private ILogger logger;

        public CancelSO(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<CancelSO>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Workflow, Goodbye");
            logger.LogInformation("Goodbye workflow");

            return ExecutionResult.Next();
        }
    }

    
    public class ShippingNotice : StepBody
    {
        private ILogger logger;

        public ShippingNotice(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ShippingNotice>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //通知发货


            return ExecutionResult.Next();
        }
    }


    public class PlanNotice : StepBody
    {
        private ILogger logger;

        public PlanNotice(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<PlanNotice>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //通知发货


            return ExecutionResult.Next();
        }
    }


    public class ShippingNotice : StepBody
    {
        private ILogger logger;

        public ShippingNotice(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ShippingNotice>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //通知发货


            return ExecutionResult.Next();
        }
    }


    public class ShippingNotice : StepBody
    {
        private ILogger logger;

        public ShippingNotice(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ShippingNotice>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //通知发货


            return ExecutionResult.Next();
        }
    }



}
