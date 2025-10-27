using RUINORERP.PacketSpec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 响应基类 - 提供所有响应的公共属性和方法
    /// Message: 用于传达操作结果的通用信息，无论成功还是失败都应该有值
    /// ErrorMessage: 专门用于存储详细的错误信息，只有在失败时才有意义，成功时应为null
    /// </summary>
    public class ResponseBase : IResponse
    {
        /// <summary>
        /// 业务级错误码；0 表示成功
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 详细的错误信息；仅在操作失败时设置，成功时为null
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 操作结果的通用描述信息；成功或失败时都应提供有意义的描述
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 请求标识
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 扩展元数据（可选）
        /// </summary>
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
     
    }

    /// <summary>
    /// 泛型响应基类 - 提供所有响应的公共属性和方法，包含数据部分
    /// 专门用于承载业务实体数据，支持复杂查询和CRUD操作结果
    /// Message: 用于传达操作结果的通用信息，无论成功还是失败都应该有值
    /// ErrorMessage: 专门用于存储详细的错误信息，只有在失败时才有意义，成功时应为null
    /// </summary>
    /// <typeparam name="TEntity">业务实体数据类型</typeparam>
    public class ResponseBase<TEntity> : ResponseBase
    {
        /// <summary>
        /// 业务实体数据
        /// </summary>
        public TEntity Data { get; set; }

        /// <summary>
        /// 数据总数（主要用于分页查询）
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 扩展数据字典（用于存放额外的业务数据）
        /// </summary>
        public Dictionary<string, object> ExtraData { get; set; }

        /// <summary>
        /// 数据版本号（用于乐观锁和缓存控制）
        /// </summary>
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