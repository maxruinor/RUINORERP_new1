namespace RUINORERP.UI.CRM
{
    partial class UCCRMChart
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
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.tabControlEx1 = new WinLib.TabControlEx();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cartesianChart1 = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.tabControlEx1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.tabControlEx1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(709, 477);
            this.kryptonPanel1.TabIndex = 3;
            // 
            // tabControlEx1
            // 
            this.tabControlEx1.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(79)))), ((int)(((byte)(125)))));
            this.tabControlEx1.Controls.Add(this.tabPage1);
            this.tabControlEx1.Controls.Add(this.tabPage2);
            this.tabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEx1.Location = new System.Drawing.Point(0, 0);
            this.tabControlEx1.Name = "tabControlEx1";
            this.tabControlEx1.SelectedIndex = 0;
            this.tabControlEx1.Size = new System.Drawing.Size(709, 477);
            this.tabControlEx1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cartesianChart1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(701, 447);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(449, 296);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(55, 37);
            this.cartesianChart1.MatchAxesScreenDataRatio = false;
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(432, 332);
            this.cartesianChart1.TabIndex = 0;
            // 
            // UCCRMChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCCRMChart";
            this.Size = new System.Drawing.Size(709, 477);
            this.Load += new System.EventHandler(this.UCCRMChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.tabControlEx1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private WinLib.TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart cartesianChart1;
    }
}
