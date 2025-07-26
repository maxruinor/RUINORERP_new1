using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderRules
{
    /// <summary>
    /// 配置哪些业务需要谁来审核，实时（超时多少小时要提醒）
    /// </summary>
    public class DocApprovalConfig : RuleConfigBase
    {


        //辅助属性，不参与JSON序列化
        [JsonIgnore]
        public string _DocumentTypeNames
        {
            get => string.Join(",", DocumentTypes);
            set => DocumentTypes = value?.Split(',')
                                 .Select(int.Parse)
                                 .ToList() ?? new List<int>();
        }

        public List<string> TargetRoles { get; set; } = new List<string>();


        /// <summary>
        /// 参与提醒的业务类型
        /// 单据类型
        /// </summary>
        public List<int> DocumentTypes { get; set; } = new List<int>();

        /// <summary>
        /// 启用实时提醒
        /// </summary>
        public bool EnableRealtimeReminders { get; set; } = true;

        public int TimeoutMinutes { get; set; } = 30;


        public int ApprovalThreshold { get; set; } // 审批金额阈值
        public List<long> Approvers { get; set; } // 审批人
        public bool EnableDelegateApproval { get; set; } // 启用委托审批




        public bool Validate()
        {
            return DocumentTypes?.Any() == true
                && TimeoutMinutes >= 30
                 ;
        }


    }
}
