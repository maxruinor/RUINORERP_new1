namespace RUINORERP.SecurityTool
{
    partial class frmEncryptionDecryption
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
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.txtOldData = new System.Windows.Forms.TextBox();
            this.txtNewData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(106, 414);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(75, 23);
            this.btnEncrypt.TabIndex = 8;
            this.btnEncrypt.Text = "加密(&E)";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(187, 414);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(75, 23);
            this.btnDecrypt.TabIndex = 9;
            this.btnDecrypt.Text = "解密(&D)";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // txtOldData
            // 
            this.txtOldData.Location = new System.Drawing.Point(108, 12);
            this.txtOldData.Multiline = true;
            this.txtOldData.Name = "txtOldData";
            this.txtOldData.Size = new System.Drawing.Size(555, 100);
            this.txtOldData.TabIndex = 10;
            // 
            // txtNewData
            // 
            this.txtNewData.Location = new System.Drawing.Point(108, 118);
            this.txtNewData.Multiline = true;
            this.txtNewData.Name = "txtNewData";
            this.txtNewData.Size = new System.Drawing.Size(555, 115);
            this.txtNewData.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "原始数据:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "加/解密后数据:";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(108, 259);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(555, 83);
            this.txtKey.TabIndex = 14;
            this.txtKey.Text = "ruinor1234567890";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "Key:";
            // 
            // frmEncryptionDecryption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 458);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.txtOldData);
            this.Controls.Add(this.txtNewData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtKey);
            this.Name = "frmEncryptionDecryption";
            this.Text = "frmEncryptionDecryption";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.TextBox txtOldData;
        private System.Windows.Forms.TextBox txtNewData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label label3;
    }
}