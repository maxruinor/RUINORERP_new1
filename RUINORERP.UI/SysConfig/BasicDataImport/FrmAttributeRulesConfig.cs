using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 属性规则配置窗体
    /// </summary>
    public partial class FrmAttributeRulesConfig : KryptonForm
    {
        /// <summary>
        /// Excel列名列表
        /// </summary>
        public List<string> ExcelColumns { get; set; }

        /// <summary>
        /// 系统字段列表
        /// </summary>
        public List<string> SystemFields { get; set; }

        /// <summary>
        /// 属性规则列表
        /// </summary>
        public List<AttributeExtractionRule> AttributeRules { get; private set; }

        public FrmAttributeRulesConfig(List<AttributeExtractionRule> existingRules, List<string> excelColumns, List<string> systemFields)
        {
            ExcelColumns = excelColumns ?? new List<string>();
            SystemFields = systemFields ?? new List<string>();
            AttributeRules = existingRules ?? new List<AttributeExtractionRule>();
            InitializeComponent();
            
            if (!this.DesignMode)
            {
                LoadRules();
            }
        }

        private void LoadRules()
        {
            dgvRules.DataSource = null;
            dgvRules.Columns.Clear();
            dgvRules.Columns.Add("AttributeName", "属性名称");
            dgvRules.Columns.Add("ExcelColumn", "Excel列名");
            dgvRules.Columns.Add("Pattern", "正则表达式");
            dgvRules.Columns.Add("IsRequired", "必填");

            foreach (var rule in AttributeRules)
            {
                dgvRules.Rows.Add(
                    rule.AttributeName,
                    string.IsNullOrEmpty(rule.ExcelColumn) ? "（从品名列提取）" : rule.ExcelColumn,
                    rule.Pattern,
                    rule.IsRequired ? "是" : "否"
                );
            }
        }

        private void kbtnAdd_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmAttributeRuleEdit(new AttributeExtractionRule(), ExcelColumns))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AttributeRules.Add(frm.Rule);
                    LoadRules();
                }
            }
        }

        private void kbtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRules.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要编辑的规则", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int index = dgvRules.SelectedRows[0].Index;
            var rule = AttributeRules[index];

            using (var frm = new FrmAttributeRuleEdit(rule, ExcelColumns))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AttributeRules[index] = frm.Rule;
                    LoadRules();
                }
            }
        }

        private void kbtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRules.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的规则", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("确定要删除选中的规则吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int index = dgvRules.SelectedRows[0].Index;
                AttributeRules.RemoveAt(index);
                LoadRules();
            }
        }

        private void dgvRules_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                kbtnEdit_Click(sender, e);
            }
        }

        private void kbtnAddDefaultRules_Click(object sender, EventArgs e)
        {
            // 添加常用属性规则
            var colorRule = new AttributeExtractionRule
            {
                AttributeName = "颜色",
                ExcelColumn = string.Empty, // 从品名列提取
                Pattern = @"颜色:\s*([\u4e00-\u9fa5\w\s\-\(\)]+)",
                GroupIndex = 1,
                IsRequired = true
            };

            var specRule = new AttributeExtractionRule
            {
                AttributeName = "规格",
                ExcelColumn = string.Empty, // 从品名列提取
                Pattern = @"规格:\s*([\u4e00-\u9fa5\w\s\-\(\)\*\u00d7]+)",
                GroupIndex = 1,
                IsRequired = false
            };

            AttributeRules.Add(colorRule);
            AttributeRules.Add(specRule);
            LoadRules();
            MessageBox.Show("已添加常用属性规则：颜色、规格", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
