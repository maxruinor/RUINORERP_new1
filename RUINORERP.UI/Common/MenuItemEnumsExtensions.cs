using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public static class MenuItemEnumsExtensions
    {
        public static string GetDisplayName(this MenuItemEnums value)
        {
            return value switch
            {
                MenuItemEnums.复制性新增 => "复制新增",
                MenuItemEnums.数据特殊修正 => "特殊修正",
                _ => value.ToString()
            };
        }
    }
}
