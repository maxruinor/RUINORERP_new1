using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global
{

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum QueryFieldType
    {

        String,

        Money,

        Qty,

        /// <summary>
        /// 下拉枚举
        /// </summary>
        CmbEnum,

        /// <summary>
        /// 下拉数据源
        /// </summary>
        CmbDbList,

        DateTime,

        DateTimeRange,

        CheckBox,

        RdbYesNo,

    }

}
