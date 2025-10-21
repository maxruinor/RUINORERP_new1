using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RUINORERP.UI.WorkFlowDesigner.Entities
{
    /// <summary>
    /// 工作流步骤实体类
    /// 表示工作流中的一个步骤节点
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkflowStep
    {
        private string _id;
        private string _name;
        private string _type;
        private string _nextStepId;
        private Dictionary<string, string> _selectNextStep;
        private List<ApprovalUser> _approvers;
        private string _stepData; // 存储步骤特定数据的JSON字符串
        private int _order;
        private string _status;

        public WorkflowStep()
        {
            _id = Guid.NewGuid().ToString();
            _approvers = new List<ApprovalUser>();
            _selectNextStep = new Dictionary<string, string>();
        }

        [JsonProperty("Id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [JsonProperty("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [JsonProperty("Type")]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [JsonProperty("NextStepId")]
        public string NextStepId
        {
            get { return _nextStepId; }
            set { _nextStepId = value; }
        }

        [JsonProperty("SelectNextStep")]
        public Dictionary<string, string> SelectNextStep
        {
            get { return _selectNextStep; }
            set { _selectNextStep = value; }
        }

        [JsonProperty("Approvers")]
        public List<ApprovalUser> Approvers
        {
            get { return _approvers; }
            set { _approvers = value; }
        }

        [JsonProperty("StepData")]
        public string StepData
        {
            get { return _stepData; }
            set { _stepData = value; }
        }

        [JsonProperty("Order")]
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        [JsonProperty("Status")]
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}