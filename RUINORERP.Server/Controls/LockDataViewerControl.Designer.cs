namespace RUINORERP.Server.Controls
{
    partial class LockDataViewerControl
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
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            groupBoxTableList = new System.Windows.Forms.GroupBox();
            listBoxTableList = new System.Windows.Forms.ListBox();
            groupBoxDataView = new System.Windows.Forms.GroupBox();
            dataGridViewData = new System.Windows.Forms.DataGridView();
            panelButtons = new System.Windows.Forms.Panel();
            btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            groupBoxTableList.SuspendLayout();
            groupBoxDataView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewData).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(0, 0);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(groupBoxTableList);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(groupBoxDataView);
            splitContainerMain.Size = new System.Drawing.Size(933, 850);
            splitContainerMain.SplitterDistance = 233;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 0;
            // 
            // groupBoxTableList
            // 
            groupBoxTableList.Controls.Add(listBoxTableList);
            groupBoxTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxTableList.Location = new System.Drawing.Point(0, 0);
            groupBoxTableList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBoxTableList.Name = "groupBoxTableList";
            groupBoxTableList.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBoxTableList.Size = new System.Drawing.Size(233, 850);
            groupBoxTableList.TabIndex = 0;
            groupBoxTableList.TabStop = false;
            groupBoxTableList.Text = "数据表列表";
            // 
            // listBoxTableList
            // 
            listBoxTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxTableList.FormattingEnabled = true;
            listBoxTableList.ItemHeight = 17;
            listBoxTableList.Location = new System.Drawing.Point(4, 20);
            listBoxTableList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            listBoxTableList.Name = "listBoxTableList";
            listBoxTableList.Size = new System.Drawing.Size(225, 826);
            listBoxTableList.TabIndex = 0;
            listBoxTableList.SelectedIndexChanged += listBoxTableList_SelectedIndexChanged;
            // 
            // groupBoxDataView
            // 
            groupBoxDataView.Controls.Add(dataGridViewData);
            groupBoxDataView.Controls.Add(panelButtons);
            groupBoxDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxDataView.Location = new System.Drawing.Point(0, 0);
            groupBoxDataView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBoxDataView.Name = "groupBoxDataView";
            groupBoxDataView.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBoxDataView.Size = new System.Drawing.Size(695, 850);
            groupBoxDataView.TabIndex = 0;
            groupBoxDataView.TabStop = false;
            groupBoxDataView.Text = "数据视图";
            // 
            // dataGridViewData
            // 
            dataGridViewData.AllowUserToAddRows = false;
            dataGridViewData.AllowUserToDeleteRows = false;
            dataGridViewData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewData.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewData.Location = new System.Drawing.Point(4, 20);
            dataGridViewData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            dataGridViewData.Name = "dataGridViewData";
            dataGridViewData.ReadOnly = true;
            dataGridViewData.Size = new System.Drawing.Size(687, 759);
            dataGridViewData.TabIndex = 1;
            dataGridViewData.CellDoubleClick += dataGridViewData_CellDoubleClick;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(btnRefresh);
            panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelButtons.Location = new System.Drawing.Point(4, 779);
            panelButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new System.Drawing.Size(687, 67);
            panelButtons.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new System.Drawing.Point(134, 17);
            btnRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(88, 33);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "刷新";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // LockDataViewerControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainerMain);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "LockDataViewerControl";
            Size = new System.Drawing.Size(933, 850);
            Load += DataViewerControl_Load;
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            groupBoxTableList.ResumeLayout(false);
            groupBoxDataView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewData).EndInit();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.GroupBox groupBoxTableList;
        private System.Windows.Forms.ListBox listBoxTableList;
        private System.Windows.Forms.GroupBox groupBoxDataView;
        private System.Windows.Forms.DataGridView dataGridViewData;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnLockStatistics;
        private System.Windows.Forms.Label lblLockStats;
        private System.Windows.Forms.Timer statsUpdateTimer;
    }
}