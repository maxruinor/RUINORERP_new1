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
using RUINORERP.Business.Cache;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;
using RUINORERP.Model.TransModel;

namespace RUINORERP.Server.Controls
{
    public partial class CacheManagementControl : UserControl
    {
        private readonly ISessionService _sessionService;
        private readonly ILogger<CacheManagementControl> _logger;

        public CacheManagementControl()
        {
            InitializeComponent();
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
            _logger = Program.ServiceProvider.GetRequiredService<ILogger<CacheManagementControl>>();
            _entityCacheManager = Program.ServiceProvider.GetRequiredService<IEntityCacheManager>();
        }

        private void CacheManagementControl_Load(object sender, EventArgs e)
        {
            LoadCacheToUI();
            // 初始化统计表格
            InitStatisticsGrids();
            // 加载缓存统计数据
            LoadCacheStatistics();
        }

        /// <summary>
        /// 初始化统计数据表格
        /// </summary>
        private void InitStatisticsGrids()
        {
            try
            {
                // 初始化按表统计表格
                dataGridViewTableStats.AutoGenerateColumns = false;
                dataGridViewTableStats.Columns.Clear();
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "TableName", HeaderText = "表名", DataPropertyName = "TableName", Width = 150 });
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "ItemCount", HeaderText = "缓存项数", DataPropertyName = "ItemCount", Width = 80 });
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstimatedSize", HeaderText = "估计大小(MB)", DataPropertyName = "EstimatedSize", Width = 100 });
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "Hits", HeaderText = "命中次数", DataPropertyName = "Hits", Width = 80 });
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "Misses", HeaderText = "未命中次数", DataPropertyName = "Misses", Width = 100 });
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "HitRatio", HeaderText = "命中率", DataPropertyName = "HitRatio", Width = 80 });
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastUpdated", HeaderText = "最后更新时间", DataPropertyName = "LastUpdated", Width = 150 });

                // 初始化缓存项统计表格
                dataGridViewItemStats.AutoGenerateColumns = false;
                dataGridViewItemStats.Columns.Clear();
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "CacheKey", HeaderText = "缓存键", DataPropertyName = "CacheKey", Width = 200 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "TypeName", HeaderText = "类型", DataPropertyName = "TypeName", Width = 120 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "TableName", HeaderText = "表名", DataPropertyName = "TableName", Width = 100 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedTime", HeaderText = "创建时间", DataPropertyName = "CreatedTime", Width = 150 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastAccessed", HeaderText = "最后访问时间", DataPropertyName = "LastAccessed", Width = 150 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "AccessCount", HeaderText = "访问次数", DataPropertyName = "AccessCount", Width = 80 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化统计表格时发生错误");
            }
        }

        /// <summary>
        /// 所有实体表都在这个命名空间下，不需要每次都反射
        /// </summary>
        //Assembly assembly = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
