using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.BusinessService.SmartMenuService
{
    /// <summary>
    /// 菜单使用情况
    /// </summary>
    public class MenuUseInfo
    {
        public long MenuId { get; set; }
        public int Frequency { get; set; }
        public DateTime LastClickTime { get; set; }
    }
}