using System;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.PacketSpec.Models.Core;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses.Authentication;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 响应工厂 - 统一创建各种类型的响应对象
    /// 支持特定类型的错误响应创建，供服务器和客户端共同使用
    /// </summary>
    public static class ResponseFactory
    {
        /// <summary>
        /// 静态日志记录器
        /// </summary>
        private static readonly ILogger _logger;

        /// <summary>
        /// 静态构造函数 - 初始化日志记录器
        /// </summary>
        static ResponseFactory()
        {
            // 使用项目中的LoggerFactory创建日志记录器（使用字符串参数而非泛型参数）
            _logger = new LoggerFactory().CreateLogger("RUINORERP.PacketSpec.Models.Responses.ResponseFactory");
        }
        /// <summary>
        /// 创建特定类型的成功响应（泛型版本，主要用于客户端）
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="message">成功消息</param>
        /// <param name="additionalMetadata">额外的元数据</param>
        /// <returns>特定类型的成功响应</returns>
        public static TResponse CreateSpecificSuccessResponse<TResponse>(
            string message = "操作成功",
            Dictionary<string, object> additionalMetadata = null
           )
            where TResponse : IResponse
        {
            try
            {
                // 使用与错误方法相同的实例化逻辑
                TResponse response;
                try
                {
                    // 检查TResponse是否为接口类型
                    if (typeof(TResponse).IsInterface)
                    {
                        // 如果是接口类型，使用ResponseBase作为默认实现
                        var defaultResponse = new ResponseBase();
                        // 将ResponseBase实例转换为TResponse接口
                        response = (TResponse)(object)defaultResponse;
                    }
                    else
                    {
                        // 尝试创建实例
                        response = Activator.CreateInstance<TResponse>();
                    }
                }
                catch (MissingMethodException)
                {
                    // 如果泛型方法失败，尝试使用Type.GetType()和CreateInstance
                    response = (TResponse)Activator.CreateInstance(typeof(TResponse), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, null, null);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "创建 {ResponseType} 实例失败", typeof(TResponse).Name);
                    throw new InvalidOperationException($"无法创建响应实例: {typeof(TResponse).Name}", ex);
                }

                // 设置基本成功属性
                response.IsSuccess = true;
                response.ErrorMessage = null;
                response.Message = message;
                response.ErrorCode = 0;

                // 设置元数据
                if (response.Metadata == null)
                {
                    response.Metadata = new Dictionary<string, object>();
                }

                // 添加成功相关的元数据
                response.Metadata["SuccessSource"] = "Client";
                response.Metadata["SuccessTime"] = DateTime.Now;

                // 添加额外的元数据
                if (additionalMetadata != null)
                {
                    foreach (var kvp in additionalMetadata)
                    {
                        response.Metadata[kvp.Key] = kvp.Value;
                    }
                }

                // 如果是ResponseBase类型，设置时间戳
                if (response is ResponseBase baseResponse)
                {
                    baseResponse.Timestamp = DateTime.Now;
                }

                _logger.LogDebug("成功创建特定类型成功响应: {ResponseType}", typeof(TResponse).Name);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建特定类型成功响应失败: {ResponseType}", typeof(TResponse).Name);
                throw new InvalidOperationException($"无法创建类型 {typeof(TResponse).Name} 的成功响应实例", ex);
            }
        }

        /// <summary>
        /// 命令ID到响应类型的映射（回退机制）
        /// </summary>
        private static readonly Dictionary<ushort, Type> _commandResponseMap = new Dictionary<ushort, Type>
        {
            // 认证相关命令
            { 0x0100, typeof(LoginResponse) },        // Authentication_Login
            { 0x0101, typeof(LogoutResponse) },       // Authentication_Logout
            // 可以根据需要继续添加其他命令的映射
        };

        /// <summary>
        /// 根据上下文创建特定类型的错误响应
        /// </summary>
        /// <param name="context">命令上下文</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="metadata">元数据</param>
        /// <param name="logger">可选的日志记录器</param>
        /// <returns>特定类型的错误响应</returns>
        /// <summary>
        /// 根据上下文创建特定类型的错误响应
        /// </summary>
        /// <param name="context">命令上下文</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="metadata">元数据</param>
        /// <param name="logger">可选的日志记录器（优先使用传入的logger，为空时使用静态logger）</param>
        /// <returns>特定类型的错误响应</returns>
        public static IResponse CreateSpecificErrorResponse(
            CommandContext context,
            string errorMessage,
            int errorCode = 500,
            Dictionary<string, object> metadata = null,
            ILogger logger = null)
        {
            if (context == null)
            {
                // 使用传入的logger，如果为null则使用静态logger
                (logger ?? _logger).LogDebug("创建特定错误响应失败：CommandContext为空");
                return CreateBasicErrorResponse(errorMessage, errorCode, metadata);
            }

            // 尝试使用上下文的辅助方法创建响应实例
            var response = context.CreateExpectedResponseInstance();
            if (response == null)
            {
                (logger ?? _logger).LogDebug("无法创建特定类型响应实例，使用基础错误响应。期望类型: {ExpectedType}",
                    context.ExpectedResponseTypeName);
                return CreateBasicErrorResponse(errorMessage, errorCode, metadata);
            }

            return InitializeErrorResponse(response, errorMessage, errorCode, metadata, logger);
        }

        /// <summary>
        /// 根据上下文和命令ID创建特定类型的错误响应（增强版）
        /// </summary>
        /// <param name="context">命令上下文</param>
        /// <param name="commandId">命令ID（用于回退）</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="metadata">元数据</param>
        /// <param name="logger">可选的日志记录器</param>
        /// <returns>特定类型的错误响应</returns>
        /// <summary>
        /// 根据上下文和命令ID创建特定类型的错误响应（增强版）
        /// </summary>
        /// <param name="context">命令上下文</param>
        /// <param name="commandId">命令ID（用于回退）</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="metadata">元数据</param>
        /// <param name="logger">可选的日志记录器（优先使用传入的logger，为空时使用静态logger）</param>
        /// <returns>特定类型的错误响应</returns>
        public static IResponse CreateSpecificErrorResponse(
            CommandContext context,
            CommandId commandId,
            string errorMessage,
            int errorCode = 500,
            Dictionary<string, object> metadata = null,
            ILogger logger = null)
        {
            var response = CreateSpecificErrorResponse(context, errorMessage, errorCode, metadata, logger);

            // 如果通过上下文创建失败，尝试根据命令ID创建特定类型的响应
            if (response is ResponseBase && context?.ExpectedResponseTypeName == null)
            {
                (logger ?? _logger).LogDebug("尝试根据命令ID创建特定错误响应: {CommandId}", commandId);
                response = CreateErrorResponseByCommandId(commandId, errorMessage, errorCode, metadata, logger);
            }

            return response;
        }

        /// <summary>
        /// 根据命令ID创建特定类型的错误响应（回退机制）
        /// </summary>
        private static IResponse CreateErrorResponseByCommandId(
            CommandId commandId,
            string errorMessage,
            int errorCode,
            Dictionary<string, object> metadata,
            ILogger logger)
        {
            try
            {
                // 根据命令ID映射到特定的响应类型
                Type responseType = GetResponseTypeByCommandId(commandId);
                if (responseType != null)
                {
                    var response = Activator.CreateInstance(responseType) as IResponse;
                    if (response != null)
                    {
                        return InitializeErrorResponse(response, errorMessage, errorCode, metadata, logger);
                    }
                }
            }
            catch (Exception ex)
            {
                (logger ?? _logger).LogError(ex, "根据命令ID创建错误响应失败: {CommandId}", commandId);
            }

            // 回退到基础错误响应
            return CreateBasicErrorResponse(errorMessage, errorCode, metadata);
        }

        /// <summary>
        /// 根据命令ID获取对应的响应类型
        /// </summary>
        private static Type GetResponseTypeByCommandId(CommandId commandId)
        {
            if (_commandResponseMap.TryGetValue(commandId.FullCode, out Type responseType))
            {
                return responseType;
            }

            // 可以根据命令类别进行通用映射
            return commandId.Category switch
            {
                CommandCategory.Authentication => typeof(LoginResponse),
                // 可以继续添加其他类别的默认响应类型
                _ => null
            };
        }

        /// <summary>
        /// 初始化错误响应对象
        /// </summary>
        private static IResponse InitializeErrorResponse(
            IResponse response,
            string errorMessage,
            int errorCode,
            Dictionary<string, object> metadata,
            ILogger logger)
        {
            try
            {
                // 设置通用错误属性
                if (response is ResponseBase baseResponse)
                {
                    baseResponse.IsSuccess = false;
                    baseResponse.ErrorMessage = errorMessage;
                    baseResponse.ErrorCode = errorCode;
                    baseResponse.Message = errorMessage;

                    // 添加元数据
                    if (metadata != null && metadata.Count > 0)
                    {
                        foreach (var kvp in metadata)
                        {
                            baseResponse = baseResponse.WithMetadata(kvp.Key, kvp.Value);
                        }
                    }
                }
                else
                {
                    // 对于非ResponseBase类型的IResponse实现，设置基本属性
                    response.IsSuccess = false;
                    response.ErrorMessage = errorMessage;
                    response.ErrorCode = errorCode;
                    response.Message = errorMessage;

                    // 尝试设置元数据
                    if (metadata != null)
                    {
                        response.Metadata ??= new Dictionary<string, object>();
                        foreach (var kvp in metadata)
                        {
                            response.Metadata[kvp.Key] = kvp.Value;
                        }
                    }
                }

                (logger ?? _logger).LogDebug("成功创建特定类型错误响应: {ResponseType}", response.GetType().Name);
                return response;
            }
            catch (Exception ex)
            {
                // 设置属性过程中出现异常，记录日志并返回基础错误响应
                (logger ?? _logger).LogError(ex, "设置特定类型错误响应属性失败: {ResponseType}", response.GetType().Name);
                return CreateBasicErrorResponse(errorMessage, errorCode, metadata);
            }
        }

        /// <summary>
        /// 创建基础错误响应
        /// </summary>
        private static IResponse CreateBasicErrorResponse(
            string errorMessage,
            int errorCode,
            Dictionary<string, object> metadata)
        {
            // 直接返回ResponseBase实例，避免递归调用
            var response = new ResponseBase
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Message = errorMessage,
                ErrorCode = errorCode,
                Timestamp = DateTime.Now,
                Metadata = new Dictionary<string, object>()
            };

            // 添加错误相关的元数据
            response.Metadata["ErrorSource"] = "Client";
            response.Metadata["ErrorTime"] = DateTime.Now;

            // 添加额外的元数据
            if (metadata != null)
            {
                foreach (var kvp in metadata)
                {
                    response.Metadata[kvp.Key] = kvp.Value;
                }
            }

            return response;
        }

        /// <summary>
        /// 创建特定类型的错误响应（通过异常对象）
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="ex">异常对象</param>
        /// <param name="errorMessage">自定义错误消息（如果为null，则使用异常消息）</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="additionalMetadata">额外的元数据</param>
        /// <returns>特定类型的错误响应</returns>
        public static TResponse CreateSpecificErrorResponse<TResponse>(
            Exception ex,
            string errorMessage = null,
            int errorCode = 500,
            Dictionary<string, object> additionalMetadata = null
           )
            where TResponse : IResponse
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            // 使用提供的错误消息，如果为null则使用异常消息
            string finalMessage = errorMessage ?? ex.Message;

            // 记录异常信息
            _logger.LogError(ex, "处理异常时创建错误响应: {ErrorMessage}", finalMessage);

            // 创建元数据并添加异常信息
            var metadata = additionalMetadata ?? new Dictionary<string, object>();
            metadata["ExceptionType"] = ex.GetType().FullName;
            metadata["ExceptionStackTrace"] = ex.StackTrace;

            // 如果存在内部异常，也添加其信息
            if (ex.InnerException != null)
            {
                metadata["InnerExceptionType"] = ex.InnerException.GetType().FullName;
                metadata["InnerExceptionMessage"] = ex.InnerException.Message;
            }

            // 调用现有的错误响应创建方法
            return CreateSpecificErrorResponse<TResponse>(finalMessage, errorCode, metadata);
        }

        /// <summary>
        /// 创建特定类型的成功响应
        /// </summary>
        public static TResponse CreateSuccessResponse<TResponse>(string message = "操作成功")
            where TResponse : IResponse, new()
        {
            var response = new TResponse
            {
                IsSuccess = true,
                Message = message,
                ErrorCode = 0,
                ErrorMessage = null
            };

            // 设置时间戳
            if (response is ResponseBase baseResponse)
            {
                baseResponse.Timestamp = DateTime.Now;
            }

            return response;
        }






        // 在 ResponseFactory 类中添加：
        /// <summary>
        /// 创建特定类型的错误响应（泛型版本，主要用于客户端）
        /// </summary>
        /// <summary>
        /// 创建特定类型的错误响应（泛型版本，主要用于客户端）
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="additionalMetadata">额外的元数据</param>
        /// <param name="logger">可选的日志记录器（优先使用传入的logger，为空时使用静态logger）</param>
        /// <returns>特定类型的错误响应</returns>
        public static TResponse CreateSpecificErrorResponse<TResponse>(
            string errorMessage,
            int errorCode = 500,
            Dictionary<string, object> additionalMetadata = null
           )
            where TResponse : IResponse
        {
            try
            {
                // 对于没有无参数构造函数的类型，我们需要提供一个更通用的解决方案
                // 在这里，我们简单地使用反射来创建实例
                TResponse response;
                try
                {
                    // 尝试创建实例
                    response = Activator.CreateInstance<TResponse>();
                }
                catch (MissingMethodException)
                {
                    // 如果泛型方法失败，尝试使用Type.GetType()和CreateInstance
                    response = (TResponse)Activator.CreateInstance(typeof(TResponse), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, null, null);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "创建 {ResponseType} 实例失败", typeof(TResponse).Name);
                    throw new InvalidOperationException($"无法创建响应实例: {typeof(TResponse).Name}", ex);
                }

                // 设置基本错误属性
                response.IsSuccess = false;
                response.ErrorMessage = errorMessage;
                response.Message = errorMessage;
                response.ErrorCode = errorCode;

                // 设置元数据
                if (response.Metadata == null)
                {
                    response.Metadata = new Dictionary<string, object>();
                }

                // 添加错误相关的元数据
                response.Metadata["ErrorSource"] = "Client";
                response.Metadata["ErrorTime"] = DateTime.Now;

                // 添加额外的元数据
                if (additionalMetadata != null)
                {
                    foreach (var kvp in additionalMetadata)
                    {
                        response.Metadata[kvp.Key] = kvp.Value;
                    }
                }

                _logger.LogDebug("成功创建特定类型错误响应: {ResponseType}", typeof(TResponse).Name);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建特定类型错误响应失败: {ResponseType}", typeof(TResponse).Name);

                // 对于无法创建的情况，尝试创建基础响应并转换
                var baseResponse = CreateBasicErrorResponse(errorMessage, errorCode, additionalMetadata);
                if (baseResponse is TResponse typedResponse)
                {
                    return typedResponse;
                }

                throw new InvalidOperationException($"无法创建类型 {typeof(TResponse).Name} 的错误响应实例", ex);
            }
        }




    }






}
