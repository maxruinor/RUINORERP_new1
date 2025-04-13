using RUINORERP.Server.Comm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server
{
    public partial class frmBlacklist : Form
    {
        public frmBlacklist()
        {
            InitializeComponent();
            BlacklistManager.Initialize(SynchronizationContext.Current); // 捕获UI线程上下文
        }

        private void 解除IPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var ip = dataGridView1.SelectedRows[0].Cells["IP"].Value.ToString();
                BlacklistManager.UnbanIp(ip);
            }

        }

        private void frmBlacklist_Load(object sender, EventArgs e)
        {
            SetupBlacklistGrid();
            StartAutoRefresh();
        }
        private void SetupBlacklistGrid()
        {
            // 绑定数据源
            dataGridView1.DataSource = BlacklistManager.BannedList;
            // 配置列
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.AddRange(
                new DataGridViewTextBoxColumn { DataPropertyName = "IP", HeaderText = "IP地址" },
                new DataGridViewTextBoxColumn { DataPropertyName = "ExpiryTime", HeaderText = "解封时间" },
                new DataGridViewTextBoxColumn { DataPropertyName = "RemainingTime", HeaderText = "剩余时间" }
            );
        }

        private void StartAutoRefresh()
        {
            var timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += (_, _) =>
            {
                // 强制刷新剩余时间显示
                dataGridView1.Refresh();
            };
            timer.Start();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource=BlacklistManager.BannedList;
        }
    }
}
