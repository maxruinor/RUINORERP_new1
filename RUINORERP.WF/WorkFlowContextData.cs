using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF
{


    /// <summary>
    /// 所有流程的开始节点，所有放到了最上层，具体的节点条件等放到对应业务文件夹下
    /// 开始节点就是一个流程数据，并且是条件全局变量，由他传入经过整个流程。
    /// </summary>
    [Serializable]
    [Description("开始")]
    [JsonObject(MemberSerialization.OptIn)]
    public class WorkFlowContextData
    {

        public WorkFlowContextData()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            var assemblyTitle = currentAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            string title = assemblyTitle.Length > 0 ? ((AssemblyTitleAttribute)assemblyTitle[0]).Title : null;
            DataType = $"{this.GetType().FullName},{title}";
            Description="流程开始这里是流程全局变量";
            Name = "xxx";
        
        }
        public string mId = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 流程步骤ID
        /// </summary>
        [JsonProperty("Id", NullValueHandling = NullValueHandling.Include)]
        public string Id
        {
            get { return mId; }
            set { mId = value; }
        }


        private string mVersion = "1";
        /// <summary>
        /// 流程版本
        /// </summary>
        [JsonProperty("Version")]
        public string Version
        {
            get { return mVersion; }
            set { mVersion = value; }
        }
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

        private string mDescription;
        /// <summary>
        /// 流程描述
        /// </summary>
        [JsonProperty("Description")]
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }

        private string mDataType;
        /// <summary>
        /// 流程参数的类型
        /// </summary>
        [JsonProperty("DataType")]
        public string DataType
        {
            get { return mDataType; }
            set { mDataType = value; }
        }

        
        /// <summary>
        /// 流程步骤
        /// </summary>
        [JsonProperty("Steps")]
        public List<BaseStepBody> Steps { get; set; } = new List<BaseStepBody>();

        public string MessageData { get; set; }

    }
}
