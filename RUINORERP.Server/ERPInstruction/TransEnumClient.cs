namespace TransInstruction
{
    #region 新的指令集合系统


    public enum ServerCommand
    {
        None = 0,
        UserManagement = 1,
        BizManagement = 2,
        OtherManagement = 3,
        // 其他主指令
    }
    public enum UserManagementCommand
    {
        Login = 1,
        Logout = 2,
        Register = 3,
        // 用户管理的子指令
    }

    public enum BizManagementCommand
    {
        AddProduct = 1,
        UpdateProduct = 2,
        DeleteProduct = 3,
        // 产品管理的子指令
    }

    #endregion

    public enum PackageSourceType
    {
        Client,
        Server
    }
    public enum MessageType
    {
        说话 = 1,
        呼喊 = 2,
        黄色警告 = 3,
        耳语,
        提示 = 150
    }




    /// <summary>
    /// 来自客户端的指令 动作
    /// </summary>
    public enum ClientCmdEnum
    {
        空指令 = 0x0,
        准备登陆 = 0x01,
        用户登陆 = 0x02,
        请求缓存 = 0x03,
        更新缓存 = 0x04,
        实时汇报异常 = 0x5,
        请求协助处理 = 0x6,
        删除缓存 = 0x07,
        工作流启动 = 0x8,
        工作流指令 = 0x9,
        更新动态配置 = 0x10,
        发送弹窗消息 = 0x11,

        客户端心跳包 = 0x99,
        打开U帮助 = 0x90156,
        删除帮助项 = 0x90158,
        换线登陆 = 0x90091,
        角色处于等待 = 0x90092,


        工作流审批 = 0x12,
        单据锁定 = 0x13,
        单据锁定释放 = 0x14,
        请求强制用户下线 = 0x15,
        
        /// <summary>
        /// 客户端提交数据有很多很多种
        /// </summary>
        工作流提醒请求 = 0x16,

        工作流提醒变化 = 0x17,

        //回复服务器是已经读取还是稍候提醒
        工作流提醒回复 = 0x18,

        /// <summary>
        /// 有用户数限制时，希望T掉别人。
        /// </summary>
        请求强制登陆上线 = 0x19,
    }

    /// <summary>
    /// 客户端操作后，传到服务器。
    /// 服务器按这个类型来处理不同的工作流任务，启动工作流
    /// </summary>
    public enum WorkflowBizType
    {
        基础数据信息推送 = 1,
        提醒业务数据推送 = 1,
    }


    /// <summary>
    /// 来自客户端的指令 动作
    /// </summary>
    public enum ClientSubCmdEnum
    {
        审批 = 0x90,
        反审 = 0x91,
    }

    /// <summary>
    /// 来自服务器的指令 已经过时  实际指令  分 主cmd  细分指令  再可能再细分 如角色信息修改变化
    /// </summary>
    public enum ServerCmdEnum
    {
        未知指令 = 0,
        用户登陆回复 = 0xA,
        发送在线列表 = 0x1,
        转发弹窗消息 = 0x2,
        转发消息结果 = 0x3,
        强制用户退出 = 0x4,
        给客户端发提示消息 = 0x5,
        通知审批人审批 = 0x6,
        通知相关人员审批完成 = 0x7,
        通知发起人启动成功 = 0x8,
        推送版本更新 = 0x9,
        发送缓存数据 = 0x10,
        发送缓存数据列表 = 0x11,
        转发异常 = 0x12,//服务器自己和客户端上传的异常信息。实时传给其他客户端（目前是在的超级管理员）
        转发协助处理 = 0x13,//服务器自己和客户端上传的异常信息。实时传给其他客户端（目前是在的超级管理员）
        转发更新缓存 = 0x14,
        转发单据锁定 = 0x15,

        /// <summary>
        /// 客户机登陆时，把服务器缓存信息数量发到客户机。客户机在空闲时再请求实际的缓存数据
        /// </summary>
        发送缓存信息列表 = 0x16,
        转发单据锁定释放 = 0x17,
        转发删除缓存 = 0x18,
        根据锁定用户释放 = 0x19,
        回复用户重复登陆 = 0x20,

        工作流提醒推送 = 0x21,
        转发更新动态配置 = 0x22,

        心跳回复 = 0x99,
        关机 = 0x94,
        删除列的配置文件 = 0x95,
        工作流数据推送 = 0x96,

        首次连接欢迎消息 = 0x99,
    }


}
