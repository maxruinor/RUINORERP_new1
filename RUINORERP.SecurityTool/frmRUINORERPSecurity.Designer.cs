namespace RUINORERP.SecurityTool
{
    partial class frmRUINORERPSecurity
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
            this.label3 = new System.Windows.Forms.Label();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.txtOldData = new System.Windows.Forms.TextBox();
            this.txtNewData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.listboxSecurityItems = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCreateRegCode = new System.Windows.Forms.Button();
            this.txtRegCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "Key:";
            this.label3.Visible = false;
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(839, 338);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(75, 23);
            this.btnEncrypt.TabIndex = 19;
            this.btnEncrypt.Text = "加密(&E)";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Visible = false;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(636, 16);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(75, 23);
            this.btnDecrypt.TabIndex = 20;
            this.btnDecrypt.Text = "解密(&D)";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // txtOldData
            // 
            this.txtOldData.Location = new System.Drawing.Point(75, 13);
            this.txtOldData.Multiline = true;
            this.txtOldData.Name = "txtOldData";
            this.txtOldData.Size = new System.Drawing.Size(555, 100);
            this.txtOldData.TabIndex = 21;
            // 
            // txtNewData
            // 
            this.txtNewData.Location = new System.Drawing.Point(75, 119);
            this.txtNewData.Multiline = true;
            this.txtNewData.Name = "txtNewData";
            this.txtNewData.Size = new System.Drawing.Size(555, 115);
            this.txtNewData.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "原始数据:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-20, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "加/解密后数据:";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(75, 252);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(555, 44);
            this.txtKey.TabIndex = 25;
            this.txtKey.Visible = false;
            // 
            // listboxSecurityItems
            // 
            this.listboxSecurityItems.FormattingEnabled = true;
            this.listboxSecurityItems.ItemHeight = 12;
            this.listboxSecurityItems.Items.AddRange(new object[] {
            "connstring"});
            this.listboxSecurityItems.Location = new System.Drawing.Point(743, 16);
            this.listboxSecurityItems.Name = "listboxSecurityItems";
            this.listboxSecurityItems.Size = new System.Drawing.Size(171, 316);
            this.listboxSecurityItems.TabIndex = 27;
            this.listboxSecurityItems.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(73, 474);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(419, 60);
            this.label4.TabIndex = 28;
            this.label4.Text = "注册码思路：\r\n1)加密\r\n由硬件标识+注册信息（人数，截止日期）的信息配合注册码（key)生成机器码\r\n2）验证\r\n将注册码和机器码解密后对比硬件标识和注册信息" +
    "。不符合就退出。\r\n";
            // 
            // btnCreateRegCode
            // 
            this.btnCreateRegCode.Location = new System.Drawing.Point(636, 338);
            this.btnCreateRegCode.Name = "btnCreateRegCode";
            this.btnCreateRegCode.Size = new System.Drawing.Size(94, 23);
            this.btnCreateRegCode.TabIndex = 20;
            this.btnCreateRegCode.Text = "生成注册码(&R)";
            this.btnCreateRegCode.UseVisualStyleBackColor = true;
            this.btnCreateRegCode.Click += new System.EventHandler(this.btnCreateRegCode_Click);
            // 
            // txtRegCode
            // 
            this.txtRegCode.Location = new System.Drawing.Point(77, 338);
            this.txtRegCode.Multiline = true;
            this.txtRegCode.Name = "txtRegCode";
            this.txtRegCode.Size = new System.Drawing.Size(553, 98);
            this.txtRegCode.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 338);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 26;
            this.label5.Text = "注册码:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(634, 400);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 36);
            this.label6.TabIndex = 30;
            this.label6.Text = "操作步骤：\r\n1)解密查看注册信息否正常\r\n2）生成注册码发给用户\r\n";
            // 
            // frmRUINORERPSecurity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 543);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtRegCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listboxSecurityItems);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.btnCreateRegCode);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.txtOldData);
            this.Controls.Add(this.txtNewData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtKey);
            this.Name = "frmRUINORERPSecurity";
            this.Text = "内部使用";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.TextBox txtOldData;
        private System.Windows.Forms.TextBox txtNewData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.ListBox listboxSecurityItems;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCreateRegCode;
        private System.Windows.Forms.TextBox txtRegCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}