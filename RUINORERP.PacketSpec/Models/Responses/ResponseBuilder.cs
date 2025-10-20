using MessagePack;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 响应数据构建器 - 简化响应对象的创建和配置
    /// 提供统一的接口来创建各种类型的响应，包括成功响应、错误响应、分页响应等
    /// </summary>
    public static class ResponseBuilder
    {
        #region 非泛型响应构建方法
        
        /// <summary>
        /// 创建默认的成功响应
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="requestId">请求ID（可选）</param>
        /// <returns>成功响应实例</returns>
        public static ResponseBase Success(string message = "操作成功", string requestId = null)
        {
            return new ResponseBase
            {
                IsSuccess = true,
                Message = message,
                Timestamp = DateTime.Now,
                RequestId = requestId,
                ErrorCode = 0,
                ErrorMessage = null
            };
        }

        /// <summary>
        /// 创建错误响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="requestId">请求ID（可选）</param>
        /// <returns>错误响应实例</returns>
        public static ResponseBase Error(string message, int errorCode = 500, string requestId = null)
        {
            return new ResponseBase
            {
                IsSuccess = false,
                Message = message,
                Timestamp = DateTime.Now,
                RequestId = requestId,
                ErrorCode = errorCode,
                ErrorMessage = message
            };
        }

        /// <summary>
        /// 从异常创建错误响应
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="requestId">请求ID（可选）</param>
        /// <param name="includeStackTrace">是否包含堆栈信息</param>
        /// <returns>错误响应实例</returns>
        public static ResponseBase FromException(Exception ex, int errorCode = 500, string requestId = null, bool includeStackTrace = false)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));
                
            var response = Error(ex.Message, errorCode, requestId);
            
            if (includeStackTrace)
            {
                response.WithMetadata("StackTrace", ex.StackTrace);
            }
            
            response.WithMetadata("ExceptionType", ex.GetType().FullName);
            
            return response;
        }

        /// <summary>
        /// 创建验证错误响应
        /// </summary>
        /// <param name="validationResult">FluentValidation验证结果</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="requestId">请求ID（可选）</param>
        /// <returns>验证错误响应实例</returns>
        public static ResponseBase ValidationError(FluentValidation.Results.ValidationResult validationResult, int errorCode = 400, string requestId = null)
        {
            if (validationResult == null || validationResult.IsValid)
                return Error("验证失败", errorCode, requestId);

            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var message = string.Join("；", errorMessages);
            
            var response = Error(message, errorCode, requestId);

            // 添加详细的验证错误信息到元数据
            response.WithMetadata("ValidationErrors", validationResult.Errors.Select(e => new 
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage,
                AttemptedValue = e.AttemptedValue
            }).ToList());

            return response;
        }
        
        #endregion

        #region 泛型响应构建方法
        
        /// <summary>
        /// 创建成功的泛型响应
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="data">响应数据</param>
        /// <param name="message">成功消息</param>
        /// <param name="requestId">请求ID（可选）</param>
        /// <returns>成功的泛型响应实例</returns>
        public static ResponseBase<TData> Success<TData>(TData data, string message = "操作成功", string requestId = null)
        {
            return new ResponseBase<TData>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                Timestamp = DateTime.Now,
                RequestId = requestId,
                ErrorCode = 0,
                ErrorMessage = null,
                DataVersion = DateTime.Now.Ticks.ToString()
            };
        }

        /// <summary>
        /// 创建错误的泛型响应
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="message">错误消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="requestId">请求ID（可选）</param>
        /// <param name="extraData">额外错误信息</param>
        /// <returns>错误的泛型响应实例</returns>
        public static ResponseBase<TData> Error<TData>(string message, int errorCode = 500, string requestId = null, Dictionary<string, object> extraData = null)
        {
            return new ResponseBase<TData>
            {
                IsSuccess = false,
                Message = message,
                Timestamp = DateTime.Now,
                RequestId = requestId,
                ErrorCode = errorCode,
                ErrorMessage = message,
                ExtraData = extraData ?? new Dictionary<string, object>()
            };
        }

        /// <summary>
        /// 从异常创建错误的泛型响应
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="ex">异常对象</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="requestId">请求ID（可选）</param>
        /// <param name="includeStackTrace">是否包含堆栈信息</param>
        /// <returns>错误的泛型响应实例</returns>
        public static ResponseBase<TData> FromException<TData>(Exception ex, int errorCode = 500, string requestId = null, bool includeStackTrace = false)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));
                
            var extraData = new Dictionary<string, object>
            {
                ["ExceptionType"] = ex.GetType().FullName
            };
            
            if (includeStackTrace)
            {
                extraData["StackTrace"] = ex.StackTrace;
            }
            
            return Error<TData>(ex.Message, errorCode, requestId, extraData);
        }

        /// <summary>
        /// 创建验证错误的泛型响应
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="validationResult">FluentValidation验证结果</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="requestId">请求ID（可选）</param>
        /// <returns>验证错误的泛型响应实例</returns>
        public static ResponseBase<TData> ValidationError<TData>(FluentValidation.Results.ValidationResult validationResult, int errorCode = 400, string requestId = null)
        {
            if (validationResult == null || validationResult.IsValid)
                return Error<TData>("验证失败", errorCode, requestId);

            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var message = string.Join("；", errorMessages);
            
            var extraData = new Dictionary<string, object>
            {
                ["ValidationErrors"] = validationResult.Errors.Select(e => new 
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage,
                    AttemptedValue = e.AttemptedValue
                }).ToList()
            };

            return Error<TData>(message, errorCode, requestId, extraData);
        }

        /// <summary>
        /// 创建分页响应
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="data">分页数据</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="message">成功消息</param>
        /// <param name="requestId">请求ID（可选）</param>
        /// <returns>分页响应实例</returns>
        public static ResponseBase<TData> Paged<TData>(TData data, int totalCount, int pageIndex, int pageSize, string message = "查询成功", string requestId = null)
        {
            var extraData = new Dictionary<string, object>
            {
                ["PageIndex"] = pageIndex,
                ["PageSize"] = pageSize,
                ["HasNextPage"] = (pageIndex + 1) * pageSize < totalCount,
                ["HasPreviousPage"] = pageIndex > 0,
                ["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            return new ResponseBase<TData>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                Timestamp = DateTime.Now,
                RequestId = requestId,
                TotalCount = totalCount,
                ExtraData = extraData,
                DataVersion = DateTime.Now.Ticks.ToString()
            };
        }

        #endregion

        #region 辅助方法
        
        /// <summary>
        /// 将请求转换为相应的响应
        /// 复制请求ID等信息到响应中
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="responseFactory">响应工厂函数</param>
        /// <returns>配置好的响应对象</returns>
        public static TResponse FromRequest<TResponse>(IRequest request, Func<TResponse> responseFactory)
            where TResponse : ResponseBase
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (responseFactory == null)
                throw new ArgumentNullException(nameof(responseFactory));
                
            var response = responseFactory();
            response.RequestId = request.RequestId;
            
            return response;
        }
        
        /// <summary>
        /// 创建带有执行时间信息的响应
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="startTime">开始时间</param>
        /// <param name="responseFactory">响应工厂函数</param>
        /// <returns>包含执行时间的响应对象</returns>
        public static TResponse WithExecutionTime<TResponse>(DateTime startTime, Func<TResponse> responseFactory)
            where TResponse : ResponseBase
        {
            if (responseFactory == null)
                throw new ArgumentNullException(nameof(responseFactory));
                
            var response = responseFactory();
            response.ExecutionTimeMs = (long)(DateTime.Now - startTime).TotalMilliseconds;
            
            return response;
        }
        
        #endregion
    }
}