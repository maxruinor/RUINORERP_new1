using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.Models
{

    [Obsolete("作废，用tb_UIMenuPersonalization代替")]
    [Serializable]
    /// <summary>
    /// 用于保存个性化菜单中属性的配置实体
    /// </summary>
    public class MenuPersonalization
    {
        /// <summary>
        /// 查询条件显示列数量
        /// </summary>
        public decimal QueryConditionShowColsQty { get; set; } = 0;
    }
}
