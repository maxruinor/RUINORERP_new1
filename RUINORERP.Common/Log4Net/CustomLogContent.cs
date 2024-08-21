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
                // Write the value for the specified key
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                // Write all the key value pairs
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
            object propertyValue = string.Empty;
            PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
            if (propertyInfo != null)
                propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
            return propertyValue;
        }
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

