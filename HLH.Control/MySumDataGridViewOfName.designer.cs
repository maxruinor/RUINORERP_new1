namespace HLH.WinControl
{
    partial class MySumDataGridViewOfName
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.watermaskRichTextBox1 = new WinLib.WatermaskRichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // watermaskRichTextBox1
            // 
            this.watermaskRichTextBox1.EmptyTextTip = null;
            this.watermaskRichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.watermaskRichTextBox1.Name = "watermaskRichTextBox1";
            this.watermaskRichTextBox1.Size = new System.Drawing.Size(100, 96);
            this.watermaskRichTextBox1.TabIndex = 0;
            this.watermaskRichTextBox1.Text = "";
            // 
            // MySumDataGridViewOfName
            // 
            this.RowTemplate.Height = 23;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WinLib.WatermaskRichTextBox watermaskRichTextBox1;
    }
}
