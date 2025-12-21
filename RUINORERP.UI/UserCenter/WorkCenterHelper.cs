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

        /// <summary>
        /// 工作台中 如果是 共享用一个表 表达了多种业务时区别菜单用。对应共享的子类业务的每个窗体的标记
        /// </summary>
        public string UIPropertyIdentifier { get; set; }
        public Type tableType { get; set; }
        public bool IncludeBillIds { get; internal set; }
        public string PrimaryKeyFieldName { get; internal set; }
        public List<long> BillIds { get; internal set; }
    }
}
