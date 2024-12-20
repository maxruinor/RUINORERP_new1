using RUINOR.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction
{
    //public enum DataTransCommand
    //{
    //    None = 0,
    //    UserManagement = 1,
    //    ProductManagement = 2,
    //    OrderManagement = 3,
    //    // 其他主指令
    //}

    /// <summary>
    /// 服务器发出的消息指令
    /// </summary>
    public enum ServerPushMessageSubCmd
    {
        Login = 1,
        Logout = 2,
        Register = 3,
        // 用户管理的子指令
    }

    public enum ClientPushMessageSubCmd
    {
        Login = 1,
        Logout = 2,
        Register = 3,
        // 用户管理的子指令
    }

    public enum ClientSubCmdUserManagement
    {
        Login = 1,
        Logout = 2,
        Register = 3,
        // 用户管理的子指令
    }

    public enum ProductManagementCommand
    {
        AddProduct = 1,
        UpdateProduct = 2,
        DeleteProduct = 3,
        // 产品管理的子指令
    }

    // 为其他主指令定义子指令枚举
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
        public void DispatchCommand(RevertCommand command, object parameters)
        {
            switch (command)
            {
                case RevertCommand.UserManagement:
                    // 根据子指令进一步分发
                    break;
                case RevertCommand.ProductManagement:
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
