using SuperSocket.ProtoBase;
using System;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 基础包信息 - 包含所有包类型的通用字段和方法
    /// </summary>
    public abstract class BasePackageInfo : IKeyedPackageInfo<string>
    {
        /// <summary>
        /// 包标识键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 包标志位
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 包体数据
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 验证包有效性
        /// </summary>
        public virtual bool IsValid()
        {
            return !string.IsNullOrEmpty(Key) && 
                   Body != null && 
                   Body.Length > 0 &&
                   Timestamp <= DateTime.UtcNow.AddMinutes(5) &&
                   Timestamp >= DateTime.UtcNow.AddMinutes(-5);
        }

        /// <summary>
        /// 获取包大小
        /// </summary>
        public int GetPackageSize()
        {
            return Body?.Length ?? 0;
        }

        /// <summary>
        /// 安全清理包数据
        /// </summary>
        public virtual void ClearSensitiveData()
        {
            if (Body != null)
            {
                Array.Clear(Body, 0, Body.Length);
                Body = null;
            }
        }

        /// <summary>
        /// 创建基础包信息
        /// </summary>
        protected static T CreateBase<T>(string key, byte[] body) where T : BasePackageInfo, new()
        {
            return new T
            {
                Key = key,
                Body = body,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// 业务包信息 - 用于SuperSocket管道处理
    /// </summary>
    public class BizPackageInfo : BasePackageInfo
    {
        /// <summary>
        /// 扩展数据
        /// </summary>
        public object ExtendedData { get; set; }

        /// <summary>
        /// 包类型
        /// </summary>
        public PackageType PackageType { get; set; }

        /// <summary>
        /// 指令编码
        /// </summary>
        public uint CommandCode { get; set; }

        /// <summary>
        /// KX协议指令码（byte类型）
        /// </summary>
        public byte Command { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 特殊指令
        /// </summary>
        public SpecialOrder SpecialOrder { get; set; }

        /// <summary>
        /// 创建业务包信息
        /// </summary>
        public static BizPackageInfo Create(string key, byte[] body, PackageType packageType = PackageType.Normal)
        {
            return new BizPackageInfo
            {
                Key = key,
                Body = body,
                PackageType = packageType,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 安全清理包数据
        /// </summary>
        public override void ClearSensitiveData()
        {
            base.ClearSensitiveData();
            ExtendedData = null;
        }
    }

    /// <summary>
    /// 登录包信息 - 用于登录相关的包处理
    /// </summary>
    public class LanderPackageInfo : BasePackageInfo
    {
        /// <summary>
        /// 认证令牌
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// 客户端版本
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 创建登录包信息
        /// </summary>
        public static LanderPackageInfo Create(string key, byte[] body, string authToken = null)
        {
            return new LanderPackageInfo
            {
                Key = key,
                Body = body,
                AuthToken = authToken,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 验证登录包有效性
        /// </summary>
        public override bool IsValid()
        {
            return base.IsValid() &&
                   Timestamp <= DateTime.UtcNow.AddMinutes(2) && // 登录包有效期更短
                   Timestamp >= DateTime.UtcNow.AddMinutes(-2);
        }

        /// <summary>
        /// 是否需要认证
        /// </summary>
        public bool RequiresAuthentication()
        {
            return !string.IsNullOrEmpty(AuthToken);
        }
    }

    /// <summary>
    /// 包类型枚举
    /// </summary>
    public enum PackageType
    {
        /// <summary>
        /// 普通数据包
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 心跳包
        /// </summary>
        Heartbeat = 1,

        /// <summary>
        /// 登录包
        /// </summary>
        Login = 2,

        /// <summary>
        /// 命令包
        /// </summary>
        Command = 3,

        /// <summary>
        /// 响应包
        /// </summary>
        Response = 4,

        /// <summary>
        /// 错误包
        /// </summary>
        Error = 5,

        /// <summary>
        /// 广播包
        /// </summary>
        Broadcast = 6,

        /// <summary>
        /// 加密包
        /// </summary>
        Encrypted = 7,

        /// <summary>
        /// 压缩包
        /// </summary>
        Compressed = 8,

        /// <summary>
        /// KX协议包
        /// </summary>
        KxProtocol = 9
    }

    /// <summary>
    /// 特殊指令枚举
    /// </summary>
    public enum SpecialOrder
    {
        /// <summary>
        /// 无特殊指令
        /// </summary>
        None = 0,

        /// <summary>
        /// 仅包头
        /// </summary>
        HeaderOnly = 1,

        /// <summary>
        /// 长度无效
        /// </summary>
        InvalidLength = 2,

        /// <summary>
        /// 解码错误
        /// </summary>
        DecodeError = 3,

        /// <summary>
        /// 加密错误
        /// </summary>
        EncryptError = 4,

        /// <summary>
        /// 校验和错误
        /// </summary>
        ChecksumError = 5
    }

    /// <summary>
    /// 包处理结果
    /// </summary>
    public class PackageProcessResult
    {
        /// <summary>
        /// 处理是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 处理消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 处理后的数据
        /// </summary>
        public byte[] ProcessedData { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 处理时间（毫秒）
        /// </summary>
        public long ProcessingTimeMs { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static PackageProcessResult CreateSuccess(byte[] processedData = null, string message = "处理成功")
        {
            return new PackageProcessResult
            {
                Success = true,
                Message = message,
                ProcessedData = processedData
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static PackageProcessResult CreateFailure(string message, string errorCode = null)
        {
            return new PackageProcessResult
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }
}