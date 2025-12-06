using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.BNR
{
    /// <summary>
    /// {D:yyyyMMdd}
    /// 时间参数
    /// </summary>
    [ParameterType("D")]
    public class DateParameterHandler : IParameterHandler
    {

        public void Execute(StringBuilder sb, string value)
        {
            // 检查value中是否包含嵌套表达式({和})
            if (value.Contains('{') && value.Contains('}'))
            {
                // 递归处理嵌套表达式
                if (Factory != null)
                {
                    value = Factory.Create(value);
                }
            }
            
            sb.Append(DateTime.Now.ToString(value));
        }

        public BNRFactory Factory
        {
            get;
            set;
        }
    }


    /*

    /// <summary>
    /// {D:yyyyMMdd}
    /// </summary>
    [ParameterType("D")]
    public class DateParameterHandler : IParameterHandler
    {

        public Task Execute(StringBuilder sb, string value)
        {
            sb.Append(DateTime.Now.ToString(value));
            return Task.CompletedTask;
        }

        void IParameterHandler.Execute(StringBuilder sb, string value)
        {
             sb.Append(DateTime.Now.ToString(value));
        }

        public BNRFactory Factory
        {
            get;
            set;
        }
    }
    */




}
