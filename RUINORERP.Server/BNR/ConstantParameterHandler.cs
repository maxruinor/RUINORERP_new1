using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUINORERP.Server.BNR
{
    /// <summary>
    /// 常量参数{CN:广州}
    /// </summary>
    [ParameterType("S")]
    public class ConstantParameterHandler : IParameterHandler
    {
        public void Execute(StringBuilder sb, string value)
        {
            sb.Append(value);
        }
        public BNRFactory Factory
        {
            get;
            set;
        }
    }
}
