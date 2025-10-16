using System.Windows.Forms;

namespace RUINORERP.UI.HelpSystem
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
            if (form == null || !HelpManager.Config.IsHelpSystemEnabled) return;

            // 注册KeyDown事件
            form.KeyPreview = true;
            form.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.F1)
                {
                    // 显示当前焦点控件或窗体的帮助
                    HelpManager.ShowHelpForControl(form, form.ActiveControl);
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
            if (form == null || !HelpManager.Config.IsHelpSystemEnabled) return;
            if (string.IsNullOrEmpty(helpPage)) return;

            HelpManager.RegisterHelpMapping(form.GetType(), helpPage, title);
        }

        /// <summary>
        /// 为控件设置帮助键
        /// </summary>
        /// <param name="control">控件实例</param>
        /// <param name="helpKey">帮助键</param>
        public static void SetControlHelpKey(this Control control, string helpKey)
        {
            if (control == null || !HelpManager.Config.IsHelpSystemEnabled) return;
            if (string.IsNullOrEmpty(helpKey)) return;

            control.Tag = helpKey; // 使用Tag存储帮助键
            control.HelpRequested += (sender, e) => {
                var ctrl = sender as Control;
                if (ctrl?.Tag is string key)
                {
                    HelpManager.ShowHelpByKey(key);
                }
                e.Handled = true;
            };
        }
        
        /// <summary>
        /// 为ToolStripItem设置帮助键
        /// </summary>
        /// <param name="item">ToolStripItem实例</param>
        /// <param name="helpKey">帮助键</param>
        public static void SetControlHelpKey(this ToolStripItem item, string helpKey)
        {
            if (item == null || !HelpManager.Config.IsHelpSystemEnabled) return;
            if (string.IsNullOrEmpty(helpKey)) return;

            item.Tag = helpKey; // 使用Tag存储帮助键
            // 注意：ToolStripItem没有HelpRequested事件，所以我们只设置Tag
            // 帮助系统会在其他地方通过Tag来获取帮助键
        }
        
        /// <summary>
        /// 获取控件的帮助键
        /// </summary>
        /// <param name="control">控件实例</param>
        /// <returns>帮助键</returns>
        public static string GetControlHelpKey(this Control control)
        {
            if (control?.Tag is string helpKey)
            {
                return helpKey;
            }
            return null;
        }
        
        /// <summary>
        /// 获取ToolStripItem的帮助键
        /// </summary>
        /// <param name="item">ToolStripItem实例</param>
        /// <returns>帮助键</returns>
        public static string GetControlHelpKey(this ToolStripItem item)
        {
            if (item?.Tag is string helpKey)
            {
                return helpKey;
            }
            return null;
        }
        
        /// <summary>
        /// 显示帮助系统主窗体
        /// </summary>
        /// <param name="form">窗体实例</param>
        public static void ShowHelpSystemForm(this Form form)
        {
            if (form == null || !HelpManager.Config.IsHelpSystemEnabled) return;
            
            try
            {
                // 创建并显示帮助系统主窗体
                var helpForm = new HelpSystemForm();
                helpForm.Show();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"无法打开帮助系统: {ex.Message}", "错误", 
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
}