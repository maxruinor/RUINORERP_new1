using RUINORERP.PacketSpec.Errors;
using System;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>一行代码返回成功/失败/异常</summary>
    public static class ResponseFactory
    {
        // ---------- 成功 ----------
        public static ResponseBase Ok(string msg = "OK") 
            => new() { IsSuccess = true, Code = (int)ErrorCode.OK, Message = msg, TimestampUtc = DateTime.UtcNow };

        public static ResponseBase<T> Ok<T>(T data, string msg = "OK") 
            => new() { IsSuccess = true, Code = (int)ErrorCode.OK, Message = msg, Data = data, TimestampUtc = DateTime.UtcNow };

        // ---------- 失败 ----------
        public static ResponseBase Fail(ErrorCode code, string userMsg = null)
            => new() { IsSuccess = false, Code = (int)code, Message = userMsg ?? GetDefaultMsg(code), TimestampUtc = DateTime.UtcNow };

        public static ResponseBase<T> Fail<T>(ErrorCode code, string userMsg = null)
            => new() { IsSuccess = false, Code = (int)code, Message = userMsg ?? GetDefaultMsg(code), Data = default, TimestampUtc = DateTime.UtcNow };

        // ---------- 异常 ----------
        public static ResponseBase Except(Exception ex, ErrorCode fallback = ErrorCode.System_Error)
            => Fail(fallback, $"[{ex.GetType().Name}] {ex.Message}")
                .WithMetadata("StackTrace", ex.StackTrace);

        // ---------- 辅助 ----------
        private static string GetDefaultMsg(ErrorCode c) => c switch
        {
            ErrorCode.Auth_UserNotFound    => "用户不存在",
            ErrorCode.Auth_PasswordWrong   => "密码错误",
            ErrorCode.Command_NotFound     => "指令未找到",
            ErrorCode.Biz_LockConflict     => "业务锁冲突",
            _                              => "操作失败"
        };
    }
}