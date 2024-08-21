namespace RUINORERP.UI.BaseForm
{
    partial class UCBillMasterQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCBillMasterQuery));
            this.newSumDataGridViewMaster = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.bindingSourceMaster = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // newSumDataGridViewMaster
            // 
            this.newSumDataGridViewMaster.AllowUserToAddRows = false;
            this.newSumDataGridViewMaster.AllowUserToDeleteRows = false;
            this.newSumDataGridViewMaster.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.newSumDataGridViewMaster.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.newSumDataGridViewMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newSumDataGridViewMaster.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newSumDataGridViewMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.newSumDataGridViewMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newSumDataGridViewMaster.FieldNameList = ((System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.KeyValuePair<string, bool>>)(resources.GetObject("newSumDataGridViewMaster.FieldNameList")));
            this.newSumDataGridViewMaster.IsShowSumRow = false;
            this.newSumDataGridViewMaster.Location = new System.Drawing.Point(0, 0);
            this.newSumDataGridViewMaster.Name = "newSumDataGridViewMaster";
            this.newSumDataGridViewMaster.RowTemplate.Height = 23;
            this.newSumDataGridViewMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newSumDataGridViewMaster.Size = new System.Drawing.Size(746, 664);
            this.newSumDataGridViewMaster.SumColumns = null;
            this.newSumDataGridViewMaster.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.newSumDataGridViewMaster.SumRowCellFormat = "N2";
            this.newSumDataGridViewMaster.TabIndex = 1;
            this.newSumDataGridViewMaster.UseCustomColumnDisplay = true;
            this.newSumDataGridViewMaster.UseSelectedColumn = false;
            this.newSumDataGridViewMaster.Use是否使用内置右键功能 = true;
            this.newSumDataGridViewMaster.XmlFileName = "";
            this.newSumDataGridViewMaster.DataSourceChanged += new System.EventHandler(this.newSumDataGridViewMaster_DataSourceChanged);
            this.newSumDataGridViewMaster.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.newSumDataGridViewMaster_CellDoubleClick);
            this.newSumDataGridViewMaster.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView1_CellFormatting);
            this.newSumDataGridViewMaster.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.newSumDataGridViewMaster_DataError);
            // 
            // bindingSourceMaster
            // 
            this.bindingSourceMaster.PositionChanged += new System.EventHandler(this.bindingSourceMaster_PositionChanged);
            // 
            // UCBillMasterQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.newSumDataGridViewMaster);
            this.Name = "UCBillMasterQuery";
            this.Size = new System.Drawing.Size(746, 664);
            this.Load += new System.EventHandler(this.UCBillMasterQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal UControls.NewSumDataGridView newSumDataGridViewMaster;
        internal System.Windows.Forms.BindingSource bindingSourceMaster;
    }
}
