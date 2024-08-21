using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Navigator;
using RUINORERP.Global.CustomAttribute;
using System.Linq.Expressions;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 只是用于修复T的资源bug
    /// </summary>
    public partial class BaseBillQuery : UserControl
    {
        public BaseBillQuery()
        {
            InitializeComponent();
        }

        private void buttonTopArrow_Click(object sender, EventArgs e)
        {
            // For the top navigator instance we will toggle the showing of 
            // the client area below the check button area. We also toggle 
            // the direction of the button spec arrow.

            if (navigatorTop.NavigatorMode == NavigatorMode.HeaderBarCheckButtonGroup)
            {
                navigatorTop.NavigatorMode = NavigatorMode.HeaderBarCheckButtonOnly;
                buttonTopArrow.TypeRestricted = PaletteNavButtonSpecStyle.ArrowDown;
            }
            else
            {
                navigatorTop.NavigatorMode = NavigatorMode.HeaderBarCheckButtonGroup;
                buttonTopArrow.TypeRestricted = PaletteNavButtonSpecStyle.ArrowUp;
            }
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            // For the left navigator instance we will toggle the showing of 
            // the client area to the right of the check button area. We also 
            // toggle the direction of the button spec arrow.

            if (navigatorLeft.NavigatorMode == NavigatorMode.HeaderBarCheckButtonGroup)
            {
                navigatorLeft.NavigatorMode = NavigatorMode.HeaderBarCheckButtonOnly;
                buttonLeft.TypeRestricted = PaletteNavButtonSpecStyle.ArrowRight;
            }
            else
            {
                navigatorLeft.NavigatorMode = NavigatorMode.HeaderBarCheckButtonGroup;
                buttonLeft.TypeRestricted = PaletteNavButtonSpecStyle.ArrowLeft;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
        }


        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="dto"></param>
        [MustOverride]
        protected virtual void Query()
        {

        }
        private void btnAdvQuery_Click(object sender, EventArgs e)
        {

        }

      



    }
}
