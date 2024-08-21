using RUINORERP.WF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.BizOperation.Condition
{
    public class ConditionConfig
    {
        // 条件类型，例如："Amount", "Submitter", "Approver"
        public WorkflowConditionType conditionType   { get; set; }

        // 金额阈值，仅当条件类型为"Amount"时使用
        public decimal AmountThreshold { get; set; }

        // 提交人，仅当条件类型为"Submitter"时使用
        public string Submitter { get; set; }

        // 审核人，仅当条件类型为"Approver"时使用
        public string Approver { get; set; }

        // 可以添加其他需要的属性或方法
    }
}
