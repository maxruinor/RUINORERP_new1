using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Network.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using RUINORERP.Model.TransModel;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Helper;
using RUINORERP.PacketSpec.Enums;

namespace RUINORERP.Server
{
    public partial class frmMemoryDataViewer : frmBase
    {
        private readonly ISessionService _sessionService;

        public frmMemoryDataViewer()
        {
            InitializeComponent();
            _sessionService = Startup.GetFromFac<ISessionService>();
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
                    dataGridView1.DataSource = list.ToBindingSortCollection();

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

        private void 发送提醒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if( dataGridView1.CurrentRow!=null && dataGridView1.CurrentRow.DataBoundItem!=null)
            {
                if (dataGridView1.CurrentRow.DataBoundItem is ReminderData exData)
                {
                   // ServerReminderData olddata = exData.DeepCloneObject<ServerReminderData>();
                    var sessions = _sessionService.GetAllUserSessions();
                    foreach (var session in sessions)
                    {
                        // 检查用户是否在接收者列表中
                        // 注意：这里需要根据实际的SessionInfo结构来获取Employee_ID
                        if (exData.ReceiverUserIDs.Contains(session.UserInfo.UserID)) // 假设SessionInfo有UserID属性对应Employee_ID
                        {
                            try
                            {
                                // 发送提醒命令
                                var reminderJson = JsonConvert.SerializeObject(exData, new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });
                                
                                var success = _sessionService.SendCommandToSession(session.SessionID, "SEND_REMINDER", reminderJson);
                                if (success)
                                {
                                    exData.RemindTimes++;
                                    if(frmMain.Instance.ReminderBizDataList.TryUpdate(exData.BizPrimaryKey, exData, exData))
                                    {
                                        //更新成功
                                    }
                                    
                                    if (frmMain.Instance.IsDebug)
                                    {
                                        frmMain.Instance.PrintInfoLog($"工作流提醒推送到{session.UserName}");
                                    }
                                }
                                else
                                {
                                    frmMain.Instance.PrintErrorLog($"工作流提醒推送到{session.UserName}失败");
                                }
                            }
                            catch (Exception ex)
                            {
                                frmMain.Instance.PrintInfoLog("服务器工作流提醒推送分布失败:" + session.UserName + ex.Message);
                            }
                        }
                    }
                }
            }
          
        }
    }
}
