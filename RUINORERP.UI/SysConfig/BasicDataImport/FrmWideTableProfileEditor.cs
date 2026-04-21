using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using Newtonsoft.Json;
using RUINORERP.Model.ImportEngine.Models;
using SqlSugar;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 宽表Profile配置编辑器
    /// </summary>
    public partial class FrmWideTableProfileEditor : KryptonForm
    {
        private ISqlSugarClient _db;
        private WideTableImportProfile _profile;
        private string _profileDirectory;
        private List<string> _availableTables;

        public FrmWideTableProfileEditor(ISqlSugarClient db, WideTableImportProfile existingProfile = null)
        {
            InitializeComponent();
            _db = db;
            _profile = existingProfile ?? new WideTableImportProfile();
            _profileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfig", "DataMigration", "Profiles");
            
            InitializeControls();
            LoadAvailableTables();
            LoadProfileToUI();
        }

        private void InitializeControls()
        {
            // 初始化DataGridView
            dgvMasterColumns.AutoGenerateColumns = false;
            dgvDependencyTables.AutoGenerateColumns = false;
            dgvChildTables.AutoGenerateColumns = false;

            // 添加列
            SetupMasterColumnsGrid();
            SetupDependencyTablesGrid();
            SetupChildTablesGrid();

            // 加载事件
            this.Load += FrmWideTableProfileEditor_Load;
        }

        private void FrmWideTableProfileEditor_Load(object sender, EventArgs e)
        {
            // 初始显示主表配置页
            kryptonNavigatorProfile.SelectedIndex = 0;
        }

        private void LoadAvailableTables()
        {
            try
            {
                var tables = _db.DbMaintenance.GetTableInfoList(false);
                _availableTables = tables.Select(t => t.Name).OrderBy(t => t).ToList();

                kcmbMasterTable.Items.Clear();
                kcmbMasterTable.Items.AddRange(_availableTables.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载表列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProfileToUI()
        {
            if (_profile == null) return;

            // 基本信息
            ktxtProfileName.Text = _profile.ProfileName;
            ktxtDescription.Text = _profile.Description;

            // 主表配置
            if (_profile.MasterTable != null)
            {
                kcmbMasterTable.SelectedItem = _profile.MasterTable.TargetTable;
                
                // 业务键
                ktxtBusinessKeys.Text = string.Join(",", _profile.MasterTable.BusinessKeys);
                
                // 列映射
                if (_profile.MasterTable.ColumnMappings != null)
                {
                    foreach (var mapping in _profile.MasterTable.ColumnMappings)
                    {
                        dgvMasterColumns.Rows.Add(
                            mapping.ExcelHeader,
                            mapping.DbColumn,
                            mapping.DataType,
                            mapping.TransformRule ?? "",
                            mapping.ForeignConfig != null ? "已配置" : ""
                        );
                    }
                }
            }

            // 依赖表
            if (_profile.DependencyTables != null)
            {
                foreach (var depTable in _profile.DependencyTables)
                {
                    dgvDependencyTables.Rows.Add(
                        depTable.TargetTable,
                        string.Join(",", depTable.BusinessKeys),
                        depTable.ColumnMappings?.Count ?? 0
                    );
                }
            }

            // 子表
            if (_profile.ChildTables != null)
            {
                foreach (var childTable in _profile.ChildTables)
                {
                    dgvChildTables.Rows.Add(
                        childTable.TargetTable,
                        childTable.ParentTableName,
                        childTable.ParentTableRefField,
                        string.Join(",", childTable.BusinessKeys)
                    );
                }
            }
        }

        #region DataGridView设置

        private void SetupMasterColumnsGrid()
        {
            dgvMasterColumns.Columns.Add("ExcelHeader", "Excel列名");
            dgvMasterColumns.Columns.Add("DbColumn", "数据库字段");
            dgvMasterColumns.Columns.Add("DataType", "数据类型");
            dgvMasterColumns.Columns.Add("TransformRule", "转换规则");
            dgvMasterColumns.Columns.Add("FKConfig", "外键配置");
        }

        private void SetupDependencyTablesGrid()
        {
            dgvDependencyTables.Columns.Add("TableName", "表名");
            dgvDependencyTables.Columns.Add("BusinessKeys", "业务键");
            dgvDependencyTables.Columns.Add("ColumnCount", "列数");
        }

        private void SetupChildTablesGrid()
        {
            dgvChildTables.Columns.Add("TableName", "表名");
            dgvChildTables.Columns.Add("ParentTable", "父表");
            dgvChildTables.Columns.Add("RefField", "关联字段");
            dgvChildTables.Columns.Add("BusinessKeys", "业务键");
        }

        #endregion

        #region 按钮事件

        /// <summary>
        /// 添加列映射
        /// </summary>
        private void kbtnAddColumn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(kcmbMasterTable.SelectedItem?.ToString()))
            {
                MessageBox.Show("请先选择主表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvMasterColumns.Rows.Add("", "", "String", "", "");
        }

        /// <summary>
        /// 编辑列映射
        /// </summary>
        private void kbtnEditColumn_Click(object sender, EventArgs e)
        {
            if (dgvMasterColumns.CurrentRow == null) return;

            var row = dgvMasterColumns.CurrentRow;
            // 这里可以弹出一个简单的编辑对话框，目前先允许直接在 Grid 中修改
            row.Cells["ExcelHeader"].ReadOnly = false;
            row.Cells["DbColumn"].ReadOnly = false;
        }

        /// <summary>
        /// 删除列映射
        /// </summary>
        private void kbtnDeleteColumn_Click(object sender, EventArgs e)
        {
            if (dgvMasterColumns.CurrentRow != null && !dgvMasterColumns.CurrentRow.IsNewRow)
            {
                dgvMasterColumns.Rows.Remove(dgvMasterColumns.CurrentRow);
            }
        }

        /// <summary>
        /// 添加依赖表
        /// </summary>
        private void kbtnAddDependency_Click(object sender, EventArgs e)
        {
            dgvDependencyTables.Rows.Add("", "", 0);
        }

        /// <summary>
        /// 添加子表
        /// </summary>
        private void kbtnAddChild_Click(object sender, EventArgs e)
        {
            dgvChildTables.Rows.Add("", kcmbMasterTable.SelectedItem?.ToString(), "", "");
        }

        /// <summary>
        /// 保存Profile
        /// </summary>
        private void kbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ktxtProfileName.Text))
                {
                    MessageBox.Show("请输入Profile名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 构建Profile对象
                BuildProfileFromUI();

                // 保存到JSON文件
                var filePath = Path.Combine(_profileDirectory, $"{_profile.ProfileName}.json");
                var json = JsonConvert.SerializeObject(_profile, Formatting.Indented);
                File.WriteAllText(filePath, json, Encoding.UTF8);

                MessageBox.Show($"Profile保存成功!\n\n文件路径: {filePath}", "成功", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 测试配置
        /// </summary>
        private void kbtnTest_Click(object sender, EventArgs e)
        {
            MessageBox.Show("测试功能开发中...\n\n此功能将验证:\n1. 表是否存在\n2. 字段是否匹配\n3. 外键关系是否正确", 
                "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 自动填充依赖表（简化版：提示用户手动配置）
        /// </summary>
        private void kbtnAutoFill_Click(object sender, EventArgs e)
        {
            try
            {
                if (kcmbMasterTable.SelectedItem == null)
                {
                    MessageBox.Show("请先选择主表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string tableName = kcmbMasterTable.SelectedItem.ToString();
                
                // 提示用户手动配置依赖表
                // 自动分析功能已移除，请根据业务需求手动添加依赖表
                MessageBox.Show(
                    $"主表 [{tableName}] 已选定。\n\n" +
                    "由于自动依赖分析功能已简化，请手动配置依赖表：\n" +
                    "1. 在下方'依赖表列表'中点击'添加行'\n" +
                    "2. 输入依赖表名称\n" +
                    "3. 设置关联字段和Excel列\n\n" +
                    "常用依赖表示例：\n" +
                    "• 产品导入 → 可能需要先导入: 供应商表、类目表\n" +
                    "• 销售订单 → 可能需要先导入: 客户表、产品表",
                    "手动配置依赖表",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void kbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        /// <summary>
        /// 从UI构建Profile对象
        /// </summary>
        private void BuildProfileFromUI()
        {
            _profile.ProfileName = ktxtProfileName.Text;
            _profile.Description = ktxtDescription.Text;

            // 主表
            _profile.MasterTable = new ImportProfile
            {
                TargetTable = kcmbMasterTable.SelectedItem?.ToString(),
                BusinessKeys = ktxtBusinessKeys.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                ColumnMappings = new List<RUINORERP.Model.ImportEngine.Models.ColumnMapping>()
            };

            // 从DataGridView读取列映射
            foreach (DataGridViewRow row in dgvMasterColumns.Rows)
            {
                if (row.IsNewRow) continue;

                var mapping = new RUINORERP.Model.ImportEngine.Models.ColumnMapping
                {
                    ExcelHeader = row.Cells["ExcelHeader"].Value?.ToString(),
                    DbColumn = row.Cells["DbColumn"].Value?.ToString(),
                    DataType = row.Cells["DataType"].Value?.ToString(),
                    TransformRule = row.Cells["TransformRule"].Value?.ToString()
                };

                _profile.MasterTable.ColumnMappings.Add(mapping);
            }

            // 依赖表
            _profile.DependencyTables = new List<ImportProfile>();
            foreach (DataGridViewRow row in dgvDependencyTables.Rows)
            {
                if (row.IsNewRow) continue;

                var depTable = new ImportProfile
                {
                    TargetTable = row.Cells["TableName"].Value?.ToString(),
                    BusinessKeys = row.Cells["BusinessKeys"].Value?.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                };

                _profile.DependencyTables.Add(depTable);
            }

            // 子表
            _profile.ChildTables = new List<ChildTableConfig>();
            foreach (DataGridViewRow row in dgvChildTables.Rows)
            {
                if (row.IsNewRow) continue;

                var childTable = new ChildTableConfig
                {
                    TargetTable = row.Cells["TableName"].Value?.ToString(),
                    ParentTableName = row.Cells["ParentTable"].Value?.ToString(),
                    ParentTableRefField = row.Cells["RefField"].Value?.ToString(),
                    BusinessKeys = row.Cells["BusinessKeys"].Value?.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                };

                _profile.ChildTables.Add(childTable);
            }
        }
    }
}
