using FastReport.Messaging.Xmpp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.CommonUI
{
    public partial class ExcelProgressForm : Form
    {
        private ProgressBar progressBar;
        private Label label;
        private string v;

        public ExcelProgressForm(string message)
        {
            InitializeComponent();
            this.Text = "请稍候";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(300, 100);
            this.ControlBox = false;

            label = new Label
            {
                Text = message,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 30
            };

            progressBar = new ProgressBar
            {
                Dock = DockStyle.Fill,
                Style = ProgressBarStyle.Continuous,
                Minimum = 0,
                Maximum = 100
            };

            this.Controls.Add(progressBar);
            this.Controls.Add(label);
        }

        

        public void SetProgress(int percent)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<int>(SetProgress), percent);
            }
            else
            {
                progressBar.Value = Math.Min(100, Math.Max(0, percent));
                label.Text = $"正在导出数据... {percent}%";
            }
        }
    }
}
