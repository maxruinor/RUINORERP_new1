using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RUINORERP.UI.WorkFlowDesigner.Entities
{
    /// <summary>
    /// 工作流定义实体类
    /// 用于持久化存储图形化设计的流程配置
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkflowDefinition
    {
        private string _id;
        private string _name;
        private string _description;
        private int _version;
        private List<WorkflowStep> _steps;
        private List<WorkflowConnection> _connections;
        private DateTime _createTime;
        private DateTime _updateTime;
        private bool _isActive;

        public WorkflowDefinition()
        {
            _id = Guid.NewGuid().ToString();
            _steps = new List<WorkflowStep>();
            _connections = new List<WorkflowConnection>();
            _createTime = DateTime.Now;
            _updateTime = DateTime.Now;
            _isActive = true;
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

        [JsonProperty("Description")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [JsonProperty("Version")]
        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        [JsonProperty("Steps")]
        public List<WorkflowStep> Steps
        {
            get { return _steps; }
            set { _steps = value; }
        }

        [JsonProperty("Connections")]
        public List<WorkflowConnection> Connections
        {
            get { return _connections; }
            set { _connections = value; }
        }

        [JsonProperty("CreateTime")]
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        [JsonProperty("UpdateTime")]
        public DateTime UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        [JsonProperty("IsActive")]
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
    }
}