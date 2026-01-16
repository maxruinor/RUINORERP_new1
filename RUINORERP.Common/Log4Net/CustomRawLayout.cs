using System;
using log4net.Core;
using log4net.Layout;

namespace RUINORERP.Common.Log4Net
{
    /// <summary>
    /// 自定义原始布局：直接返回对象（支持DBNull），解决数值类型字段空值问题
    /// </summary>
    public class CustomRawLayout : IRawLayout
    {
        /// <summary>
        /// 要获取的日志属性名（如User_ID、Operator等）
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 是否为数值类型（仅用于User_ID等Int64字段）
        /// </summary>
        public bool IsNumeric { get; set; }

        /// <summary>
        /// 核心方法：直接返回原始对象，避免字符串转换
        /// </summary>
        public virtual object Format(LoggingEvent loggingEvent)
        {
            // 从日志事件中获取属性值
            object propertyValue = loggingEvent.LookupProperty(PropertyName);

            // 处理数值类型（User_ID）的空值逻辑
            if (IsNumeric && string.Equals(PropertyName, "User_ID", StringComparison.OrdinalIgnoreCase))
            {
                // 空值直接返回DBNull.Value
                if (propertyValue == null || string.IsNullOrWhiteSpace(propertyValue.ToString()))
                {
                    return DBNull.Value;
                }

                // 尝试转换为long，失败则返回DBNull
                if (long.TryParse(propertyValue.ToString(), out long userId))
                {
                    return userId;
                }
                return DBNull.Value;
            }

            // 非数值类型：空值返回空字符串，非空返回原值
            return propertyValue ?? string.Empty;
        }
    }
}
