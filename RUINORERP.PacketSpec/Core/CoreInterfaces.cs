using System;

namespace RUINORERP.PacketSpec.Core
{
    /// <summary>
    /// 命令结果接口 - 定义命令执行结果的基本结构
    /// </summary>
    public interface ICommandResult
    {
        /// <summary>
        /// 命令执行是否成功
        /// </summary>
        bool IsSuccess { get; set; }

        /// <summary>
        /// 执行结果消息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        string ErrorCode { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        long ExecutionTimeMs { get; set; }
    }

    /// <summary>
    /// 数据包接口 - 定义数据包的基本结构
    /// </summary>
    public interface IPacketData
    {
        /// <summary>
        /// 包标志位
        /// </summary>
        string Flag { get; set; }

        /// <summary>
        /// 包体数据
        /// </summary>
        byte[] Body { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        string SessionId { get; set; }

        /// <summary>
        /// 获取包大小
        /// </summary>
        int GetPackageSize();

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        void ClearSensitiveData();
    }

    /// <summary>
    /// 可跟踪对象接口 - 定义可被跟踪的对象基本结构
    /// </summary>
    public interface ITraceable
    {
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        DateTime CreatedTime { get; set; }

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// 模型版本
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// 更新时间戳
        /// </summary>
        void UpdateTimestamp();
    }

    /// <summary>
    /// 可验证对象接口 - 定义可被验证的对象
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        bool IsValid();
    }

    /// <summary>
    /// 基础命令结果实现
    /// </summary>
    public class BaseCommandResult : ICommandResult
    {
        /// <summary>
        /// 命令执行是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = false;

        /// <summary>
        /// 执行结果消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }
    }

    /// <summary>
    /// 基础数据包实现
    /// </summary>
    public class BasePacketData : IPacketData
    {
        /// <summary>
        /// 包标志位
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 包体数据
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 获取包大小
        /// </summary>
        public int GetPackageSize()
        {
            return Body?.Length ?? 0;
        }

        /// <summary>
        /// 安全清理敏感数据
        /// </summary>
        public virtual void ClearSensitiveData()
        {
            // 清理包体数据
            if (Body != null)
            {
                Array.Clear(Body, 0, Body.Length);
                Body = null;
            }
        }
    }

    /// <summary>
    /// 基础可跟踪对象实现
    /// </summary>
    public class BaseTraceable : ITraceable
    {
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 模型版本
        /// </summary>
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 更新时间戳
        /// </summary>
        public void UpdateTimestamp()
        {
            Timestamp = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// 基础可验证对象实现
    /// </summary>
    public class BaseValidatable : IValidatable
    {
        /// <summary>
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public virtual bool IsValid()
        {
            return true;
        }
    }

    /// <summary>
    /// 命令执行结果扩展方法
    /// </summary>
    public static class CommandResultExtensions
    {
        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static T Success<T>(this T result, string message = null)
            where T : ICommandResult
        {
            result.IsSuccess = true;
            result.Message = message;
            result.ErrorCode = null;
            return result;
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static T Failure<T>(this T result, string message, string errorCode = null)
            where T : ICommandResult
        {
            result.IsSuccess = false;
            result.Message = message;
            result.ErrorCode = errorCode;
            return result;
        }
    }
}