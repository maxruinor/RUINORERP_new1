using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Requests;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Validation;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 泛型命令处理器基类 - 提供通用的命令处理框架
    /// 适用于标准化的请求-响应模式业务场景
    /// </summary>
    /// <typeparam name="TRequest">请求类型</typeparam>
    /// <typeparam name="TResponse">响应类型</typeparam>
    public abstract class GenericCommandHandler<TRequest, TResponse> : BaseCommandHandler
        where TRequest : IRequest
        where TResponse : IResponse
    {
    

        /// <summary>
        /// 无参构造函数，用于动态创建实例
        /// </summary>
        protected GenericCommandHandler() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected GenericCommandHandler(ILogger<GenericCommandHandler<TRequest, TResponse>> logger) : base(logger)
        {
            Logger = logger;
        }

        /// <summary>
        /// 核心处理方法 - 统一处理泛型命令（重构版：使用基础类的预处理流程）
        /// </summary>
        /// <param name="cmd">队列命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>响应结果</returns>
        protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            if (cmd?.Command == null)
            {
                Logger?.LogError("命令或命令数据为空");
                return BaseCommand<IResponse>.CreateError("命令数据无效", 400);
            }

            try
            {
                // 类型转换验证
                if (!(cmd.Command is TRequest request))
                {
                    Logger?.LogError($"命令类型转换失败: 期望 {typeof(TRequest).Name}, 实际 {cmd.Command.GetType().Name}");
                    return BaseCommand<IResponse>.CreateError($"不支持的命令类型: {cmd.Command.GetType().Name}", 400);
                }

                // 请求验证（调用基础验证方法）
                var validationResult = await ValidateRequestAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errorMessage = validationResult.Errors.Count > 0 ? validationResult.Errors[0].ErrorMessage : "验证失败";
                    Logger?.LogWarning($"请求验证失败: {errorMessage}");
                    return BaseCommand<IResponse>.CreateError(errorMessage, 400);
                }

                // 执行业务逻辑
                var response = await HandleRequestAsync(request, cancellationToken);
                
                // 响应验证和转换
                return ValidateAndConvertResponse(response);
            }
            catch (OperationCanceledException)
            {
                Logger?.LogWarning("命令处理被取消");
                return BaseCommand<IResponse>.CreateError("操作被取消", 499);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"处理 {typeof(TRequest).Name} 时发生异常");
                return BaseCommand<IResponse>.CreateError($"处理失败: {ex.Message}", 500);
            }
        }

        /// <summary>
        /// 验证和转换响应数据 - 提取为独立方法提高可读性
        /// </summary>
        private BaseCommand<IResponse> ValidateAndConvertResponse(TResponse response)
        {
            // 响应验证
            if (response == null)
            {
                Logger?.LogError("业务处理返回空响应");
                return BaseCommand<IResponse>.CreateError("处理结果为空", 500);
            }

            // 类型转换验证
            if (response is ResponseBase responseBase)
            {
                return BaseCommand<IResponse>.CreateSuccess(responseBase as IResponse, responseBase.Message);
            }

            // 如果TResponse不是ResponseBase，需要转换或包装
            throw new InvalidCastException($"响应类型 {typeof(TResponse).Name} 无法转换为 ResponseBase");
        }

        /// <summary>
        /// 请求验证 - 可被子类重写以添加自定义验证逻辑
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        protected virtual async Task<FluentValidation.Results.ValidationResult> ValidateRequestAsync(TRequest request, CancellationToken cancellationToken)
        {
            // 基础验证 - 检查请求是否为空
            if (request == null)
            {
                return new FluentValidation.Results.ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure(string.Empty, UnifiedErrorCodes.Biz_DataInvalid.Message)
                    {
                        ErrorCode = UnifiedErrorCodes.Biz_DataInvalid.Code.ToString()
                    }
                });
            }

            // 使用FluentValidation进行验证
            if (request is RequestBase requestBase)
            {
                var validator = new RequestBaseValidator();
                var validationResult = await validator.ValidateAsync(requestBase, cancellationToken);
                
                if (!validationResult.IsValid)
                {
                    return validationResult;
                }
            }
            
            // 默认验证通过
            return new FluentValidation.Results.ValidationResult();
        }

        /// <summary>
        /// 业务逻辑处理 - 必须由子类实现
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>响应数据</returns>
        protected abstract Task<TResponse> HandleRequestAsync(TRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// 创建成功响应的便捷方法
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <param name="message">成功消息</param>
        /// <returns>成功响应</returns>
        protected ResponseBase<T> CreateSuccessResponse<T>(T data, string message = "操作成功")
        {
            return ResponseBase<T>.CreateSuccess(data, message);
        }

        /// <summary>
        /// 创建失败响应的便捷方法
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <returns>失败响应</returns>
        protected ResponseBase CreateErrorResponse(string message, int code = 500)
        {
            return ResponseBase.CreateError(message, code);
        }
    }

    
}
