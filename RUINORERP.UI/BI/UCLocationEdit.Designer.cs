namespace RUINORERP.UI.BI
{
    partial class UCLocationEdit
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
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.CmbContactPerson = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.txtTel = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.lblName = new Krypton.Toolkit.KryptonLabel();
            this.txtLocationCode = new Krypton.Toolkit.KryptonTextBox();
            this.txtName = new Krypton.Toolkit.KryptonTextBox();
            this.txtLocationType_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblLocationType_ID = new Krypton.Toolkit.KryptonLabel();
            this.txtDesc = new Krypton.Toolkit.KryptonTextBox();
            this.lblDesc = new Krypton.Toolkit.KryptonLabel();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.rdbis_enabledNo = new Krypton.Toolkit.KryptonRadioButton();
            this.rdbis_enabledYes = new Krypton.Toolkit.KryptonRadioButton();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbContactPerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocationType_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(118, 353);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(236, 353);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Controls.Add(this.lblIs_enabled);
            this.kryptonPanel1.Controls.Add(this.CmbContactPerson);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.txtTel);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.lblName);
            this.kryptonPanel1.Controls.Add(this.txtLocationCode);
            this.kryptonPanel1.Controls.Add(this.txtName);
            this.kryptonPanel1.Controls.Add(this.txtLocationType_ID);
            this.kryptonPanel1.Controls.Add(this.lblLocationType_ID);
            this.kryptonPanel1.Controls.Add(this.txtDesc);
            this.kryptonPanel1.Controls.Add(this.lblDesc);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(475, 439);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // CmbContactPerson
            // 
            this.CmbContactPerson.DropDownWidth = 208;
            this.CmbContactPerson.IntegralHeight = false;
            this.CmbContactPerson.Location = new System.Drawing.Point(117, 119);
            this.CmbContactPerson.Name = "CmbContactPerson";
            this.CmbContactPerson.Size = new System.Drawing.Size(208, 21);
            this.CmbContactPerson.TabIndex = 16;
            this.CmbContactPerson.Visible = false;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(65, 120);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(49, 20);
            this.kryptonLabel1.TabIndex = 15;
            this.kryptonLabel1.Values.Text = "联系人";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(78, 151);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel2.TabIndex = 13;
            this.kryptonLabel2.Values.Text = "电话";
            // 
            // txtTel
            // 
            this.txtTel.Location = new System.Drawing.Point(118, 147);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(208, 23);
            this.txtTel.TabIndex = 14;
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(53, 52);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel3.TabIndex = 9;
            this.kryptonLabel3.Values.Text = "仓库代码";
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(53, 88);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(62, 20);
            this.lblName.TabIndex = 9;
            this.lblName.Values.Text = "仓库名称";
            // 
            // txtLocationCode
            // 
            this.txtLocationCode.Location = new System.Drawing.Point(118, 50);
            this.txtLocationCode.Name = "txtLocationCode";
            this.txtLocationCode.Size = new System.Drawing.Size(208, 23);
            this.txtLocationCode.TabIndex = 10;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(118, 86);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(208, 23);
            this.txtName.TabIndex = 10;
            // 
            // txtLocationType_ID
            // 
            this.txtLocationType_ID.DropDownWidth = 208;
            this.txtLocationType_ID.IntegralHeight = false;
            this.txtLocationType_ID.Location = new System.Drawing.Point(118, 16);
            this.txtLocationType_ID.Name = "txtLocationType_ID";
            this.txtLocationType_ID.Size = new System.Drawing.Size(208, 21);
            this.txtLocationType_ID.TabIndex = 8;
            // 
            // lblLocationType_ID
            // 
            this.lblLocationType_ID.Location = new System.Drawing.Point(53, 16);
            this.lblLocationType_ID.Name = "lblLocationType_ID";
            this.lblLocationType_ID.Size = new System.Drawing.Size(62, 20);
            this.lblLocationType_ID.TabIndex = 6;
            this.lblLocationType_ID.Values.Text = "仓库类型";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(117, 178);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(208, 90);
            this.txtDesc.TabIndex = 5;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(57, 183);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(57, 20);
            this.lblDesc.TabIndex = 4;
            this.lblDesc.Values.Text = "描      述";
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.CaptionVisible = false;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(120, 287);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.rdbis_enabledNo);
            this.kryptonGroupBox1.Panel.Controls.Add(this.rdbis_enabledYes);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(147, 40);
            this.kryptonGroupBox1.TabIndex = 65;
            this.kryptonGroupBox1.Values.Heading = "是";
            // 
            // rdbis_enabledNo
            // 
            this.rdbis_enabledNo.Location = new System.Drawing.Point(58, 8);
            this.rdbis_enabledNo.Name = "rdbis_enabledNo";
            this.rdbis_enabledNo.Size = new System.Drawing.Size(35, 20);
            this.rdbis_enabledNo.TabIndex = 1;
            this.rdbis_enabledNo.Values.Text = "否";
            // 
            // rdbis_enabledYes
            // 
            this.rdbis_enabledYes.Location = new System.Drawing.Point(10, 8);
            this.rdbis_enabledYes.Name = "rdbis_enabledYes";
            this.rdbis_enabledYes.Size = new System.Drawing.Size(35, 20);
            this.rdbis_enabledYes.TabIndex = 0;
            this.rdbis_enabledYes.Values.Text = "是";
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(52, 287);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 64;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // UCLocationEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 439);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCLocationEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbContactPerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocationType_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonTextBox txtDesc;
        private Krypton.Toolkit.KryptonLabel lblDesc;
        private Krypton.Toolkit.KryptonLabel lblLocationType_ID;
        private Krypton.Toolkit.KryptonComboBox txtLocationType_ID;
        private Krypton.Toolkit.KryptonLabel lblName;
        private Krypton.Toolkit.KryptonTextBox txtName;
        private Krypton.Toolkit.KryptonComboBox CmbContactPerson;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonTextBox txtTel;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonTextBox txtLocationCode;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonRadioButton rdbis_enabledNo;
        private Krypton.Toolkit.KryptonRadioButton rdbis_enabledYes;
        private Krypton.Toolkit.KryptonLabel lblIs_enabled;
    }
}
