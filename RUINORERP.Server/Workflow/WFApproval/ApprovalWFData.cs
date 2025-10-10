using RUINORERP.Global;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RUINORERP.Server.Workflow.WFApproval
{
    /// <summary>
    /// 审批数据 用于工作流数据流转
    /// </summary>
    public class ApprovalWFData
    {
        public string WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public ApprovalEntity approvalEntity { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
     
    }
}
