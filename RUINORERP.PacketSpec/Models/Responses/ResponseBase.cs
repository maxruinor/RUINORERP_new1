using MessagePack;
using RUINORERP.PacketSpec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 响应基类 - 提供所有响应的公共属性和方法
    /// </summary>
    [MessagePackObject]
    public class ResponseBase : IResponse
    {
        /// <summary>
        /// 业务级错误码；0 表示成功
        /// </summary>
        [Key(0)]
        public int ErrorCode { get; set; }

        /// <summary>
        /// 人类可读错误消息；Success 时可为空
        /// </summary>
        [Key(1)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        [Key(2)]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        [Key(3)]
        public string Message { get; set; }

        /// <summary>
        /// 响应时间戳
        /// </summary>
        [Key(4)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 请求标识
        /// </summary>
        [Key(6)]
        public string RequestId { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        [Key(7)]
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 扩展元数据（可选）
        /// </summary>
        [Key(8)]
        [MessagePack.IgnoreMember]
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ResponseBase()
        {
            Timestamp = DateTime.Now;
            Metadata = new Dictionary<string, object>();
        }


        /// <summary>
        /// 添加元数据
        /// </summary>
        /// <param name="key">元数据键</param>
        /// <param name="value">元数据值</param>
        /// <returns>当前实例</returns>
        public virtual ResponseBase WithMetadata(string key, object value)
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
        public virtual ResponseBase WithMetadata(Dictionary<string, object> metadata)
        {
            Metadata ??= new Dictionary<string, object>();
            foreach (var item in metadata)
            {
                Metadata[item.Key] = item.Value;
            }
            return this;
        }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <returns>响应实例</returns>
        public static ResponseBase CreateSuccess(string message = "操作成功")
        {
            return new ResponseBase
            {
                IsSuccess = true,
                Message = message,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="code">错误代码</param>
        /// <returns>响应实例</returns>
        public static ResponseBase CreateError(string message, int code = 500)
        {
            return new ResponseBase
            {
                IsSuccess = false,
                Message = message,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 从FluentValidation验证结果创建失败响应
        /// </summary>
        /// <param name="validationResult">FluentValidation验证结果</param>
        /// <param name="code">错误代码</param>
        /// <returns>响应实例</returns>
        public static ResponseBase CreateValidationError(FluentValidation.Results.ValidationResult validationResult, int code = 400)
        {
            if (validationResult == null || validationResult.IsValid)
                return CreateError("验证失败", code);

            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var message = string.Join("; ", errorMessages);
            
            var response = new ResponseBase
            {
                IsSuccess = false,
                Message = message,
                Timestamp = DateTime.Now
            };

            // 添加详细的验证错误信息到元数据
            response.WithMetadata("ValidationErrors", validationResult.Errors.Select(e => new 
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage,
                AttemptedValue = e.AttemptedValue
            }).ToList());

            return response;
        }
    }

    /// <summary>
    /// 泛型响应基类 - 提供所有响应的公共属性和方法，包含数据部分
    /// 专门用于承载业务实体数据，支持复杂查询和CRUD操作结果
    /// </summary>
    /// <typeparam name="TEntity">业务实体数据类型</typeparam>
    [MessagePackObject]
    public class ResponseBase<TEntity> : ResponseBase
    {
        /// <summary>
        /// 业务实体数据
        /// </summary>
        [Key(9)]
        public TEntity Data { get; set; }

        /// <summary>
        /// 数据总数（主要用于分页查询）
        /// </summary>
        [Key(10)]
        public int TotalCount { get; set; }

        /// <summary>
        /// 扩展数据字典（用于存放额外的业务数据）
        /// </summary>
        [Key(11)]
        public Dictionary<string, object> ExtraData { get; set; }

        /// <summary>
        /// 数据版本号（用于乐观锁和缓存控制）
        /// </summary>
        [Key(12)]
        public string DataVersion { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ResponseBase() : base()
        {
            ExtraData = new Dictionary<string, object>();
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public ResponseBase(bool success, string message, TEntity data = default(TEntity), int code = 200)
        {
            this.IsSuccess = success;
            this.Message = message;
            this.Data = data;
            this.Timestamp = DateTime.Now;
            this.ExtraData = new Dictionary<string, object>();
        }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="data">业务实体数据</param>
        /// <param name="message">成功消息</param>
        /// <param name="totalCount">数据总数（用于分页）</param>
        /// <param name="dataVersion">数据版本号</param>
        /// <returns>响应实例</returns>
        public static ResponseBase<TEntity> CreateSuccess(TEntity data, string message = "操作成功", int totalCount = 0, string dataVersion = null)
        {
            return new ResponseBase<TEntity>
            {
                Data = data,
                IsSuccess = true,
                Message = message,
                Timestamp = DateTime.Now,
                TotalCount = totalCount,
                DataVersion = dataVersion ?? DateTime.Now.Ticks.ToString()
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="extraData">扩展错误信息</param>
        /// <returns>响应实例</returns>
        public new static ResponseBase<TEntity> CreateError(string message, int code = 500, Dictionary<string, object> extraData = null)
        {
            return new ResponseBase<TEntity>
            {
                IsSuccess = false,
                Message = message,
                Timestamp = DateTime.Now,
                ExtraData = extraData ?? new Dictionary<string, object>()
            };
        }

        /// <summary>
        /// 创建失败响应（简化方法）
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="code">错误代码</param>
        /// <returns>响应实例</returns>
        public static ResponseBase<TEntity> Failure(string message, int code = 500)
        {
            return new ResponseBase<TEntity>
            {
                IsSuccess = false,
                Message = message,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 从FluentValidation验证结果创建失败响应
        /// </summary>
        /// <param name="validationResult">FluentValidation验证结果</param>
        /// <param name="code">错误代码</param>
        /// <returns>响应实例</returns>
        public static ResponseBase<TEntity> CreateValidationError(FluentValidation.Results.ValidationResult validationResult, int code = 400)
        {
            if (validationResult == null || validationResult.IsValid)
                return Failure("验证失败", code);

            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var message = string.Join("; ", errorMessages);
            
            var extraData = new Dictionary<string, object>
            {
                ["ValidationErrors"] = validationResult.Errors.Select(e => new 
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage,
                    AttemptedValue = e.AttemptedValue
                }).ToList()
            };

            return new ResponseBase<TEntity>
            {
                IsSuccess = false,
                Message = message,
                Timestamp = DateTime.Now,
                ExtraData = extraData
            };
        }

        /// <summary>
        /// 创建分页查询成功响应
        /// </summary>
        /// <param name="data">实体数据列表</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="message">成功消息</param>
        /// <returns>响应实例</returns>
        public static ResponseBase<TEntity> CreatePagedSuccess(TEntity data, int totalCount, int pageIndex, int pageSize, string message = "查询成功")
        {
            var extraData = new Dictionary<string, object>
            {
                ["PageIndex"] = pageIndex,
                ["PageSize"] = pageSize,
                ["HasNextPage"] = (pageIndex + 1) * pageSize < totalCount,
                ["HasPreviousPage"] = pageIndex > 0
            };

            return new ResponseBase<TEntity>
            {
                Data = data,
                IsSuccess = true,
                Message = message,
                Timestamp = DateTime.Now,
                TotalCount = totalCount,
                ExtraData = extraData
            };
        }

        /// <summary>
        /// 添加扩展数据
        /// </summary>
        /// <param name="key">数据键</param>
        /// <param name="value">数据值</param>
        /// <returns>当前实例</returns>
        public new ResponseBase<TEntity> WithMetadata(string key, object value)
        {
            ExtraData ??= new Dictionary<string, object>();
            ExtraData[key] = value;
            return this;
        }

        /// <summary>
        /// 批量添加扩展数据
        /// </summary>
        /// <param name="metadata">元数据字典</param>
        /// <returns>当前实例</returns>
        public new ResponseBase<TEntity> WithMetadata(Dictionary<string, object> metadata)
        {
            if (metadata == null) return this;
            
            ExtraData ??= new Dictionary<string, object>();
            foreach (var item in metadata)
            {
                ExtraData[item.Key] = item.Value;
            }
            return this;
        }
    }

 
}
