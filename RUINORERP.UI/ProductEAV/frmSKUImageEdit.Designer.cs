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
            this.components = new System.ComponentModel.Container();
            this.magicPictureBox = new RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblInfo = new Krypton.Toolkit.KryptonLabel();
            this.lblStatus = new Krypton.Toolkit.KryptonLabel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOK = new Krypton.Toolkit.KryptonButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTop.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // magicPictureBox
            // 
            this.magicPictureBox.BackColor = System.Drawing.Color.White;
            this.magicPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magicPictureBox.Location = new System.Drawing.Point(0, 45);
            this.magicPictureBox.Name = "magicPictureBox";
            this.magicPictureBox.Size = new System.Drawing.Size(850, 505);
            this.magicPictureBox.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlTop.Controls.Add(this.lblStatus);
            this.pnlTop.Controls.Add(this.lblInfo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(10);
            this.pnlTop.Size = new System.Drawing.Size(850, 45);
            this.pnlTop.TabIndex = 1;
            // 
            // lblInfo
            // 
            this.lblInfo.Location = new System.Drawing.Point(10, 10);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(300, 25);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Values.Text = "提示：支持拖拽上传、Ctrl+V粘贴截图、双击查看大图";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(350, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(150, 25);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Values.Text = "图片数量：0 | 已修改：0";
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 550);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(10);
            this.pnlButtons.Size = new System.Drawing.Size(850, 60);
            this.pnlButtons.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(628, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 35);
            this.btnOK.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnOK.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.btnOK.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnOK.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.TabIndex = 0;
            this.btnOK.Values.Text = "确定(&S)";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(738, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnCancel.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSKUImageEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 610);
            this.Controls.Add(this.magicPictureBox);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlButtons);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "frmSKUImageEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SKU图片编辑";
            this.Load += new System.EventHandler(this.frmSKUImageEdit_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// MagicPictureBox控件
        /// </summary>
        private RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox magicPictureBox;

        /// <summary>
        /// 顶部信息面板
        /// </summary>
        private System.Windows.Forms.Panel pnlTop;

        /// <summary>
        /// 操作提示标签
        /// </summary>
        private Krypton.Toolkit.KryptonLabel lblInfo;

        /// <summary>
        /// 状态标签
        /// </summary>
        private Krypton.Toolkit.KryptonLabel lblStatus;

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

        /// <summary>
        /// 工具提示
        /// </summary>
        private System.Windows.Forms.ToolTip toolTip;
    }
}
