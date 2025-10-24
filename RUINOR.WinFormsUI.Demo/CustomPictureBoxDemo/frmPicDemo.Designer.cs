namespace RUINOR.WinFormsUI.Demo.CustomPictureBoxDemo
{
    partial class frmPicDemo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPicDemo));
            this.ucMagicPictrueBoxes1 = new RUINOR.WinFormsUI.CustomPictureBox.UCMagicPictrueBoxes();
            this.magicPictureBox1 = new RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.magicPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ucMagicPictrueBoxes1
            // 
            this.ucMagicPictrueBoxes1.AllowUpload = true;
            this.ucMagicPictrueBoxes1.ImagePaths = "";
            this.ucMagicPictrueBoxes1.Location = new System.Drawing.Point(510, 12);
            this.ucMagicPictrueBoxes1.MaxImageCount = 10;
            this.ucMagicPictrueBoxes1.Name = "ucMagicPictrueBoxes1";
            this.ucMagicPictrueBoxes1.Size = new System.Drawing.Size(387, 426);
            this.ucMagicPictrueBoxes1.TabIndex = 1;
            // 
            // magicPictureBox1
            // 
            this.magicPictureBox1.AllowDrop = true;
            this.magicPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magicPictureBox1.ImagePaths = "";
            this.magicPictureBox1.Location = new System.Drawing.Point(12, 12);
            this.magicPictureBox1.MultiImageSupport = true;
            this.magicPictureBox1.Name = "magicPictureBox1";
            this.magicPictureBox1.RowImage = ((RUINORERP.Global.Model.DataRowImage)(resources.GetObject("magicPictureBox1.RowImage")));
            this.magicPictureBox1.Size = new System.Drawing.Size(469, 426);
            this.magicPictureBox1.TabIndex = 0;
            this.magicPictureBox1.TabStop = false;
            // 
            // frmPicDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 598);
            this.Controls.Add(this.ucMagicPictrueBoxes1);
            this.Controls.Add(this.magicPictureBox1);
            this.Name = "frmPicDemo";
            this.Text = "frmPicDemo";
            ((System.ComponentModel.ISupportInitialize)(this.magicPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomPictureBox.MagicPictureBox magicPictureBox1;
        private CustomPictureBox.UCMagicPictrueBoxes ucMagicPictrueBoxes1;
    }
}