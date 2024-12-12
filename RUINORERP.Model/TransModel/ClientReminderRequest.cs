using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.TransModel
{
    public class ClientReminderRequest : ReminderDataBase
    {
        // 可以添加客户端特有的属性和方法
        public string ClientId { get; set; } // 客户端的唯一标识符
        /// <summary>
        /// 用户ID，不是员工ID
        /// </summary>
        public long RemindTargetID { get; set; }
        public string RemindTargetName { get; set; }
        public BizType BizType { get; set; }
        public long BizID { get; set; }
        public string ReceiverID { get; set; }
        public string ReminderSubject { get; set; }
        public string ReminderContent { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
