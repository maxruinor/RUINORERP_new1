using Newtonsoft.Json;
using RUINORERP.WF.BizOperation.Condition;
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
    public class DecisionStep : BaseStepBody
    {
        public DecisionStep()
        {
            Id = Guid.NewGuid().ToString("N");
            Name = "判断";
        }
        public string RequestData { get; set; }
        public bool Approved { get; private set; }

        public List<UserSelector> UserSelector { get; set; }

        /// <summary>
        /// 判断节点拥有的条件
        /// </summary>
        public List<IWorkflowCondition> WorkflowConditions { get; set; }



        public override ExecutionResult Run(IStepExecutionContext context)
        {
            // 模拟申请过程，这里可以添加实际的业务逻辑
            Approved = true; // 假设申请被批准
            MessageBox.Show("判断成功!" + context.Step.Name);
            MessageBox.Show("判断input:"+RequestData);
            RequestData += "已经经过了判断步骤的处理。";
            
            // RequestData  是判断步骤中的一个属性   如果流程开始的 DataType 为空才可以这样设置。  "Inputs": {"RequestData": "\"您的流程已完成\""},
            //如果传入的指定类型作为全局变量可以 data.是固定的。Name是全局变量中的一个属性，RequestData是判断步骤中的一个属性

            /*
             {
  "Id": "19f1c6c7d77d448fc8de616f76140c4d1",
  "Version": "1",
  "Name": "xxx",
  "Description": "流程开始这里是流程全局变量",
  "DataType": "RUINORERP.WF.WorkFlowContextData,RUINORERP.WF",
  "Steps": [
    {
      "StepType": "RUINORERP.WF.BizOperation.Steps.DecisionStep,RUINORERP.WF",
      "Id": "296d0e1e14a7440aad7f6e31d96808e3",
      "Name": "判断",
      "Inputs": {"RequestData":"data.Name"},
      "Outputs": {},
      "SelectNextStep": {}
    }
  ]
}
             */
            //  "Outputs": {"MessageData": "RequestData"},
            //根据条件执行下一步，比方金额小于500自己审核，大于500要上级审核


            if (Approved)
            {
                return ExecutionResult.Next();
            }

            return ExecutionResult.Next();
        }
    }
}
