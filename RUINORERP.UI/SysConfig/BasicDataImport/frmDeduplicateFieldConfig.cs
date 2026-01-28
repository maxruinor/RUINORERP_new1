using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.Common;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 去重字段配置对话框
    /// 用于配置数据导入时的去重字段和策略
    /// </summary>
    public partial class frmDeduplicateFieldConfig : KryptonForm
    {
        /// <summary>
        /// 可用的字段列表
        /// </summary>
        public List<SerializableKeyValuePair<string>> AvailableFields { get; set; }

        /// <summary>
        /// 已选择的去重字段列表
        /// </summary>
        public List<string> SelectedFields { get; set; }

        /// <summary>
        /// 是否忽略空值
        /// </summary>
        public bool IgnoreEmptyValues { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmDeduplicateFieldConfig()
        {
            InitializeComponent();
            AvailableFields = new List<SerializableKeyValuePair<string>>();
            SelectedFields = new List<string>();
            IgnoreEmptyValues = true;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void frmDeduplicateFieldConfig_Load(object sender, EventArgs e)
        {
            try
            {
                // 清空列表
                chkListAvailableFields.Items.Clear();
                listBoxSelectedFields.Items.Clear();

                // 加载可用字段
                foreach (var field in AvailableFields)
                {
                    if (field != null && !string.IsNullOrEmpty(field.Key))
                    {
                        // 检查是否已选中
                        bool isSelected = SelectedFields.Contains(field.Key);
                        chkListAvailableFields.Items.Add(field.Value, isSelected);
                    }
                }

                // 加载已选字段
                RefreshSelectedFieldsList();

                // 初始化忽略空值复选框
                chkIgnoreEmptyValues.Checked = IgnoreEmptyValues;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化窗体失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新已选字段列表
        /// </summary>
        private void RefreshSelectedFieldsList()
        {
            listBoxSelectedFields.Items.Clear();
            foreach (var field in AvailableFields)
            {
                if (field != null && SelectedFields.Contains(field.Key))
                {
                    listBoxSelectedFields.Items.Add(field.Value);
                }
            }
        }

        /// <summary>
        /// 全选按钮点击事件
        /// </summary>
        private void kbtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListAvailableFields.Items.Count; i++)
            {
                chkListAvailableFields.SetItemCheckState(i, CheckState.Checked);
            }
            UpdateSelectedFields();
        }

        /// <summary>
        /// 全不选按钮点击事件
        /// </summary>
        private void kbtnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListAvailableFields.Items.Count; i++)
            {
                chkListAvailableFields.SetItemCheckState(i, CheckState.Unchecked);
            }
            UpdateSelectedFields();
        }

        /// <summary>
        /// 可用字段复选框改变事件
        /// </summary>
        private void chkListAvailableFields_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // 使用BeginInvoke确保在事件处理完成后执行
            this.BeginInvoke(new Action(() =>
            {
                UpdateSelectedFields();
            }));
        }

        /// <summary>
        /// 更新已选字段列表
        /// </summary>
        private void UpdateSelectedFields()
        {
            SelectedFields.Clear();

            for (int i = 0; i < chkListAvailableFields.Items.Count; i++)
            {
                if (chkListAvailableFields.GetItemCheckState(i) == CheckState.Checked)
                {
                    string displayText = chkListAvailableFields.Items[i].ToString();
                    // 找到对应的Key
                    var field = AvailableFields.FirstOrDefault(f => f.Value == displayText);
                    if (field != null)
                    {
                        SelectedFields.Add(field.Key);
                    }
                }
            }

            RefreshSelectedFieldsList();

            // 更新状态标签
            klblFieldCount.Text = $"已选择 {SelectedFields.Count} 个去重字段";
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void kbtnOK_Click(object sender, EventArgs e)
        {
            if (SelectedFields.Count == 0)
            {
                MessageBox.Show("请至少选择一个去重字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IgnoreEmptyValues = chkIgnoreEmptyValues.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void kbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 忽略空值复选框改变事件
        /// </summary>
        private void chkIgnoreEmptyValues_CheckedChanged(object sender, EventArgs e)
        {
            IgnoreEmptyValues = chkIgnoreEmptyValues.Checked;
        }
    }
}
