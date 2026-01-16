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

                // User_ID 为 null 时返回 DBNull.Value
                if (property == "User_ID" && propertyValue == null)
                {
                    return DBNull.Value;
                }

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

            // 默认返回 DBNull.Value 用于可空字段
            if (property == "User_ID")
            {
                return DBNull.Value;
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


    //=========================上面的方法是将字段统计反射了。下面是分开了

    //https://www.cnblogs.com/niuniu1985/p/7943463.html
    public class ActionLoggerInfo
    {
        public int UserID { get; set; }
        public string UnitCode { get; set; }
        public int MenuID { get; set; }
        public int OperaterType { get; set; }
        public string sMessage { get; set; }
        public ActionLoggerInfo(int userId, string unitCode, int menuId, int operaterType, string smessage)
        {
            this.UserID = userId;
            this.UnitCode = unitCode;
            this.MenuID = menuId;
            this.OperaterType = operaterType;
            this.sMessage = smessage;
        }
    }

    public class ActionConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)
        {
            var actionInfo = loggingEvent.MessageObject as ActionLoggerInfo;

            if (actionInfo == null)
            {
                writer.Write("");
            }
            else
            {
                switch (this.Option.ToLower())
                {
                    case "userid":
                        writer.Write(actionInfo.UserID);
                        break;
                    case "unitcode":
                        writer.Write(actionInfo.UnitCode);
                        break;
                    case "menuid":
                        writer.Write(actionInfo.MenuID);
                        break;
                    case "operatertype":
                        writer.Write(actionInfo.OperaterType);
                        break;
                    case "smessage":
                        writer.Write(actionInfo.sMessage);
                        break;
                    default:
                        writer.Write("");
                        break;
                }
            }
        }
    }

    public class ActionLayoutPattern : PatternLayout
    {
        public ActionLayoutPattern()
        {
            this.AddConverter("actionInfo", typeof(ActionConverter));
        }
    }

    //注册对应的配置文件 

    /*
    public class MessageLog

    {

        /// <summary>

        /// 短信发送是否成功

        /// </summary>

        public int Success { get; set; }

        /// <summary>

        /// 发送号码(可以多个，逗号分隔)

        /// </summary>

        public string Mobiles { get; set; }

        public string Message { get; set; }

        /// <summary>

        /// 发送内容

        /// </summary>

        public string Content { get; set; }

        public override string ToString()

        {

            return this.Message;

        }

    }

    internal sealed class ContentPatternConverter : PatternLayoutConverter

    {

        protected override void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)

        {

            var messageLog = loggingEvent.MessageObject as MessageLog;

            if (messageLog != null)

            {

                writer.Write(messageLog.Content);

            }

        }

    }


    public class MessageLayout : PatternLayout

    {

        public MessageLayout()
        {

            //this.AddConverter("Success", typeof(SuccessPatternConverter));

            //this.AddConverter("Mobiles", typeof(MobilesPatternConverter));

            this.AddConverter("UserName", typeof(ContentPatternConverter));

        }

    }
    */
}

