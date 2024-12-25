using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{

    //生产模板用到的枚举


    /// <summary>
    /// 替代品的优先使用类型
    /// 按这些类型会有一个规则和使用提示的策略
    /// </summary>
    public enum PriorityUseType
    {
        库存数量=1,
        成本=2,
        入库时间=3
    }
}
