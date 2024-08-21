namespace CommonProcess.StringProcess
{
    partial class UCHtmltagProcess
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
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnNoSelectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lvForHtml = new System.Windows.Forms.ListView();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnNoSelectAll);
            this.groupBox8.Controls.Add(this.btnSelectAll);
            this.groupBox8.Controls.Add(this.lvForHtml);
            this.groupBox8.Location = new System.Drawing.Point(45, 31);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(551, 374);
            this.groupBox8.TabIndex = 118;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "html标签处理";
            // 
            // btnNoSelectAll
            // 
            this.btnNoSelectAll.Location = new System.Drawing.Point(455, 292);
            this.btnNoSelectAll.Name = "btnNoSelectAll";
            this.btnNoSelectAll.Size = new System.Drawing.Size(75, 36);
            this.btnNoSelectAll.TabIndex = 0;
            this.btnNoSelectAll.Text = "全空";
            this.btnNoSelectAll.UseVisualStyleBackColor = true;
            this.btnNoSelectAll.Click += new System.EventHandler(this.btnNoSelectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(354, 292);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 36);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // lvForHtml
            // 
            this.lvForHtml.CheckBoxes = true;
            this.lvForHtml.Location = new System.Drawing.Point(27, 20);
            this.lvForHtml.Name = "lvForHtml";
            this.lvForHtml.ShowGroups = false;
            this.lvForHtml.Size = new System.Drawing.Size(503, 245);
            this.lvForHtml.TabIndex = 59;
            this.lvForHtml.UseCompatibleStateImageBehavior = false;
            // 
            // UCHtmltagProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox8);
            this.Name = "UCHtmltagProcess";
            this.Size = new System.Drawing.Size(837, 563);
            this.Load += new System.EventHandler(this.UCHtmltagProcess_Load);
            this.groupBox8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btnNoSelectAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.ListView lvForHtml;
    }
}
