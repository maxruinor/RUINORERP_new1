using System;
using System.Collections.Generic;
using System.Text;
using TransInstruction.DataModel;

namespace TransInstruction.CommandService
{

    /// <summary>
    /// 定义事件和事件处理器，用于解耦命令执行和副作用。
    /// </summary>
    // IEventHandler.cs
    public interface IEventHandler<TEvent>
    {
        void Handle(TEvent eventInfo);
    }

    // LoginEventHandler.cs
    public class LoginEventHandler : IEventHandler<LoginEvent>
    {
        public void Handle(LoginEvent eventData)
        {
            // 在这里处理登录事件，例如记录日志、更新UI、发送通知等
            Console.WriteLine($"User {eventData.Username} logged in at {eventData.LoginTime}. Success: {eventData.IsSuccess}");
        }
    }
}
