using System;
using System.Collections.Generic;
using log4net.Layout;
using log4net.Layout.Pattern;
using System.Linq;
using System.Reflection;
using System.Web;
using log4net.Core;

namespace RUINORERP.Common.Log4Net
{
    /// <summary>
    /// 还有些问题
    /// </summary>
    public class CustomLayout : PatternLayout
    {
        public CustomLayout()
        {
            this.AddConverter("property", typeof(LogInfoPatternConverter));
        }
    }

    public class LogInfoPatternConverter : PatternLayoutConverter
    {

        protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
        {
            if (Option != null)
            {
                // 获取指定属性的值
                object value = LookupProperty(Option, loggingEvent);
                WriteObject(writer, loggingEvent.Repository, value);
                // Write the value for the specified key
                //WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                // Write all the key value pairs  // 如果没有指定属性，则写入所有属性
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }
        /// <summary>
        /// 通过反射获取传入的日志对象的某个属性的值
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>

        private object LookupProperty(string property, log4net.Core.LoggingEvent loggingEvent)
        {
            // 优先从 ThreadContext.Properties 获取值（推荐方式）
            var properties = loggingEvent.GetProperties();
            if (properties != null && properties.Contains(property))
            {
                object propertyValue = properties[property];
                return propertyValue ?? string.Empty;
            }

            // 如果 ThreadContext 中没有，尝试从 MessageObject 反射获取（兼容旧代码）
            if (loggingEvent.MessageObject != null)
            {
                PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(loggingEvent.MessageObject, null);
                }
            }

            return string.Empty;
        }

        //private object LookupProperty(string property, LoggingEvent loggingEvent)
        //{
        //    // 尝试从 MDC 中获取值
        //    object value = loggingEvent.Repository.GetContext().GetProperty(property);

        //    // 如果 MDC 中没有找到，尝试从消息对象中获取
        //    if (value == null && loggingEvent.MessageObject != null)
        //    {
        //        PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
        //        if (propertyInfo != null)
        //        {
        //            value = propertyInfo.GetValue(loggingEvent.MessageObject, null);
        //        }
        //    }

        //    return value ?? string.Empty;
        //}

    }
 
}

