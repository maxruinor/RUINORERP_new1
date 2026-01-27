namespace RUINORERP.UI.BaseForm
{
    partial class UCBillChildQuery
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCBillChildQuery));
            this.newSumDataGridViewChild = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.bindingSourceChild = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceChild)).BeginInit();
            this.SuspendLayout();
            // 
            // newSumDataGridViewChild
            // 
            this.newSumDataGridViewChild.AllowUserToAddRows = false;
            this.newSumDataGridViewChild.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.newSumDataGridViewChild.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.newSumDataGridViewChild.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newSumDataGridViewChild.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newSumDataGridViewChild.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newSumDataGridViewChild.IsShowSumRow = false;
            this.newSumDataGridViewChild.Location = new System.Drawing.Point(0, 0);
            this.newSumDataGridViewChild.Name = "newSumDataGridViewChild";
            this.newSumDataGridViewChild.ReadOnly = false;
            this.newSumDataGridViewChild.RowTemplate.Height = 23;
            this.newSumDataGridViewChild.Size = new System.Drawing.Size(741, 623);
            this.newSumDataGridViewChild.SumColumns = null;
            this.newSumDataGridViewChild.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.newSumDataGridViewChild.SumRowCellFormat = "N2";
            this.newSumDataGridViewChild.TabIndex = 2;
            this.newSumDataGridViewChild.UseCustomColumnDisplay = true;
            this.newSumDataGridViewChild.UseSelectedColumn = false;
            this.newSumDataGridViewChild.Use是否使用内置右键功能 = true;
            this.newSumDataGridViewChild.XmlFileName = "";
            // 
            // UCBillChildQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.newSumDataGridViewChild);
            this.Name = "UCBillChildQuery";
            this.Size = new System.Drawing.Size(741, 623);
            this.Load += new System.EventHandler(this.UCBillChildQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceChild)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal UControls.NewSumDataGridView newSumDataGridViewChild;
        internal System.Windows.Forms.BindingSource bindingSourceChild;
    }
}
