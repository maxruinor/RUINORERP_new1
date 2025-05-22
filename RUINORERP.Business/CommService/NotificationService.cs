using RUINORERP.Global;
using RUINORERP.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Services;

namespace RUINORERP.Business.CommService
{

    // 添加自定义状态处理器
    public interface IStatusHandler
    {
        bool CanHandle(DataStatus status);
        void Handle(IStatusMachine machine);
    }

    public class ProductionStatusHandler : IStatusHandler
    {
        public bool CanHandle(DataStatus status)
        {
            throw new NotImplementedException();
        }

        public void Handle(IStatusMachine machine)
        {
            throw new NotImplementedException();
        }
    }

    // 注册自定义处理器
    //services.AddSingleton<IStatusHandler, ProductionStatusHandler>();


    /// <summary>
    ///多级审批工作流：
    /// </summary>
    //public class MultiLevelApprovalWorkflow : IWorkflow<WorkflowData>
    //{
    //    public void Build(IWorkflowBuilder<WorkflowData> builder)
    //    {
    //        builder
    //            .StartWith<DepartmentApproveStep>()
    //            .WaitFor("DeptApprove", data => data.DocumentId)
    //            .If(data => data.Result == true)
    //                .Do(then => then
    //                    .StartWith<FinancialApproveStep>()
    //                    .WaitFor("FinanceApprove", data => data.DocumentId))
    //            .Then<FinalApproveStep>();
    //    }
    //}

    //智能提醒集成
    public class NotificationStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //var users = _roleService.GetUsers("WarehouseManager");
            //_notificationService.Send(users, "有新订单需要备货");
            return ExecutionResult.Next();
        }
    }

    // WorkflowCore集成示例
    //public class ApprovalWorkflow : IWorkflow<WorkflowData>
    //{
    //    public void Build(IWorkflowBuilder<WorkflowData> builder)
    //    {
    //        builder
    //            .UseDefaultErrorBehavior(WorkflowErrorHandling.Retry)
    //            .StartWith<SubmitStep>()
    //                .Input(step => step.DocumentId, data => data.DocumentId)
    //            .WaitFor("ApproveEvent", data => data.DocumentId)
    //                .Output(data => data.ApprovalResult, step => step.Result)
    //            .Then<ApproveStep>()
    //            .WaitFor("CloseEvent", data => data.DocumentId)
    //            .Then<CloseStep>();
    //    }
    //}

    // 使用示例
//    var workflowHost = new WorkflowHost();
//    workflowHost.RegisterWorkflow<ApprovalWorkflow>();
//workflowHost.Start();

//var statusMachine = new StatusMachine(..., workflowHost);
    

//using var binder = new UIStateBinder(statusMachine, mainForm, workflowHost)
//    {
//        // 手动注册未标记的控件
//        { btnSubmit, MenuItemEnums.提交 },
//    { btnApprove, MenuItemEnums.审核 }
//    };


}
