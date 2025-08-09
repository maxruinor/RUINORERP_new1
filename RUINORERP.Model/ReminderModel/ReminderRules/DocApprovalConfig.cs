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

        //什么单 经过什么动作，什么状态 到什么单  

        /// <summary>
        /// 目标角色
        /// </summary>
        public List<long> TargetRoles { get; set; } = new List<long>();


        public int FromBizType { get; set; }

        public int ToBizType { get; set; }


        /// <summary>
        /// 比较提交，还是审核，还是完结 等动作
        /// 还是增加删除修改
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 要处理的业务类型对，来源和目标
        /// </summary>
        public KeyValuePair<int, int> FromToBizType { get; set; }

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

        /// <summary>
        /// 重写验证方法，先执行基类验证，再添加子类特有验证
        /// </summary>
        /// <returns></returns>
        public override RuleValidationResult Validate()
        {
            // 先执行基类的验证逻辑
            var result = base.Validate();

            // 产品ID验证
            if (DocumentTypes == null || !DocumentTypes.Any())
            {
                result.AddError("必须选择至少提醒的单据类型");
            }

            if (TimeoutMinutes < 30)
            {
                result.AddError("必须大于等于30分钟");
            }
            return result;
        }



    }
}
