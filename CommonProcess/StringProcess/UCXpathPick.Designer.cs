namespace CommonProcess.StringProcess
{
    partial class UCXpathPick
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
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txt指定属性 = new WinLib.WatermarkTextBox();
            this.rdb返回值为指定属性值 = new System.Windows.Forms.RadioButton();
            this.rdb返回值为InnerText = new System.Windows.Forms.RadioButton();
            this.rdb返回值为InnerHtml = new System.Windows.Forms.RadioButton();
            this.txtXpath = new WinLib.WatermarkTextBox();
            this.rdb返回值为OuterHtml = new System.Windows.Forms.RadioButton();
            this.lblXPath = new System.Windows.Forms.Label();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txt指定属性);
            this.groupBox7.Controls.Add(this.rdb返回值为指定属性值);
            this.groupBox7.Controls.Add(this.rdb返回值为InnerText);
            this.groupBox7.Controls.Add(this.rdb返回值为InnerHtml);
            this.groupBox7.Controls.Add(this.txtXpath);
            this.groupBox7.Controls.Add(this.rdb返回值为OuterHtml);
            this.groupBox7.Controls.Add(this.lblXPath);
            this.groupBox7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox7.Location = new System.Drawing.Point(48, 21);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(923, 94);
            this.groupBox7.TabIndex = 103;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "精确Xpath指定内容";
            // 
            // txt指定属性
            // 
            this.txt指定属性.EmptyTextTip = "如：src img href";
            this.txt指定属性.Location = new System.Drawing.Point(681, 59);
            this.txt指定属性.Name = "txt指定属性";
            this.txt指定属性.Size = new System.Drawing.Size(100, 21);
            this.txt指定属性.TabIndex = 105;
            // 
            // rdb返回值为指定属性值
            // 
            this.rdb返回值为指定属性值.AutoSize = true;
            this.rdb返回值为指定属性值.Location = new System.Drawing.Point(543, 61);
            this.rdb返回值为指定属性值.Name = "rdb返回值为指定属性值";
            this.rdb返回值为指定属性值.Size = new System.Drawing.Size(131, 16);
            this.rdb返回值为指定属性值.TabIndex = 104;
            this.rdb返回值为指定属性值.Text = "返回值为指定属性值";
            this.rdb返回值为指定属性值.UseVisualStyleBackColor = true;
            // 
            // rdb返回值为InnerText
            // 
            this.rdb返回值为InnerText.AutoSize = true;
            this.rdb返回值为InnerText.Location = new System.Drawing.Point(378, 61);
            this.rdb返回值为InnerText.Name = "rdb返回值为InnerText";
            this.rdb返回值为InnerText.Size = new System.Drawing.Size(125, 16);
            this.rdb返回值为InnerText.TabIndex = 103;
            this.rdb返回值为InnerText.Text = "返回值为InnerText";
            this.rdb返回值为InnerText.UseVisualStyleBackColor = true;
            // 
            // rdb返回值为InnerHtml
            // 
            this.rdb返回值为InnerHtml.AutoSize = true;
            this.rdb返回值为InnerHtml.Location = new System.Drawing.Point(219, 61);
            this.rdb返回值为InnerHtml.Name = "rdb返回值为InnerHtml";
            this.rdb返回值为InnerHtml.Size = new System.Drawing.Size(125, 16);
            this.rdb返回值为InnerHtml.TabIndex = 102;
            this.rdb返回值为InnerHtml.Text = "返回值为InnerHtml";
            this.rdb返回值为InnerHtml.UseVisualStyleBackColor = true;
            // 
            // txtXpath
            // 
            this.txtXpath.EmptyTextTip = "chrome->检查html->copy->copy xpath";
            this.txtXpath.Location = new System.Drawing.Point(66, 20);
            this.txtXpath.Name = "txtXpath";
            this.txtXpath.Size = new System.Drawing.Size(825, 21);
            this.txtXpath.TabIndex = 100;
            // 
            // rdb返回值为OuterHtml
            // 
            this.rdb返回值为OuterHtml.AutoSize = true;
            this.rdb返回值为OuterHtml.Checked = true;
            this.rdb返回值为OuterHtml.Location = new System.Drawing.Point(66, 61);
            this.rdb返回值为OuterHtml.Name = "rdb返回值为OuterHtml";
            this.rdb返回值为OuterHtml.Size = new System.Drawing.Size(125, 16);
            this.rdb返回值为OuterHtml.TabIndex = 101;
            this.rdb返回值为OuterHtml.TabStop = true;
            this.rdb返回值为OuterHtml.Text = "返回值为OuterHtml";
            this.rdb返回值为OuterHtml.UseVisualStyleBackColor = true;
            // 
            // lblXPath
            // 
            this.lblXPath.AutoSize = true;
            this.lblXPath.Location = new System.Drawing.Point(19, 27);
            this.lblXPath.Name = "lblXPath";
            this.lblXPath.Size = new System.Drawing.Size(35, 12);
            this.lblXPath.TabIndex = 99;
            this.lblXPath.Text = "xPath";
            // 
            // UCXpathPick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox7);
            this.Name = "UCXpathPick";
            this.Size = new System.Drawing.Size(1010, 140);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox7;
        private WinLib.WatermarkTextBox txt指定属性;
        private System.Windows.Forms.RadioButton rdb返回值为指定属性值;
        private System.Windows.Forms.RadioButton rdb返回值为InnerText;
        private System.Windows.Forms.RadioButton rdb返回值为InnerHtml;
        private WinLib.WatermarkTextBox txtXpath;
        private System.Windows.Forms.RadioButton rdb返回值为OuterHtml;
        private System.Windows.Forms.Label lblXPath;
    }
}
