using RUINOR.WinFormsUI;

namespace RUINOR.WinFormsUI.Demo.TreeGridView
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            RUINOR.WinFormsUI.TreeGridNode treeGridNode1 = new RUINOR.WinFormsUI.TreeGridNode();
            RUINOR.WinFormsUI.TreeGridNode treeGridNode2 = new RUINOR.WinFormsUI.TreeGridNode();
            RUINOR.WinFormsUI.TreeGridNode treeGridNode3 = new RUINOR.WinFormsUI.TreeGridNode();
            RUINOR.WinFormsUI.TreeGridNode treeGridNode4 = new RUINOR.WinFormsUI.TreeGridNode();
            RUINOR.WinFormsUI.TreeGridNode treeGridNode5 = new RUINOR.WinFormsUI.TreeGridNode();
            this.imageStrip = new System.Windows.Forms.ImageList(this.components);
            this.treeGridView1 = new RUINOR.WinFormsUI.TreeGridView();
            this.attachmentColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.subjectColumn = new RUINOR.WinFormsUI.TreeGridColumn();
            this.fromColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new RUINOR.WinFormsUI.TreeGridColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeGridView2 = new RUINOR.WinFormsUI.TreeGridView();
            this.Tree = new RUINOR.WinFormsUI.TreeGridColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // imageStrip
            // 
            this.imageStrip.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageStrip.ImageSize = new System.Drawing.Size(16, 16);
            this.imageStrip.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // treeGridView1
            // 
            this.treeGridView1.AllowUserToAddRows = false;
            this.treeGridView1.AllowUserToDeleteRows = false;
            this.treeGridView1.AllowUserToOrderColumns = true;
            this.treeGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.treeGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.treeGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.treeGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.attachmentColumn,
            this.subjectColumn,
            this.fromColumn,
            this.dateColumn});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.treeGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.treeGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.treeGridView1.ImageList = null;
            this.treeGridView1.Location = new System.Drawing.Point(12, 12);
            this.treeGridView1.Name = "treeGridView1";
            this.treeGridView1.RowHeadersVisible = false;
            this.treeGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.treeGridView1.ShowLines = false;
            this.treeGridView1.Size = new System.Drawing.Size(530, 442);
            this.treeGridView1.TabIndex = 3;
            // 
            // attachmentColumn
            // 
            this.attachmentColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = null;
            this.attachmentColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.attachmentColumn.FillWeight = 51.53443F;
            this.attachmentColumn.HeaderText = "";
            this.attachmentColumn.MinimumWidth = 25;
            this.attachmentColumn.Name = "attachmentColumn";
            this.attachmentColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.attachmentColumn.Width = 25;
            // 
            // subjectColumn
            // 
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.subjectColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.subjectColumn.DefaultNodeImage = null;
            this.subjectColumn.FillWeight = 386.9562F;
            this.subjectColumn.HeaderText = "Subject";
            this.subjectColumn.Name = "subjectColumn";
            this.subjectColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // fromColumn
            // 
            this.fromColumn.FillWeight = 50F;
            this.fromColumn.HeaderText = "From";
            this.fromColumn.Name = "fromColumn";
            this.fromColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dateColumn
            // 
            this.dateColumn.FillWeight = 50F;
            this.dateColumn.HeaderText = "Date";
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            this.Column1.DefaultNodeImage = null;
            this.Column1.Name = "Column1";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "cdmusic.ico");
            this.imageList1.Images.SetKeyName(1, "cellphone.ico");
            this.imageList1.Images.SetKeyName(2, "CONTACTS.ICO");
            this.imageList1.Images.SetKeyName(3, "delete_16x.ico");
            this.imageList1.Images.SetKeyName(4, "disconnect2.ico");
            this.imageList1.Images.SetKeyName(5, "disconnect3.ico");
            this.imageList1.Images.SetKeyName(6, "document.ico");
            this.imageList1.Images.SetKeyName(7, "error.ico");
            this.imageList1.Images.SetKeyName(8, "GenVideoDoc.ico");
            this.imageList1.Images.SetKeyName(9, "globe.ico");
            this.imageList1.Images.SetKeyName(10, "group.ico");
            this.imageList1.Images.SetKeyName(11, "help.ico");
            this.imageList1.Images.SetKeyName(12, "helpdoc.ico");
            this.imageList1.Images.SetKeyName(13, "homenet.ico");
            this.imageList1.Images.SetKeyName(14, "hotplug.ico");
            this.imageList1.Images.SetKeyName(15, "ICS client.ico");
            // 
            // treeGridView2
            // 
            this.treeGridView2.AllowUserToAddRows = false;
            this.treeGridView2.AllowUserToDeleteRows = false;
            this.treeGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Tree,
            this.nameColumn,
            this.Desc});
            this.treeGridView2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.treeGridView2.ImageList = this.imageList1;
            this.treeGridView2.Location = new System.Drawing.Point(570, 12);
            this.treeGridView2.Name = "treeGridView2";
            treeGridNode1.Height = 23;
            treeGridNode2.Height = 23;
            treeGridNode2.ImageIndex = 6;
            treeGridNode3.Height = 23;
            treeGridNode3.ImageIndex = 11;
            treeGridNode4.Height = 23;
            treeGridNode4.ImageIndex = 14;
            treeGridNode3.Nodes.Add(treeGridNode4);
            treeGridNode5.Height = 23;
            this.treeGridView2.Nodes.Add(treeGridNode1);
            this.treeGridView2.Nodes.Add(treeGridNode2);
            this.treeGridView2.Nodes.Add(treeGridNode3);
            this.treeGridView2.Nodes.Add(treeGridNode5);
            this.treeGridView2.Size = new System.Drawing.Size(451, 442);
            this.treeGridView2.TabIndex = 6;
            // 
            // Tree
            // 
            this.Tree.DefaultNodeImage = null;
            this.Tree.HeaderText = "Tree";
            this.Tree.Name = "Tree";
            this.Tree.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // nameColumn
            // 
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.nameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Desc
            // 
            this.Desc.HeaderText = "Desc";
            this.Desc.Name = "Desc";
            this.Desc.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Desc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(41, 601);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1033, 686);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.treeGridView2);
            this.Controls.Add(this.treeGridView1);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.Text = "News Reader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        private RUINOR.WinFormsUI.TreeGridView expandGrid1;
        private TreeGridNode treeGridNode1;
        private TreeGridNode treeGridNode2;
        private TreeGridNode treeGridNode3;
        private RUINOR.WinFormsUI.TreeGridView treeGridView1;
        private TreeGridColumn treeGridColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private TreeGridColumn treeGridColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private TreeGridColumn Column1;
        private System.Windows.Forms.ImageList imageStrip;
        private System.Windows.Forms.DataGridViewImageColumn attachmentColumn;
        private TreeGridColumn subjectColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.ImageList imageList1;
        private WinFormsUI.TreeGridView treeGridView2;
        private TreeGridColumn Tree;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Desc;
        private System.Windows.Forms.Button button1;
    }
}

