using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction.DataModel
{
    public class LoginEvent
    {
        public string Username { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime LoginTime { get; set; }

        // 可以添加更多与登录事件相关的属性
    }
}
