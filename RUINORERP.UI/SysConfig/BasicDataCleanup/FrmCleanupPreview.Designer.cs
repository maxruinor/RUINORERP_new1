namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    partial class FrmCleanupPreview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kbtnClose = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel2 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonGroupBox3 = new Krypton.Toolkit.KryptonGroupBox();
            this.dgvDetailPreview = new System.Windows.Forms.DataGridView();
            this.kryptonGroupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            this.dgvSummary = new System.Windows.Forms.DataGridView();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.dgvRuleStats = new System.Windows.Forms.DataGridView();
            this.colRuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonPanel3 = new Krypton.Toolkit.KryptonPanel();
            this.klblPreviewTime = new Krypton.Toolkit.KryptonLabel();
            this.klblTotalRecords = new Krypton.Toolkit.KryptonLabel();
            this.klblConfigName = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3.Panel)).BeginInit();
            this.kryptonGroupBox3.Panel.SuspendLayout();
            this.kryptonGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetailPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRuleStats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel3)).BeginInit();
            this.kryptonPanel3.SuspendLayout();
            this.SuspendLayout();
            //
            // kryptonPanel1
            //
            this.kryptonPanel1.Controls.Add(this.kbtnClose);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 650);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(900, 60);
            this.kryptonPanel1.TabIndex = 0;
            //
            // kbtnClose
            //
            this.kbtnClose.Location = new System.Drawing.Point(405, 16);
            this.kbtnClose.Name = "kbtnClose";
            this.kbtnClose.Size = new System.Drawing.Size(90, 28);
            this.kbtnClose.TabIndex = 0;
            this.kbtnClose.Values.Text = "关闭";
            this.kbtnClose.Click += new System.EventHandler(this.KbtnClose_Click);
            //
            // kryptonPanel2
            //
            this.kryptonPanel2.Controls.Add(this.kryptonGroupBox3);
            this.kryptonPanel2.Controls.Add(this.kryptonGroupBox2);
            this.kryptonPanel2.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel2.Controls.Add(this.kryptonPanel3);
            this.kryptonPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel2.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(900, 650);
            this.kryptonPanel2.TabIndex = 1;
            //
            // kryptonGroupBox3
            //
            this.kryptonGroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox3.Location = new System.Drawing.Point(0, 400);
            this.kryptonGroupBox3.Name = "kryptonGroupBox3";
            //
            // kryptonGroupBox3.Panel
            //
            this.kryptonGroupBox3.Panel.Controls.Add(this.dgvDetailPreview);
            this.kryptonGroupBox3.Size = new System.Drawing.Size(900, 250);
            this.kryptonGroupBox3.TabIndex = 3;
            this.kryptonGroupBox3.Values.Heading = "详细数据预览";
            //
            // dgvDetailPreview
            //
            this.dgvDetailPreview.AllowUserToAddRows = false;
            this.dgvDetailPreview.AllowUserToDeleteRows = false;
            this.dgvDetailPreview.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetailPreview.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDetailPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDetailPreview.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDetailPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetailPreview.Location = new System.Drawing.Point(0, 0);
            this.dgvDetailPreview.Name = "dgvDetailPreview";
            this.dgvDetailPreview.ReadOnly = true;
            this.dgvDetailPreview.RowTemplate.Height = 23;
            this.dgvDetailPreview.Size = new System.Drawing.Size(896, 226);
            this.dgvDetailPreview.TabIndex = 0;
            //
            // kryptonGroupBox2
            //
            this.kryptonGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(0, 280);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            //
            // kryptonGroupBox2.Panel
            //
            this.kryptonGroupBox2.Panel.Controls.Add(this.dgvSummary);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(900, 120);
            this.kryptonGroupBox2.TabIndex = 2;
            this.kryptonGroupBox2.Values.Heading = "操作摘要";
            //
            // dgvSummary
            //
            this.dgvSummary.AllowUserToAddRows = false;
            this.dgvSummary.AllowUserToDeleteRows = false;
            this.dgvSummary.BackgroundColor = System.Drawing.Color.White;
            this.dgvSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSummary.Location = new System.Drawing.Point(0, 0);
            this.dgvSummary.Name = "dgvSummary";
            this.dgvSummary.ReadOnly = true;
            this.dgvSummary.RowTemplate.Height = 23;
            this.dgvSummary.Size = new System.Drawing.Size(896, 96);
            this.dgvSummary.TabIndex = 0;
            //
            // kryptonGroupBox1
            //
            this.kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 80);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            //
            // kryptonGroupBox1.Panel
            //
            this.kryptonGroupBox1.Panel.Controls.Add(this.dgvRuleStats);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(900, 200);
            this.kryptonGroupBox1.TabIndex = 1;
            this.kryptonGroupBox1.Values.Heading = "规则匹配统计";
            //
            // dgvRuleStats
            //
            this.dgvRuleStats.AllowUserToAddRows = false;
            this.dgvRuleStats.AllowUserToDeleteRows = false;
            this.dgvRuleStats.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRuleStats.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRuleStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRuleStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRuleName,
            this.colMatchCount,
            this.colPercentage});
            this.dgvRuleStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRuleStats.Location = new System.Drawing.Point(0, 0);
            this.dgvRuleStats.Name = "dgvRuleStats";
            this.dgvRuleStats.ReadOnly = true;
            this.dgvRuleStats.RowTemplate.Height = 23;
            this.dgvRuleStats.Size = new System.Drawing.Size(896, 176);
            this.dgvRuleStats.TabIndex = 0;
            //
            // colRuleName
            //
            this.colRuleName.HeaderText = "规则名称";
            this.colRuleName.Name = "colRuleName";
            this.colRuleName.ReadOnly = true;
            this.colRuleName.Width = 300;
            //
            // colMatchCount
            //
            this.colMatchCount.HeaderText = "匹配记录数";
            this.colMatchCount.Name = "colMatchCount";
            this.colMatchCount.ReadOnly = true;
            this.colMatchCount.Width = 150;
            //
            // colPercentage
            //
            this.colPercentage.HeaderText = "占比";
            this.colPercentage.Name = "colPercentage";
            this.colPercentage.ReadOnly = true;
            this.colPercentage.Width = 150;
            //
            // kryptonPanel3
            //
            this.kryptonPanel3.Controls.Add(this.klblPreviewTime);
            this.kryptonPanel3.Controls.Add(this.klblTotalRecords);
            this.kryptonPanel3.Controls.Add(this.klblConfigName);
            this.kryptonPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanel3.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel3.Name = "kryptonPanel3";
            this.kryptonPanel3.Size = new System.Drawing.Size(900, 80);
            this.kryptonPanel3.TabIndex = 0;
            //
            // klblPreviewTime
            //
            this.klblPreviewTime.Location = new System.Drawing.Point(20, 50);
            this.klblPreviewTime.Name = "klblPreviewTime";
            this.klblPreviewTime.Size = new System.Drawing.Size(75, 20);
            this.klblPreviewTime.TabIndex = 2;
            this.klblPreviewTime.Values.Text = "预览时间:";
            //
            // klblTotalRecords
            //
            this.klblTotalRecords.Location = new System.Drawing.Point(350, 20);
            this.klblTotalRecords.Name = "klblTotalRecords";
            this.klblTotalRecords.Size = new System.Drawing.Size(75, 20);
            this.klblTotalRecords.TabIndex = 1;
            this.klblTotalRecords.Values.Text = "总记录数:";
            //
            // klblConfigName
            //
            this.klblConfigName.Location = new System.Drawing.Point(20, 20);
            this.klblConfigName.Name = "klblConfigName";
            this.klblConfigName.Size = new System.Drawing.Size(75, 20);
            this.klblConfigName.TabIndex = 0;
            this.klblConfigName.Values.Text = "配置名称:";
            //
            // FrmCleanupPreview
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 710);
            this.Controls.Add(this.kryptonPanel2);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCleanupPreview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据清理预览";
            this.Load += new System.EventHandler(this.FrmCleanupPreview_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3.Panel)).EndInit();
            this.kryptonGroupBox3.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3)).EndInit();
            this.kryptonGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetailPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRuleStats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel3)).EndInit();
            this.kryptonPanel3.ResumeLayout(false);
            this.kryptonPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kbtnClose;
        private Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private Krypton.Toolkit.KryptonPanel kryptonPanel3;
        private Krypton.Toolkit.KryptonLabel klblConfigName;
        private Krypton.Toolkit.KryptonLabel klblTotalRecords;
        private Krypton.Toolkit.KryptonLabel klblPreviewTime;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private System.Windows.Forms.DataGridView dgvRuleStats;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatchCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercentage;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private System.Windows.Forms.DataGridView dgvSummary;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox3;
        private System.Windows.Forms.DataGridView dgvDetailPreview;
    }
}
