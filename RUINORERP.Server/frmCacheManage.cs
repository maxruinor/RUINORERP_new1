using HLH.Lib.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Comm;
using RUINORERP.Common.Extensions;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Server.BizService;
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
using RUINORERP.Business.CommService;
using Newtonsoft.Json.Linq;
using System.Collections;

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
            try
            {
                if (listBoxTableList.SelectedItem != null && listBoxTableList.SelectedItem is SuperValue kv)
                {
                    string tableName = kv.superDataTypeName;
                    if (tableName == "锁定信息列表")
                    {
                        // 显示锁定信息
                        dataGridView1.DataSource = null;
                        var lockItems = frmMain.Instance.lockManager.GetLockItems();
                        dataGridView1.DataSource = lockItems;
                        return;
                    }

                    var CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    if (CacheList != null)
                    {
                        // 根据不同的缓存类型处理数据
                        if (CacheList is IList list)
                        {
                            // 直接使用IList
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = list;
                        }
                        else if (CacheList is JArray jArray)
                        {
                            // 将JArray转换为DataTable
                            DataTable dt = new DataTable();
                            if (jArray.Count > 0)
                            {
                                JObject firstItem = jArray.First as JObject;
                                foreach (JProperty prop in firstItem.Properties())
                                {
                                    Type columnType = typeof(string); // 默认为字符串类型

                                    // 尝试确定更精确的类型
                                    if (prop.Value.Type == JTokenType.Integer)
                                    {
                                        columnType = typeof(long);
                                    }
                                    else if (prop.Value.Type == JTokenType.Float)
                                    {
                                        columnType = typeof(decimal);
                                    }
                                    else if (prop.Value.Type == JTokenType.Date)
                                    {
                                        columnType = typeof(DateTime);
                                    }
                                    else if (prop.Value.Type == JTokenType.Boolean)
                                    {
                                        columnType = typeof(bool);
                                    }

                                    dt.Columns.Add(prop.Name, columnType);
                                }

                                foreach (JObject item in jArray)
                                {
                                    DataRow dr = dt.NewRow();
                                    foreach (JProperty prop in item.Properties())
                                    {
                                        Type targetType = typeof(string); // 默认为字符串类型

                                        // 根据JTokenType确定目标类型
                                        switch (prop.Value.Type)
                                        {
                                            case JTokenType.Integer:
                                                targetType = typeof(long);
                                                break;
                                            case JTokenType.Float:
                                                targetType = typeof(decimal);
                                                break;
                                            case JTokenType.Date:
                                                targetType = typeof(DateTime);
                                                break;
                                            case JTokenType.Boolean:
                                                targetType = typeof(bool);
                                                break;
                                        }

                                        object value = prop.Value.ToObject(targetType);
                                        dr[prop.Name] = value ?? DBNull.Value;
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }

                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = dt;
                        }
                        else
                        {
                            // 尝试转换为IEnumerable
                            try
                            {
                                var enumerable = CacheList as IEnumerable;
                                if (enumerable != null)
                                {
                                    var dataList = enumerable.Cast<object>().ToList();
                                    dataGridView1.DataSource = null;
                                    dataGridView1.DataSource = dataList;
                                }
                            }
                            catch
                            {
                                // 如果无法转换，创建一个简单的显示
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Value", typeof(object));
                                dt.Rows.Add(CacheList.ToString());
                                dataGridView1.DataSource = null;
                                dataGridView1.DataSource = dt;
                            }
                        }
                    }
                    else
                    {
                        // 没有缓存数据
                        dataGridView1.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示缓存数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton刷新缓存_Click(object sender, EventArgs e)
        {
            //这里添加所有缓存
            try
            {
                // 先确保缓存已初始化
                if (BizCacheHelper.Manager.CacheEntityList == null)
                {
                    // 如果缓存为空，尝试重新初始化
                    Task.Run(async () => await frmMain.Instance.InitConfig(true)).Wait();
                }

                // 刷新UI显示
                LoadCacheToUI();

                // 如果有选中的表，刷新该表的数据
                if (listBoxTableList.SelectedItem != null && listBoxTableList.SelectedItem is SuperValue kv)
                {
                    listBoxTableList_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新缓存时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadCacheToUI()
        {
            try
            {
                //加载所有用户
                cmbUser.Items.Clear();
                foreach (var user in frmMain.Instance.sessionListBiz.ToArray())
                {
                    SessionforBiz sessionforBiz = user.Value as SessionforBiz;
                    SuperValue skv = new SuperValue(sessionforBiz.User.姓名, user.Key);
                    cmbUser.Items.Add(skv);
                }

                //加载所有缓存的表
                listBoxTableList.Items.Clear();

                List<string> tableNameList = new List<string>();

                // 确保缓存管理器已初始化
                if (BizCacheHelper.Manager == null)
                {
                    MessageBox.Show("缓存管理器未初始化", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 确保有缓存表
                if (BizCacheHelper.Manager.NewTableList == null || BizCacheHelper.Manager.NewTableList.Count == 0)
                {
                    MessageBox.Show("没有找到缓存表配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var tableName in BizCacheHelper.Manager.NewTableList.Keys)
                {
                    tableNameList.Add(tableName);
                }
                tableNameList.Sort();

                foreach (var tableName in tableNameList)
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
                        kv = new SuperValue(tableName + "[0]" + cacheInfoView, tableName);
                    }
                    else
                    {
                        int count = 0;
                        // 处理不同类型的缓存数据
                        if (CacheList is IList list)
                        {
                            count = list.Count;
                        }
                        else if (CacheList is JArray jArray)
                        {
                            count = jArray.Count;
                        }
                        else
                        {
                            // 尝试转换为IEnumerable
                            try
                            {
                                var enumerable = CacheList as IEnumerable<dynamic>;
                                if (enumerable != null)
                                {
                                    count = enumerable.Count();
                                }
                            }
                            catch
                            {
                                // 如果无法转换，显示为1（表示有数据但无法计数）
                                count = 1;
                            }
                        }

                        kv = new SuperValue(tableName + $"[{count}]" + cacheInfoView, tableName);
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
            catch (Exception ex)
            {
                MessageBox.Show($"加载缓存到UI时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void toolStripButton加载缓存_Click(object sender, EventArgs e)
        {
            try
            {
                // 显示加载中状态
                this.Cursor = Cursors.WaitCursor;
                toolStripButton加载缓存.Enabled = false;

                await frmMain.Instance.InitConfig(true);

                // 加载完成后刷新UI
                LoadCacheToUI();

                MessageBox.Show("缓存加载完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载缓存时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                this.Cursor = Cursors.Default;
                toolStripButton加载缓存.Enabled = true;
            }
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
                    //UserService.发送缓存数据列表(PlayerSession, tableName);
                }

            }
            else
            {
                MessageBox.Show("没有选择具体用户时，则向当前所有在线用户推送缓存数据。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (listBoxTableList.SelectedItem is SuperValue kv)
                {
                    string tableName = kv.superDataTypeName;
                    SuperValue skv = cmbUser.SelectedItem as SuperValue;

                    foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
                    {
                        SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                        //UserService.发送缓存数据列表(sessionforBiz, tableName);
                    }
                }
            }
        }

        private void 加载缓存数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxTableList.SelectedItem != null && listBoxTableList.SelectedItem is SuperValue kv)
                {
                    string tableName = kv.superDataTypeName;

                    // 显示加载中状态
                    this.Cursor = Cursors.WaitCursor;

                    Stopwatch stopwatchLoadUI = Stopwatch.StartNew();

                    // 确保BizCacheHelper实例存在
                    if (BizCacheHelper.Instance == null)
                    {
                        MessageBox.Show("缓存帮助类未初始化", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    BizCacheHelper.Instance.SetDictDataSource(tableName, true);
                    stopwatchLoadUI.Stop();

                    // 刷新UI显示
                    LoadCacheToUI();

                    // 重新选中当前项以刷新数据显示
                    listBoxTableList.SelectedItem = kv;
                    listBoxTableList_SelectedIndexChanged(null, null);

                    if (frmMain.Instance.IsDebug)
                    {
                        frmMain.Instance.PrintInfoLog($"加载缓存数据 {tableName} 执行时间：{stopwatchLoadUI.ElapsedMilliseconds} 毫秒");
                    }

                    MessageBox.Show($"表 {tableName} 的缓存数据已加载", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("请先选择要加载的缓存表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载缓存数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                this.Cursor = Cursors.Default;
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

