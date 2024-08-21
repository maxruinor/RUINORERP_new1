using System;
using System.Windows.Forms;

namespace HLH.WinControl
{


    /// <summary>
    /// 只能输入数字
    /// </summary>
    public class NumberTextBox : TextBox
    {
        protected override void WndProc(ref System.Windows.Forms.Message msg)
        {
            if (msg.Msg == 0x0102 && !Char.IsControl((char)msg.WParam))
            {
                if (char.IsNumber((char)msg.WParam))
                {
                    base.WndProc(ref msg);
                }
                if (msg.WParam == (IntPtr)46)
                {
                    if (this.Text.Contains("."))
                    {
                        return;
                    }
                    else
                    {
                        base.WndProc(ref msg);
                    }
                }

            }
            else
            {
                base.WndProc(ref msg);
            }

        }
    }

}
