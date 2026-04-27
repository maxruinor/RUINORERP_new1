using RUINORERP.Global;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RUINORERP.Server.Workflow.WFApproval
{
    /// <summary>
    /// 审批工作流数据 - 用于工作流引擎数据传递
    /// </summary>
    public class ApprovalWFData
    {
        public string WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        
        /// <summary>
        /// 审批实体（必填）
        /// </summary>
        [Required(ErrorMessage = "审批实体不能为空")]
        public ApprovalEntity approvalEntity { get; set; }
        
        public DateTime? ApprovedDateTime { get; set; }

        /// <summary>
        /// 验证数据有效性
        /// </summary>
        public bool IsValid(out List<string> errors)
        {
            errors = new List<string>();
            
            if (approvalEntity == null)
            {
                errors.Add("审批实体不能为空");
                return false;
            }
            
            if (approvalEntity.BillID <= 0)
            {
                errors.Add("单据ID必须大于0");
            }
            
            return errors.Count == 0;
        }
    }
}
