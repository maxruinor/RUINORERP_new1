namespace RUINORERP.UI.IM
{
    partial class MoveForm
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
            this.moveUpTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.mouseStateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // moveUpTimer
            // 
            this.moveUpTimer.Interval = 50;
            this.moveUpTimer.Tick += new System.EventHandler(this.moveUpTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // mouseStateTimer
            // 
            this.mouseStateTimer.Interval = 50;
            // 
            // MoveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::RUINORERP.UI.Properties.Resources.logo11;
            this.ClientSize = new System.Drawing.Size(358, 342);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MoveForm";
            this.ShowInTaskbar = false;
            this.Text = "MoveForm";
            this.Load += new System.EventHandler(this.MoveForm_Load);
            this.MouseEnter += new System.EventHandler(this.MoveForm_MouseEnter);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MoveForm_FormClosed);
            this.MouseLeave += new System.EventHandler(this.MoveForm_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer moveUpTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer mouseStateTimer;
    }
}