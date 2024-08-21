using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}
namespace RUINOR.WinFormsUI.RegTextBox
{

    /// <summary>
    /// 创建：天智海网络
    /// 邮箱：service@tzhtec.com
    /// 网址：http://www.tzhtec.com
    /// 功能描述：BaseButton扩展方法
    /// </summary>
    public static class TzhtecButton
    {
        private static Hashtable ht = new Hashtable();  // 存储相关控件与对应的按钮关系
        private static Hashtable eventHt = new Hashtable(); // 存储按钮原有事件
        private static ToolTip _tooltip;   // 控件的tooltip

        /// <summary>
        /// 将控件添加到控制列表
        /// </summary>
        /// <param name="baseButton"></param>
        /// <param name="control"></param>
        public static void AddControl(this ButtonBase baseButton, Control control)
        {
            if (control as ITzhtecControl == null) return;
            if (!ht.ContainsKey(control))
            {
                ht.Add(control, baseButton);
                HookButtonClickEvent(baseButton);
                control.TextChanged += new EventHandler(control_TextChanged);
            }
        }

        /// <summary>
        /// 将控件从控制列表移除
        /// </summary>
        /// <param name="baseButton"></param>
        /// <param name="control"></param>
        public static void RemoveControl(this ButtonBase baseButton, Control control)
        {
            if (control as ITzhtecControl == null) return;
            if (ht.ContainsKey(control))
            {
                ht.Remove(control);
                RemoveHookEvent(baseButton);
                control.TextChanged -= new EventHandler(control_TextChanged);
            }
        }

        /// <summary>
        /// 文本改变消除提示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void control_TextChanged(object sender, EventArgs e)
        {
            if (_tooltip != null) _tooltip.Dispose();
        }

