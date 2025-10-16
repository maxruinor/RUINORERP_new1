using System;
using System.Drawing;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助系统演示窗体
    /// </summary>
    public partial class HelpSystemDemoForm : frmBase
    {
        public HelpSystemDemoForm()
        {
            InitializeComponent();
            InitializeHelpSystem();
        }
        
        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        private void InitializeHelpSystem()
        {
            // 为窗体设置帮助页面
            this.SetHelpPage("forms/help_demo.html", "帮助系统演示");
            
            // 为特定控件设置帮助键
            btnSave.SetControlHelpKey("button_save");
            txtName.SetControlHelpKey("textbox_name");
            chkActive.SetControlHelpKey("checkbox_active");
            cmbOptions.SetControlHelpKey("combobox_options");
        }
    }
}