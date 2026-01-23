using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.Model.ProductAttribute;

namespace RUINORERP.UI.ProductEAV
{
    /// <summary>
    /// 选择属性组合对话框
    /// 用于让用户选择需要生成的SKU组合
    /// </summary>
    public partial class frmSelectCombinations : KryptonForm
    {
        /// <summary>
        /// 供选择的属性组合列表
        /// </summary>
        private List<AttributeCombination> _availableCombinations;

        /// <summary>
        /// 用户选择的属性组合列表
        /// </summary>
        private List<AttributeCombination> _selectedCombinations;

        /// <summary>
        /// 获取用户选择的组合
        /// </summary>
        public List<AttributeCombination> SelectedCombinations
        {
            get { return _selectedCombinations; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="availableCombinations">供选择的组合列表</param>
        public frmSelectCombinations(List<AttributeCombination> availableCombinations)
        {
            InitializeComponent();
            _availableCombinations = availableCombinations ?? new List<AttributeCombination>();
            _selectedCombinations = new List<AttributeCombination>();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void frmSelectCombinations_Load(object sender, EventArgs e)
        {
            LoadCombinations();
        }

        /// <summary>
        /// 加载组合列表
        /// </summary>
        private void LoadCombinations()
        {
            dataGridViewCombinations.Rows.Clear();

            if (_availableCombinations == null || _availableCombinations.Count == 0)
            {
                return;
            }

            foreach (var combination in _availableCombinations)
            {
                int rowIndex = dataGridViewCombinations.Rows.Add();
                DataGridViewRow row = dataGridViewCombinations.Rows[rowIndex];

                // 选中列
                row.Cells[0].Value = false;

                // 组合文本列
                row.Cells[1].Value = GetPropertiesText(combination);

                // 存储组合对象
                row.Tag = combination;
            }
        }

        /// <summary>
        /// 根据属性组合生成显示文本
        /// </summary>
        /// <param name="combination">属性组合</param>
        /// <returns>显示文本</returns>
        private string GetPropertiesText(AttributeCombination combination)
        {
            if (combination == null || combination.Properties == null)
            {
                return string.Empty;
            }

            // 按 Property_ID 排序确保固定顺序
            var sortedProperties = combination.Properties
                .Where(p => p.Property != null && p.PropertyValue != null)
                .OrderBy(p => p.Property.Property_ID)
                .ThenBy(p => p.PropertyValue.PropertyValueID)
                .ToList();

            // 只显示属性值名称，不显示属性名称
            return string.Join(",", sortedProperties.Select(p => p.PropertyValue.PropertyValueName));
        }

        /// <summary>
        /// 全选按钮点击事件
        /// </summary>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewCombinations.Rows)
            {
                row.Cells[0].Value = true;
            }
        }

        /// <summary>
        /// 取消全选按钮点击事件
        /// </summary>
        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewCombinations.Rows)
            {
                row.Cells[0].Value = false;
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            _selectedCombinations.Clear();

            foreach (DataGridViewRow row in dataGridViewCombinations.Rows)
            {
                if (row.Cells[0].Value != null && (bool)row.Cells[0].Value)
                {
                    if (row.Tag is AttributeCombination combination)
                    {
                        _selectedCombinations.Add(combination);
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _selectedCombinations.Clear();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 组合文本列头点击事件（排序）
        /// </summary>
        private void dataGridViewCombinations_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                // 实现排序逻辑
                // 这里可以根据需要实现升序/降序切换
            }
        }

        /// <summary>
        /// 双击行事件 - 切换复选框状态
        /// </summary>
        private void dataGridViewCombinations_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cell = dataGridViewCombinations.Rows[e.RowIndex].Cells[0];
                if (cell.Value != null)
                {
                    cell.Value = !(bool)cell.Value;
                }
                else
                {
                    cell.Value = true;
                }
            }
        }
    }
}
