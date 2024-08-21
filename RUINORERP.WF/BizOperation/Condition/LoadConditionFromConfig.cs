using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.BizOperation.Condition
{
    /// <summary>
    /// 条件加载器
    /// </summary>
    public class WorkflowConditionLoader
    {
        public IWorkflowCondition LoadConditionFromConfig(string config)
        {
            IWorkflowCondition workflowCondition;
            // surrounding_code_snippet:
            // 假设config是JSON字符串
            var conditionConfig = JsonConvert.DeserializeObject<ConditionConfig>(config);
            switch (conditionConfig.conditionType)
            {
                case Enums.WorkflowConditionType.AmountThreshold:
                    workflowCondition = new AmountCondition(conditionConfig.AmountThreshold);
                    break;
                case Enums.WorkflowConditionType.Submitter:
                    workflowCondition = new SubmitterCondition(conditionConfig.Submitter);
                    break;
                case Enums.WorkflowConditionType.Approver:
                    workflowCondition = new ApproverCondition(conditionConfig.Approver);
                    break;
                default:
                    throw new ArgumentException("Unknown condition type");
            }
            return workflowCondition;
        }
    }

}
