namespace RUINORERP.UI.ToolForm
{
    partial class frmAdvanceSelector<T>
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdvanceSelector));
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonSplitContainerNei = new Krypton.Toolkit.KryptonSplitContainer();
            this.txtFilter = new Krypton.Toolkit.KryptonTextBox();
            this.lblFilter = new Krypton.Toolkit.KryptonLabel();
            this.dgvItems = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.timerForToolTip = new System.Windows.Forms.Timer(this.components);
            this.errorProviderForAllInput = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTipBase = new System.Windows.Forms.ToolTip(this.components);
            this.bindingSourceEdit = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerNei)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerNei.Panel1)).BeginInit();
            this.kryptonSplitContainerNei.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerNei.Panel2)).BeginInit();
            this.kryptonSplitContainerNei.Panel2.SuspendLayout();
            this.kryptonSplitContainerNei.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonSplitContainer1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(761, 488);
            this.kryptonPanel1.TabIndex = 5;
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
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonSplitContainerNei);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btnOk);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(761, 488);
            this.kryptonSplitContainer1.SplitterDistance = 419;
            this.kryptonSplitContainer1.TabIndex = 14;
            // 
            // kryptonSplitContainerNei
            // 
            this.kryptonSplitContainerNei.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainerNei.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainerNei.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainerNei.Name = "kryptonSplitContainerNei";
            this.kryptonSplitContainerNei.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainerNei.Panel1
            // 
            this.kryptonSplitContainerNei.Panel1.Controls.Add(this.txtFilter);
            this.kryptonSplitContainerNei.Panel1.Controls.Add(this.lblFilter);
            // 
            // kryptonSplitContainerNei.Panel2
            // 
            this.kryptonSplitContainerNei.Panel2.Controls.Add(this.dgvItems);
            this.kryptonSplitContainerNei.Size = new System.Drawing.Size(761, 419);
            this.kryptonSplitContainerNei.SplitterDistance = 39;
            this.kryptonSplitContainerNei.TabIndex = 2;
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(73, 8);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(179, 23);
            this.txtFilter.TabIndex = 3;
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(18, 9);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(49, 20);
            this.lblFilter.TabIndex = 2;
            this.lblFilter.Values.Text = "过滤器";
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige;
            this.dgvItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItems.BizInvisibleCols = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("dgvItems.BizInvisibleCols")));
            this.dgvItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.CustomRowNo = false;
            this.dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItems.EnableFiltering = false;
            this.dgvItems.FieldNameList = null;
            this.dgvItems.IsShowSumRow = false;
            this.dgvItems.Location = new System.Drawing.Point(0, 0);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.NeedSaveColumnsXml = false;
            this.dgvItems.RowTemplate.Height = 23;
            this.dgvItems.Size = new System.Drawing.Size(761, 375);
            this.dgvItems.SumColumns = null;
            this.dgvItems.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dgvItems.SumRowCellFormat = "N2";
            this.dgvItems.TabIndex = 1;
            this.dgvItems.UseBatchEditColumn = false;
            this.dgvItems.UseCustomColumnDisplay = true;
            this.dgvItems.UseSelectedColumn = false;
            this.dgvItems.Use是否使用内置右键功能 = true;
            this.dgvItems.XmlFileName = "";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(382, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Values.Text = "取消(&N)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(258, 14);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 12;
            this.btnOk.Values.Text = "确定(&Y)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // timerForToolTip
            // 
            this.timerForToolTip.Interval = 1000;
            // 
            // errorProviderForAllInput
            // 
            this.errorProviderForAllInput.ContainerControl = this;
            // 
            // frmAdvanceSelector
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(761, 488);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "frmAdvanceSelector";
            this.Text = "选择单据";
            this.Load += new System.EventHandler(this.frmAdvanceSelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerNei.Panel1)).EndInit();
            this.kryptonSplitContainerNei.Panel1.ResumeLayout(false);
            this.kryptonSplitContainerNei.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerNei.Panel2)).EndInit();
            this.kryptonSplitContainerNei.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerNei)).EndInit();
            this.kryptonSplitContainerNei.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEdit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private System.Windows.Forms.Timer timerForToolTip;
        public System.Windows.Forms.ErrorProvider errorProviderForAllInput;
        internal System.Windows.Forms.ToolTip toolTipBase;
        internal System.Windows.Forms.BindingSource bindingSourceEdit;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private UControls.NewSumDataGridView dgvItems;
        private Krypton.Toolkit.KryptonTextBox txtFilter;
        private Krypton.Toolkit.KryptonLabel lblFilter;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainerNei;
    }
}