using System;
using System.IO;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem;

namespace RUINORERP.UI
{
    /// <summary>
    /// 帮助系统测试程序
    /// </summary>
    public class HelpSystemTestProgram
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // 初始化帮助系统
                string helpFilePath = Path.Combine(Application.StartupPath, "RUINORERP.Helper.chm");
                
                // 如果在应用程序目录中找不到，尝试在Helper目录中查找
                if (!File.Exists(helpFilePath))
                {
                    helpFilePath = Path.Combine(Application.StartupPath, "..\\RUINORERP.Helper\\help.chm");
                    if (!File.Exists(helpFilePath))
                    {
                        helpFilePath = Path.Combine(Application.StartupPath, "..\\..\\RUINORERP.Helper\\help.chm");
                    }
                }
                
                // 如果找到了帮助文件，则初始化帮助系统
                if (File.Exists(helpFilePath))
                {
                    HelpManager.Initialize(helpFilePath);
                    MessageBox.Show($"帮助系统初始化成功！\n帮助文件路径: {helpFilePath}", "帮助系统测试", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"帮助文件未找到: {helpFilePath}\n请确保已编译帮助文件。", "帮助系统测试", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化帮助系统时出错: {ex.Message}", "帮助系统测试", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // 运行主窗体
            // Application.Run(new MainForm());
        }
    }
}