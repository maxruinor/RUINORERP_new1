using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Model
{
    /// <summary>
    /// 业务编码参数类
    /// 用于传递生成业务编码所需的额外参数
    /// 合表时根据这个参考生成对应的业务编号
    /// 如：预收付款表。
    /// 是预收 还是预付通过这个参数来定
    /// </summary>
    public class BizCodeParameter
    {
        //财务用
        public ReceivePaymentType PaymentType = ReceivePaymentType.收款;

        /// <summary>
        /// 业务类型名称（如果使用字符串形式）
        /// </summary>
        public string BizTypeName { get; set; }

        /// <summary>
        /// 常量参数
        /// 用于一些需要前缀或特定常量的编码生成
        /// </summary>
        public string ConstValue { get; set; }

        /// <summary>
        /// 自定义参数字典
        /// 用于传递各种自定义参数
        /// </summary>
        public Dictionary<string, object> CustomParams { get; set; }

        /// <summary>
        /// 初始化参数类
        /// </summary>
        public BizCodeParameter()
        {
            CustomParams = new Dictionary<string, object>();
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="key">参数键</param>
        /// <param name="value">参数值</param>
        public void AddCustomParam(string key, object value)
        {
            CustomParams[key] = value;
        }

        /// <summary>
        /// 获取自定义参数值
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="key">参数键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public T GetCustomParam<T>(string key, T defaultValue = default)
        {
            if (CustomParams.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return defaultValue;
        }
    }
}