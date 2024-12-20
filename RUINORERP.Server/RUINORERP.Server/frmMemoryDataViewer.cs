using Microsoft.Extensions.Options;
using RUINORERP.Server.BizService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server
{
    public partial class frmMemoryDataViewer : frmBase
    {
        public frmMemoryDataViewer()
        {
            InitializeComponent();
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


        private void treeViewDataType_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Node.Text)
            {
                case "提醒数据":
                    dataGridView1.DataSource = null;
                    DataServiceChannel loadService = Startup.GetFromFac<DataServiceChannel>();
                    loadService.LoadCRMFollowUpPlansData(frmMain.Instance.ReminderBizDataList);
                    var list = frmMain.Instance.ReminderBizDataList.Values.ToList();
                    dataGridView1.DataSource = list;

                    frmMain.Instance.PrintMsg("提醒数据已刷新" + list.Count);

                    break;
                default:
                    break;
            }

            if (true)
            {

            }

        }

        private void tsbtnRefresh_Click(object sender, EventArgs e)
        {

            MessageBox.Show(frmMain.Instance.Globalconfig.CurrentValue.SomeSetting);

            //IOptionsMonitor<ConfigOptions> optionsMonitor = new OptionsMonitor<ConfigOptions>();
            //optionsMonitor.OnChange(config =>
            //{
            //    // 更新界面
            //    UpdateTreeView(config);
            //});
        }

        private void frmMemoryDataViewer_Load(object sender, EventArgs e)
        {
            //加载数据类型
            treeViewDataType.Nodes.Clear();
            TreeNode treeNode = new TreeNode();
            treeNode.Text = "提醒数据";
            treeViewDataType.Nodes.Add(treeNode);
        }

        private void 加载数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
