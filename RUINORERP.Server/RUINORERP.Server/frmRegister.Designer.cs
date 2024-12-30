namespace RUINORERP.Server
{
    partial class frmRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRegister));
            label1 = new System.Windows.Forms.Label();
            txtCompanyName = new System.Windows.Forms.TextBox();
            txtPhoneNumber = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            txtMachineCode = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            txtRegistrationCode = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            txtConcurrentUsers = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            txtProductVersion = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            dtpExpirationDate = new System.Windows.Forms.DateTimePicker();
            label8 = new System.Windows.Forms.Label();
            cmbLicenseType = new System.Windows.Forms.ComboBox();
            dtpPurchaseDate = new System.Windows.Forms.DateTimePicker();
            lblPurchaseDate = new System.Windows.Forms.Label();
            dtpRegistrationDate = new System.Windows.Forms.DateTimePicker();
            label9 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            chkIsRegistered = new System.Windows.Forms.CheckBox();
            txtRemarks = new System.Windows.Forms.TextBox();
            label11 = new System.Windows.Forms.Label();
            txtContactName = new System.Windows.Forms.TextBox();
            label12 = new System.Windows.Forms.Label();
            btnRegister = new System.Windows.Forms.Button();
            btnGenerateMachineCode = new System.Windows.Forms.Button();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsbtnSaveRegInfo = new System.Windows.Forms.ToolStripButton();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(61, 34);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 17);
            label1.TabIndex = 0;
            label1.Text = "注册公司名";
            // 
            // txtCompanyName
            // 
            txtCompanyName.Location = new System.Drawing.Point(140, 28);
            txtCompanyName.Name = "txtCompanyName";
            txtCompanyName.Size = new System.Drawing.Size(461, 23);
            txtCompanyName.TabIndex = 1;
            // 
            // txtPhoneNumber
            // 
            txtPhoneNumber.Location = new System.Drawing.Point(441, 60);
            txtPhoneNumber.Name = "txtPhoneNumber";
            txtPhoneNumber.Size = new System.Drawing.Size(160, 23);
            txtPhoneNumber.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(393, 61);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 17);
            label2.TabIndex = 2;
            label2.Text = "手机号";
            // 
            // txtMachineCode
            // 
            txtMachineCode.Location = new System.Drawing.Point(140, 89);
            txtMachineCode.Multiline = true;
            txtMachineCode.Name = "txtMachineCode";
            txtMachineCode.Size = new System.Drawing.Size(461, 137);
            txtMachineCode.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(85, 89);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(44, 17);
            label3.TabIndex = 4;
            label3.Text = "机器码";
            // 
            // txtRegistrationCode
            // 
            txtRegistrationCode.Location = new System.Drawing.Point(140, 244);
            txtRegistrationCode.Name = "txtRegistrationCode";
            txtRegistrationCode.Size = new System.Drawing.Size(464, 23);
            txtRegistrationCode.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(85, 247);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(44, 17);
            label4.TabIndex = 6;
            label4.Text = "注册码";
            // 
            // txtConcurrentUsers
            // 
            txtConcurrentUsers.Location = new System.Drawing.Point(140, 278);
            txtConcurrentUsers.Name = "txtConcurrentUsers";
            txtConcurrentUsers.Size = new System.Drawing.Size(100, 23);
            txtConcurrentUsers.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(37, 281);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(92, 17);
            label5.TabIndex = 8;
            label5.Text = "同时在线用户数";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(381, 282);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(56, 17);
            label6.TabIndex = 10;
            label6.Text = "截止时间";
            // 
            // txtProductVersion
            // 
            txtProductVersion.Location = new System.Drawing.Point(441, 312);
            txtProductVersion.Name = "txtProductVersion";
            txtProductVersion.Size = new System.Drawing.Size(160, 23);
            txtProductVersion.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(381, 316);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(56, 17);
            label7.TabIndex = 12;
            label7.Text = "版本信息";
            // 
            // dtpExpirationDate
            // 
            dtpExpirationDate.Location = new System.Drawing.Point(441, 278);
            dtpExpirationDate.Name = "dtpExpirationDate";
            dtpExpirationDate.Size = new System.Drawing.Size(160, 23);
            dtpExpirationDate.TabIndex = 14;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(73, 315);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(56, 17);
            label8.TabIndex = 15;
            label8.Text = "授权类型";
            // 
            // cmbLicenseType
            // 
            cmbLicenseType.FormattingEnabled = true;
            cmbLicenseType.Location = new System.Drawing.Point(140, 311);
            cmbLicenseType.Name = "cmbLicenseType";
            cmbLicenseType.Size = new System.Drawing.Size(173, 25);
            cmbLicenseType.TabIndex = 16;
            // 
            // dtpPurchaseDate
            // 
            dtpPurchaseDate.Location = new System.Drawing.Point(140, 354);
            dtpPurchaseDate.Name = "dtpPurchaseDate";
            dtpPurchaseDate.Size = new System.Drawing.Size(173, 23);
            dtpPurchaseDate.TabIndex = 18;
            // 
            // lblPurchaseDate
            // 
            lblPurchaseDate.AutoSize = true;
            lblPurchaseDate.Location = new System.Drawing.Point(73, 357);
            lblPurchaseDate.Name = "lblPurchaseDate";
            lblPurchaseDate.Size = new System.Drawing.Size(56, 17);
            lblPurchaseDate.TabIndex = 17;
            lblPurchaseDate.Text = "购买日期";
            // 
            // dtpRegistrationDate
            // 
            dtpRegistrationDate.Location = new System.Drawing.Point(441, 354);
            dtpRegistrationDate.Name = "dtpRegistrationDate";
            dtpRegistrationDate.Size = new System.Drawing.Size(160, 23);
            dtpRegistrationDate.TabIndex = 20;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(381, 358);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(56, 17);
            label9.TabIndex = 19;
            label9.Text = "注册时间";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(85, 405);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(44, 17);
            label10.TabIndex = 21;
            label10.Text = "已注册";
            // 
            // chkIsRegistered
            // 
            chkIsRegistered.AutoSize = true;
            chkIsRegistered.Location = new System.Drawing.Point(140, 406);
            chkIsRegistered.Name = "chkIsRegistered";
            chkIsRegistered.Size = new System.Drawing.Size(15, 14);
            chkIsRegistered.TabIndex = 22;
            chkIsRegistered.UseVisualStyleBackColor = true;
            // 
            // txtRemarks
            // 
            txtRemarks.Location = new System.Drawing.Point(140, 453);
            txtRemarks.Multiline = true;
            txtRemarks.Name = "txtRemarks";
            txtRemarks.Size = new System.Drawing.Size(461, 99);
            txtRemarks.TabIndex = 24;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(97, 459);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(32, 17);
            label11.TabIndex = 23;
            label11.Text = "备注";
            // 
            // txtContactName
            // 
            txtContactName.Location = new System.Drawing.Point(140, 57);
            txtContactName.Name = "txtContactName";
            txtContactName.Size = new System.Drawing.Size(173, 23);
            txtContactName.TabIndex = 26;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(61, 63);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(68, 17);
            label12.TabIndex = 25;
            label12.Text = "联系人姓名";
            // 
            // btnRegister
            // 
            btnRegister.Location = new System.Drawing.Point(180, 402);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new System.Drawing.Size(75, 23);
            btnRegister.TabIndex = 27;
            btnRegister.Text = "注册";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // btnGenerateMachineCode
            // 
            btnGenerateMachineCode.Location = new System.Drawing.Point(607, 89);
            btnGenerateMachineCode.Name = "btnGenerateMachineCode";
            btnGenerateMachineCode.Size = new System.Drawing.Size(89, 31);
            btnGenerateMachineCode.TabIndex = 28;
            btnGenerateMachineCode.Text = "生成机器码";
            btnGenerateMachineCode.UseVisualStyleBackColor = true;
            btnGenerateMachineCode.Click += btnGenerateMachineCode_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbtnSaveRegInfo });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(746, 25);
            toolStrip1.TabIndex = 29;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnSaveRegInfo
            // 
            tsbtnSaveRegInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtnSaveRegInfo.Image = (System.Drawing.Image)resources.GetObject("tsbtnSaveRegInfo.Image");
            tsbtnSaveRegInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnSaveRegInfo.Name = "tsbtnSaveRegInfo";
            tsbtnSaveRegInfo.Size = new System.Drawing.Size(84, 22);
            tsbtnSaveRegInfo.Text = "保存注册信息";
            tsbtnSaveRegInfo.Click += tsbtnSaveRegInfo_Click;
            // 
            // frmRegister
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(746, 579);
            Controls.Add(toolStrip1);
            Controls.Add(btnGenerateMachineCode);
            Controls.Add(btnRegister);
            Controls.Add(txtContactName);
            Controls.Add(label12);
            Controls.Add(txtRemarks);
            Controls.Add(label11);
            Controls.Add(chkIsRegistered);
            Controls.Add(label10);
            Controls.Add(dtpRegistrationDate);
            Controls.Add(label9);
            Controls.Add(dtpPurchaseDate);
            Controls.Add(lblPurchaseDate);
            Controls.Add(cmbLicenseType);
            Controls.Add(label8);
            Controls.Add(dtpExpirationDate);
            Controls.Add(txtProductVersion);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(txtConcurrentUsers);
            Controls.Add(label5);
            Controls.Add(txtRegistrationCode);
            Controls.Add(label4);
            Controls.Add(txtMachineCode);
            Controls.Add(label3);
            Controls.Add(txtPhoneNumber);
            Controls.Add(label2);
            Controls.Add(txtCompanyName);
            Controls.Add(label1);
            Name = "frmRegister";
            Text = "注册信息";
            Load += frmRegister_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMachineCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRegistrationCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtConcurrentUsers;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtProductVersion;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpExpirationDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbLicenseType;
        private System.Windows.Forms.DateTimePicker dtpPurchaseDate;
        private System.Windows.Forms.Label lblPurchaseDate;
        private System.Windows.Forms.DateTimePicker dtpRegistrationDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkIsRegistered;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtContactName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnGenerateMachineCode;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnSaveRegInfo;
    }
}