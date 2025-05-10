namespace RUINORERP.UI
{
    partial class frmTest
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
            this.kryptonButton1 = new Krypton.Toolkit.KryptonButton();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.kryptonButton2 = new Krypton.Toolkit.KryptonButton();
            this.btnUseSqlsugar = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.grid1 = new SourceGrid2.Grid();
            this.btnGridTest = new System.Windows.Forms.Button();
            this.grid2 = new SourceGrid.Grid();
            this.btnCslaQuery = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnBIndTest = new System.Windows.Forms.Button();
            this.dataGrid1 = new SourceGrid.DataGrid();
            this.btnDefaultAddElseUpdateTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(770, 47);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton1.TabIndex = 0;
            this.kryptonButton1.Values.Text = "事务测试";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(52, 47);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(370, 47);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(75, 23);
            this.btnUndo.TabIndex = 2;
            this.btnUndo.Text = "撤销";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Location = new System.Drawing.Point(202, 47);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(75, 23);
            this.btnRedo.TabIndex = 3;
            this.btnRedo.Text = "重做";
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.Location = new System.Drawing.Point(566, 47);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.Size = new System.Drawing.Size(134, 25);
            this.kryptonButton2.TabIndex = 4;
            this.kryptonButton2.Values.Text = "AOP内存级缓存测试";
            this.kryptonButton2.Click += new System.EventHandler(this.kryptonButton2_Click);
            // 
            // btnUseSqlsugar
            // 
            this.btnUseSqlsugar.Location = new System.Drawing.Point(684, 13);
            this.btnUseSqlsugar.Name = "btnUseSqlsugar";
            this.btnUseSqlsugar.Size = new System.Drawing.Size(75, 23);
            this.btnUseSqlsugar.TabIndex = 5;
            this.btnUseSqlsugar.Text = "调用DB";
            this.btnUseSqlsugar.UseVisualStyleBackColor = true;
            this.btnUseSqlsugar.Click += new System.EventHandler(this.btnUseSqlsugar_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(52, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // grid1
            // 
            this.grid1.AutoSizeMinHeight = 10;
            this.grid1.AutoSizeMinWidth = 10;
            this.grid1.AutoStretchColumnsToFitWidth = false;
            this.grid1.AutoStretchRowsToFitHeight = false;
            this.grid1.BorderWidth = 0;
            this.grid1.ColumnNames = null;
            this.grid1.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None;
            this.grid1.CustomSort = false;
            this.grid1.DefaultHeight = 18;
            this.grid1.DefaultWidth = 50;
            this.grid1.DrawGrid = true;
            this.grid1.FocusCellEdit = false;
            this.grid1.FocusStyle = SourceGrid2.FocusStyle.None;
            this.grid1.GridToolTipActive = true;
            this.grid1.Header3D = true;
            this.grid1.InEdit = true;
            this.grid1.KeepFocus = true;
            this.grid1.Location = new System.Drawing.Point(69, 175);
            this.grid1.Name = "grid1";
            this.grid1.NoHScroll = false;
            this.grid1.NoVScroll = false;
            this.grid1.ShowFocus = true;
            this.grid1.Size = new System.Drawing.Size(351, 278);
            this.grid1.SpecialKeys = ((SourceGrid2.GridSpecialKeys)(((((((((SourceGrid2.GridSpecialKeys.Ctrl_C | SourceGrid2.GridSpecialKeys.Ctrl_V) 
            | SourceGrid2.GridSpecialKeys.Ctrl_X) 
            | SourceGrid2.GridSpecialKeys.Delete) 
            | SourceGrid2.GridSpecialKeys.Arrows) 
            | SourceGrid2.GridSpecialKeys.Tab) 
            | SourceGrid2.GridSpecialKeys.PageDownUp) 
            | SourceGrid2.GridSpecialKeys.Enter) 
            | SourceGrid2.GridSpecialKeys.Escape)));
            this.grid1.Summary = false;
            this.grid1.SummaryCells = new SourceGrid2.Cells.ICell[] {
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null};
            this.grid1.SummaryColor = System.Drawing.Color.LightBlue;
            this.grid1.SummaryHeight = 30;
            this.grid1.TabIndex = 7;
            // 
            // btnGridTest
            // 
            this.btnGridTest.Location = new System.Drawing.Point(69, 471);
            this.btnGridTest.Name = "btnGridTest";
            this.btnGridTest.Size = new System.Drawing.Size(75, 23);
            this.btnGridTest.TabIndex = 8;
            this.btnGridTest.Text = "GridTest";
            this.btnGridTest.UseVisualStyleBackColor = true;
            this.btnGridTest.Click += new System.EventHandler(this.btnGridTest_Click);
            // 
            // grid2
            // 
            this.grid2.AutoStretchColumnsToFitWidth = true;
            this.grid2.AutoStretchRowsToFitHeight = true;
            this.grid2.ColumnsCount = 5;
            this.grid2.EnableSort = true;
            this.grid2.HasSummary = true;
            this.grid2.Location = new System.Drawing.Point(52, 520);
            this.grid2.Name = "grid2";
            this.grid2.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid2.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid2.Size = new System.Drawing.Size(285, 236);
            this.grid2.TabIndex = 9;
            this.grid2.TabStop = true;
            this.grid2.ToolTipText = "";
            // 
            // btnCslaQuery
            // 
            this.btnCslaQuery.Location = new System.Drawing.Point(779, 90);
            this.btnCslaQuery.Name = "btnCslaQuery";
            this.btnCslaQuery.Size = new System.Drawing.Size(75, 23);
            this.btnCslaQuery.TabIndex = 10;
            this.btnCslaQuery.Text = "CslaQuery";
            this.btnCslaQuery.UseVisualStyleBackColor = true;
            this.btnCslaQuery.Click += new System.EventHandler(this.btnCslaQuery_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(458, 175);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(414, 261);
            this.dataGridView1.TabIndex = 11;
            // 
            // btnBIndTest
            // 
            this.btnBIndTest.Location = new System.Drawing.Point(458, 461);
            this.btnBIndTest.Name = "btnBIndTest";
            this.btnBIndTest.Size = new System.Drawing.Size(75, 23);
            this.btnBIndTest.TabIndex = 12;
            this.btnBIndTest.Text = "数据绑定";
            this.btnBIndTest.UseVisualStyleBackColor = true;
            this.btnBIndTest.Click += new System.EventHandler(this.btnBIndTest_Click);
            // 
            // dataGrid1
            // 
            this.dataGrid1.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dataGrid1.EnableSort = false;
            this.dataGrid1.FixedRows = 1;
            this.dataGrid1.Location = new System.Drawing.Point(427, 520);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dataGrid1.Size = new System.Drawing.Size(464, 254);
            this.dataGrid1.TabIndex = 13;
            this.dataGrid1.TabStop = true;
            this.dataGrid1.ToolTipText = "";
            // 
            // btnDefaultAddElseUpdateTest
            // 
            this.btnDefaultAddElseUpdateTest.Location = new System.Drawing.Point(629, 132);
            this.btnDefaultAddElseUpdateTest.Name = "btnDefaultAddElseUpdateTest";
            this.btnDefaultAddElseUpdateTest.Size = new System.Drawing.Size(225, 23);
            this.btnDefaultAddElseUpdateTest.TabIndex = 14;
            this.btnDefaultAddElseUpdateTest.Text = "DefaultAddElseUpdate";
            this.btnDefaultAddElseUpdateTest.UseVisualStyleBackColor = true;
            this.btnDefaultAddElseUpdateTest.Click += new System.EventHandler(this.btnDefaultAddElseUpdateTest_Click);
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 810);
            this.Controls.Add(this.btnDefaultAddElseUpdateTest);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.btnBIndTest);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnCslaQuery);
            this.Controls.Add(this.grid2);
            this.Controls.Add(this.btnGridTest);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnUseSqlsugar);
            this.Controls.Add(this.kryptonButton2);
            this.Controls.Add(this.btnRedo);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.kryptonButton1);
            this.Name = "frmTest";
            this.Text = "frmTest";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton kryptonButton1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnRedo;
        private Krypton.Toolkit.KryptonButton kryptonButton2;
        private System.Windows.Forms.Button btnUseSqlsugar;
        private System.Windows.Forms.Button button1;
        private SourceGrid2.Grid grid1;
        private System.Windows.Forms.Button btnGridTest;
        private SourceGrid.Grid grid2;
        private System.Windows.Forms.Button btnCslaQuery;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnBIndTest;
        private SourceGrid.DataGrid dataGrid1;
        private System.Windows.Forms.Button btnDefaultAddElseUpdateTest;
    }
}