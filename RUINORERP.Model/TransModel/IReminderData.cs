using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.TransModel
{
    internal interface IReminderData
    {
        string Id { get; set; }

        /// <summary>
        /// 所有业务的唯一主键值
        /// </summary>
        long BizPrimaryKey { get; set; }

        string WorkflowId { get; set; }
        DateTime ReminderTime { get; set; }
        string Message { get; set; }
    }
}
