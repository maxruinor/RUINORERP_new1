using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.WF.BizOperation.Steps
{
    public class CloseCaseStep : BaseStepBody
    {
        public CloseCaseStep()
        {
            Id = Guid.NewGuid().ToString("N");
            Name = "结案";
        }

        /// <summary>
        /// 提交人
        /// </summary>
        public long UserId { get; set; }

        public string RequestData { get; set; }

        public bool Approved { get; private set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // 模拟申请过程，这里可以添加实际的业务逻辑
            Approved = true; // 假设申请被批准
            MessageBox.Show("结案成功!" + context.Step.Name);

            return ExecutionResult.Next();
        }
    }
}
