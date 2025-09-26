namespace RUINORERP.UI.Network.TimeoutStatistics
{
    /// <summary>
    /// 网络错误类型枚举
    /// 定义系统中可能发生的各种网络相关错误类型
    /// 用于错误分类和针对性处理
    /// </summary>
    public enum NetworkErrorType
    {
        /// <summary>
        /// 连接错误
        /// 包括网络连接断开、无法连接服务器等
        /// </summary>
        ConnectionError,
        
        /// <summary>
        /// 认证错误
        /// 包括用户名密码错误、令牌过期等认证失败情况
        /// </summary>
        AuthenticationError,
        
        /// <summary>
        /// 授权错误
        /// 包括权限不足、访问被拒绝等授权问题
        /// </summary>
        AuthorizationError,
        
        /// <summary>
        /// 超时错误
        /// 包括请求超时、响应超时等
        /// </summary>
        TimeoutError,
        
        /// <summary>
        /// 序列化错误
        /// 数据序列化过程中发生的错误
        /// </summary>
        SerializationError,
        
        /// <summary>
        /// 反序列化错误
        /// 数据反序列化过程中发生的错误
        /// </summary>
        DeserializationError,
        
        /// <summary>
        /// 命令错误
        /// 命令格式错误、命令不存在等问题
        /// </summary>
        CommandError,
        
        /// <summary>
        /// 服务器错误
        /// 服务器内部错误、服务器不可用等
        /// </summary>
        ServerError,
        
        /// <summary>
        /// 客户端错误
        /// 客户端内部错误、请求格式错误等
        /// </summary>
        ClientError,
        
        /// <summary>
        /// 未知错误
        /// 无法分类的其他错误
        /// </summary>
        UnknownError
    }
}