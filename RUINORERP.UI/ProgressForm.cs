using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI
{
    public partial class ProgressForm : KryptonForm
    {
        public event EventHandler CancelRequested;
        public void UpdateTimeInfo(TimeSpan elapsed)
        {
            lblElapsed.Text = $"已用时间：{elapsed:mm\\:ss}";
        }

    
        public ProgressForm(string title, bool allowCancel)
        {
            InitializeComponent();
            Text = title;
            btnCancel.Visible = allowCancel;
            progressBar1.Style = ProgressBarStyle.Continuous;
        }

        public void UpdateProgress(int percentage, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateProgress(percentage, message)));
                return;
            }

            lblStatus.Text = message ?? lblStatus.Text;

            if (percentage >= 0)
            {
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Value = Math.Min(Math.Max(percentage, 0), 100);
            }
            else
            {
                progressBar1.Style = ProgressBarStyle.Marquee;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}