        /// <summary>
        /// 捕获按钮click事件
        /// </summary>
        /// <param name="baseButton"></param>
        private static void HookButtonClickEvent(ButtonBase baseButton)
        {
            if (baseButton == null) return;
            if (eventHt.Contains(baseButton)) return;
            PropertyInfo pi = baseButton.GetType().GetProperty
("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            if (pi != null)
            {
                //获得事件列表
                EventHandlerList eventList = (EventHandlerList)pi.GetValue
(baseButton, null);
                if (eventList != null && eventList is EventHandlerList)
                {
                    //查找按钮点击事件
                    FieldInfo fi = (typeof(Control)).GetField("EventClick",
BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                    if (fi != null)
                    {
                        EventHandler eh = eventList[fi.GetValue(baseButton)] as
EventHandler;   //记录原有按钮事件
                        if (eh != null) baseButton.Click -= eh; //移除原有的按钮事件
                        baseButton.Click += new EventHandler(button_Click); //替换按钮事件
                        eventHt.Add(baseButton, eh);
                    }
                }
            }
        }

        /// <summary>
        /// 还原按钮click事件
        /// </summary>
        /// <param name="baseButton"></param>
        private static void RemoveHookEvent(ButtonBase baseButton)
        {
            if (!ht.ContainsValue(baseButton))
            {
                foreach (DictionaryEntry de in eventHt)  //遍历hashtable得到与之相关的按钮
                {
                    if (de.Key == baseButton)
                    {
                        EventHandler eh = de.Value as EventHandler;
                        baseButton.Click -= button_Click;   //移除替换事件
                        baseButton.Click += eh; // 还原事件
                        break;
                    }
                }
                eventHt.Remove(baseButton);
            }
        }

        /// <summary>
        /// 替换按钮的click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void button_Click(object sender, EventArgs e)
        {
            if (sender == null) return;
            // 获取当前按钮相关的所有控件，并根据tabindex升序排序
            //var ctls = (from item in ht.Cast<DictionaryEntry>()
            //            where item.Value == sender as ButtonBase
            //            select item.Key as Control).Distinct().OrderBy(c => c.TabIndex);


            List<Control> ctlist = new List<Control>();
            foreach (DictionaryEntry kv in ht)
            {
                if (kv.Value == sender as ButtonBase)
                {
                    ctlist.Add(kv.Key as Control);
                }
            }

            ctlist.Sort(delegate(Control x, Control y)
            {
                return x.TabIndex.CompareTo(y.TabIndex);
            });

            foreach (var ctl in ctlist)
            {
                // 未激活或者已隐藏控件不验证
                if (!(ctl.Enabled && ctl.Visible && ctl.Parent.Visible &&
ctl.Parent.Enabled)) continue;
                ITzhtecControl c = ctl as ITzhtecControl;
                if (c != null)
                {
                    if (!EmptyValidate(ctl)) return;

                    if (!RegexExpressionValidate(ctl)) return;

                    if (!InvokeCustomerEvent(ctl)) return;
                }
            }

            foreach (DictionaryEntry de in eventHt)  //遍历hashtable得到与之相关的按钮
            {
                if (de.Key == sender as ButtonBase)
                {
                    EventHandler eh = de.Value as EventHandler;
                    if (eh != null) eh(sender, e);
                    break;
                }
            }
        }

        /// <summary>
        /// 非空验证
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        private static bool EmptyValidate(Control ctl)
        {
            ITzhtecControl c = ctl as ITzhtecControl;

            if (!c.AllowEmpty)
            {
                if ((c.RemoveSpace && ctl.Text.Trim() == "") || ctl.Text == "")
                {
                    ShowErrorMessage(ctl, c.EmptyMessage);
                    c.SelectAll();
                    ctl.Focus();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 正则验证
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        private static bool RegexExpressionValidate(Control ctl)
        {
            ITzhtecControl c = ctl as ITzhtecControl;

            if (!((c.RemoveSpace && ctl.Text.Trim() == "") ||
ctl.Text == ""))
            {
                if (!string.IsNullOrEmpty(c.RegexExpression) &&
                    !Regex.IsMatch((c.RemoveSpace ? ctl.Text.Trim() : ctl.Text),
                    c.RegexExpression))
                {
                    ShowErrorMessage(ctl, c.ErrorMessage);
                    c.SelectAll();
                    ctl.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 自定义验证
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        private static bool InvokeCustomerEvent(Control ctl)
        {
            PropertyInfo pi = ctl.GetType().GetProperty("Events",
BindingFlags.Instance | BindingFlags.NonPublic);
            if (pi != null)
            {
                EventHandlerList ehl = (EventHandlerList)pi.GetValue(ctl, null);
                if (ehl != null)
                {   // 得到自定义验证事件
                    FieldInfo fi = ctl.GetType().GetField("CustomerValidated",
BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (fi != null)
                    {
                        Delegate d = fi.GetValue(ctl) as Delegate;
                        if (d != null)
                        {
                            CustomerEventArgs ce = new CustomerEventArgs();
                            ce.Value = ctl.Text;
                            ce.Validated = true;
                            d.DynamicInvoke(ctl, ce);   // 执行自定义验证方法
                            if (!ce.Validated)
                            {
                                ShowErrorMessage(ctl, ce.ErrorMessage);
                                (ctl as ITzhtecControl).SelectAll();
                                ctl.Focus();
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="err"></param>
        private static void ShowErrorMessage(Control ctl, string err)
        {
            if (_tooltip != null) _tooltip.Dispose(); // 如果tooltip已经存在则销毁
            _tooltip = new ToolTip();
            _tooltip.ToolTipIcon = ToolTipIcon.Warning;
            _tooltip.IsBalloon = true;
            _tooltip.ToolTipTitle = "提示";
            _tooltip.AutoPopDelay = 5000;
            //得到信息显示的行数
            if (err == null) err = " ";
            int l = err.Split(new string[] { "\r\n", "\r", "\n" },
StringSplitOptions.RemoveEmptyEntries).Length;
            _tooltip.Show(err, ctl, new System.Drawing.Point(10, -47 - l * 18 +
(ctl.Height - 21) / 2));
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="err"></param>
        private static void ShowErrorMessage(Control ctl, string err, int showTime)
        {
            if (_tooltip != null) _tooltip.Dispose(); // 如果tooltip已经存在则销毁
            _tooltip = new ToolTip();
            _tooltip.ToolTipIcon = ToolTipIcon.Warning;
            _tooltip.IsBalloon = true;
            _tooltip.ShowAlways = false;
            _tooltip.ToolTipTitle = "提示";
            _tooltip.AutoPopDelay = showTime;
            //得到信息显示的行数
            if (err == null) err = " ";
            int l = err.Split(new string[] { "\r\n", "\r", "\n" },
StringSplitOptions.RemoveEmptyEntries).Length;
            _tooltip.Show(err, ctl, new System.Drawing.Point(10, -47 - l * 18 +
(ctl.Height - 21) / 2));
        }
    }
}

