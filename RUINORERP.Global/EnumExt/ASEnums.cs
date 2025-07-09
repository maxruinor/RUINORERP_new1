using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{

    /// <summary>
    /// 费用承担方类型
    /// </summary>
    public enum ExpenseBearerType
    {
        [Description("客户")]
        客户 = 1,

        [Description("供应商")]
        供应商 = 2,

        [Description("己方公司")]
        己方公司 = 3,

        [Description("混合分摊")]
        Mixed = 4,    // 部分客户/部分厂家/部分本司  分摊比例{0}{1}{2}
    }
    public enum ExpenseAllocationMode
    {
        [Description("单一承担")]
        单一承担 = 1,
        [Description("多方分摊")]
        多方分摊 = 2
    }


    /// <summary>
    /// 售后申请单提交后的处理状态
    /// 
    /// </summary>
    public enum ASProcessStatus
    {

        [Description("【登记】")]
        登记 = 1,

        [Description("【评估报价】")]
        评估报价 = 1,

        [Description("【待领料】")]
        待领料 = 2,

        [Description("【维修中】")]
        维修中 = 3,

        [Description("【待交付】")]
        待交付 = 4,
  
    }

}
