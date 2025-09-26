using System.Threading.Tasks;

namespace RUINORERP.UI.Network.ErrorHandling
{
    /// <summary>
    /// 错误处理策略接口
    /// 定义错误处理的核心方法
    /// 不同类型的错误可以有不同的处理策略实现
    /// </summary>
    public interface IErrorHandlingStrategy
    {
        /// <summary>
        /// 处理特定类型的错误
        /// </summary>
        /// <param name="errorType">错误类型</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="commandId">相关命令ID</param>
        /// <returns>异步任务</returns>
        Task HandleErrorAsync(TimeoutStatistics.NetworkErrorType errorType, string errorMessage, string commandId);
        
        /// <summary>
        /// 获取支持的错误类型
        /// </summary>
        /// <param name="errorType">错误类型</param>
        /// <returns>是否支持该错误类型的处理</returns>
        bool SupportsErrorType(TimeoutStatistics.NetworkErrorType errorType);
    }
}