using System;
using System.IO;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助系统测试程序入口
    /// </summary>
    public class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // 初始化帮助系统
            InitializeHelpSystem();
            
            // 显示测试窗体
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
            // 可能的帮助文件路径
            string[] possiblePaths = {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "help.chm"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\RUINORERP.Helper\\help.chm"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\RUINORERP.Helper\\help.chm")
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
}