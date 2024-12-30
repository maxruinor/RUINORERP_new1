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
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(73, 415);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(75, 23);
            this.btnEncrypt.TabIndex = 19;
            this.btnEncrypt.Text = "加密(&E)";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(154, 415);
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
            this.txtKey.Size = new System.Drawing.Size(555, 83);
            this.txtKey.TabIndex = 25;
            // 
            // listboxSecurityItems
            // 
            this.listboxSecurityItems.FormattingEnabled = true;
            this.listboxSecurityItems.ItemHeight = 12;
            this.listboxSecurityItems.Items.AddRange(new object[] {
            "connstring"});
            this.listboxSecurityItems.Location = new System.Drawing.Point(700, 16);
            this.listboxSecurityItems.Name = "listboxSecurityItems";
            this.listboxSecurityItems.Size = new System.Drawing.Size(214, 316);
            this.listboxSecurityItems.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(447, 390);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(419, 60);
            this.label4.TabIndex = 28;
            this.label4.Text = "注册码思路：\r\n1)加密\r\n由硬件标识+注册信息（人数，截止日期）的信息配合注册码（key)生成机器码\r\n2）验证\r\n将注册码和机器码解密后对比硬件标识和注册信息" +
    "。不符合就退出。\r\n";
            // 
            // frmRUINORERPSecurity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 482);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listboxSecurityItems);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnEncrypt);
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
    }
}