using Krypton.Toolkit;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem.Core;
using RUINORERP.UI.HelpSystem.Extensions;

namespace RUINORERP.UI.BaseForm
{
    public partial class frmBase : KryptonForm
    {
        public frmBase()
        {
            InitializeComponent();

            // 初始化帮助系统
            InitializeHelpSystem();
        }

        #region 帮助系统集成

        /// <summary>
        /// 是否启用智能帮助
        /// </summary>
        [Category("帮助系统")]
        [Description("是否启用智能帮助功能")]
        public bool EnableSmartHelp { get; set; } = true;

        /// <summary>
        /// 窗体帮助键
        /// </summary>
        [Category("帮助系统")]
        [Description("窗体帮助键,留空则使用窗体类型名称")]
        public string FormHelpKey { get; set; }

        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        protected virtual void InitializeHelpSystem()
        {
            if (!EnableSmartHelp) return;

            try
            {
                // 为控件启用智能提示
                HelpManager.Instance.EnableSmartTooltipForAll(this, FormHelpKey);

                // 启用F1帮助
                this.EnableF1Help();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化帮助系统失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示窗体帮助
        /// </summary>
        public void ShowFormHelp()
        {
            if (!EnableSmartHelp) return;
            HelpManager.Instance.ShowControlHelp(this);
        }

        #endregion

        /// <summary>
        /// esc退出窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        this.Close();//csc关闭窗体
                        break;
                    case Keys.F1:
                        // 显示帮助 - 优先显示当前焦点控件的帮助
                        var focusedControl = this.ActiveControl;
                        HelpManager.Instance.ShowControlHelp(focusedControl);
                        return true;
                    case Keys.F2:
                        // 显示帮助系统主窗体
                        //if (HelpManager.Config.IsHelpSystemEnabled)
                        //{
                        //    this.ShowHelpSystemForm();
                        //    return true;
                        //}
                        break;
                }

            }
            return base.ProcessCmdKey(ref msg, keyData); // 调用基类方法处理其他按键
        }

    }
}