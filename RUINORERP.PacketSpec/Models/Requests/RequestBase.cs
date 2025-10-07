using RUINORERP.PacketSpec.Core;
using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 请求基类 - 提供所有请求的公共属性和方法
    /// </summary>
    [Serializable]
    public abstract class RequestBase : IRequest
    {
        /// <summary>
        /// 请求唯一标识
        /// </summary>
        public string RequestId { get; set; }


        /// <summary>
        /// 请求操作类型
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 请求时间戳（UTC时间）
        /// </summary>
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// 客户端信息
        /// </summary>
        public string ClientInfo { get; set; }

        /// <summary>
        /// 扩展元数据（可选）
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected RequestBase()
        {
            RequestId = Guid.NewGuid().ToString();
            TimestampUtc = DateTime.UtcNow;
            Metadata = new Dictionary<string, object>();
        }

        /// <summary>
        /// 设置请求标识
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>当前实例</returns>
        public virtual RequestBase WithRequestId(string requestId)
        {
            RequestId = requestId;
            return this;
        }

        /// <summary>
        /// 设置操作类型
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <returns>当前实例</returns>
        public virtual RequestBase WithOperationType(string operationType)
        {
            OperationType = operationType;
            return this;
        }

        /// <summary>
        /// 设置客户端信息
        /// </summary>
        /// <param name="clientInfo">客户端信息</param>
        /// <returns>当前实例</returns>
        public virtual RequestBase WithClientInfo(string clientInfo)
        {
            ClientInfo = clientInfo;
            return this;
        }

        /// <summary>
        /// 添加元数据
        /// </summary>
        /// <param name="key">元数据键</param>
        /// <param name="value">元数据值</param>
        /// <returns>当前实例</returns>
        public virtual RequestBase WithMetadata(string key, object value)
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
        public virtual RequestBase WithMetadata(Dictionary<string, object> metadata)
        {
            Metadata ??= new Dictionary<string, object>();
            foreach (var item in metadata)
            {
                Metadata[item.Key] = item.Value;
            }
            return this;
        }
    }
}
