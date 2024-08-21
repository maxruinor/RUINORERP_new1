using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.WF.WorkFlow.Steps
{
    public class ApprovalStep : StepBody
    {
        public string RequestData { get; set; }
        public bool Approved { get; private set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // 模拟申请过程，这里可以添加实际的业务逻辑
            Approved = true; // 假设申请被批准
            MessageBox.Show("审核成功!");

            //根据条件执行下一步，比方金额小于500自己审核，大于500要上级审核


            if (Approved)
            {
                return ExecutionResult.Next();
            }

            return ExecutionResult.Next();
        }
    }
}
