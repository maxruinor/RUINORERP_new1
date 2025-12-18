using AutoUpdateTools;
using CacheManager.Core;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using HLH.Lib.Helper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;

using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebSockets;
using System.Windows.Forms;
using System.Windows.Media;
using Krypton.Toolkit;
using Krypton.Navigator;
using RUINORERP.UI.AdvancedUIModule;

namespace RUINORERP.UI.SysConfig
{
    [MenuAttrAssemblyInfo("缓存管理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCCacheManage : UserControl, IContextMenuInfoAuth
    {
        private readonly IEntityCacheManager _cacheManager;
        private readonly ITableSchemaManager _tableSchemaManager;
        private readonly CacheSyncMetadataManager _cacheSyncMetadataManager;
        private System.Windows.Forms.Timer _autoRefreshTimer;
        private DateTime _lastTableStatsRefresh = DateTime.MinValue;
        private DateTime _lastItemStatsRefresh = DateTime.MinValue;
        private DateTime _lastMetadataRefresh = DateTime.MinValue;
        


        public UCCacheManage()
        {
            InitializeComponent();
            // 使用静态缓存管理器
            _cacheManager = Startup.GetFromFac<IEntityCacheManager>();
            _tableSchemaManager = Startup.GetFromFac<ITableSchemaManager>();
            _cacheSyncMetadataManager = Startup.GetFromFac<CacheSyncMetadataManager>();
            
            // 初始化定时刷新器
            InitializeAutoRefreshTimer();
        }

        private async void 请求缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kryptonListBox1.SelectedItem is SuperValue kv)
            {
                string tableName = kv.superDataTypeName;
                await UIBizService.RequestCache(tableName);
            }
        }

        private void UCCacheManage_Load(object sender, EventArgs e)
        {
            //初始化表格
            InitStatisticsGrid();
            //加载缓存
            LoadCacheToUI();
            //加载缓存统计数据
            LoadCacheStatistics();
            // 初始化缓存元数据
            InitCacheMetadataGrid();
            // 加载缓存元数据
            LoadCacheMetadata();
            
            // 初始化刷新时间戳
            DateTime now = DateTime.Now;
            _lastTableStatsRefresh = now;
            _lastItemStatsRefresh = now;
            _lastMetadataRefresh = now;
        }

        private void InitStatisticsGrid()
        {
            // 配置按表统计数据网格
            dgvTableStatistics.AutoGenerateColumns = false;
            dgvTableStatistics.Columns.Clear();

            DataGridViewTextBoxColumn colTableName = new DataGridViewTextBoxColumn();
            colTableName.Name = "TableName";
            colTableName.HeaderText = "表名";
            colTableName.DataPropertyName = "TableName";
            colTableName.Width = 150;
            dgvTableStatistics.Columns.Add(colTableName);

            DataGridViewTextBoxColumn colItemCount = new DataGridViewTextBoxColumn();
            colItemCount.Name = "ItemCount";
            colItemCount.HeaderText = "缓存项数量";
            colItemCount.DataPropertyName = "ItemCount";
            colItemCount.Width = 100;
            colItemCount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvTableStatistics.Columns.Add(colItemCount);

            DataGridViewTextBoxColumn colSize = new DataGridViewTextBoxColumn();
            colSize.Name = "TotalSize";
            colSize.HeaderText = "总大小(KB)";
            colSize.DataPropertyName = "TotalSize";
            colSize.Width = 100;
            colSize.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvTableStatistics.Columns.Add(colSize);

            DataGridViewTextBoxColumn colHitRatio = new DataGridViewTextBoxColumn();
            colHitRatio.Name = "HitRatio";
            colHitRatio.HeaderText = "命中率(%)";
            colHitRatio.DataPropertyName = "HitRatio";
            colHitRatio.Width = 100;
            colHitRatio.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvTableStatistics.Columns.Add(colHitRatio);

            DataGridViewTextBoxColumn colLastUpdate = new DataGridViewTextBoxColumn();
            colLastUpdate.Name = "LastUpdateTime";
            colLastUpdate.HeaderText = "最后更新时间";
            colLastUpdate.DataPropertyName = "LastUpdateTime";
            colLastUpdate.Width = 150;
            dgvTableStatistics.Columns.Add(colLastUpdate);

            // 配置缓存项统计数据网格
            dgvItemStatistics.AutoGenerateColumns = false;
            dgvItemStatistics.Columns.Clear();

            DataGridViewTextBoxColumn colCacheKey = new DataGridViewTextBoxColumn();
            colCacheKey.Name = "CacheKey";
            colCacheKey.HeaderText = "缓存键";
            colCacheKey.DataPropertyName = "CacheKey";
            colCacheKey.Width = 250;
            dgvItemStatistics.Columns.Add(colCacheKey);

            DataGridViewTextBoxColumn colType = new DataGridViewTextBoxColumn();
            colType.Name = "EntityType";
            colType.HeaderText = "实体类型";
            colType.DataPropertyName = "EntityType";
            colType.Width = 150;
            dgvItemStatistics.Columns.Add(colType);

            DataGridViewTextBoxColumn colItemTableName = new DataGridViewTextBoxColumn();
            colItemTableName.Name = "TableName";
            colItemTableName.HeaderText = "表名";
            colItemTableName.DataPropertyName = "TableName";
            colItemTableName.Width = 120;
            dgvItemStatistics.Columns.Add(colItemTableName);

            DataGridViewTextBoxColumn colCreationTime = new DataGridViewTextBoxColumn();
            colCreationTime.Name = "CreationTime";
            colCreationTime.HeaderText = "创建时间";
            colCreationTime.DataPropertyName = "CreationTime";
            colCreationTime.Width = 150;
            dgvItemStatistics.Columns.Add(colCreationTime);

            DataGridViewTextBoxColumn colItemSize = new DataGridViewTextBoxColumn();
            colItemSize.Name = "Size";
            colItemSize.HeaderText = "大小(KB)";
            colItemSize.DataPropertyName = "Size";
            colItemSize.Width = 100;
            colItemSize.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvItemStatistics.Columns.Add(colItemSize);
        }

        /// <summary>
        /// 初始化缓存元数据表格
        /// </summary>
        private void InitCacheMetadataGrid()
        {
            dgvCacheMetadata.AutoGenerateColumns = false;
            dgvCacheMetadata.Columns.Clear();

            DataGridViewTextBoxColumn colTableName = new DataGridViewTextBoxColumn();
            colTableName.Name = "TableName";
            colTableName.HeaderText = "表名";
            colTableName.DataPropertyName = "TableName";
            colTableName.Width = 150;
            dgvCacheMetadata.Columns.Add(colTableName);

            DataGridViewTextBoxColumn colDataCount = new DataGridViewTextBoxColumn();
            colDataCount.Name = "DataCount";
            colDataCount.HeaderText = "数据数量";
            colDataCount.DataPropertyName = "DataCount";
            colDataCount.Width = 80;
            colDataCount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCacheMetadata.Columns.Add(colDataCount);

            DataGridViewTextBoxColumn colEstimatedSize = new DataGridViewTextBoxColumn();
            colEstimatedSize.Name = "EstimatedSizeMB";
            colEstimatedSize.HeaderText = "估计大小(MB)";
            colEstimatedSize.DataPropertyName = "EstimatedSizeMB";
            colEstimatedSize.Width = 100;
            colEstimatedSize.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCacheMetadata.Columns.Add(colEstimatedSize);

            DataGridViewTextBoxColumn colLastUpdateTime = new DataGridViewTextBoxColumn();
            colLastUpdateTime.Name = "LastUpdateTime";
            colLastUpdateTime.HeaderText = "最后更新时间";
            colLastUpdateTime.DataPropertyName = "LastUpdateTime";
            colLastUpdateTime.Width = 150;
            dgvCacheMetadata.Columns.Add(colLastUpdateTime);

            DataGridViewTextBoxColumn colExpirationTime = new DataGridViewTextBoxColumn();
            colExpirationTime.Name = "ExpirationTime";
            colExpirationTime.HeaderText = "过期时间";
            colExpirationTime.DataPropertyName = "ExpirationTime";
            colExpirationTime.Width = 150;
            dgvCacheMetadata.Columns.Add(colExpirationTime);

            DataGridViewTextBoxColumn colHasExpiration = new DataGridViewTextBoxColumn();
            colHasExpiration.Name = "HasExpiration";
            colHasExpiration.HeaderText = "是否有过期设置";
            colHasExpiration.DataPropertyName = "HasExpiration";
            colHasExpiration.Width = 100;
            dgvCacheMetadata.Columns.Add(colHasExpiration);

            DataGridViewTextBoxColumn colSourceInfo = new DataGridViewTextBoxColumn();
            colSourceInfo.Name = "SourceInfo";
            colSourceInfo.HeaderText = "源信息";
            colSourceInfo.DataPropertyName = "SourceInfo";
            colSourceInfo.Width = 200;
            dgvCacheMetadata.Columns.Add(colSourceInfo);
        }

        /// <summary>
        /// 加载缓存统计数据
        /// </summary>
        private async void LoadCacheStatistics()
        {
            try
            {
                // 使用异步方式加载统计信息，避免UI阻塞
                await Task.Run(() =>
                {
                    // 在UI线程上更新控件
                    this.Invoke(new Action(() =>
                    {
                        // 更新总体统计指标
                        txtHitRatio.Text = $"{_cacheManager.HitRatio:P2}";
                        txtTotalItems.Text = _cacheManager.CacheItemCount.ToString();
                        // 改为MB单位
                        txtCacheSize.Text = $"{(_cacheManager.EstimatedCacheSize / (1024.0 * 1024.0)):F2} MB";

                        LoadTableStatistics();
                        LoadItemStatistics();
                    }));
                });
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show($"加载缓存统计失败: {ex.Message}", "错误", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载按表统计数据
        /// </summary>
        private void LoadTableStatistics()
        {
            try
            {
                // 更新按表统计数据，直接使用TableCacheStatistics类
                var tableStats = _cacheManager.GetTableCacheStatistics();
                
                // 创建显示用的匿名对象列表
                var tableStatsList = tableStats.Values.Select(stats => new
                {
                    TableName = stats.TableName,
                    ItemCount = stats.TotalItemCount,
                    // 改为MB单位
                    TotalSize = (stats.EstimatedTotalSize / (1024.0 * 1024.0)).ToString("F2"),
                    HitRatio = stats.HitRatio.ToString("P2"),
                    LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToList();
                
                // 更新表个数统计信息
                txtTableCount.Text = tableStats.Count.ToString();
                
                dgvTableStatistics.DataSource = tableStatsList;
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show($"加载按表统计失败: {ex.Message}", "错误", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载缓存项统计数据
        /// </summary>
        private void LoadItemStatistics()
        {
            try
            {
                // 更新缓存项统计数据，直接使用CacheItemStatistics类
                var itemStats = _cacheManager.GetCacheItemStatistics();
                
                // 创建显示用的匿名对象列表
                var itemStatsList = itemStats.Values.Select(stats => new
                {
                    CacheKey = stats.Key,
                    EntityType = stats.ValueType ?? "Unknown",
                    TableName = stats.TableName,
                    CreationTime = stats.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    // 改为MB单位
                    Size = (stats.EstimatedSize / (1024.0 * 1024.0)).ToString("F3")
                }).ToList();
                
                dgvItemStatistics.DataSource = itemStatsList;
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show($"加载缓存项统计失败: {ex.Message}", "错误", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载缓存元数据
        /// </summary>
        private void LoadCacheMetadata()
        {
            try
            {
                if (_cacheSyncMetadataManager != null)
                {
                    // 获取所有表的缓存同步元数据
                    var allSyncInfo = _cacheSyncMetadataManager.GetAllTableSyncInfo();
                    
                    // 创建显示用的匿名对象列表，将字节转换为MB并格式化时间
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
                    dgvCacheMetadata.DataSource = viewModelList;
                }
                else
                {
                    dgvCacheMetadata.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show($"加载缓存元数据失败: {ex.Message}", "错误", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Error);
            }
        }

        private void LoadCacheToUI()
        {
            //加载所有缓存的表
            kryptonListBox1.Items.Clear();

            // 获取所有可缓存的表名
            List<string> list = new List<string>();
            // 从TableSchemaManager获取所有表信息
            foreach (var schemaInfo in _tableSchemaManager.GetAllSchemaInfo())
            {
                list.Add(schemaInfo.TableName);
            }
            list.Sort();

            foreach (var tableName in list)
            {
                try
                {
                    // 获取实体类型
                    var entityType = _tableSchemaManager.GetEntityType(tableName);
                    if (entityType != null)
                    {
                        // 使用反射调用泛型方法
                        var method = typeof(IEntityCacheManager).GetMethod("GetEntityList", new Type[] { typeof(string) })
                            .MakeGenericMethod(entityType);
                        var cacheList = method.Invoke(_cacheManager, new object[] { tableName }) as System.Collections.IEnumerable;

                        int count = 0;
                        if (cacheList != null)
                        {
                            count = cacheList.Cast<object>().Count();
                        }

                        SuperValue kv = new SuperValue(tableName + "[" + count + "]", tableName);
                        kryptonListBox1.Items.Add(kv);
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但继续处理其他表
                    System.Diagnostics.Debug.WriteLine($"处理表 {tableName} 时发生错误: {ex.Message}");
                }
            }
        }

        private async void btnRefreshCache_Click(object sender, EventArgs e)
        {
            try
            {
                // 显示加载状态
                btnRefreshCache.Enabled = false;
                btnRefreshCache.Values.Text = "刷新中...";

                // 使用异步方式刷新，避免UI阻塞
                await Task.Run(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        LoadCacheToUI();
                        LoadTableStatistics();
                        LoadItemStatistics();
                        LoadCacheMetadata();
                    }));
                });
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show($"刷新缓存失败: {ex.Message}", "错误", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                btnRefreshCache.Enabled = true;
                btnRefreshCache.Values.Text = "刷新缓存";
            }
        }

        /// <summary>
        /// 所有实体表都在这个命名空间下，不需要每次都反射
        /// </summary>
        Assembly assembly = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
        private void kryptonListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {   
            if (kryptonListBox1.SelectedItem is SuperValue kv)
            {   
                string tableName = kv.superDataTypeName;

                try
                {   
                    // 获取实体类型
                    Type type = assembly.GetType("RUINORERP.Model." + tableName);
                    if (type == null)
                    {   
                        dataGridView1.DataSource = null;
                        return;
                    }

                    // 使用反射调用泛型方法获取实体列表
                    var method = typeof(IEntityCacheManager).GetMethod("GetEntityList", new Type[] { typeof(string) })
                        .MakeGenericMethod(type);
                    var cacheList = method.Invoke(_cacheManager, new object[] { tableName });

                    if (cacheList == null)
                    {   
                        dataGridView1.DataSource = null;
                        return;
                    }

                    // 设置DataGridView属性
                    dataGridView1.NeedSaveColumnsXml = true;
                    dataGridView1.FieldNameList = UIHelper.GetFieldNameColList(true, type);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView1.XmlFileName = "UCCacheManage" + tableName;
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = cacheList;
                }
                catch (Exception ex)
                {   
                    System.Diagnostics.Debug.WriteLine($"加载表 {tableName} 数据时发生错误: {ex.Message}");
                    dataGridView1.DataSource = null;
                }
            }
            else
            {   
                dataGridView1.DataSource = null;
                return;
            }
        }

        private void 清空选中缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            if (kryptonListBox1.SelectedItem is SuperValue kv)
            {   
                string tableName = kv.superDataTypeName;
                
                try
                {   
                    // 直接使用DeleteEntityList的非泛型重载方法
                    _cacheManager.DeleteEntityList(tableName);
                    
                    // 刷新UI
                    LoadCacheToUI();
                    // 刷新统计数据
                    LoadCacheStatistics();
                    
                    KryptonMessageBox.Show($"缓存 '{tableName}' 已清空", "提示", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Information);
                }
                catch (Exception ex)
                {   
                    System.Diagnostics.Debug.WriteLine($"清空表 {tableName} 缓存时发生错误: {ex.Message}");
                    KryptonMessageBox.Show($"清空缓存失败: {ex.Message}", "错误", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {   
            // 保存配置
            KryptonMessageBox.Show("配置已保存", "提示", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Information);
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {   
            // 全选/取消全选
            for (int i = 0; i < kryptonListBox1.Items.Count; i++)
            {   
                kryptonListBox1.SetSelected(i, chkALL.Checked);
            }
        }

        private async void btnRefreshStats_Click(object sender, EventArgs e)
        {   
            try
            {
                // 显示加载状态
                btnRefreshStats.Enabled = false;
                btnRefreshStats.Values.Text = "刷新中...";

                // 使用异步方式刷新，避免UI阻塞
                await Task.Run(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        LoadTableStatistics();
                        LoadItemStatistics();
                        LoadCacheMetadata();
                    }));
                });
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show($"刷新统计数据失败: {ex.Message}", "错误", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                btnRefreshStats.Enabled = true;
                btnRefreshStats.Values.Text = "刷新统计数据";
            }
        }

        private void btnResetStats_Click(object sender, EventArgs e)
        {   
            // 重置统计信息
            try
            {   
                _cacheManager.ResetStatistics();
                KryptonMessageBox.Show("缓存统计信息已重置", "提示", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Information);
                // 重新加载统计数据
                LoadCacheStatistics();
            }
            catch (Exception ex)
            {   
                KryptonMessageBox.Show($"重置统计信息失败: {ex.Message}", "错误", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Error);
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
                btnRefreshMetadata.Values.Text = "刷新中...";

                // 使用异步方式刷新，避免UI阻塞
                await Task.Run(() =>
                {
                    this.Invoke(new Action(() => LoadCacheMetadata()));
                });
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show($"刷新缓存元数据失败: {ex.Message}", "错误", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                btnRefreshMetadata.Enabled = true;
                btnRefreshMetadata.Values.Text = "刷新元数据";
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
                if (statisticsNavigator.SelectedPage != null)
                {
                    DateTime now = DateTime.Now;
                    
                    // 按表统计：每60秒刷新一次
                    if (statisticsNavigator.SelectedPage.Name == tabPageTableStats.Name && 
                        (now - _lastTableStatsRefresh).TotalSeconds >= 60)
                    {
                        LoadTableStatistics();
                        _lastTableStatsRefresh = now;
                    }
                    
                    // 缓存项统计：不自动刷新（数据量大，手动刷新更合适）
                    
                    // 缓存元数据：每30秒刷新一次
                    if (statisticsNavigator.SelectedPage.Name == tabPageCacheMetadata.Name && 
                        (now - _lastMetadataRefresh).TotalSeconds >= 30)
                    {
                        LoadCacheMetadata();
                        _lastMetadataRefresh = now;
                    }
                }
            }
            catch (Exception ex)
            {
                // 静默处理错误，避免频繁弹窗
                System.Diagnostics.Debug.WriteLine($"自动刷新缓存统计时发生错误: {ex.Message}");
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

        public List<UControls.ContextMenuController> AddContextMenu()
        {
            return new List<UControls.ContextMenuController>();
        }
    }
}
