namespace RUINORERP.UI.ProductEAV
{
    partial class frmSKUImageEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.magicPictureBox = new RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnOK = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // magicPictureBox
            // 
            this.magicPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magicPictureBox.Location = new System.Drawing.Point(0, 0);
            this.magicPictureBox.Name = "magicPictureBox";
            this.magicPictureBox.Size = new System.Drawing.Size(800, 540);
            this.magicPictureBox.TabIndex = 0;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 540);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(800, 60);
            this.pnlButtons.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(550, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 35);
            this.btnOK.TabIndex = 0;
            this.btnOK.Values.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(670, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSKUImageEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.magicPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSKUImageEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SKU图片编辑";
            this.Load += new System.EventHandler(this.frmSKUImageEdit_Load);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// MagicPictureBox控件
        /// </summary>
        private RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox magicPictureBox;

        /// <summary>
        /// 按钮面板
        /// </summary>
        private System.Windows.Forms.Panel pnlButtons;

        /// <summary>
        /// 确定按钮
        /// </summary>
        private Krypton.Toolkit.KryptonButton btnOK;

        /// <summary>
        /// 取消按钮
        /// </summary>
        private Krypton.Toolkit.KryptonButton btnCancel;
    }
}
