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
using RUINORERP.Server.Network.Interfaces.Services;
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
using Newtonsoft.Json;
using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace RUINORERP.Server.Controls
{
    public partial class CacheManagementControl : UserControl
    {
        private readonly ISessionService _sessionService;

        public CacheManagementControl()
        {
            InitializeComponent();
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
        }

        private void CacheManagementControl_Load(object sender, EventArgs e)
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

                    var CacheList = MyCacheManager.Instance.CacheEntityList.Get(tableName);
                    if (CacheList != null)
                    {
                        // 根据不同的缓存类型处理数据
                        if (CacheList is IList list)
                        {
                            // 直接使用IList
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = list;
                        }
                        else if (CacheList is string jsonString)
                        {
                            try
                            {
                                // 将JSON字符串转换为JArray
                                var jArray = JsonConvert.DeserializeObject<JArray>(jsonString);
                                DataTable dt = ConvertJArrayToDataTable(jArray);
                                dataGridView1.DataSource = null;
                                dataGridView1.DataSource = dt;
                            }
                            catch
                            {
                                // 如果不是有效的JSON，创建单列表格
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Value");
                                dt.Rows.Add(jsonString);
                                dataGridView1.DataSource = null;
                                dataGridView1.DataSource = dt;
                            }
                        }
                        // 兼容旧的JArray格式
                        else if (CacheList is JArray jArray)
                        {
                            DataTable dt = ConvertJArrayToDataTable(jArray);
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

        /// <summary>
        /// 将JArray转换为DataTable
        /// </summary>
        /// <param name="jArray">要转换的JArray</param>
        /// <returns>转换后的DataTable</returns>
        private DataTable ConvertJArrayToDataTable(JArray jArray)
        {
            DataTable dataTable = new DataTable();
            
            // 如果JArray为空，创建一个空表格
            if (jArray.Count == 0)
            {
                dataTable.Columns.Add("Empty");
                dataTable.Rows.Add("No data");
                return dataTable;
            }
            
            // 获取第一个对象来确定列结构
            var firstItem = jArray[0];
            
            // 如果是JObject，根据其属性创建列
            if (firstItem is JObject jObject)
            {
                foreach (var property in jObject.Properties())
                {
                    // 根据JTokenType确定列类型
                    Type columnType = typeof(string);
                    switch (property.Value.Type)
                    {
                        case JTokenType.Integer:
                            columnType = typeof(long);
                            break;
                        case JTokenType.Float:
                            columnType = typeof(double);
                            break;
                        case JTokenType.Date:
                            columnType = typeof(DateTime);
                            break;
                        case JTokenType.Boolean:
                            columnType = typeof(bool);
                            break;
                        default:
                            columnType = typeof(string);
                            break;
                    }
                    
                    dataTable.Columns.Add(property.Name, columnType);
                }
                
                // 填充数据行
                foreach (var item in jArray)
                {
                    if (item is JObject obj)
                    {
                        var row = dataTable.NewRow();
                        foreach (var property in obj.Properties())
                        {
                            if (dataTable.Columns.Contains(property.Name))
                            {
                                // 处理可能的null值
                                row[property.Name] = property.Value.Type == JTokenType.Null ? DBNull.Value : property.Value.ToObject(dataTable.Columns[property.Name].DataType);
                            }
                        }
                        dataTable.Rows.Add(row);
                    }
                }
            }
            // 如果不是JObject，创建一个单列表格
            else
            {
                dataTable.Columns.Add("Value", firstItem?.GetType() ?? typeof(string));
                foreach (var item in jArray)
                {
                    dataTable.Rows.Add(item?.ToString() ?? "");
                }
            }
            
            return dataTable;
        }

        private async void toolStripButton刷新缓存_Click(object sender, EventArgs e)
        {
            //这里添加所有缓存
            try
            {
                // 先确保缓存已初始化
                if (MyCacheManager.Instance.CacheEntityList == null)
                {
                    // 如果缓存为空，尝试重新初始化
                    await frmMain.Instance.InitConfig(true);
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
                var sessions = _sessionService.GetAllUserSessions();
                foreach (var session in sessions)
                {
                    SuperValue skv = new SuperValue(session.UserName, session.SessionID);
                    cmbUser.Items.Add(skv);
                }

                //加载所有缓存的表
                listBoxTableList.Items.Clear();

                List<string> tableNameList = new List<string>();

                // 确保缓存管理器已初始化
                if (MyCacheManager.Instance == null)
                {
                    MessageBox.Show("缓存管理器未初始化", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 确保有缓存表
                if (MyCacheManager.Instance.NewTableList == null || MyCacheManager.Instance.NewTableList.Count == 0)
                {
                    MessageBox.Show("没有找到缓存表配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var tableName in MyCacheManager.Instance.NewTableList.Keys)
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

                    var CacheList = MyCacheManager.Instance.CacheEntityList.Get(tableName);
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
                    else if (CacheList is string jsonString)
                    {
                        try
                        {
                            // 尝试解析为JArray
                            var parsedArray = JsonConvert.DeserializeObject<JArray>(jsonString);
                            count = parsedArray.Count();
                        }
                        catch
                        {
                            // 如果不是有效的JSON，计为1
                            count = 1;
                        }
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
                    var session = _sessionService.GetSession(skv.superDataTypeName);
                    if (session != null)
                    {
                        // 发送推送缓存命令
                        var success = _sessionService.SendCommandToSession(session.SessionID, "PUSH_CACHE_DATA", tableName);
                        if (success)
                        {
                            frmMain.Instance.PrintInfoLog($"已向用户 {session.UserName} 推送缓存数据: {tableName}");
                        }
                        else
                        {
                            frmMain.Instance.PrintErrorLog($"向用户 {session.UserName} 推送缓存数据失败: {tableName}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("没有选择具体用户时，则向当前所有在线用户推送缓存数据。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (listBoxTableList.SelectedItem is SuperValue kv)
                {
                    string tableName = kv.superDataTypeName;
                    var sessions = _sessionService.GetAllUserSessions();
                    
                    foreach (var session in sessions)
                    {
                        // 发送推送缓存命令
                        var success = _sessionService.SendCommandToSession(session.SessionID, "PUSH_CACHE_DATA", tableName);
                        if (success)
                        {
                            frmMain.Instance.PrintInfoLog($"已向用户 {session.UserName} 推送缓存数据: {tableName}");
                        }
                        else
                        {
                            frmMain.Instance.PrintErrorLog($"向用户 {session.UserName} 推送缓存数据失败: {tableName}");
                        }
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
                    if (MyCacheManager.Instance == null)
                    {
                        MessageBox.Show("缓存帮助类未初始化", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

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