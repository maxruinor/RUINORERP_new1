using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RUINORERP.WF.WFApproval
{
   public  class ApprovalWFData
    {
        public string WorkflowId { get; set; }
        public string WorkflowName { get; set; }
      
        public string Status { get; set; }
        public string DocumentName { get; set; }
        public long BillId { get; set; }
        public string Url { get; set; }
        public string Applicant { get; set; }
        public string Approver { get; set; }
        public string Outcome { get; set; }
        public string Comments { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public DateTime RequestDateTime { get; set; }
    }
}
