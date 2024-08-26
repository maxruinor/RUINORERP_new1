using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    /// <summary>
    /// 审核信息实体  将来如果有些字段是辅助的，则用特性标记处理
    /// </summary>
    public class ApprovalEntity
    {
        public long BillID { get; set; }

        /// <summary>
        /// 冗余枚举值
        /// </summary>
        public string bizName { get; set; }

        public BizType bizType { get; set; }
        public string BillNo { get; set; }

        /// <summary>
        /// 审批意见Comments批注
        /// </summary>
        public string ApprovalOpinions { get; set; }

        /// <summary>
        /// 用于结案情况说明
        /// </summary>
        public string CloseCaseOpinions { get; set; }

        public string To { get; set; }
        public string From { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public long Approver_by { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime Approver_at
        {
            get { return System.DateTime.Now; }
        }


        private int _ApprovalStatus = (int)RUINORERP.Global.ApprovalStatus.未审核;

        public int ApprovalStatus { get => _ApprovalStatus; set => _ApprovalStatus = value; }

 

        private bool _ApprovalResults = false;

        public bool ApprovalResults { get => _ApprovalResults; set => _ApprovalResults = value; }


        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            // 检查属性值并添加到StringBuilder中
            AppendPropertyIfNotEmpty(stringBuilder, "BillID", BillID.ToString());
            AppendPropertyIfNotEmpty(stringBuilder, "bizName", bizName);
            AppendPropertyIfNotEmpty(stringBuilder, "bizType", bizType.ToString());
            AppendPropertyIfNotEmpty(stringBuilder, "BillNo", BillNo);
            AppendPropertyIfNotEmpty(stringBuilder, "ApprovalOpinions", ApprovalOpinions);
            AppendPropertyIfNotEmpty(stringBuilder, "CloseCaseOpinions", CloseCaseOpinions);
            AppendPropertyIfNotEmpty(stringBuilder, "To", To);
            AppendPropertyIfNotEmpty(stringBuilder, "From", From);
            AppendPropertyIfNotEmpty(stringBuilder, "Approver_by", Approver_by.ToString());
            AppendPropertyIfNotEmpty(stringBuilder, "Approver_at", Approver_at.ToString());
            AppendPropertyIfNotEmpty(stringBuilder, "ApprovalStatus", ApprovalStatus.ToString());
            AppendPropertyIfNotEmpty(stringBuilder, "ApprovalResults", ApprovalResults.ToString());

            return stringBuilder.ToString();
        }

        private void AppendPropertyIfNotEmpty<T>(StringBuilder sb, string propertyName, T value)
        {
            if (!string.IsNullOrEmpty(propertyName) && value != null)
            {
                sb.AppendLine($"{propertyName}: {value}");
            }
        }


    }
}
