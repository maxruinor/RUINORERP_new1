﻿namespace RUINORERP.UI.UserCenter.DataParts
{
    partial class UCPURCell_old
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCPURCell));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.kryptonHeaderGroup1 = new Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecHeaderGroup1 = new Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.kryptonCommandRefresh = new Krypton.Toolkit.KryptonCommand();
            this.kryptonPanelSaleMain = new Krypton.Toolkit.KryptonPanel();
            this.kryptonTreeGridView1 = new Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView();
            this.订单状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelSaleMain)).BeginInit();
            this.kryptonPanelSaleMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonTreeGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // kryptonHeaderGroup1
            // 
            this.kryptonHeaderGroup1.ButtonSpecs.Add(this.buttonSpecHeaderGroup1);
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
            // 
            // kryptonHeaderGroup1.Panel
            // 
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.kryptonPanelSaleMain);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(280, 194);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "采购";
            // 
            // buttonSpecHeaderGroup1
            // 
            this.buttonSpecHeaderGroup1.Enabled = Krypton.Toolkit.ButtonEnabled.True;
            this.buttonSpecHeaderGroup1.KryptonCommand = this.kryptonCommandRefresh;
            this.buttonSpecHeaderGroup1.UniqueName = "8303ef0a912e4bd8ab0de4162c460d75";
            // 
            // kryptonCommandRefresh
            // 
            this.kryptonCommandRefresh.AssignedButtonSpec = this.buttonSpecHeaderGroup1;
            this.kryptonCommandRefresh.Text = "刷新";
            this.kryptonCommandRefresh.Execute += new System.EventHandler(this.kryptonCommandRefresh_Execute);
            // 
            // kryptonPanelSaleMain
            // 
            this.kryptonPanelSaleMain.Controls.Add(this.kryptonTreeGridView1);
            this.kryptonPanelSaleMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelSaleMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelSaleMain.Name = "kryptonPanelSaleMain";
            this.kryptonPanelSaleMain.Size = new System.Drawing.Size(278, 141);
            this.kryptonPanelSaleMain.TabIndex = 0;
            // 
            // kryptonTreeGridView1
            // 
            this.kryptonTreeGridView1.AllowUserToAddRows = false;
            this.kryptonTreeGridView1.AllowUserToDeleteRows = false;
            this.kryptonTreeGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.kryptonTreeGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.订单状态,
            this.数量});
            this.kryptonTreeGridView1.DataSource = null;
            this.kryptonTreeGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.kryptonTreeGridView1.HideColumns = ((System.Collections.Generic.List<string>)(resources.GetObject("kryptonTreeGridView1.HideColumns")));
            this.kryptonTreeGridView1.ImageList = null;
            this.kryptonTreeGridView1.Location = new System.Drawing.Point(3, 3);
            this.kryptonTreeGridView1.Name = "kryptonTreeGridView1";
            this.kryptonTreeGridView1.ParentIdRootValue = ((long)(0));
            this.kryptonTreeGridView1.RowHeadersVisible = false;
            this.kryptonTreeGridView1.SelectFilter = "";
            this.kryptonTreeGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonTreeGridView1.Size = new System.Drawing.Size(412, 273);
            this.kryptonTreeGridView1.SortColumnName = "";
            this.kryptonTreeGridView1.TabIndex = 2;
            this.kryptonTreeGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonTreeGridView1_CellContentDoubleClick);
            // 
            // 订单状态
            // 
            this.订单状态.Frozen = true;
            this.订单状态.HeaderText = "订单状态";
            this.订单状态.Name = "订单状态";
            this.订单状态.ReadOnly = true;
            this.订单状态.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.订单状态.Width = 150;
            // 
            // 数量
            // 
            this.数量.Frozen = true;
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            this.数量.ReadOnly = true;
            this.数量.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.数量.Width = 120;
            // 
            // UCPURCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonHeaderGroup1);
            this.Name = "UCPURCell";
            this.Size = new System.Drawing.Size(280, 194);
            this.Load += new System.EventHandler(this.UCPURCell_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelSaleMain)).EndInit();
            this.kryptonPanelSaleMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonTreeGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private Krypton.Toolkit.KryptonPanel kryptonPanelSaleMain;
        private System.Windows.Forms.Timer timer1;
        private Krypton.Toolkit.Suite.Extended.TreeGridView.KryptonTreeGridView kryptonTreeGridView1;
        private Krypton.Toolkit.KryptonCommand kryptonCommandRefresh;
        private Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单状态;
        private System.Windows.Forms.DataGridViewLinkColumn 数量;
    }
}
