namespace WindowsApplication23
{
    partial class FormFormat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxDate = new System.Windows.Forms.GroupBox();
            this.panelControl = new System.Windows.Forms.Panel();
            this.textBoxNull = new System.Windows.Forms.TextBox();
            this.labelNull = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelExample = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxFormatType = new System.Windows.Forms.ListBox();
            this.labelExplain = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.labelDateType = new System.Windows.Forms.Label();
            this.listBoxDate = new System.Windows.Forms.ListBox();
            this.groupBoxDate.SuspendLayout();
            this.panelControl.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDate
            // 
            this.groupBoxDate.Controls.Add(this.panelControl);
            this.groupBoxDate.Controls.Add(this.groupBox2);
            this.groupBoxDate.Controls.Add(this.label1);
            this.groupBoxDate.Controls.Add(this.listBoxFormatType);
            this.groupBoxDate.Controls.Add(this.labelExplain);
            this.groupBoxDate.Location = new System.Drawing.Point(12, 12);
            this.groupBoxDate.Name = "groupBoxDate";
            this.groupBoxDate.Size = new System.Drawing.Size(371, 240);
            this.groupBoxDate.TabIndex = 0;
            this.groupBoxDate.TabStop = false;
            this.groupBoxDate.Text = "格式";
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.listBoxDate);
            this.panelControl.Controls.Add(this.labelDateType);
            this.panelControl.Controls.Add(this.textBoxNull);
            this.panelControl.Controls.Add(this.labelNull);
            this.panelControl.Location = new System.Drawing.Point(123, 105);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(243, 111);
            this.panelControl.TabIndex = 6;
            // 
            // textBoxNull
            // 
            this.textBoxNull.Location = new System.Drawing.Point(80, 3);
            this.textBoxNull.Name = "textBoxNull";
            this.textBoxNull.Size = new System.Drawing.Size(159, 21);
            this.textBoxNull.TabIndex = 5;
            // 
            // labelNull
            // 
            this.labelNull.AutoSize = true;
            this.labelNull.Location = new System.Drawing.Point(3, 6);
            this.labelNull.Name = "labelNull";
            this.labelNull.Size = new System.Drawing.Size(71, 12);
            this.labelNull.TabIndex = 4;
            this.labelNull.Text = "Null 值(&N):";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelExample);
            this.groupBox2.Location = new System.Drawing.Point(126, 55);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(236, 44);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "事例";
            // 
            // labelExample
            // 
            this.labelExample.AutoSize = true;
            this.labelExample.Location = new System.Drawing.Point(6, 17);
            this.labelExample.Name = "labelExample";
            this.labelExample.Size = new System.Drawing.Size(29, 12);
            this.labelExample.TabIndex = 1;
            this.labelExample.Text = "事例";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "格式类型(&E):";
            // 
            // listBoxFormatType
            // 
            this.listBoxFormatType.FormattingEnabled = true;
            this.listBoxFormatType.ItemHeight = 12;
            this.listBoxFormatType.Items.AddRange(new object[] {
            "无格式设置",
            "数字",
            "货币",
            "日期时间",
            "科学型"});
            this.listBoxFormatType.Location = new System.Drawing.Point(6, 80);
            this.listBoxFormatType.Name = "listBoxFormatType";
            this.listBoxFormatType.Size = new System.Drawing.Size(114, 136);
            this.listBoxFormatType.TabIndex = 1;
            this.listBoxFormatType.SelectedIndexChanged += new System.EventHandler(this.listBoxFormatType_SelectedIndexChanged);
            // 
            // labelExplain
            // 
            this.labelExplain.AutoSize = true;
            this.labelExplain.Location = new System.Drawing.Point(6, 17);
            this.labelExplain.Name = "labelExplain";
            this.labelExplain.Size = new System.Drawing.Size(29, 12);
            this.labelExplain.TabIndex = 0;
            this.labelExplain.Text = "说明";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(185, 261);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancle.Location = new System.Drawing.Point(266, 261);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 2;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            // 
            // labelDateType
            // 
            this.labelDateType.AutoSize = true;
            this.labelDateType.Location = new System.Drawing.Point(3, 30);
            this.labelDateType.Name = "labelDateType";
            this.labelDateType.Size = new System.Drawing.Size(47, 12);
            this.labelDateType.TabIndex = 6;
            this.labelDateType.Text = "类型(&T)";
            this.labelDateType.Visible = false;
            // 
            // listBoxDate
            // 
            this.listBoxDate.FormattingEnabled = true;
            this.listBoxDate.ItemHeight = 12;
            this.listBoxDate.Location = new System.Drawing.Point(5, 45);
            this.listBoxDate.Name = "listBoxDate";
            this.listBoxDate.Size = new System.Drawing.Size(234, 64);
            this.listBoxDate.TabIndex = 7;
            this.listBoxDate.Visible = false;
            this.listBoxDate.SelectedIndexChanged += new System.EventHandler(this.listBoxDate_SelectedIndexChanged);
            // 
            // FormFormat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 291);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBoxDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFormat";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "格式字符串对话框";
            this.groupBoxDate.ResumeLayout(false);
            this.groupBoxDate.PerformLayout();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDate;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxFormatType;
        private System.Windows.Forms.Label labelExplain;
        private System.Windows.Forms.TextBox textBoxNull;
        private System.Windows.Forms.Label labelNull;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Label labelExample;
        private System.Windows.Forms.ListBox listBoxDate;
        private System.Windows.Forms.Label labelDateType;
    }
}