using Microsoft.Extensions.Logging;
using System;
using RUINORERP.Model;
using RUINORERP.Common.Helper;

namespace RUINORERP.Extensions.Filter
{
    /// <summary>
    /// 全局异常错误日志
    /// </summary>
    public class GlobalExceptionsFilter
    {

        private readonly ILogger<GlobalExceptionsFilter> _loggerHelper;

        //这里应该传入用户信息
        public GlobalExceptionsFilter(ILogger<GlobalExceptionsFilter> loggerHelper)
        {
            _loggerHelper = loggerHelper;
        }

        public void OnException(Exception _exception)
        {
            var json = new Model.MessageModel<string>();

            json.msg = _exception.Message;//错误信息
            json.status = 500;//500异常 
            var errorAudit = "Unable to resolve service for";
            if (!string.IsNullOrEmpty(json.msg) && json.msg.Contains(errorAudit))
            {
                json.msg = json.msg.Replace(errorAudit, $"（若新添加服务，需要重新编译项目）{errorAudit}");
            }

            //if (_env.EnvironmentName.ObjToString().Equals("Development"))
            //{
            //    json.msgDev = context.Exception.StackTrace;//堆栈信息
            //}
            //保存？
            string exContext = JsonHelper.GetJSON<MessageModel<string>>(json);



            //MiniProfiler.Current.CustomTiming("Errors：", json.msg);


            //采用log4net 进行错误日志记录
            _loggerHelper.LogError(json.msg + WriteLog(json.msg, _exception));
            //if (AppSettings.app(new string[] { "Middleware", "SignalRSendLog", "Enabled" }).ObjToBool())
            //{

            //}


        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(string throwMsg, Exception ex)
        {
            return string.Format("\r\n【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
        }

    }
    //public class InternalServerErrorObjectResult : ObjectResult
    //{
    //    public InternalServerErrorObjectResult(object value) : base(value)
    //    {
    //        StatusCode = StatusCodes.Status500InternalServerError;
    //    }
    //}
    //返回错误信息
    public class JsonErrorResponse
    {
        /// <summary>
        /// 生产环境的消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 开发环境的消息
        /// </summary>
        public string DevelopmentMessage { get; set; }
    }

}
