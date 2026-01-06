using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Models.Message
{
    internal interface IReminderData
    {
        string Id { get; set; }

        /// <summary>
        ///  是否取消提醒，是的话，先按这个条件停止工作流，然后会在集合中删除这些要提醒的数据
        /// </summary>
        bool IsCancelled { get; set; }
        /// <summary>
        /// 所有业务的唯一主键值
        /// </summary>
        long BizPrimaryKey { get; set; }

        string WorkflowId { get; set; }
        DateTime ReminderTime { get; set; }
        string Message { get; set; }
    }
}
