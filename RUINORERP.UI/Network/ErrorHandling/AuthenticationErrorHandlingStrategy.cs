using System.Threading.Tasks;
using RUINORERP.UI.Network.TimeoutStatistics;

namespace RUINORERP.UI.Network.ErrorHandling
{
    /// <summary>
    /// 认证错误处理策略
    /// 处理用户认证相关的错误
    /// </summary>
    public class AuthenticationErrorHandlingStrategy : IErrorHandlingStrategy
    {
        /// <summary>
        /// 处理认证错误
        /// </summary>
        /// <param name="errorType">错误类型</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="commandId">相关命令ID</param>
        /// <returns>异步任务</returns>
        public Task HandleErrorAsync(NetworkErrorType errorType, string errorMessage, string commandId)
        {
            // 实现认证错误的具体处理逻辑
            // 例如：通知用户重新登录、清除无效令牌、触发重新认证流程等
            
            // 这里仅作为示例，实际实现需要根据项目具体需求进行完善
            // 可能包括：
            // 1. 记录认证错误日志
            // 2. 清除本地存储的认证信息
            // 3. 提示用户重新登录
            // 4. 导航到登录页面
            
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// 获取是否支持该错误类型
        /// </summary>
        /// <param name="errorType">错误类型</param>
        /// <returns>是否支持该错误类型的处理</returns>
        public bool SupportsErrorType(NetworkErrorType errorType)
        {
            return errorType == NetworkErrorType.AuthenticationError;
        }
    }
}