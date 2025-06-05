namespace RUINORERP.UI.DevTools
{
    partial class frmDbColumnToExlColumnConfig
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
            this.components = new System.ComponentModel.Container();
            this.label3 = new System.Windows.Forms.Label();
            this.listbox匹配结果 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.指定为这次操作的主键标识ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空匹配结果ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxDbColumns = new System.Windows.Forms.ListBox();
            this.listBoxExcelColumns = new System.Windows.Forms.ListBox();
            this.btmCancel = new System.Windows.Forms.Button();
            this.btnSaveMatchResult = new System.Windows.Forms.Button();
            this.txtMatchConfigFileName = new WinLib.WatermarkTextBox();
            this.listBoxExitFile = new System.Windows.Forms.ListBox();
            this.contextMenuStripDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除选中配置文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStripDelete.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(449, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "操作提示:请选择配对的字段,【标签】选择一项，【目标】选择一项后双击目标项。";
            // 
            // listbox匹配结果
            // 
            this.listbox匹配结果.ContextMenuStrip = this.contextMenuStrip1;
            this.listbox匹配结果.FormattingEnabled = true;
            this.listbox匹配结果.ItemHeight = 12;
            this.listbox匹配结果.Location = new System.Drawing.Point(778, 51);
            this.listbox匹配结果.Name = "listbox匹配结果";
            this.listbox匹配结果.Size = new System.Drawing.Size(402, 496);
            this.listbox匹配结果.TabIndex = 15;
            this.listbox匹配结果.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listbox匹配结果_DrawItem);
            this.listbox匹配结果.SelectedIndexChanged += new System.EventHandler(this.listbox匹配结果_SelectedIndexChanged);
            this.listbox匹配结果.DoubleClick += new System.EventHandler(this.listbox匹配结果_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.指定为这次操作的主键标识ToolStripMenuItem,
            this.清空匹配结果ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(221, 48);
            // 
            // 指定为这次操作的主键标识ToolStripMenuItem
            // 
            this.指定为这次操作的主键标识ToolStripMenuItem.Name = "指定为这次操作的主键标识ToolStripMenuItem";
            this.指定为这次操作的主键标识ToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.指定为这次操作的主键标识ToolStripMenuItem.Text = "指定为这次操作的主键标识";
            this.指定为这次操作的主键标识ToolStripMenuItem.Click += new System.EventHandler(this.指定为这次操作的主键标识ToolStripMenuItem_Click);
            // 
            // 清空匹配结果ToolStripMenuItem
            // 
            this.清空匹配结果ToolStripMenuItem.Name = "清空匹配结果ToolStripMenuItem";
            this.清空匹配结果ToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.清空匹配结果ToolStripMenuItem.Text = "清空匹配结果";
            this.清空匹配结果ToolStripMenuItem.Click += new System.EventHandler(this.清空匹配结果ToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "数据库字段";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(305, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "Excel字段";
            // 
            // listBoxDbColumns
            // 
            this.listBoxDbColumns.FormattingEnabled = true;
            this.listBoxDbColumns.ItemHeight = 12;
            this.listBoxDbColumns.Location = new System.Drawing.Point(26, 51);
            this.listBoxDbColumns.Name = "listBoxDbColumns";
            this.listBoxDbColumns.Size = new System.Drawing.Size(305, 496);
            this.listBoxDbColumns.TabIndex = 11;
            this.listBoxDbColumns.SelectedIndexChanged += new System.EventHandler(this.listBoxDbColumns_SelectedIndexChanged);
            // 
            // listBoxExcelColumns
            // 
            this.listBoxExcelColumns.FormattingEnabled = true;
            this.listBoxExcelColumns.ItemHeight = 12;
            this.listBoxExcelColumns.Location = new System.Drawing.Point(389, 51);
            this.listBoxExcelColumns.Name = "listBoxExcelColumns";
            this.listBoxExcelColumns.Size = new System.Drawing.Size(267, 496);
            this.listBoxExcelColumns.TabIndex = 12;
            this.listBoxExcelColumns.SelectedIndexChanged += new System.EventHandler(this.listBoxExcelColumns_SelectedIndexChanged);
            this.listBoxExcelColumns.DoubleClick += new System.EventHandler(this.listBoxExcelColumns_DoubleClick);
            // 
            // btmCancel
            // 
            this.btmCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btmCancel.Location = new System.Drawing.Point(1078, 679);
            this.btmCancel.Name = "btmCancel";
            this.btmCancel.Size = new System.Drawing.Size(191, 72);
            this.btmCancel.TabIndex = 16;
            this.btmCancel.Text = "取消";
            this.btmCancel.UseVisualStyleBackColor = true;
            this.btmCancel.Click += new System.EventHandler(this.btmCancel_Click);
            // 
            // btnSaveMatchResult
            // 
            this.btnSaveMatchResult.Location = new System.Drawing.Point(754, 679);
            this.btnSaveMatchResult.Name = "btnSaveMatchResult";
            this.btnSaveMatchResult.Size = new System.Drawing.Size(187, 72);
            this.btnSaveMatchResult.TabIndex = 18;
            this.btnSaveMatchResult.Text = "保存配对结果";
            this.btnSaveMatchResult.UseVisualStyleBackColor = true;
            this.btnSaveMatchResult.Click += new System.EventHandler(this.btnSaveMatchResult_Click);
            // 
            // txtMatchConfigFileName
            // 
            this.txtMatchConfigFileName.EmptyTextTip = "配对配置文件名字 保持默认";
            this.txtMatchConfigFileName.Location = new System.Drawing.Point(754, 636);
            this.txtMatchConfigFileName.Name = "txtMatchConfigFileName";
            this.txtMatchConfigFileName.Size = new System.Drawing.Size(255, 21);
            this.txtMatchConfigFileName.TabIndex = 19;
            // 
            // listBoxExitFile
            // 
            this.listBoxExitFile.ContextMenuStrip = this.contextMenuStripDelete;
            this.listBoxExitFile.FormattingEnabled = true;
            this.listBoxExitFile.ItemHeight = 12;
            this.listBoxExitFile.Location = new System.Drawing.Point(24, 598);
            this.listBoxExitFile.Name = "listBoxExitFile";
            this.listBoxExitFile.Size = new System.Drawing.Size(667, 196);
            this.listBoxExitFile.TabIndex = 20;
            this.listBoxExitFile.DoubleClick += new System.EventHandler(this.listBoxExitFile_DoubleClick);
            // 
            // contextMenuStripDelete
            // 
            this.contextMenuStripDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除选中配置文件ToolStripMenuItem});
            this.contextMenuStripDelete.Name = "contextMenuStripDelete";
            this.contextMenuStripDelete.Size = new System.Drawing.Size(173, 26);
            // 
            // 删除选中配置文件ToolStripMenuItem
            // 
            this.删除选中配置文件ToolStripMenuItem.Name = "删除选中配置文件ToolStripMenuItem";
            this.删除选中配置文件ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.删除选中配置文件ToolStripMenuItem.Text = "删除选中配置文件";
            this.删除选中配置文件ToolStripMenuItem.Click += new System.EventHandler(this.删除选中配置文件ToolStripMenuItem_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 583);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "已经存在的配对文件";
            // 
            // frmDbColumnToExlColumnConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btmCancel;
            this.ClientSize = new System.Drawing.Size(1301, 807);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listBoxExitFile);
            this.Controls.Add(this.txtMatchConfigFileName);
            this.Controls.Add(this.btnSaveMatchResult);
            this.Controls.Add(this.btmCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxDbColumns);
            this.Controls.Add(this.listBoxExcelColumns);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listbox匹配结果);
            this.Name = "frmDbColumnToExlColumnConfig";
            this.Text = "frmLogisticDbColumnToExlColumnConfig";
            this.Load += new System.EventHandler(this.frmLogisticDbColumnToExlColumnConfig_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStripDelete.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listbox匹配结果;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxDbColumns;
        private System.Windows.Forms.ListBox listBoxExcelColumns;
        private System.Windows.Forms.Button btmCancel;
        private System.Windows.Forms.Button btnSaveMatchResult;
        private System.Windows.Forms.ListBox listBoxExitFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 指定为这次操作的主键标识ToolStripMenuItem;
        public WinLib.WatermarkTextBox txtMatchConfigFileName;
        private System.Windows.Forms.ToolStripMenuItem 清空匹配结果ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDelete;
        private System.Windows.Forms.ToolStripMenuItem 删除选中配置文件ToolStripMenuItem;
    }
}