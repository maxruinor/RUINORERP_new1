using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI
{
    public partial class ProgressFormBar : Form
    {
        private readonly BackgroundWorker _worker;
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        public ProgressFormBar(string title = "处理中...")
        {
            InitializeComponent();
            Text = title;

            // 初始化BackgroundWorker
            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _worker.DoWork += Worker_DoWork;
            _worker.ProgressChanged += Worker_ProgressChanged;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        // 绑定外部任务的方法
        public void RunAsync(Action<IProgress<string>, CancellationToken> action)
        {
            _worker.RunWorkerAsync(action);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var action = e.Argument as Action<IProgress<string>, CancellationToken>;
            var progress = new Progress<string>(msg => _worker.ReportProgress(0, msg));
            action?.Invoke(progress, CancellationTokenSource.Token);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is string msg)
            {
                lblStatus.Text = msg;
                progressBar1.Value = e.ProgressPercentage;
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show($"操作失败：{e.Error.Message}");
            }
            DialogResult = e.Cancelled ? DialogResult.Cancel : DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancellationTokenSource.Cancel();
            _worker.CancelAsync();
        }
    }
}
