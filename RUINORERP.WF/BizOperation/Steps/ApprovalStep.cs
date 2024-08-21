using Newtonsoft.Json;
using RUINORERP.WF.WorkFlow;
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

    [JsonObject(MemberSerialization.OptIn)]
    public class ApprovalStep : BaseStepBody
    {
        public ApprovalStep()
        {
            Id = Guid.NewGuid().ToString("N");
            Name = "审核";
        }
        public string RequestData { get; set; }
        public bool Approved { get; private set; }

        public List<UserSelector> UserSelector { get; set; }

        public string  Message {get;set;}
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // 模拟申请过程，这里可以添加实际的业务逻辑
            Approved = true; // 假设申请被批准
            MessageBox.Show("审核成功!"+ context.Step.Name);

            //根据条件执行下一步，比方金额小于500自己审核，大于500要上级审核
            MessageBox.Show("审核时收到外部数据：" + Message);

            if (Approved)
            {
                return ExecutionResult.Next();
            }

            return ExecutionResult.Next();
        }
    }
}
