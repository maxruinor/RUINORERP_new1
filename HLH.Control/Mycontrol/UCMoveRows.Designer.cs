namespace SHControls.Mycontrol
{
    partial class UCMoveRows
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
            this.btnFirstRow = new System.Windows.Forms.Button();
            this.btnLastRow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFirstRow
            // 
            this.btnFirstRow.Location = new System.Drawing.Point(3, 1);
            this.btnFirstRow.Name = "btnFirstRow";
            this.btnFirstRow.Size = new System.Drawing.Size(55, 23);
            this.btnFirstRow.TabIndex = 0;
            this.btnFirstRow.Text = "首行(&I)";
            this.btnFirstRow.UseVisualStyleBackColor = true;
            this.btnFirstRow.Click += new System.EventHandler(this.btnFirstRow_Click);
            // 
            // btnLastRow
            // 
            this.btnLastRow.Location = new System.Drawing.Point(60, 1);
            this.btnLastRow.Name = "btnLastRow";
            this.btnLastRow.Size = new System.Drawing.Size(55, 23);
            this.btnLastRow.TabIndex = 1;
            this.btnLastRow.Text = "尾行(&L)";
            this.btnLastRow.UseVisualStyleBackColor = true;
            this.btnLastRow.Click += new System.EventHandler(this.btnLastRow_Click);
            // 
            // UCMoveRows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLastRow);
            this.Controls.Add(this.btnFirstRow);
            this.Name = "UCMoveRows";
            this.Size = new System.Drawing.Size(119, 25);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFirstRow;
        private System.Windows.Forms.Button btnLastRow;
    }
}
