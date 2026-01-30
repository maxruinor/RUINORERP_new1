using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 多属性产品导入配置窗体
    /// </summary>
    public partial class FrmMultiAttributeImportConfig : KryptonForm
    {
        /// <summary>
        /// Excel列名列表
        /// </summary>
        public List<string> ExcelColumns { get; set; }

        /// <summary>
        /// 多属性导入配置
        /// </summary>
        public MultiAttributeImportConfig Config { get; private set; }

        /// <summary>
        /// 属性提取规则配置
        /// </summary>
        public List<AttributeExtractionRule> AttributeRules { get; private set; }

        /// <summary>
        /// SKU明细字段映射配置
        /// </summary>
        public List<SKUDetailFieldMapping> SKUMappings { get; private set; }

        public FrmMultiAttributeImportConfig()
        {
            InitializeComponent();
            Config = new MultiAttributeImportConfig();
            AttributeRules = new List<AttributeExtractionRule>();
            SKUMappings = new List<SKUDetailFieldMapping>();
        }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="excelColumns">Excel列名列表</param>
        /// <param name="existingConfig">现有配置（编辑模式）</param>
        public FrmMultiAttributeImportConfig(List<string> excelColumns, MultiAttributeImportConfig existingConfig = null)
            : this()
        {
            ExcelColumns = excelColumns ?? new List<string>();

            if (existingConfig != null)
            {
                LoadExistingConfig(existingConfig);
            }
            else
            {
                SetDefaultValues();
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmMultiAttributeImportConfig_Load(object sender, EventArgs e)
        {
            // 可以在这里添加更多UI初始化逻辑
        }

        /// <summary>
        /// 启用多属性复选框改变事件
        /// </summary>
        private void chkEnableMultiAttribute_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = chkEnableMultiAttribute.Checked;
            EnableControls(isEnabled);
        }

        /// <summary>
        /// 启用/禁用控件
        /// </summary>
        private void EnableControls(bool enabled)
        {
            txtProductNoColumn.Enabled = enabled;
            txtCNNameColumn.Enabled = enabled;
            txtSpecificationsColumn.Enabled = enabled;
            txtUnitColumn.Enabled = enabled;
            txtCNNameCleanupPattern.Enabled = enabled;
            txtGroupByField.Enabled = enabled;
        }

        /// <summary>
        /// 设置默认值
        /// </summary>
        private void SetDefaultValues()
        {
            // 默认不启用
            chkEnableMultiAttribute.Checked = false;
            EnableControls(false);

            // 设置默认品名清理规则
            txtCNNameCleanupPattern.Text = @"[\s]+\w+:\s*[\u4e00-\u9fa5\w\s\-\(\)]+$";

            // 尝试自动检测常见字段名
            AutoDetectColumns();
        }

        /// <summary>
        /// 自动检测常见字段名
        /// </summary>
        private void AutoDetectColumns()
        {
            if (ExcelColumns == null || ExcelColumns.Count == 0)
            {
                return;
            }

            // 品号检测
            var productNoColumns = ExcelColumns.Where(c => c.Contains("品号") || c.Contains("货号") || c.Contains("编码")).ToList();
            if (productNoColumns.Count > 0)
            {
                txtProductNoColumn.Text = productNoColumns[0];
            }

            // 品名检测
            var cnNameColumns = ExcelColumns.Where(c => c.Contains("品名") || c.Contains("名称")).ToList();
            if (cnNameColumns.Count > 0)
            {
                txtCNNameColumn.Text = cnNameColumns[0];
            }

            // 规格检测
            var specColumns = ExcelColumns.Where(c => c.Contains("规格")).ToList();
            if (specColumns.Count > 0)
            {
                txtSpecificationsColumn.Text = specColumns[0];
            }

            // 单位检测
            var unitColumns = ExcelColumns.Where(c => c.Contains("单位")).ToList();
            if (unitColumns.Count > 0)
            {
                txtUnitColumn.Text = unitColumns[0];
            }

            // 分组字段检测
            var groupColumns = ExcelColumns.Where(c => c.Contains("流水") || c.Contains("编号") || c.Contains("代码")).ToList();
            if (groupColumns.Count > 0)
            {
                txtGroupByField.Text = groupColumns[0];
            }
        }

        /// <summary>
        /// 加载现有配置
        /// </summary>
        private void LoadExistingConfig(MultiAttributeImportConfig existingConfig)
        {
            if (existingConfig == null)
            {
                return;
            }

            chkEnableMultiAttribute.Checked = existingConfig.IsEnabled;
            EnableControls(existingConfig.IsEnabled);

            if (existingConfig.BaseProductFields != null)
            {
                txtProductNoColumn.Text = existingConfig.BaseProductFields.ProductNoColumn ?? string.Empty;
                txtCNNameColumn.Text = existingConfig.BaseProductFields.CNNameColumn ?? string.Empty;
                txtSpecificationsColumn.Text = existingConfig.BaseProductFields.SpecificationsColumn ?? string.Empty;
                txtUnitColumn.Text = existingConfig.BaseProductFields.UnitColumn ?? string.Empty;
                txtCNNameCleanupPattern.Text = existingConfig.BaseProductFields.CNNameCleanupPattern ?? string.Empty;
            }

            txtGroupByField.Text = existingConfig.GroupByField ?? string.Empty;

            // 保存属性规则和SKU映射（用于后续配置）
            AttributeRules = existingConfig.AttributeExtractionRules ?? new List<AttributeExtractionRule>();
            SKUMappings = existingConfig.SKUDetailFields ?? new List<SKUDetailFieldMapping>();
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void kbtnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            BuildConfig();
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
        /// 验证输入
        /// </summary>
        private bool ValidateInput()
        {
            if (chkEnableMultiAttribute.Checked)
            {
                if (string.IsNullOrEmpty(txtGroupByField.Text.Trim()))
                {
                    MessageBox.Show("请输入产品分组字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGroupByField.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(txtProductNoColumn.Text.Trim()))
                {
                    MessageBox.Show("请输入品号列名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProductNoColumn.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 构建配置对象
        /// </summary>
        private void BuildConfig()
        {
            Config.IsEnabled = chkEnableMultiAttribute.Checked;

            if (Config.IsEnabled)
            {
                Config.GroupByField = txtGroupByField.Text.Trim();

                Config.BaseProductFields = new BaseProductFieldMapping
                {
                    ProductNoColumn = txtProductNoColumn.Text.Trim(),
                    CNNameColumn = txtCNNameColumn.Text.Trim(),
                    SpecificationsColumn = txtSpecificationsColumn.Text.Trim(),
                    UnitColumn = txtUnitColumn.Text.Trim(),
                    CNNameCleanupPattern = txtCNNameCleanupPattern.Text.Trim()
                };

                Config.AttributeExtractionRules = AttributeRules;
                Config.SKUDetailFields = SKUMappings;
            }
            else
            {
                Config.GroupByField = null;
                Config.BaseProductFields = null;
                Config.AttributeExtractionRules = null;
                Config.SKUDetailFields = null;
            }
        }

        /// <summary>
        /// 配置属性提取规则
        /// </summary>
        public void ConfigureAttributeRules(List<string> systemFields)
        {
            using (var frm = new FrmAttributeRulesConfig(AttributeRules, ExcelColumns, systemFields))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AttributeRules = frm.AttributeRules;
                }
            }
        }

        /// <summary>
        /// 配置SKU明细字段映射
        /// </summary>
        public void ConfigureSKUMappings(List<string> systemFields)
        {
            // TODO: 实现SKU明细字段映射配置窗体
            // using (var frm = new FrmSKUMappingConfig(SKUMappings, ExcelColumns, systemFields))
            // {
            //     if (frm.ShowDialog() == DialogResult.OK)
            //     {
            //         SKUMappings = frm.SKUMappings;
            //     }
            // }
        }
    }
}
