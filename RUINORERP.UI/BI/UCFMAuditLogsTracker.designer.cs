namespace RUINORERP.UI.BI
{
    partial class UCFMAuditLogsTracker
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.btnAnalysis = new Krypton.Toolkit.KryptonButton();
            this.txtProdNo = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.lblObjectNo = new Krypton.Toolkit.KryptonLabel();
            this.txtProdName = new Krypton.Toolkit.KryptonTextBox();
            this.txtObjectType = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.lblObjectType = new Krypton.Toolkit.KryptonLabel();
            this.txtSKU = new Krypton.Toolkit.KryptonTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(278, 720);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(525, 720);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonSplitContainer1);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(977, 757);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            this.kryptonSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer1.Panel1
            // 
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.btnAnalysis);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.txtProdNo);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonLabel2);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblObjectNo);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.txtProdName);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.txtObjectType);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonLabel1);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblObjectType);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.txtSKU);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(977, 757);
            this.kryptonSplitContainer1.SplitterDistance = 136;
            this.kryptonSplitContainer1.TabIndex = 40;
            // 
            // btnAnalysis
            // 
            this.btnAnalysis.Location = new System.Drawing.Point(861, 29);
            this.btnAnalysis.Name = "btnAnalysis";
            this.btnAnalysis.Size = new System.Drawing.Size(90, 25);
            this.btnAnalysis.TabIndex = 40;
            this.btnAnalysis.Values.Text = "分析数据";
            this.btnAnalysis.Click += new System.EventHandler(this.btnAnalysis_Click);
            // 
            // txtProdNo
            // 
            this.txtProdNo.Location = new System.Drawing.Point(367, 28);
            this.txtProdNo.Name = "txtProdNo";
            this.txtProdNo.Size = new System.Drawing.Size(160, 23);
            this.txtProdNo.TabIndex = 25;
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(294, 58);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(49, 20);
            this.kryptonLabel2.TabIndex = 38;
            this.kryptonLabel2.Values.Text = "品名：";
            // 
            // lblObjectNo
            // 
            this.lblObjectNo.Location = new System.Drawing.Point(294, 29);
            this.lblObjectNo.Name = "lblObjectNo";
            this.lblObjectNo.Size = new System.Drawing.Size(49, 20);
            this.lblObjectNo.TabIndex = 24;
            this.lblObjectNo.Values.Text = "品号： ";
            // 
            // txtProdName
            // 
            this.txtProdName.Location = new System.Drawing.Point(367, 57);
            this.txtProdName.Name = "txtProdName";
            this.txtProdName.Size = new System.Drawing.Size(160, 23);
            this.txtProdName.TabIndex = 39;
            // 
            // txtObjectType
            // 
            this.txtObjectType.Location = new System.Drawing.Point(106, 26);
            this.txtObjectType.Name = "txtObjectType";
            this.txtObjectType.Size = new System.Drawing.Size(160, 23);
            this.txtObjectType.TabIndex = 21;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(578, 30);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(46, 20);
            this.kryptonLabel1.TabIndex = 36;
            this.kryptonLabel1.Values.Text = "SKU码";
            // 
            // lblObjectType
            // 
            this.lblObjectType.Location = new System.Drawing.Point(33, 27);
            this.lblObjectType.Name = "lblObjectType";
            this.lblObjectType.Size = new System.Drawing.Size(62, 20);
            this.lblObjectType.TabIndex = 20;
            this.lblObjectType.Values.Text = "单据类型";
            // 
            // txtSKU
            // 
            this.txtSKU.Location = new System.Drawing.Point(651, 29);
            this.txtSKU.Name = "txtSKU";
            this.txtSKU.Size = new System.Drawing.Size(160, 23);
            this.txtSKU.TabIndex = 37;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(977, 616);
            this.dataGridView1.TabIndex = 0;
            // 
            // UCAuditLogsTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 757);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCAuditLogsTracker";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            this.kryptonSplitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblObjectType;
        private Krypton.Toolkit.KryptonTextBox txtObjectType;
        private Krypton.Toolkit.KryptonLabel lblObjectNo;
        private Krypton.Toolkit.KryptonTextBox txtProdNo;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonTextBox txtSKU;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonTextBox txtProdName;
        private System.Windows.Forms.BindingSource bindingSource1;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Krypton.Toolkit.KryptonButton btnAnalysis;
    }
}
