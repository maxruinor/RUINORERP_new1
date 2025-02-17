namespace RUINORERP.Server
{
    partial class frmPasswordInput
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
            txtSupperPassword = new System.Windows.Forms.TextBox();
            btnOK = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // txtSupperPassword
            // 
            txtSupperPassword.Location = new System.Drawing.Point(7, 18);
            txtSupperPassword.Name = "txtSupperPassword";
            txtSupperPassword.PasswordChar = '*';
            txtSupperPassword.Size = new System.Drawing.Size(251, 23);
            txtSupperPassword.TabIndex = 0;
            // 
            // btnOK
            // 
            btnOK.Location = new System.Drawing.Point(277, 18);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 23);
            btnOK.TabIndex = 1;
            btnOK.Text = "确定";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += button1_Click;
            // 
            // frmPasswordInput
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(391, 71);
            Controls.Add(btnOK);
            Controls.Add(txtSupperPassword);
            Name = "frmPasswordInput";
            Text = "请输入超级密码";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtSupperPassword;
        private System.Windows.Forms.Button btnOK;
    }
}