using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 去重字段项（支持双类别：系统字段 / Excel源列）
    /// </summary>
    public class DeduplicateFieldItem
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
    }

    /// <summary>
    /// 去重字段配置对话框
    /// 用于配置数据导入时的去重字段和策略
    /// 支持同时选择系统字段和Excel源列作为去重依据
    /// </summary>
    public partial class frmDeduplicateFieldConfig : KryptonForm
    {
        private List<DeduplicateFieldItem> _allFields = new List<DeduplicateFieldItem>();

        private const string CAT_SYSTEM = "系统字段";
        private const string CAT_EXCEL = "Excel源列";

        public List<DeduplicateFieldItem> AvailableFields
        {
            get => _allFields;
            set => _allFields = value ?? new List<DeduplicateFieldItem>();
        }

        public List<string> SelectedFields { get; set; } = new List<string>();
        public bool IgnoreEmptyValues { get; set; } = true;

        public frmDeduplicateFieldConfig()
        {
            InitializeComponent();
        }

        private void frmDeduplicateFieldConfig_Load(object sender, EventArgs e)
        {
            chkListAvailableFields.Items.Clear();
            listBoxSelectedFields.Items.Clear();

            var systemFields = _allFields.Where(f => f.Category == CAT_SYSTEM).ToList();
            var excelFields = _allFields.Where(f => f.Category == CAT_EXCEL).ToList();

            if (systemFields.Any())
                AddCategoryHeader(CAT_SYSTEM);

            foreach (var field in systemFields)
            {
                if (!string.IsNullOrEmpty(field?.Key))
                    chkListAvailableFields.Items.Add(field, SelectedFields.Contains(field.Key));
            }

            if (excelFields.Any())
                AddCategoryHeader(CAT_EXCEL);

            foreach (var field in excelFields)
            {
                if (!string.IsNullOrEmpty(field?.Key))
                    chkListAvailableFields.Items.Add(field, SelectedFields.Contains(field.Key));
            }

            RefreshSelectedFieldsList();
            chkIgnoreEmptyValues.Checked = IgnoreEmptyValues;
        }

        private void AddCategoryHeader(string categoryText)
        {
            chkListAvailableFields.Items.Add(new CategoryHeaderItem(categoryText));
        }

        private void RefreshSelectedFieldsList()
        {
            listBoxSelectedFields.Items.Clear();
            foreach (var item in _allFields)
            {
                if (item != null && SelectedFields.Contains(item.Key))
                    listBoxSelectedFields.Items.Add($"{item.DisplayName} [{item.Category}]");
            }
        }

        private void kbtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListAvailableFields.Items.Count; i++)
            {
                if (!(chkListAvailableFields.Items[i] is CategoryHeaderItem))
                    chkListAvailableFields.SetItemCheckState(i, CheckState.Checked);
            }
            UpdateSelectedFields();
        }

        private void kbtnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListAvailableFields.Items.Count; i++)
            {
                if (!(chkListAvailableFields.Items[i] is CategoryHeaderItem))
                    chkListAvailableFields.SetItemCheckState(i, CheckState.Unchecked);
            }
            UpdateSelectedFields();
        }

        private void chkListAvailableFields_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (chkListAvailableFields.Items[e.Index] is CategoryHeaderItem)
            {
                e.NewValue = CheckState.Unchecked;
                return;
            }
            this.BeginInvoke(new Action(() => UpdateSelectedFields()));
        }

        private void UpdateSelectedFields()
        {
            SelectedFields.Clear();
            for (int i = 0; i < chkListAvailableFields.Items.Count; i++)
            {
                if (chkListAvailableFields.GetItemCheckState(i) == CheckState.Checked &&
                    chkListAvailableFields.Items[i] is DeduplicateFieldItem field)
                    SelectedFields.Add(field.Key);
            }
            RefreshSelectedFieldsList();
            klblFieldCount.Text = $"已选择 {SelectedFields.Count} 个去重字段";
        }

        private void kbtnOK_Click(object sender, EventArgs e)
        {
            if (SelectedFields.Count == 0)
            {
                MessageBox.Show("请至少选择一个去重字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            IgnoreEmptyValues = chkIgnoreEmptyValues.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void kbtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void chkIgnoreEmptyValues_CheckedChanged(object sender, EventArgs e)
        {
            IgnoreEmptyValues = chkIgnoreEmptyValues.Checked;
        }
    }

    internal class CategoryHeaderItem
    {
        public string Text { get; }
        public CategoryHeaderItem(string text) => Text = text;
        public override string ToString() => $"【{Text}】";
    }
}
