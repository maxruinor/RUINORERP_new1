namespace RUINORERP.UI.Testing
{
    partial class FrmServiceCacheTest
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCacheStatistics = new System.Windows.Forms.Label();
            this.btnTestService = new System.Windows.Forms.Button();
            this.txtIterations = new System.Windows.Forms.TextBox();
            this.txtServiceType = new System.Windows.Forms.TextBox();
            this.lblIterations = new System.Windows.Forms.Label();
            this.lblServiceType = new System.Windows.Forms.Label();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTestAppContext = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblAppContextStats = new System.Windows.Forms.Label();
            this.btnRunConsoleTest = new System.Windows.Forms.Button();
            this.btnTestCurrentContext = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCacheStatistics
            // 
            this.lblCacheStatistics.AutoSize = true;
            this.lblCacheStatistics.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCacheStatistics.Location = new System.Drawing.Point(12, 9);
            this.lblCacheStatistics.Name = "lblCacheStatistics";
            this.lblCacheStatistics.Size = new System.Drawing.Size(0, 15);
            this.lblCacheStatistics.TabIndex = 0;
            // 
            // btnTestService
            // 
            this.btnTestService.Location = new System.Drawing.Point(15, 95);
            this.btnTestService.Name = "btnTestService";
            this.btnTestService.Size = new System.Drawing.Size(75, 23);
            this.btnTestService.TabIndex = 1;
            this.btnTestService.Text = "测试服务";
            this.btnTestService.UseVisualStyleBackColor = true;
            this.btnTestService.Click += new System.EventHandler(this.btnTestService_Click);
            // 
            // txtIterations
            // 
            this.txtIterations.Location = new System.Drawing.Point(96, 30);
            this.txtIterations.Name = "txtIterations";
            this.txtIterations.Size = new System.Drawing.Size(100, 21);
            this.txtIterations.TabIndex = 2;
            this.txtIterations.Text = "1000";
            // 
            // txtServiceType
            // 
            this.txtServiceType.Location = new System.Drawing.Point(96, 57);
            this.txtServiceType.Name = "txtServiceType";
            this.txtServiceType.Size = new System.Drawing.Size(300, 21);
            this.txtServiceType.TabIndex = 3;
            this.txtServiceType.Text = "RUINORERP.IServices.IEntityCacheManager";
            // 
            // lblIterations
            // 
            this.lblIterations.AutoSize = true;
            this.lblIterations.Location = new System.Drawing.Point(15, 33);
            this.lblIterations.Name = "lblIterations";
            this.lblIterations.Size = new System.Drawing.Size(59, 12);
            this.lblIterations.TabIndex = 4;
            this.lblIterations.Text = "迭代次数:";
            // 
            // lblServiceType
            // 
            this.lblServiceType.AutoSize = true;
            this.lblServiceType.Location = new System.Drawing.Point(15, 60);
            this.lblServiceType.Name = "lblServiceType";
            this.lblServiceType.Size = new System.Drawing.Size(59, 12);
            this.lblServiceType.TabIndex = 5;
            this.lblServiceType.Text = "服务类型:";
            // 
            // lstResults
            // 
            this.lstResults.FormattingEnabled = true;
            this.lstResults.ItemHeight = 12;
            this.lstResults.Location = new System.Drawing.Point(15, 19);
            this.lstResults.Name = "lstResults";
            this.lstResults.ScrollAlwaysVisible = true;
            this.lstResults.Size = new System.Drawing.Size(381, 268);
            this.lstResults.TabIndex = 6;
            // 
            // btnClearCache
            // 
            this.btnClearCache.Location = new System.Drawing.Point(177, 95);
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.Size = new System.Drawing.Size(75, 23);
            this.btnClearCache.TabIndex = 7;
            this.btnClearCache.Text = "清空缓存";
            this.btnClearCache.UseVisualStyleBackColor = true;
            this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(179, 20);
            this.lblTitle.TabIndex = 8;
            this.lblTitle.Text = "服务实例缓存测试工具";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTestCurrentContext);
            this.groupBox1.Controls.Add(this.btnTestAppContext);
            this.groupBox1.Controls.Add(this.lblIterations);
            this.groupBox1.Controls.Add(this.txtIterations);
            this.groupBox1.Controls.Add(this.lblServiceType);
            this.groupBox1.Controls.Add(this.txtServiceType);
            this.groupBox1.Controls.Add(this.btnTestService);
            this.groupBox1.Controls.Add(this.btnClearCache);
            this.groupBox1.Location = new System.Drawing.Point(12, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(413, 153);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "测试参数";
            // 
            // btnTestAppContext
            // 
            this.btnTestAppContext.Location = new System.Drawing.Point(301, 95);
            this.btnTestAppContext.Name = "btnTestAppContext";
            this.btnTestAppContext.Size = new System.Drawing.Size(75, 23);
            this.btnTestAppContext.TabIndex = 12;
            this.btnTestAppContext.Text = "测试AppCtx";
            this.btnTestAppContext.UseVisualStyleBackColor = true;
            this.btnTestAppContext.Click += new System.EventHandler(this.btnTestAppContext_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstResults);
            this.groupBox2.Location = new System.Drawing.Point(12, 165);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(413, 293);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "测试结果";
            // 
            // lblAppContextStats
            // 
            this.lblAppContextStats.AutoSize = true;
            this.lblAppContextStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAppContextStats.Location = new System.Drawing.Point(12, 25);
            this.lblAppContextStats.Name = "lblAppContextStats";
            this.lblAppContextStats.Size = new System.Drawing.Size(0, 15);
            this.lblAppContextStats.TabIndex = 11;
            // 
            // btnRunConsoleTest
            // 
            this.btnRunConsoleTest.Location = new System.Drawing.Point(267, 95);
            this.btnRunConsoleTest.Name = "btnRunConsoleTest";
            this.btnRunConsoleTest.Size = new System.Drawing.Size(120, 23);
            this.btnRunConsoleTest.TabIndex = 13;
            this.btnRunConsoleTest.Text = "运行控制台测试";
            this.btnRunConsoleTest.UseVisualStyleBackColor = true;
            this.btnRunConsoleTest.Click += new System.EventHandler(this.btnRunConsoleTest_Click);
            // 
            // btnTestCurrentContext
            // 
            this.btnTestCurrentContext.Location = new System.Drawing.Point(15, 124);
            this.btnTestCurrentContext.Name = "btnTestCurrentContext";
            this.btnTestCurrentContext.Size = new System.Drawing.Size(120, 23);
            this.btnTestCurrentContext.TabIndex = 14;
            this.btnTestCurrentContext.Text = "测试Current属性";
            this.btnTestCurrentContext.UseVisualStyleBackColor = true;
            this.btnTestCurrentContext.Click += new System.EventHandler(this.btnTestCurrentContext_Click);
            // 
            // FrmServiceCacheTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 470);
            this.Controls.Add(this.lblAppContextStats);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblCacheStatistics);
            this.Name = "FrmServiceCacheTest";
            this.Text = "服务缓存测试";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmServiceCacheTest_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCacheStatistics;
        private System.Windows.Forms.Button btnTestService;
        private System.Windows.Forms.TextBox txtIterations;
        private System.Windows.Forms.TextBox txtServiceType;
        private System.Windows.Forms.Label lblIterations;
        private System.Windows.Forms.Label lblServiceType;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblAppContextStats;
        private System.Windows.Forms.Button btnRunConsoleTest;
        private System.Windows.Forms.Button btnTestCurrentContext;
        private System.Windows.Forms.Button btnTestAppContext;
    }
}