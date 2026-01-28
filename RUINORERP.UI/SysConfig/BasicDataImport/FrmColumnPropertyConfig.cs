using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.UI.Common;
using RUINORERP.Common;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 列属性配置对话框
    /// 用于配置列的各种属性，包括外键、唯一性、默认值、系统生成等
    /// </summary>
    public partial class FrmColumnPropertyConfig : KryptonForm
    {
        /// <summary>
        /// 当前映射配置
        /// </summary>
        public ColumnMapping CurrentMapping { get; set; }

        /// <summary>
        /// 目标实体类型
        /// </summary>
        public Type TargetEntityType { get; set; }

        /// <summary>
        /// 是否为外键
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 是否值唯一
        /// </summary>
        public bool IsUniqueValue { get; set; }

        /// <summary>
        /// 是否忽略空值（为空时不导入）
        /// </summary>
        public bool IgnoreEmptyValue { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是否系统生成
        /// </summary>
        public bool IsSystemGenerated { get; set; }

        /// <summary>
        /// 外键表引用
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyTable { get; set; }

        /// <summary>
        /// 外键字段引用
        /// </summary>
        public SerializableKeyValuePair<string> ForeignKeyField { get; set; }

        /// <summary>
        /// 数据来源类型
        /// </summary>
        public DataSourceType SelectedDataSourceType { get; set; }

        /// <summary>
        /// 自身引用字段
        /// </summary>
        public SerializableKeyValuePair<string> SelfReferenceField { get; set; }

        /// <summary>
        /// 字段信息字典（字段名 -> 中文名）
        /// </summary>
        private System.Collections.Concurrent.ConcurrentDictionary<string, string> _fieldInfoDict;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmColumnPropertyConfig()
        {
            InitializeComponent();
            LoadRelatedTables();
            LoadDataSourceTypes();

            // 手动绑定事件
            kcmbDataSourceType.SelectedIndexChanged += kcmbDataSourceType_SelectedIndexChanged;
            kcmbSelfReferenceField.SelectedIndexChanged += kcmbSelfReferenceField_SelectedIndexChanged;
        }

        /// <summary>
        /// 加载关联表列表
        /// </summary>
        private void LoadRelatedTables()
        {
            try
            {
                // 添加支持的关联表
                kcmbRelatedTable.Items.Clear();
                kcmbRelatedTable.Items.Add("请选择");
                kcmbRelatedTable.Items.Add("供应商表 (tb_Supplier)");
                kcmbRelatedTable.Items.Add("产品类目表 (tb_ProdCategories)");
                kcmbRelatedTable.Items.Add("产品信息表 (tb_Prod)");
                kcmbRelatedTable.Items.Add("产品属性表 (tb_ProdProperty)");
                kcmbRelatedTable.Items.Add("产品属性值表 (tb_ProdPropertyValue)");
                kcmbRelatedTable.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载关联表列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载数据来源类型列表
        /// </summary>
        private void LoadDataSourceTypes()
        {
            try
            {
                kcmbDataSourceType.Items.Clear();
                kcmbDataSourceType.Items.Add("Excel数据源");
                kcmbDataSourceType.Items.Add("默认值");
                kcmbDataSourceType.Items.Add("系统生成");
                kcmbDataSourceType.Items.Add("外键关联");
                kcmbDataSourceType.Items.Add("自身字段引用");
                kcmbDataSourceType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据来源类型失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void FrmColumnPropertyConfig_Load(object sender, EventArgs e)
        {
            if (CurrentMapping != null)
            {
                // 初始化当前配置
                kchkIsForeignKey.Checked = CurrentMapping.IsForeignKey;
                kchkIsUniqueValue.Checked = CurrentMapping.IsUniqueValue;
                kchkIgnoreEmptyValue.Checked = CurrentMapping.IgnoreEmptyValue;
                kchkIsSystemGenerated.Checked = CurrentMapping.IsSystemGenerated;
                ktxtDefaultValue.Text = CurrentMapping.DefaultValue ?? string.Empty;

                IsForeignKey = CurrentMapping.IsForeignKey;
                IsUniqueValue = CurrentMapping.IsUniqueValue;
                IgnoreEmptyValue = CurrentMapping.IgnoreEmptyValue;
                DefaultValue = CurrentMapping.DefaultValue;
                IsSystemGenerated = CurrentMapping.IsSystemGenerated;

                // 初始化数据来源类型
                kcmbDataSourceType.SelectedIndex = (int)CurrentMapping.DataSourceType;
                SelectedDataSourceType = CurrentMapping.DataSourceType;

                // 初始化关联表信息
                if (!string.IsNullOrEmpty(CurrentMapping.ForeignKeyTable?.Key))
                {
                    // 查找对应的显示文本
                    for (int i = 0; i < kcmbRelatedTable.Items.Count; i++)
                    {
                        string itemText = kcmbRelatedTable.Items[i].ToString();
                        if (itemText.Contains(CurrentMapping.ForeignKeyTable.Key))
                        {
                            kcmbRelatedTable.SelectedIndex = i;
                            break;
                        }
                    }
                }

                ForeignKeyTable = CurrentMapping.ForeignKeyTable;
                ktxtRelatedField.Text = CurrentMapping.ForeignKeyField?.Value;

                // 初始化自身引用字段
                if (CurrentMapping.DataSourceType == DataSourceType.SelfReference &&
                    CurrentMapping.SelfReferenceField != null)
                {
                    LoadSelfReferenceFields();
                    SelfReferenceField = CurrentMapping.SelfReferenceField;
                    kcmbSelfReferenceField.SelectedItem = CurrentMapping.SelfReferenceField?.Value;
                }
            }

            // 更新控件状态
            UpdateControlStates();
        }

        /// <summary>
        /// 更新控件状态
        /// </summary>
        private void UpdateControlStates()
        {
            DataSourceType dataSourceType = (DataSourceType)kcmbDataSourceType.SelectedIndex;

            // 根据数据来源类型控制控件状态
            kcmbRelatedTable.Enabled = (dataSourceType == DataSourceType.ForeignKey);
            ktxtRelatedField.Enabled = (dataSourceType == DataSourceType.ForeignKey);
            kcmbSelfReferenceField.Enabled = (dataSourceType == DataSourceType.SelfReference);
            ktxtDefaultValue.Enabled = (dataSourceType == DataSourceType.DefaultValue);

            // 同步复选框状态
            kchkIsForeignKey.Enabled = (dataSourceType == DataSourceType.ForeignKey);
            kchkIsSystemGenerated.Enabled = (dataSourceType == DataSourceType.SystemGenerated);

            // 根据数据来源类型设置复选框
            if (dataSourceType == DataSourceType.ForeignKey)
            {
                kchkIsForeignKey.Checked = true;
            }
            else if (dataSourceType == DataSourceType.SystemGenerated)
            {
                kchkIsSystemGenerated.Checked = true;
            }
            else
            {
                kchkIsForeignKey.Checked = false;
                kchkIsSystemGenerated.Checked = false;
            }
        }

        /// <summary>
        /// 关联表选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kcmbRelatedTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbRelatedTable.SelectedIndex <= 0)
            {
                ktxtRelatedField.Items.Clear();
                return;
            }

            // 获取选中的表名
            string selectedTable = kcmbRelatedTable.SelectedItem.ToString();
            int startIndex = selectedTable.IndexOf('(') + 1;
            int endIndex = selectedTable.IndexOf(')');
            string tableName = string.Empty;

            if (startIndex > 0 && endIndex > startIndex)
            {
                tableName = selectedTable.Substring(startIndex, endIndex - startIndex);
            }

            // 加载表的字段列表
            LoadTableFields(tableName);
        }

        /// <summary>
        /// 加载表的字段列表
        /// </summary>
        /// <param name="tableName">表名</param>
        private void LoadTableFields(string tableName)
        {
            try
            {
                Type tableType = GetTableType(tableName);
                if (tableType == null)
                {
                    MessageBox.Show($"无法找到表类型: {tableName}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 获取字段信息
                var fieldNameList = UIHelper.GetFieldNameList(false, tableType);
                _fieldInfoDict = fieldNameList;

                // 清空并添加字段到下拉框
                ktxtRelatedField.Items.Clear();
                foreach (var field in fieldNameList)
                {
                    ktxtRelatedField.Items.Add(field.Value); // 添加中文名
                }

                // 如果有已选中的字段，保持选中状态
                if (!string.IsNullOrEmpty(ForeignKeyField?.Value))
                {
                    int index = ktxtRelatedField.Items.IndexOf(ForeignKeyField?.Value);
                    if (index >= 0)
                    {
                        ktxtRelatedField.SelectedIndex = index;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载表字段失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据表名获取类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表类型</returns>
        private Type GetTableType(string tableName)
        {
            // 这里需要根据实际的命名空间和类名来获取
            // 简化处理，使用字符串匹配
            switch (tableName)
            {
                case "tb_Supplier":
                    return Type.GetType("RUINORERP.Model.tb_Supplier");
                case "tb_ProdCategories":
                    return Type.GetType("RUINORERP.Model.tb_ProdCategories");
                case "tb_Prod":
                    return Type.GetType("RUINORERP.Model.tb_Prod");
                case "tb_ProdProperty":
                    return Type.GetType("RUINORERP.Model.tb_ProdProperty");
                case "tb_ProdPropertyValue":
                    return Type.GetType("RUINORERP.Model.tb_ProdPropertyValue");
                default:
                    return null;
            }
        }

        /// <summary>
        /// 关联表字段选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void ktxtRelatedField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ktxtRelatedField.SelectedIndex < 0 || _fieldInfoDict == null)
            {
                return;
            }

            // 根据中文显示名称获取实际字段名
            string displayName = ktxtRelatedField.SelectedItem.ToString();
            var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
            if (field.Value != null)
            {
                ForeignKeyField = new SerializableKeyValuePair<string>(field.Key, field.Value);
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnOK_Click(object sender, EventArgs e)
        {
            DataSourceType dataSourceType = (DataSourceType)kcmbDataSourceType.SelectedIndex;
            SelectedDataSourceType = dataSourceType;

            // 根据数据来源类型进行验证
            if (dataSourceType == DataSourceType.ForeignKey)
            {
                if (kcmbRelatedTable.SelectedIndex <= 0)
                {
                    MessageBox.Show("请选择关联表", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ktxtRelatedField.SelectedIndex < 0 && string.IsNullOrWhiteSpace(ktxtRelatedField.Text))
                {
                    MessageBox.Show("请选择关联表字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 获取关联表名（提取括号中的表名）
                string selectedTable = kcmbRelatedTable.SelectedItem.ToString();
                int startIndex = selectedTable.IndexOf('(') + 1;
                int endIndex = selectedTable.IndexOf(')');
                string tableName;
                if (startIndex > 0 && endIndex > startIndex)
                {
                    tableName = selectedTable.Substring(startIndex, endIndex - startIndex);
                }
                else
                {
                    tableName = selectedTable;
                }

                string displayName = ktxtRelatedField.SelectedItem?.ToString() ?? ktxtRelatedField.Text;
                var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
                if (field.Value != null)
                {
                    ForeignKeyTable = new SerializableKeyValuePair<string>(tableName, GetTableDisplayName(tableName));
                    ForeignKeyField = new SerializableKeyValuePair<string>(field.Key, field.Value);
                }
            }
            else if (dataSourceType == DataSourceType.SelfReference)
            {
                if (kcmbSelfReferenceField.SelectedIndex < 0 && string.IsNullOrWhiteSpace(kcmbSelfReferenceField.Text))
                {
                    MessageBox.Show("请选择自身引用字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string displayName = kcmbSelfReferenceField.SelectedItem?.ToString() ?? kcmbSelfReferenceField.Text;
                var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
                if (field.Value != null)
                {
                    SelfReferenceField = new SerializableKeyValuePair<string>(field.Key, field.Value);
                }
            }
            else if (dataSourceType == DataSourceType.DefaultValue)
            {
                if (string.IsNullOrWhiteSpace(ktxtDefaultValue.Text))
                {
                    MessageBox.Show("请输入默认值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 根据数据来源类型设置属性
            if (dataSourceType == DataSourceType.ForeignKey)
            {
                IsForeignKey = true;
                IsSystemGenerated = false;
            }
            else if (dataSourceType == DataSourceType.SystemGenerated)
            {
                IsForeignKey = false;
                IsSystemGenerated = true;
            }
            else if (dataSourceType == DataSourceType.DefaultValue)
            {
                IsForeignKey = false;
                IsSystemGenerated = false;
                DefaultValue = ktxtDefaultValue.Text.Trim();
            }
            else if (dataSourceType == DataSourceType.SelfReference)
            {
                IsForeignKey = false;
                IsSystemGenerated = false;
                DefaultValue = string.Empty;
            }
            else
            {
                IsForeignKey = false;
                IsSystemGenerated = false;
            }

            IsUniqueValue = kchkIsUniqueValue.Checked;
            IgnoreEmptyValue = kchkIgnoreEmptyValue.Checked;

            // 验证：如果既不是系统生成，又没有设置默认值，也不是外键或自身引用，需要提示
            if (dataSourceType == DataSourceType.Excel && string.IsNullOrWhiteSpace(DefaultValue))
            {
                var result = MessageBox.Show(
                    "该字段既不是系统生成的，也没有设置默认值，也没有特殊来源。\n" +
                    "如果没有Excel数据源，该字段将为空。\n\n" +
                    "确定要继续吗？",
                    "提示",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 是否为外键复选框点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kchkIsForeignKey_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        /// <summary>
        /// 是否系统生成复选框点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kchkIsSystemGenerated_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        /// <summary>
        /// 数据来源类型选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kcmbDataSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlStates();

            // 如果选择了自身引用，加载当前表的字段列表
            if (kcmbDataSourceType.SelectedIndex == (int)DataSourceType.SelfReference)
            {
                LoadSelfReferenceFields();
            }
        }

        /// <summary>
        /// 加载自身引用字段列表
        /// </summary>
        private void LoadSelfReferenceFields()
        {
            try
            {
                if (TargetEntityType == null)
                {
                    MessageBox.Show("未设置目标实体类型", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 获取字段信息
                var fieldNameList = UIHelper.GetFieldNameList(false, TargetEntityType);
                _fieldInfoDict = fieldNameList;

                // 清空并添加字段到下拉框
                kcmbSelfReferenceField.Items.Clear();
                foreach (var field in fieldNameList)
                {
                    kcmbSelfReferenceField.Items.Add(field.Value); // 添加中文名
                }

                // 如果有已选中的字段，保持选中状态
                if (!string.IsNullOrEmpty(SelfReferenceField?.Value))
                {
                    int index = kcmbSelfReferenceField.Items.IndexOf(SelfReferenceField?.Value);
                    if (index >= 0)
                    {
                        kcmbSelfReferenceField.SelectedIndex = index;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载自身引用字段失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 自身引用字段选择改变事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kcmbSelfReferenceField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kcmbSelfReferenceField.SelectedIndex < 0 || _fieldInfoDict == null)
            {
                return;
            }

            // 根据中文显示名称获取实际字段名
            string displayName = kcmbSelfReferenceField.SelectedItem.ToString();
            var field = _fieldInfoDict.FirstOrDefault(f => f.Value == displayName);
            if (field.Value != null)
            {
                SelfReferenceField = new SerializableKeyValuePair<string>(field.Key, field.Value);

                // 显示字段信息提示,类似外键关联的处理方式
                MessageBox.Show(
                    $"已选择自身引用字段:\n" +
                    $"字段名称: {field.Key}\n" +
                    $"显示名称: {field.Value}\n\n" +
                    $"说明: 当前字段将引用同一条记录的【{field.Value}】字段的值。",
                    "自身引用字段提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 获取表的中文显示名称
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>中文显示名称</returns>
        private string GetTableDisplayName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return string.Empty;

            var tableNames = new Dictionary<string, string>
            {
                { "tb_CustomerVendor", "客户供应商表" },
                { "tb_ProdCategories", "产品类目表" },
                { "tb_Prod", "产品基本信息表" },
                { "tb_ProdDetail", "产品详情信息表" },
                { "tb_ProdProperty", "产品属性表" },
                { "tb_ProdPropertyValue", "产品属性值表" }
            };

            return tableNames.ContainsKey(tableName) ? tableNames[tableName] : tableName;
        }
    }
}
