namespace RUINORERP.PacketSpec.Errors
{
    /// <summary>全局错误码，按模块分段</summary>
    public enum ErrorCode : int
    {
        // 0 保留给 Success
        OK = 0,

        // 1-999  系统级
        System_Error         = 100,
        System_Timeout       = 101,
        System_DeserializeFail=102,

        // 1000-1999  网络/指令
        Command_NotFound     = 1000,
        Command_ValidateFail = 1001,
        Command_Timeout      = 1002,

        // 2000-2999  认证
        Auth_UserNotFound    = 2000,
        Auth_PasswordWrong   = 2001,
        Auth_TokenExpired    = 2002,

        // 3000-3999  业务
        Biz_LockConflict     = 3000,
        Biz_DuplicateSubmit  = 3001,
    }
}