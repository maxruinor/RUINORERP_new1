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

        private KryptonPanel kryptonPanel1;
        private DataGridView dgvRules;
        private KryptonGroupBox kryptonGroupBox1;
        private KryptonButton kbtnAdd;
        private KryptonButton kbtnEdit;
        private KryptonButton kbtnDelete;
        private KryptonGroupBox kryptonGroupBox2;
        private KryptonButton kbtnAddDefaultRules;
        private KryptonButton kbtnOK;
        private KryptonButton kbtnCancel;

        public FrmAttributeRulesConfig(List<AttributeExtractionRule> existingRules, List<string> excelColumns, List<string> systemFields)
        {
            ExcelColumns = excelColumns ?? new List<string>();
            SystemFields = systemFields ?? new List<string>();
            AttributeRules = existingRules ?? new List<AttributeExtractionRule>();
            InitializeComponent();
            LoadRules();
        }

        private void InitializeComponent()
        {
            this.kryptonPanel1 = new KryptonPanel();
            this.dgvRules = new DataGridView();
            this.kryptonGroupBox1 = new KryptonGroupBox();
            this.kbtnAdd = new KryptonButton();
            this.kbtnEdit = new KryptonButton();
            this.kbtnDelete = new KryptonButton();
            this.kryptonGroupBox2 = new KryptonGroupBox();
            this.kbtnAddDefaultRules = new KryptonButton();
            this.kbtnOK = new KryptonButton();
            this.kbtnCancel = new KryptonButton();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRules)).BeginInit();
            this.kryptonGroupBox2.SuspendLayout();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();

            // kryptonPanel1
            this.kryptonPanel1.Controls.Add(this.kbtnOK);
            this.kryptonPanel1.Controls.Add(this.kbtnCancel);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox2);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(684, 461);
            this.kryptonPanel1.TabIndex = 0;

            // dgvRules
            this.dgvRules.AllowUserToAddRows = false;
            this.dgvRules.AllowUserToDeleteRows = false;
            this.dgvRules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRules.Location = new System.Drawing.Point(3, 17);
            this.dgvRules.Name = "dgvRules";
            this.dgvRules.RowHeadersVisible = false;
            this.dgvRules.RowTemplate.Height = 23;
            this.dgvRules.Size = new System.Drawing.Size(678, 180);
            this.dgvRules.TabIndex = 0;
            this.dgvRules.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvRules_CellDoubleClick);

            // kryptonGroupBox1
            this.kryptonGroupBox1.Controls.Add(this.dgvRules);
            this.kryptonGroupBox1.Controls.Add(this.kbtnAdd);
            this.kryptonGroupBox1.Controls.Add(this.kbtnEdit);
            this.kryptonGroupBox1.Controls.Add(this.kbtnDelete);
            this.kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            this.kryptonGroupBox1.Size = new System.Drawing.Size(684, 380);
            this.kryptonGroupBox1.TabIndex = 0;
            this.kryptonGroupBox1.Values.Heading = "属性提取规则";

            // kbtnAdd
            this.kbtnAdd.Location = new System.Drawing.Point(15, 205);
            this.kbtnAdd.Name = "kbtnAdd";
            this.kbtnAdd.Size = new System.Drawing.Size(75, 23);
            this.kbtnAdd.TabIndex = 1;
            this.kbtnAdd.Values.Text = "添加";
            this.kbtnAdd.Click += new EventHandler(this.kbtnAdd_Click);

            // kbtnEdit
            this.kbtnEdit.Location = new System.Drawing.Point(96, 205);
            this.kbtnEdit.Name = "kbtnEdit";
            this.kbtnEdit.Size = new System.Drawing.Size(75, 23);
            this.kbtnEdit.TabIndex = 2;
            this.kbtnEdit.Values.Text = "编辑";
            this.kbtnEdit.Click += new EventHandler(this.kbtnEdit_Click);

            // kbtnDelete
            this.kbtnDelete.Location = new System.Drawing.Point(177, 205);
            this.kbtnDelete.Name = "kbtnDelete";
            this.kbtnDelete.Size = new System.Drawing.Size(75, 23);
            this.kbtnDelete.TabIndex = 3;
            this.kbtnDelete.Values.Text = "删除";
            this.kbtnDelete.Click += new EventHandler(this.kbtnDelete_Click);

            // kryptonGroupBox2
            this.kryptonGroupBox2.Controls.Add(this.kbtnAddDefaultRules);
            this.kryptonGroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(0, 380);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            this.kryptonGroupBox2.Size = new System.Drawing.Size(684, 60);
            this.kryptonGroupBox2.TabIndex = 1;
            this.kryptonGroupBox2.Values.Heading = "快速配置";

            // kbtnAddDefaultRules
            this.kbtnAddDefaultRules.Location = new System.Drawing.Point(15, 20);
            this.kbtnAddDefaultRules.Name = "kbtnAddDefaultRules";
            this.kbtnAddDefaultRules.Size = new System.Drawing.Size(200, 23);
            this.kbtnAddDefaultRules.TabIndex = 0;
            this.kbtnAddDefaultRules.Values.Text = "添加常用属性规则（颜色、规格）";
            this.kbtnAddDefaultRules.Click += new EventHandler(this.kbtnAddDefaultRules_Click);

            // kbtnOK
            this.kbtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kbtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.kbtnOK.Location = new System.Drawing.Point(524, 426);
            this.kbtnOK.Name = "kbtnOK";
            this.kbtnOK.Size = new System.Drawing.Size(75, 23);
            this.kbtnOK.TabIndex = 4;
            this.kbtnOK.Values.Text = "确定";

            // kbtnCancel
            this.kbtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kbtnCancel.Location = new System.Drawing.Point(609, 426);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(75, 23);
            this.kbtnCancel.TabIndex = 5;
            this.kbtnCancel.Values.Text = "取消";

            // FrmAttributeRulesConfig
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAttributeRulesConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "属性提取规则配置";
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRules)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            this.kryptonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
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
