namespace RUINORERP.UI.BaseForm
{
    partial class UCBillOutlookGridAnalysis
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
            this.components = new System.ComponentModel.Container();
            Krypton.Toolkit.Suite.Extended.Outlook.Grid.OutlookGridGroupCollection outlookGridGroupCollection1 = new Krypton.Toolkit.Suite.Extended.Outlook.Grid.OutlookGridGroupCollection();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCBillOutlookGridAnalysis));
            this.panel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonOutlookGridGroupBox1 = new Krypton.Toolkit.Suite.Extended.Outlook.Grid.KryptonOutlookGridGroupBox();
            this.kryptonOutlookGrid1 = new Krypton.Toolkit.Suite.Extended.Outlook.Grid.KryptonOutlookGrid();
            this.bindingSourceOutlook = new System.Windows.Forms.BindingSource(this.components);
            this.kryptonOutlookGridLanguageManager1 = new Krypton.Toolkit.Suite.Extended.Outlook.Grid.KryptonOutlookGridLanguageManager();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonOutlookGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceOutlook)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.kryptonSplitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(1047, 601);
            this.panel1.TabIndex = 3;
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(5, 5);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            this.kryptonSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer1.Panel1
            // 
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonOutlookGridGroupBox1);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.kryptonOutlookGrid1);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(1037, 591);
            this.kryptonSplitContainer1.SplitterDistance = 42;
            this.kryptonSplitContainer1.TabIndex = 2;
            // 
            // kryptonOutlookGridGroupBox1
            // 
            this.kryptonOutlookGridGroupBox1.AllowDrop = true;
            this.kryptonOutlookGridGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonOutlookGridGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonOutlookGridGroupBox1.Name = "kryptonOutlookGridGroupBox1";
            this.kryptonOutlookGridGroupBox1.Size = new System.Drawing.Size(1037, 42);
            this.kryptonOutlookGridGroupBox1.TabIndex = 0;
            // 
            // kryptonOutlookGrid1
            // 
            this.kryptonOutlookGrid1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.kryptonOutlookGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonOutlookGrid1.FillMode = Krypton.Toolkit.Suite.Extended.Outlook.Grid.FillMode.GroupsAndNodes;
            this.kryptonOutlookGrid1.GroupCollection = outlookGridGroupCollection1;
            this.kryptonOutlookGrid1.Location = new System.Drawing.Point(0, 0);
            this.kryptonOutlookGrid1.Name = "kryptonOutlookGrid1";
            this.kryptonOutlookGrid1.PreviousSelectedGroupRow = -1;
            this.kryptonOutlookGrid1.ShowLines = false;
            this.kryptonOutlookGrid1.Size = new System.Drawing.Size(1037, 544);
            this.kryptonOutlookGrid1.SubtotalColumns = ((System.Collections.Generic.List<string>)(resources.GetObject("kryptonOutlookGrid1.SubtotalColumns")));
            this.kryptonOutlookGrid1.TabIndex = 1;
            this.kryptonOutlookGrid1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonOutlookGrid1_CellDoubleClick);
            this.kryptonOutlookGrid1.Resize += new System.EventHandler(this.OutlookGrid1_Resize);
            // 
            // UCBillOutlookGridAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "UCBillOutlookGridAnalysis";
            this.Size = new System.Drawing.Size(1047, 601);
            this.Load += new System.EventHandler(this.UCBillOutlookGridAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonOutlookGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceOutlook)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel panel1;
        internal System.Windows.Forms.BindingSource bindingSourceOutlook;
        internal Krypton.Toolkit.Suite.Extended.Outlook.Grid.KryptonOutlookGrid kryptonOutlookGrid1;
        private Krypton.Toolkit.Suite.Extended.Outlook.Grid.KryptonOutlookGridGroupBox kryptonOutlookGridGroupBox1;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private Krypton.Toolkit.Suite.Extended.Outlook.Grid.KryptonOutlookGridLanguageManager kryptonOutlookGridLanguageManager1;
    }
}
