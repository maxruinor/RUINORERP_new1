namespace RUINOR.WinFormsUI.CustomPictureBox.Implementations
{
    partial class SimpleImageUploadProgressForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTotalProgress;
        private System.Windows.Forms.ProgressBar progressBarOverall;
        private System.Windows.Forms.Label lblSuccessCount;
        private System.Windows.Forms.Label lblFailedCount;
        private System.Windows.Forms.ListView listViewUploads;

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
            this.lblTotalProgress = new System.Windows.Forms.Label();
            this.progressBarOverall = new System.Windows.Forms.ProgressBar();
            this.lblSuccessCount = new System.Windows.Forms.Label();
            this.lblFailedCount = new System.Windows.Forms.Label();
            this.listViewUploads = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            
            this.lblTotalProgress.AutoSize = true;
            this.lblTotalProgress.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblTotalProgress.Location = new System.Drawing.Point(20, 20);
            this.lblTotalProgress.Name = "lblTotalProgress";
            this.lblTotalProgress.Size = new System.Drawing.Size(200, 24);
            this.lblTotalProgress.TabIndex = 0;
            this.lblTotalProgress.Text = "总体进度: 0% (0/0)";
            
            this.progressBarOverall.Location = new System.Drawing.Point(20, 60);
            this.progressBarOverall.Name = "progressBarOverall";
            this.progressBarOverall.Size = new System.Drawing.Size(560, 30);
            this.progressBarOverall.TabIndex = 1;
            
            this.lblSuccessCount.AutoSize = true;
            this.lblSuccessCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblSuccessCount.ForeColor = System.Drawing.Color.Green;
            this.lblSuccessCount.Location = new System.Drawing.Point(20, 100);
            this.lblSuccessCount.Name = "lblSuccessCount";
            this.lblSuccessCount.Size = new System.Drawing.Size(80, 24);
            this.lblSuccessCount.TabIndex = 2;
            this.lblSuccessCount.Text = "成功: 0";
            
            this.lblFailedCount.AutoSize = true;
            this.lblFailedCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblFailedCount.ForeColor = System.Drawing.Color.Red;
            this.lblFailedCount.Location = new System.Drawing.Point(240, 100);
            this.lblFailedCount.Name = "lblFailedCount";
            this.lblFailedCount.Size = new System.Drawing.Size(80, 24);
            this.lblFailedCount.TabIndex = 3;
            this.lblFailedCount.Text = "失败: 0";
            
            this.listViewUploads.FullRowSelect = false;
            this.listViewUploads.GridLines = true;
            this.listViewUploads.Location = new System.Drawing.Point(20, 140);
            this.listViewUploads.Name = "listViewUploads";
            this.listViewUploads.Size = new System.Drawing.Size(560, 220);
            this.listViewUploads.TabIndex = 4;
            this.listViewUploads.UseCompatibleStateImageBehavior = false;
            this.listViewUploads.View = System.Windows.Forms.View.Details;
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.listViewUploads);
            this.Controls.Add(this.lblFailedCount);
            this.Controls.Add(this.lblSuccessCount);
            this.Controls.Add(this.progressBarOverall);
            this.Controls.Add(this.lblTotalProgress);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "SimpleImageUploadProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图片上传进度";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}