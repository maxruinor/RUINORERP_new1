using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Communication
{
    /// <summary>
    /// 请求处理器基类
    /// 提供通用的请求处理实现
    /// </summary>
    /// <typeparam name="TRequest">请求数据类型</typeparam>
    /// <typeparam name="TResponse">响应数据类型</typeparam>
    public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    {
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>响应结果</returns>
        public async Task<ApiResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                // 验证请求
                var validationResult = await ValidateRequestAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return ApiResponse<TResponse>.Failure(validationResult.ErrorMessage, 400);
                }

                // 执行业务逻辑
                var result = await ProcessRequestAsync(request, cancellationToken);
                
                // 返回成功响应
                return ApiResponse<TResponse>.CreateSuccess(result.Data, result.Message);
            }
            catch (Exception ex)
            {
                // 处理异常
                return HandleException(ex);
            }
        }

        /// <summary>
        /// 验证请求
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        protected virtual async Task<RequestValidationResult> ValidateRequestAsync(TRequest request, CancellationToken cancellationToken)
        {
            // 默认实现，子类可以重写
            await Task.CompletedTask;
            return RequestValidationResult.Success();
        }

        /// <summary>
        /// 处理请求的核心逻辑
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        protected abstract Task<RequestProcessResult<TResponse>> ProcessRequestAsync(TRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns>错误响应</returns>
        protected virtual ApiResponse<TResponse> HandleException(Exception ex)
        {
            // 记录异常日志
            LogException(ex);
            
            // 返回通用错误响应
            return ApiResponse<TResponse>.Failure($"处理请求时发生错误: {ex.Message}", 500);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex">异常对象</param>
        protected virtual void LogException(Exception ex)
        {
            // 默认实现，子类可以重写
            Console.WriteLine($"请求处理异常: {ex}");
        }
    }

    /// <summary>
    /// 请求验证结果
    /// </summary>
    public class RequestValidationResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static RequestValidationResult Success()
        {
            return new RequestValidationResult { IsValid = true };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static RequestValidationResult Failure(string errorMessage)
        {
            return new RequestValidationResult { IsValid = false, ErrorMessage = errorMessage };
        }
    }

    /// <summary>
    /// 请求处理结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class RequestProcessResult<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 创建处理结果
        /// </summary>
        public static RequestProcessResult<T> Create(T data, string message = "操作成功")
        {
            return new RequestProcessResult<T> { Data = data, Message = message };
        }
    }
}