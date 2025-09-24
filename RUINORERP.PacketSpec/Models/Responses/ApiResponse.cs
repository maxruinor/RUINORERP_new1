﻿using System;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 分页信息
    /// </summary>
    [Serializable]
    public class PaginationInfo
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPrevious => Page > 1;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNext => Page < TotalPages;

        /// <summary>
        /// 当前页记录数
        /// </summary>
        public int CurrentCount { get; set; }

        /// <summary>
        /// 起始记录索引
        /// </summary>
        public long StartIndex => (Page - 1) * PageSize + 1;

        /// <summary>
        /// 结束记录索引
        /// </summary>
        public long EndIndex => StartIndex + CurrentCount - 1;
    }
    /// <summary>
    /// API响应基类
    /// 用于统一表示API请求的响应格式
    /// 包含状态码、消息、时间戳等公共字段
    /// 客户端和服务器端共享使用
    /// </summary>
    [Serializable]
    public abstract class ApiResponseBase
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应时间戳（UTC时间）
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 响应代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 请求标识
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 扩展元数据（可选）
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected ApiResponseBase()
        {
            Timestamp = DateTime.UtcNow;
            Code = 200;
            RequestId = Guid.NewGuid().ToString();
            Metadata = new Dictionary<string, object>();
        }

        /// <summary>
        /// 设置请求标识
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>当前实例</returns>
        public ApiResponseBase WithRequestId(string requestId)
        {
            RequestId = requestId;
            return this;
        }

        /// <summary>
        /// 设置响应代码
        /// </summary>
        /// <param name="code">响应代码</param>
        /// <returns>当前实例</returns>
        public ApiResponseBase WithCode(int code)
        {
            Code = code;
            return this;
        }

        /// <summary>
        /// 添加元数据
        /// </summary>
        /// <param name="key">元数据键</param>
        /// <param name="value">元数据值</param>
        /// <returns>当前实例</returns>
        public ApiResponseBase WithMetadata(string key, object value)
        {
            Metadata ??= new Dictionary<string, object>();
            Metadata[key] = value;
            return this;
        }

        /// <summary>
        /// 批量添加元数据
        /// </summary>
        /// <param name="metadata">元数据字典</param>
        /// <returns>当前实例</returns>
        public ApiResponseBase WithMetadata(Dictionary<string, object> metadata)
        {
            Metadata ??= new Dictionary<string, object>();
            foreach (var item in metadata)
            {
                Metadata[item.Key] = item.Value;
            }
            return this;
        }

        /// <summary>
        /// 验证响应有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return Timestamp <= DateTime.UtcNow.AddMinutes(5) &&
                   Timestamp >= DateTime.UtcNow.AddMinutes(-5) &&
                   !string.IsNullOrEmpty(Message);
        }

        /// <summary>
        /// 检查响应是否成功
        /// </summary>
        /// <returns>是否成功</returns>
        public bool IsSuccess()
        {
            return Success && Code >= 200 && Code < 300;
        }

        /// <summary>
        /// 获取元数据值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="key">元数据键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>元数据值</returns>
        public TValue GetMetadata<TValue>(string key, TValue defaultValue = default)
        {
            if (Metadata != null && Metadata.TryGetValue(key, out var value) && value is TValue typedValue)
            {
                return typedValue;
            }
            return defaultValue;
        }

        /// <summary>
        /// 从CommandResult创建API响应
        /// </summary>
        /// <param name="commandResult">命令结果</param>
        /// <returns>API响应基类实例</returns>
        public virtual ApiResponseBase FromCommandResult(CommandResult commandResult)
        {
            Success = commandResult.IsSuccess;
            Message = commandResult.IsSuccess ? "操作成功" : commandResult.Message;
            Code = commandResult.IsSuccess ? 200 : (int.TryParse(commandResult.ErrorCode, out int code) ? code : 500);

            if (commandResult.ExtraData != null && commandResult.ExtraData.Count > 0)
            {
                WithMetadata(commandResult.ExtraData);
            }

            if (!string.IsNullOrEmpty(commandResult.CommandId))
            {
                WithRequestId(commandResult.CommandId);
            }

            return this;
        }

        /// <summary>
        /// 转换为JSON字符串
        /// </summary>
        /// <param name="formatting">格式化选项</param>
        /// <returns>JSON字符串</returns>
        public string ToJson(Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>响应信息字符串</returns>
        public override string ToString()
        {
            return $"ApiResponse[Success:{Success}, Code:{Code}, Message:{Message}]";
        }
    }

    /// <summary>
    /// 统一API响应模型 - 替代原有的BaseResponse和各类响应类
    /// 提供标准化的成功/失败响应结构，支持分页、元数据等扩展功能
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [Serializable]
    public class ApiResponse<T> : ApiResponseBase
    {
        #region 核心属性
        
        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 分页信息（可选）
        /// </summary>
        public PaginationInfo Pagination { get; set; }

        #endregion

        #region 构造函数
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ApiResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">响应消息</param>
        /// <param name="data">响应数据</param>
        /// <param name="code">响应代码</param>
        public ApiResponse(bool success, string message, T data = default, int code = 200) : base()
        {
            Success = success;
            Message = message;
            Data = data;
            Code = code;
        }

        #endregion

        #region 静态创建方法
        
        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <param name="message">成功消息</param>
        /// <returns>成功响应实例</returns>
        public static ApiResponse<T> CreateSuccess(T data, string message = "操作成功")
        {
            return new ApiResponse<T>(true, message, data, 200);
        }

        /// <summary>
        /// 创建带分页的成功响应
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="message">成功消息</param>
        /// <returns>成功响应实例</returns>
        public static ApiResponse<T> CreateSuccess(T data, long totalCount, int page, int pageSize, string message = "操作成功")
        {
            var response = new ApiResponse<T>(true, message, data, 200);
            response.WithPagination(totalCount, page, pageSize);
            return response;
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="data">可选数据</param>
        /// <returns>失败响应实例</returns>
        public static ApiResponse<T> Failure(string message, int code = 500, T data = default)
        {
            return new ApiResponse<T>(false, message, data, code);
        }

        /// <summary>
        /// 创建未授权响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>未授权响应实例</returns>
        public static ApiResponse<T> Unauthorized(string message = "未授权访问")
        {
            return new ApiResponse<T>(false, message, default, 401);
        }

        /// <summary>
        /// 创建未找到响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>未找到响应实例</returns>
        public static ApiResponse<T> NotFound(string message = "资源未找到")
        {
            return new ApiResponse<T>(false, message, default, 404);
        }

        /// <summary>
        /// 创建验证失败响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>验证失败响应实例</returns>
        public static ApiResponse<T> ValidationFailed(string message = "数据验证失败")
        {
            return new ApiResponse<T>(false, message, default, 400);
        }

        /// <summary>
        /// 创建禁止访问响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>禁止访问响应实例</returns>
        public static ApiResponse<T> Forbidden(string message = "禁止访问")
        {
            return new ApiResponse<T>(false, message, default, 403);
        }

        /// <summary>
        /// 创建服务不可用响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>服务不可用响应实例</returns>
        public static ApiResponse<T> ServiceUnavailable(string message = "服务暂时不可用")
        {
            return new ApiResponse<T>(false, message, default, 503);
        }

        /// <summary>
        /// 从CommandResult创建API响应
        /// </summary>
        /// <param name="commandResult">命令结果</param>
        /// <returns>API响应实例</returns>
        public static ApiResponse<T> FromCommandResult(CommandResult commandResult)
        {
            var response = new ApiResponse<T>();
            // 改为调用基类的实例方法
            ((ApiResponseBase)response).FromCommandResult(commandResult);
            
            if (commandResult.Data is T data)
            {
                response.Data = data;
            }
            
            return response;
        }

        #endregion

        #region 实例方法
        
        /// <summary>
        /// 设置请求标识（覆盖基类方法以支持链式调用）
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>当前实例</returns>
        public new ApiResponse<T> WithRequestId(string requestId)
        {
            base.WithRequestId(requestId);
            return this;
        }

        /// <summary>
        /// 设置响应代码（覆盖基类方法以支持链式调用）
        /// </summary>
        /// <param name="code">响应代码</param>
        /// <returns>当前实例</returns>
        public new ApiResponse<T> WithCode(int code)
        {
            base.WithCode(code);
            return this;
        }

        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="totalCount">总记录数</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>当前实例</returns>
        public ApiResponse<T> WithPagination(long totalCount, int page, int pageSize)
        {
            Pagination = new PaginationInfo
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
            return this;
        }

        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="pagination">分页信息对象</param>
        /// <returns>当前实例</returns>
        public ApiResponse<T> WithPagination(PaginationInfo pagination)
        {
            Pagination = pagination;
            return this;
        }

        /// <summary>
        /// 添加元数据（覆盖基类方法以支持链式调用）
        /// </summary>
        /// <param name="key">元数据键</param>
        /// <param name="value">元数据值</param>
        /// <returns>当前实例</returns>
        public new ApiResponse<T> WithMetadata(string key, object value)
        {
            base.WithMetadata(key, value);
            return this;
        }

        /// <summary>
        /// 批量添加元数据（覆盖基类方法以支持链式调用）
        /// </summary>
        /// <param name="metadata">元数据字典</param>
        /// <returns>当前实例</returns>
        public new ApiResponse<T> WithMetadata(Dictionary<string, object> metadata)
        {
            base.WithMetadata(metadata);
            return this;
        }

        /// <summary>
        /// 从JSON字符串创建响应
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>响应实例</returns>
        public static ApiResponse<T> FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ApiResponse<T>>(json);
        }

        #endregion

        #region 重写方法
        
        /// <summary>
        /// 转换为字符串表示（覆盖基类方法）
        /// </summary>
        /// <returns>响应信息字符串</returns>
        public override string ToString()
        {
            return $"ApiResponse[Success:{Success}, Code:{Code}, Message:{Message}, DataType:{typeof(T).Name}]";
        }

        #endregion
    }

    /// <summary>
    /// 无数据类型的API响应（用于void操作）
    /// </summary>
    [Serializable]
    public class ApiResponse : ApiResponseBase
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ApiResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">响应消息</param>
        /// <param name="code">响应代码</param>
        public ApiResponse(bool success, string message, int code = 200) : base()
        {
            Success = success;
            Message = message;
            Code = code;
        }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <returns>成功响应实例</returns>
        public static ApiResponse CreateSuccess(string message = "操作成功")
        {
            return new ApiResponse(true, message, 200);
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <returns>失败响应实例</returns>
        public static ApiResponse Failure(string message, int code = 500)
        {
            return new ApiResponse(false, message, code);
        }

        /// <summary>
        /// 创建未授权响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>未授权响应实例</returns>
        public static new ApiResponse Unauthorized(string message = "未授权访问")
        {
            return new ApiResponse(false, message, 401);
        }

        /// <summary>
        /// 创建未找到响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>未找到响应实例</returns>
        public static new ApiResponse NotFound(string message = "资源未找到")
        {
            return new ApiResponse(false, message, 404);
        }

        /// <summary>
        /// 创建验证失败响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>验证失败响应实例</returns>
        public static new ApiResponse ValidationFailed(string message = "数据验证失败")
        {
            return new ApiResponse(false, message, 400);
        }

        /// <summary>
        /// 创建禁止访问响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>禁止访问响应实例</returns>
        public static ApiResponse Forbidden(string message = "禁止访问")
        {
            return new ApiResponse(false, message, 403);
        }

        

        #region 实例方法

        /// <summary>
        /// 设置请求标识（覆盖基类方法以支持链式调用）
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>当前实例</returns>
        public new ApiResponse WithRequestId(string requestId)
        {
            base.WithRequestId(requestId);
            return this;
        }

        /// <summary>
        /// 设置响应代码（覆盖基类方法以支持链式调用）
        /// </summary>
        /// <param name="code">响应代码</param>
        /// <returns>当前实例</returns>
        public new ApiResponse WithCode(int code)
        {
            base.WithCode(code);
            return this;
        }

        /// <summary>
        /// 添加元数据（覆盖基类方法以支持链式调用）
        /// </summary>
        /// <param name="key">元数据键</param>
        /// <param name="value">元数据值</param>
        /// <returns>当前实例</returns>
        public new ApiResponse WithMetadata(string key, object value)
        {
            base.WithMetadata(key, value);
            return this;
        }

        /// <summary>
        /// 批量添加元数据（覆盖基类方法以支持链式调用）
        /// </summary>
        /// <param name="metadata">元数据字典</param>
        /// <returns>当前实例</returns>
        public new ApiResponse WithMetadata(Dictionary<string, object> metadata)
        {
            base.WithMetadata(metadata);
            return this;
        }

        /// <summary>
        /// 从JSON字符串创建响应
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>响应实例</returns>
        public static ApiResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ApiResponse>(json);
        }

        #endregion

        #region 重写方法

        /// <summary>
        /// 转换为字符串表示（覆盖基类方法）
        /// </summary>
        /// <returns>响应信息字符串</returns>
        public override string ToString()
        {
            return $"ApiResponse[Success:{Success}, Code:{Code}, Message:{Message}]";
        }

        #endregion
    }

}
