namespace RUINORERP.UI.UserCenter.DataParts
{
    partial class UCTodoList
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点0");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点1");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("节点2");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCTodoList));
            this.kryptonTreeViewJobList = new Krypton.Toolkit.KryptonTreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RefreshData = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonTreeViewJobList
            // 
            this.kryptonTreeViewJobList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonTreeViewJobList.ImageIndex = 0;
            this.kryptonTreeViewJobList.ImageList = this.imageList1;
            this.kryptonTreeViewJobList.Location = new System.Drawing.Point(0, 0);
            this.kryptonTreeViewJobList.Name = "kryptonTreeViewJobList";
            treeNode1.Name = "节点0";
            treeNode1.Text = "节点0";
            treeNode2.Name = "节点1";
            treeNode2.Text = "节点1";
            treeNode3.Name = "节点2";
            treeNode3.Text = "节点2";
            this.kryptonTreeViewJobList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.kryptonTreeViewJobList.SelectedImageIndex = 0;
            this.kryptonTreeViewJobList.Size = new System.Drawing.Size(148, 483);
            this.kryptonTreeViewJobList.TabIndex = 1;
            this.kryptonTreeViewJobList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.kryptonTreeViewJobList_NodeMouseDoubleClickAsync);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "TaskListWindow.ico");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshData});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(127, 26);
            // 
            // RefreshData
            // 
            this.RefreshData.Name = "RefreshData";
            this.RefreshData.Size = new System.Drawing.Size(180, 22);
            this.RefreshData.Text = "刷新数据";
            this.RefreshData.Click += new System.EventHandler(this.RefreshData_Click);
            // 
            // UCTodoList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonTreeViewJobList);
            this.Name = "UCTodoList";
            this.Size = new System.Drawing.Size(148, 483);
            this.Load += new System.EventHandler(this.UCTodoList_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal Krypton.Toolkit.KryptonTreeView kryptonTreeViewJobList;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem RefreshData;
    }
}
