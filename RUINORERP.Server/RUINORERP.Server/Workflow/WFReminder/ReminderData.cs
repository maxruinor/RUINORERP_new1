using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Workflow.WFReminder
{
    /// <summary>
    /// 提醒的数据：提示次数，提示时间
    /// </summary>
    public class ReminderData
    {
        /// <summary>
        /// 接收人ID
        /// </summary>
        public long RecipientID { get; set; }

        public string BizKey { get; set; }
        public int BizType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int RemindCount { get; set; }
        public string WorkflowId { get; set; }
        public string WorkflowName { get; set; }

    }
}
