using HLH.Lib.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;
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
    /// <summary>
    /// 缓存管理控制类
    /// 
    /// <remarks>
    /// 缓存管理最佳实践说明：
    /// 1. 避免直接依赖于ICacheSyncMetadata接口，应通过IEntityCacheManager间接访问缓存同步元数据功能
    ///    这是因为两个服务实例在初始化顺序上可能存在差异，导致不一致的问题
    /// 
    /// 2. 依赖注入注意事项：
    ///    - 依赖注入容器（Autofac）的初始化是按顺序进行的，某些服务可能在应用生命周期早期无法获取
    ///    - 总是使用GetFromFac方法获取服务实例，并添加适当的null检查
    ///    - 避免在构造函数中执行复杂的初始化逻辑，特别是依赖于其他尚未初始化的服务时
    /// 
    /// 3. 缓存同步机制：
    ///    - EntityCacheManager内部包含ICacheSyncMetadata的引用，通过反射可以安全访问
    ///    - 当需要访问缓存同步元数据时，优先使用EntityCacheManager内部的实例
    ///    - 作为备选方案，才直接从容器中获取ICacheSyncMetadata服务
    /// </remarks>
    /// </summary>
    public partial class CacheManagementControl : UserControl
    {
        private readonly ISessionService _sessionService;
        private readonly ILogger<CacheManagementControl> _logger;
        private readonly IEntityCacheManager _entityCacheManager;
        private readonly EntityCacheInitializationService _initializationService;
        private readonly ITableSchemaManager _tableSchemaManager;
        private System.Windows.Forms.Timer _autoRefreshTimer;
        private DateTime _lastTableStatsRefresh = DateTime.MinValue;
        private DateTime _lastItemStatsRefresh = DateTime.MinValue;
        private DateTime _lastMetadataRefresh = DateTime.MinValue;
        // 注意：不再直接依赖ICacheSyncMetadata接口，而是通过_entityCacheManager间接访问

        public CacheManagementControl()
        {
            InitializeComponent();
            // 统一使用Startup.GetFromFac方法，确保与其他地方获取相同的实例
            _sessionService = Startup.GetFromFac<ISessionService>();
            _logger = Startup.GetFromFac<ILogger<CacheManagementControl>>();
            _entityCacheManager = Startup.GetFromFac<IEntityCacheManager>();
            _initializationService = Startup.GetFromFac<EntityCacheInitializationService>();
            _tableSchemaManager = Startup.GetFromFac<ITableSchemaManager>();

            // 初始化定时刷新器
            InitializeAutoRefreshTimer();
        }

        private void CacheManagementControl_Load(object sender, EventArgs e)
        {
            LoadCacheToUI();
            // 初始化统计表格
            InitStatisticsGrids();
            // 加载缓存统计数据
            LoadCacheStatistics();
            LoadTableStatistics();
            LoadItemStatistics();
            LoadCacheMetadata();

            // 初始化刷新时间戳
            DateTime now = DateTime.Now;
            _lastTableStatsRefresh = now;
            _lastItemStatsRefresh = now;
            _lastMetadataRefresh = now;
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
                dataGridViewTableStats.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dataGridViewTableStats.Columns.Clear();
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "TableName", HeaderText = "表名", DataPropertyName = "TableName", Width = 150 });
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "ItemCount", HeaderText = "缓存项数", DataPropertyName = "ItemCount", Width = 80 });
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstimatedSize", HeaderText = "估计大小(MB)", DataPropertyName = "EstimatedSize", Width = 100 });
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "HitRatio", HeaderText = "命中率", DataPropertyName = "HitRatio", Width = 80 });
                dataGridViewTableStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastUpdated", HeaderText = "最后更新时间", DataPropertyName = "LastUpdated", Width = 150 });

                // 初始化缓存项统计表格
                dataGridViewItemStats.AutoGenerateColumns = false;
                dataGridViewItemStats.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dataGridViewItemStats.Columns.Clear();
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "CacheKey", HeaderText = "缓存键", DataPropertyName = "CacheKey", Width = 200 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "TypeName", HeaderText = "类型", DataPropertyName = "TypeName", Width = 120 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "TableName", HeaderText = "表名", DataPropertyName = "TableName", Width = 100 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedTime", HeaderText = "创建时间", DataPropertyName = "CreatedTime", Width = 150 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastAccessed", HeaderText = "最后访问时间", DataPropertyName = "LastAccessed", Width = 150 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "AccessCount", HeaderText = "访问次数", DataPropertyName = "AccessCount", Width = 80 });
                dataGridViewItemStats.Columns.Add(new DataGridViewTextBoxColumn { Name = "Size", HeaderText = "大小", DataPropertyName = "Size", Width = 80 });

                // 初始化缓存元数据表格
                dataGridViewCacheMetadata.AutoGenerateColumns = false;
                dataGridViewCacheMetadata.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dataGridViewCacheMetadata.Columns.Clear();
                dataGridViewCacheMetadata.Columns.Add(new DataGridViewTextBoxColumn { Name = "TableName", HeaderText = "表名", DataPropertyName = "TableName", Width = 150 });
                dataGridViewCacheMetadata.Columns.Add(new DataGridViewTextBoxColumn { Name = "DataCount", HeaderText = "数据数量", DataPropertyName = "DataCount", Width = 80 });
                dataGridViewCacheMetadata.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstimatedSizeMB", HeaderText = "估计大小(MB)", DataPropertyName = "EstimatedSizeMB", Width = 100 });
                dataGridViewCacheMetadata.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastUpdateTime", HeaderText = "最后更新时间", DataPropertyName = "LastUpdateTime", Width = 150 });
                dataGridViewCacheMetadata.Columns.Add(new DataGridViewTextBoxColumn { Name = "ExpirationTime", HeaderText = "过期时间", DataPropertyName = "ExpirationTime", Width = 150 });
                dataGridViewCacheMetadata.Columns.Add(new DataGridViewTextBoxColumn { Name = "HasExpiration", HeaderText = "是否有过期设置", DataPropertyName = "HasExpiration", Width = 100 });
                dataGridViewCacheMetadata.Columns.Add(new DataGridViewTextBoxColumn { Name = "SourceInfo", HeaderText = "源信息", DataPropertyName = "SourceInfo", Width = 200 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化统计表格时发生错误");
            }
        }

        /// <summary>
        /// 加载缓存统计数据
        /// </summary>
        private async void LoadCacheStatistics()
        {
            try
            {
                // 使用构造函数中注入的缓存管理器实例，确保与初始化服务使用的是同一个实例
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

        /// <summary>
        /// 加载按表统计数据
        /// </summary>
        private async void LoadTableStatistics()
        {
            try
            {
                // 使用异步方式加载统计信息，避免UI线程阻塞
                await Task.Run(() =>
                {
                    // 检查是否实现了ICacheStatistics接口
                    if (_entityCacheManager is ICacheStatistics cacheStats)
                    {
                        // 在UI线程上更新控件
                        this.Invoke(new Action(() =>
                        {
                            // 加载按表统计数据，直接使用TableCacheStatistics类
                            var tableStats = cacheStats.GetTableCacheStatistics();
                            // 创建显示用的匿名对象，将字节转换为MB并格式化显示
                            var viewModelList = tableStats.Values.Select(ts => new
                            {
                                ts.TableName,
                                ItemCount = ts.TotalItemCount,
                                EstimatedSize = Math.Round(ts.EstimatedTotalSize / (1024.0 * 1024.0), 2),
                                HitRatio = ts.HitRatio.ToString("P2"), // 格式化为百分比
                                LastUpdated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            }).ToList();
                            dataGridViewTableStats.DataSource = viewModelList;

                            // 更新状态栏
                            toolStripStatusLabel1.Text = $"按表统计已更新 - 共 {viewModelList.Count} 个表";
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载按表统计数据时发生错误");
            }
        }

        /// <summary>
        /// 加载缓存项统计数据
        /// </summary>
        private async void LoadItemStatistics()
        {
            try
            {
                // 使用异步方式加载统计信息，避免UI线程阻塞
                await Task.Run(() =>
                {
                    // 检查是否实现了ICacheStatistics接口
                    if (_entityCacheManager is ICacheStatistics cacheStats)
                    {
                        // 在UI线程上更新控件
                        this.Invoke(new Action(() =>
                        {
                            // 加载缓存项统计数据，直接使用CacheItemStatistics类
                            var itemStats = cacheStats.GetCacheItemStatistics();
                            // 创建显示用的匿名对象，格式化时间和大小
                            var displayList = itemStats.Values.Select(item => new
                            {
                                CacheKey = item.Key,
                                TypeName = item.ValueType,
                                item.TableName,
                                CreatedTime = item.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                LastAccessed = item.LastAccessedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                AccessCount = item.AccessCount,
                                Size = item.EstimatedSize > 0 ? $"{Math.Round(item.EstimatedSize / 1024.0, 2)} KB" : "N/A"
                            }).ToList();
                            dataGridViewItemStats.DataSource = displayList;

                            // 更新状态栏
                            toolStripStatusLabel1.Text = $"缓存项统计已更新 - 共 {itemStats?.Count ?? 0} 个缓存项";
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载缓存项统计数据时发生错误");
            }
        }

        #region 事件处理


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
                            // 使用构造函数中注入的缓存管理器实例，确保与初始化服务使用的是同一个实例
                            var cacheManager = _entityCacheManager;
                            var datalist = cacheManager.GetEntityListByTableName(tableName);
                            if (datalist != null)
                            {
                                // 在UI线程上更新DataGridView
                                this.Invoke(new Action(() =>
                                {
                                    // 绑定数据到dataGridView
                                    dataGridView1.DataSource = null;
                                    dataGridView1.DataSource = datalist;

                                    // 更新状态栏
                                    int count = datalist.Count;
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
                    this.Invoke(new Action(() => LoadTableStatistics()));
                    this.Invoke(new Action(() => LoadItemStatistics()));
                    this.Invoke(new Action(() => LoadCacheMetadata()));

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

                // 使用构造函数中注入的缓存管理器实例，确保与初始化服务使用的是同一个实例
                var cacheManager = _entityCacheManager;

                // 获取所有可缓存的表名
                List<string> tableNameList = _tableSchemaManager.GetAllTableNames();

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
                        var List = _entityCacheManager.GetEntityListByTableName(tableName);
                        int count = 0;
                        
                        // 直接转换为 IEnumerable 并计算数量
                        if (List != null)
                        {
                            try
                            {
                                count = ((System.Collections.IEnumerable)List).Cast<object>().Count();
                            }
                            catch
                            {
                                // 如果转换失败，默认为0
                                count = 0;
                            }
                        }
                        
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
                    this.Invoke(new Action(() => LoadTableStatistics()));
                    this.Invoke(new Action(() => LoadItemStatistics()));
                    this.Invoke(new Action(() => LoadCacheMetadata()));
                });

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
                        // 使用构造函数中注入的缓存管理器实例，确保与初始化服务使用的是同一个实例
                        var cacheManager = _entityCacheManager;
                        var jArray = cacheManager.GetEntityListByTableName(tableName);
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
                        // 使用构造函数中注入的初始化服务实例，确保与当前缓存管理器使用的是同一个实例
                        // 调用方法初始化单个表的缓存
                        _initializationService.InitializeCacheForTable(tableName);
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
            LoadTableStatistics();
            LoadItemStatistics();
            LoadCacheMetadata();
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

        /// <summary>
        /// 获取所有表的缓存同步信息
        /// 通过_entityCacheManager间接访问ICacheSyncMetadata功能
        /// </summary>
        /// <remarks>
        /// 【根本原因说明】为什么通过_entityCacheManager可以获取正确的元数据字典值：
        /// 
        /// 1. EntityCacheManager在构造函数中接收ICacheSyncMetadata作为可选依赖
        ///    (private readonly ICacheSyncMetadata _cacheSyncMetadata)
        /// 
        /// 2. 当通过依赖注入容器解析EntityCacheManager时，容器会正确地将同一个ICacheSyncMetadata实例
        ///    注入到EntityCacheManager中，而EntityCacheManager内部会使用这个实例进行所有缓存同步操作
        /// 
        /// 3. 直接注入ICacheSyncMetadata可能会因为服务注册顺序或容器初始化问题导致获取到不同的实例
        ///    或者在某些场景下该服务尚未完全初始化
        /// 
        /// 4. 通过反射访问EntityCacheManager内部的_cacheSyncMetadata字段，确保我们使用的是
        ///    与EntityCacheManager实际工作时完全相同的ICacheSyncMetadata实例
        /// 
        /// 5. 这解释了为什么直接使用ICacheSyncMetadata可能会导致元数据不一致，而通过_entityCacheManager
        ///    间接访问却能获取到正确的值
        /// </remarks>
        /// <returns>所有表的缓存同步信息字典</returns>
        private Dictionary<string, CacheSyncInfo> GetAllTableSyncInfo()
        {
            try
            {
                // 尝试从_entityCacheManager中获取ICacheSyncMetadata实例
                Type entityCacheManagerType = _entityCacheManager.GetType();
                var fieldInfo = entityCacheManagerType.GetField("_cacheSyncMetadata", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    var cacheSyncMetadata = fieldInfo.GetValue(_entityCacheManager) as ICacheSyncMetadata;
                    if (cacheSyncMetadata != null)
                    {
                        return cacheSyncMetadata.GetAllTableSyncInfo();
                    }
                    else
                    {
                        _logger?.LogWarning("EntityCacheManager中的_cacheSyncMetadata字段存在但为空");
                    }
                }
                else
                {
                    _logger?.LogWarning("无法通过反射获取EntityCacheManager中的_cacheSyncMetadata字段");
                }

                // 备选方案：尝试直接获取ICacheSyncMetadata服务
                try
                {
                    var cacheSyncMetadata = Startup.GetFromFac<ICacheSyncMetadata>();
                    if (cacheSyncMetadata != null)
                    {
                        return cacheSyncMetadata.GetAllTableSyncInfo();
                    }
                    else
                    {
                        _logger?.LogWarning("从依赖注入容器获取到的ICacheSyncMetadata实例为空");
                    }
                }
                catch (Exception ex)
                {
                    // 如果获取失败，记录详细日志但不抛出异常
                    _logger?.LogWarning(ex, "无法从依赖注入容器获取ICacheSyncMetadata实例");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取缓存同步元数据失败");
            }

            // 返回空字典作为后备方案
            return new Dictionary<string, CacheSyncInfo>();
        }

        /// <summary>
        /// 加载缓存元数据
        /// </summary>
        private void LoadCacheMetadata()
        {
            try
            {
                // 获取所有表的缓存同步元数据
                var allSyncInfo = GetAllTableSyncInfo();

                if (allSyncInfo.Any())
                {
                    // 直接使用现有的CacheSyncInfo模型，添加格式化属性用于显示
                    var viewModelList = allSyncInfo.Values.Select(info => new
                    {
                        info.TableName,
                        info.DataCount,
                        EstimatedSizeMB = Math.Round(info.EstimatedSize / (1024.0 * 1024.0), 2),
                        LastUpdateTime = info.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        ExpirationTime = info.HasExpiration ? info.ExpirationTime.ToString("yyyy-MM-dd HH:mm:ss") : "永不过期",
                        info.HasExpiration,
                        info.SourceInfo
                    }).ToList();

                    // 绑定数据到DataGridView
                    dataGridViewCacheMetadata.DataSource = viewModelList;

                    // 更新状态栏显示记录数
                    toolStripStatusLabel1.Text = $"缓存元数据已更新 - 共 {viewModelList.Count} 个表";

                }
                else
                {
                    dataGridViewCacheMetadata.DataSource = null;
                    _logger?.LogWarning("无法获取缓存同步元数据信息");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载缓存元数据时发生错误");
                dataGridViewCacheMetadata.DataSource = null;
            }
        }

        /// <summary>
        /// 刷新缓存元数据按钮点击事件
        /// </summary>
        private async void btnRefreshMetadata_Click(object sender, EventArgs e)
        {
            try
            {
                // 显示刷新状态
                btnRefreshMetadata.Enabled = false;
                btnRefreshMetadata.Text = "刷新中...";
                toolStripStatusLabel1.Text = "正在刷新缓存元数据...";
                this.Cursor = Cursors.WaitCursor;

                // 使用异步方式刷新，避免UI阻塞
                await Task.Run(() =>
                {
                    this.Invoke(new Action(() => LoadCacheMetadata()));
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新缓存元数据时发生错误");
                MessageBox.Show($"刷新缓存元数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                btnRefreshMetadata.Enabled = true;
                btnRefreshMetadata.Text = "刷新元数据";
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 初始化自动刷新定时器
        /// </summary>
        private void InitializeAutoRefreshTimer()
        {
            _autoRefreshTimer = new System.Windows.Forms.Timer();
            _autoRefreshTimer.Interval = 10000; // 10秒检查一次是否需要刷新
            _autoRefreshTimer.Tick += AutoRefreshTimer_Tick;
            _autoRefreshTimer.Start(); // 启动定时器
        }

        /// <summary>
        /// 定时刷新事件处理
        /// </summary>
        private void AutoRefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // 只在缓存统计页面且处于前台时自动刷新
                if (tabControl1.SelectedTab == tabPageCacheStatistics)
                {
                    DateTime now = DateTime.Now;

                    // 按表统计：每60秒刷新一次
                    if (tabControl2.SelectedTab == tabPageTableStats &&
                        (now - _lastTableStatsRefresh).TotalSeconds >= 60)
                    {
                        LoadTableStatistics();
                        _lastTableStatsRefresh = now;
                    }

                    // 缓存项统计：不自动刷新（数据量大，手动刷新更合适）
                    // 这里保持不自动刷新，避免性能影响

                    // 缓存元数据：每30秒刷新一次
                    if (tabControl2.SelectedTab == tabPageCacheMetadata &&
                        (now - _lastMetadataRefresh).TotalSeconds >= 30)
                    {
                        LoadCacheMetadata();
                        _lastMetadataRefresh = now;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "自动刷新缓存统计时发生错误");
            }
        }

        /// <summary>
        /// 刷新按表统计按钮点击事件
        /// </summary>
        private async void btnRefreshTableStats_Click(object sender, EventArgs e)
        {
            try
            {
                // 显示刷新状态
                btnRefreshTableStats.Enabled = false;
                btnRefreshTableStats.Text = "刷新中...";
                toolStripStatusLabel1.Text = "正在刷新按表统计...";
                this.Cursor = Cursors.WaitCursor;

                // 使用异步方式刷新，避免UI阻塞
                await Task.Run(() =>
                {
                    this.Invoke(new Action(() => LoadTableStatistics()));
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新按表统计时发生错误");
                MessageBox.Show($"刷新按表统计失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                btnRefreshTableStats.Enabled = true;
                btnRefreshTableStats.Text = "刷新统计";
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 刷新缓存项统计按钮点击事件
        /// </summary>
        private async void btnRefreshItemStats_Click(object sender, EventArgs e)
        {
            try
            {
                // 显示刷新状态
                btnRefreshItemStats.Enabled = false;
                btnRefreshItemStats.Text = "刷新中...";
                toolStripStatusLabel1.Text = "正在刷新缓存项统计...";
                this.Cursor = Cursors.WaitCursor;

                // 使用异步方式刷新，避免UI阻塞
                await Task.Run(() =>
                {
                    this.Invoke(new Action(() => LoadItemStatistics()));
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新缓存项统计时发生错误");
                MessageBox.Show($"刷新缓存项统计失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                btnRefreshItemStats.Enabled = true;
                btnRefreshItemStats.Text = "刷新项统计";
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 重写Dispose方法以释放定时器资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 停止并释放定时器
                if (_autoRefreshTimer != null)
                {
                    _autoRefreshTimer.Stop();
                    _autoRefreshTimer.Dispose();
                    _autoRefreshTimer = null;
                }

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion





        // 查找并替换原有对_cacheSyncMetadataManager的调用
        private void RefreshMetadataOnTimerTick()
        {
            // 确保只在必要时刷新元数据，避免过于频繁的更新
            if ((DateTime.Now - _lastMetadataRefresh).TotalSeconds >= 5)
            {
                BeginInvoke((Action)(() => LoadCacheMetadata()));
            }
        }
    }
}