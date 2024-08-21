using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Persistence.EntityFramework.Services;

namespace RUINORERP.WF.BizOperation.Condition
{
    public class AmountCondition : IWorkflowCondition
    {
        private readonly decimal _threshold;

        public AmountCondition(decimal threshold)
        {
            _threshold = threshold;
        }

        public bool Evaluate(WorkflowContext context)
        {
            decimal amount = context.OrderDetails.Amount; // 假设OrderDetails是订单详情的属性
            return amount > _threshold;
        }
    }

    public class SubmitterCondition : IWorkflowCondition
    {
        private readonly string _expectedSubmitter;

        public SubmitterCondition(string expectedSubmitter)
        {
            _expectedSubmitter = expectedSubmitter;
        }

        public bool Evaluate(WorkflowContext context)
        {
            return context.OrderDetails.Submitter == _expectedSubmitter;
        }
    }

    public class ApproverCondition : IWorkflowCondition
    {
        private readonly string _expectedApprover;

        public ApproverCondition(string expectedApprover)
        {
            _expectedApprover = expectedApprover;
        }

        public bool Evaluate(WorkflowContext context)
        {
            return context.OrderDetails.Approver == _expectedApprover;
        }
    }
}
