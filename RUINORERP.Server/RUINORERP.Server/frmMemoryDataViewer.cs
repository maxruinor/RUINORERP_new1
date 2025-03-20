using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
using TransInstruction.DataPortal;
using TransInstruction;
using RUINORERP.Model.TransModel;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Helper;

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
                    foreach (var item in frmMain.Instance.sessionListBiz)
                    {
                        if (exData.ReceiverEmployeeIDs.Contains(item.Value.User.Employee_ID))
                        {
                            try
                            {
                                exData.RemindTimes++;
                                //  WorkflowServiceReceiver.发送工作流提醒();
                                OriginalData exMsg = new OriginalData();
                                exMsg.cmd = (byte)ServerCmdEnum.工作流提醒推送;
                                exMsg.One = null;

                                //这种可以写一个扩展方法
                                ByteBuff tx = new ByteBuff(100);
                                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                tx.PushString(sendtime);
                                string json = JsonConvert.SerializeObject(exData,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                        });

                                tx.PushString(json);
                                //tx.PushString("【系统提醒】" + System.DateTime.Now.ToString());//发送者
                                //tx.PushString(item.Value.SessionID);
                                //tx.PushString(exData.RemindSubject);
                                //tx.PushString(exData.ReminderContent);
                                tx.PushBool(true);//是否强制弹窗
                                exMsg.Two = tx.toByte();
                                item.Value.AddSendData(exMsg);

                                if(frmMain.Instance.ReminderBizDataList.TryUpdate(exData.BizPrimaryKey, exData, exData))
                                {
                                    //更新成功
                                }
                                else
                                {

                                }
                                if (frmMain.Instance.IsDebug)
                                {
                                    frmMain.Instance.PrintInfoLog($"工作流提醒推送到{item.Value.User.用户名}");
                                }

                            }
                            catch (Exception ex)
                            {
                                frmMain.Instance.PrintInfoLog("服务器工作流提醒推送分布失败:" + item.Value.User.用户名 + ex.Message);
                            }
                        }
                        //如果不注释，相同的员工有多个帐号时。员工只会提醒一个。
                        //else
                        //{
                        //    continue;
                        //}


                    }
                }
            }
          
        }
    }
}
