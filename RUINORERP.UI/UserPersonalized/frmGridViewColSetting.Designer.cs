namespace RUINORERP.UI.UserPersonalized
{
    partial class frmGridViewColSetting
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
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.cmbColsDisplayModel = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.btnInitCol = new Krypton.Toolkit.KryptonButton();
            this.panelConditionEdit = new System.Windows.Forms.Panel();
            this.chkReverseSelection = new Krypton.Toolkit.KryptonCheckBox();
            this.chkAll = new Krypton.Toolkit.KryptonCheckBox();
            this.listView1 = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbColsDisplayModel)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(475, 576);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Values.Text = "取消(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(308, 576);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 12;
            this.btnOk.Values.Text = "确定(&O)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.cmbColsDisplayModel);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.btnInitCol);
            this.kryptonPanel1.Controls.Add(this.panelConditionEdit);
            this.kryptonPanel1.Controls.Add(this.chkReverseSelection);
            this.kryptonPanel1.Controls.Add(this.chkAll);
            this.kryptonPanel1.Controls.Add(this.listView1);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(898, 624);
            this.kryptonPanel1.TabIndex = 14;
            // 
            // cmbColsDisplayModel
            // 
            this.cmbColsDisplayModel.DropDownWidth = 100;
            this.cmbColsDisplayModel.IntegralHeight = false;
            this.cmbColsDisplayModel.Location = new System.Drawing.Point(308, 25);
            this.cmbColsDisplayModel.Name = "cmbColsDisplayModel";
            this.cmbColsDisplayModel.Size = new System.Drawing.Size(336, 21);
            this.cmbColsDisplayModel.TabIndex = 46;
            this.cmbColsDisplayModel.SelectedIndexChanged += new System.EventHandler(this.cmbColsDisplayModel_SelectedIndexChanged);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(240, 25);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel1.TabIndex = 45;
            this.kryptonLabel1.Values.Text = "列宽模式";
            // 
            // btnInitCol
            // 
            this.btnInitCol.Location = new System.Drawing.Point(776, 25);
            this.btnInitCol.Name = "btnInitCol";
            this.btnInitCol.Size = new System.Drawing.Size(90, 25);
            this.btnInitCol.TabIndex = 44;
            this.btnInitCol.Values.Text = "恢复默认设置";
            this.btnInitCol.Click += new System.EventHandler(this.btnInitCol_Click);
            // 
            // panelConditionEdit
            // 
            this.panelConditionEdit.Location = new System.Drawing.Point(238, 60);
            this.panelConditionEdit.Name = "panelConditionEdit";
            this.panelConditionEdit.Size = new System.Drawing.Size(628, 490);
            this.panelConditionEdit.TabIndex = 22;
            // 
            // chkReverseSelection
            // 
            this.chkReverseSelection.Location = new System.Drawing.Point(100, 556);
            this.chkReverseSelection.Name = "chkReverseSelection";
            this.chkReverseSelection.Size = new System.Drawing.Size(49, 20);
            this.chkReverseSelection.TabIndex = 20;
            this.chkReverseSelection.Values.Text = "反选";
            this.chkReverseSelection.CheckedChanged += new System.EventHandler(this.chkReverseSelection_CheckedChanged);
            // 
            // chkAll
            // 
            this.chkAll.Location = new System.Drawing.Point(49, 556);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(49, 20);
            this.chkAll.TabIndex = 19;
            this.chkAll.Values.Text = "全选";
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(16, 60);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(216, 490);
            this.listView1.TabIndex = 16;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // frmGridViewColSetting
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(898, 624);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "frmGridViewColSetting";
            this.Text = "表格显示设置";
            this.Load += new System.EventHandler(this.frmMenuPersonalization_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbColsDisplayModel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private System.Windows.Forms.ListView listView1;
        private Krypton.Toolkit.KryptonCheckBox chkReverseSelection;
        private Krypton.Toolkit.KryptonCheckBox chkAll;
        public System.Windows.Forms.Panel panelConditionEdit;
        private Krypton.Toolkit.KryptonButton btnInitCol;
        public Krypton.Toolkit.KryptonComboBox cmbColsDisplayModel;
        public Krypton.Toolkit.KryptonLabel kryptonLabel1;
    }
}