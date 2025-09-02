using log4net.Layout;
using log4net.Core;
using log4net.Layout.Pattern;
using System.Text;
using System.IO;
using System.Reflection;
using System;

namespace RUINORERP.Common.Log4Net {
    /// <summary>
    /// 自定义布局器，扩展了PatternLayout，提供增强的日志属性支持
    /// </summary>
    public class EnhancedCustomLayout : PatternLayout
    {
        public EnhancedCustomLayout()
        {
            // 添加对自定义属性的支持
            this.AddConverter("property", typeof(EnhancedLogInfoPatternConverter));
        }

        /// <summary>
        /// 初始化布局器选项
        /// </summary>
        public override void ActivateOptions()
        {
            try
            {
                // 调用基类的ActivateOptions
                base.ActivateOptions();
            }
            catch (Exception ex)
            {
                // 记录初始化过程中的异常
                System.Console.WriteLine("初始化EnhancedCustomLayout失败: " + ex.Message);
            }
        }
    }

    /// <summary>
    /// 增强版日志信息转换器，提供更强大的属性查找和异常处理功能
    /// </summary>
    public class EnhancedLogInfoPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)
        {
            try
            {
                if (Option != null)
                {
                    // 尝试从ThreadContext.Properties中获取属性值
                    object propertyValue = null;
                    if (loggingEvent.Properties.Contains(Option))
                    {
                        propertyValue = loggingEvent.Properties[Option];
                    }

                    // 如果未找到，尝试使用基类的方式查找
                    if (propertyValue == null)
                    {
                        // 使用LookupProperty方法尝试获取属性值
                        propertyValue = LookupProperty(Option, loggingEvent);
                    }

                    // 写入找到的属性值，如果没有找到则写入空字符串
                    WriteObject(writer, loggingEvent.Repository, propertyValue ?? string.Empty);
                }
                else
                {
                    // 如果没有指定属性，则写入所有属性
                    WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
                }
            }
            catch (Exception ex)
            {
                // 处理转换过程中的异常，确保日志系统不会因格式化问题而崩溃
                writer.Write("日志属性转换失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 通过反射获取传入的日志对象的某个属性的值
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="loggingEvent">日志事件对象</param>
        /// <returns>属性值，如果未找到则返回null</returns>
        private object LookupProperty(string property, LoggingEvent loggingEvent)
        {
            try
            {
                if (loggingEvent.MessageObject != null)
                {
                    PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
                    if (propertyInfo != null)
                    {
                        return propertyInfo.GetValue(loggingEvent.MessageObject, null);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("查找日志属性失败: " + ex.Message);
                return null;
            }
        }
    }
}