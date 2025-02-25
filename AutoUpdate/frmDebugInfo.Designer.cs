namespace AutoUpdate
{
    partial class frmDebugInfo
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
            this.richtxt = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richtxt
            // 
            this.richtxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richtxt.Location = new System.Drawing.Point(0, 0);
            this.richtxt.Name = "richtxt";
            this.richtxt.Size = new System.Drawing.Size(800, 450);
            this.richtxt.TabIndex = 12;
            this.richtxt.Text = "";
            // 
            // frmDebugInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richtxt);
            this.Name = "frmDebugInfo";
            this.Text = "frmDebugInfo";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.RichTextBox richtxt;
    }
}