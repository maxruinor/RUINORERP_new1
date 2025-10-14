using System.Windows.Forms;

namespace RUINORERP.UI.Common.HelpSystem
{
    /// <summary>
    /// 帮助系统扩展方法
    /// </summary>
    public static class HelpExtensions
    {
        /// <summary>
        /// 为窗体启用F1帮助功能
        /// </summary>
        /// <param name="form">窗体实例</param>
        public static void EnableF1Help(this Form form)
        {
            if (form == null) return;

            // 注册KeyDown事件
            form.KeyPreview = true;
            form.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.F1)
                {
                    HelpManager.ShowHelp(form);
                    e.Handled = true;
                }
            };

            // 也可以注册HelpRequested事件
            form.HelpRequested += (sender, hlpevent) =>
            {
                HelpManager.ShowHelp(form);
                hlpevent.Handled = true;
            };
        }

        /// <summary>
        /// 设置窗体的帮助页面
        /// </summary>
        /// <param name="form">窗体实例</param>
        /// <param name="helpPage">帮助页面路径</param>
        /// <param name="title">帮助页面标题</param>
        public static void SetHelpPage(this Form form, string helpPage, string title = null)
        {
            if (form == null) return;
            if (string.IsNullOrEmpty(helpPage)) return;

            HelpManager.RegisterHelpMapping(form.GetType(), helpPage, title);
        }
    }
}