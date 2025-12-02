using System;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.PacketSpec.Models.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using Polly;
using RUINORERP.PacketSpec.Errors; // 添加统一错误代码的引用

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 响应工厂 - 统一创建各种类型的响应对象
    /// 支持特定类型的错误响应创建，供服务器和客户端共同使用
    /// </summary>
    public static class ResponseFactory
    {

        /// <summary>
        /// 默认的命令处理器注册表
        /// </summary>
        private static CommandHandlerRegistry _defaultRegistry;


        /// <summary>
        /// 设置默认的命令处理器注册表，用于响应类型缓存
        /// </summary>
        /// <param name="registry">命令处理器注册表实例</param>
        public static void SetDefaultCommandHandlerRegistry(CommandHandlerRegistry registry)
        {
            lock (_registryLock)
            {
                _defaultRegistry = registry;
                _logger?.Debug("响应工厂默认命令处理器注册表已设置");
            }
        }


        /// <summary>
        /// 静态日志记录器
        /// </summary>
        private static readonly ILogger _logger;


        /// <summary>
        /// 注册表访问锁
        /// </summary>
        private static readonly object _registryLock = new object();


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
        public static IResponse CreateSpecificSuccessResponse(
               CommandContext commandContext,
             string message = "操作成功",
             Dictionary<string, object> additionalMetadata = null
           )
        {
            // 使用与错误方法相同的实例化逻辑
            IResponse lastResponse = null;
            string responseTypeName = commandContext.ExpectedResponseTypeName;
            try
            {
                if (commandContext != null && !string.IsNullOrEmpty(commandContext.ExpectedResponseTypeName))
                {
                    responseTypeName = commandContext.ExpectedResponseTypeName;
                }
                // 如果成功解析了响应类型名称，使用注册表创建实例
                if (!string.IsNullOrEmpty(responseTypeName))
                {
                    var instance = _defaultRegistry?.CreateResponseInstance(commandContext);
                    if (instance is IResponse response)
                    {

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

                        _logger.LogDebug("成功创建特定类型成功响应: {ResponseType}", responseTypeName);
                        lastResponse = response;
                    }
                }
            }
            catch (Exception ex)

            {
                _logger.LogError(ex, "创建特定类型成功响应失败: {ResponseType}", responseTypeName);
                throw new InvalidOperationException($"无法创建类型 {responseTypeName} 的成功响应实例", ex);
            }
            return lastResponse;
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
        public static IResponse CreateSpecificErrorResponse(
            CommandContext executionContext,
            Exception ex,
            string errorMessage = null,
            Dictionary<string, object> additionalMetadata = null
           )
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
            var errorResponse = CreateSpecificErrorResponse(executionContext, finalMessage, metadata);
            // 确保响应不为空
            if (errorResponse == null)
            {
                _logger.LogWarning("通过异常创建错误响应失败，返回默认ResponseBase对象");
                errorResponse = new ResponseBase
                {
                    IsSuccess = false,
                    ErrorMessage = finalMessage,
                    Message = finalMessage,
                    Metadata = metadata
                };
            }
            return errorResponse;
        }


        /// <summary>
        /// 创建特定类型的错误响应（支持传入PacketModel）
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="packetModel">数据包模型</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="additionalMetadata">额外的元数据</param>
        /// <returns>特定类型的错误响应</returns>
        public static IResponse CreateSpecificErrorResponse(
            PacketModel packetModel,
            string errorMessage,
            int errorCode = 500,
            Dictionary<string, object> additionalMetadata = null
           )
        {
            errorMessage += "指令信息: " + packetModel.CommandId.ToString();
            return CreateSpecificErrorResponse(packetModel.ExecutionContext, errorMessage, additionalMetadata);
        }


        /// <summary>
        /// 创建特定类型的错误响应（主要实现方法，支持多种参数组合）
        /// </summary>
        /// <typeparam name="TResponse">响应类型,不能是接口</typeparam>
        /// <param name="packetModel">数据包模型</param>
        /// <param name="context">命令上下文</param>
        /// <param name="responseTypeName">响应类型名称</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="additionalMetadata">额外的元数据</param>
        /// <returns>特定类型的错误响应</returns>
        public static IResponse CreateSpecificErrorResponse(
            CommandContext context,
            string errorMessage,
            Dictionary<string, object> additionalMetadata = null
        )
        {
            IResponse response = null;
            // 获取CommandHandlerRegistry实例
            string responseTypeName = context.ExpectedResponseTypeName;
            if (context != null && !string.IsNullOrEmpty(context.ExpectedResponseTypeName))
            {
                responseTypeName = context.ExpectedResponseTypeName;
            }
            if (responseTypeName == null)
            {
                _logger.LogDebug("返回类型为空!");
                responseTypeName = "ResponseBase";
                context.ExpectedResponseTypeName= "ResponseBase";
            }
            // 如果成功解析了响应类型名称，使用注册表创建实例
            if (!string.IsNullOrEmpty(responseTypeName))
            {
                try
                {
                    var instance = _defaultRegistry?.CreateResponseInstance(context);
                    if (instance is IResponse newResponse)
                    {
                        // 初始化错误响应
                        newResponse.IsSuccess = false;
                        newResponse.ErrorMessage = errorMessage;
                        newResponse.Message = errorMessage;

                        // 设置元数据
                        if (newResponse.Metadata == null)
                        {
                            newResponse.Metadata = new Dictionary<string, object>();
                        }

                        // 添加错误相关的元数据
                        newResponse.Metadata["ErrorSource"] = "";
                        newResponse.Metadata["ErrorTime"] = DateTime.Now;
                        newResponse.ErrorMessage = $"";

                        // 添加额外的元数据
                        if (additionalMetadata != null)
                        {
                            foreach (var kvp in additionalMetadata)
                            {
                                newResponse.Metadata[kvp.Key] = kvp.Value;
                            }
                        }
                        response = newResponse;

                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但继续尝试其他方法
                    _logger.LogWarning(ex, "使用解析的类型名称创建响应失败: {ResponseTypeName}", responseTypeName);
                }
            }

            // 确保即使创建特定类型失败，也返回基本的ResponseBase对象
            if (response == null)
            {
                _logger.LogWarning("创建特定类型响应失败，返回默认ResponseBase对象");
                response = new ResponseBase
                {
                    IsSuccess = false,
                    ErrorMessage = errorMessage,
                    Message = errorMessage,
                    Metadata = additionalMetadata ?? new Dictionary<string, object> { { "ErrorSource", "ResponseFactory" }, { "ErrorTime", DateTime.Now } }
                };
            }
            return response;
        }






        /// <summary>
        /// 创建特定类型的错误响应（主要实现方法，支持多种参数组合）
        /// </summary>
        /// <typeparam name="TResponse">响应类型,不能是接口</typeparam>
        /// <param name="packetModel">数据包模型</param>
        /// <param name="context">命令上下文</param>
        /// <param name="responseTypeName">响应类型名称</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="additionalMetadata">额外的元数据</param>
        /// <returns>特定类型的错误响应</returns>
        public static TResponse CreateSpecificErrorResponse<TResponse>(
            string errorMessage,
            Dictionary<string, object> additionalMetadata = null
        ) where TResponse : class, IResponse
        {
            TResponse response = null;
            // 获取CommandHandlerRegistry实例
            string responseTypeName = typeof(TResponse).Name;
            // 如果成功解析了响应类型名称，使用注册表创建实例
            if (!string.IsNullOrEmpty(responseTypeName))
            {
                try
                {
                    var instance = _defaultRegistry?.CreateInstance(typeof(TResponse));
                    if (instance is TResponse newResponse)
                    {
                        // 初始化错误响应
                        newResponse.IsSuccess = false;
                        newResponse.ErrorMessage = errorMessage;
                        newResponse.Message = errorMessage;

                        // 设置元数据
                        if (newResponse.Metadata == null)
                        {
                            newResponse.Metadata = new Dictionary<string, object>();
                        }

                        // 添加错误相关的元数据
                        newResponse.Metadata["ErrorSource"] = "Client";
                        newResponse.Metadata["ErrorTime"] = DateTime.Now;

                        // 添加额外的元数据
                        if (additionalMetadata != null)
                        {
                            foreach (var kvp in additionalMetadata)
                            {
                                newResponse.Metadata[kvp.Key] = kvp.Value;
                            }
                        }
                        response = newResponse;

                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但继续尝试其他方法
                    _logger.LogWarning(ex, "使用解析的类型名称创建响应失败: {ResponseTypeName}", responseTypeName);
                }
            }

            // 确保即使创建特定类型失败，也返回基本的ResponseBase对象
            if (response == null)
            {
            }
            return response;
        }

     
    }
}
