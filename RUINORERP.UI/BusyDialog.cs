using RUINORERP.Common.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI
{
    [NoWantIOC]
   
    public partial class BusyDialog : Form
    {

        public BusyDialog()
        {
            
        }

        private static BusyDialog _instance;

        public BusyDialog(string message)
        {

            InitializeComponent();
            Label lblMessage = new Label { Dock = DockStyle.Fill };
            ProgressBar progressBar = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                Dock = DockStyle.Fill,
                Height = 20
            };

            this.Controls.Add(lblMessage);
            this.Controls.Add(progressBar);
            this.ClientSize = new Size(300, 100);
            this.FormBorderStyle = FormBorderStyle.None;
            lblMessage.Text = message;
            StartPosition = FormStartPosition.CenterParent;
        }


        public static void ShowDialog(Form owner, string message, Action work)
        {
            _instance = new BusyDialog(message);
            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) => work();
            worker.RunWorkerCompleted += (s, e) => _instance.Close();

            worker.RunWorkerAsync();
            _instance.ShowDialog(owner);
        }

        public static async Task ShowAsync(Func<Task> asyncWork, string message = "Processing...")
        {
            var dialog = new BusyDialog(message);
            dialog.Show();

            try
            {
                await asyncWork();
            }
            finally
            {
                dialog.Close();
            }
        }
    }
}
