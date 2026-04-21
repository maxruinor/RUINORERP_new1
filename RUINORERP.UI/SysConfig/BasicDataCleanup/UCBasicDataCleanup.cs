// **************************************
// 文件：UCBasicDataCleanup.cs
// 项目：RUINORERP
// 作者：AI Assistant
// 时间：2026-04-20
// 描述：简洁版数据清理组件，支持级联删除
// **************************************

using Krypton.Navigator;
using Krypton.Toolkit;
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
    /// 用于基础数据的清理操作，支持级联删除
    /// </summary>
    [MenuAttrAssemblyInfo("基础数据清理", ModuleMenuDefine.模块定义.系统设置,ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCBasicDataCleanup : UserControl
    {
        private ISqlSugarClient _db;
        private Type _selectedEntityType;
        private List<long> _selectedIds;
        private EntityRelationshipAnalyzer _analyzer;

        /// <summary>
        /// 实体类型映射字典
        /// </summary>
        public static Dictionary<string, Type> EntityTypeMappings { get; private set; }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static UCBasicDataCleanup()
        {
            EntityTypeMappings = new Dictionary<string, Type>
            {
                { "供应商表", typeof(tb_CustomerVendor) },
                { "客户表", typeof(tb_CustomerVendor) },
                { "产品类目表", typeof(tb_ProdCategories) },
                { "产品基本信息表", typeof(tb_Prod) },
                { "产品详情信息表", typeof(tb_ProdDetail) },
                { "产品属性表", typeof(tb_ProdProperty) },
                { "产品属性值表", typeof(tb_ProdPropertyValue) },
                { "库位表", typeof(tb_Location) },
                { "货架表", typeof(tb_StorageRack) },
                { "单位表", typeof(tb_Unit) },
                { "产品类型表", typeof(tb_ProductType) },
                { "部门表", typeof(tb_Department) },
            };
        }

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
            
            // 初始化数据网格视图
            dgvDataPreview.AutoGenerateColumns = true;
            dgvDataPreview.AllowUserToAddRows = false;
            dgvDataPreview.AllowUserToDeleteRows = false;

            // 初始化数据库连接
            LoadDbConnection();

            // 初始化实体类型选择
            InitializeEntityTypes();

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
                _analyzer = new EntityRelationshipAnalyzer(_db);
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
            kcmbEntityType.Items.Clear();
            kcmbEntityType.Items.Add("请选择");

            foreach (var mapping in EntityTypeMappings)
            {
                kcmbEntityType.Items.Add(mapping.Key);
            }

            kcmbEntityType.SelectedIndex = 0;
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            kcmbEntityType.SelectedIndexChanged += KcmbEntityType_SelectedIndexChanged;
            kbtnRefresh.Click += KbtnRefresh_Click;
            kbtnSelectAll.Click += KbtnSelectAll_Click;
            kbtnSelectNone.Click += KbtnSelectNone_Click;
            kbtnSelectInvert.Click += KbtnSelectInvert_Click;
            kbtnDeleteSelected.Click += KbtnDeleteSelected_Click;
        }

        /// <summary>
        /// 实体类型选择改变事件
        /// </summary>
        private void KcmbEntityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (kcmbEntityType.SelectedIndex <= 0)
                {
                    _selectedEntityType = null;
                    dgvDataPreview.Rows.Clear();
                    return;
                }

                string selectedText = kcmbEntityType.SelectedItem.ToString();
                if (EntityTypeMappings.ContainsKey(selectedText))
                {
                    _selectedEntityType = EntityTypeMappings[selectedText];
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
            if (_selectedEntityType == null)
            {
                MessageBox.Show("请先选择要清理的数据表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                kbtnRefresh.Enabled = false;
                MainForm.Instance.ShowStatusText("正在查询数据...");
                Application.DoEvents();

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
                kbtnRefresh.Enabled = true;
            }
        }

        /// <summary>
        /// 使用反射查询数据
        /// </summary>
        private async Task<List<object>> QueryDataAsync(Type entityType)
        {
            // 使用反射调用 Queryable<T>
            var queryableMethod = typeof(SqlSugarClient).GetMethod("Queryable", Type.EmptyTypes);
            var genericQueryable = queryableMethod.MakeGenericMethod(entityType);
            var queryable = genericQueryable.Invoke(_db, null);

            // 调用 ToListAsync 方法
            var toListAsyncMethod = queryable.GetType().GetMethod("ToListAsync");
            var task = toListAsyncMethod.Invoke(queryable, null);

            return await (Task<List<object>>)task;
        }

        /// <summary>
        /// 加载数据到表格
        /// </summary>
        private async Task LoadDataAsync()
        {
            dgvDataPreview.Rows.Clear();

            // 使用反射查询数据
            var records = await QueryDataAsync(_selectedEntityType);

            // 动态添加列（除了 CheckBox 列）
            var properties = _selectedEntityType.GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .Take(20) // 限制显示列数
                .ToList();

            // 添加列
            if (dgvDataPreview.Columns.Count <= 1)
            {
                foreach (var prop in properties)
                {
                    var column = new DataGridViewTextBoxColumn
                    {
                        Name = $"col_{prop.Name}",
                        HeaderText = prop.Name,
                        DataPropertyName = prop.Name,
                        Width = 120
                    };
                    dgvDataPreview.Columns.Add(column);
                }
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
                    cell.Value = !(cell.Value != null && (bool)cell.Value);
                }
            }
            UpdateSelectedCount();
        }

        /// <summary>
        /// 删除选中按钮点击事件
        /// </summary>
        private async void KbtnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (_selectedEntityType == null)
            {
                MessageBox.Show("请先选择要清理的数据表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _selectedIds = GetSelectedRecordIds();

            if (_selectedIds.Count == 0)
            {
                MessageBox.Show("请先选择要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 显示级联删除确认
            var cascadeInfo = BuildCascadeDeleteInfo();
            var confirmMsg = $"确定要删除选中的 {_selectedIds.Count} 条记录吗？\n\n{cascadeInfo}\n\n此操作将同时删除所有关联数据！";

            if (MessageBox.Show(confirmMsg, "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                kbtnDeleteSelected.Enabled = false;
                MainForm.Instance.ShowStatusText("正在删除数据...");
                Application.DoEvents();

                await DeleteRecordsAsync();

                MessageBox.Show($"删除完成！共删除 {_selectedIds.Count} 条记录及其关联数据", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                MainForm.Instance.ShowStatusText("删除完成");

                // 重新加载数据
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.Instance.ShowStatusText("删除失败");
            }
            finally
            {
                kbtnDeleteSelected.Enabled = true;
            }
        }

        /// <summary>
        /// 构建级联删除信息
        /// </summary>
        private string BuildCascadeDeleteInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("级联删除范围:");

            // 获取清理顺序
            var cleanupOrder = _analyzer.GenerateCleanupOrder(_selectedEntityType);

            foreach (var node in cleanupOrder)
            {
                if (node.EntityType != _selectedEntityType)
                {
                    sb.AppendLine($"  - {node.TableName} (关联数据)");
                }
            }

            if (cleanupOrder.Count <= 1)
            {
                sb.AppendLine("  无关联数据");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取选中的记录 ID 列表
        /// </summary>
        private List<long> GetSelectedRecordIds()
        {
            var selectedIds = new List<long>();
            var pkName = GetPrimaryKeyName(_selectedEntityType);

            foreach (DataGridViewRow row in dgvDataPreview.Rows)
            {
                if (row.Cells["colSelect"] is DataGridViewCheckBoxCell cell &&
                    cell.Value != null && (bool)cell.Value)
                {
                    var record = row.Tag;
                    if (record != null)
                    {
                        var pkProp = _selectedEntityType.GetProperty(pkName);
                        if (pkProp != null)
                        {
                            var idValue = pkProp.GetValue(record);
                            if (long.TryParse(idValue?.ToString(), out long id))
                            {
                                selectedIds.Add(id);
                            }
                        }
                    }
                }
            }

            return selectedIds;
        }

        /// <summary>
        /// 删除记录（包括关联数据）
        /// </summary>
        private async Task DeleteRecordsAsync()
        {
            // 生成清理顺序
            var cleanupOrder = _analyzer.GenerateCleanupOrder(_selectedEntityType);

            // 按顺序删除（从最外层关联开始删除）
            foreach (var node in cleanupOrder)
            {
                MainForm.Instance.ShowStatusText($"正在删除：{node.TableName}");
                Application.DoEvents();

                if (node.EntityType == _selectedEntityType)
                {
                    // 目标表，根据 ID 删除
                    await DeleteByIdsAsync(node.EntityType, _selectedIds);
                }
                else
                {
                    // 关联表，根据外键关系删除
                    await DeleteRelatedAsync(node.EntityType, _selectedEntityType, _selectedIds);
                }
            }
        }

        /// <summary>
        /// 根据 ID 删除记录
        /// </summary>
        private async Task DeleteByIdsAsync(Type entityType, List<long> ids)
        {
            var deleteableMethod = typeof(SqlSugarClient).GetMethod("Deleteable", Type.EmptyTypes);
            var genericDeleteable = deleteableMethod.MakeGenericMethod(entityType);
            var deleteable = genericDeleteable.Invoke(_db, null);

            var pkName = GetPrimaryKeyName(entityType);
            var idsArray = ids.Cast<object>().ToArray();
            var inMethod = deleteable.GetType().GetMethod("In", new[] { typeof(string), typeof(object[]) });
            var filteredDeleteable = inMethod.Invoke(deleteable, new object[] { pkName, idsArray });

            var executeMethod = filteredDeleteable.GetType().GetMethod("ExecuteCommandAsync");
            var task = executeMethod.Invoke(filteredDeleteable, null);

            await (Task)task;
        }

        /// <summary>
        /// 删除关联数据
        /// </summary>
        private async Task DeleteRelatedAsync(Type relatedEntityType, Type mainEntityType, List<long> mainEntityIds)
        {
            // 获取外键字段
            var foreignKeyField = _analyzer.GetForeignKeyField(relatedEntityType, mainEntityType);

            if (!string.IsNullOrEmpty(foreignKeyField))
            {
                var deleteableMethod = typeof(SqlSugarClient).GetMethod("Deleteable", Type.EmptyTypes);
                var genericDeleteable = deleteableMethod.MakeGenericMethod(relatedEntityType);
                var deleteable = genericDeleteable.Invoke(_db, null);

                var idsArray = mainEntityIds.Cast<object>().ToArray();
                var inMethod = deleteable.GetType().GetMethod("In", new[] { typeof(string), typeof(object[]) });
                var filteredDeleteable = inMethod.Invoke(deleteable, new object[] { foreignKeyField, idsArray });

                var executeMethod = filteredDeleteable.GetType().GetMethod("ExecuteCommandAsync");
                var task = executeMethod.Invoke(filteredDeleteable, null);

                await (Task)task;
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
        private void KbtnPreview_Click(object sender, EventArgs e) { }
        private void KbtnTestExecute_Click(object sender, EventArgs e) { }
        private void KbtnExecute_Click(object sender, EventArgs e) { }
        private void DgvRules_SelectionChanged(object sender, EventArgs e) { }

        #endregion
    }
}
