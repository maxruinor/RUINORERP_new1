using System;
using System.Drawing;
using System.Windows.Forms;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 控件级别帮助演示窗体
    /// </summary>
    public partial class ControlHelpDemoForm : frmBase
    {
        public ControlHelpDemoForm()
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
            this.SetHelpPage("forms/control_help_demo.html", "控件帮助演示");
            
            // 为控件设置帮助键
            txtName.SetControlHelpKey("textbox_name");
            btnSave.SetControlHelpKey("button_save");
            btnCancel.SetControlHelpKey("button_cancel");
            chkActive.SetControlHelpKey("checkbox_active");
            cmbOptions.SetControlHelpKey("combobox_options");
        }
    }
}