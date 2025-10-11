namespace RUINORERP.Plugin.OfficeAssistant
{
    partial class ComparisonResultForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageAdded = new System.Windows.Forms.TabPage();
            this.dataGridViewAdded = new System.Windows.Forms.DataGridView();
            this.tabPageDeleted = new System.Windows.Forms.TabPage();
            this.dataGridViewDeleted = new System.Windows.Forms.DataGridView();
            this.tabPageModified = new System.Windows.Forms.TabPage();
            this.dataGridViewModified = new System.Windows.Forms.DataGridView();
            this.tabControl.SuspendLayout();
            this.tabPageAdded.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAdded)).BeginInit();
            this.tabPageDeleted.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeleted)).BeginInit();
            this.tabPageModified.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModified)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageAdded);
            this.tabControl.Controls.Add(this.tabPageDeleted);
            this.tabControl.Controls.Add(this.tabPageModified);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 450);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageAdded
            // 
            this.tabPageAdded.Controls.Add(this.dataGridViewAdded);
            this.tabPageAdded.Location = new System.Drawing.Point(4, 22);
            this.tabPageAdded.Name = "tabPageAdded";
            this.tabPageAdded.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAdded.Size = new System.Drawing.Size(792, 424);
            this.tabPageAdded.TabIndex = 0;
            this.tabPageAdded.Text = "新数据有旧数据没有";
            this.tabPageAdded.UseVisualStyleBackColor = true;
            // 
            // dataGridViewAdded
            // 
            this.dataGridViewAdded.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAdded.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAdded.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewAdded.Name = "dataGridViewAdded";
            this.dataGridViewAdded.Size = new System.Drawing.Size(786, 418);
            this.dataGridViewAdded.TabIndex = 0;
            // 
            // tabPageDeleted
            // 
            this.tabPageDeleted.Controls.Add(this.dataGridViewDeleted);
            this.tabPageDeleted.Location = new System.Drawing.Point(4, 22);
            this.tabPageDeleted.Name = "tabPageDeleted";
            this.tabPageDeleted.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDeleted.Size = new System.Drawing.Size(792, 424);
            this.tabPageDeleted.TabIndex = 1;
            this.tabPageDeleted.Text = "旧数据有新数据没有";
            this.tabPageDeleted.UseVisualStyleBackColor = true;
            // 
            // dataGridViewDeleted
            // 
            this.dataGridViewDeleted.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDeleted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDeleted.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewDeleted.Name = "dataGridViewDeleted";
            this.dataGridViewDeleted.Size = new System.Drawing.Size(786, 418);
            this.dataGridViewDeleted.TabIndex = 0;
            // 
            // tabPageModified
            // 
            this.tabPageModified.Controls.Add(this.dataGridViewModified);
            this.tabPageModified.Location = new System.Drawing.Point(4, 22);
            this.tabPageModified.Name = "tabPageModified";
            this.tabPageModified.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageModified.Size = new System.Drawing.Size(792, 424);
            this.tabPageModified.TabIndex = 2;
            this.tabPageModified.Text = "数据发生变化";
            this.tabPageModified.UseVisualStyleBackColor = true;
            // 
            // dataGridViewModified
            // 
            this.dataGridViewModified.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewModified.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewModified.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewModified.Name = "dataGridViewModified";
            this.dataGridViewModified.Size = new System.Drawing.Size(786, 418);
            this.dataGridViewModified.TabIndex = 0;
            // 
            // ComparisonResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl);
            this.Name = "ComparisonResultForm";
            this.Text = "数据对比详细结果";
            this.tabControl.ResumeLayout(false);
            this.tabPageAdded.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAdded)).EndInit();
            this.tabPageDeleted.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeleted)).EndInit();
            this.tabPageModified.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModified)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageAdded;
        private System.Windows.Forms.DataGridView dataGridViewAdded;
        private System.Windows.Forms.TabPage tabPageDeleted;
        private System.Windows.Forms.DataGridView dataGridViewDeleted;
        private System.Windows.Forms.TabPage tabPageModified;
        private System.Windows.Forms.DataGridView dataGridViewModified;
    }
}