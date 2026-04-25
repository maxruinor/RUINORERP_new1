// **************************************
// 文件：UCBasicDataCleanup.cs
// 项目：RUINORERP
// 作者：AI Assistant
// 时间：2026-04-20
// 描述：简洁版数据清理组件，支持级联删除
// **************************************

using Krypton.Toolkit;
using RUINORERP.Business;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 基础数据清理 UI 组件 - 简洁版
    /// 使用 DataCleanupEngine 复用 BaseController.BaseDeleteByNavAsync
    /// </summary>
    [MenuAttrAssemblyInfo("基础数据清理", ModuleMenuDefine.模块定义.系统设置,ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCBasicDataCleanup : UserControl
    {
        private ISqlSugarClient _db;
        private Type _selectedEntityType;
        private List<long> _selectedIds;
        private DataCleanupEngine _cleanupEngine; // 使用新的清理引擎
        private List<EntityMetadata> _allEntities; // 所有实体元数据
        private bool _isInitializing = false; // 防止搜索时触发递归
        private bool _isBusy = false; // 防止重复操作
        private CancellationTokenSource _currentCancellationTokenSource; // 当前操作的取消令牌
        private Dictionary<string, long> _tableRecordCounts; // 表名 -> 记录数的缓存



        /// <summary>
        /// 构造函数
        /// </summary>
        public UCBasicDataCleanup()
        {
            InitializeComponent();
            InitializeData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitializeData()
        {
            _selectedIds = new List<long>();
            _isInitializing = false;
            _isBusy = false;
            
            // 初始化数据网格视图
            dgvDataPreview.AutoGenerateColumns = true;
            dgvDataPreview.AllowUserToAddRows = false;
            dgvDataPreview.AllowUserToDeleteRows = false;

            // 初始化数据库连接
            LoadDbConnection();

            // 初始化清理引擎
            _cleanupEngine = new DataCleanupEngine();
            // 将日志事件绑定到实时日志面板
            _cleanupEngine.OnLog += (sender, log) => AppendRealTimeLog(log);
            _cleanupEngine.OnProgressChanged += (sender, e) => 
            {
                MainForm.Instance.ShowStatusText($"{e.Message} ({e.Percentage}%)");
                UpdateProgressBar(e.Percentage);
            };

            // 加载所有实体元数据
            _allEntities = EntityRegistry.Entities;

            // 初始化实体类型选择
            InitializeEntityTypes();
            
            // 异步加载表记录数（不阻塞UI）
            _ = LoadTableRecordCountsAsync();
            
            // 初始化删除方式选择
            InitializeDeleteMode();
            
            // 绑定事件
            BindEvents();

            // 添加数据选择列到预览表格
            AddCheckBoxColumnToPreview();
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void KbtnCancel_Click(object sender, EventArgs e)
        {
            if (_currentCancellationTokenSource != null && !_currentCancellationTokenSource.IsCancellationRequested)
            {
                var result = MessageBox.Show("确定要取消当前操作吗？\n\n已执行的操作可能无法撤销。", "确认取消", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    AppendRealTimeLog("用户请求取消操作...");
                    _cleanupEngine.CancelOperation();
                    _currentCancellationTokenSource?.Cancel();
                    kbtnCancel.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 添加 CheckBox 列到预览表格
        /// </summary>
        private void AddCheckBoxColumnToPreview()
        {
            var checkboxColumn = new DataGridViewCheckBoxColumn
            {
                Name = "colSelect",
                HeaderText = "选择",
                Width = 50,
                Frozen = true
            };
            dgvDataPreview.Columns.Insert(0, checkboxColumn);
        }

        /// <summary>
        /// 加载数据库连接
        /// </summary>
        private void LoadDbConnection()
        {
            try
            {
                _db = MainForm.Instance.AppContext.Db;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"数据库连接失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化实体类型选择 TreeView
        /// </summary>
        private void InitializeEntityTypes()
        {
            _isInitializing = true;
            treeViewTableList.Nodes.Clear();
            
            // 使用 EntityRegistry 获取分组数据
            var groupedEntities = EntityRegistry.GetAllGrouped();
            
            foreach (var group in groupedEntities)
            {
                // 添加分类节点（支持 CheckBox，用于全选/取消全选）
                TreeNode categoryNode = new TreeNode(group.Key);
                categoryNode.Tag = "category"; // 标记为分类节点
                categoryNode.Checked = false; // 默认不选中
                
                // 添加该分类下的所有实体
                foreach (var entity in group.Value)
                {
                    TreeNode entityNode = new TreeNode(entity.DisplayName);
                    // 初始时使用 Type，后续会更新为 TreeNodeData
                    entityNode.Tag = entity.EntityType;
                    entityNode.ToolTipText = $"表名: {entity.TableName}\n描述: {entity.Description}";
                    categoryNode.Nodes.Add(entityNode);
                }
                
                treeViewTableList.Nodes.Add(categoryNode);
            }
            
            // 默认展开第一个分类
            if (treeViewTableList.Nodes.Count > 0)
            {
                treeViewTableList.Nodes[0].Expand();
            }
            
            _isInitializing = false;
        }

        /// <summary>
        /// TreeView CheckBox 选中状态改变事件（处理单选/多选逻辑）
        /// </summary>
        private void treeViewTableList_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_isInitializing)
                return;

            // 判断是否是分类节点点击
            if (e.Node.Tag != null && e.Node.Tag.ToString() == "category")
            {
                // 分类节点被点击：全选/取消全选该分类下的所有子节点
                HandleCategoryCheckChanged(e.Node);
                return;
            }

            // 实体节点被点击：处理选择逻辑
            ProcessSelectionAfterCheck();
        }

        /// <summary>
        /// 处理选择逻辑（提取为独立方法，避免重复代码）
        /// </summary>
        private void ProcessSelectionAfterCheck()
        {
            // 获取所有选中的子节点（实体节点）
            var selectedNodes = GetSelectedEntityNodes();
            
            if (selectedNodes.Count == 0)
            {
                // 没有选中任何实体
                _selectedEntityType = null;
                dgvDataPreview.Rows.Clear();
                kbtnDeleteSelected.Enabled = false;
                kbtnPreview.Enabled = false;
                return;
            }

            if (selectedNodes.Count == 1)
            {
                // 单选：加载预览数据
                var node = selectedNodes[0];
                
                // ✅ 修复：支持 Type 和 TreeNodeData 两种Tag类型
                if (node.Tag is Type type)
                {
                    _selectedEntityType = type;
                }
                else if (node.Tag is TreeNodeData nodeData)
                {
                    _selectedEntityType = nodeData.EntityType;
                }
                
                if (_selectedEntityType != null)
                {
                    _ = LoadDataAsync();
                    kbtnPreview.Enabled = true;
                    kbtnDeleteSelected.Enabled = true;
                }
            }
            else
            {
                // 多选：不加载预览，直接启用删除按钮
                _selectedEntityType = null;
                dgvDataPreview.Rows.Clear();
                kbtnPreview.Enabled = false;
                kbtnDeleteSelected.Enabled = true;
                
                // 显示多选提示
                AppendRealTimeLog($"已选择 {selectedNodes.Count} 个数据表，将进行批量删除");
            }
        }

        /// <summary>
        /// 处理分类节点 CheckBox 变化（全选/取消全选子节点）
        /// </summary>
        private void HandleCategoryCheckChanged(TreeNode categoryNode)
        {
            _isInitializing = true;
            
            bool isChecked = categoryNode.Checked;
            
            // ✅ 修复：使用 ByCode 避免触发 AfterCheck 事件
            foreach (TreeNode childNode in categoryNode.Nodes)
            {
                childNode.Checked = isChecked;
            }
            
            _isInitializing = false;
            
            // 手动触发后续逻辑（不通过事件，直接调用）
            ProcessSelectionAfterCheck();
        }

        /// <summary>
        /// 获取所有选中的实体节点（排除分类节点）
        /// </summary>
        private List<TreeNode> GetSelectedEntityNodes()
        {
            var result = new List<TreeNode>();
            
            foreach (TreeNode categoryNode in treeViewTableList.Nodes)
            {
                foreach (TreeNode entityNode in categoryNode.Nodes)
                {
                    // 支持两种Tag类型：Type 或 TreeNodeData
                    if (entityNode.Checked && (entityNode.Tag is Type || entityNode.Tag is TreeNodeData))
                    {
                        result.Add(entityNode);
                    }
                }
            }
            
            return result;
        }

        /// <summary>
        /// 获取所有选中的实体类型
        /// </summary>
        private List<Type> GetSelectedEntityTypes()
        {
            var selectedNodes = GetSelectedEntityNodes();
            return selectedNodes
                .Where(n => n.Tag is Type || n.Tag is TreeNodeData)
                .Select(n => 
                {
                    if (n.Tag is Type type)
                        return type;
                    else if (n.Tag is TreeNodeData nodeData)
                        return nodeData.EntityType;
                    return null;
                })
                .Where(t => t != null)
                .ToList();
        }

        /// <summary>
        /// 异步加载所有表的记录数并缓存
        /// </summary>
        private async Task LoadTableRecordCountsAsync()
        {
            try
            {
                MainForm.Instance.ShowStatusText("正在加载表记录数...");
                AppendRealTimeLog("开始预加载表记录数...");
                
                _tableRecordCounts = new Dictionary<string, long>();
                var db = MainForm.Instance.AppContext.Db;
                
                int loadedCount = 0;
                int totalCount = _allEntities.Count;
                
                foreach (var entity in _allEntities)
                {
                    try
                    {
                        // 使用 COUNT(*) 查询记录数
                        var count = await db.Queryable<object>()
                            .AS(entity.TableName)
                            .CountAsync();
                        
                        _tableRecordCounts[entity.TableName] = count;
                        loadedCount++;
                        
                        // 每加载50个表更新一次状态
                        if (loadedCount % 50 == 0)
                        {
                            MainForm.Instance.ShowStatusText($"已加载 {loadedCount}/{totalCount} 个表的记录数");
                        }
                    }
                    catch (Exception ex)
                    {
                        // 查询失败时记录为0，不影响其他表
                        _tableRecordCounts[entity.TableName] = 0;
                        AppendRealTimeLog($"[警告] 查询表 {entity.TableName} 记录数失败: {ex.Message}");
                    }
                }
                
                AppendRealTimeLog($"表记录数加载完成，共 {loadedCount} 个表");
                MainForm.Instance.ShowStatusText($"表记录数加载完成 ({loadedCount} 个表)");
                
                // 更新TreeView显示，添加记录数信息
                UpdateTreeViewWithRecordCounts();
            }
            catch (Exception ex)
            {
                AppendRealTimeLog($"[错误] 加载表记录数失败: {ex.Message}");
                MainForm.Instance.ShowStatusText("加载表记录数失败");
            }
        }

        /// <summary>
        /// 更新TreeView节点，显示记录数
        /// </summary>
        private void UpdateTreeViewWithRecordCounts()
        {
            if (treeViewTableList.InvokeRequired)
            {
                treeViewTableList.Invoke(new Action(UpdateTreeViewWithRecordCounts));
                return;
            }
            
            foreach (TreeNode categoryNode in treeViewTableList.Nodes)
            {
                foreach (TreeNode entityNode in categoryNode.Nodes)
                {
                    if (entityNode.Tag is Type entityType)
                    {
                        var metadata = EntityRegistry.Entities.FirstOrDefault(e => e.EntityType == entityType);
                        if (metadata != null && _tableRecordCounts != null)
                        {
                            long recordCount = _tableRecordCounts.ContainsKey(metadata.TableName) 
                                ? _tableRecordCounts[metadata.TableName] 
                                : 0;
                            
                            // 更新节点文本，添加记录数
                            string displayText = $"{metadata.DisplayName} ({recordCount:N0})";
                            entityNode.Text = displayText;
                            
                            // 将记录数保存到Tag中（使用封装类）
                            entityNode.Tag = new TreeNodeData
                            {
                                EntityType = entityType,
                                RecordCount = recordCount,
                                TableName = metadata.TableName
                            };
                        }
                    }
                }
            }
        }

        /// <summary>
        /// TreeView节点数据封装类
        /// </summary>
        private class TreeNodeData
        {
            public Type EntityType { get; set; }
            public long RecordCount { get; set; }
            public string TableName { get; set; }
        }

        /// <summary>
        /// TreeView 节点选中事件（保留用于单击选择）
        /// </summary>
        private void TreeViewTableList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_isInitializing)
                return;
            
            // 获取选中的节点
            TreeNode selectedNode = treeViewTableList.SelectedNode;
            if (selectedNode == null || selectedNode.Tag == null)
                return;
            
            // 如果是分类节点，不处理
            if (selectedNode.Nodes.Count > 0)
                return;
            
            // 选中节点（设置 CheckBox 状态）
            selectedNode.Checked = true;
            
            // 触发 AfterCheck 事件处理逻辑
            treeViewTableList_AfterCheck(sender, e);
        }

        /// <summary>
        /// 删除操作类型枚举
        /// </summary>
        public enum DeleteOperation
        {
            /// <summary>
            /// 按选中行删除（使用级联删除引擎）
            /// </summary>
            CascadeDelete,
            
            /// <summary>
            /// 整表清空（使用TRUNCATE或DELETE）
            /// </summary>
            TruncateTable
        }

        /// <summary>
        /// 初始化删除方式选择下拉框
        /// </summary>
        private void InitializeDeleteMode()
        {
            kcmbDeleteMode.Items.Clear();
            // 按选中行删除（级联）- 未选中时默认删除全部
            kcmbDeleteMode.Items.Add(new ComboBoxItem("按选中行删除（未选中则全删）", DeleteOperation.CascadeDelete));
            // 整表清空模式
            kcmbDeleteMode.Items.Add(new ComboBoxItem("整表清空（TRUNCATE）", DeleteOperation.TruncateTable));
            kcmbDeleteMode.DisplayMember = "DisplayText";
            kcmbDeleteMode.ValueMember = "Value";
            kcmbDeleteMode.SelectedIndex = 0;
        }

        /// <summary>
        /// 下拉框项封装类
        /// </summary>
        private class ComboBoxItem
        {
            public string DisplayText { get; }
            public DeleteOperation Value { get; }
            
            public ComboBoxItem(string displayText, DeleteOperation value)
            {
                DisplayText = displayText;
                Value = value;
            }
            
            public override string ToString() => DisplayText;
        }

        /// <summary>
        /// 根据搜索关键词过滤实体类型
        /// </summary>
        /// <param name="searchText">搜索关键词</param>
        private void FilterEntityTypes(string searchText)
        {
            _isInitializing = true;
            kcmbEntityType.Items.Clear();
            kcmbEntityType.Items.Add("请选择");
            
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // 无搜索条件时显示全部
                var groupedEntities = EntityRegistry.GetAllGrouped();
                foreach (var group in groupedEntities)
                {
                    kcmbEntityType.Items.Add($"--- {group.Key} ---");
                    foreach (var entity in group.Value)
                    {
                        kcmbEntityType.Items.Add(entity.DisplayName);
                    }
                }
            }
            else
            {
                // 有搜索条件时只显示匹配的实体（不显示分组标题）
                var keywords = searchText.Trim().Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                
                var matchedEntities = _allEntities.Where(e =>
                {
                    // 匹配 DisplayName、Description、TableName、Category
                    string searchTextLower = searchText.ToLower();
                    return e.DisplayName.ToLower().Contains(searchTextLower) ||
                           e.Description.ToLower().Contains(searchTextLower) ||
                           e.TableName.ToLower().Contains(searchTextLower) ||
                           e.Category.ToLower().Contains(searchTextLower);
                })
                .OrderBy(e => e.Category)
                .ThenBy(e => e.DisplayName)
                .ToList();
                
                // 按分类显示匹配的实体
                string currentCategory = "";
                foreach (var entity in matchedEntities)
                {
                    if (currentCategory != entity.Category)
                    {
                        kcmbEntityType.Items.Add($"--- {entity.Category} ---");
                        currentCategory = entity.Category;
                    }
                    kcmbEntityType.Items.Add(entity.DisplayName);
                }
                
                if (matchedEntities.Count == 0)
                {
                    kcmbEntityType.Items.Add($"未找到匹配 \"{searchText}\" 的实体");
                }
            }
            
            kcmbEntityType.SelectedIndex = 0;
            _isInitializing = false;
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            treeViewTableList.AfterSelect += TreeViewTableList_AfterSelect;
            kbtnRefresh.Click += KbtnRefresh_Click;
            kbtnSelectAll.Click += KbtnSelectAll_Click;
            kbtnSelectNone.Click += KbtnSelectNone_Click;
            kbtnSelectInvert.Click += KbtnSelectInvert_Click;
            kbtnDeleteSelected.Click += KbtnDeleteSelected_Click;
            kbtnPreview.Click += KbtnPreview_Click; // 添加预览按钮事件
            kbtnTestExecute.Click += KbtnTestExecute_Click; // 添加测试执行按钮事件
        }

        /// <summary>
        /// 实体类型选择改变事件
        /// </summary>
        private void KcmbEntityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 防止初始化时触发
            if (_isInitializing)
                return;
            
            try
            {
                if (kcmbEntityType.SelectedIndex <= 0)
                {
                    _selectedEntityType = null;
                    dgvDataPreview.Rows.Clear();
                    return;
                }

                string selectedText = kcmbEntityType.SelectedItem.ToString();
                
                // 跳过分组标题和提示信息
                if (selectedText.StartsWith("---") || selectedText.StartsWith("未找到"))
                {
                    kcmbEntityType.SelectedIndex = 0;
                    return;
                }
                
                // 从 EntityRegistry 查找实体
                var metadata = EntityRegistry.GetByDisplayName(selectedText);
                if (metadata != null)
                {
                    _selectedEntityType = metadata.EntityType;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择实体类型失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新数据按钮点击事件
        /// </summary>
        private async void KbtnRefresh_Click(object sender, EventArgs e)
        {
            if (_isBusy)
            {
                // 静默返回,不弹出提示框打扰用户
                return;
            }

            if (_selectedEntityType == null)
            {
                MessageBox.Show("请先选择要清理的数据表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _isBusy = true;
                kbtnRefresh.Enabled = false;
                kbtnDeleteSelected.Enabled = false;
                MainForm.Instance.ShowStatusText("正在查询数据...");

                // ✅ 优化：刷新数据时同步更新当前表的记录数缓存
                await RefreshTableRecordCountAsync(_selectedEntityType);

                await LoadDataAsync();

                MainForm.Instance.ShowStatusText($"查询完成，共 {dgvDataPreview.Rows.Count} 条记录");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.Instance.ShowStatusText("查询失败");
            }
            finally
            {
                _isBusy = false;
                kbtnRefresh.Enabled = true;
                kbtnDeleteSelected.Enabled = true;
            }
        }

        /// <summary>
        /// 加载数据到表格
        /// </summary>
        private async Task LoadDataAsync()
        {
            // 清空数据和列(保留 CheckBox 列)
            dgvDataPreview.Rows.Clear();
            
            // 移除除 CheckBox 列外的所有列
            var columnsToRemove = dgvDataPreview.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Name != "colSelect")
                .ToList();
            
            foreach (var column in columnsToRemove)
            {
                dgvDataPreview.Columns.Remove(column);
            }

            // 根据实体类型查询数据
            var records = await QueryDataByTypeAsync(_selectedEntityType);

            // 获取实体属性(限制显示列数)
            var properties = _selectedEntityType.GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .Take(20)
                .ToList();

            // 动态添加列(优先使用 AdvQueryAttribute 的 ColDesc 作为标题)
            foreach (var prop in properties)
            {
                // 尝试获取 AdvQueryAttribute 特性
                var advQueryAttr = prop.GetCustomAttribute<RUINORERP.Global.CustomAttribute.AdvQueryAttribute>();
                string headerText = advQueryAttr?.ColDesc ?? prop.Name; // 如果有 ColDesc 则使用,否则使用属性名

                var column = new DataGridViewTextBoxColumn
                {
                    Name = $"col_{prop.Name}",
                    HeaderText = headerText,
                    DataPropertyName = prop.Name,
                    Width = 120,
                    ReadOnly = true // 数据列设置为只读，只有 CheckBox 列可编辑
                };
                dgvDataPreview.Columns.Add(column);
            }

            // 添加数据行
            foreach (var record in records)
            {
                int rowIndex = dgvDataPreview.Rows.Add();
                DataGridViewRow row = dgvDataPreview.Rows[rowIndex];
                row.Tag = record; // 保存实体对象引用

                // 填充数据
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(record);
                    var cellName = $"col_{prop.Name}";
                    if (dgvDataPreview.Columns.Contains(cellName))
                    {
                        row.Cells[cellName].Value = value?.ToString() ?? "";
                    }
                }
            }

            UpdateSelectedCount();
        }

        /// <summary>
        /// 刷新单个表的记录数缓存并更新TreeView显示
        /// </summary>
        private async Task RefreshTableRecordCountAsync(Type entityType)
        {
            try
            {
                var metadata = EntityRegistry.Entities.FirstOrDefault(e => e.EntityType == entityType);
                if (metadata == null)
                    return;
                
                var tableName = metadata.TableName;
                var db = MainForm.Instance.AppContext.Db;
                
                // 查询最新的记录数
                var count = await db.Queryable<object>()
                    .AS(tableName)
                    .CountAsync();
                
                // 更新缓存
                if (_tableRecordCounts == null)
                    _tableRecordCounts = new Dictionary<string, long>();
                
                _tableRecordCounts[tableName] = count;
                
                AppendRealTimeLog($"[缓存更新] 表 {tableName} 记录数: {count:N0}");
                
                // 更新TreeView中对应节点的显示
                UpdateTreeNodeRecordCount(entityType, count);
            }
            catch (Exception ex)
            {
                AppendRealTimeLog($"[警告] 更新表记录数缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新TreeView中指定实体类型的节点记录数显示
        /// </summary>
        private void UpdateTreeNodeRecordCount(Type entityType, long recordCount)
        {
            if (treeViewTableList.InvokeRequired)
            {
                treeViewTableList.Invoke(new Action<Type, long>(UpdateTreeNodeRecordCount), entityType, recordCount);
                return;
            }
            
            foreach (TreeNode categoryNode in treeViewTableList.Nodes)
            {
                foreach (TreeNode entityNode in categoryNode.Nodes)
                {
                    // 查找匹配的节点
                    bool isMatch = false;
                    if (entityNode.Tag is Type type && type == entityType)
                        isMatch = true;
                    else if (entityNode.Tag is TreeNodeData nodeData && nodeData.EntityType == entityType)
                        isMatch = true;
                    
                    if (isMatch)
                    {
                        var metadata = EntityRegistry.Entities.FirstOrDefault(e => e.EntityType == entityType);
                        if (metadata != null)
                        {
                            // 更新节点文本
                            entityNode.Text = $"{metadata.DisplayName} ({recordCount:N0})";
                            
                            // 更新Tag中的数据
                            entityNode.Tag = new TreeNodeData
                            {
                                EntityType = entityType,
                                RecordCount = recordCount,
                                TableName = metadata.TableName
                            };
                            
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据实体类型查询数据(使用 EntityRegistry 统一查询)
        /// </summary>
        private async Task<List<object>> QueryDataByTypeAsync(Type entityType)
        {
            var db = MainForm.Instance.AppContext.Db;
            return await EntityRegistry.QueryAsync(entityType, db);
        }

        /// <summary>
        /// 全选按钮点击事件
        /// </summary>
        private void KbtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvDataPreview.Rows)
            {
                if (row.Cells["colSelect"] is DataGridViewCheckBoxCell cell)
                {
                    cell.Value = true;
                }
            }
            UpdateSelectedCount();
        }

        /// <summary>
        /// 取消选择按钮点击事件
        /// </summary>
        private void KbtnSelectNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvDataPreview.Rows)
            {
                if (row.Cells["colSelect"] is DataGridViewCheckBoxCell cell)
                {
                    cell.Value = false;
                }
            }
            UpdateSelectedCount();
        }

        /// <summary>
        /// 反选按钮点击事件
        /// </summary>
        private void KbtnSelectInvert_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvDataPreview.Rows)
            {
                if (row.Cells["colSelect"] is DataGridViewCheckBoxCell cell)
                {
                    // 获取当前状态
                    bool currentValue = cell.Value != null && Convert.ToBoolean(cell.Value);
                    // 取反
                    cell.Value = !currentValue;
                }
            }
            UpdateSelectedCount();
        }

        /// <summary>
        /// 删除选中按钮点击事件
        /// </summary>
        private async void KbtnDeleteSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isBusy)
                {
                    return;
                }

                // 获取所有选中的实体类型
                var selectedTypes = GetSelectedEntityTypes();
                
                if (selectedTypes.Count == 0)
                {
                    MessageBox.Show("请先选择要清理的数据表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 获取删除操作类型
                DeleteOperation deleteOp = DeleteOperation.CascadeDelete;
                if (kcmbDeleteMode.SelectedItem is ComboBoxItem selectedItem)
                {
                    deleteOp = selectedItem.Value;
                }

                if (selectedTypes.Count == 1)
                {
                    // 单选：按选中行删除或整表删除
                    _selectedEntityType = selectedTypes[0];
                    
                    if (deleteOp == DeleteOperation.TruncateTable)
                    {
                        await ExecuteTruncateTableAsync();
                    }
                    else
                    {
                        await ExecuteCascadeDeleteWithSelectionAsync();
                    }
                }
                else
                {
                    // 多选：批量删除整个表
                    await ExecuteBatchDeleteTablesAsync(selectedTypes, deleteOp);
                }
            }
            catch (OperationCanceledException)
            {
                AppendRealTimeLog("[取消] 操作已被用户取消");
                MainForm.Instance.ShowStatusText("操作已取消");
                MessageBox.Show("操作已取消", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendRealTimeLog($"[异常] 删除操作异常: {ex.Message}");
                MessageBox.Show($"删除失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isBusy = false;
                kbtnDeleteSelected.Enabled = true;
                kbtnRefresh.Enabled = true;
                kbtnCancel.Enabled = false;
                progressBar.Visible = false; // 隐藏进度条
                progressBar.Value = 0;
                _currentCancellationTokenSource?.Dispose();
                _currentCancellationTokenSource = null;
            }
        }

        /// <summary>
        /// 批量删除多个表（多选模式）
        /// </summary>
        private async Task ExecuteBatchDeleteTablesAsync(List<Type> entityTypes, DeleteOperation deleteOp)
        {
            // 构建确认消息
            var tableNames = entityTypes
                .Select(t => EntityRegistry.Entities.FirstOrDefault(e => e.EntityType == t)?.TableName ?? t.Name)
                .ToList();
            
            var tableListStr = string.Join("\n  • ", tableNames);
            var confirmMsg = $"确定要删除以下 {entityTypes.Count} 个数据表吗？\n\n  • {tableListStr}\n\n⚠️ 此操作不可恢复！\n• 所有相关数据将被删除";
            
            if (MessageBox.Show(confirmMsg, "确认批量删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                AppendRealTimeLog("用户取消批量删除操作");
                return;
            }

            _isBusy = true;
            kbtnDeleteSelected.Enabled = false;
            kbtnRefresh.Enabled = false;
            kbtnCancel.Enabled = true; // 启用取消按钮
            progressBar.Visible = true; // 显示进度条
            progressBar.Value = 0;
            
            // 创建取消令牌
            _currentCancellationTokenSource = _cleanupEngine.CreateCancellationTokenSource();
            
            AppendRealTimeLog($"========== 开始批量删除 {entityTypes.Count} 个表 ==========");

            try
            {
                foreach (var entityType in entityTypes)
                {
                    var metadata = EntityRegistry.Entities.FirstOrDefault(e => e.EntityType == entityType);
                    var tableName = metadata?.TableName ?? entityType.Name;
                    
                    AppendRealTimeLog($"正在删除表: {tableName}...");
                    
                    var db = MainForm.Instance.AppContext.Db;
                    
                    if (deleteOp == DeleteOperation.TruncateTable)
                    {
                        // TRUNCATE 删除（使用原生SQL）
                        var truncateSql = $"TRUNCATE TABLE [{tableName}]";
                        AppendRealTimeLog($"  [SQL] {truncateSql}");
                        await db.Ado.ExecuteCommandAsync(truncateSql);
                        AppendRealTimeLog($"  ✓ 表 {tableName} 已清空");
                    }
                    else
                    {
                        // 级联删除（删除所有数据，使用原生SQL）
                        var deleteSql = $"DELETE FROM [{tableName}]";
                        AppendRealTimeLog($"  [SQL] {deleteSql}");
                        var deletedCount = await db.Ado.ExecuteCommandAsync(deleteSql);
                        AppendRealTimeLog($"  ✓ 表 {tableName} 已删除 {deletedCount} 条记录");
                    }
                }
                
                AppendRealTimeLog($"========== 批量删除完成 ==========");
                MessageBox.Show($"批量删除完成，共删除 {entityTypes.Count} 个表", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // ✅ 优化：批量删除后刷新所有涉及的表的记录数缓存
                foreach (var entityType in entityTypes)
                {
                    await RefreshTableRecordCountAsync(entityType);
                }
                
                // 清空选择状态
                ClearTreeViewSelection();
            }
            catch (Exception ex)
            {
                AppendRealTimeLog($"[异常] 批量删除异常: {ex.Message}");
                MessageBox.Show($"批量删除失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isBusy = false;
                kbtnDeleteSelected.Enabled = true;
                kbtnRefresh.Enabled = true;
            }
        }

        /// <summary>
        /// 清空 TreeView 选择状态
        /// </summary>
        private void ClearTreeViewSelection()
        {
            _isInitializing = true;
            foreach (TreeNode categoryNode in treeViewTableList.Nodes)
            {
                foreach (TreeNode entityNode in categoryNode.Nodes)
                {
                    entityNode.Checked = false;
                }
            }
            _isInitializing = false;
            _selectedEntityType = null;
            kbtnDeleteSelected.Enabled = false;
            kbtnPreview.Enabled = false;
        }

        /// <summary>
        /// 执行整表清空（TRUNCATE/DELETE）
        /// </summary>
        private async Task ExecuteTruncateTableAsync()
        {
            // ✅ 从元数据中获取表名
            var metadata = ModelMetadataHelper.GetMetadata(_selectedEntityType);
            var tableName = metadata.TableName;
            
            // 确认对话框
            var confirmMsg = $"确定要清空表 [{tableName}] 的所有数据吗？\n\n⚠️ 此操作不可恢复！\n• TRUNCATE会重置自增ID\n• 不会触发DELETE触发器\n• 如果存在外键引用，将自动级联TRUNCATE相关表\n\n建议先备份重要数据。";
            
            if (MessageBox.Show(confirmMsg, "确认清空整表", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                AppendRealTimeLog("用户取消清空操作");
                return;
            }

            _isBusy = true;
            kbtnDeleteSelected.Enabled = false;
            kbtnRefresh.Enabled = false;
            kbtnCancel.Enabled = true; // 启用取消按钮
            progressBar.Visible = true; // 显示进度条
            progressBar.Value = 0;
            
            // 创建取消令牌
            _currentCancellationTokenSource = _cleanupEngine.CreateCancellationTokenSource();
            
            AppendRealTimeLog($"========== 开始清空表: {tableName} ==========");

            try
            {
                var db = _cleanupEngine.GetDbClient();
                
                // ⚠️ 关键修复：检查是否有外键约束引用该表
                var hasForeignKey = await CheckHasForeignKeys(tableName);
                
                if (hasForeignKey)
                {
                    AppendRealTimeLog($"[警告] 表 {tableName} 存在外键约束，将采用级联TRUNCATE策略:");
                    AppendRealTimeLog($"  1. 查询所有引用该表的子表");
                    AppendRealTimeLog($"  2. 按依赖顺序TRUNCATE子表（从叶子节点到父节点）");
                    AppendRealTimeLog($"  3. 最后TRUNCATE目标表");
                    
                    await ExecuteCascadeTruncate(db, tableName);
                }
                else
                {
                    AppendRealTimeLog($"[TRUNCATE] 开始清空表: {tableName}");
                    await ExecuteTruncate(tableName);
                }
                
                AppendRealTimeLog($"[成功] 表 {tableName} 已清空");
                MessageBox.Show($"表 [{tableName}] 已清空！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // ✅ 优化：TRUNCATE后刷新当前表的记录数缓存
                await RefreshTableRecordCountAsync(_selectedEntityType);
                
                // 重新加载数据
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                AppendRealTimeLog($"[错误] 清空表失败：{ex.Message}");
                MessageBox.Show($"清空表失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 检查表是否有外键约束
        /// </summary>
        private async Task<bool> CheckHasForeignKeys(string tableName)
        {
            try
            {
                var db = _cleanupEngine.GetDbClient();
                var sql = @"
                    SELECT COUNT(*) 
                    FROM sys.foreign_keys 
                    WHERE referenced_object_id = OBJECT_ID(@tableName)
                       OR parent_object_id = OBJECT_ID(@tableName)";

                var count = await db.Ado.GetIntAsync(sql, new { tableName = tableName });
                return count > 0;
            }
            catch
            {
                // 如果查询失败，保守起见返回true，使用DELETE
                return true;
            }
        }

        /// <summary>
        /// 执行级联 TRUNCATE（智能处理外键依赖）
        /// 策略：先TRUNCATE所有子表（被引用的表），最后TRUNCATE目标表
        /// </summary>
        private async Task ExecuteCascadeTruncate(ISqlSugarClient db, string targetTableName)
        {
            AppendRealTimeLog($"\n========== [级联TRUNCATE] 开始分析 ==========");
            
            try
            {
                // 步骤1：查询所有引用目标表的外键关系
                AppendRealTimeLog($"[步骤1] 查询引用表 [{targetTableName}] 的所有外键...");
                
                var foreignKeys = await db.Ado.SqlQueryAsync<DbForeignKeyInfo>(@"
                    SELECT 
                        fk.name AS ForeignKeyName,
                        tp.name AS ReferencingTableName,
                        cp.name AS ReferencingColumnName,
                        tr.name AS ReferencedTableName,
                        cr.name AS ReferencedColumnName
                    FROM sys.foreign_keys AS fk
                    INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
                    INNER JOIN sys.tables AS tp ON fkc.parent_object_id = tp.object_id
                    INNER JOIN sys.columns AS cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
                    INNER JOIN sys.tables AS tr ON fkc.referenced_object_id = tr.object_id
                    INNER JOIN sys.columns AS cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
                    WHERE tr.name = @tableName", 
                    new { tableName = targetTableName });
                
                AppendRealTimeLog($"[步骤1] 找到 {foreignKeys.Count} 个外键引用");
                
                if (foreignKeys.Count == 0)
                {
                    AppendRealTimeLog($"[步骤1] 无外键引用，直接TRUNCATE目标表");
                    await ExecuteTruncate(targetTableName);
                    return;
                }
                
                foreach (var fk in foreignKeys)
                {
                    AppendRealTimeLog($"  → {fk.ReferencingTableName}.{fk.ReferencingColumnName} 引用 {targetTableName}");
                }
                
                // 步骤2：递归收集所有需要TRUNCATE的表（包括多层级子表）
                AppendRealTimeLog($"\n[步骤2] 递归收集所有依赖表...");
                var tablesToTruncate = new List<string>();
                var visitedTables = new HashSet<string>();
                
                await CollectDependentTablesRecursiveAsync(db, targetTableName, tablesToTruncate, visitedTables, 0);
                
                AppendRealTimeLog($"[步骤2] 共需TRUNCATE {tablesToTruncate.Count} 个表");
                for (int i = 0; i < tablesToTruncate.Count; i++)
                {
                    AppendRealTimeLog($"  顺序 {i + 1}: {tablesToTruncate[i]}");
                }
                
                // 步骤3：按依赖顺序执行TRUNCATE（从叶子节点到根节点）
                AppendRealTimeLog($"\n[步骤3] 开始执行TRUNCATE...");
                int successCount = 0;
                
                for (int i = 0; i < tablesToTruncate.Count; i++)
                {
                    string tableName = tablesToTruncate[i];
                    AppendRealTimeLog($"\n  [{i + 1}/{tablesToTruncate.Count}] TRUNCATE [{tableName}]...");
                    
                    try
                    {
                        await ExecuteTruncate(tableName);
                        successCount++;
                        AppendRealTimeLog($"  ✓ [{tableName}] 已清空");
                    }
                    catch (Exception ex)
                    {
                        AppendRealTimeLog($"  ✗ [{tableName}] TRUNCATE失败: {ex.Message}");
                        AppendRealTimeLog($"  → 尝试使用 DELETE FROM 降级处理...");
                        
                        // 降级方案：使用 DELETE FROM
                        await ExecuteDeleteAll(tableName);
                        successCount++;
                        AppendRealTimeLog($"  ✓ [{tableName}] 已通过DELETE清空");
                    }
                }
                
                AppendRealTimeLog($"\n========== [级联TRUNCATE] 完成 ==========");
                AppendRealTimeLog($"成功清空 {successCount}/{tablesToTruncate.Count} 个表");
            }
            catch (Exception ex)
            {
                AppendRealTimeLog($"[错误] 级联TRUNCATE失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 递归收集所有依赖表（子表及其子表）
        /// 返回顺序：先子表后父表（拓扑排序）
        /// </summary>
        private async Task CollectDependentTablesRecursiveAsync(
            ISqlSugarClient db,
            string tableName,
            List<string> result,
            HashSet<string> visited,
            int depth)
        {
            // 防止循环引用和过深递归
            if (visited.Contains(tableName) || depth > 10)
            {
                return;
            }
            
            visited.Add(tableName);
            
            // 查询引用当前表的所有外键（子表）
            var foreignKeys = await db.Ado.SqlQueryAsync<DbForeignKeyInfo>(@"
                SELECT 
                    fk.name AS ForeignKeyName,
                    tp.name AS ReferencingTableName,
                    cp.name AS ReferencingColumnName,
                    tr.name AS ReferencedTableName,
                    cr.name AS ReferencedColumnName
                FROM sys.foreign_keys AS fk
                INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
                INNER JOIN sys.tables AS tp ON fkc.parent_object_id = tp.object_id
                INNER JOIN sys.columns AS cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
                INNER JOIN sys.tables AS tr ON fkc.referenced_object_id = tr.object_id
                INNER JOIN sys.columns AS cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
                WHERE tr.name = @tableName", 
                new { tableName });
            
            // 先递归处理所有子表
            foreach (var fk in foreignKeys)
            {
                string childTable = fk.ReferencingTableName;
                if (!visited.Contains(childTable))
                {
                    await CollectDependentTablesRecursiveAsync(db, childTable, result, visited, depth + 1);
                }
            }
            
            // 最后添加当前表（保证子表在前，父表在后）
            result.Add(tableName);
        }

        /// <summary>
        /// 执行TRUNCATE TABLE（带外键约束处理）
        /// </summary>
        private async Task ExecuteTruncateWithDisabledConstraints(ISqlSugarClient db, string tableName)
        {
            try
            {
                // 步骤1：禁用所有外键约束
                AppendRealTimeLog($"[步骤1] 禁用所有外键约束...");
                await db.Ado.ExecuteCommandAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
                AppendRealTimeLog($"[步骤1] ✓ 外键约束已禁用");
                
                // 步骤2：执行 TRUNCATE
                AppendRealTimeLog($"[步骤2] 执行 TRUNCATE TABLE [{tableName}]...");
                var sql = $"TRUNCATE TABLE [{tableName}]";
                await db.Ado.ExecuteCommandAsync(sql);
                AppendRealTimeLog($"[步骤2] ✓ TRUNCATE 执行成功");
                
                // 步骤3：重新启用所有外键约束
                AppendRealTimeLog($"[步骤3] 重新启用所有外键约束...");
                await db.Ado.ExecuteCommandAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");
                AppendRealTimeLog($"[步骤3] ✓ 外键约束已重新启用");
            }
            catch (Exception ex)
            {
                AppendRealTimeLog($"[错误] TRUNCATE 过程中出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 执行TRUNCATE TABLE
        /// </summary>
        private async Task ExecuteTruncate(string tableName)
        {
            var db = _cleanupEngine.GetDbClient();
            var sql = $"TRUNCATE TABLE [{tableName}]";
            
            AppendRealTimeLog($"[SQL] {sql}");
            await db.Ado.ExecuteCommandAsync(sql);
        }

        /// <summary>
        /// 执行DELETE FROM（无WHERE条件）
        /// </summary>
        private async Task ExecuteDeleteAll(string tableName)
        {
            var db = _cleanupEngine.GetDbClient();
            var sql = $"DELETE FROM [{tableName}]";
            
            AppendRealTimeLog($"[SQL] {sql}");
            var deletedCount = await db.Ado.ExecuteCommandAsync(sql);
            
            AppendRealTimeLog($"[成功] 影响 {deletedCount} 条记录");
        }

        /// <summary>
        /// 执行级联删除（需要用户先选择行）
        /// </summary>
        private async Task ExecuteCascadeDeleteWithSelectionAsync()
        {
            // 获取选中的ID
            _selectedIds = GetSelectedRecordIds();
            
            // ⚠️ 关键修复：如果没有选中任何行，默认获取所有记录的ID
            if (_selectedIds.Count == 0)
            {
                AppendRealTimeLog("未选中任何行，将获取表中所有记录ID进行删除...");
                _selectedIds = await GetAllRecordIdsAsync();
                
                if (_selectedIds.Count == 0)
                {
                    MessageBox.Show("表中没有数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                AppendRealTimeLog($"获取到表中所有 {_selectedIds.Count} 条记录ID");
            }
            else
            {
                AppendRealTimeLog($"准备删除，选中记录数: {_selectedIds.Count}");
            }

            // 显示确认对话框
            var confirmMsg = $"确定要删除 {_selectedIds.Count} 条记录吗？\n\n此操作将同时删除所有关联数据(通过数据库元数据自动级联)！\n\n建议先备份重要数据。";

            if (MessageBox.Show(confirmMsg, "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                AppendRealTimeLog("用户取消删除操作");
                return;
            }

            _isBusy = true;
            kbtnDeleteSelected.Enabled = false;
            kbtnRefresh.Enabled = false;
            kbtnCancel.Enabled = true; // 启用取消按钮
            progressBar.Visible = true; // 显示进度条
            progressBar.Value = 0;
            
            // 创建取消令牌
            _currentCancellationTokenSource = _cleanupEngine.CreateCancellationTokenSource();
            
            AppendRealTimeLog($"开始删除 {_selectedEntityType.Name} 表的 {_selectedIds.Count} 条记录...");

            // 使用 DataCleanupEngine 执行级联删除
            var result = await ExecuteCascadeDeleteAsync(_selectedIds, isTestMode: false, _currentCancellationTokenSource.Token);

            if (result.IsSuccess)
            {
                AppendRealTimeLog($"删除成功！共删除 {result.TotalDeletedCount} 条主记录及其关联数据，耗时: {result.TotalElapsedMs}ms");
                MessageBox.Show($"删除完成！\n共删除 {result.TotalDeletedCount} 条主记录及其关联数据\n耗时: {result.TotalElapsedMs}ms", 
                    "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                MainForm.Instance.ShowStatusText("删除完成");

                // ✅ 优化：删除后刷新当前表的记录数缓存
                await RefreshTableRecordCountAsync(_selectedEntityType);
                
                // 重新加载数据
                await LoadDataAsync();
            }
            else
            {
                AppendRealTimeLog($"[错误] 删除失败：{result.ErrorMessage}");
                MessageBox.Show($"删除失败：{result.ErrorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行级联删除(使用 DataCleanupEngine)
        /// </summary>
        /// <param name="ids">要删除的ID列表，如果为null则使用 _selectedIds</param>
        /// <param name="isTestMode">是否测试模式</param>
        /// <param name="cancellationToken">取消令牌</param>
        private async Task<CascadeDeleteResult> ExecuteCascadeDeleteAsync(List<long> ids = null, bool isTestMode = false, CancellationToken cancellationToken = default)
        {
            var targetIds = ids ?? _selectedIds;
            
            AppendRealTimeLog($"========== 开始级联删除 ==========");
            AppendRealTimeLog($"实体类型: {_selectedEntityType?.Name}, ID数量: {targetIds.Count}, 测试模式: {isTestMode}");
            
            // 使用反射调用泛型方法 ExecuteCascadeDeleteAsync<T>（无 DeleteMode 参数）
            var methodTypes = new[] { typeof(List<long>), typeof(bool), typeof(CancellationToken) };
            var method = typeof(DataCleanupEngine).GetMethod("ExecuteCascadeDeleteAsync", methodTypes);
            
            if (method == null)
            {
                throw new InvalidOperationException("未找到 ExecuteCascadeDeleteAsync 方法");
            }
            
            var genericMethod = method.MakeGenericMethod(_selectedEntityType);
            
            AppendRealTimeLog("正在构建依赖图和拓扑排序...");
            var task = (Task<CascadeDeleteResult>)genericMethod.Invoke(_cleanupEngine, new object[] { targetIds, isTestMode, cancellationToken });
            
            AppendRealTimeLog("等待执行结果...");
            var result = await task;
            
            AppendRealTimeLog($"========== 执行完成 ==========");
            AppendRealTimeLog($"结果: IsSuccess={result.IsSuccess}, TotalDeletedCount={result.TotalDeletedCount}");
            
            if (!result.IsSuccess)
            {
                AppendRealTimeLog($"错误信息: {result.ErrorMessage}");
            }
            
            return result;
        }

        /// <summary>
        /// 获取选中的记录 ID 列表
        /// </summary>
        private List<long> GetSelectedRecordIds()
        {
            var selectedIds = new List<long>();
            var pkName = GetPrimaryKeyName(_selectedEntityType);
            
            AppendRealTimeLog($"开始获取选中记录ID，主键字段: {pkName}，总行数: {dgvDataPreview.Rows.Count}");

            foreach (DataGridViewRow row in dgvDataPreview.Rows)
            {
                if (row.Cells["colSelect"] is DataGridViewCheckBoxCell cell)
                {
                    bool isChecked = cell.Value != null && Convert.ToBoolean(cell.Value);
                    
                    if (isChecked)
                    {
                        var record = row.Tag;
                        if (record != null)
                        {
                            var pkProp = _selectedEntityType.GetProperty(pkName);
                            if (pkProp != null)
                            {
                                var idValue = pkProp.GetValue(record);
                                AppendRealTimeLog($"  找到选中行，ID值: {idValue}");
                                
                                if (long.TryParse(idValue?.ToString(), out long id))
                                {
                                    selectedIds.Add(id);
                                }
                                else
                                {
                                    AppendRealTimeLog($"  [错误] ID转换失败: {idValue}");
                                }
                            }
                            else
                            {
                                AppendRealTimeLog($"  [错误] 未找到主键属性: {pkName}");
                            }
                        }
                        else
                        {
                            AppendRealTimeLog("  [错误] 行Tag为null");
                        }
                    }
                }
            }

            AppendRealTimeLog($"最终获取到 {selectedIds.Count} 个选中ID");
            return selectedIds;
        }

        /// <summary>
        /// 获取表中所有记录的主键ID
        /// </summary>
        private async Task<List<long>> GetAllRecordIdsAsync()
        {
            var allIds = new List<long>();
            var pkName = GetPrimaryKeyName(_selectedEntityType);
            var metadata = ModelMetadataHelper.GetMetadata(_selectedEntityType);
            var tableName = metadata.TableName;
            
            AppendRealTimeLog($"开始查询表 [{tableName}] 的所有记录ID，主键字段: {pkName}");

            try
            {
                var db = MainForm.Instance.AppContext.Db;
                
                // 直接查询所有主键ID
                var sql = $"SELECT [{pkName}] FROM [{tableName}]";
                var ids = await db.Ado.SqlQueryAsync<long>(sql);
                
                allIds = ids.ToList();
                
                AppendRealTimeLog($"查询完成，共 {allIds.Count} 条记录");
            }
            catch (Exception ex)
            {
                AppendRealTimeLog($"[错误] 查询所有记录ID失败: {ex.Message}");
                throw;
            }

            return allIds;
        }

        /// <summary>
        /// 追加实时日志到 UI
        /// </summary>
        private void AppendRealTimeLog(string message)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action<string>(AppendRealTimeLog), message);
            }
            else
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                rtbLog.AppendText($"[{timestamp}] {message}\r\n");
                rtbLog.ScrollToCaret(); // 自动滚动到底部
            }
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        private void UpdateProgressBar(int percentage)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<int>(UpdateProgressBar), percentage);
            }
            else
            {
                if (percentage > 0 && percentage <= 100)
                {
                    progressBar.Visible = true;
                    progressBar.Value = percentage;
                }
                else
                {
                    progressBar.Visible = false;
                }
            }
        }

        /// <summary>
        /// 获取主键名称
        /// </summary>
        private string GetPrimaryKeyName(Type entityType)
        {
            var sugarProps = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute<SqlSugar.SugarColumn>()?.IsPrimaryKey == true);

            if (sugarProps.Any())
            {
                return sugarProps.First().Name;
            }

            var idProp = entityType.GetProperty("Id") ?? entityType.GetProperty("ID");
            return idProp?.Name ?? "Id";
        }



        /// <summary>
        /// 更新选中计数
        /// </summary>
        private void UpdateSelectedCount()
        {
            int count = 0;
            foreach (DataGridViewRow row in dgvDataPreview.Rows)
            {
                if (row.Cells["colSelect"] is DataGridViewCheckBoxCell cell &&
                    cell.Value != null && (bool)cell.Value)
                {
                    count++;
                }
            }

            MainForm.Instance.ShowStatusText($"已选择 {count} 条记录");
        }

        /// <summary>
        /// 清除日志菜单项点击事件
        /// </summary>
        private void TsmiClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
            AppendRealTimeLog("日志已清除");
        }

        #region 移除复杂配置相关的方法（保留空实现防止编译错误）

        private void KcmbConfigName_SelectedIndexChanged(object sender, EventArgs e) { }
        private void KbtnNewConfig_Click(object sender, EventArgs e) { }
        private void KbtnEditConfig_Click(object sender, EventArgs e) { }
        private void KbtnDeleteConfig_Click(object sender, EventArgs e) { }
        private void KbtnSaveConfig_Click(object sender, EventArgs e) { }
        private void KbtnAddRule_Click(object sender, EventArgs e) { }
        private void KbtnEditRule_Click(object sender, EventArgs e) { }
        private void KbtnDeleteRule_Click(object sender, EventArgs e) { }
        private void KbtnMoveUp_Click(object sender, EventArgs e) { }
        private void KbtnMoveDown_Click(object sender, EventArgs e) { }
        
        /// <summary>
        /// 预览按钮点击事件 - 显示将要删除的数据预览
        /// </summary>
        private async void KbtnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedEntityType == null)
                {
                    MessageBox.Show("请先选择要清理的数据表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedIds = GetSelectedRecordIds();
                
                if (selectedIds.Count == 0)
                {
                    MessageBox.Show("请先勾选要预览的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                AppendRealTimeLog($"预览模式：将检查 {selectedIds.Count} 条记录及其关联数据");

                // 创建取消令牌
                _currentCancellationTokenSource = _cleanupEngine.CreateCancellationTokenSource();
                kbtnCancel.Enabled = true;
                progressBar.Visible = true; // 显示进度条
                progressBar.Value = 0;
                
                // 使用测试模式执行，不实际删除
                var result = await ExecuteCascadeDeleteAsync(selectedIds, isTestMode: true, _currentCancellationTokenSource.Token);

                if (result.IsSuccess)
                {
                    string previewMsg = $"【删除预览】\n\n" +
                        $"选中主记录数: {selectedIds.Count}\n" +
                        $"预计删除总数: {result.TotalDeletedCount} 条(含关联数据)\n" +
                        $"实体类型: {_selectedEntityType.Name}\n" +
                        $"\n注意：这只是预览，不会实际删除任何数据。";
                    
                    MessageBox.Show(previewMsg, "删除预览", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AppendRealTimeLog($"预览完成：预计删除 {result.TotalDeletedCount} 条记录");
                }
                else
                {
                    AppendRealTimeLog($"[错误] 预览失败：{result.ErrorMessage}");
                    MessageBox.Show($"预览失败：{result.ErrorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (OperationCanceledException)
            {
                AppendRealTimeLog("[取消] 预览已被用户取消");
                MainForm.Instance.ShowStatusText("预览已取消");
            }
            catch (Exception ex)
            {
                AppendRealTimeLog($"[异常] 预览异常: {ex.Message}");
                MessageBox.Show($"预览失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                kbtnCancel.Enabled = false;
                progressBar.Visible = false; // 隐藏进度条
                progressBar.Value = 0;
                _currentCancellationTokenSource?.Dispose();
                _currentCancellationTokenSource = null;
            }
        }

        /// <summary>
        /// 测试执行按钮点击事件 - 执行但不提交事务
        /// </summary>
        private async void KbtnTestExecute_Click(object sender, EventArgs e)
        {
            List<long> selectedIds = null; // 提升到 try 外部，catch 块才能访问
            
            try
            {
                if (_isBusy)
                {
                    MainForm.Instance.ShowStatusText("操作正在进行中，请稍候...");
                    return;
                }

                if (_selectedEntityType == null)
                {
                    MessageBox.Show("请先选择要清理的数据表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                selectedIds = GetSelectedRecordIds();
                
                if (selectedIds.Count == 0)
                {
                    MessageBox.Show("请先勾选要测试的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmMsg = $"【测试执行模式】\n\n将对 {selectedIds.Count} 条记录执行级联删除测试\n\n此模式会验证删除逻辑但不会真正删除数据。\n\n是否继续？";

                if (MessageBox.Show(confirmMsg, "测试执行确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                _isBusy = true;
                kbtnTestExecute.Enabled = false;
                kbtnCancel.Enabled = true; // 启用取消按钮
                progressBar.Visible = true; // 显示进度条
                progressBar.Value = 0;
                MainForm.Instance.ShowStatusText("正在测试执行...");
                AppendRealTimeLog($"开始测试执行 {_selectedEntityType.Name} 表的 {selectedIds.Count} 条记录...");

                // 创建取消令牌
                _currentCancellationTokenSource = _cleanupEngine.CreateCancellationTokenSource();
                
                // 使用测试模式执行
                var result = await ExecuteCascadeDeleteAsync(selectedIds, isTestMode: true, _currentCancellationTokenSource.Token);

                if (result.IsSuccess)
                {
                    string testResultMsg = $"【测试执行结果】\n\n" +
                        $"✅ 测试成功\n" +
                        $"测试记录数: {selectedIds.Count}\n" +
                        $"预计影响记录: {result.TotalDeletedCount} 条\n" +
                        $"耗时: {result.TotalElapsedMs}ms\n\n" +
                        $"注意：这只是测试，没有实际删除数据。\n" +
                        $"如需正式删除，请使用“正式执行”按钮。";
                    
                    MessageBox.Show(testResultMsg, "测试执行结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AppendRealTimeLog($"测试执行成功：预计删除 {result.TotalDeletedCount} 条记录");
                }
                else
                {
                    MessageBox.Show($"测试执行失败：{result.ErrorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AppendRealTimeLog($"[错误] 测试执行失败：{result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                // 详细错误日志输出
                string errorMsg = $"[异常] 测试执行异常:\n" +
                    $"  • 实体类型：{_selectedEntityType?.FullName ?? "未知"}\n" +
                    $"  • 选中记录数：{selectedIds?.Count ?? 0}\n" +
                    $"  • 异常类型：{ex.GetType().Name}\n" +
                    $"  • 异常消息：{ex.Message}\n" +
                    $"  • 堆栈跟踪：{ex.StackTrace}";
                
                if (ex.InnerException != null)
                {
                    errorMsg += $"\n  • 内部异常：{ex.InnerException.Message}";
                }
                
                AppendRealTimeLog(errorMsg);
                
                // 显示用户友好的错误提示
                string userMsg = $"❌ 测试执行失败!\n\n" +
                    $"错误类型：{ex.GetType().Name}\n" +
                    $"错误信息：{ex.Message}\n\n" +
                    $"可能原因:\n" +
                    $"  1. 数据正在被其他单据引用\n" +
                    $"  2. 数据库连接异常\n\n" +
                    $"详细日志已记录，请联系管理员查看。";
                
                MessageBox.Show(userMsg, "测试执行失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isBusy = false;
                kbtnTestExecute.Enabled = true;
                kbtnCancel.Enabled = false;
                progressBar.Visible = false; // 隐藏进度条
                progressBar.Value = 0;
                _currentCancellationTokenSource?.Dispose();
                _currentCancellationTokenSource = null;
            }
        }

        /// <summary>
        /// 正式执行按钮点击事件 - 真正删除数据
        /// </summary>
        private async void KbtnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isBusy)
                {
                    MainForm.Instance.ShowStatusText("操作正在进行中，请稍候...");
                    return;
                }

                if (_selectedEntityType == null)
                {
                    MessageBox.Show("请先选择要清理的数据表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedIds = GetSelectedRecordIds();
                
                if (selectedIds.Count == 0)
                {
                    MessageBox.Show("请先勾选要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 显示严重警告确认对话框
                var confirmMsg = $"⚠️ 【正式删除警告】⚠️\n\n" +
                    $"您即将删除 {selectedIds.Count} 条主记录及其所有关联数据!\n\n" +
                    $"• 此操作不可撤销\n" +
                    $"• 将同时删除所有外键关联的记录\n" +
                    $"• 建议先备份重要数据\n\n" +
                    $"确定要继续吗?";

                if (MessageBox.Show(confirmMsg, "⚠️ 严重警告 - 确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
                {
                    AppendRealTimeLog("用户取消正式删除操作");
                    return;
                }

                _isBusy = true;
                kbtnRefresh.Enabled = false;
                MainForm.Instance.ShowStatusText("正在执行正式删除...");
                AppendRealTimeLog($"开始正式删除 {_selectedEntityType.Name} 表的 {selectedIds.Count} 条记录...");

                // 使用正式模式执行(会真正删除并提交事务)
                var result = await ExecuteCascadeDeleteAsync(selectedIds, isTestMode: false);

                if (result.IsSuccess)
                {
                    AppendRealTimeLog($"删除成功！共删除 {result.TotalDeletedCount} 条主记录及其关联数据，耗时: {result.TotalElapsedMs}ms");
                    
                    string successMsg = $"✅ 删除完成!\n\n" +
                        $"主记录数: {selectedIds.Count}\n" +
                        $"总删除数: {result.TotalDeletedCount} 条(含关联数据)\n" +
                        $"耗时: {result.TotalElapsedMs}ms\n\n" +
                        $"数据已永久删除，无法恢复!";
                    
                    MessageBox.Show(successMsg, "删除成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm.Instance.ShowStatusText("删除完成");

                    // 重新加载数据
                    await LoadDataAsync();
                }
                else
                {
                    AppendRealTimeLog($"[错误] 删除失败：{result.ErrorMessage}");
                    MessageBox.Show($"删除失败：{result.ErrorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainForm.Instance.ShowStatusText("删除失败");
                }
            }
            catch (Exception ex)
            {
                AppendRealTimeLog($"[异常] 删除操作异常: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"删除失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.Instance.ShowStatusText("删除失败");
            }
            finally
            {
                _isBusy = false;
                kbtnRefresh.Enabled = true;
            }
        }

        #endregion
    }
}
