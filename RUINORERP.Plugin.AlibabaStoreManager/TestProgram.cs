using System;
using System.Windows.Forms;
using RUINORERP.Plugin.AlibabaStoreManager;

namespace RUINORERP.Plugin.AlibabaStoreManager.Test
{
    /// <summary>
    /// 技术验证程序入口点
    /// 用于独立测试WebView2功能和插件核心功能
    /// </summary>
    public class TestProgram
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // 显示技术验证窗体
            Application.Run(new ValidationForm());
        }
    }
}