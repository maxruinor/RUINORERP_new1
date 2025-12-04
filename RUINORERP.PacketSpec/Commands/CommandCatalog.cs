using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令目录 - 包含所有系统命令码的集中定义
    /// </summary>
    public static class CommandCatalog
    {
        #region 系统命令 (0x00xx)
        /// <summary>
        /// 空命令/心跳包
        /// </summary>
        public const ushort System_None = 0x0000;

        /// <summary>
        /// 心跳包 - 保持连接活跃
        /// </summary>
        public const ushort System_Heartbeat = 0x0001;


        /// <summary>
        /// 电脑状态查询
        /// </summary>
        public const ushort System_ComputerStatus = 0x0003;

        /// <summary>
        /// 异常报告
        /// </summary>
        public const ushort System_ExceptionReport = 0x0004;

 

        /// <summary>
        /// 系统管理：服务器推送版本更新
        /// </summary>
        public const ushort System_SystemManagement = 0x0005;
        #endregion

        #region 认证命令 (0x01xx)
        /// <summary>
        /// 用户登录 - 客户端向服务器发起登录请求
        /// </summary>
        public const ushort Authentication_Login = 0x0100;

        /// <summary>
        /// 用户登出 - 用户主动或被动退出系统
        /// </summary>
        public const ushort Authentication_Logout = 0x0101;

        /// <summary>
        /// 验证Token - 验证用户身份令牌的有效性
        /// </summary>
        public const ushort Authentication_ValidateToken = 0x0102;

        /// <summary>
        /// 刷新Token - 更新用户身份令牌
        /// </summary>
        public const ushort Authentication_RefreshToken = 0x0103;

        /// <summary>
        /// 准备登录 - 登录前的准备工作
        /// </summary>
        public const ushort Authentication_PrepareLogin = 0x0104;

        /// <summary>
        /// 用户登录请求 - 客户端发起的登录请求
        /// </summary>
        public const ushort Authentication_LoginRequest = 0x0105;

        /// <summary>
        /// 登录回复 - 服务器对登录请求的响应
        /// </summary>
        public const ushort Authentication_LoginResponse = 0x0106;

        /// <summary>
        /// 登录验证 - 服务器验证用户登录信息
        /// </summary>
        public const ushort Authentication_LoginValidation = 0x0107;

        /// <summary>
        /// 重复登录通知 - 通知用户账号在其他地方登录
        /// </summary>
        public const ushort Authentication_DuplicateLoginNotification = 0x0108;

        /// <summary>
        /// 强制用户下线 - 管理员强制用户退出系统
        /// </summary>
        public const ushort Authentication_ForceLogout = 0x0109;

        /// <summary>
        /// 强制登录上线 - 强制用户登录到系统
        /// </summary>
        public const ushort Authentication_ForceLogin = 0x010A;

        /// <summary>
        /// 用户状态同步 - 同步用户在线状态信息
        /// </summary>
        public const ushort Authentication_UserStatusSync = 0x010B;

        /// <summary>
        /// 在线用户列表 - 获取当前在线用户列表
        /// </summary>
        public const ushort Authentication_OnlineUserList = 0x010C;

        /// <summary>
        /// 用户信息查询 - 查询特定用户信息
        /// </summary>
        public const ushort Authentication_UserInfo = 0x010D;

        /// <summary>
        /// 用户列表 - 获取系统用户列表
        /// </summary>
        public const ushort Authentication_UserList = 0x010E;

        /// <summary>
        /// 在线用户 - 获取在线用户信息
        /// </summary>
        public const ushort Authentication_OnlineUsers = 0x010F;

        public const ushort Authentication_Connected = 0x0110;

        /// <summary>
        /// 重复登陆时的处理方式 - 处理用户重复登录的命令T掉
        /// </summary>
        public const ushort Authentication_DuplicateLogin = 0x0111;
        #endregion

        #region 缓存命令 (0x02xx)
        /// <summary>
        /// 缓存操作 - 统一的缓存操作命令
        /// 通过CacheRequest.Operation区分具体操作类型
        /// </summary>
        public const ushort Cache_CacheOperation = 0x0201;

        /// <summary>
        /// 缓存同步 - 用于缓存数据的双向同步
        /// </summary>
        public const ushort Cache_CacheSync = 0x0202;

        /// <summary>
        /// 缓存订阅管理 - 用于缓存订阅和取消订阅操作
        /// </summary>
        public const ushort Cache_CacheSubscription = 0x0203;

        /// <summary>
        /// 缓存元数据同步 - 同步缓存的元数据信息
        /// </summary>
        public const ushort Cache_CacheMetadataSync = 0x0204;

        #endregion

        #region 消息命令 (0x03xx)
        /// <summary>
        /// 发送弹窗消息 - 向指定用户发送弹窗消息
        /// </summary>
        public const ushort Message_SendPopupMessage = 0x0300;

        /// <summary>
        /// 转发弹窗消息 - 转发弹窗消息给其他用户
        /// </summary>
        public const ushort Message_ForwardPopupMessage = 0x0301;

        /// <summary>
        /// 消息响应 - 对消息的响应
        /// </summary>
        public const ushort Message_MessageResponse = 0x0302;

        /// <summary>
        /// 转发消息结果 - 转发消息处理结果
        /// </summary>
        public const ushort Message_ForwardMessageResult = 0x0303;

        /// <summary>
        /// 发送用户消息 - 向指定用户发送消息
        /// </summary>
        public const ushort Message_SendMessageToUser = 0x0304;

        /// <summary>
        /// 发送部门消息 - 向指定部门发送消息
        /// </summary>
        public const ushort Message_SendMessageToDepartment = 0x0305;

        /// <summary>
        /// 广播消息 - 向所有用户广播消息
        /// </summary>
        public const ushort Message_BroadcastMessage = 0x0306;

        /// <summary>
        /// 发送系统通知 - 发送系统级别的通知消息
        /// </summary>
        public const ushort Message_SendSystemNotification = 0x0307;

        /// <summary>
        /// 系统消息 - 系统内部消息
        /// </summary>
        public const ushort Message_SystemMessage = 0x0308;

        /// <summary>
        /// 提示消息 - 系统提示信息
        /// </summary>
        public const ushort Message_NotificationMessage = 0x0309;
        #endregion

        #region 工作流命令 (0x04xx)
        /// <summary>
        /// 工作流提醒 - 提醒用户处理工作流任务
        /// </summary>
        public const ushort Workflow_WorkflowReminder = 0x0400;

        /// <summary>
        /// 工作流状态更新 - 更新工作流任务状态
        /// </summary>
        public const ushort Workflow_WorkflowStatusUpdate = 0x0401;

        /// <summary>
        /// 工作流审批 - 处理工作流审批任务
        /// </summary>
        public const ushort Workflow_WorkflowApproval = 0x0402;

        /// <summary>
        /// 工作流启动 - 启动一个新的工作流实例
        /// </summary>
        public const ushort Workflow_WorkflowStart = 0x0403;

        /// <summary>
        /// 工作流指令 - 工作流相关指令操作
        /// </summary>
        public const ushort Workflow_WorkflowCommand = 0x0404;

        /// <summary>
        /// 通知审批人审批 - 通知审批人处理审批任务
        /// </summary>
        public const ushort Workflow_NotifyApprover = 0x0405;

        /// <summary>
        /// 通知审批完成 - 通知相关人员审批已完成
        /// </summary>
        public const ushort Workflow_NotifyApprovalComplete = 0x0406;

        /// <summary>
        /// 通知启动成功 - 通知工作流启动成功
        /// </summary>
        public const ushort Workflow_NotifyStartSuccess = 0x0407;

        /// <summary>
        /// 工作流提醒请求 - 请求工作流提醒信息
        /// </summary>
        public const ushort Workflow_WorkflowReminderRequest = 0x0408;

        /// <summary>
        /// 工作流提醒变化 - 工作流提醒状态发生变化
        /// </summary>
        public const ushort Workflow_WorkflowReminderChanged = 0x0409;

        /// <summary>
        /// 工作流提醒回复 - 对工作流提醒的回复
        /// </summary>
        public const ushort Workflow_WorkflowReminderReply = 0x040A;
        #endregion

        #region 文件命令 (0x06xx)
        /// <summary>
        /// 文件操作 - 执行文件相关操作
        /// </summary>
        public const ushort File_FileOperation = 0x0600;

        /// <summary>
        /// 文件上传 - 上传文件到服务器
        /// </summary>
        public const ushort File_FileUpload = 0x0601;

        /// <summary>
        /// 文件下载 - 从服务器下载文件
        /// </summary>
        public const ushort File_FileDownload = 0x0602;

        /// <summary>
        /// 文件删除 - 从服务器删除文件
        /// </summary>
        public const ushort File_FileDelete = 0x0603;

        /// <summary>
        /// 文件信息查询 - 查询文件信息
        /// </summary>
        public const ushort File_FileInfoQuery = 0x0604;

        /// <summary>
        /// 文件列表 - 获取文件列表
        /// </summary>
        public const ushort File_FileList = 0x0605;

        /// <summary>
        /// 文件权限检查 - 检查文件访问权限
        /// </summary>
        public const ushort File_FilePermissionCheck = 0x0606;

        /// <summary>
        /// 文件存储信息 - 获取文件存储使用情况
        /// </summary>
        public const ushort File_FileStorageInfo = 0x060A;

        /// <summary>
        /// 文件重命名 - 重命名文件
        /// </summary>
        public const ushort File_FileRename = 0x0607;

        /// <summary>
        /// 文件移动 - 移动文件位置
        /// </summary>
        public const ushort File_FileMove = 0x0608;

        /// <summary>
        /// 文件复制 - 复制文件
        /// </summary>
        public const ushort File_FileCopy = 0x0609;
        #endregion

        #region 数据同步命令 (0x07xx)
        /// <summary>
        /// 数据请求 - 请求同步数据
        /// </summary>
        public const ushort DataSync_DataRequest = 0x0700;

        /// <summary>
        /// 全量同步 - 执行全量数据同步
        /// </summary>
        public const ushort DataSync_FullSync = 0x0701;

        /// <summary>
        /// 增量同步 - 执行增量数据同步
        /// </summary>
        public const ushort DataSync_IncrementalSync = 0x0702;

        /// <summary>
        /// 同步状态查询 - 查询数据同步状态
        /// </summary>
        public const ushort DataSync_SyncStatus = 0x0703;

        /// <summary>
        /// 数据同步 - 执行数据同步操作
        /// </summary>
        public const ushort DataSync_DataSyncRequest = 0x0704;

        /// <summary>
        /// 实体数据传输 - 传输业务实体数据
        /// </summary>
        public const ushort DataSync_EntityDataTransfer = 0x0705;

        /// <summary>
        /// 更新动态配置 - 更新系统动态配置
        /// </summary>
        public const ushort DataSync_UpdateDynamicConfig = 0x0706;

        /// <summary>
        /// 转发更新动态配置 - 转发动态配置更新请求
        /// </summary>
        public const ushort DataSync_ForwardUpdateDynamicConfig = 0x0707;

        /// <summary>
        /// 更新全局配置 - 更新系统全局配置
        /// </summary>
        public const ushort DataSync_UpdateGlobalConfig = 0x0708;
        #endregion

        #region 锁管理命令 (0x08xx) - 优化版本
        /// <summary>
        /// 锁定单据 - 申请锁定指定单据，防止其他用户同时编辑
        /// 命令码: 0x0800
        /// </summary>
        public const ushort Lock_Lock = 0x0800;

        /// <summary>
        /// 解锁单据 - 释放已锁定的单据，允许其他用户编辑
        /// 命令码: 0x0801
        /// </summary>
        public const ushort Lock_Unlock = 0x0801;

        /// <summary>
        /// 检查锁定状态 - 查询单据当前的锁定状态
        /// 命令码: 0x0802
        /// </summary>
        public const ushort Lock_CheckLockStatus = 0x0802;

        /// <summary>
        /// 请求解锁 - 当单据被其他用户锁定时，请求当前锁定用户释放锁定
        /// 命令码: 0x0803
        /// </summary>
        public const ushort Lock_RequestUnlock = 0x0803;

        /// <summary>
        /// 拒绝解锁 - 当前锁定用户拒绝其他用户的解锁请求
        /// 命令码: 0x0804
        /// </summary>
        public const ushort Lock_RefuseUnlock = 0x0804;

        /// <summary>
        /// 同意解锁 - 当前锁定用户同意其他用户的解锁请求并释放锁定
        /// 命令码: 0x0805
        /// </summary>
        public const ushort Lock_AgreeUnlock = 0x0805;

        /// <summary>
        /// 强制解锁 - 管理员强制释放锁定（特殊情况使用）
        /// 命令码: 0x0806
        /// </summary>
        public const ushort Lock_ForceUnlock = 0x0806;

        /// <summary>
        /// 广播锁定状态 - 向相关用户广播锁定状态变化
        /// 命令码: 0x0807
        /// </summary>
        public const ushort Lock_BroadcastLockStatus = 0x0807;
        #endregion

        #region 配置命令 (0x09xx)
        /// <summary>
        /// 配置更新 - 更新系统配置信息
        /// </summary>
        public const ushort Config_ConfigUpdate = 0x0900;

        /// <summary>
        /// 配置请求 - 请求系统配置信息
        /// </summary>
        public const ushort Config_ConfigRequest = 0x0901;

        /// <summary>
        /// 配置同步 - 同步系统配置信息
        /// </summary>
        public const ushort Config_ConfigSync = 0x0902;
        #endregion

        #region 复合型命令 (0x10xx)
        /// <summary>
        /// 复合命令执行 - 执行包含多个子命令的复合操作
        /// </summary>
        public const ushort Composite_CompositeCommandExecute = 0x1000;

        /// <summary>
        /// 复合命令结果 - 复合命令执行结果反馈
        /// </summary>
        public const ushort Composite_CompositeCommandResult = 0x1001;
        #endregion

        #region 连接管理命令 (0x11xx)
        /// <summary>
        /// 连接状态通知 - 通知连接状态变化
        /// </summary>
        public const ushort Connection_ConnectionStatusNotification = 0x1100;

        /// <summary>
        /// 连接请求 - 请求建立新连接
        /// </summary>
        public const ushort Connection_ConnectionRequest = 0x1101;

        /// <summary>
        /// 连接断开 - 请求断开连接
        /// </summary>
        public const ushort Connection_ConnectionDisconnect = 0x1102;

        /// <summary>
        /// 连接池管理 - 管理连接池相关操作
        /// </summary>
        public const ushort Connection_ConnectionPoolManagement = 0x1103;
        #endregion

        #region 业务编码命令 (0x0Fxx)
        /// <summary>
        /// 业务编码操作 - 生成业务相关编码
        /// </summary>
        public const ushort BizCode_BizCodeOperation = 0x0F00;

        /// <summary>
        /// 生成业务单据编号 - 根据业务类型生成单据编号
        /// </summary>
        public const ushort BizCode_GenerateBizBillNo = 0x0F01;

        /// <summary>
        /// 生成基础信息编号 - 根据数据表生成基础信息编号
        /// </summary>
        public const ushort BizCode_GenerateBaseInfoNo = 0x0F02;

        /// <summary>
        /// 生成产品相关的  产品编码。短码  SKU码等
        /// </summary>
        public const ushort BizCode_ProductRelatedCode = 0x0F03;

        /// <summary>
        /// 生成条码 - 根据原始编码生成条形码
        /// </summary>
        public const ushort BizCode_GenerateBarCode = 0x0F05;

        /// <summary>
        /// 获取所有规则配置 - 获取所有编号规则配置
        /// </summary>
        public const ushort BizCode_GetAllRuleConfigs = 0x0F06;

        /// <summary>
        /// 保存规则配置 - 保存编号规则配置
        /// </summary>
        public const ushort BizCode_SaveRuleConfig = 0x0F07;

        /// <summary>
        /// 删除规则配置 - 删除编号规则配置
        /// </summary>
        public const ushort BizCode_DeleteRuleConfig = 0x0F08;
        #endregion

        #region 特殊功能命令 (0x90xx)
        /// <summary>
        /// 特殊操作 - 预留的特殊功能操作命令
        /// </summary>
        public const ushort Special_SpecialOperation = 0x9000;

        /// <summary>
        /// 诊断模式 - 系统诊断模式命令
        /// </summary>
        public const ushort Special_DiagnosticMode = 0x9001;

        /// <summary>
        /// 调试信息 - 获取系统调试信息
        /// </summary>
        public const ushort Special_DebugInfo = 0x9002;
        #endregion
    }
}
