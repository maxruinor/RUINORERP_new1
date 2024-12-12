using Mapster;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;

namespace RUINORERP.Server.Workflow.WFReminder
{

    public class ReminderBizData: ServerReminderData
    {
        /// <summary>
        /// 单号
        /// </summary>
        public long BizID { get; set; }

        /// <summary>
        /// 接收人-用户名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 接收人-用户名
        /// </summary>
        public long ReceiverID { get; set; }

        public string BizKey { get; set; }
 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


        //这里设置一些回应的信息，让管理员看到？

        /// <summary>
        /// 直到停止（可能延期,后面根据规则来）
        /// </summary>
        public bool StopRemind { get; set; } = false;

        /// <summary>
        /// 提醒间隔
        /// </summary>
        public int RemindInterval { get; set; }

        /// <summary>
        /// 提醒次数
        /// </summary>
        public int RemindTimes { get; set; }

        /// <summary>
        /// 是一个枚举，应对如何处理提醒
        /// </summary>
        public int ProcessRemind { get; set; }

        public string RemindSubject { get; set; }
        public string ReminderContent { get; set; }
        public string WorkflowName { get; set; }


        // 新增的业务逻辑属性
        public bool IsReminderActive { get; set; } // 提醒是否激活
        public List<ReminderAction> ActionsTaken { get; set; } // 已执行的动作列表

        // 业务逻辑方法
        public void UpdateReminderStatus(ReminderAction action)
        {
            // 更新提醒状态的逻辑
            ActionsTaken.Add(action);
            // 根据action决定是否需要更新IsProcessed或其他状态
        }
  
        // 业务逻辑方法
        public void ProcessReminder()
        {
            // 处理提醒的业务逻辑
        }

        public void UpdateAction(ReminderAction action)
        {
            // 更新动作的业务逻辑
            ActionsTaken.Add(action);
            // 根据action决定是否需要更新IsProcessed或其他状态
        }
        
    }

    public enum ReminderAction
    {
        ReminderSent,
        ReminderAcknowledged,
        ReminderCancelled,
        // 其他可能的动作...
    }
}
