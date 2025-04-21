namespace RUINORERP.UI.UserPersonalized
{
    partial class UCInputDataCol
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
            this.lblCaption = new Krypton.Toolkit.KryptonLabel();
            this.txtCaption = new Krypton.Toolkit.KryptonTextBox();
            this.lblValueType = new Krypton.Toolkit.KryptonLabel();
            this.txtValueType = new Krypton.Toolkit.KryptonTextBox();
            this.lblDefault1 = new Krypton.Toolkit.KryptonLabel();
            this.txtDefault1 = new Krypton.Toolkit.KryptonTextBox();
            this.lblFocused = new Krypton.Toolkit.KryptonLabel();
            this.chkFocused = new Krypton.Toolkit.KryptonCheckBox();
            this.chkEnableDefault1 = new Krypton.Toolkit.KryptonCheckBox();
            this.lblVisble = new Krypton.Toolkit.KryptonLabel();
            this.chkVisble = new Krypton.Toolkit.KryptonCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.Location = new System.Drawing.Point(22, 12);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(75, 20);
            this.lblCaption.TabIndex = 15;
            this.lblCaption.Values.Text = "查询条件名";
            // 
            // txtCaption
            // 
            this.txtCaption.Location = new System.Drawing.Point(113, 12);
            this.txtCaption.Name = "txtCaption";
            this.txtCaption.Size = new System.Drawing.Size(172, 23);
            this.txtCaption.TabIndex = 16;
            // 
            // lblValueType
            // 
            this.lblValueType.Location = new System.Drawing.Point(48, 41);
            this.lblValueType.Name = "lblValueType";
            this.lblValueType.Size = new System.Drawing.Size(49, 20);
            this.lblValueType.TabIndex = 19;
            this.lblValueType.Values.Text = "值类型";
            // 
            // txtValueType
            // 
            this.txtValueType.Location = new System.Drawing.Point(113, 41);
            this.txtValueType.Name = "txtValueType";
            this.txtValueType.Size = new System.Drawing.Size(172, 23);
            this.txtValueType.TabIndex = 20;
            // 
            // lblDefault1
            // 
            this.lblDefault1.Location = new System.Drawing.Point(41, 95);
            this.lblDefault1.Name = "lblDefault1";
            this.lblDefault1.Size = new System.Drawing.Size(56, 20);
            this.lblDefault1.TabIndex = 25;
            this.lblDefault1.Values.Text = "默认值1";
            // 
            // txtDefault1
            // 
            this.txtDefault1.Location = new System.Drawing.Point(136, 95);
            this.txtDefault1.Multiline = true;
            this.txtDefault1.Name = "txtDefault1";
            this.txtDefault1.Size = new System.Drawing.Size(172, 21);
            this.txtDefault1.TabIndex = 26;
            // 
            // lblFocused
            // 
            this.lblFocused.Location = new System.Drawing.Point(35, 135);
            this.lblFocused.Name = "lblFocused";
            this.lblFocused.Size = new System.Drawing.Size(62, 20);
            this.lblFocused.TabIndex = 35;
            this.lblFocused.Values.Text = "默认焦点";
            // 
            // chkFocused
            // 
            this.chkFocused.Location = new System.Drawing.Point(113, 140);
            this.chkFocused.Name = "chkFocused";
            this.chkFocused.Size = new System.Drawing.Size(19, 13);
            this.chkFocused.TabIndex = 36;
            this.chkFocused.ToolTipValues.Description = "打开查询页面，第一个获取焦点的输入框。";
            this.chkFocused.ToolTipValues.Heading = "";
            this.chkFocused.Values.Text = "";
            // 
            // chkEnableDefault1
            // 
            this.chkEnableDefault1.Location = new System.Drawing.Point(113, 99);
            this.chkEnableDefault1.Name = "chkEnableDefault1";
            this.chkEnableDefault1.Size = new System.Drawing.Size(19, 13);
            this.chkEnableDefault1.TabIndex = 37;
            this.chkEnableDefault1.Values.Text = "";
            // 
            // lblVisble
            // 
            this.lblVisble.Location = new System.Drawing.Point(35, 67);
            this.lblVisble.Name = "lblVisble";
            this.lblVisble.Size = new System.Drawing.Size(62, 20);
            this.lblVisble.TabIndex = 39;
            this.lblVisble.Values.Text = "是否可见";
            // 
            // chkVisble
            // 
            this.chkVisble.Location = new System.Drawing.Point(113, 70);
            this.chkVisble.Name = "chkVisble";
            this.chkVisble.Size = new System.Drawing.Size(19, 13);
            this.chkVisble.TabIndex = 40;
            this.chkVisble.Values.Text = "";
            // 
            // UCInputDataCol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 218);
            this.ControlBox = false;
            this.Controls.Add(this.lblVisble);
            this.Controls.Add(this.chkVisble);
            this.Controls.Add(this.chkEnableDefault1);
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.txtCaption);
            this.Controls.Add(this.lblValueType);
            this.Controls.Add(this.txtValueType);
            this.Controls.Add(this.lblDefault1);
            this.Controls.Add(this.txtDefault1);
            this.Controls.Add(this.lblFocused);
            this.Controls.Add(this.chkFocused);
            this.Name = "UCInputDataCol";
            this.Leave += new System.EventHandler(this.UCQueryCondition_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Krypton.Toolkit.KryptonLabel lblCaption;
        private Krypton.Toolkit.KryptonTextBox txtCaption;
        private Krypton.Toolkit.KryptonLabel lblValueType;
        private Krypton.Toolkit.KryptonTextBox txtValueType;
        private Krypton.Toolkit.KryptonLabel lblDefault1;
        private Krypton.Toolkit.KryptonTextBox txtDefault1;
        private Krypton.Toolkit.KryptonLabel lblFocused;
        private Krypton.Toolkit.KryptonCheckBox chkFocused;
        private Krypton.Toolkit.KryptonCheckBox chkEnableDefault1;
        private Krypton.Toolkit.KryptonLabel lblVisble;
        private Krypton.Toolkit.KryptonCheckBox chkVisble;
    }
}
