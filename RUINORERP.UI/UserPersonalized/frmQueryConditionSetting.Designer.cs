namespace RUINORERP.UI.UserPersonalized
{
    partial class frmQueryConditionSetting
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
            this.QueryShowColQty = new Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.panelConditionEdit = new System.Windows.Forms.Panel();
            this.chkReverseSelection = new Krypton.Toolkit.KryptonCheckBox();
            this.chkAll = new Krypton.Toolkit.KryptonCheckBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chkEnableQuerySettings = new Krypton.Toolkit.KryptonCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // QueryShowColQty
            // 
            this.QueryShowColQty.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.QueryShowColQty.Location = new System.Drawing.Point(161, 14);
            this.QueryShowColQty.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.QueryShowColQty.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.QueryShowColQty.Name = "QueryShowColQty";
            this.QueryShowColQty.Size = new System.Drawing.Size(68, 22);
            this.QueryShowColQty.TabIndex = 0;
            this.QueryShowColQty.ToolTipValues.Description = "查询条件过多时，显示不完整，可以将数字调大。";
            this.QueryShowColQty.ToolTipValues.EnableToolTips = true;
            this.QueryShowColQty.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(25, 14);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(130, 20);
            this.kryptonLabel1.TabIndex = 1;
            this.kryptonLabel1.Values.Text = "查询条件显示列数量:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(379, 565);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Values.Text = "取消(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(212, 565);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 12;
            this.btnOk.Values.Text = "确定(&O)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.chkEnableQuerySettings);
            this.kryptonPanel1.Controls.Add(this.panelConditionEdit);
            this.kryptonPanel1.Controls.Add(this.chkReverseSelection);
            this.kryptonPanel1.Controls.Add(this.chkAll);
            this.kryptonPanel1.Controls.Add(this.listView1);
            this.kryptonPanel1.Controls.Add(this.QueryShowColQty);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(729, 636);
            this.kryptonPanel1.TabIndex = 14;
            // 
            // panelConditionEdit
            // 
            this.panelConditionEdit.Location = new System.Drawing.Point(221, 60);
            this.panelConditionEdit.Name = "panelConditionEdit";
            this.panelConditionEdit.Size = new System.Drawing.Size(496, 435);
            this.panelConditionEdit.TabIndex = 21;
            // 
            // chkReverseSelection
            // 
            this.chkReverseSelection.Location = new System.Drawing.Point(89, 515);
            this.chkReverseSelection.Name = "chkReverseSelection";
            this.chkReverseSelection.Size = new System.Drawing.Size(49, 20);
            this.chkReverseSelection.TabIndex = 20;
            this.chkReverseSelection.Values.Text = "反选";
            this.chkReverseSelection.CheckedChanged += new System.EventHandler(this.chkReverseSelection_CheckedChanged);
            // 
            // chkAll
            // 
            this.chkAll.Location = new System.Drawing.Point(38, 515);
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
            this.listView1.Location = new System.Drawing.Point(7, 60);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(204, 435);
            this.listView1.TabIndex = 16;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // chkEnableQuerySettings
            // 
            this.chkEnableQuerySettings.Location = new System.Drawing.Point(603, 16);
            this.chkEnableQuerySettings.Name = "chkEnableQuerySettings";
            this.chkEnableQuerySettings.Size = new System.Drawing.Size(114, 20);
            this.chkEnableQuerySettings.TabIndex = 22;
            this.chkEnableQuerySettings.Values.Text = "启用查询预设值";
            // 
            // frmQueryConditionSetting
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(729, 636);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "frmQueryConditionSetting";
            this.Text = "查询条件设置";
            this.Load += new System.EventHandler(this.frmMenuPersonalization_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        public Krypton.Toolkit.KryptonNumericUpDown QueryShowColQty;
        private System.Windows.Forms.ListView listView1;
        private Krypton.Toolkit.KryptonCheckBox chkReverseSelection;
        private Krypton.Toolkit.KryptonCheckBox chkAll;
        public System.Windows.Forms.Panel panelConditionEdit;
        private Krypton.Toolkit.KryptonCheckBox chkEnableQuerySettings;
    }
}