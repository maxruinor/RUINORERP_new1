using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction.CommandService
{
    public enum RequestReminderType
    {
        删除提醒,
        添加提醒,
    }

    public enum LoginProcessType
    {
        登陆,
        登陆回复,
        已经在线,
        超过限制,
    }

}
