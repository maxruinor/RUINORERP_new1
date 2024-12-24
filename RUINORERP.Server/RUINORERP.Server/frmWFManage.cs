using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.BizService;
using RUINORERP.Server.ServerSession;
using RUINORERP.Server.ToolsUI;
using SuperSocket.Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server
{
    public partial class frmWFManage : frmBase
    {
        public frmWFManage()
        {
            InitializeComponent();
        }
        public ObservableCollection<ReminderData> WFInfos = new ObservableCollection<ReminderData>();
        private void frmUserManage_Load(object sender, EventArgs e)
        {
            dataGridView1.VirtualMode = false;
            // 订阅CollectionChanged事件
            WFInfos.CollectionChanged -= ServerReminderDatas_CollectionChanged;
            WFInfos.CollectionChanged += ServerReminderDatas_CollectionChanged;

            RefreshData();


        }

        // 刷新数据的方法
        public void RefreshData()
        {
            // 这里你可以执行数据更新的逻辑，例如从数据库重新读取数据
            // 然后重新绑定数据源到DataGridView
            ServerBizDataBindingSource.DataSource = WFInfos;
            //dataGridView1.DataSource = null;
            dataGridView1.DataSource = ServerBizDataBindingSource;

           // DataGridViewColumn LastbeatTime = dataGridView1.Columns["最后心跳时间" + "DataGridViewTextBoxColumn"];

            // 获取SessionID列
           // DataGridViewColumn sessionIDColumn = dataGridView1.Columns["sessionIDDataGridViewTextBoxColumn"];

            // 如果找到了该列，将其设置为不可见
            //if (sessionIDColumn != null)
            //{
            //    // sessionIDColumn.Visible = false;
            //}
        }


        public void ServerReminderDatas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                // 在这里处理集合变化的逻辑
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        if (e.NewItems.Count == 1)
                        {
                            ReminderData newItem = e.NewItems[0] as ReminderData;
                            if (!WFInfos.Contains(newItem))
                            {
                                WFInfos.Add(newItem);                 // 处理删除元素的逻辑
                            }
                            if (!ServerBizDataBindingSource.Contains(newItem))
                            {
                                ServerBizDataBindingSource.Add(newItem);
                            }

                        }

                        //SessionforBiz biz = frmMain.Instance.ReminderBizDataList[((ServerReminderData)e.NewItems[e.NewStartingIndex]).SessionId];
                        //ServerReminderDataBindingSource.Add(((ServerReminderData)e.NewItems[e.NewStartingIndex]));
                        // 处理添加元素的逻辑
                        //ServerReminderDataBindingSource.DataSource = frmMain.Instance.ServerReminderDatas;
                        //dataGridView1.DataSource = null;
                        //dataGridView1.DataSource = ServerReminderDataBindingSource;

                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        if (e.OldItems.Count == 1)
                        {
                            ReminderData old = e.OldItems[0] as ReminderData;
                            if (WFInfos.Contains(old))
                            {
                                WFInfos.Remove(old);                 // 处理删除元素的逻辑
                            }
                            if (ServerBizDataBindingSource.Contains(old))
                            {
                                ServerBizDataBindingSource.Remove(old);
                            }

                        }

                        break;
                    default:
                        // return;
                        break;
                        // 可以根据需要处理其他事件类型
                }
                //if (ServerReminderDatas.Count == 0)
                //{
                //   // dataGridView1.Rows.Clear();
                //}
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ServerReminderDatas_CollectionChanged时出错" + ex.Message);

            }


        }


        /// <summary>
        /// 刷新DataGridView控件
        /// 如果当前线程不是UI线程，它会使用Invoke来调用自己。如果当前线程是UI线程，它将直接更新UI。
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                if (this.dataGridView1.InvokeRequired)
                {
                    // 如果当前线程不是UI线程，则使用Invoke来更新UI
                    this.dataGridView1.Invoke(new MethodInvoker(RefreshDataGridView));
                }
                else
                {
                    // 如果当前线程是UI线程，则直接更新UI
                    this.ServerBizDataBindingSource.ResetBindings(false);
                    this.dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("RefreshDataGridView时出错" + ex.Message);

            }

        }

        public void ServerReminderData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            try
            {
                if (sender != null && sender is ReminderData info)
                {
                    // 当用户信息发生变化时，刷新数据
                    if (frmMain.Instance.ReminderBizDataList.ContainsKey(info.BizPrimaryKey))
                    {
                        ReminderData biz = frmMain.Instance.ReminderBizDataList[info.BizPrimaryKey];
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].DataBoundItem is ReminderData ServerReminderData)
                            {
                                if (ServerReminderData.BizPrimaryKey == biz.BizPrimaryKey)
                                {
                                    foreach (DataGridViewColumn dc in dataGridView1.Columns)
                                    {
                                        if (dc.DataPropertyName == e.PropertyName)
                                        {
                                            dataGridView1.Rows[i].Cells[dc.Name].Value = HLH.Lib.Helper.ReflectionHelper.GetPropertyValue(info, e.PropertyName);
                                            dataGridView1.Refresh();
                                            // dataGridView1.PerformLayout();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        //RefreshData();
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ServerReminderData_PropertyChanged时出错" + ex.Message);

            }
        }


        private  void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (e.ClickedItem.Text == "工作流测试")
            {
                frmWorkFlowManage frm = Startup.GetFromFac<frmWorkFlowManage>();
                frm.MdiParent = this;
                frm.Show();
                frm.Activate();
            }
            
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void frmUserManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

  

       

 
 
    }
}



