namespace CommonProcess.StringProcess
{
    partial class frmDynamicCompilation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDynamicCompilation));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.watermaskRichTextBox1 = new WinLib.WatermaskRichTextBox();
            this.txtEditorControl动态代码 = new ICSharpCode.TextEditor.TextEditorControl();
            this.richTextBox临时不用的 = new System.Windows.Forms.RichTextBox();
            this.richTextBoxinput = new System.Windows.Forms.RichTextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btmCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxinput);
            this.splitContainer1.Panel2.Controls.Add(this.btnTest);
            this.splitContainer1.Panel2.Controls.Add(this.btmCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Panel2.Controls.Add(this.listBox1);
            this.splitContainer1.Size = new System.Drawing.Size(920, 701);
            this.splitContainer1.SplitterDistance = 532;
            this.splitContainer1.TabIndex = 0;
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
            this.splitContainer2.Panel1.Controls.Add(this.watermaskRichTextBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtEditorControl动态代码);
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox临时不用的);
            this.splitContainer2.Size = new System.Drawing.Size(920, 532);
            this.splitContainer2.SplitterDistance = 63;
            this.splitContainer2.TabIndex = 0;
            // 
            // watermaskRichTextBox1
            // 
            this.watermaskRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.watermaskRichTextBox1.EmptyTextTip = "System.Windows.Forms.dll,这里输入动态代码中需要用到的引用。一行一个";
            this.watermaskRichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.watermaskRichTextBox1.Name = "watermaskRichTextBox1";
            this.watermaskRichTextBox1.Size = new System.Drawing.Size(920, 63);
            this.watermaskRichTextBox1.TabIndex = 0;
            this.watermaskRichTextBox1.Text = "";
            // 
            // txtEditorControl动态代码
            // 
            this.txtEditorControl动态代码.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEditorControl动态代码.Encoding = ((System.Text.Encoding)(resources.GetObject("txtEditorControl动态代码.Encoding")));
            this.txtEditorControl动态代码.Location = new System.Drawing.Point(0, 0);
            this.txtEditorControl动态代码.Name = "txtEditorControl动态代码";
            this.txtEditorControl动态代码.ShowEOLMarkers = true;
            this.txtEditorControl动态代码.ShowSpaces = true;
            this.txtEditorControl动态代码.ShowTabs = true;
            this.txtEditorControl动态代码.ShowVRuler = true;
            this.txtEditorControl动态代码.Size = new System.Drawing.Size(920, 465);
            this.txtEditorControl动态代码.TabIndex = 3;
            // 
            // richTextBox临时不用的
            // 
            this.richTextBox临时不用的.Location = new System.Drawing.Point(171, 2);
            this.richTextBox临时不用的.Name = "richTextBox临时不用的";
            this.richTextBox临时不用的.Size = new System.Drawing.Size(290, 14);
            this.richTextBox临时不用的.TabIndex = 2;
            this.richTextBox临时不用的.Text = resources.GetString("richTextBox临时不用的.Text");
            // 
            // richTextBoxinput
            // 
            this.richTextBoxinput.Location = new System.Drawing.Point(252, 58);
            this.richTextBoxinput.Name = "richTextBoxinput";
            this.richTextBoxinput.Size = new System.Drawing.Size(533, 96);
            this.richTextBoxinput.TabIndex = 4;
            this.richTextBoxinput.Text = "";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(791, 79);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(117, 44);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "运行代码测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btmCancel
            // 
            this.btmCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btmCancel.Location = new System.Drawing.Point(171, 90);
            this.btmCancel.Name = "btmCancel";
            this.btmCancel.Size = new System.Drawing.Size(75, 23);
            this.btmCancel.TabIndex = 1;
            this.btmCancel.Text = "取消";
            this.btmCancel.UseVisualStyleBackColor = true;
            this.btmCancel.Click += new System.EventHandler(this.btmCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(39, 90);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(920, 52);
            this.listBox1.TabIndex = 0;
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // frmDynamicCompilation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 701);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmDynamicCompilation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "动态c#代码数据处理";
            this.Load += new System.EventHandler(this.frmDynamicCompilation_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btmCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.RichTextBox richTextBox临时不用的;
        private WinLib.WatermaskRichTextBox watermaskRichTextBox1;
        private System.Windows.Forms.RichTextBox richTextBoxinput;
        internal ICSharpCode.TextEditor.TextEditorControl txtEditorControl动态代码;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}