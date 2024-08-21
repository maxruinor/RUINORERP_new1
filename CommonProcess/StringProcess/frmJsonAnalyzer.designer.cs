namespace CommonProcess.StringProcess
{
    partial class frmJsonAnalyzer
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
            this.btmCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chk值包含 = new System.Windows.Forms.CheckBox();
            this.richTextBoxinput = new System.Windows.Forms.RichTextBox();
            this.btnGetValue = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearchKeyValue = new System.Windows.Forms.Button();
            this.btn = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.rtxtLastValue = new System.Windows.Forms.RichTextBox();
            this.rtxt提取Para = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmCancel
            // 
            this.btmCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btmCancel.Location = new System.Drawing.Point(640, 12);
            this.btmCancel.Name = "btmCancel";
            this.btmCancel.Size = new System.Drawing.Size(65, 39);
            this.btmCancel.TabIndex = 3;
            this.btmCancel.Text = "取消";
            this.btmCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(396, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 39);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chk值包含);
            this.splitContainer1.Panel1.Controls.Add(this.richTextBoxinput);
            this.splitContainer1.Panel1.Controls.Add(this.btnGetValue);
            this.splitContainer1.Panel1.Controls.Add(this.txtValue);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.txtKey);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.btnSearchKeyValue);
            this.splitContainer1.Panel1.Controls.Add(this.btn);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1243, 964);
            this.splitContainer1.SplitterDistance = 578;
            this.splitContainer1.TabIndex = 5;
            // 
            // chk值包含
            // 
            this.chk值包含.AutoSize = true;
            this.chk值包含.Location = new System.Drawing.Point(668, 277);
            this.chk值包含.Name = "chk值包含";
            this.chk值包含.Size = new System.Drawing.Size(96, 16);
            this.chk值包含.TabIndex = 11;
            this.chk值包含.Text = "值为包含相近";
            this.chk值包含.UseVisualStyleBackColor = true;
            // 
            // richTextBoxinput
            // 
            this.richTextBoxinput.Dock = System.Windows.Forms.DockStyle.Left;
            this.richTextBoxinput.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxinput.Name = "richTextBoxinput";
            this.richTextBoxinput.Size = new System.Drawing.Size(621, 578);
            this.richTextBoxinput.TabIndex = 10;
            this.richTextBoxinput.Text = "";
            // 
            // btnGetValue
            // 
            this.btnGetValue.Location = new System.Drawing.Point(640, 454);
            this.btnGetValue.Name = "btnGetValue";
            this.btnGetValue.Size = new System.Drawing.Size(116, 40);
            this.btnGetValue.TabIndex = 0;
            this.btnGetValue.Text = "3）测试提取值";
            this.btnGetValue.UseVisualStyleBackColor = true;
            this.btnGetValue.Click += new System.EventHandler(this.btnGetValue_Click);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(668, 207);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(100, 21);
            this.txtValue.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(627, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Value";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(668, 180);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(100, 21);
            this.txtKey.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(627, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "Key";
            // 
            // btnSearchKeyValue
            // 
            this.btnSearchKeyValue.Location = new System.Drawing.Point(640, 237);
            this.btnSearchKeyValue.Name = "btnSearchKeyValue";
            this.btnSearchKeyValue.Size = new System.Drawing.Size(116, 34);
            this.btnSearchKeyValue.TabIndex = 7;
            this.btnSearchKeyValue.Text = "2）找查指定节点";
            this.btnSearchKeyValue.UseVisualStyleBackColor = true;
            this.btnSearchKeyValue.Click += new System.EventHandler(this.btnSearchKeyValue_Click);
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(640, 23);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(116, 33);
            this.btn.TabIndex = 5;
            this.btn.Text = "1）json格式化";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Right;
            this.treeView1.Location = new System.Drawing.Point(890, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(353, 578);
            this.treeView1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btmCancel);
            this.splitContainer2.Panel2.Controls.Add(this.btnOK);
            this.splitContainer2.Size = new System.Drawing.Size(1243, 382);
            this.splitContainer2.SplitterDistance = 319;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.rtxtLastValue);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.rtxt提取Para);
            this.splitContainer3.Size = new System.Drawing.Size(1243, 319);
            this.splitContainer3.SplitterDistance = 412;
            this.splitContainer3.TabIndex = 1;
            // 
            // rtxtLastValue
            // 
            this.rtxtLastValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtLastValue.Location = new System.Drawing.Point(0, 0);
            this.rtxtLastValue.Name = "rtxtLastValue";
            this.rtxtLastValue.Size = new System.Drawing.Size(412, 319);
            this.rtxtLastValue.TabIndex = 1;
            this.rtxtLastValue.Text = "";
            // 
            // rtxt提取Para
            // 
            this.rtxt提取Para.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxt提取Para.Location = new System.Drawing.Point(0, 0);
            this.rtxt提取Para.Name = "rtxt提取Para";
            this.rtxt提取Para.Size = new System.Drawing.Size(827, 319);
            this.rtxt提取Para.TabIndex = 0;
            this.rtxt提取Para.Text = "";
            // 
            // frmJsonAnalyzer
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btmCancel;
            this.ClientSize = new System.Drawing.Size(1243, 964);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmJsonAnalyzer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmJsonAnalyzer";
            this.Load += new System.EventHandler(this.frmJsonAnalyzer_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btmCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Button btnGetValue;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.RichTextBox rtxtLastValue;
        private System.Windows.Forms.RichTextBox rtxt提取Para;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearchKeyValue;
        public System.Windows.Forms.RichTextBox richTextBoxinput;
        private System.Windows.Forms.CheckBox chk值包含;
        public System.Windows.Forms.TextBox txtKey;
    }
}