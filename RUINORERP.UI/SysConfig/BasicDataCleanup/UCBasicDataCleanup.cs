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
        private Timer _searchTimer; // 用于搜索防抖
        private bool _isBusy = false; // 防止重复操作



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
            
            // 初始化搜索防抖 Timer
            _searchTimer = new Timer { Interval = 300 };
            _searchTimer.Tick += SearchTimer_Tick;
            
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
            };

            // 加载所有实体元数据
            _allEntities = EntityRegistry.Entities;

            // 初始化实体类型选择
            InitializeEntityTypes();
            
            // 初始化删除方式选择
            InitializeDeleteMode();

            // 绑定事件
            BindEvents();

            // 添加数据选择列到预览表格
            AddCheckBoxColumnToPreview();
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
        /// 初始化实体类型选择下拉框
        /// </summary>
        private void InitializeEntityTypes()
        {
            _isInitializing = true;
            kcmbEntityType.Items.Clear();
            kcmbEntityType.Items.Add("请选择");
            
            // 使用 EntityRegistry 获取分组数据
            var groupedEntities = EntityRegistry.GetAllGrouped();
            
            foreach (var group in groupedEntities)
            {
                // 添加分组标题 (不可选)
                kcmbEntityType.Items.Add($"--- {group.Key} ---");
                
                // 添加该分类下的所有实体
                foreach (var entity in group.Value)
                {
                    kcmbEntityType.Items.Add(entity.DisplayName);
                }
            }
            
            kcmbEntityType.SelectedIndex = 0;
            _isInitializing = false;
        }

        /// <summary>
        /// 初始化删除方式选择下拉框
        /// </summary>
        private void InitializeDeleteMode()
        {
            kcmbDeleteMode.Items.Clear();
            kcmbDeleteMode.Items.Add(new ComboBoxItem("实体导航模式", DataCleanupEngine.DeleteMode.EntityNavigation));
            kcmbDeleteMode.Items.Add(new ComboBoxItem("数据库元数据模式", DataCleanupEngine.DeleteMode.DatabaseMetadata));
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
            public DataCleanupEngine.DeleteMode Value { get; }
            
            public ComboBoxItem(string displayText, DataCleanupEngine.DeleteMode value)
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
            kcmbEntityType.SelectedIndexChanged += KcmbEntityType_SelectedIndexChanged;
            ktbSearchEntity.TextChanged += KtbSearchEntity_TextChanged;
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
        /// 搜索防抖 Timer 事件
        /// </summary>
        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            string searchText = ktbSearchEntity.Text;
            FilterEntityTypes(searchText);
            
            // 如果有搜索结果且只有一个，自动选中
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var matchedEntities = _allEntities.Where(e =>
                    e.DisplayName.ToLower().Contains(searchText.ToLower()) ||
                    e.Description.ToLower().Contains(searchText.ToLower()) ||
                    e.TableName.ToLower().Contains(searchText.ToLower()) ||
                    e.Category.ToLower().Contains(searchText.ToLower())
                ).ToList();
                
                if (matchedEntities.Count == 1)
                {
                    kcmbEntityType.SelectedItem = matchedEntities[0].DisplayName;
                }
            }
        }

        /// <summary>
        /// 搜索文本框变化事件 - 实时过滤实体类型
        /// </summary>
        private void KtbSearchEntity_TextChanged(object sender, EventArgs e)
        {
            // 防止初始化时触发
            if (_isInitializing)
                return;
            
            try
            {
                // 重启防抖 Timer
                _searchTimer.Stop();
                _searchTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"搜索实体类型失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    // 静默返回，不弹出提示打扰用户
                    return;
                }

                if (_selectedEntityType == null)
                {
                    MessageBox.Show("请先选择要清理的数据表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 获取选中的ID
                _selectedIds = GetSelectedRecordIds();
                AppendRealTimeLog($"准备删除，选中记录数: {_selectedIds.Count}");

                if (_selectedIds.Count == 0)
                {
                    MessageBox.Show("请先选择要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 显示确认对话框
                var confirmMsg = $"确定要删除选中的 {_selectedIds.Count} 条记录吗？\n\n此操作将同时删除所有关联数据(通过导航属性自动级联)！\n\n建议先备份重要数据。";

                if (MessageBox.Show(confirmMsg, "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    AppendRealTimeLog("用户取消删除操作");
                    return;
                }

                _isBusy = true;
                kbtnDeleteSelected.Enabled = false;
                kbtnRefresh.Enabled = false;
                AppendRealTimeLog($"开始删除 {_selectedEntityType.Name} 表的 {_selectedIds.Count} 条记录...");

                // 使用 DataCleanupEngine 执行级联删除
                var result = await ExecuteCascadeDeleteAsync(_selectedIds, isTestMode: false);

                if (result.IsSuccess)
                {
                    AppendRealTimeLog($"删除成功！共删除 {result.TotalDeletedCount} 条主记录及其关联数据，耗时: {result.TotalElapsedMs}ms");
                    MessageBox.Show($"删除完成！\n共删除 {result.TotalDeletedCount} 条主记录及其关联数据\n耗时: {result.TotalElapsedMs}ms", 
                        "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MainForm.Instance.ShowStatusText("删除完成");

                    // 重新加载数据
                    await LoadDataAsync();
                }
                else
                {
                    AppendRealTimeLog($"[错误] 删除失败：{result.ErrorMessage}");
                    MessageBox.Show($"删除失败：{result.ErrorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            }
        }

        /// <summary>
        /// 执行级联删除(使用 DataCleanupEngine)
        /// </summary>
        /// <param name="ids">要删除的ID列表，如果为null则使用 _selectedIds</param>
        /// <param name="isTestMode">是否测试模式</param>
        private async Task<CascadeDeleteResult> ExecuteCascadeDeleteAsync(List<long> ids = null, bool isTestMode = false)
        {
            var targetIds = ids ?? _selectedIds;
            
            // 获取删除模式
            var deleteMode = DataCleanupEngine.DeleteMode.EntityNavigation;
            if (kcmbDeleteMode.SelectedItem is ComboBoxItem selectedItem)
            {
                deleteMode = selectedItem.Value;
            }
            
            AppendRealTimeLog($"========== 开始级联删除 ==========");
            AppendRealTimeLog($"实体类型: {_selectedEntityType?.Name}, ID数量: {targetIds.Count}, 删除模式: {deleteMode}, 测试模式: {isTestMode}");
            
            // 使用反射调用泛型方法 ExecuteCascadeDeleteAsync<T>（带 DeleteMode 参数）
            // 注意：需要显式指定参数类型以匹配正确的重载
            var methodTypes = new[] { typeof(List<long>), typeof(DataCleanupEngine.DeleteMode), typeof(bool) };
            var method = typeof(DataCleanupEngine).GetMethod("ExecuteCascadeDeleteAsync", methodTypes);
            
            if (method == null)
            {
                throw new InvalidOperationException("未找到带 DeleteMode 参数的 ExecuteCascadeDeleteAsync 方法");
            }
            
            var genericMethod = method.MakeGenericMethod(_selectedEntityType);
            
            AppendRealTimeLog("正在构建依赖图和拓扑排序...");
            var task = (Task<CascadeDeleteResult>)genericMethod.Invoke(_cleanupEngine, new object[] { targetIds, deleteMode, isTestMode });
            
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

                // 使用测试模式执行，不实际删除
                var result = await ExecuteCascadeDeleteAsync(selectedIds, isTestMode: true);

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
            catch (Exception ex)
            {
                AppendRealTimeLog($"[异常] 预览异常: {ex.Message}");
                MessageBox.Show($"预览失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MainForm.Instance.ShowStatusText("正在测试执行...");
                AppendRealTimeLog($"开始测试执行 {_selectedEntityType.Name} 表的 {selectedIds.Count} 条记录...");

                // 使用测试模式执行
                var result = await ExecuteCascadeDeleteAsync(selectedIds, isTestMode: true);

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
