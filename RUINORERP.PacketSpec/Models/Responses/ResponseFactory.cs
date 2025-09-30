using RUINORERP.PacketSpec.Errors;
using System;
using RUINORERP.PacketSpec.Errors;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>一行代码返回成功/失败/异常</summary>
    public static class ResponseFactory
    {
        // ---------- 成功 ----------
        public static ResponseBase Ok(string msg = "操作成功") 
            => new() { IsSuccess = true, Code = 0, Message = msg, TimestampUtc = DateTime.UtcNow };

        public static ResponseBase<T> Ok<T>(T data, string msg = "操作成功") 
            => new() { IsSuccess = true, Code = 0, Message = msg, Data = data, TimestampUtc = DateTime.UtcNow };

        // ---------- 失败 ----------
        public static ResponseBase Fail(ErrorCode errorCode, string additionalMessage = null)
        {
            var message = string.IsNullOrEmpty(additionalMessage) ? errorCode.Message : $"{errorCode.Message}: {additionalMessage}";
            return new() { IsSuccess = false, Code = errorCode.Code, Message = message, TimestampUtc = DateTime.UtcNow };
        }

        public static ResponseBase<T> Fail<T>(ErrorCode errorCode, string additionalMessage = null)
        {
            var message = string.IsNullOrEmpty(additionalMessage) ? errorCode.Message : $"{errorCode.Message}: {additionalMessage}";
            return new() { IsSuccess = false, Code = errorCode.Code, Message = message, Data = default, TimestampUtc = DateTime.UtcNow };
        }

        // ---------- 异常 ----------
        public static ResponseBase Except(Exception ex, ErrorCode fallback = default)
        {
            var errorCode = fallback.Code == 0 ? UnifiedErrorCodes.System_InternalError : fallback;
            return Fail(errorCode, $"[{ex.GetType().Name}] {ex.Message}")
                .WithMetadata("StackTrace", ex.StackTrace);
        }
    }
}
