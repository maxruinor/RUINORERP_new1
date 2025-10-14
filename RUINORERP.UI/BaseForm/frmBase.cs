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
using RUINORERP.UI.Common.HelpSystem;

namespace RUINORERP.UI.BaseForm
{
    public partial class frmBase : KryptonForm
    {
        public frmBase()
        {
            InitializeComponent();
            
            // 启用F1帮助功能
            this.EnableF1Help();
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
                        // 显示帮助
                        HelpManager.ShowHelp(this);
                        return true;
                }

            }
            return false;
        }

    }
}