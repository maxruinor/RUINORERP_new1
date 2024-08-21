namespace Mainframe.SysTools
{
    partial class frmDataImportTools
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataImportTools));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cmbColumnMappingFile = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn保存结果 = new WinLib.ButtonEx();
            this.btn查看结果 = new WinLib.ButtonEx();
            this.btn确定操作的列 = new WinLib.ButtonEx();
            this.cmb导入所属数据表 = new WinLib.ComboBoxEx();
            this.label4 = new System.Windows.Forms.Label();
            this.customizeGrid1 = new HLH.WinControl.Mycontrol.CustomizeGrid();
            this.btn导入数据 = new WinLib.ButtonEx();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new HLH.WinControl.MyDataGrid.NewSumDataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new HLH.WinControl.MyDataGrid.NewSumDataGridView();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.从导入结果表格中更新数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.粘贴来自excel的数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.处理当前列值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成自定义SQL脚本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.cmbColumnMappingFile);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.btn保存结果);
            this.splitContainer1.Panel1.Controls.Add(this.btn查看结果);
            this.splitContainer1.Panel1.Controls.Add(this.btn确定操作的列);
            this.splitContainer1.Panel1.Controls.Add(this.cmb导入所属数据表);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.customizeGrid1);
            this.splitContainer1.Panel1.Controls.Add(this.btn导入数据);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1041, 874);
            this.splitContainer1.SplitterDistance = 119;
            this.splitContainer1.TabIndex = 0;
            // 
            // cmbColumnMappingFile
            // 
            this.cmbColumnMappingFile.FormattingEnabled = true;
            this.cmbColumnMappingFile.Location = new System.Drawing.Point(109, 40);
            this.cmbColumnMappingFile.Name = "cmbColumnMappingFile";
            this.cmbColumnMappingFile.Size = new System.Drawing.Size(181, 20);
            this.cmbColumnMappingFile.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "对应的列映射文件";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(827, 12);
            this.label1.TabIndex = 25;
            this.label1.Text = "思路:以DB表为依据每次导入时列可变化。，通过配对识别.如果通过某一列为唯一配对更新数据可以分两次导入到同一个表.可以先保存，会更据主键更新。\r\n";
            // 
            // btn保存结果
            // 
            this.btn保存结果.Location = new System.Drawing.Point(846, 31);
            this.btn保存结果.Name = "btn保存结果";
            this.btn保存结果.Size = new System.Drawing.Size(70, 35);
            this.btn保存结果.TabIndex = 24;
            this.btn保存结果.Text = "保存结果";
            this.btn保存结果.UseVisualStyleBackColor = true;
            this.btn保存结果.Click += new System.EventHandler(this.btn保存结果_Click);
            // 
            // btn查看结果
            // 
            this.btn查看结果.Location = new System.Drawing.Point(677, 31);
            this.btn查看结果.Name = "btn查看结果";
            this.btn查看结果.Size = new System.Drawing.Size(72, 35);
            this.btn查看结果.TabIndex = 23;
            this.btn查看结果.Text = "查看结果";
            this.btn查看结果.UseVisualStyleBackColor = true;
            this.btn查看结果.Click += new System.EventHandler(this.btn查看结果_Click);
            // 
            // btn确定操作的列
            // 
            this.btn确定操作的列.Location = new System.Drawing.Point(473, 31);
            this.btn确定操作的列.Name = "btn确定操作的列";
            this.btn确定操作的列.Size = new System.Drawing.Size(120, 35);
            this.btn确定操作的列.TabIndex = 22;
            this.btn确定操作的列.Text = "确定操作的数据列";
            this.btn确定操作的列.UseVisualStyleBackColor = true;
            this.btn确定操作的列.Click += new System.EventHandler(this.buttonEx2_Click);
            // 
            // cmb导入所属数据表
            // 
            this.cmb导入所属数据表.FormattingEnabled = true;
            this.cmb导入所属数据表.Location = new System.Drawing.Point(109, 12);
            this.cmb导入所属数据表.Name = "cmb导入所属数据表";
            this.cmb导入所属数据表.Size = new System.Drawing.Size(181, 20);
            this.cmb导入所属数据表.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "导入所属数据表";
            // 
            // customizeGrid1
            // 
            this.customizeGrid1.DisplayText = "本控制功能为设置指定表格的列。";
            this.customizeGrid1.Location = new System.Drawing.Point(944, 43);
            this.customizeGrid1.Name = "customizeGrid1";
            this.customizeGrid1.Size = new System.Drawing.Size(75, 23);
            this.customizeGrid1.TabIndex = 19;
            this.customizeGrid1.targetDataGridView = null;
            this.customizeGrid1.UseVisualStyleBackColor = true;
            // 
            // btn导入数据
            // 
            this.btn导入数据.Location = new System.Drawing.Point(324, 31);
            this.btn导入数据.Name = "btn导入数据";
            this.btn导入数据.Size = new System.Drawing.Size(101, 35);
            this.btn导入数据.TabIndex = 18;
            this.btn导入数据.Text = "导入数据表格";
            this.btn导入数据.UseVisualStyleBackColor = true;
            this.btn导入数据.Click += new System.EventHandler(this.buttonEx1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1041, 751);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1033, 725);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "导入数据";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Beige;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【删除选中行】", true, false, "删除选中行"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【批量修改列值】", true, false, "NewSumDataGridView_批量修改列值"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【复制单元格数据】", true, false, "NewSumDataGridView_复制单元数据"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【导出为Excel(97-2003)】", true, false, "NewSumDataGridView_导出excel"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【保存修改的值】", true, false, "NewSumDataGridView_保存数据到DB"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【自定义显示列】", true, false, "NewSumDataGridView_自定义列"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【test】", true, false, "NewSumDataGridView_Test"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【删除选中行】", true, false, "删除选中行"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【批量修改列值】", true, false, "NewSumDataGridView_批量修改列值"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【复制单元格数据】", true, false, "NewSumDataGridView_复制单元数据"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【导出为Excel(97-2003)】", true, false, "NewSumDataGridView_导出excel"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【保存修改的值】", true, false, "NewSumDataGridView_保存数据到DB"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【自定义显示列】", true, false, "NewSumDataGridView_自定义列"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【test】", true, false, "NewSumDataGridView_Test"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【删除选中行】", true, false, "删除选中行"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【批量修改列值】", true, false, "NewSumDataGridView_批量修改列值"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【复制单元格数据】", true, false, "NewSumDataGridView_复制单元数据"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【导出为Excel(97-2003)】", true, false, "NewSumDataGridView_导出excel"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【保存修改的值】", true, false, "NewSumDataGridView_保存数据到DB"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【自定义显示列】", true, false, "NewSumDataGridView_自定义列"));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView1.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【test】", true, false, "NewSumDataGridView_Test"));
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.MistyRose;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.SkyBlue;
            this.dataGridView1.IsShowSumRow = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Green;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView1.RowHeadersWidth = 59;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1033, 725);
            this.dataGridView1.SumColumns = null;
            this.dataGridView1.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView1.SumRowCellFormat = "N2";
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Use是否使用内置右键功能 = true;
            this.dataGridView1.VarStoragePara = ((object)(resources.GetObject("dataGridView1.VarStoragePara")));
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1033, 725);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "导入结果";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Beige;
            this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【删除选中行】", true, false, "删除选中行"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【批量修改列值】", true, false, "NewSumDataGridView_批量修改列值"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【复制单元格数据】", true, false, "NewSumDataGridView_复制单元数据"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【导出为Excel(97-2003)】", true, false, "NewSumDataGridView_导出excel"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【保存修改的值】", true, false, "NewSumDataGridView_保存数据到DB"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【自定义显示列】", true, false, "NewSumDataGridView_自定义列"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【test】", true, false, "NewSumDataGridView_Test"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【删除选中行】", true, false, "删除选中行"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【批量修改列值】", true, false, "NewSumDataGridView_批量修改列值"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【复制单元格数据】", true, false, "NewSumDataGridView_复制单元数据"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【导出为Excel(97-2003)】", true, false, "NewSumDataGridView_导出excel"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【保存修改的值】", true, false, "NewSumDataGridView_保存数据到DB"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【自定义显示列】", true, false, "NewSumDataGridView_自定义列"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【test】", true, false, "NewSumDataGridView_Test"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【删除选中行】", true, false, "删除选中行"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【批量修改列值】", true, false, "NewSumDataGridView_批量修改列值"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【复制单元格数据】", true, false, "NewSumDataGridView_复制单元数据"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【导出为Excel(97-2003)】", true, false, "NewSumDataGridView_导出excel"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【保存修改的值】", true, false, "NewSumDataGridView_保存数据到DB"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【自定义显示列】", true, false, "NewSumDataGridView_自定义列"));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【line】", true, true, ""));
            this.dataGridView2.ContextMenucCnfigurator.Add(new HLH.WinControl.MyDataGrid.ContextMenuController("【test】", true, false, "NewSumDataGridView_Test"));
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.MistyRose;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.GridColor = System.Drawing.Color.SkyBlue;
            this.dataGridView2.IsShowSumRow = false;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.Name = "dataGridView2";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.Gold;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Green;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridView2.RowHeadersWidth = 59;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(1033, 725);
            this.dataGridView2.SumColumns = null;
            this.dataGridView2.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView2.SumRowCellFormat = "N2";
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.Use是否使用内置右键功能 = true;
            this.dataGridView2.VarStoragePara = ((object)(resources.GetObject("dataGridView2.VarStoragePara")));
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.从导入结果表格中更新数据ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(221, 26);
            // 
            // 从导入结果表格中更新数据ToolStripMenuItem
            // 
            this.从导入结果表格中更新数据ToolStripMenuItem.Name = "从导入结果表格中更新数据ToolStripMenuItem";
            this.从导入结果表格中更新数据ToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.从导入结果表格中更新数据ToolStripMenuItem.Text = "从导入结果表格中更新数据";
            this.从导入结果表格中更新数据ToolStripMenuItem.Click += new System.EventHandler(this.从导入结果表格中更新数据ToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.粘贴来自excel的数据ToolStripMenuItem,
            this.清空数据ToolStripMenuItem,
            this.处理当前列值ToolStripMenuItem,
            this.生成自定义SQL脚本ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(190, 114);
            // 
            // 粘贴来自excel的数据ToolStripMenuItem
            // 
            this.粘贴来自excel的数据ToolStripMenuItem.Name = "粘贴来自excel的数据ToolStripMenuItem";
            this.粘贴来自excel的数据ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.粘贴来自excel的数据ToolStripMenuItem.Text = "粘贴来自excel的数据";
            this.粘贴来自excel的数据ToolStripMenuItem.Click += new System.EventHandler(this.粘贴来自excel的数据ToolStripMenuItem_Click);
            // 
            // 清空数据ToolStripMenuItem
            // 
            this.清空数据ToolStripMenuItem.Name = "清空数据ToolStripMenuItem";
            this.清空数据ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.清空数据ToolStripMenuItem.Text = "清空数据";
            this.清空数据ToolStripMenuItem.Click += new System.EventHandler(this.清空数据ToolStripMenuItem_Click);
            // 
            // 处理当前列值ToolStripMenuItem
            // 
            this.处理当前列值ToolStripMenuItem.Name = "处理当前列值ToolStripMenuItem";
            this.处理当前列值ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.处理当前列值ToolStripMenuItem.Text = "处理当前选择中的值";
            this.处理当前列值ToolStripMenuItem.Click += new System.EventHandler(this.处理当前列值ToolStripMenuItem_Click);
            // 
            // 生成自定义SQL脚本ToolStripMenuItem
            // 
            this.生成自定义SQL脚本ToolStripMenuItem.Name = "生成自定义SQL脚本ToolStripMenuItem";
            this.生成自定义SQL脚本ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.生成自定义SQL脚本ToolStripMenuItem.Text = "生成自定义SQL脚本";
            this.生成自定义SQL脚本ToolStripMenuItem.Click += new System.EventHandler(this.生成自定义SQL脚本ToolStripMenuItem_Click);
            // 
            // frmDataImportTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 874);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmDataImportTools";
            this.Text = "frmDataImportTools";
            this.Load += new System.EventHandler(this.frmDataImportTools_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private HLH.WinControl.Mycontrol.CustomizeGrid customizeGrid1;
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
        private HLH.WinControl.MyDataGrid.NewSumDataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 从导入结果表格中更新数据ToolStripMenuItem;
        private HLH.WinControl.MyDataGrid.NewSumDataGridView dataGridView2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbColumnMappingFile;
        private System.Windows.Forms.ToolStripMenuItem 生成自定义SQL脚本ToolStripMenuItem;
    }
}