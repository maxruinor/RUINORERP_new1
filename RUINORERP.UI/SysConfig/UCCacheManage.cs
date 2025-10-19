﻿using AutoUpdateTools;
using CacheManager.Core;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using HLH.Lib.Helper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.Middlewares;
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
        private readonly TableSchemaManager _tableSchemaManager;
        


        public UCCacheManage()
        {
            InitializeComponent();
            // 通过依赖注入获取缓存管理器
            _cacheManager = Startup.GetFromFac<IEntityCacheManager>();
            _tableSchemaManager = TableSchemaManager.Instance;
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

        private void LoadCacheStatistics()
        {
            try
            {
                // 更新总体统计指标
                txtHitRatio.Text = $"{_cacheManager.HitRatio:P2}";
                txtTotalItems.Text = _cacheManager.CacheItemCount.ToString();
                // 改为MB单位
                txtCacheSize.Text = $"{(_cacheManager.EstimatedCacheSize / (1024.0 * 1024.0)):F2} MB";

                // 更新按表统计数据
                var tableStats = _cacheManager.GetTableCacheStatistics();
                var tableStatsList = new List<TableStatisticsDisplay>();
                
                foreach (var stats in tableStats)
                {
                    tableStatsList.Add(new TableStatisticsDisplay
                    {
                        TableName = stats.Value.TableName,
                        ItemCount = stats.Value.TotalItemCount,
                        // 改为MB单位
                        TotalSize = (stats.Value.EstimatedTotalSize / (1024.0 * 1024.0)).ToString("F2"),
                        HitRatio = stats.Value.HitRatio.ToString("P2"),
                        LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") // 使用当前时间替代，因为TableCacheStatistics没有LastAccessedTime属性
                    });
                }
                
                // 更新表个数统计信息
                txtTableCount.Text = tableStats.Count.ToString();
                
                dgvTableStatistics.DataSource = tableStatsList;

                // 更新缓存项统计数据
                var itemStats = _cacheManager.GetCacheItemStatistics();
                var itemStatsList = new List<ItemStatisticsDisplay>();
                
                foreach (var stats in itemStats)
                {
                    itemStatsList.Add(new ItemStatisticsDisplay
                    {
                        CacheKey = stats.Value.Key,
                        EntityType = stats.Value.ValueType ?? "Unknown",
                        TableName = stats.Value.TableName,
                        CreationTime = stats.Value.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        // 改为MB单位
                        Size = (stats.Value.EstimatedSize / (1024.0 * 1024.0)).ToString("F3")
                    });
                }
                
                dgvItemStatistics.DataSource = itemStatsList;
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show($"加载缓存统计失败: {ex.Message}", "错误", Krypton.Toolkit.KryptonMessageBoxButtons.OK, Krypton.Toolkit.KryptonMessageBoxIcon.Error);
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
                    Console.WriteLine($"处理表 {tableName} 时发生错误: {ex.Message}");
                }
            }
        }

        private void btnRefreshCache_Click(object sender, EventArgs e)
        {
            LoadCacheToUI();
            // 同时刷新统计数据
            LoadCacheStatistics();
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
                    Console.WriteLine($"加载表 {tableName} 数据时发生错误: {ex.Message}");
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
                    Console.WriteLine($"清空表 {tableName} 缓存时发生错误: {ex.Message}");
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

        private void btnRefreshStats_Click(object sender, EventArgs e)
        {   
            // 刷新统计数据
            LoadCacheStatistics();
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

        public List<UControls.ContextMenuController> AddContextMenu()
        {
            return new List<UControls.ContextMenuController>();
        }
    }

    public class TableStatisticsDisplay
        {   
            public string TableName { get; set; }
            public int ItemCount { get; set; }
            public string TotalSize { get; set; }
            public string HitRatio { get; set; }
            public string LastUpdateTime { get; set; }
        }

        public class ItemStatisticsDisplay
        {   
            public string CacheKey { get; set; }
            public string EntityType { get; set; }
            public string TableName { get; set; }
            public string CreationTime { get; set; }
            public string Size { get; set; }
        }
}
