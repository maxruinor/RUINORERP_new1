namespace RUINORERP.UI.UCSourceGrid
{
    partial class QueryFormGeneric
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
            this.kryptonPanelQuery = new Krypton.Toolkit.KryptonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanelQuery
            // 
            this.kryptonPanelQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelQuery.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelQuery.Name = "kryptonPanelQuery";
            this.kryptonPanelQuery.Size = new System.Drawing.Size(1004, 730);
            this.kryptonPanelQuery.TabIndex = 1;
            // 
            // QueryFormGeneric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 730);
            this.Controls.Add(this.kryptonPanelQuery);
            this.Name = "QueryFormGeneric";
            this.Text = "QueryFormGeneric";
            this.Load += new System.EventHandler(this.QueryFormGeneric_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal Krypton.Toolkit.KryptonPanel kryptonPanelQuery;
    }
}