using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction.CommandService
{
    /// <summary>
    /// 发送和接收合并的指令中要有这个标识
    /// </summary>
    public enum CmdOperation
    {
        Send=1,
        Receive=2
    }


    public enum NextProcesszStep
    {
        无 = 0,
        转发 = 2,
    }

    public enum EntityTransferCmdType
    {
        提交实体数据=1,
        接受实体数据=2,
        处理动态配置 = 3,
    }


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
