using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{

    /// <summary>
    /// 费用承担方
    /// </summary>
    public enum FeeBearer
    {
        [Description("客户")]
        客户 = 1,
        [Description("供应商")]
        供应商 = 2,
        [Description("本司")]
        本司 = 3,
        [Description("混合分摊")]
        Mixed = 4        // 部分客户/部分厂家/部分本司  分摊比例{0}{1}{2}

    }

    /// <summary>
    /// 售后申请类型
    /// </summary>
    public enum ASApplyType
    {
        [Description("维修")]
        维修 = 1,
        [Description("退货")]
        退货 = 2,
        [Description("换货")]
        换货 = 3
    }
 
    /// <summary>
    /// 售后处理方式
    /// </summary>
    public enum ASProcessWay
    {
        
    }


}
