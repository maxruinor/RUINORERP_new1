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
            lblRegistrationDate = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            chkIsRegistered = new System.Windows.Forms.CheckBox();
            txtRemarks = new System.Windows.Forms.TextBox();
            label11 = new System.Windows.Forms.Label();
            txtContactName = new System.Windows.Forms.TextBox();
            label12 = new System.Windows.Forms.Label();
            btnRegister = new System.Windows.Forms.Button();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsbtnSaveRegInfo = new System.Windows.Forms.ToolStripButton();
            groupBox1 = new System.Windows.Forms.GroupBox();
            btnCreateRegInfo = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            txtRegInfo = new System.Windows.Forms.TextBox();
            toolStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(55, 28);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 17);
            label1.TabIndex = 0;
            label1.Text = "注册公司名";
            // 
            // txtCompanyName
            // 
            txtCompanyName.Location = new System.Drawing.Point(134, 22);
            txtCompanyName.Name = "txtCompanyName";
            txtCompanyName.Size = new System.Drawing.Size(461, 23);
            txtCompanyName.TabIndex = 1;
            // 
            // txtPhoneNumber
            // 
            txtPhoneNumber.Location = new System.Drawing.Point(435, 54);
            txtPhoneNumber.Name = "txtPhoneNumber";
            txtPhoneNumber.Size = new System.Drawing.Size(160, 23);
            txtPhoneNumber.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(387, 55);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 17);
            label2.TabIndex = 2;
            label2.Text = "手机号";
            // 
            // txtRegistrationCode
            // 
            txtRegistrationCode.Location = new System.Drawing.Point(242, 548);
            txtRegistrationCode.Name = "txtRegistrationCode";
            txtRegistrationCode.Size = new System.Drawing.Size(464, 23);
            txtRegistrationCode.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(187, 551);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(44, 17);
            label4.TabIndex = 6;
            label4.Text = "注册码";
            // 
            // txtConcurrentUsers
            // 
            txtConcurrentUsers.Location = new System.Drawing.Point(134, 80);
            txtConcurrentUsers.Name = "txtConcurrentUsers";
            txtConcurrentUsers.Size = new System.Drawing.Size(100, 23);
            txtConcurrentUsers.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(31, 83);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(92, 17);
            label5.TabIndex = 8;
            label5.Text = "同时在线用户数";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(375, 84);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(56, 17);
            label6.TabIndex = 10;
            label6.Text = "截止时间";
            // 
            // txtProductVersion
            // 
            txtProductVersion.Location = new System.Drawing.Point(435, 114);
            txtProductVersion.Name = "txtProductVersion";
            txtProductVersion.Size = new System.Drawing.Size(160, 23);
            txtProductVersion.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(375, 118);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(56, 17);
            label7.TabIndex = 12;
            label7.Text = "版本信息";
            // 
            // dtpExpirationDate
            // 
            dtpExpirationDate.Location = new System.Drawing.Point(435, 80);
            dtpExpirationDate.Name = "dtpExpirationDate";
            dtpExpirationDate.Size = new System.Drawing.Size(160, 23);
            dtpExpirationDate.TabIndex = 14;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(67, 117);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(56, 17);
            label8.TabIndex = 15;
            label8.Text = "授权类型";
            // 
            // cmbLicenseType
            // 
            cmbLicenseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbLicenseType.FormattingEnabled = true;
            cmbLicenseType.Items.AddRange(new object[] { "试用版", "正式版" });
            cmbLicenseType.Location = new System.Drawing.Point(134, 113);
            cmbLicenseType.Name = "cmbLicenseType";
            cmbLicenseType.Size = new System.Drawing.Size(173, 25);
            cmbLicenseType.TabIndex = 16;
            cmbLicenseType.SelectedIndexChanged += cmbLicenseType_SelectedIndexChanged;
            // 
            // dtpPurchaseDate
            // 
            dtpPurchaseDate.Location = new System.Drawing.Point(134, 156);
            dtpPurchaseDate.Name = "dtpPurchaseDate";
            dtpPurchaseDate.Size = new System.Drawing.Size(173, 23);
            dtpPurchaseDate.TabIndex = 18;
            // 
            // lblPurchaseDate
            // 
            lblPurchaseDate.AutoSize = true;
            lblPurchaseDate.Location = new System.Drawing.Point(67, 159);
            lblPurchaseDate.Name = "lblPurchaseDate";
            lblPurchaseDate.Size = new System.Drawing.Size(56, 17);
            lblPurchaseDate.TabIndex = 17;
            lblPurchaseDate.Text = "购买日期";
            // 
            // dtpRegistrationDate
            // 
            dtpRegistrationDate.Location = new System.Drawing.Point(242, 519);
            dtpRegistrationDate.Name = "dtpRegistrationDate";
            dtpRegistrationDate.Size = new System.Drawing.Size(160, 23);
            dtpRegistrationDate.TabIndex = 20;
            // 
            // lblRegistrationDate
            // 
            lblRegistrationDate.AutoSize = true;
            lblRegistrationDate.Location = new System.Drawing.Point(182, 523);
            lblRegistrationDate.Name = "lblRegistrationDate";
            lblRegistrationDate.Size = new System.Drawing.Size(56, 17);
            lblRegistrationDate.TabIndex = 19;
            lblRegistrationDate.Text = "注册时间";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(187, 580);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(44, 17);
            label10.TabIndex = 21;
            label10.Text = "已注册";
            // 
            // chkIsRegistered
            // 
            chkIsRegistered.AutoSize = true;
            chkIsRegistered.Enabled = false;
            chkIsRegistered.Location = new System.Drawing.Point(242, 581);
            chkIsRegistered.Name = "chkIsRegistered";
            chkIsRegistered.Size = new System.Drawing.Size(15, 14);
            chkIsRegistered.TabIndex = 22;
            chkIsRegistered.UseVisualStyleBackColor = true;
            // 
            // txtRemarks
            // 
            txtRemarks.Location = new System.Drawing.Point(134, 195);
            txtRemarks.Multiline = true;
            txtRemarks.Name = "txtRemarks";
            txtRemarks.Size = new System.Drawing.Size(461, 49);
            txtRemarks.TabIndex = 24;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(91, 198);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(32, 17);
            label11.TabIndex = 23;
            label11.Text = "备注";
            // 
            // txtContactName
            // 
            txtContactName.Location = new System.Drawing.Point(134, 51);
            txtContactName.Name = "txtContactName";
            txtContactName.Size = new System.Drawing.Size(173, 23);
            txtContactName.TabIndex = 26;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(55, 57);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(68, 17);
            label12.TabIndex = 25;
            label12.Text = "联系人姓名";
            // 
            // btnRegister
            // 
            btnRegister.Location = new System.Drawing.Point(267, 577);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new System.Drawing.Size(75, 23);
            btnRegister.TabIndex = 27;
            btnRegister.Text = "注册";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbtnSaveRegInfo });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(997, 25);
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
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtCompanyName);
            groupBox1.Controls.Add(btnCreateRegInfo);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtPhoneNumber);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(txtRegInfo);
            groupBox1.Controls.Add(txtRemarks);
            groupBox1.Controls.Add(txtContactName);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(txtConcurrentUsers);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(txtProductVersion);
            groupBox1.Controls.Add(dtpExpirationDate);
            groupBox1.Controls.Add(dtpPurchaseDate);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(lblPurchaseDate);
            groupBox1.Controls.Add(cmbLicenseType);
            groupBox1.Location = new System.Drawing.Point(108, 65);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(816, 448);
            groupBox1.TabIndex = 30;
            groupBox1.TabStop = false;
            groupBox1.Text = "注册信息";
            // 
            // btnCreateRegInfo
            // 
            btnCreateRegInfo.Location = new System.Drawing.Point(611, 276);
            btnCreateRegInfo.Name = "btnCreateRegInfo";
            btnCreateRegInfo.Size = new System.Drawing.Size(89, 31);
            btnCreateRegInfo.TabIndex = 28;
            btnCreateRegInfo.Text = "生成注册信息";
            btnCreateRegInfo.UseVisualStyleBackColor = true;
            btnCreateRegInfo.Click += btnCreateRegInfo_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(67, 273);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(56, 17);
            label3.TabIndex = 23;
            label3.Text = "注册信息";
            // 
            // txtRegInfo
            // 
            txtRegInfo.Location = new System.Drawing.Point(134, 273);
            txtRegInfo.Multiline = true;
            txtRegInfo.Name = "txtRegInfo";
            txtRegInfo.Size = new System.Drawing.Size(461, 169);
            txtRegInfo.TabIndex = 24;
            // 
            // frmRegister
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(997, 644);
            Controls.Add(groupBox1);
            Controls.Add(toolStrip1);
            Controls.Add(btnRegister);
            Controls.Add(chkIsRegistered);
            Controls.Add(label10);
            Controls.Add(txtRegistrationCode);
            Controls.Add(label4);
            Controls.Add(dtpRegistrationDate);
            Controls.Add(lblRegistrationDate);
            Name = "frmRegister";
            Text = "注册信息";
            Load += frmRegister_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.Label lblRegistrationDate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkIsRegistered;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtContactName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnSaveRegInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCreateRegInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRegInfo;
    }
}