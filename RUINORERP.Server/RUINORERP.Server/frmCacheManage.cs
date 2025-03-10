using HLH.Lib.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Comm;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

        /// <summary>
        /// 所有实体表都在这个命名空间下，不需要每次都反射
        /// </summary>
        //Assembly assembly = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
        private void listBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBoxTableList.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;
                if (tableName == "锁定信息列表")
                {
                    dataGridView1.DataSource = frmMain.Instance.lockManager.GetLockItems();
                }
                else
                {
                    var CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    if (CacheList == null)
                    {
                        dataGridView1.DataSource = null;
                        return;
                    }
                    // 使用 Assembly.Load 加载包含 PrintHelper<T> 类的程序集

                    // 使用 GetType 方法获取 PrintHelper<T> 的类型
                    //Type type = assembly.GetType("RUINORERP.Model." + tableName);
                    //dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(type);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //dataGridView1.XmlFileName = "UCCacheManage" + tableName;
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = CacheList;
                }
               
            }
            else
            {
                dataGridView1.DataSource = null;
                return;
            }

            //var CacheList = BizCacheHelper.Manager.CacheEntityList.Get(listBoxTableList.SelectedItem.ToString());
            //dataGridView1.DataSource = CacheList;

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
            //frmMain.Instance.CacheInfoList.Clear();
            //不能清掉。不然无效
            //MyCacheManager.Instance.Cache.Clear();
            foreach (var tableName in tableList)
            {
                SuperValue kv = null;
                string cacheInfoView = string.Empty;
                CacheInfo cacheInfo = MyCacheManager.Instance.CacheInfoList.Get(tableName) as CacheInfo;
                if (cacheInfo != null)
                {
                    if (cacheInfo.HasExpire)
                    {
                        cacheInfoView = $"  {cacheInfo.CacheCount}-{cacheInfo.ExpirationTime}";
                    }
                    else
                    {
                        cacheInfoView = $"  {cacheInfo.CacheCount}";
                    }
                }

                var CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                if (CacheList == null)
                {
                    kv = new SuperValue(tableName + "[" + 0 + "]" + cacheInfoView, tableName);

                    //frmMain.Instance.CacheInfoList.TryAdd(tableName, new CacheInfo(tableName, 0));
                    //MyCacheManager.Instance.Cache.Add(tableName, new CacheInfo(tableName, 0));
                    //var lastCacheInfo = new CacheInfo(tableName, 0);
                    //MyCacheManager.Instance.Cache.AddOrUpdate(tableName, lastCacheInfo, c => lastCacheInfo);
                }
                else
                {
                    var lastlist = ((IEnumerable<dynamic>)CacheList).ToList();
                    if (lastlist != null)
                    {
                        kv = new SuperValue(tableName + "[" + lastlist.Count + "]" + cacheInfoView, tableName);
                        // frmMain.Instance.CacheInfoList.TryAdd(tableName, new CacheInfo(tableName, lastlist.Count));
                        //var lastCacheInfo = new CacheInfo(tableName, lastlist.Count);
                        //MyCacheManager.Instance.Cache.AddOrUpdate(tableName, lastCacheInfo, c => lastCacheInfo);
                    }
                }

                listBoxTableList.Items.Add(kv);
            }

            //添加锁定信息
            if (frmMain.Instance.lockManager != null)
            {
                SuperValue kv = new SuperValue($"锁定信息列表{frmMain.Instance.lockManager.GetLockItemCount()}", "锁定信息列表");
                listBoxTableList.Items.Add(kv);
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
                if (listBoxTableList.SelectedItem is SuperValue kv)
                {
                    string tableName = kv.superDataTypeName;
                    SuperValue skv = cmbUser.SelectedItem as SuperValue;
                    SessionforBiz PlayerSession = frmMain.Instance.sessionListBiz[skv.superDataTypeName];
                    UserService.发送缓存数据列表(PlayerSession, tableName);
                }

            }
            else
            {
                MessageBox.Show("没有选择具体用户时，则向当前所有在线用户推送缓存数据。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (listBoxTableList.SelectedItem is SuperValue kv)
                {
                    string tableName = kv.superDataTypeName;
                    SuperValue skv = cmbUser.SelectedItem as SuperValue;

                    foreach (var item in frmMain.Instance.sessionListBiz)
                    {
                        SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                        UserService.发送缓存数据列表(sessionforBiz, tableName);
                    }
                }
            }
        }

        private void 加载缓存数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (listBoxTableList.SelectedItem != null)
            {
                if (listBoxTableList.SelectedItem is SuperValue kv)
                {
                    string tableName = kv.superDataTypeName;
                    Stopwatch stopwatchLoadUI = Stopwatch.StartNew();
                    BizCacheHelper.Instance.SetDictDataSource(tableName, true);
                    stopwatchLoadUI.Stop();
                    if (frmMain.Instance.IsDebug)
                    {
                        frmMain.Instance.PrintInfoLog($"LoadUIPages 执行时间：{stopwatchLoadUI.ElapsedMilliseconds} 毫秒");
                    }

                }
            }
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

