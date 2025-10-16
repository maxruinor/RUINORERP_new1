using System;
using System.Drawing;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助系统测试窗体
    /// </summary>
    public partial class HelpSystemTestForm : frmBase
    {
        public HelpSystemTestForm()
        {
            InitializeComponent();
            InitializeHelpSystem();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // 创建测试控件
            Button testButton = new Button();
            testButton.Text = "测试按钮";
            testButton.Location = new Point(50, 50);
            testButton.Size = new Size(100, 30);
            testButton.Name = "testButton";
            
            TextBox testTextBox = new TextBox();
            testTextBox.Text = "测试文本框";
            testTextBox.Location = new Point(50, 100);
            testTextBox.Size = new Size(150, 21);
            testTextBox.Name = "testTextBox";
            
            Label testLabel = new Label();
            testLabel.Text = "测试标签";
            testLabel.Location = new Point(50, 150);
            testLabel.Size = new Size(100, 20);
            testLabel.Name = "testLabel";
            
            // 添加控件
            this.Controls.Add(testButton);
            this.Controls.Add(testTextBox);
            this.Controls.Add(testLabel);
            
            // 设置窗体属性
            this.ClientSize = new Size(300, 200);
            this.Text = "帮助系统测试";
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        
        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        private void InitializeHelpSystem()
        {
            // 为窗体设置帮助页面
            this.SetHelpPage("test_help_content.html", "帮助系统测试");
            
            // 为控件设置帮助键
            foreach (Control control in this.Controls)
            {
                switch (control.Name)
                {
                    case "testButton":
                        control.SetControlHelpKey("button_test");
                        break;
                    case "testTextBox":
                        control.SetControlHelpKey("textbox_general");
                        break;
                }
            }
        }
    }
}