using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.ReminderModel.ReminderResults
{
    public class DocApprovalResult : BaseReminderResult
    {
        public override ReminderBizType BusinessType => ReminderBizType.单据审批提醒;
        public override string Title => $"{DocumentType}待审批 - {DocumentNumber}";
        public override string Summary => $"状态: {DocumentStatus}，创建人: {Creator}";

        // 具体业务属性
        public string DocumentType { get; set; } // 如：采购订单、付款申请
        public string DocumentNumber { get; set; }
        public string DocumentStatus { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string ApprovalAction { get; set; } // 需要执行的动作：审批、查看等
        public string DocumentLink { get; set; } // 单据详情链接

        public override IEnumerable<DataColumn> GetDataColumns()
        {
            return new List<DataColumn>
        {
            new DataColumn("单据类型", typeof(string)),
            new DataColumn("单据编号", typeof(string)),
            new DataColumn("当前状态", typeof(string)),
            new DataColumn("创建人", typeof(string)),
            new DataColumn("创建时间", typeof(DateTime)),
            new DataColumn("需执行操作", typeof(string)),
            new DataColumn("提醒时间", typeof(DateTime))
        };
        }

        public override IEnumerable<object[]> GetDataRows()
        {
            return new List<object[]>
        {
            new object[] { DocumentType, DocumentNumber, DocumentStatus, Creator, CreateTime, ApprovalAction, TriggerTime }
        };
        }
    }
}
