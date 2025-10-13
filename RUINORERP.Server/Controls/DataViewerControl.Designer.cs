namespace RUINORERP.Server.Controls
{
    partial class DataViewerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.groupBoxTableList = new System.Windows.Forms.GroupBox();
            this.listBoxTableList = new System.Windows.Forms.ListBox();
            this.groupBoxDataView = new System.Windows.Forms.GroupBox();
            this.dataGridViewData = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnReloadCache = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.groupBoxTableList.SuspendLayout();
            this.groupBoxDataView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewData)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.groupBoxTableList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.groupBoxDataView);
            this.splitContainerMain.Size = new System.Drawing.Size(800, 600);
            this.splitContainerMain.SplitterDistance = 200;
            this.splitContainerMain.TabIndex = 0;
            // 
            // groupBoxTableList
            // 
            this.groupBoxTableList.Controls.Add(this.listBoxTableList);
            this.groupBoxTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTableList.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTableList.Name = "groupBoxTableList";
            this.groupBoxTableList.Size = new System.Drawing.Size(200, 600);
            this.groupBoxTableList.TabIndex = 0;
            this.groupBoxTableList.TabStop = false;
            this.groupBoxTableList.Text = "数据表列表";
            // 
            // listBoxTableList
            // 
            this.listBoxTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxTableList.FormattingEnabled = true;
            this.listBoxTableList.ItemHeight = 12;
            this.listBoxTableList.Location = new System.Drawing.Point(3, 19);
            this.listBoxTableList.Name = "listBoxTableList";
            this.listBoxTableList.Size = new System.Drawing.Size(194, 578);
            this.listBoxTableList.TabIndex = 0;
            this.listBoxTableList.SelectedIndexChanged += new System.EventHandler(this.listBoxTableList_SelectedIndexChanged);
            // 
            // groupBoxDataView
            // 
            this.groupBoxDataView.Controls.Add(this.dataGridViewData);
            this.groupBoxDataView.Controls.Add(this.panelButtons);
            this.groupBoxDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDataView.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDataView.Name = "groupBoxDataView";
            this.groupBoxDataView.Size = new System.Drawing.Size(596, 600);
            this.groupBoxDataView.TabIndex = 0;
            this.groupBoxDataView.TabStop = false;
            this.groupBoxDataView.Text = "数据视图";
            // 
            // dataGridViewData
            // 
            this.dataGridViewData.AllowUserToAddRows = false;
            this.dataGridViewData.AllowUserToDeleteRows = false;
            this.dataGridViewData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewData.Location = new System.Drawing.Point(3, 19);
            this.dataGridViewData.Name = "dataGridViewData";
            this.dataGridViewData.ReadOnly = true;
            this.dataGridViewData.Size = new System.Drawing.Size(590, 531);
            this.dataGridViewData.TabIndex = 1;
            this.dataGridViewData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewData_CellDoubleClick);
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnReloadCache);
            this.panelButtons.Controls.Add(this.btnExport);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(3, 550);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(590, 47);
            this.panelButtons.TabIndex = 0;
            // 
            // btnReloadCache
            // 
            this.btnReloadCache.Location = new System.Drawing.Point(200, 12);
            this.btnReloadCache.Name = "btnReloadCache";
            this.btnReloadCache.Size = new System.Drawing.Size(85, 23);
            this.btnReloadCache.TabIndex = 2;
            this.btnReloadCache.Text = "重新加载缓存";
            this.btnReloadCache.UseVisualStyleBackColor = true;
            this.btnReloadCache.Click += new System.EventHandler(this.btnReloadCache_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(100, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "导出数据";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(15, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // DataViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Name = "DataViewerControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.DataViewerControl_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.groupBoxTableList.ResumeLayout(false);
            this.groupBoxDataView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewData)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.GroupBox groupBoxTableList;
        private System.Windows.Forms.ListBox listBoxTableList;
        private System.Windows.Forms.GroupBox groupBoxDataView;
        private System.Windows.Forms.DataGridView dataGridViewData;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnReloadCache;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnRefresh;
    }
}