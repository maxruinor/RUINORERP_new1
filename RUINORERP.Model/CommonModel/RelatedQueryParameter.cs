using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.CommonModel
{
    /// <summary>
    /// 针对单个单据查询时要用到的参数
    /// </summary>
    public class RelatedQueryParameter
    {
        public BizType bizType { get; set; }
        public long billId { get; set; }
        public string billNo { get; set; }
    }
}
