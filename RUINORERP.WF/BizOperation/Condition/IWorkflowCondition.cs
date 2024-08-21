using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Persistence.EntityFramework.Services;

namespace RUINORERP.WF.BizOperation.Condition
{
    public interface IWorkflowCondition
    {
        bool Evaluate(WorkflowContext context);
    }

    public interface ICondition
    {
        bool IsSatisfied(Dictionary<string, object> data);
    }

}
