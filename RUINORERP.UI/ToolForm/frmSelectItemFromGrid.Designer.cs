namespace RUINORERP.UI.ToolForm
{
    partial class frmSelectItemFromGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.bindingSourcePlanChildItems = new System.Windows.Forms.BindingSource(this.components);
            this.newSumDataGridViewPlanChildItems = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePlanChildItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewPlanChildItems)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            this.kryptonSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer1.Panel1
            // 
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.newSumDataGridViewPlanChildItems);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.btnOk);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(619, 364);
            this.kryptonSplitContainer1.SplitterDistance = 270;
            this.kryptonSplitContainer1.TabIndex = 0;
            // 
            // newSumDataGridViewPlanChildItems
            // 
            this.newSumDataGridViewPlanChildItems.AllowUserToAddRows = false;
            this.newSumDataGridViewPlanChildItems.AllowUserToDeleteRows = false;
            this.newSumDataGridViewPlanChildItems.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.newSumDataGridViewPlanChildItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.newSumDataGridViewPlanChildItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newSumDataGridViewPlanChildItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newSumDataGridViewPlanChildItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newSumDataGridViewPlanChildItems.FieldNameList = null;
            this.newSumDataGridViewPlanChildItems.IsShowSumRow = false;
            this.newSumDataGridViewPlanChildItems.Location = new System.Drawing.Point(0, 0);
            this.newSumDataGridViewPlanChildItems.Name = "newSumDataGridViewPlanChildItems";
            this.newSumDataGridViewPlanChildItems.RowTemplate.Height = 23;
            this.newSumDataGridViewPlanChildItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.newSumDataGridViewPlanChildItems.Size = new System.Drawing.Size(619, 270);
            this.newSumDataGridViewPlanChildItems.SumColumns = null;
            this.newSumDataGridViewPlanChildItems.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.newSumDataGridViewPlanChildItems.SumRowCellFormat = "N2";
            this.newSumDataGridViewPlanChildItems.TabIndex = 1;
            this.newSumDataGridViewPlanChildItems.UseCustomColumnDisplay = true;
            this.newSumDataGridViewPlanChildItems.UseSelectedColumn = false;
            this.newSumDataGridViewPlanChildItems.Use是否使用内置右键功能 = true;
            this.newSumDataGridViewPlanChildItems.XmlFileName = "";
            this.newSumDataGridViewPlanChildItems.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.newSumDataGridViewPlanChildItems_CellFormatting);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(324, 34);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Values.Text = "取消";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(158, 34);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 8;
            this.btnOk.Values.Text = "确定";
            // 
            // frmSelectItemFromGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 364);
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Name = "frmSelectItemFromGrid";
            this.Text = "选择数据行";
            this.Load += new System.EventHandler(this.frmSelectItemFromGrid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePlanChildItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newSumDataGridViewPlanChildItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private System.Windows.Forms.BindingSource bindingSourcePlanChildItems;
        private UControls.NewSumDataGridView newSumDataGridViewPlanChildItems;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
    }
}