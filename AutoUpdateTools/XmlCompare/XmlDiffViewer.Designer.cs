namespace AutoUpdateTools
{
    partial class XmlDiffViewer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.leftBox = new System.Windows.Forms.RichTextBox();
            this.rightBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.leftBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rightBox);
            this.splitContainer1.Size = new System.Drawing.Size(1113, 803);
            this.splitContainer1.SplitterDistance = 540;
            this.splitContainer1.TabIndex = 0;
            // 
            // leftBox
            // 
            this.leftBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftBox.Location = new System.Drawing.Point(0, 0);
            this.leftBox.Name = "leftBox";
            this.leftBox.Size = new System.Drawing.Size(540, 803);
            this.leftBox.TabIndex = 0;
            this.leftBox.Text = "";
            // 
            // rightBox
            // 
            this.rightBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightBox.Location = new System.Drawing.Point(0, 0);
            this.rightBox.Name = "rightBox";
            this.rightBox.Size = new System.Drawing.Size(569, 803);
            this.rightBox.TabIndex = 0;
            this.rightBox.Text = "";
            // 
            // XmlDiffViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "XmlDiffViewer";
            this.Size = new System.Drawing.Size(1113, 803);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox leftBox;
        private System.Windows.Forms.RichTextBox rightBox;
    }
}
