using System;
using System.Windows.Forms;
using System.Linq;
using RUINORERP.UI.HelpSystem.Core;
using RUINORERP.UI.HelpSystem.Components;

namespace RUINORERP.UI.HelpSystem.Extensions
{
    /// <summary>
    /// 控件帮助扩展方法
    /// 为控件提供便捷的帮助功能扩展
    /// </summary>
    public static class ControlHelpExtensions
    {
        #region 扩展方法 - 帮助键设置

        /// <summary>
        /// 为控件设置帮助键
        /// 帮助键用于定位帮助内容
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <param name="helpKey">帮助键</param>
        public static void SetHelpKey(this Control control, string helpKey)
        {
            if (control == null || string.IsNullOrEmpty(helpKey))
            {
                return;
            }

            control.Tag = $"HelpKey:{helpKey}";
        }

        /// <summary>
        /// 获取控件的帮助键
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <returns>帮助键,未设置则返回null</returns>
        public static string GetHelpKey(this Control control)
        {
            if (control?.Tag == null)
            {
                return null;
            }

            if (control.Tag is string tagString && tagString.StartsWith("HelpKey:"))
            {
                return tagString.Substring("HelpKey:".Length);
            }

            return null;
        }

        #endregion

        #region 扩展方法 - 智能提示

        /// <summary>
        /// 为控件启用智能提示
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <param name="helpKey">帮助键(可选)</param>
        public static void EnableHelpTooltip(this Control control, string helpKey = null)
        {
            HelpManager.Instance.EnableSmartTooltipForControl(control, helpKey);
        }

