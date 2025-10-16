using System;
using System.IO;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助系统测试程序
    /// </summary>
    public class HelpSystemTestProgram
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // 初始化帮助系统
            InitializeHelpSystem();
            
            // 显示测试主窗体
            Application.Run(new HelpSystemTestForm());
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
            // 首先检查CHM文件
            string[] possibleChmPaths = {
                Path.Combine(Application.StartupPath, "help.chm"),
                Path.Combine(Application.StartupPath, "..\\RUINORERP.Helper\\help.chm"),
                Path.Combine(Application.StartupPath, "..\\..\\RUINORERP.Helper\\help.chm")
            };
            
            foreach (string path in possibleChmPaths)
            {
                if (File.Exists(path))
                    return path;
            }
            
            // 如果没有CHM文件，检查是否有WCP项目文件
            string wcpPath = Path.Combine(Application.StartupPath, "..\\RUINORERP.Helper\\ERP系统帮助.wcp");
            if (File.Exists(wcpPath))
            {
                MessageBox.Show("检测到WinCHM Pro项目文件，但需要编译为CHM文件才能使用帮助功能。", "帮助系统测试", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            return null;
        }
    }
    
    /// <summary>
    /// 帮助系统测试主窗体
    /// </summary>
    public partial class HelpSystemTestForm : frmBase
    {
        public HelpSystemTestForm()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // 创建菜单
            MenuStrip menuStrip = new MenuStrip();
            
            // 帮助菜单
            ToolStripMenuItem helpMenu = new ToolStripMenuItem("帮助(&H)");
            ToolStripMenuItem showFormHelp = new ToolStripMenuItem("显示窗体帮助(&F)", null, ShowFormHelp_Click);
            ToolStripMenuItem showControlHelp = new ToolStripMenuItem("显示控件帮助(&C)", null, ShowControlHelp_Click);
            ToolStripMenuItem showHelpSystem = new ToolStripMenuItem("显示帮助系统(&S)", null, ShowHelpSystem_Click);
            
            helpMenu.DropDownItems.Add(showFormHelp);
            helpMenu.DropDownItems.Add(showControlHelp);
            helpMenu.DropDownItems.Add(new ToolStripSeparator());
            helpMenu.DropDownItems.Add(showHelpSystem);
            menuStrip.Items.Add(helpMenu);
            
            // 创建测试控件
            Button testButton = new Button();
            testButton.Text = "测试按钮";
            testButton.Location = new System.Drawing.Point(50, 100);
            testButton.Size = new System.Drawing.Size(100, 30);
            testButton.Name = "testButton";
            
            // 为控件设置帮助键
            testButton.SetControlHelpKey("button_test");
            
            // 创建标签
            Label testLabel = new Label();
            testLabel.Text = "这是一个测试标签";
            testLabel.Location = new System.Drawing.Point(50, 150);
            testLabel.Size = new System.Drawing.Size(150, 20);
            testLabel.Name = "testLabel";
            
            // 添加控件
            this.Controls.Add(testButton);
            this.Controls.Add(testLabel);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
            
            // 设置窗体属性
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Text = "帮助系统测试程序";
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        
        private void ShowFormHelp_Click(object sender, EventArgs e)
        {
            HelpManager.ShowHelp(this);
        }
        
        private void ShowControlHelp_Click(object sender, EventArgs e)
        {
            // 显示当前焦点控件的帮助
            HelpManager.ShowHelpForControl(this, this.ActiveControl);
        }
        
        private void ShowHelpSystem_Click(object sender, EventArgs e)
        {
            this.ShowHelpSystemForm();
        }
    }
}