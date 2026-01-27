using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.UI.Common;

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
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是否系统生成
        /// </summary>
        public bool IsSystemGenerated { get; set; }

        /// <summary>
        /// 关联表名
        /// </summary>
        public string RelatedTableName { get; set; }

        /// <summary>
        /// 关联表字段（中文名）
        /// </summary>
        public string RelatedTableField { get; set; }

        /// <summary>
        /// 关联表字段（实际字段名）
        /// </summary>
        public string RelatedTableFieldName { get; set; }

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
                kchkIsSystemGenerated.Checked = CurrentMapping.IsSystemGenerated;
                ktxtDefaultValue.Text = CurrentMapping.DefaultValue ?? string.Empty;

                IsForeignKey = CurrentMapping.IsForeignKey;
                IsUniqueValue = CurrentMapping.IsUniqueValue;
                DefaultValue = CurrentMapping.DefaultValue;
                IsSystemGenerated = CurrentMapping.IsSystemGenerated;

                // 初始化关联表信息
                if (!string.IsNullOrEmpty(CurrentMapping.RelatedTableName))
                {
                    // 查找对应的显示文本
                    for (int i = 0; i < kcmbRelatedTable.Items.Count; i++)
                    {
                        string itemText = kcmbRelatedTable.Items[i].ToString();
                        if (itemText.Contains(CurrentMapping.RelatedTableName))
                        {
                            kcmbRelatedTable.SelectedIndex = i;
                            break;
                        }
                    }
                }

                RelatedTableName = CurrentMapping.RelatedTableName;
                RelatedTableField = CurrentMapping.RelatedTableField;
                ktxtRelatedField.Text = CurrentMapping.RelatedTableField;
            }

            // 更新控件状态
            UpdateControlStates();
        }

        /// <summary>
        /// 更新控件状态
        /// </summary>
        private void UpdateControlStates()
        {
            bool isForeignKey = kchkIsForeignKey.Checked;
            kcmbRelatedTable.Enabled = isForeignKey;
            ktxtRelatedField.Enabled = isForeignKey;

            // 系统生成的字段不能设置默认值
            ktxtDefaultValue.Enabled = !kchkIsSystemGenerated.Checked;
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
                if (!string.IsNullOrEmpty(RelatedTableField))
                {
                    int index = ktxtRelatedField.Items.IndexOf(RelatedTableField);
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
            RelatedTableFieldName = field.Key;
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void kbtnOK_Click(object sender, EventArgs e)
        {
            // 如果是外键，验证关联表和字段
            if (kchkIsForeignKey.Checked)
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
                if (startIndex > 0 && endIndex > startIndex)
                {
                    RelatedTableName = selectedTable.Substring(startIndex, endIndex - startIndex);
                }
                else
                {
                    RelatedTableName = selectedTable;
                }

                RelatedTableField = ktxtRelatedField.SelectedItem?.ToString() ?? ktxtRelatedField.Text;
            }
            else
            {
                RelatedTableName = string.Empty;
                RelatedTableField = string.Empty;
                RelatedTableFieldName = string.Empty;
            }

            IsForeignKey = kchkIsForeignKey.Checked;
            IsUniqueValue = kchkIsUniqueValue.Checked;
            DefaultValue = ktxtDefaultValue.Text.Trim();
            IsSystemGenerated = kchkIsSystemGenerated.Checked;

            // 验证：如果既不是系统生成，又没有设置默认值，也没有外键，需要提示
            if (!IsSystemGenerated && !IsForeignKey && string.IsNullOrWhiteSpace(DefaultValue))
            {
                var result = MessageBox.Show(
                    "该字段既不是系统生成的，也没有设置默认值。\n" +
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

            // 如果是系统生成，清空默认值
            if (IsSystemGenerated)
            {
                DefaultValue = string.Empty;
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
    }
}
