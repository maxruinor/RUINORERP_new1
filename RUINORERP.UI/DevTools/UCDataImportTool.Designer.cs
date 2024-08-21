using RUINORERP.UI.UControls;

namespace RUINORERP.UI.DevTools
{
    partial class UCDataImportTool
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清空数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成自定义SQL脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.粘贴来自excel的数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.从导入结果表格中更新数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.处理当前列值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbColumnMappingFile = new System.Windows.Forms.ComboBox();
            this.btn保存结果 = new WinLib.ButtonEx();
            this.label4 = new System.Windows.Forms.Label();
            this.btn查看结果 = new WinLib.ButtonEx();
            this.cmb导入所属数据表 = new WinLib.ComboBoxEx();
            this.btn确定操作的列 = new WinLib.ButtonEx();
            this.btn导入数据 = new WinLib.ButtonEx();
            this.bindingSource结果 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource结果)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.kryptonPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1041, 874);
            this.splitContainer1.SplitterDistance = 733;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(733, 874);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(725, 848);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "导入数据";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.FieldNameList = null;
            this.dataGridView1.IsShowSumRow = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 59;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(726, 848);
            this.dataGridView1.SumColumns = null;
            this.dataGridView1.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView1.SumRowCellFormat = "N2";
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.UseCustomColumnDisplay = true;
            this.dataGridView1.UseSelectedColumn = false;
            this.dataGridView1.Use是否使用内置右键功能 = false;
            this.dataGridView1.XmlFileName = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清空数据ToolStripMenuItem,
            this.生成自定义SQL脚本ToolStripMenuItem,
            this.粘贴来自excel的数据ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(193, 70);
            // 
            // 清空数据ToolStripMenuItem
            // 
            this.清空数据ToolStripMenuItem.Name = "清空数据ToolStripMenuItem";
            this.清空数据ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.清空数据ToolStripMenuItem.Text = "清空数据";
            this.清空数据ToolStripMenuItem.Click += new System.EventHandler(this.清空数据ToolStripMenuItem_Click);
            // 
            // 生成自定义SQL脚本ToolStripMenuItem
            // 
            this.生成自定义SQL脚本ToolStripMenuItem.Name = "生成自定义SQL脚本ToolStripMenuItem";
            this.生成自定义SQL脚本ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.生成自定义SQL脚本ToolStripMenuItem.Text = "生成自定义SQL脚本";
            this.生成自定义SQL脚本ToolStripMenuItem.Click += new System.EventHandler(this.生成自定义SQL脚本ToolStripMenuItem_Click);
            // 
            // 粘贴来自excel的数据ToolStripMenuItem
            // 
            this.粘贴来自excel的数据ToolStripMenuItem.Name = "粘贴来自excel的数据ToolStripMenuItem";
            this.粘贴来自excel的数据ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.粘贴来自excel的数据ToolStripMenuItem.Text = "粘贴来自excel的数据";
            this.粘贴来自excel的数据ToolStripMenuItem.Click += new System.EventHandler(this.粘贴来自excel的数据ToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(725, 848);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "导入结果";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige;
            this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.FieldNameList = null;
            this.dataGridView2.IsShowSumRow = false;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 59;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(725, 848);
            this.dataGridView2.SumColumns = null;
            this.dataGridView2.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView2.SumRowCellFormat = "N2";
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.UseCustomColumnDisplay = true;
            this.dataGridView2.UseSelectedColumn = false;
            this.dataGridView2.Use是否使用内置右键功能 = false;
            this.dataGridView2.XmlFileName = "";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.从导入结果表格中更新数据ToolStripMenuItem,
            this.处理当前列值ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(231, 48);
            // 
            // 从导入结果表格中更新数据ToolStripMenuItem
            // 
            this.从导入结果表格中更新数据ToolStripMenuItem.Name = "从导入结果表格中更新数据ToolStripMenuItem";
            this.从导入结果表格中更新数据ToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.从导入结果表格中更新数据ToolStripMenuItem.Text = "从导入结果表格中更新数据";
            this.从导入结果表格中更新数据ToolStripMenuItem.Click += new System.EventHandler(this.从导入结果表格中更新数据ToolStripMenuItem_Click);
            // 
            // 处理当前列值ToolStripMenuItem
            // 
            this.处理当前列值ToolStripMenuItem.Name = "处理当前列值ToolStripMenuItem";
            this.处理当前列值ToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.处理当前列值ToolStripMenuItem.Text = "处理当前选择中的值";
            this.处理当前列值ToolStripMenuItem.Click += new System.EventHandler(this.处理当前列值ToolStripMenuItem_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.label2);
            this.kryptonPanel1.Controls.Add(this.label1);
            this.kryptonPanel1.Controls.Add(this.cmbColumnMappingFile);
            this.kryptonPanel1.Controls.Add(this.btn保存结果);
            this.kryptonPanel1.Controls.Add(this.label4);
            this.kryptonPanel1.Controls.Add(this.btn查看结果);
            this.kryptonPanel1.Controls.Add(this.cmb导入所属数据表);
            this.kryptonPanel1.Controls.Add(this.btn确定操作的列);
            this.kryptonPanel1.Controls.Add(this.btn导入数据);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(304, 874);
            this.kryptonPanel1.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "对应列映射文件";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 362);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 48);
            this.label1.TabIndex = 25;
            this.label1.Text = "思路:以DB表为依据每次导入时列可变化\r\n通过配对识别.如果通过某一列为唯一配对\r\n更新数据可以分两次导入到同一个表.\r\n可以先保存，会更据主键更新。\r\n";
            // 
            // cmbColumnMappingFile
            // 
            this.cmbColumnMappingFile.FormattingEnabled = true;
            this.cmbColumnMappingFile.Location = new System.Drawing.Point(116, 67);
            this.cmbColumnMappingFile.Name = "cmbColumnMappingFile";
            this.cmbColumnMappingFile.Size = new System.Drawing.Size(174, 20);
            this.cmbColumnMappingFile.TabIndex = 28;
            // 
            // btn保存结果
            // 
            this.btn保存结果.Location = new System.Drawing.Point(83, 279);
            this.btn保存结果.Name = "btn保存结果";
            this.btn保存结果.Size = new System.Drawing.Size(122, 35);
            this.btn保存结果.TabIndex = 24;
            this.btn保存结果.Text = "保存结果";
            this.btn保存结果.UseVisualStyleBackColor = true;
            this.btn保存结果.Click += new System.EventHandler(this.btn保存结果_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "导入所属数据表";
            // 
            // btn查看结果
            // 
            this.btn查看结果.Location = new System.Drawing.Point(83, 238);
            this.btn查看结果.Name = "btn查看结果";
            this.btn查看结果.Size = new System.Drawing.Size(122, 35);
            this.btn查看结果.TabIndex = 23;
            this.btn查看结果.Text = "查看结果";
            this.btn查看结果.UseVisualStyleBackColor = true;
            this.btn查看结果.Click += new System.EventHandler(this.btn查看结果_Click);
            // 
            // cmb导入所属数据表
            // 
            this.cmb导入所属数据表.FormattingEnabled = true;
            this.cmb导入所属数据表.Location = new System.Drawing.Point(109, 22);
            this.cmb导入所属数据表.Name = "cmb导入所属数据表";
            this.cmb导入所属数据表.Size = new System.Drawing.Size(181, 20);
            this.cmb导入所属数据表.TabIndex = 21;
            // 
            // btn确定操作的列
            // 
            this.btn确定操作的列.Location = new System.Drawing.Point(83, 182);
            this.btn确定操作的列.Name = "btn确定操作的列";
            this.btn确定操作的列.Size = new System.Drawing.Size(122, 35);
            this.btn确定操作的列.TabIndex = 22;
            this.btn确定操作的列.Text = "确定操作数据列";
            this.btn确定操作的列.UseVisualStyleBackColor = true;
            this.btn确定操作的列.Click += new System.EventHandler(this.buttonEx2_Click);
            // 
            // btn导入数据
            // 
            this.btn导入数据.Location = new System.Drawing.Point(83, 128);
            this.btn导入数据.Name = "btn导入数据";
            this.btn导入数据.Size = new System.Drawing.Size(122, 35);
            this.btn导入数据.TabIndex = 18;
            this.btn导入数据.Text = "导入数据表格";
            this.btn导入数据.UseVisualStyleBackColor = true;
            this.btn导入数据.Click += new System.EventHandler(this.buttonEx1_Click);
            // 
            // UCDataImportTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UCDataImportTool";
            this.Size = new System.Drawing.Size(1041, 874);
            this.Load += new System.EventHandler(this.frmDataImportTools_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource结果)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        // private CustomizeGrid customizeGrid1;
        private WinLib.ButtonEx btn导入数据;
        private WinLib.ComboBoxEx cmb导入所属数据表;
        private System.Windows.Forms.Label label4;
        private WinLib.ButtonEx btn查看结果;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private WinLib.ButtonEx btn保存结果;
        private WinLib.ButtonEx btn确定操作的列;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 粘贴来自excel的数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 处理当前列值ToolStripMenuItem;
        private NewSumDataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 从导入结果表格中更新数据ToolStripMenuItem;
        private NewSumDataGridView dataGridView2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbColumnMappingFile;
        private System.Windows.Forms.ToolStripMenuItem 生成自定义SQL脚本ToolStripMenuItem;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private System.Windows.Forms.BindingSource bindingSource结果;
    }
}
