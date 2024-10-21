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
            this.magicPictureBox1 = new RUINOR.WinFormsUI.CustomPictureBox.MagicPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.magicPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // magicPictureBox1
            // 
            this.magicPictureBox1.AllowDrop = true;
            this.magicPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.magicPictureBox1.Location = new System.Drawing.Point(12, 12);
            this.magicPictureBox1.Name = "magicPictureBox1";
            this.magicPictureBox1.Size = new System.Drawing.Size(469, 426);
            this.magicPictureBox1.TabIndex = 0;
            this.magicPictureBox1.TabStop = false;
            // 
            // frmPicDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.magicPictureBox1);
            this.Name = "frmPicDemo";
            this.Text = "frmPicDemo";
            ((System.ComponentModel.ISupportInitialize)(this.magicPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomPictureBox.MagicPictureBox magicPictureBox1;
    }
}