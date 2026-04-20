using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.Global;
using RUINORERP.Model.ImportEngine.Models;
using SqlSugar;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 外键关联配置对话框
    /// </summary>
    public partial class FrmForeignKeyConfig : KryptonForm
    {
        private ISqlSugarClient _db;
        private ForeignRelatedConfig _config;
        private List<string> _availableTables;

        public ForeignRelatedConfig Result => _config;

        public FrmForeignKeyConfig(ISqlSugarClient db, ForeignRelatedConfig existingConfig = null)
        {
            InitializeComponent();
            _db = db;
            _config = existingConfig ?? new ForeignRelatedConfig();
            
            InitializeControls();
            LoadAvailableTables();
        }

        private void InitializeControls()
        {
            // 初始化控件
            ktxtTableName.ReadOnly = true;
            ktxtKeyField.ReadOnly = true;
            ktxtSourceField.ReadOnly = true;
            
            // 加载事件
            this.Load += FrmForeignKeyConfig_Load;
        }

        private void FrmForeignKeyConfig_Load(object sender, EventArgs e)
        {
            // 如果已有配置,填充到界面
            if (_config != null)
            {
                ktxtTableName.Text = _config.ForeignKeyTable?.Key;
                ktxtKeyField.Text = _config.ForeignKeyField?.Key;
                ktxtSourceField.Text = _config.ForeignKeySourceColumn?.Key;
                kchkAutoCreate.Checked = _config.AutoCreateIfNotExists;
            }
        }

        /// <summary>
        /// 加载可用的数据库表列表
        /// </summary>
        private void LoadAvailableTables()
        {
            try
            {
                // 获取数据库中所有表名
                var tables = _db.DbMaintenance.GetTableInfoList(false);
                _availableTables = tables.Select(t => t.Name).OrderBy(t => t).ToList();
                
                kcmbSelectTable.Items.Clear();
                kcmbSelectTable.Items.AddRange(_availableTables.ToArray());
                
                if (_availableTables.Any())
                {
                    kcmbSelectTable.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载表列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 选择表后加载字段列表
        /// </summary>
        private void kcmbSelectTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbSelectTable.SelectedIndex < 0)
                return;

            string tableName = kcmbSelectTable.SelectedItem.ToString();
            LoadTableFields(tableName);
        }

        /// <summary>
        /// 加载指定表的字段列表
        /// </summary>
        private void LoadTableFields(string tableName)
        {
            try
            {
                var columns = _db.DbMaintenance.GetColumnInfosByTableName(tableName, false);
                
                // 填充主键字段下拉框
                kcmbKeyField.Items.Clear();
                foreach (var col in columns)
                {
                    kcmbKeyField.Items.Add(col.DbColumnName);
                    if (col.IsPrimarykey)
                    {
                        kcmbKeyField.SelectedItem = col.DbColumnName;
                    }
                }

                // 填充源字段下拉框(排除主键)
                kcmbSourceField.Items.Clear();
                foreach (var col in columns.Where(c => !c.IsPrimarykey))
                {
                    kcmbSourceField.Items.Add(col.DbColumnName);
                }

                if (kcmbKeyField.Items.Count > 0 && kcmbKeyField.SelectedItem == null)
                {
                    kcmbKeyField.SelectedIndex = 0;
                }

                if (kcmbSourceField.Items.Count > 0 && kcmbSourceField.SelectedItem == null)
                {
                    kcmbSourceField.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载字段列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 确定按钮点击
        /// </summary>
        private void kbtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ktxtTableName.Text))
            {
                MessageBox.Show("请选择关联表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ktxtKeyField.Text))
            {
                MessageBox.Show("请选择主键字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ktxtSourceField.Text))
            {
                MessageBox.Show("请选择源字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 保存配置
            _config.ForeignKeyTable = new SerializableKeyValuePair<string>
            {
                Key = ktxtTableName.Text,
                Value = ktxtTableName.Text  // 暂时使用相同的值
            };
            _config.ForeignKeyField = new SerializableKeyValuePair<string>
            {
                Key = ktxtKeyField.Text,
                Value = ktxtKeyField.Text
            };
            _config.ForeignKeySourceColumn = new SerializableKeyValuePair<string>
            {
                Key = ktxtSourceField.Text,
                Value = ktxtSourceField.Text
            };
            _config.AutoCreateIfNotExists = kchkAutoCreate.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 取消按钮点击
        /// </summary>
        private void kbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 从下拉框选择表
        /// </summary>
        private void kbtnSelectTable_Click(object sender, EventArgs e)
        {
            if (kcmbSelectTable.SelectedIndex >= 0)
            {
                ktxtTableName.Text = kcmbSelectTable.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// 从下拉框选择主键字段
        /// </summary>
        private void kbtnSelectKeyField_Click(object sender, EventArgs e)
        {
            if (kcmbKeyField.SelectedIndex >= 0)
            {
                ktxtKeyField.Text = kcmbKeyField.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// 从下拉框选择源字段
        /// </summary>
        private void kbtnSelectSourceField_Click(object sender, EventArgs e)
        {
            if (kcmbSourceField.SelectedIndex >= 0)
            {
                ktxtSourceField.Text = kcmbSourceField.SelectedItem.ToString();
            }
        }
    }
}
