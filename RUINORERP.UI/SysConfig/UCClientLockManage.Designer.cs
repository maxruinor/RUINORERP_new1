namespace RUINORERP.UI.SysConfig
{
    partial class UCClientLockManage
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
                if (_refreshTimer != null)
                {
                    _refreshTimer.Stop();
                    _refreshTimer.Dispose();
                }
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnClearAllCache = new Krypton.Toolkit.KryptonButton();
            this.btnUnlockSelected = new Krypton.Toolkit.KryptonButton();
            this.btnRefreshLockData = new Krypton.Toolkit.KryptonButton();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblCurrentUserLockedCount = new System.Windows.Forms.Label();
            this.lblExpiredCount = new System.Windows.Forms.Label();
            this.lblLockedCount = new System.Windows.Forms.Label();
            this.lblTotalCacheCount = new System.Windows.Forms.Label();
            this.dgvLockInfo = new Krypton.Toolkit.KryptonDataGridView();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLockInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnClearAllCache);
            this.panelTop.Controls.Add(this.btnUnlockSelected);
            this.panelTop.Controls.Add(this.btnRefreshLockData);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 40);
            this.panelTop.TabIndex = 0;
            // 
            // btnClearAllCache
            // 
            this.btnClearAllCache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAllCache.Location = new System.Drawing.Point(690, 5);
            this.btnClearAllCache.Name = "btnClearAllCache";
            this.btnClearAllCache.Size = new System.Drawing.Size(90, 25);
            this.btnClearAllCache.TabIndex = 2;
            this.btnClearAllCache.Values.Text = "清空所有缓存";
            this.btnClearAllCache.Click += new System.EventHandler(this.btnClearAllCache_Click);
            // 
            // btnUnlockSelected
            // 
            this.btnUnlockSelected.Location = new System.Drawing.Point(100, 5);
            this.btnUnlockSelected.Name = "btnUnlockSelected";
            this.btnUnlockSelected.Size = new System.Drawing.Size(90, 25);
            this.btnUnlockSelected.TabIndex = 1;
            this.btnUnlockSelected.Values.Text = "解锁选中项";
            this.btnUnlockSelected.Click += new System.EventHandler(this.btnUnlockSelected_Click);
            // 
            // btnRefreshLockData
            // 
            this.btnRefreshLockData.Location = new System.Drawing.Point(5, 5);
            this.btnRefreshLockData.Name = "btnRefreshLockData";
            this.btnRefreshLockData.Size = new System.Drawing.Size(90, 25);
            this.btnRefreshLockData.TabIndex = 0;
            this.btnRefreshLockData.Values.Text = "刷新数据";
            this.btnRefreshLockData.Click += new System.EventHandler(this.btnRefreshLockData_Click);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.lblCurrentUserLockedCount);
            this.panelBottom.Controls.Add(this.lblExpiredCount);
            this.panelBottom.Controls.Add(this.lblLockedCount);
            this.panelBottom.Controls.Add(this.lblTotalCacheCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 360);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(800, 30);
            this.panelBottom.TabIndex = 1;
            // 
            // lblCurrentUserLockedCount
            // 
            this.lblCurrentUserLockedCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentUserLockedCount.AutoSize = true;
            this.lblCurrentUserLockedCount.Location = new System.Drawing.Point(600, 8);
            this.lblCurrentUserLockedCount.Name = "lblCurrentUserLockedCount";
            this.lblCurrentUserLockedCount.Size = new System.Drawing.Size(185, 12);
            this.lblCurrentUserLockedCount.TabIndex = 3;
            this.lblCurrentUserLockedCount.Text = "当前用户锁定项数: 0";
            // 
            // lblExpiredCount
            // 
            this.lblExpiredCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExpiredCount.AutoSize = true;
            this.lblExpiredCount.Location = new System.Drawing.Point(485, 8);
            this.lblExpiredCount.Name = "lblExpiredCount";
            this.lblExpiredCount.Size = new System.Drawing.Size(109, 12);
            this.lblExpiredCount.TabIndex = 2;
            this.lblExpiredCount.Text = "过期项数: 0";
            // 
            // lblLockedCount
            // 
            this.lblLockedCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLockedCount.AutoSize = true;
            this.lblLockedCount.Location = new System.Drawing.Point(395, 8);
            this.lblLockedCount.Name = "lblLockedCount";
            this.lblLockedCount.Size = new System.Drawing.Size(85, 12);
            this.lblLockedCount.TabIndex = 1;
            this.lblLockedCount.Text = "锁定项数: 0";
            // 
            // lblTotalCacheCount
            // 
            this.lblTotalCacheCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalCacheCount.AutoSize = true;
            this.lblTotalCacheCount.Location = new System.Drawing.Point(310, 8);
            this.lblTotalCacheCount.Name = "lblTotalCacheCount";
            this.lblTotalCacheCount.Size = new System.Drawing.Size(79, 12);
            this.lblTotalCacheCount.TabIndex = 0;
            this.lblTotalCacheCount.Text = "总缓存数: 0";
            // 
            // dgvLockInfo
            // 
            this.dgvLockInfo.AllowUserToAddRows = false;
            this.dgvLockInfo.AllowUserToDeleteRows = false;
            this.dgvLockInfo.AllowUserToOrderColumns = true;
            this.dgvLockInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLockInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLockInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLockInfo.Location = new System.Drawing.Point(0, 40);
            this.dgvLockInfo.Name = "dgvLockInfo";
            this.dgvLockInfo.ReadOnly = true;
            this.dgvLockInfo.RowTemplate.Height = 23;
            this.dgvLockInfo.Size = new System.Drawing.Size(800, 320);
            this.dgvLockInfo.TabIndex = 2;
            this.dgvLockInfo.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvLockInfo_RowPrePaint);
            // 
            // UCClientLockManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvLockInfo);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Name = "UCClientLockManage";
            this.Size = new System.Drawing.Size(800, 390);
            this.panelTop.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLockInfo)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private Krypton.Toolkit.KryptonDataGridView dgvLockInfo;
        private Krypton.Toolkit.KryptonButton btnRefreshLockData;
        private Krypton.Toolkit.KryptonButton btnUnlockSelected;
        private Krypton.Toolkit.KryptonButton btnClearAllCache;
        private System.Windows.Forms.Label lblTotalCacheCount;
        private System.Windows.Forms.Label lblLockedCount;
        private System.Windows.Forms.Label lblExpiredCount;
        private System.Windows.Forms.Label lblCurrentUserLockedCount;
    }
}
