using System;


namespace RUINORERP.PacketSpec.Errors
{
    /// <summary>
    /// 统一错误代码体系
    /// 格式：模块_功能_错误类型
    /// </summary>
    public static class UnifiedErrorCodes
    {
        #region 系统级错误 (1000-1999)
        public static readonly ErrorCode System_InternalError = new(1001, "系统内部错误");
        public static readonly ErrorCode System_Timeout = new(1002, "系统操作超时");
        public static readonly ErrorCode System_SerializationError = new(1003, "数据序列化失败");
        public static readonly ErrorCode System_DeserializationError = new(1004, "数据反序列化失败");
        public static readonly ErrorCode System_ResourceBusy = new(1005, "系统资源繁忙");
        public static readonly ErrorCode System_NotInitialized = new(1006, "系统未初始化");
        #endregion

        #region 网络通信错误 (2000-2999)
        public static readonly ErrorCode Network_ConnectionFailed = new(2001, "网络连接失败");
        public static readonly ErrorCode Network_ConnectionLost = new(2002, "网络连接已断开");
        public static readonly ErrorCode Network_Timeout = new(2003, "网络请求超时");
        public static readonly ErrorCode Network_ProtocolError = new(2004, "网络协议错误");
        public static readonly ErrorCode Network_SendFailed = new(2005, "数据发送失败");
        public static readonly ErrorCode Network_ReceiveFailed = new(2006, "数据接收失败");
        #endregion

        #region 认证授权错误 (3000-3999)
        public static readonly ErrorCode Auth_UserNotFound = new(3001, "用户不存在");
        public static readonly ErrorCode Auth_PasswordError = new(3002, "密码错误");
        public static readonly ErrorCode Auth_TokenExpired = new(3003, "Token已过期");
        public static readonly ErrorCode Auth_TokenInvalid = new(3004, "Token无效");
        public static readonly ErrorCode Auth_AccessDenied = new(3005, "访问权限不足");
        public static readonly ErrorCode Auth_SessionExpired = new(3006, "会话已过期");
        public static readonly ErrorCode Auth_DuplicateLogin = new(3007, "账号在其他地方登录");
        public static readonly ErrorCode Auth_ValidationFailed = new(3008, "认证验证失败");
        #endregion

        #region 业务逻辑错误 (4000-4999)
        public static readonly ErrorCode Biz_DataNotFound = new(4001, "数据不存在");
        public static readonly ErrorCode Biz_DataInvalid = new(4002, "数据格式无效");
        public static readonly ErrorCode Biz_OperationFailed = new(4003, "业务操作失败");
        public static readonly ErrorCode Biz_DuplicateOperation = new(4004, "重复操作");
        public static readonly ErrorCode Biz_StateError = new(4005, "业务状态错误");
        public static readonly ErrorCode Biz_LimitExceeded = new(4006, "操作限制已超出");
        #endregion

        #region 命令处理错误 (5000-5999)
        public static readonly ErrorCode Command_NotFound = new(5001, "命令不存在");
        public static readonly ErrorCode Command_InvalidFormat = new(5002, "命令格式无效");
        public static readonly ErrorCode Command_ExecuteFailed = new(5003, "命令执行失败");
        public static readonly ErrorCode Command_Timeout = new(5004, "命令执行超时");
        public static readonly ErrorCode Command_ValidationFailed = new(5005, "命令验证失败");
        public static readonly ErrorCode Command_ProcessCancelled = new(5006, "命令处理被取消");
        #endregion

        #region 数据缓存错误 (6000-6999)
        public static readonly ErrorCode Cache_DataExpired = new(6001, "缓存数据已过期");
        public static readonly ErrorCode Cache_UpdateFailed = new(6002, "缓存更新失败");
        public static readonly ErrorCode Cache_SyncFailed = new(6003, "缓存同步失败");
        #endregion
    }

    public readonly struct ErrorCode
    {
        public int Code { get; }
        public string Message { get; }
        
        public ErrorCode(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public override string ToString() => $"{Code}: {Message}";
        
        public static implicit operator int(ErrorCode error) => error.Code;
        public static implicit operator string(ErrorCode error) => error.Message;
    }
}