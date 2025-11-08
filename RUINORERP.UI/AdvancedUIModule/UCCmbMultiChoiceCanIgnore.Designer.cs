namespace RUINORERP.UI.AdvancedUIModule
{
    partial class UCCmbMultiChoiceCanIgnore
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
            RUINOR.WinFormsUI.ChkComboBox.CheckBoxProperties checkBoxProperties1 = new RUINOR.WinFormsUI.ChkComboBox.CheckBoxProperties();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCCmbMultiChoiceCanIgnore));
            this.chkMulti = new RUINOR.WinFormsUI.ChkComboBox.CheckBoxComboBox();
            this.chkCanIgnore = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonSplitContainer右边详情 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonPanelQuery = new Krypton.Toolkit.KryptonPanel();
            this.groupLine1 = new WinLib.Line.GroupLine();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer右边详情)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer右边详情.Panel1)).BeginInit();
            this.kryptonSplitContainer右边详情.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer右边详情.Panel2)).BeginInit();
            this.kryptonSplitContainer右边详情.Panel2.SuspendLayout();
            this.kryptonSplitContainer右边详情.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).BeginInit();
            this.kryptonPanelQuery.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkMulti
            // 
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkMulti.CheckBoxProperties = checkBoxProperties1;
            this.chkMulti.DisplayMemberSingleItem = "";
            this.chkMulti.FormattingEnabled = true;
            this.chkMulti.Location = new System.Drawing.Point(0, 0);
            this.chkMulti.Margin = new System.Windows.Forms.Padding(0);
            this.chkMulti.MultiChoiceResults = ((System.Collections.Generic.List<object>)(resources.GetObject("chkMulti.MultiChoiceResults")));
            this.chkMulti.Name = "chkMulti";
            this.chkMulti.Size = new System.Drawing.Size(163, 20);
            this.chkMulti.TabIndex = 0;
            // 
            // chkCanIgnore
            // 
            this.chkCanIgnore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkCanIgnore.Location = new System.Drawing.Point(0, 0);
            this.chkCanIgnore.Margin = new System.Windows.Forms.Padding(0);
            this.chkCanIgnore.Name = "chkCanIgnore";
            this.chkCanIgnore.Size = new System.Drawing.Size(16, 22);
            this.chkCanIgnore.TabIndex = 1;
            this.chkCanIgnore.Values.Text = "";
            // 
            // kryptonSplitContainer右边详情
            // 
            this.kryptonSplitContainer右边详情.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer右边详情.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer右边详情.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer右边详情.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonSplitContainer右边详情.Name = "kryptonSplitContainer右边详情";
            // 
            // kryptonSplitContainer右边详情.Panel1
            // 
            this.kryptonSplitContainer右边详情.Panel1.Controls.Add(this.chkCanIgnore);
            this.kryptonSplitContainer右边详情.Panel1MinSize = 10;
            // 
            // kryptonSplitContainer右边详情.Panel2
            // 
            this.kryptonSplitContainer右边详情.Panel2.Controls.Add(this.kryptonPanelQuery);
            this.kryptonSplitContainer右边详情.Panel2MinSize = 10;
            this.kryptonSplitContainer右边详情.Size = new System.Drawing.Size(190, 22);
            this.kryptonSplitContainer右边详情.SplitterDistance = 16;
            this.kryptonSplitContainer右边详情.SplitterWidth = 1;
            this.kryptonSplitContainer右边详情.TabIndex = 3;
            // 
            // kryptonPanelQuery
            // 
            this.kryptonPanelQuery.AutoSize = true;
            this.kryptonPanelQuery.Controls.Add(this.groupLine1);
            this.kryptonPanelQuery.Controls.Add(this.chkMulti);
            this.kryptonPanelQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelQuery.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelQuery.Name = "kryptonPanelQuery";
            this.kryptonPanelQuery.Size = new System.Drawing.Size(173, 22);
            this.kryptonPanelQuery.TabIndex = 2;
            // 
            // groupLine1
            // 
            this.groupLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupLine1.Location = new System.Drawing.Point(0, 21);
            this.groupLine1.Name = "groupLine1";
            this.groupLine1.Size = new System.Drawing.Size(173, 1);
            this.groupLine1.TabIndex = 2;
            // 
            // UCCmbMultiChoiceCanIgnore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Controls.Add(this.kryptonSplitContainer右边详情);
            this.Name = "UCCmbMultiChoiceCanIgnore";
            this.Size = new System.Drawing.Size(190, 22);
            this.Load += new System.EventHandler(this.UCCmbMultiChoiceCanIgnore_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer右边详情.Panel1)).EndInit();
            this.kryptonSplitContainer右边详情.Panel1.ResumeLayout(false);
            this.kryptonSplitContainer右边详情.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer右边详情.Panel2)).EndInit();
            this.kryptonSplitContainer右边详情.Panel2.ResumeLayout(false);
            this.kryptonSplitContainer右边详情.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer右边详情)).EndInit();
            this.kryptonSplitContainer右边详情.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelQuery)).EndInit();
            this.kryptonPanelQuery.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public Krypton.Toolkit.KryptonCheckBox chkCanIgnore;
        public RUINOR.WinFormsUI.ChkComboBox.CheckBoxComboBox chkMulti;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer右边详情;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelQuery;
        private WinLib.Line.GroupLine groupLine1;
    }
}
