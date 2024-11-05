using HLH.Lib.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Comm;
using RUINORERP.Server.ServerSession;
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
    public partial class frmCacheManage : frmBase
    {
        public frmCacheManage()
        {
            InitializeComponent();
        }

        private void frmCacheManagement_Load(object sender, EventArgs e)
        {

            LoadCacheToUI();
        }

        private void listBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var CacheList = BizCacheHelper.Manager.CacheEntityList.Get(listBoxTableList.SelectedItem.ToString());
            dataGridView1.DataSource = CacheList;

        }

        private void toolStripButton刷新缓存_Click(object sender, EventArgs e)
        {
            //这里添加所有缓存
            LoadCacheToUI();
            frmMain.Instance._logger.LogError("启动了服务器5566");
            frmMain.Instance._logger.Error("启动了服务器55667788");
            frmMain.Instance._logger.LogInformation("123131");

        }


        private void LoadCacheToUI()
        {
            //加载所有用户
            cmbUser.Items.Clear();
            foreach (var user in frmMain.Instance.sessionListBiz)
            {
                SessionforBiz sessionforBiz = user.Value as SessionforBiz;
                SuperValue skv = new SuperValue(sessionforBiz.User.姓名, user.Key);
                cmbUser.Items.Add(skv);
            }

            //加载所有缓存的表
            listBoxTableList.Items.Clear();

            List<string> tableList = new List<string>();
            foreach (var tableName in BizCacheHelper.Manager.NewTableList.Keys)
            {
                tableList.Add(tableName);
            }
            tableList.Sort();
            foreach (var tableName in tableList)
            {
                listBoxTableList.Items.Add(tableName);
            }
        }

        private async void toolStripButton加载缓存_Click(object sender, EventArgs e)
        {
            await frmMain.Instance.InitConfig(true);
        }

        private void 推送缓存数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmbUser.SelectedItem != null)
            {
                if (listBoxTableList.SelectedItem != null)
                {
                    string tableName = listBoxTableList.SelectedItem.ToString();

                    SuperValue skv = cmbUser.SelectedItem as SuperValue;

                    SessionforBiz PlayerSession = frmMain.Instance.sessionListBiz[skv.superDataTypeName];
                    UserService.发送缓存数据列表(PlayerSession, tableName);
                }

            }
            else
            {
                MessageBox.Show("请选择接收用户");
            }
        }

        private void 加载缓存数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (listBoxTableList.SelectedItem != null)
            {
                string tableName = listBoxTableList.SelectedItem.ToString();
                BizCacheHelper.Instance.SetDictDataSource(tableName, true);
            }


        }
    }
}
