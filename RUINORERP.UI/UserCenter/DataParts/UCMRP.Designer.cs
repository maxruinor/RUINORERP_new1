namespace RUINORERP.UI.UserCenter.DataParts
{
    partial class UCMRP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMRP));
            this.kryptonTreeGridView1 = new Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonTreeGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonTreeGridView1
            // 
            this.kryptonTreeGridView1.AllowUserToAddRows = false;
            this.kryptonTreeGridView1.AllowUserToDeleteRows = false;
            this.kryptonTreeGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.kryptonTreeGridView1.DataSource = null;
            this.kryptonTreeGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonTreeGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.kryptonTreeGridView1.HideColumns = ((System.Collections.Generic.List<string>)(resources.GetObject("kryptonTreeGridView1.HideColumns")));
            this.kryptonTreeGridView1.ImageList = null;
            this.kryptonTreeGridView1.Location = new System.Drawing.Point(0, 0);
            this.kryptonTreeGridView1.Name = "kryptonTreeGridView1";
            this.kryptonTreeGridView1.ParentIdRootValue = ((long)(0));
            this.kryptonTreeGridView1.RowHeadersVisible = false;
            this.kryptonTreeGridView1.SelectFilter = "";
            this.kryptonTreeGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonTreeGridView1.Size = new System.Drawing.Size(1138, 371);
            this.kryptonTreeGridView1.SortColumnName = "";
            this.kryptonTreeGridView1.TabIndex = 3;
            this.kryptonTreeGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonTreeGridView1_CellContentDoubleClick);
            this.kryptonTreeGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.treeGridView1_CellPainting);
            // 
            // UCMRP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonTreeGridView1);
            this.Name = "UCMRP";
            this.Size = new System.Drawing.Size(1138, 371);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonTreeGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView kryptonTreeGridView1;
    }
}
