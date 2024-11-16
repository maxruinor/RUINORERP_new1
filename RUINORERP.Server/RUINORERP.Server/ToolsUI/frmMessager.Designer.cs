namespace RUINORERP.Server.ToolsUI
{
    partial class frmMessager
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
            btnOK = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            txtMessage = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            chkMustDisplay = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Location = new System.Drawing.Point(144, 394);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 23);
            btnOK.TabIndex = 0;
            btnOK.Text = "确定";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(330, 394);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // txtMessage
            // 
            txtMessage.Location = new System.Drawing.Point(82, 27);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new System.Drawing.Size(431, 287);
            txtMessage.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 27);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(56, 17);
            label1.TabIndex = 3;
            label1.Text = "消息内容";
            // 
            // chkMustDisplay
            // 
            chkMustDisplay.AutoSize = true;
            chkMustDisplay.Location = new System.Drawing.Point(82, 345);
            chkMustDisplay.Name = "chkMustDisplay";
            chkMustDisplay.Size = new System.Drawing.Size(123, 21);
            chkMustDisplay.TabIndex = 6;
            chkMustDisplay.Text = "强制显示给客户端";
            chkMustDisplay.UseVisualStyleBackColor = true;
            // 
            // frmMessager
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(567, 429);
            Controls.Add(chkMustDisplay);
            Controls.Add(label1);
            Controls.Add(txtMessage);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Name = "frmMessager";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "消息发送框";
            Load += frmMessager_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkMustDisplay;
    }
}