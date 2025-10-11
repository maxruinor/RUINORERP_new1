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
        /// 心跳回复
        /// </summary>
        public const ushort System_HeartbeatResponse = 0x0002;

        /// <summary>
        /// 系统状态查询
        /// </summary>
        public const ushort System_SystemStatus = 0x0003;

        /// <summary>
        /// 异常报告
        /// </summary>
        public const ushort System_ExceptionReport = 0x0004;
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

        #endregion

        #region 缓存命令 (0x02xx)
        /// <summary>
        /// 缓存更新
        /// </summary>
        public const ushort Cache_CacheUpdate = 0x0200;

        /// <summary>
        /// 缓存清理
        /// </summary>
        public const ushort Cache_CacheClear = 0x0201;

        /// <summary>
        /// 缓存统计
        /// </summary>
        public const ushort Cache_CacheStats = 0x0202;

        /// <summary>
        /// 请求缓存
        /// </summary>
        public const ushort Cache_CacheRequest = 0x0203;

        /// <summary>
        /// 发送缓存数据
        /// </summary>
        public const ushort Cache_CacheDataSend = 0x0204;

        /// <summary>
        /// 删除缓存
        /// </summary>
        public const ushort Cache_CacheDelete = 0x0205;

        /// <summary>
        /// 缓存数据列表
        /// </summary>
        public const ushort Cache_CacheDataList = 0x0206;

        /// <summary>
        /// 缓存信息列表
        /// </summary>
        public const ushort Cache_CacheInfoList = 0x0207;

        /// <summary>
        /// 缓存状态查询 - 查询缓存状态信息
        /// </summary>
        public const ushort Cache_CacheStatus = 0x0204;

        /// <summary>
        /// 批量缓存操作 - 执行批量缓存操作
        /// </summary>
        public const ushort Cache_CacheBatchOperation = 0x0205;

        /// <summary>
        /// 缓存同步 - 同步缓存数据
        /// </summary>
        public const ushort Cache_CacheSync = 0x0206;

        /// <summary>
        /// 缓存预热 - 预热缓存数据
        /// </summary>
        public const ushort Cache_CacheWarmup = 0x0207;

        /// <summary>
        /// 缓存失效 - 使缓存数据失效
        /// </summary>
        public const ushort Cache_CacheInvalidate = 0x0208;

        /// <summary>
        /// 缓存统计信息 - 获取缓存统计信息
        /// </summary>
        public const ushort Cache_CacheStatistics = 0x0209;

        /// <summary>
        /// 缓存订阅 - 订阅缓存变更
        /// </summary>
        public const ushort Cache_CacheSubscribe = 0x020A;

        /// <summary>
        /// 缓存取消订阅 - 取消订阅缓存变更
        /// </summary>
        public const ushort Cache_CacheUnsubscribe = 0x020B;

        public const ushort Cache_CacheRefresh = 0x020B;
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

        #region 锁管理命令 (0x08xx)
        /// <summary>
        /// 申请锁 - 申请获取资源锁
        /// </summary>
        public const ushort Lock_LockRequest = 0x0800;

        /// <summary>
        /// 释放锁 - 释放已获取的资源锁
        /// </summary>
        public const ushort Lock_LockRelease = 0x0801;

        /// <summary>
        /// 锁状态查询 - 查询资源锁状态
        /// </summary>
        public const ushort Lock_LockStatus = 0x0802;

        /// <summary>
        /// 强制解锁 - 强制释放资源锁
        /// </summary>
        public const ushort Lock_ForceUnlock = 0x0803;

        /// <summary>
        /// 检查锁状态 - 检查资源锁是否可用
        /// </summary>
        public const ushort Lock_CheckLock = 0x0804;

        /// <summary>
        /// 广播 - 广播锁状态信息
        /// </summary>
        public const ushort Lock_Broadcast = 0x0805;

        /// <summary>
        /// 请求解锁 - 请求释放锁资源
        /// </summary>
        public const ushort Lock_RequestUnlock = 0x0806;

        /// <summary>
        /// 拒绝解锁 - 拒绝解锁请求
        /// </summary>
        public const ushort Lock_RefuseUnlock = 0x0807;

        /// <summary>
        /// 请求锁定单据 - 请求锁定业务单据
        /// </summary>
        public const ushort Lock_RequestLock = 0x0808;

        /// <summary>
        /// 释放锁定的单据 - 释放已锁定的业务单据
        /// </summary>
        public const ushort Lock_ReleaseLock = 0x0809;

        /// <summary>
        /// 强制释放锁定 - 强制释放业务单据锁
        /// </summary>
        public const ushort Lock_ForceReleaseLock = 0x080A;

        /// <summary>
        /// 锁定状态查询 - 查询业务单据锁定状态
        /// </summary>
        public const ushort Lock_QueryLockStatus = 0x080B;

        /// <summary>
        /// 广播锁定状态变化 - 广播业务单据锁定状态变化
        /// </summary>
        public const ushort Lock_BroadcastLockStatus = 0x080C;

        /// <summary>
        /// 锁管理 - 锁资源管理操作
        /// </summary>
        public const ushort Lock_LockManagement = 0x080D;

        /// <summary>
        /// 转发单据锁定 - 转发单据锁定请求
        /// </summary>
        public const ushort Lock_ForwardDocumentLock = 0x080E;
        #endregion
    }
}
