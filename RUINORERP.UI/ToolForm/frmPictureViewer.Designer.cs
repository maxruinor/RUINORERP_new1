
namespace RUINORERP.UI.ToolForm
{
    partial class frmPictureViewer
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
            this.PictureBoxViewer = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBoxViewer
            // 
            this.PictureBoxViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PictureBoxViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBoxViewer.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxViewer.Name = "PictureBoxViewer";
            this.PictureBoxViewer.Size = new System.Drawing.Size(732, 698);
            this.PictureBoxViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBoxViewer.TabIndex = 0;
            this.PictureBoxViewer.TabStop = false;
            // 
            // frmPictureViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(732, 698);
            this.Controls.Add(this.PictureBoxViewer);
            this.DoubleBuffered = true;
            this.MinimizeBox = false;
            this.Name = "frmPictureViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "可放大缩小拖动的图片查看器";
            this.Load += new System.EventHandler(this.frmPictureViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxViewer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox PictureBoxViewer;
    }
}