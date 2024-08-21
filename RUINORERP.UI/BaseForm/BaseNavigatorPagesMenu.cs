using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.BaseForm
{
    public class Navigator
    {
        List<NavigatorMenu> navigatorMenus = new List<NavigatorMenu>();

        internal List<NavigatorMenu> NavigatorMenus { get => navigatorMenus; set => navigatorMenus = value; }
    }

    public class NavigatorMenu
    {
        int _MenuLevel = 0;
        string _MenuName = string.Empty;

        /// <summary>
        /// 标识UI是否已经加载过
        /// </summary>
        public bool UILoaded { get; set; } = false;
        public int MenuSort { get; set; } = 0;
        public int MenuLevel { get => _MenuLevel; set => _MenuLevel = value; }
        public string MenuName { get => _MenuName; set => _MenuName = value; }

        public NavigatorMenuType menuType { get; set; }
    }



    public enum NavigatorMenuType
    {
        销售明细分析,
        销售订单数据汇总,
        销售出库业绩汇总,
    }
}
