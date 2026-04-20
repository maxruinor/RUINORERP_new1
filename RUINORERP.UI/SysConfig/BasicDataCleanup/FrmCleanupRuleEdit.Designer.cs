namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    partial class FrmCleanupRuleEdit
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
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            this.kbtnOK = new Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.kchkEnabled = new Krypton.Toolkit.KryptonCheckBox();
            this.kcmbActionType = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            this.kcmbRuleType = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.ktxtDescription = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.ktxtRuleName = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonGroupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonPanel2 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonLabel5 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbActionType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbRuleType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            this.SuspendLayout();
            //
            // kryptonPanel1
            //
            this.kryptonPanel1.Controls.Add(this.kbtnCancel);
            this.kryptonPanel1.Controls.Add(this.kbtnOK);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 520);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(600, 60);
            this.kryptonPanel1.TabIndex = 0;
            //
            // kbtnCancel
            //
            this.kbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kbtnCancel.Location = new System.Drawing.Point(310, 18);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(90, 28);
            this.kbtnCancel.TabIndex = 1;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.KbtnCancel_Click);
            //
            // kbtnOK
            //
            this.kbtnOK.Location = new System.Drawing.Point(200, 18);
            this.kbtnOK.Name = "kbtnOK";
            this.kbtnOK.Size = new System.Drawing.Size(90, 28);
            this.kbtnOK.TabIndex = 0;
            this.kbtnOK.Values.Text = "确定";
            this.kbtnOK.Click += new System.EventHandler(this.KbtnOK_Click);
            //
            // kryptonGroupBox1
            //
            this.kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            //
            // kryptonGroupBox1.Panel
            //
            this.kryptonGroupBox1.Panel.Controls.Add(this.kchkEnabled);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kcmbActionType);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel4);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kcmbRuleType);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel3);
            this.kryptonGroupBox1.Panel.Controls.Add(this.ktxtDescription);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel2);
            this.kryptonGroupBox1.Panel.Controls.Add(this.ktxtRuleName);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(600, 200);
            this.kryptonGroupBox1.TabIndex = 1;
            this.kryptonGroupBox1.Values.Heading = "基本信息";
            //
            // kchkEnabled
            //
            this.kchkEnabled.Checked = true;
            this.kchkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.kchkEnabled.Location = new System.Drawing.Point(450, 20);
            this.kchkEnabled.Name = "kchkEnabled";
            this.kchkEnabled.Size = new System.Drawing.Size(49, 20);
            this.kchkEnabled.TabIndex = 8;
            this.kchkEnabled.Values.Text = "启用";
            //
            // kcmbActionType
            //
            this.kcmbActionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbActionType.DropDownWidth = 200;
            this.kcmbActionType.IntegralHeight = false;
            this.kcmbActionType.Location = new System.Drawing.Point(90, 115);
            this.kcmbActionType.Name = "kcmbActionType";
            this.kcmbActionType.Size = new System.Drawing.Size(200, 21);
            this.kcmbActionType.TabIndex = 7;
            //
            // kryptonLabel4
            //
            this.kryptonLabel4.Location = new System.Drawing.Point(20, 115);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel4.TabIndex = 6;
            this.kryptonLabel4.Values.Text = "操作类型:";
            //
            // kcmbRuleType
            //
            this.kcmbRuleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbRuleType.DropDownWidth = 200;
            this.kcmbRuleType.IntegralHeight = false;
            this.kcmbRuleType.Location = new System.Drawing.Point(90, 85);
            this.kcmbRuleType.Name = "kcmbRuleType";
            this.kcmbRuleType.Size = new System.Drawing.Size(200, 21);
            this.kcmbRuleType.TabIndex = 5;
            this.kcmbRuleType.SelectedIndexChanged += new System.EventHandler(this.KcmbRuleType_SelectedIndexChanged);
            //
            // kryptonLabel3
            //
            this.kryptonLabel3.Location = new System.Drawing.Point(20, 85);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel3.TabIndex = 4;
            this.kryptonLabel3.Values.Text = "规则类型:";
            //
            // ktxtDescription
            //
            this.ktxtDescription.Location = new System.Drawing.Point(90, 50);
            this.ktxtDescription.Name = "ktxtDescription";
            this.ktxtDescription.Size = new System.Drawing.Size(480, 23);
            this.ktxtDescription.TabIndex = 3;
            //
            // kryptonLabel2
            //
            this.kryptonLabel2.Location = new System.Drawing.Point(20, 50);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel2.TabIndex = 2;
            this.kryptonLabel2.Values.Text = "规则描述:";
            //
            // ktxtRuleName
            //
            this.ktxtRuleName.Location = new System.Drawing.Point(90, 18);
            this.ktxtRuleName.Name = "ktxtRuleName";
            this.ktxtRuleName.Size = new System.Drawing.Size(320, 23);
            this.ktxtRuleName.TabIndex = 1;
            //
            // kryptonLabel1
            //
            this.kryptonLabel1.Location = new System.Drawing.Point(20, 18);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "规则名称:";
            //
            // kryptonGroupBox2
            //
            this.kryptonGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(0, 200);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            //
            // kryptonGroupBox2.Panel
            //
            this.kryptonGroupBox2.Panel.Controls.Add(this.kryptonPanel2);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(600, 320);
            this.kryptonGroupBox2.TabIndex = 2;
            this.kryptonGroupBox2.Values.Heading = "规则配置";
            //
            // kryptonPanel2
            //
            this.kryptonPanel2.Controls.Add(this.kryptonLabel5);
            this.kryptonPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel2.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(596, 296);
            this.kryptonPanel2.TabIndex = 0;
            //
            // kryptonLabel5
            //
            this.kryptonLabel5.Location = new System.Drawing.Point(20, 20);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(350, 20);
            this.kryptonLabel5.TabIndex = 0;
            this.kryptonLabel5.Values.Text = "请根据选择的规则类型，在下方配置具体的清理条件和参数。";
            //
            // FrmCleanupRuleEdit
            //
            this.AcceptButton = this.kbtnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.kbtnCancel;
            this.ClientSize = new System.Drawing.Size(600, 580);
            this.Controls.Add(this.kryptonGroupBox2);
            this.Controls.Add(this.kryptonGroupBox1);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCleanupRuleEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "清理规则编辑";
            this.Load += new System.EventHandler(this.FrmCleanupRuleEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kcmbActionType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbRuleType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            this.kryptonPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonButton kbtnOK;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonTextBox ktxtRuleName;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonTextBox ktxtDescription;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonComboBox kcmbRuleType;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonComboBox kcmbActionType;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonCheckBox kchkEnabled;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private Krypton.Toolkit.KryptonLabel kryptonLabel5;
    }
}
