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

    /// <summary>
    /// 
    /// </summary>
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

 



    //    // 客户端发送指令示例
    //    CommandDispatcher dispatcher = new CommandDispatcher();
    //    dispatcher.DispatchCommand(Command.UserManagement, new LoginCommandParameters { Username = "user", Password = "pass" });

    //// 服务器端接收指令示例
    //byte[] receivedData = // 从socket接收数据;
    //// 解析receivedData，获取指令和参数
    //dispatcher.DispatchCommand(receivedCommand, receivedParameters);



 
}
