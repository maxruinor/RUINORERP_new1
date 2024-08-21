using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.WF
{

    /// <summary>
    /// 在构造函数中赋值才会json
    /// </summary>

    [JsonObject(MemberSerialization.OptIn)]
    public class BaseStepBody : StepBody
    {

        [JsonProperty("StepType", NullValueHandling = NullValueHandling.Ignore)]
        public string StepType { get; set; } // 22行

        /// <summary>
        /// 是固定的，要与设计器中的对应，但在一个流程中不要有重复的
        /// </summary>
        [JsonProperty("Id")]
        public string Id { get; set; } // 23行



        private string mName;
        /// <summary>
        /// 流程名称
        /// </summary>
        [JsonProperty("Name")]
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }



        [JsonProperty("CancelCondition", NullValueHandling = NullValueHandling.Ignore)]
        public string CancelCondition { get; set; } // 25行

        [JsonProperty("ErrorBehavior", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorBehavior { get; set; } // 26行

        [JsonProperty("RetryInterval", NullValueHandling = NullValueHandling.Ignore)]
        public string RetryInterval { get; set; } // 27行
        public List<string> Do { get; set; } // 28行，假设是一个操作列表
        public List<string> CompensateWith { get; set; } // 29行，假设是一个补偿操作列表
        public bool Saga { get; set; } // 30行

        [JsonProperty("NextStepId", NullValueHandling = NullValueHandling.Ignore)]
        public string NextStepId { get; set; } // 31行


        [JsonProperty("Inputs", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Inputs { get; set; } // 32-33行，输入参数字典

        [JsonProperty("Outputs", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Outputs { get; set; } // 35行，输出参数字典


        [JsonProperty("SelectNextStep", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> SelectNextStep { get; set; } // 36

  
        public BaseStepBody()
        {
            Do = new List<string>();

            Id = Guid.NewGuid().ToString("N");
            Name = string.Empty;
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            var assemblyTitle = currentAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            string title = assemblyTitle.Length > 0 ? ((AssemblyTitleAttribute)assemblyTitle[0]).Title : null;

            StepType = $"{this.GetType().FullName},{title}";
            CompensateWith = new List<string>();
            Inputs = new Dictionary<string, string>();
            Outputs = new Dictionary<string, string>();
            SelectNextStep = new Dictionary<string, string>();
        }


        public override ExecutionResult Run(IStepExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
