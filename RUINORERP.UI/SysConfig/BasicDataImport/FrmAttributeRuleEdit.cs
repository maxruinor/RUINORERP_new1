using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 属性规则编辑窗体
    /// </summary>
    public partial class FrmAttributeRuleEdit : KryptonForm
    {
        /// <summary>
        /// Excel列名列表
        /// </summary>
        public List<string> ExcelColumns { get; set; }

        /// <summary>
        /// 属性规则
        /// </summary>
        public AttributeExtractionRule Rule { get; private set; }

        /// <summary>
        /// 正则表达式测试结果
        /// </summary>
        private List<string> TestResults { get; set; }

        public FrmAttributeRuleEdit(AttributeExtractionRule rule, List<string> excelColumns)
        {
            ExcelColumns = excelColumns ?? new List<string>();
            Rule = rule ?? new AttributeExtractionRule();
            TestResults = new List<string>();
            InitializeComponent();
            LoadExistingRule();
        }

        private void LoadExistingRule()
        {
            txtAttributeName.Text = Rule.AttributeName ?? string.Empty;
            txtRegexPattern.Text = Rule.Pattern ?? string.Empty;
            txtTestText.Text = "8连长方肥皂蛋糕模紫色 颜色: 紫色 规格: 2PCS";
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            string testInput = txtTestText.Text;
            string pattern = txtRegexPattern.Text;

            if (string.IsNullOrEmpty(pattern))
            {
                MessageBox.Show("请输入正则表达式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var rule = new AttributeExtractionRule
                {
                    Pattern = pattern,
                    GroupIndex = 1
                };

                string result = rule.ExtractValue(testInput);
                txtTestResult.Text = result ?? "（未匹配）";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"正则表达式错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAttributeName.Text.Trim()))
            {
                MessageBox.Show("请输入属性名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAttributeName.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrEmpty(txtRegexPattern.Text.Trim()))
            {
                MessageBox.Show("请输入正则表达式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRegexPattern.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            Rule.AttributeName = txtAttributeName.Text.Trim();
            Rule.Pattern = txtRegexPattern.Text.Trim();
            Rule.GroupIndex = 1;
        }
    }
}
