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
            dataGridView1.CellPainting += dataGridView1_CellPainting;
            BlacklistManager.Initialize(SynchronizationContext.Current); // 捕获UI线程上下文
        }

        private void 解除IPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var ip = dataGridView1.SelectedRows[0].Cells["IP地址"].Value.ToString();
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
            // 先取消绑定
            dataGridView1.DataSource = null;

            // 设置自动生成列
            dataGridView1.AutoGenerateColumns = true;

            // 重新绑定
            dataGridView1.DataSource = BlacklistManager.BannedList;

            // 禁用行头
            dataGridView1.RowHeadersVisible = false;

            // 设置选择模式
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void StartAutoRefresh()
        {
            var timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += (_, _) =>
            {
                // 安全刷新
                try
                {
                    if (dataGridView1.InvokeRequired)
                    {
                        dataGridView1.Invoke(new Action(() => dataGridView1.Refresh()));
                    }
                    else
                    {
                        dataGridView1.Refresh();
                    }
                }
                catch { /* 忽略刷新错误 */ }
            };
            timer.Start();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 更安全的刷新方式
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = BlacklistManager.BannedList;
            }
            catch { /* 忽略刷新错误 */ }

        }

        private void 添加IPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frmInput = new frmInput())
            {
                frmInput.Text = "请输入要禁止的IP和小时数（如: 192.168.0.27:1）";
                frmInput.txtInputContent.Text = "192.168.0.27:10";

                if (frmInput.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var parts = frmInput.txtInputContent.Text.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int hour))
                        {
                            BlacklistManager.BanIp(parts[0], TimeSpan.FromHours(hour));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"添加IP失败: {ex.Message}");
                    }
                }
            }
        }
        // 添加这个方法来处理DataGridView的DataError
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 忽略数据错误
            e.ThrowException = false;
        }
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            #region 画行号

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewPaintParts paintParts =
                    e.PaintParts & ~DataGridViewPaintParts.Focus;

                e.Paint(e.ClipBounds, paintParts);
                e.Handled = true;
            }

            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }

            #endregion

            //画总行数行号
            if (e.ColumnIndex < 0 && e.RowIndex < 0)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (this.dataGridView1.Rows.Count + "#").ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

    }
}
