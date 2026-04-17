namespace RUINORERP.UI.DevTools
{
    partial class UCDataCleanupTool
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
            this.cmb清理模式 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb目标数据表 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chk模拟运行 = new System.Windows.Forms.CheckBox();
            this.btn快速示例 = new System.Windows.Forms.Button();
            this.txt清理条件 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt确认码 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn执行清理 = new System.Windows.Forms.Button();
            this.dataGridView预览 = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView预览)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmb清理模式);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmb目标数据表);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(600, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. 选择清理目标";
            // 
            // cmb清理模式
            // 
            this.cmb清理模式.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb清理模式.FormattingEnabled = true;
            this.cmb清理模式.Items.AddRange(new object[] {
            "基础数据清理",
            "单据数据清理"});
            this.cmb清理模式.Location = new System.Drawing.Point(350, 45);
            this.cmb清理模式.Name = "cmb清理模式";
            this.cmb清理模式.Size = new System.Drawing.Size(120, 20);
            this.cmb清理模式.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(300, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "模式:";
            // 
            // cmb目标数据表
            // 
            this.cmb目标数据表.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb目标数据表.FormattingEnabled = true;
            this.cmb目标数据表.Location = new System.Drawing.Point(80, 45);
            this.cmb目标数据表.Name = "cmb目标数据表";
            this.cmb目标数据表.Size = new System.Drawing.Size(200, 20);
            this.cmb目标数据表.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "目标数据表:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chk模拟运行);
            this.groupBox2.Controls.Add(this.btn快速示例);
            this.groupBox2.Controls.Add(this.txt清理条件);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 120);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. 设置清理条件 (SQL WHERE 子句)";
            // 
            // chk模拟运行
            // 
            this.chk模拟运行.AutoSize = true;
            this.chk模拟运行.Checked = true;
            this.chk模拟运行.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk模拟运行.Location = new System.Drawing.Point(12, 90);
            this.chk模拟运行.Name = "chk模拟运行";
            this.chk模拟运行.Size = new System.Drawing.Size(150, 16);
            this.chk模拟运行.TabIndex = 3;
            this.chk模拟运行.Text = "模拟运行 (仅统计行数)";
            this.chk模拟运行.UseVisualStyleBackColor = true;
            // 
            // btn快速示例
            // 
            this.btn快速示例.Location = new System.Drawing.Point(500, 85);
            this.btn快速示例.Name = "btn快速示例";
            this.btn快速示例.Size = new System.Drawing.Size(80, 25);
            this.btn快速示例.TabIndex = 2;
            this.btn快速示例.Text = "填入示例";
            this.btn快速示例.UseVisualStyleBackColor = true;
            this.btn快速示例.Click += new System.EventHandler(this.btn快速示例_Click);
            // 
            // txt清理条件
            // 
            this.txt清理条件.Location = new System.Drawing.Point(12, 30);
            this.txt清理条件.Multiline = true;
            this.txt清理条件.Name = "txt清理条件";
            this.txt清理条件.Size = new System.Drawing.Size(570, 50);
            this.txt清理条件.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(10, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(275, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "注意: 留空将清空整张表! 请输入合法的 SQL 条件";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt确认码);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(12, 224);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(600, 60);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3. 安全确认";
            // 
            // txt确认码
            // 
            this.txt确认码.Location = new System.Drawing.Point(80, 25);
            this.txt确认码.Name = "txt确认码";
            this.txt确认码.Size = new System.Drawing.Size(100, 21);
            this.txt确认码.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "输入 DELETE:";
            // 
            // btn执行清理
            // 
            this.btn执行清理.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btn执行清理.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold);
            this.btn执行清理.ForeColor = System.Drawing.Color.White;
            this.btn执行清理.Location = new System.Drawing.Point(450, 290);
            this.btn执行清理.Name = "btn执行清理";
            this.btn执行清理.Size = new System.Drawing.Size(150, 40);
            this.btn执行清理.TabIndex = 3;
            this.btn执行清理.Text = "⚠ 执行清理";
            this.btn执行清理.UseVisualStyleBackColor = false;
            this.btn执行清理.Click += new System.EventHandler(this.btn执行清理_Click);
            // 
            // dataGridView预览
            // 
            this.dataGridView预览.AllowUserToAddRows = false;
            this.dataGridView预览.AllowUserToDeleteRows = false;
            this.dataGridView预览.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView预览.Location = new System.Drawing.Point(12, 336);
            this.dataGridView预览.Name = "dataGridView预览";
            this.dataGridView预览.ReadOnly = true;
            this.dataGridView预览.RowTemplate.Height = 23;
            this.dataGridView预览.Size = new System.Drawing.Size(600, 200);
            this.dataGridView预览.TabIndex = 4;
            // 
            // UCDataCleanupTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn执行清理);
            this.Controls.Add(this.dataGridView预览);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "UCDataCleanupTool";
            this.Size = new System.Drawing.Size(699, 572);
            this.Load += new System.EventHandler(this.frmDataCleanupTool_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView预览)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmb目标数据表;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt清理条件;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn快速示例;
        private System.Windows.Forms.CheckBox chk模拟运行;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txt确认码;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn执行清理;
        private System.Windows.Forms.ComboBox cmb清理模式;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView预览;
    }
}
