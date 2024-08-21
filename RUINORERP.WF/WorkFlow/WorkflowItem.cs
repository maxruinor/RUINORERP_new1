using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.WorkFlow
{
    /// <summary>
    /// 后面再细分 审核 自动任何啥的。
    /// </summary>
    public class WorkflowItem
    {
        public string WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string BillName { get; set; }
        public BizType bizType { get; set; }
        public long BillId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
