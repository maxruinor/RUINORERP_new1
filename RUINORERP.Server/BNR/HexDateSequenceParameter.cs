using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Server.BNR
{

    /// <summary>
    /// {Hex:yyyyMMdd} 
    /// 用一种简单方式编码日期，不轻易让人看出来
    /// </summary>
    [ParameterType("Hex")]
    public class HexDateSequenceParameter : IParameterHandler
    {

        public void Execute(StringBuilder sb, string value)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            sb.Append(year.ToString("X") + month.ToString("X") + day.ToString("X"));
        }
   

        public BNRFactory Factory
        {
            get;
            set;
        }
    }
}
