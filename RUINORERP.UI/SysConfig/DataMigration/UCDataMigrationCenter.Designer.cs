namespace RUINORERP.UI.SysConfig.DataMigration
{
    partial class UCDataMigrationCenter
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn选择文件 = new System.Windows.Forms.Button();
            this.txt文件路径 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tree依赖 = new System.Windows.Forms.TreeView();
            this.clb方案 = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView预览 = new System.Windows.Forms.DataGridView();
            this.btn预览 = new System.Windows.Forms.Button();
            this.btn执行导入 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView预览)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn选择文件);
            this.groupBox1.Controls.Add(this.txt文件路径);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 60);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. 选择数据源";
            // 
            // btn选择文件
            // 
            this.btn选择文件.Location = new System.Drawing.Point(680, 22);
            this.btn选择文件.Name = "btn选择文件";
            this.btn选择文件.Size = new System.Drawing.Size(80, 25);
            this.btn选择文件.TabIndex = 2;
            this.btn选择文件.Text = "浏览...";
            this.btn选择文件.UseVisualStyleBackColor = true;
            this.btn选择文件.Click += new System.EventHandler(this.btn选择文件_Click);
            // 
            // txt文件路径
            // 
            this.txt文件路径.Location = new System.Drawing.Point(80, 24);
            this.txt文件路径.Name = "txt文件路径";
            this.txt文件路径.ReadOnly = true;
            this.txt文件路径.Size = new System.Drawing.Size(580, 21);
            this.txt文件路径.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Excel文件:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tree依赖);
            this.groupBox2.Controls.Add(this.clb方案);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 78);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 200);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. 选择导入方案";
            // 
            // tree依赖
            // 
            this.tree依赖.Location = new System.Drawing.Point(400, 20);
            this.tree依赖.Name = "tree依赖";
            this.tree依赖.Size = new System.Drawing.Size(360, 170);
            this.tree依赖.TabIndex = 2;
            // 
            // clb方案
            // 
            this.clb方案.CheckOnClick = true;
            this.clb方案.FormattingEnabled = true;
            this.clb方案.Location = new System.Drawing.Point(80, 20);
            this.clb方案.Name = "clb方案";
            this.clb方案.Size = new System.Drawing.Size(300, 164);
            this.clb方案.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "导入方案:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView预览);
            this.groupBox3.Location = new System.Drawing.Point(12, 284);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(776, 250);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3. 数据预览";
            // 
            // dataGridView预览
            // 
            this.dataGridView预览.AllowUserToAddRows = false;
            this.dataGridView预览.AllowUserToDeleteRows = false;
            this.dataGridView预览.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView预览.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView预览.Location = new System.Drawing.Point(3, 17);
            this.dataGridView预览.Name = "dataGridView预览";
            this.dataGridView预览.ReadOnly = true;
            this.dataGridView预览.RowTemplate.Height = 23;
            this.dataGridView预览.Size = new System.Drawing.Size(770, 230);
            this.dataGridView预览.TabIndex = 0;
            // 
            // btn预览
            // 
            this.btn预览.Location = new System.Drawing.Point(600, 540);
            this.btn预览.Name = "btn预览";
            this.btn预览.Size = new System.Drawing.Size(90, 35);
            this.btn预览.TabIndex = 3;
            this.btn预览.Text = "数据预览";
            this.btn预览.UseVisualStyleBackColor = true;
            this.btn预览.Click += new System.EventHandler(this.btn预览_Click);
            // 
            // btn执行导入
            // 
            this.btn执行导入.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn执行导入.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn执行导入.ForeColor = System.Drawing.Color.White;
            this.btn执行导入.Location = new System.Drawing.Point(700, 540);
            this.btn执行导入.Name = "btn执行导入";
            this.btn执行导入.Size = new System.Drawing.Size(90, 35);
            this.btn执行导入.TabIndex = 4;
            this.btn执行导入.Text = "执行导入";
            this.btn执行导入.UseVisualStyleBackColor = false;
            this.btn执行导入.Click += new System.EventHandler(this.btn执行导入_Click);
            // 
            // UCDataMigrationCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn执行导入);
            this.Controls.Add(this.btn预览);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "UCDataMigrationCenter";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.UCDataMigrationCenter_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView预览)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn选择文件;
        private System.Windows.Forms.TextBox txt文件路径;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView tree依赖;
        private System.Windows.Forms.CheckedListBox clb方案;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView预览;
        private System.Windows.Forms.Button btn预览;
        private System.Windows.Forms.Button btn执行导入;
    }
}
