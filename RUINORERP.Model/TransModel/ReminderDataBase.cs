﻿using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.TransModel
{
    public abstract class ReminderDataBase : IReminderData
    {
        public string Id { get; set; }

        public long BizPrimaryKey { get; set; }

        public BizType BizType { get; set; }
        public string WorkflowId { get; set; } // 工作流的唯一标识符，用于关联特定的工作流
        public DateTime ReminderTime { get; set; } // 提醒的时间
        public string Message { get; set; } // 提醒的内容
                                            // 其他可能的字段，如提醒相关的业务数据
        // 可以添加通用方法，例如验证提醒时间是否在未来
        public bool IsReminderTimeInFuture()
        {
            return ReminderTime > DateTime.UtcNow;
        }
    }

}