private readonly IEntityCacheManager _entityCacheManager;
        
        /// <summary>
        /// 加载缓存统计数据
        /// </summary>
        private void LoadCacheStatistics()
        {
            try
            {
                // 检查是否实现了ICacheStatistics接口
                if (_entityCacheManager is ICacheStatistics cacheStats)
                {
                    // 更新全局统计指标
                    txtHits.Text = cacheStats.CacheHits.ToString();
                    txtMisses.Text = cacheStats.CacheMisses.ToString();
                    txtItemCount.Text = cacheStats.CacheItemCount.ToString();
                    txtEstimatedSize.Text = $"{cacheStats.EstimatedCacheSize / (1024.0 * 1024.0):F2} MB";
                    
                    // 计算并显示命中率
                    long totalAccess = cacheStats.CacheHits + cacheStats.CacheMisses;
                    if (totalAccess > 0)
                    {
                        double hitRate = (double)cacheStats.CacheHits / totalAccess * 100;
                        txtHitRatio.Text = $"{hitRate:F2}%";
                    }
                    else
                    {
                        txtHitRatio.Text = "0.00%";
                    }

                    // 加载按表统计数据
                    var tableStats = cacheStats.GetTableCacheStatistics();
                    // 创建视图模型列表，将字节转换为MB
                    var viewModelList = tableStats.Values.Select(ts => new
                    {
                        ts.TableName,
                        ItemCount = ts.TotalItemCount,
                        EstimatedSize = Math.Round(ts.EstimatedTotalSize / (1024.0 * 1024.0), 2),
                        ts.HitRatio
                    }).ToList();
                    dataGridViewTableStats.DataSource = viewModelList;

                    // 加载缓存项统计数据
                    var itemStats = cacheStats.GetCacheItemStatistics();
                    dataGridViewItemStats.DataSource = itemStats;
                }
                else
                {
                    // 接口未实现的提示
                    MessageBox.Show("当前缓存管理器未实现ICacheStatistics接口，无法显示缓存统计数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载缓存统计数据时发生错误");
                MessageBox.Show($"加载缓存统计数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 事件处理

       
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
                        var lockItems = frmMainNew.Instance.lockManager.GetLockItems();
                        if (lockItems == null || lockItems.Count == 0)
                        {
                            // 创建空表结构，避免绑定空列表导致显示异常
                            var emptyTable = new DataTable();
                            emptyTable.Columns.Add("Empty");
                            emptyTable.Rows.Add("No lock data");
                            dataGridView1.DataSource = emptyTable;
                        }
                        else
                        {
                            dataGridView1.DataSource = lockItems;
                        }
                        return;
                    }

                    // 使用新的缓存管理器获取数据
                    var cacheManager = Startup.GetFromFac<IEntityCacheManager>();
                    var schemaManager = TableSchemaManager.Instance;
                    var entityType = schemaManager.GetEntityType(tableName);

                    if (entityType != null)
                    {
                        // 使用反射调用GetEntityList<T>(string tableName)方法
                        var method = typeof(IEntityCacheManager).GetMethod("GetEntityList",
                            BindingFlags.Public | BindingFlags.Instance,
                            null,
                            new[] { typeof(string) },
                            null);

                        if (method != null)
                        {
                            var genericMethod = method.MakeGenericMethod(entityType);
                            var entityList = genericMethod.Invoke(cacheManager, new object[] { tableName });

                            // 绑定数据到dataGridView
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = entityList;
                        }

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
                // 使用新的缓存管理器检查缓存状态
                var cacheManager = Startup.GetFromFac<IEntityCacheManager>();
                // 这里可以添加对新缓存管理器的检查逻辑
                // 例如检查某个关键表是否已缓存

                // 刷新UI显示
                LoadCacheToUI();
                
                // 刷新缓存统计数据
                LoadCacheStatistics();

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

                // 获取新的缓存管理器实例
                var cacheManager = Startup.GetFromFac<IEntityCacheManager>();

                // 获取所有可缓存的表名
                List<string> tableNameList = RUINORERP.Server.Comm.CacheUIHelper.GetCacheableTableNames();

                if (tableNameList == null || tableNameList.Count == 0)
                {
                    return;
                }

                tableNameList.Sort();

                foreach (var tableName in tableNameList)
                {
                    SuperValue kv = null;
                    string cacheInfoView = string.Empty;

                    // 获取实体列表数量
                    int count = RUINORERP.Server.Comm.CacheUIHelper.GetCacheItemCount(cacheManager, tableName);
                    kv = new SuperValue(tableName + $"[{count}]" + cacheInfoView, tableName);

                    listBoxTableList.Items.Add(kv);
                }

                //添加锁定信息
                if (frmMainNew.Instance.lockManager != null)
                {
                    SuperValue kv = new SuperValue($"锁定信息列表{frmMainNew.Instance.lockManager.GetLockItemCount()}", "锁定信息列表");
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

                await frmMainNew.Instance.InitConfig(true);

                // 加载完成后刷新UI
                LoadCacheToUI();
                
                // 刷新缓存统计数据
                LoadCacheStatistics();

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
                        // 发送推送缓存命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "PUSH_CACHE_DATA",
                            TableName = tableName
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID, 
                            MessageCommands.SendMessageToUser, 
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {session.UserName} 推送缓存数据: {tableName}");
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {session.UserName} 推送缓存数据失败: {tableName}");
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
                        // 发送推送缓存命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "PUSH_CACHE_DATA",
                            TableName = tableName
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID, 
                            MessageCommands.SendMessageToUser, 
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {session.UserName} 推送缓存数据: {tableName}");
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {session.UserName} 推送缓存数据失败: {tableName}");
                        }
                    }
                }
            }
        }

        private async void btnPushCacheData_Click(object sender, EventArgs e)
        {
            if (listBoxTableList.SelectedItem != null)
            {
                if (cmbUser.SelectedItem != null)
                {
                    SuperValue kv = listBoxTableList.SelectedItem as SuperValue;
                    string tableName = kv.superDataTypeName;
                    SuperValue skv = cmbUser.SelectedItem as SuperValue;
                    var session = _sessionService.GetSession(skv.superDataTypeName);
                    if (session != null)
                    {
                        // 发送推送缓存命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "PUSH_CACHE_DATA",
                            TableName = tableName
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID, 
                            MessageCommands.SendMessageToUser, 
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {session.UserName} 推送缓存数据: {tableName}");
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {session.UserName} 推送缓存数据失败: {tableName}");
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
                            // 发送推送缓存命令 - 使用新的发送方法
                            var messageData = new
                            {
                                Command = "PUSH_CACHE_DATA",
                                TableName = tableName
                            };

                            var request = new MessageRequest(MessageType.Unknown, messageData);
                            var success = _sessionService.SendCommandAsync(
                                session.SessionID, 
                                MessageCommands.SendMessageToUser, 
                                request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                            if (success)
                            {
                                frmMainNew.Instance.PrintInfoLog($"已向用户 {session.UserName} 推送缓存数据: {tableName}");
                            }
                            else
                            {
                                frmMainNew.Instance.PrintErrorLog($"向用户 {session.UserName} 推送缓存数据失败: {tableName}");
                            }
                        }
                    }
                }
            }
        }

        private async void 加载缓存数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxTableList.SelectedItem != null && listBoxTableList.SelectedItem is SuperValue kv)
                {
                    string tableName = kv.superDataTypeName;

                    // 显示加载中状态
                    this.Cursor = Cursors.WaitCursor;

                    Stopwatch stopwatchLoadUI = Stopwatch.StartNew();

                    // 从数据库加载指定表的数据到缓存
                    try
                    {
                        // 获取EntityCacheInitializationService实例
                        var initializationService = Startup.GetFromFac<EntityCacheInitializationService>();
                        // 调用新方法初始化单个表的缓存
                        await initializationService.InitializeSingleTableCacheAsync(tableName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"加载表 {tableName} 数据时发生错误");
                        MessageBox.Show($"加载表 {tableName} 数据时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    stopwatchLoadUI.Stop();

                    // 刷新UI显示
                    LoadCacheToUI();
                    
                    // 刷新缓存统计数据
                    LoadCacheStatistics();

                    // 重新选中当前项以刷新数据显示
                    listBoxTableList.SelectedItem = kv;
                    listBoxTableList_SelectedIndexChanged(null, null);

                    if (frmMainNew.Instance.IsDebug)
                    {
                        frmMainNew.Instance.PrintInfoLog($"加载缓存数据 {tableName} 执行时间：{stopwatchLoadUI.ElapsedMilliseconds} 毫秒");
                    }

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

        // 刷新缓存统计数据
        public void btnRefreshStatistics_Click(object sender, EventArgs e)
        {
            LoadCacheStatistics();
        }

        // 重置缓存统计数据
        public void btnResetStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                if (_entityCacheManager is ICacheStatistics cacheStats)
                {
                    cacheStats.ResetStatistics();
                    LoadCacheStatistics();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重置缓存统计数据时发生错误");
                MessageBox.Show($"重置缓存统计数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

    }
}