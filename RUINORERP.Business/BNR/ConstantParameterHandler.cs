using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUINORERP.Business.BNR
{
    /// <summary>
    /// 常量参数{S:xxx}
    /// 支持在值后面添加:upper来转换为大写
    /// 例如: {S:RP:upper} 将输出 RP
    /// </summary>
    [ParameterType("S")]
    public class ConstantParameterHandler : IParameterHandler
    {
        public void Execute(StringBuilder sb, string value)
        {
            // 检查是否需要转换为大写
            if (value.EndsWith(":upper"))
            {
                sb.Append(value.Substring(0, value.Length - 6).ToUpper());
            }
            else
            {
                sb.Append(value);
            }
        }
        public object Factory
        {
            get;
            set;
        }
    }
}
