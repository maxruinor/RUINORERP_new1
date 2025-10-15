using System;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Validation;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 布尔值请求模型
    /// 用于处理布尔值相关的业务请求
    /// </summary>
    [Serializable]
    public class BooleanRequest : RequestBase
    {
        /// <summary>
        /// 布尔值参数
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <returns>布尔值</returns>
        public bool GetBoolValue()
        {
            return Value;
        }

        /// <summary>
        /// 设置布尔值
        /// </summary>
        /// <param name="value">布尔值</param>
        public void SetBoolValue(bool value)
        {
            Value = value;
        }

        /// <summary>
        /// 创建布尔请求实例
        /// </summary>
        /// <param name="value">布尔值</param>
        /// <returns>布尔请求实例</returns>
        public static BooleanRequest CreateBool(bool value)
        {
            return new BooleanRequest
            {
                Value = value,
                RequestId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 创建布尔请求实例（异步）
        /// </summary>
        /// <param name="value">布尔值</param>
        /// <returns>布尔请求实例</returns>
        public static async Task<BooleanRequest> CreateBoolAsync(bool value)
        {
            return await Task.FromResult(CreateBool(value));
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString()
        {
            return $"BooleanRequest: Value={Value}, RequestId={RequestId}";
        }
    }
}
