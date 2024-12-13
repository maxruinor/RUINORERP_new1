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
        ToolStripItem[] AddExtendButton();
    }
}
