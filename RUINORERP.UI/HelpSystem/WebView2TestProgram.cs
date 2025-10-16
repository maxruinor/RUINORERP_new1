using System;
using System.IO;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// WebView2帮助系统测试程序
    /// </summary>
    public class WebView2TestProgram
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // 初始化帮助系统
            InitializeHelpSystem();
            
            // 显示测试窗体
            Application.Run(new WebView2TestForm());
        }
        
        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        private static void InitializeHelpSystem()
        {
            try
            {
                // 查找帮助文件
                string helpFilePath = FindHelpFile();
                if (!string.IsNullOrEmpty(helpFilePath) && File.Exists(helpFilePath))
                {
                    HelpManager.Initialize(helpFilePath);
                    MessageBox.Show($"帮助系统初始化成功！\n帮助文件路径: {helpFilePath}", "帮助系统测试", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("未找到帮助文件，帮助系统功能将不可用。", "帮助系统测试", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化帮助系统时出错: {ex.Message}", "帮助系统测试", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 查找帮助文件
        /// </summary>
        /// <returns>帮助文件路径</returns>
        private static string FindHelpFile()
        {
            // 可能的帮助文件路径
            string[] possiblePaths = {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "help.chm"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\RUINORERP.Helper\\help.chm"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\RUINORERP.Helper\\help.chm"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\..\\RUINORERP.Helper\\help.chm")
            };
            
            foreach (string path in possiblePaths)
            {
                string fullPath = Path.GetFullPath(path);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            
            return null;
        }
    }
    
    /// <summary>
    /// WebView2帮助系统测试窗体
    /// </summary>
    public partial class WebView2TestForm : frmBase
    {
        public WebView2TestForm()
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
            testButton.Location = new System.Drawing.Point(50, 50);
            testButton.Size = new System.Drawing.Size(100, 30);
            testButton.Name = "testButton";
            
            TextBox testTextBox = new TextBox();
            testTextBox.Text = "测试文本框";
            testTextBox.Location = new System.Drawing.Point(50, 100);
            testTextBox.Size = new System.Drawing.Size(150, 21);
            testTextBox.Name = "testTextBox";
            
            Label testLabel = new Label();
            testLabel.Text = "测试标签";
            testLabel.Location = new System.Drawing.Point(50, 150);
            testLabel.Size = new System.Drawing.Size(100, 20);
            testLabel.Name = "testLabel";
            
            // 添加控件
            this.Controls.Add(testButton);
            this.Controls.Add(testTextBox);
            this.Controls.Add(testLabel);
            
            // 设置窗体属性
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Text = "WebView2帮助系统测试";
            
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