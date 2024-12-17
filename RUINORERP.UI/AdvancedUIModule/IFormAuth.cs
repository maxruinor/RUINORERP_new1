using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.AdvancedUIModule
{
    /// <summary>
    /// 给窗体添加一些特殊的权限控制等
    /// </summary>
    public interface IFormAuth
    {
        /// <summary>
        /// 根据菜单判断来扩展可能要额外添加的按钮
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <returns></returns>
        ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo);
    }
}
