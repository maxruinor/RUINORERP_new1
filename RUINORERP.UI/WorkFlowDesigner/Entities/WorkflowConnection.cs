using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RUINORERP.UI.WorkFlowDesigner.Entities
{
    /// <summary>
    /// 工作流连接实体类
    /// 表示两个步骤节点之间的连接关系
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkflowConnection
    {
        private string _id;
        private string _fromStepId;
        private string _toStepId;
        private string _condition; // 连接条件
        private int _order;

        public WorkflowConnection()
        {
            _id = Guid.NewGuid().ToString();
        }

        [JsonProperty("Id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [JsonProperty("FromStepId")]
        public string FromStepId
        {
            get { return _fromStepId; }
            set { _fromStepId = value; }
        }

        [JsonProperty("ToStepId")]
        public string ToStepId
        {
            get { return _toStepId; }
            set { _toStepId = value; }
        }

        [JsonProperty("Condition")]
        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        [JsonProperty("Order")]
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }
    }
}