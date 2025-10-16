using System;
using System.Drawing;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 测试控件帮助功能的窗体
    /// </summary>
    public partial class TestControlHelpForm : frmBase
    {
        public TestControlHelpForm()
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
            this.SetHelpPage("forms/test_control_help.html", "控件帮助测试");
            
            // 为控件设置帮助键
            txtName.SetControlHelpKey("textbox_name");
            btnSave.SetControlHelpKey("button_save");
            btnDelete.SetControlHelpKey("button_delete");
            btnAdd.SetControlHelpKey("button_add");
            chkActive.SetControlHelpKey("checkbox_active");
            rdoOption1.SetControlHelpKey("radiobutton_option1");
            rdoOption2.SetControlHelpKey("radiobutton_option2");
            cmbOptions.SetControlHelpKey("combobox_options");
            nudQuantity.SetControlHelpKey("numericupdown_quantity");
        }
    }
}