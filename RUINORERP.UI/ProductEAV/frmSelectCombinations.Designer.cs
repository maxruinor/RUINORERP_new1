namespace RUINORERP.UI.ProductEAV
{
    partial class frmSelectCombinations
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
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOK = new Krypton.Toolkit.KryptonButton();
            this.btnUnselectAll = new Krypton.Toolkit.KryptonButton();
            this.btnSelectAll = new Krypton.Toolkit.KryptonButton();
            this.lblTitle = new Krypton.Toolkit.KryptonLabel();
            this.dataGridViewCombinations = new Krypton.Toolkit.KryptonDataGridView();
            this.colCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCombinationText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCombinations)).BeginInit();
            this.SuspendLayout();
            //
            // panelButtons
            //
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnOK);
            this.panelButtons.Controls.Add(this.btnUnselectAll);
            this.panelButtons.Controls.Add(this.btnSelectAll);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 480);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(800, 60);
            this.panelButtons.TabIndex = 0;
            //
            // btnCancel
            //
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(670, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // btnOK
            //
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(550, 15);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(110, 30);
            this.btnOK.TabIndex = 2;
            this.btnOK.Values.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            //
            // btnUnselectAll
            //
            this.btnUnselectAll.Location = new System.Drawing.Point(130, 15);
            this.btnUnselectAll.Name = "btnUnselectAll";
            this.btnUnselectAll.Size = new System.Drawing.Size(110, 30);
            this.btnUnselectAll.TabIndex = 1;
            this.btnUnselectAll.Values.Text = "取消全选";
            this.btnUnselectAll.Click += new System.EventHandler(this.btnUnselectAll_Click);
            //
            // btnSelectAll
            //
            this.btnSelectAll.Location = new System.Drawing.Point(10, 15);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(110, 30);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Values.Text = "全选";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            //
            // lblTitle
            //
            this.lblTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 25);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Values.Text = "请选择需要生成的SKU组合";
            //
            // dataGridViewCombinations
            //
            this.dataGridViewCombinations.AllowUserToAddRows = false;
            this.dataGridViewCombinations.AllowUserToDeleteRows = false;
            this.dataGridViewCombinations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewCombinations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCombinations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCombinations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheck,
            this.colCombinationText});
            this.dataGridViewCombinations.Location = new System.Drawing.Point(10, 45);
            this.dataGridViewCombinations.Name = "dataGridViewCombinations";
            this.dataGridViewCombinations.ReadOnly = false;
            this.dataGridViewCombinations.RowHeadersVisible = false;
            this.dataGridViewCombinations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewCombinations.Size = new System.Drawing.Size(780, 425);
            this.dataGridViewCombinations.TabIndex = 1;
            this.dataGridViewCombinations.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewCombinations_ColumnHeaderMouseClick);
            //
            // colCheck
            //
            this.colCheck.HeaderText = "选择";
            this.colCheck.Name = "colCheck";
            this.colCheck.ReadOnly = false;
            this.colCheck.Width = 60;
            //
            // colCombinationText
            //
            this.colCombinationText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCombinationText.HeaderText = "组合内容";
            this.colCombinationText.Name = "colCombinationText";
            this.colCombinationText.ReadOnly = true;
            //
            // frmSelectCombinations
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 540);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dataGridViewCombinations);
            this.Controls.Add(this.panelButtons);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "frmSelectCombinations";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择SKU组合";
            this.Load += new System.EventHandler(this.frmSelectCombinations_Load);
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCombinations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOK;
        private Krypton.Toolkit.KryptonButton btnUnselectAll;
        private Krypton.Toolkit.KryptonButton btnSelectAll;
        private Krypton.Toolkit.KryptonLabel lblTitle;
        private Krypton.Toolkit.KryptonDataGridView dataGridViewCombinations;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCombinationText;
    }
}
