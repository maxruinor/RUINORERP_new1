using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Workflow.WFReminder
{
    public class ReminderBizData
    {
        /// <summary>
        /// 接收人ID
        /// </summary>
        public long RecipientID { get; set; }

        public string BizKey { get; set; }
        public int BizType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 直到停止（可能延期,后面根据规则来）
        /// </summary>
        public bool StopRemind { get; set; }=false;

        /// <summary>
        /// 提醒间隔
        /// </summary>
        public int RemindInterval { get; set; }

        /// <summary>
        /// 提醒次数
        /// </summary>
        public int RemindTimes { get; set; }

        /// <summary>
        /// 是一个枚举，应对如何处理提醒
        /// </summary>
        public int ProcessRemind { get; set; }
        public int RemindCount { get; set; }
        public string WorkflowId { get; set; }
        public string WorkflowName { get; set; }

    }
}
