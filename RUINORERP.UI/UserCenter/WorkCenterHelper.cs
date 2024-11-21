using RUINORERP.Business.Processor;
using RUINORERP.Global;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UserCenter
{
    internal class WorkCenterHelper
    {
    }

    /// <summary>
    /// 工作台节点事件中传递参数实体
    /// </summary>
    public class QueryParameter
    {
        public List<IConditionalModel> conditionals { get; set; }

        public QueryFilter queryFilter { get; set; }
        public BizType bizType { get; set; }
        public Type tableType { get; set; }
    }
}