        /// <summary>
        /// 为控件禁用智能提示
        /// </summary>
        /// <param name="control">目标控件</param>
        public static void DisableHelpTooltip(this Control control)
        {
            if (control == null) return;

            try
            {
                control.MouseHover -= Control_MouseHover;
                control.MouseLeave -= Control_MouseLeave;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"禁用智能提示失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 为所有子控件启用智能提示
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="helpKeyPrefix">帮助键前缀(可选)</param>
        public static void EnableHelpTooltipForAll(this Control parent, string helpKeyPrefix = null)
        {
            HelpManager.Instance.EnableSmartTooltipForAll(parent, helpKeyPrefix);
        }

        #endregion

        #region 扩展方法 - F1帮助

        /// <summary>
        /// 为控件启用F1帮助
        /// </summary>
        /// <param name="control">目标控件</param>
        public static void EnableF1Help(this Control control)
        {
            if (control == null) return;

            control.KeyDown += Control_KeyDown;
        }

        /// <summary>
        /// 为控件禁用F1帮助
        /// </summary>
        /// <param name="control">目标控件</param>
        public static void DisableF1Help(this Control control)
        {
            if (control == null) return;

            control.KeyDown -= Control_KeyDown;
        }

        #endregion

        #region 扩展方法 - 帮助显示

        /// <summary>
        /// 显示控件的帮助
        /// </summary>
        /// <param name="control">目标控件</param>
        public static void ShowHelp(this Control control)
        {
            if (control == null) return;
            HelpManager.Instance.ShowControlHelp(control);
        }

        /// <summary>
        /// 显示指定帮助键的帮助
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <param name="helpKey">帮助键</param>
        public static void ShowHelp(this Control control, string helpKey)
        {
            if (control == null || string.IsNullOrEmpty(helpKey)) return;

            var context = new HelpContext
            {
                Level = HelpLevel.Control,
                TargetControl = control,
                ControlName = control.Name,
                HelpKey = helpKey
            };

            HelpManager.Instance.ShowHelp(context);
        }

        /// <summary>
        /// 显示字段级别的帮助
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="fieldName">字段名称</param>
        public static void ShowFieldHelp(this Control control, Type entityType, string fieldName)
        {
            if (control == null || entityType == null || string.IsNullOrEmpty(fieldName)) return;

            var context = new HelpContext
            {
                Level = HelpLevel.Field,
                TargetControl = control,
                EntityType = entityType,
                FieldName = fieldName
            };

            HelpManager.Instance.ShowHelp(context);
        }

        #endregion

        #region 扩展方法 - 帮助气泡

        /// <summary>
        /// 显示简短的帮助气泡
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <param name="message">提示消息</param>
        /// <param name="duration">持续时间(毫秒),0表示不自动隐藏</param>
        public static void ShowHelpBubble(this Control control, string message, int duration = 5000)
        {
            if (control == null || string.IsNullOrEmpty(message)) return;

            var tooltip = new HelpTooltip
            {
                Timeout = duration,
                Text = message
            };

            tooltip.Show(message, control);
        }

        #endregion

        #region 事件处理 - 控件事件

        /// <summary>
        /// 控件悬停事件处理
        /// </summary>
        private static void Control_MouseHover(object sender, EventArgs e)
        {
            if (!(sender is Control control)) return;

            try
            {
                var context = HelpContext.FromControl(control);
                HelpManager.Instance.ShowHelp(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"控件悬停事件处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 控件鼠标离开事件处理
        /// </summary>
        private static void Control_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                // 隐藏提示气泡的具体实现
                // 这里需要访问HelpTooltip实例
                // 由于扩展方法的限制,这个功能需要在HelpManager中实现
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"控件鼠标离开事件处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 控件KeyDown事件处理
        /// </summary>
        private static void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (sender is Control control)
                {
                    e.Handled = true;
                    // 获取当前焦点的控件
                    Control focusedControl = GetFocusedControl(control);
                    if (focusedControl != null)
                    {
                        HelpManager.Instance.ShowControlHelp(focusedControl);
                    }
                    else
                    {
                        // 如果没有焦点控件,显示控件本身的帮助
                        HelpManager.Instance.ShowControlHelp(control);
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前焦点的控件
        /// </summary>
        private static Control GetFocusedControl(Control control)
        {
            if (control is Form form)
            {
                return form.ActiveControl;
            }

            // 对于UserControl,递归查找焦点控件
            if (control is ContainerControl container && container.ActiveControl != null)
            {
                return container.ActiveControl;
            }

            return null;
        }

        #endregion
    }

    /// <summary>
    /// 窗体帮助扩展方法
    /// 为窗体提供便捷的帮助功能扩展
    /// </summary>
    public static class FormHelpExtensions
    {
        #region 扩展方法 - 帮助显示

        /// <summary>
        /// 为窗体启用F1帮助
        /// </summary>
        /// <param name="form">目标窗体</param>
        public static void EnableF1Help(this Form form)
        {
            if (form == null) return;

            form.KeyPreview = true;
            form.KeyDown += Form_KeyDown;
        }

        /// <summary>
        /// 为窗体禁用F1帮助
        /// </summary>
        /// <param name="form">目标窗体</param>
        public static void DisableF1Help(this Form form)
        {
            if (form == null) return;

            form.KeyDown -= Form_KeyDown;
        }

        /// <summary>
        /// 显示窗体帮助
        /// </summary>
        /// <param name="form">目标窗体</param>
        public static void ShowFormHelp(this Form form)
        {
            if (form == null) return;
            HelpManager.Instance.ShowFormHelp(form);
        }

        /// <summary>
        /// 显示当前焦点控件的帮助
        /// </summary>
        /// <param name="form">目标窗体</param>
        public static void ShowFocusedControlHelp(this Form form)
        {
            if (form == null) return;

            Control focusedControl = GetFocusedControl(form);
            if (focusedControl != null)
            {
                HelpManager.Instance.ShowControlHelp(focusedControl);
            }
        }

        #endregion

        #region 扩展方法 - 帮助面板

        /// <summary>
        /// 在窗体中显示帮助面板
        /// </summary>
        /// <param name="form">目标窗体</param>
        /// <param name="content">帮助内容</param>
        /// <param name="context">帮助上下文</param>
        public static void ShowHelpPanel(this Form form, string content, HelpContext context = null)
        {
            if (form == null || string.IsNullOrEmpty(content)) return;

            try
            {
                var helpPanel = new HelpPanel(content, context);
                helpPanel.ShowDialog(form);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示帮助面板失败: {ex.Message}");
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取窗体中当前焦点的控件
        /// </summary>
        /// <param name="form">目标窗体</param>
        /// <returns>焦点控件</returns>
        private static Control GetFocusedControl(Form form)
        {
            if (form.ActiveControl != null)
            {
                return form.ActiveControl;
            }

            // 如果没有活动控件,尝试遍历查找
            return FindFocusedControl(form);
        }

        /// <summary>
        /// 递归查找焦点控件
        /// </summary>
        /// <param name="control">起始控件</param>
        /// <returns>焦点控件</returns>
        private static Control FindFocusedControl(Control control)
        {
            if (control.Focused)
            {
                return control;
            }

            foreach (Control child in control.Controls)
            {
                Control focused = FindFocusedControl(child);
                if (focused != null)
                {
                    return focused;
                }
            }

            return null;
        }

        /// <summary>
        /// 窗体KeyDown事件处理
        /// </summary>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (sender is Form form)
                {
                    e.Handled = true;
                    form.ShowFocusedControlHelp();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 工具条帮助扩展方法
    /// 为工具条按钮提供便捷的帮助功能扩展
    /// </summary>
    public static class ToolStripItemHelpExtensions
    {
        #region 扩展方法 - 帮助设置

        /// <summary>
        /// 为工具条项设置帮助键
        /// </summary>
        /// <param name="item">工具条项</param>
        /// <param name="helpKey">帮助键</param>
        public static void SetHelpKey(this ToolStripItem item, string helpKey)
        {
            if (item == null || string.IsNullOrEmpty(helpKey)) return;

            item.Tag = $"HelpKey:{helpKey}";
        }

        /// <summary>
        /// 获取工具条项的帮助键
        /// </summary>
        /// <param name="item">工具条项</param>
        /// <returns>帮助键,未设置则返回null</returns>
        public static string GetHelpKey(this ToolStripItem item)
        {
            if (item?.Tag == null) return null;

            if (item.Tag is string tagString && tagString.StartsWith("HelpKey:"))
            {
                return tagString.Substring("HelpKey:".Length);
            }

            return null;
        }

        #endregion

        #region 扩展方法 - 帮助显示

        /// <summary>
        /// 显示工具条项的帮助
        /// </summary>
        /// <param name="item">工具条项</param>
        public static void ShowHelp(this ToolStripItem item)
        {
            if (item == null) return;

            string helpKey = item.GetHelpKey();
            if (!string.IsNullOrEmpty(helpKey))
            {
                var context = new HelpContext
                {
                    Level = HelpLevel.Control,
                    HelpKey = helpKey,
                    ControlName = item.Name
                };

                HelpManager.Instance.ShowHelp(context);
            }
        }

        #endregion

        #region 扩展方法 - 工具提示

        /// <summary>
        /// 为工具条项设置工具提示和帮助键
        /// </summary>
        /// <param name="item">工具条项</param>
        /// <param name="tooltipText">工具提示文本</param>
        /// <param name="helpKey">帮助键(可选)</param>
        public static void SetHelpTooltip(this ToolStripItem item, string tooltipText, string helpKey = null)
        {
            if (item == null) return;

            item.ToolTipText = tooltipText;

            if (!string.IsNullOrEmpty(helpKey))
            {
                item.SetHelpKey(helpKey);
            }
        }

        #endregion
    }
}
