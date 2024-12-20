using RUINOR.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction
{
    public interface ICommandHandler
    {
        void HandleCommand(object parameters);
    }

    public class UserManagementCommandHandler : ICommandHandler
    {
        public void HandleCommand(object parameters)
        {
            // 处理用户管理指令
        }
    }

    public class LoginCommandHandler : ICommandHandler
    {
        public void HandleCommand(object parameters)
        {
            var loginParams = parameters as LoginCommandParameters;
            // 处理登录指令
        }
    }

    /// <summary>
    /// 指令调度器
    /// </summary>
    public class CommandDispatcher
    {
        public void DispatchCommand(Command command, object parameters)
        {
            switch (command)
            {
                case Command.UserManagement:
                    // 根据子指令进一步分发
                    break;
                case Command.ProductManagement:
                    // 根据子指令进一步分发
                    break;
                    // 其他主指令
            }
        }

        private void DispatchUserManagementCommand(UserManagementCommand command, object parameters)
        {
            switch (command)
            {
                case UserManagementCommand.Login:
                    new LoginCommandHandler().HandleCommand(parameters);
                    break;
                    // 其他子指令
            }
        }

        // 为其他主指令实现分发方法

    }


    //    // 客户端发送指令示例
    //    CommandDispatcher dispatcher = new CommandDispatcher();
    //    dispatcher.DispatchCommand(Command.UserManagement, new LoginCommandParameters { Username = "user", Password = "pass" });

    //// 服务器端接收指令示例
    //byte[] receivedData = // 从socket接收数据;
    //// 解析receivedData，获取指令和参数
    //dispatcher.DispatchCommand(receivedCommand, receivedParameters);


    public class LoginCommandParameters
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AddProductCommandParameters
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        // 其他产品信息
    }
}
