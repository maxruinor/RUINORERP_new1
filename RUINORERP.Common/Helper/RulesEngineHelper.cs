using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;


namespace RUINORERP.Common.Helper
{
    internal class RulesEngineHelper
    {

    }

    //https://www.cnblogs.com/wl-blog/p/17202834.html
    public class RuleResultWithFilter
    {
        public bool IsSuccess { get; set; }
        public string FilterExpression { get; set; }
    }

    public static class FilterHelper
    {
        public static bool SetFilter(string filterExpression)
        {
            // 此函数仅用于规则引擎内部设置参数，返回值不影响逻辑
            return true;
        }
    }
}
