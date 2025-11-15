using System;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Validation;
using SqlSugar.Extensions;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 数值请求模型
    /// 用于处理数值相关的业务请求
    /// </summary>
    [Serializable]
    public class NumericRequest : RequestBase
    {
        /// <summary>
        /// 整数值参数
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// 获取整数值
        /// </summary>
        /// <returns>整数值</returns>
        public int GetIntValue()
        {
            return Value.ObjToInt();
        }

        /// <summary>
        /// 设置整数值
        /// </summary>
        /// <param name="value">整数值</param>
        public void SetIntValue(int value)
        {
            Value = value;
        }

        /// <summary>
        /// 获取浮点数值
        /// </summary>
        /// <returns>浮点数值</returns>
        public double GetDoubleValue()
        {
            return Convert.ToDouble(Value);
        }

        /// <summary>
        /// 设置浮点数值
        /// </summary>
        /// <param name="value">浮点数值</param>
        public void SetDoubleValue(double value)
        {
            Value = Convert.ToInt32(value);
        }

        /// <summary>
        /// 创建数值请求实例
        /// </summary>
        /// <param name="value">整数值</param>
        /// <returns>数值请求实例</returns>
        public static NumericRequest CreateInt(int value)
        {
            return new NumericRequest
            {
                Value = value,
                RequestId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 创建数值请求实例（异步）
        /// </summary>
        /// <param name="value">整数值</param>
        /// <returns>数值请求实例</returns>
        public static async Task<NumericRequest> CreateIntAsync(int value)
        {
            return await Task.FromResult(CreateInt(value));
        }

        /// <summary>
        /// 创建数值请求实例
        /// </summary>
        /// <param name="value">浮点数值</param>
        /// <returns>数值请求实例</returns>
        public static NumericRequest CreateDouble(double value)
        {
            return new NumericRequest
            {
                Value = Convert.ToInt32(value),
                RequestId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 创建数值请求实例（异步）
        /// </summary>
        /// <param name="value">浮点数值</param>
        /// <returns>数值请求实例</returns>
        public static async Task<NumericRequest> CreateDoubleAsync(double value)
        {
            return await Task.FromResult(CreateDouble(value));
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString()
        {
            return $"NumericRequest: Value={Value}, RequestId={RequestId}";
        }
    }
}
