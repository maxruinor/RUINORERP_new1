using HLH.Lib.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Cache;
using RUINORERP.PacketSpec.Models.Messaging;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Comm;
using RUINORERP.Server.Network.CommandHandlers;
using RUINORERP.Server.Network.Interfaces.Services;
using System;
using System.Collections;
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
        private async void LoadCacheStatistics()
        {
            try
            {
                // 显示加载状态
                toolStripStatusLabel1.Text = "正在加载缓存统计...";

                // 使用异步方式加载统计信息，避免UI线程阻塞
                await Task.Run(() =>
                {
                    // 检查是否实现了ICacheStatistics接口
                    if (_entityCacheManager is ICacheStatistics cacheStats)
                    {
                        // 在UI线程上更新控件
                        this.Invoke(new Action(() =>
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

                            // 更新状态栏
                            toolStripStatusLabel1.Text = $"缓存统计已更新 - 命中率: {txtHitRatio.Text}, 总命中: {cacheStats.CacheHits:N0}";
                        }));
                    }
                    else
                    {
                        // 接口未实现的提示
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("当前缓存管理器未实现ICacheStatistics接口，无法显示缓存统计数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载缓存统计数据时发生错误");
                MessageBox.Show($"加载缓存统计数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 事件处理


        private JArray GetEntityListFromCache(IEntityCacheManager cacheManager, string tableName)
        {
            try
            {
                // 获取表对应的实体类型
                var schemaManager = TableSchemaManager.Instance;
                var entityType = schemaManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    return null;
                }

                // 使用反射调用GetEntityList<T>方法
                var method = typeof(IEntityCacheManager).GetMethod("GetEntityList",
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    new[] { typeof(string) },
                    null);

                if (method == null)
                {
                    return null;
                }

                var genericMethod = method.MakeGenericMethod(entityType);
                var entityList = genericMethod.Invoke(cacheManager, new object[] { tableName });

                // 转换为JArray（使用异步方式处理大数据集）
                if (entityList != null)
                {
                    // 限制处理的数据量，避免大数据集导致性能问题
                    var list = entityList as System.Collections.IList;
                    if (list != null && list.Count > 10000)
                    {
                        // 大数据集只返回前10000条记录，并添加提示
                        var limitedList = new ArrayList();
                        for (int i = 0; i < Math.Min(10000, list.Count); i++)
                        {
                            limitedList.Add(list[i]);
                        }

                        // 在UI线程上显示提示信息
                        this.Invoke(new Action(() =>
                        {
                            toolStripStatusLabel1.Text = $"表 {tableName} 数据量过大({list.Count}条)，仅显示前10000条记录";
                        }));

                        return JArray.FromObject(limitedList);
                    }

                    return JArray.FromObject(entityList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"从缓存获取实体列表时出错: {tableName}");
            }

            return null;
        }

        private async void listBoxTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBoxTableList.SelectedItem != null && listBoxTableList.SelectedItem is SuperValue kv)
                {
                    string tableName = kv.superDataTypeName;
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        // 显示加载状态
                        this.Cursor = Cursors.WaitCursor;
                        toolStripStatusLabel1.Text = $"正在加载表 {tableName} 的缓存数据...";

                        // 使用异步方式加载数据，避免UI线程阻塞
                        await Task.Run(() =>
                        {
                            var cacheManager = Startup.GetFromFac<IEntityCacheManager>();
                            var jArray = GetEntityListFromCache(cacheManager, tableName);

                            if (jArray != null)
                            {
                                // 转换为DataTable
                                var dataTable = ConvertJArrayToDataTable(jArray);

                                // 在UI线程上更新DataGridView
                                this.Invoke(new Action(() =>
                                {
                                    // 绑定数据到dataGridView
                                    dataGridView1.DataSource = null;
                                    dataGridView1.DataSource = dataTable;

                                    // 更新状态栏
                                    int count = dataTable.Rows.Count;
                                    toolStripStatusLabel1.Text = $"表 {tableName} 缓存数据已加载，共 {count} 条记录";
                                }));
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示缓存数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 将JArray转换为DataTable
        /// </summary>
        /// <param name="jArray">要转换的JArray</param>
        /// <returns>转换后的DataTable</returns>
        private DataTable ConvertJArrayToDataTable(JArray jArray)
        {
            try
            {
                if (jArray == null || jArray.Count == 0)
                {
                    return null;
                }

                // 创建DataTable
                DataTable dataTable = new DataTable();

                // 获取第一个对象的结构
                var firstObject = jArray[0] as JObject;
                if (firstObject == null)
                {
                    return null;
                }

                // 添加列（限制列数，避免过多列导致性能问题）
                int columnCount = 0;
                foreach (var property in firstObject.Properties())
                {
                    if (columnCount >= 50) // 限制最多50列
                    {
                        break;
                    }
                    dataTable.Columns.Add(property.Name, typeof(string));
                    columnCount++;
                }

                // 添加行（限制行数，避免大数据集导致性能问题）
                int rowCount = 0;
                int maxRows = 10000; // 最多显示10000行

                foreach (var item in jArray)
                {
                    if (rowCount >= maxRows)
                    {
                        break;
                    }

                    var row = dataTable.NewRow();
                    var obj = item as JObject;
                    if (obj != null)
                    {
                        int colIndex = 0;
                        foreach (var property in obj.Properties())
                        {
                            if (colIndex >= 50) // 与列限制保持一致
                            {
                                break;
                            }
                            row[property.Name] = property.Value?.ToString() ?? string.Empty;
                            colIndex++;
                        }
                    }
                    dataTable.Rows.Add(row);
                    rowCount++;
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "转换JArray到DataTable时出错");
                return null;
            }
        }

        private async void toolStripButton刷新缓存_Click(object sender, EventArgs e)
        {
            //这里添加所有缓存
            try
            {
                // 显示加载状态
                this.Cursor = Cursors.WaitCursor;
                toolStripButton刷新缓存.Enabled = false;

                // 使用后台线程执行耗时操作，避免UI线程阻塞
                await Task.Run(() =>
                {
                    // 刷新UI显示
                    this.Invoke(new Action(() => LoadCacheToUI()));

                    // 刷新缓存统计数据
                    this.Invoke(new Action(() => LoadCacheStatistics()));

                    // 如果有选中的表，刷新该表的数据
                    this.Invoke(new Action(() =>
                    {
                        if (listBoxTableList.SelectedItem != null && listBoxTableList.SelectedItem is SuperValue kv)
                        {
                            listBoxTableList_SelectedIndexChanged(null, null);
                        }
                    }));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新缓存时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                this.Cursor = Cursors.Default;
                toolStripButton刷新缓存.Enabled = true;
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

                // 使用批量操作和异步处理来提高性能
                var itemsToAdd = new List<SuperValue>();

                // 并行处理缓存项数量获取（限制并发度避免过度占用资源）
                var options = new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount / 2 // 使用一半的CPU核心
                };

                Parallel.ForEach(tableNameList, options, tableName =>
                {
                    try
                    {
                        // 获取实体列表数量
                        int count = RUINORERP.Server.Comm.CacheUIHelper.GetCacheItemCount(cacheManager, tableName);
                        var kv = new SuperValue(tableName + $"[{count}]", tableName);
                        lock (itemsToAdd)
                        {
                            itemsToAdd.Add(kv);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"获取表 {tableName} 缓存数量时出错");
                        // 出错时仍然添加表项，但显示为0
                        var kv = new SuperValue(tableName + "[0]", tableName);
                        lock (itemsToAdd)
                        {
                            itemsToAdd.Add(kv);
                        }
                    }
                });

                // 按表名排序后添加到列表
                itemsToAdd.Sort((a, b) => string.Compare(a.superDataTypeName, b.superDataTypeName, StringComparison.Ordinal));

                // 批量添加到ListBox（使用BeginUpdate/EndUpdate减少重绘）
                listBoxTableList.BeginUpdate();
                try
                {
                    foreach (var item in itemsToAdd)
                    {
                        listBoxTableList.Items.Add(item);
                    }
                }
                finally
                {
                    listBoxTableList.EndUpdate();
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
                // 显示加载状态
                this.Cursor = Cursors.WaitCursor;
                toolStripButton加载缓存.Enabled = false;
                toolStripStatusLabel1.Text = "正在加载缓存...";

                // 使用异步方式加载缓存，避免UI线程阻塞
                await Task.Run(async () =>
                {
                    await frmMainNew.Instance.InitConfig(true);
                });

                // 重新加载缓存数据
                await Task.Run(() =>
                {
                    this.Invoke(new Action(() => LoadCacheToUI()));
                    this.Invoke(new Action(() => LoadCacheStatistics()));
                });

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
                toolStripStatusLabel1.Text = "缓存加载完成";
            }
        }

        private async void 推送缓存数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxTableList.SelectedItem != null && listBoxTableList.SelectedItem is SuperValue kv)
                {
                    string tableName = kv.superDataTypeName;

                    // 获取当前用户会话
                    var session = cmbUser.SelectedItem as SuperValue;
                    if (session == null)
                    {
                        MessageBox.Show("请先选择目标用户会话", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string sessionId = session.superDataTypeName;

                    // 显示加载状态
                    this.Cursor = Cursors.WaitCursor;
                    toolStripStatusLabel1.Text = $"正在推送表 {tableName} 的缓存数据...";

                    // 使用异步方式获取和推送缓存数据
                    await Task.Run(async () =>
                    {
                        // 获取缓存数据
                        var cacheManager = Startup.GetFromFac<IEntityCacheManager>();
                        var jArray = GetEntityListFromCache(cacheManager, tableName);

                        if (jArray == null || jArray.Count == 0)
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show($"表 {tableName} 没有缓存数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }));
                            return;
                        }




                        CacheCommandHandler cacheCommandHandler = Startup.GetFromFac<CacheCommandHandler>();
                        // 发送缓存数据到指定用户
                        var cachedata = await cacheCommandHandler.GetTableDataList(tableName);

                        CacheRequest cacheRequest = new CacheRequest();
                        cacheRequest.CacheData = cachedata;
                        cacheRequest.TableName = tableName;
                        cacheRequest.Operation = CacheOperation.Set;



                        // 使用异步方式发送命令，避免阻塞
                        var result = await _sessionService.SendCommandAsync<CacheRequest>(sessionId, CacheCommands.CacheSync, cacheRequest);

                        // 在UI线程上显示结果
                        this.Invoke(new Action(() =>
                        {
                            if (result)
                            {
                                MessageBox.Show($"表 {tableName} 的缓存数据已成功推送到用户 {session.superDataTypeName}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show($"推送缓存数据失败: {result}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }));
                    });
                }
                else
                {
                    MessageBox.Show("请先选择要推送的表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"推送缓存数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                this.Cursor = Cursors.Default;
                toolStripStatusLabel1.Text = "缓存推送完成";
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