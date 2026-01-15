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
using RUINORERP.UI.HelpSystem;
using RUINORERP.UI.HelpSystem.Core;

namespace RUINORERP.UI.BaseForm
{
    public partial class frmBase : KryptonForm
    {
        public frmBase()
        {
            InitializeComponent();


        }

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