namespace RUINORERP.UI.ProductEAV
{
    partial class UCSkuManagement
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.lblSkuCount = new System.Windows.Forms.Label();
            this.btnGenerateSku = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            
            // dataGridView
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(10, 40);
            this.dataGridView.Name = \"dataGridView\";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(730, 550);
            this.dataGridView.TabIndex = 0;
            
            // lblSkuCount
            this.lblSkuCount.AutoSize = true;
            this.lblSkuCount.Location = new System.Drawing.Point(10, 10);
            this.lblSkuCount.Name = \"lblSkuCount\";
            this.lblSkuCount.Size = new System.Drawing.Size(59, 12);
            this.lblSkuCount.TabIndex = 1;
            this.lblSkuCount.Text = \"暂无SKU\";
            
            // btnGenerateSku
            this.btnGenerateSku.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateSku.Location = new System.Drawing.Point(620, 8);
            this.btnGenerateSku.Name = \"btnGenerateSku\";
            this.btnGenerateSku.Size = new System.Drawing.Size(120, 25);
            this.btnGenerateSku.TabIndex = 2;
            this.btnGenerateSku.Text = \"生成缺失的SKU\";
            this.btnGenerateSku.Click += new System.EventHandler(this.btnGenerateSku_Click);
            
            // UCSkuManagement
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGenerateSku);
            this.Controls.Add(this.lblSkuCount);
            this.Controls.Add(this.dataGridView);
            this.Name = \"UCSkuManagement\";
            this.Size = new System.Drawing.Size(750, 600);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label lblSkuCount;
        private System.Windows.Forms.Button btnGenerateSku;
    }
}
