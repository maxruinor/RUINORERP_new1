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
using RUINORERP.UI.HelpSystem.Core;
using RUINORERP.UI.HelpSystem.Extensions;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 只是用于修复T的资源bug
    /// </summary>
    public partial class BaseBillQuery : UserControl
    {
        #region 帮助系统集成

        /// <summary>
        /// 是否启用智能帮助
        /// </summary>
        [Category("帮助系统")]
        [Description("是否启用智能帮助功能")]
        public bool EnableSmartHelp { get; set; } = true;

        /// <summary>
        /// 窗体帮助键
        /// </summary>
        [Category("帮助系统")]
        [Description("窗体帮助键,留空则使用控件类型名称")]
        public string FormHelpKey { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        [Category("帮助系统")]
        [Description("关联的实体类型,用于字段级帮助")]
        public Type EntityType { get; private set; }

        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        protected virtual void InitializeHelpSystem()
        {
            if (!EnableSmartHelp) return;

            try
            {
                // 为控件启用智能提示
                HelpManager.Instance.EnableSmartTooltipForAll(this, FormHelpKey, EntityType);

                // 启用F1帮助
                this.EnableF1Help();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化帮助系统失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示窗体帮助
        /// </summary>
        public void ShowFormHelp()
        {
            if (!EnableSmartHelp) return;
            HelpManager.Instance.ShowControlHelp(this);
        }

        #endregion

        public BaseBillQuery()
        {
            InitializeComponent();

            // 初始化帮助系统
            InitializeHelpSystem();
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
