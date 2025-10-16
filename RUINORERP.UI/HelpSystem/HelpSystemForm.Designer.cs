namespace RUINORERP.UI.HelpSystem
{
    partial class HelpSystemForm
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
            this.kryptonSplitContainerMain = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonSplitContainerLeft = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonPanelSearch = new Krypton.Toolkit.KryptonPanel();
            this.kryptonTextBoxSearch = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonButtonSearch = new Krypton.Toolkit.KryptonButton();
            this.kryptonNavigatorTabs = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPageSearch = new Krypton.Navigator.KryptonPage();
            this.kryptonListBoxSearchResults = new Krypton.Toolkit.KryptonListBox();
            this.kryptonPageRecommendations = new Krypton.Navigator.KryptonPage();
            this.kryptonListBoxRecommendations = new Krypton.Toolkit.KryptonListBox();
            this.kryptonPageHistory = new Krypton.Navigator.KryptonPage();
            this.kryptonPanelHistory = new Krypton.Toolkit.KryptonPanel();
            this.kryptonListBoxHistory = new Krypton.Toolkit.KryptonListBox();
            this.kryptonButtonClearHistory = new Krypton.Toolkit.KryptonButton();
            this.webView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.kryptonStatusStrip = new Krypton.Toolkit.KryptonStatusStrip();
            this.kryptonStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain.Panel1)).BeginInit();
            this.kryptonSplitContainerMain.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain.Panel2)).BeginInit();
            this.kryptonSplitContainerMain.Panel2.SuspendLayout();
            this.kryptonSplitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerLeft.Panel1)).BeginInit();
            this.kryptonSplitContainerLeft.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerLeft.Panel2)).BeginInit();
            this.kryptonSplitContainerLeft.Panel2.SuspendLayout();
            this.kryptonSplitContainerLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelSearch)).BeginInit();
            this.kryptonPanelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorTabs)).BeginInit();
            this.kryptonNavigatorTabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSearch)).BeginInit();
            this.kryptonPageSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageRecommendations)).BeginInit();
            this.kryptonPageRecommendations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageHistory)).BeginInit();
            this.kryptonPageHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelHistory)).BeginInit();
            this.kryptonPanelHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView2)).BeginInit();
            this.kryptonStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonSplitContainerMain
            // 
            this.kryptonSplitContainerMain.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainerMain.Name = "kryptonSplitContainerMain";
            this.kryptonSplitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainerMain.Panel1
            // 
            this.kryptonSplitContainerMain.Panel1.Controls.Add(this.kryptonSplitContainerLeft);
            // 
            // kryptonSplitContainerMain.Panel2
            // 
            this.kryptonSplitContainerMain.Panel2.Controls.Add(this.webView2);
            this.kryptonSplitContainerMain.Panel2.Controls.Add(this.kryptonStatusStrip);
            this.kryptonSplitContainerMain.Size = new System.Drawing.Size(800, 580);
            this.kryptonSplitContainerMain.SplitterDistance = 300;
            this.kryptonSplitContainerMain.TabIndex = 0;
            // 
            // kryptonSplitContainerLeft
            // 
            this.kryptonSplitContainerLeft.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainerLeft.Name = "kryptonSplitContainerLeft";
            // 
            // kryptonSplitContainerLeft.Panel1
            // 
            this.kryptonSplitContainerLeft.Panel1.Controls.Add(this.kryptonPanelSearch);
            // 
            // kryptonSplitContainerLeft.Panel2
            // 
            this.kryptonSplitContainerLeft.Panel2.Controls.Add(this.kryptonNavigatorTabs);
            this.kryptonSplitContainerLeft.Size = new System.Drawing.Size(800, 300);
            this.kryptonSplitContainerLeft.SplitterDistance = 200;
            this.kryptonSplitContainerLeft.TabIndex = 0;
            // 
            // kryptonPanelSearch
            // 
            this.kryptonPanelSearch.Controls.Add(this.kryptonTextBoxSearch);
            this.kryptonPanelSearch.Controls.Add(this.kryptonButtonSearch);
            this.kryptonPanelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanelSearch.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelSearch.Name = "kryptonPanelSearch";
            this.kryptonPanelSearch.Size = new System.Drawing.Size(200, 30);
            this.kryptonPanelSearch.TabIndex = 0;
            // 
            // kryptonTextBoxSearch
            // 
            this.kryptonTextBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonTextBoxSearch.Location = new System.Drawing.Point(0, 0);
            this.kryptonTextBoxSearch.Name = "kryptonTextBoxSearch";
            this.kryptonTextBoxSearch.Size = new System.Drawing.Size(140, 23);
            this.kryptonTextBoxSearch.TabIndex = 0;
            // 
            // kryptonButtonSearch
            // 
            this.kryptonButtonSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.kryptonButtonSearch.Location = new System.Drawing.Point(140, 0);
            this.kryptonButtonSearch.Name = "kryptonButtonSearch";
            this.kryptonButtonSearch.Size = new System.Drawing.Size(60, 30);
            this.kryptonButtonSearch.TabIndex = 1;
            this.kryptonButtonSearch.Values.Text = "搜索";
            // 
            // kryptonNavigatorTabs
            // 
            this.kryptonNavigatorTabs.ControlKryptonFormFeatures = false;
            this.kryptonNavigatorTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonNavigatorTabs.Location = new System.Drawing.Point(0, 0);
            this.kryptonNavigatorTabs.Name = "kryptonNavigatorTabs";
            this.kryptonNavigatorTabs.NavigatorMode = Krypton.Navigator.NavigatorMode.BarTabGroup;
            this.kryptonNavigatorTabs.Owner = null;
            this.kryptonNavigatorTabs.PageBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonNavigatorTabs.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageSearch,
            this.kryptonPageRecommendations,
            this.kryptonPageHistory});
            this.kryptonNavigatorTabs.SelectedIndex = 0;
            this.kryptonNavigatorTabs.Size = new System.Drawing.Size(595, 300);
            this.kryptonNavigatorTabs.TabIndex = 0;
            this.kryptonNavigatorTabs.Text = "kryptonNavigatorTabs";
            // 
            // kryptonPageSearch
            // 
            this.kryptonPageSearch.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageSearch.Controls.Add(this.kryptonListBoxSearchResults);
            this.kryptonPageSearch.Flags = 65534;
            this.kryptonPageSearch.LastVisibleSet = true;
            this.kryptonPageSearch.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageSearch.Name = "kryptonPageSearch";
            this.kryptonPageSearch.Size = new System.Drawing.Size(593, 273);
            this.kryptonPageSearch.Text = "搜索结果";
            this.kryptonPageSearch.ToolTipTitle = "Page ToolTip";
            this.kryptonPageSearch.UniqueName = "kryptonPageSearch";
            // 
            // kryptonListBoxSearchResults
            // 
            this.kryptonListBoxSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonListBoxSearchResults.Location = new System.Drawing.Point(0, 0);
            this.kryptonListBoxSearchResults.Name = "kryptonListBoxSearchResults";
            this.kryptonListBoxSearchResults.Size = new System.Drawing.Size(593, 273);
            this.kryptonListBoxSearchResults.TabIndex = 0;
            // 
            // kryptonPageRecommendations
            // 
            this.kryptonPageRecommendations.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageRecommendations.Controls.Add(this.kryptonListBoxRecommendations);
            this.kryptonPageRecommendations.Flags = 65534;
            this.kryptonPageRecommendations.LastVisibleSet = true;
            this.kryptonPageRecommendations.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageRecommendations.Name = "kryptonPageRecommendations";
            this.kryptonPageRecommendations.Size = new System.Drawing.Size(593, 273);
            this.kryptonPageRecommendations.Text = "推荐内容";
            this.kryptonPageRecommendations.ToolTipTitle = "Page ToolTip";
            this.kryptonPageRecommendations.UniqueName = "kryptonPageRecommendations";
            // 
            // kryptonListBoxRecommendations
            // 
            this.kryptonListBoxRecommendations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonListBoxRecommendations.Location = new System.Drawing.Point(0, 0);
            this.kryptonListBoxRecommendations.Name = "kryptonListBoxRecommendations";
            this.kryptonListBoxRecommendations.Size = new System.Drawing.Size(593, 273);
            this.kryptonListBoxRecommendations.TabIndex = 0;
            // 
            // kryptonPageHistory
            // 
            this.kryptonPageHistory.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageHistory.Controls.Add(this.kryptonPanelHistory);
            this.kryptonPageHistory.Flags = 65534;
            this.kryptonPageHistory.LastVisibleSet = true;
            this.kryptonPageHistory.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageHistory.Name = "kryptonPageHistory";
            this.kryptonPageHistory.Size = new System.Drawing.Size(593, 273);
            this.kryptonPageHistory.Text = "查看历史";
            this.kryptonPageHistory.ToolTipTitle = "Page ToolTip";
            this.kryptonPageHistory.UniqueName = "kryptonPageHistory";
            // 
            // kryptonPanelHistory
            // 
            this.kryptonPanelHistory.Controls.Add(this.kryptonListBoxHistory);
            this.kryptonPanelHistory.Controls.Add(this.kryptonButtonClearHistory);
            this.kryptonPanelHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelHistory.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelHistory.Name = "kryptonPanelHistory";
            this.kryptonPanelHistory.Size = new System.Drawing.Size(593, 273);
            this.kryptonPanelHistory.TabIndex = 0;
            // 
            // kryptonListBoxHistory
            // 
            this.kryptonListBoxHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonListBoxHistory.Location = new System.Drawing.Point(0, 0);
            this.kryptonListBoxHistory.Name = "kryptonListBoxHistory";
            this.kryptonListBoxHistory.Size = new System.Drawing.Size(593, 253);
            this.kryptonListBoxHistory.TabIndex = 1;
            // 
            // kryptonButtonClearHistory
            // 
            this.kryptonButtonClearHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonButtonClearHistory.Location = new System.Drawing.Point(0, 253);
            this.kryptonButtonClearHistory.Name = "kryptonButtonClearHistory";
            this.kryptonButtonClearHistory.Size = new System.Drawing.Size(593, 20);
            this.kryptonButtonClearHistory.TabIndex = 0;
            this.kryptonButtonClearHistory.Values.Text = "清除历史记录";
            // 
            // webView2
            // 
            this.webView2.AllowExternalDrop = true;
            this.webView2.CreationProperties = null;
            this.webView2.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView2.Location = new System.Drawing.Point(0, 0);
            this.webView2.Name = "webView2";
            this.webView2.Size = new System.Drawing.Size(800, 253);
            this.webView2.TabIndex = 0;
            this.webView2.ZoomFactor = 1D;
            // 
            // kryptonStatusStrip
            // 
            this.kryptonStatusStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.kryptonStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kryptonStatusLabel});
            this.kryptonStatusStrip.Location = new System.Drawing.Point(0, 253);
            this.kryptonStatusStrip.Name = "kryptonStatusStrip";
            this.kryptonStatusStrip.ProgressBars = null;
            this.kryptonStatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.kryptonStatusStrip.Size = new System.Drawing.Size(800, 22);
            this.kryptonStatusStrip.TabIndex = 1;
            this.kryptonStatusStrip.Text = "kryptonStatusStrip";
            // 
            // kryptonStatusLabel
            // 
            this.kryptonStatusLabel.Name = "kryptonStatusLabel";
            this.kryptonStatusLabel.Size = new System.Drawing.Size(33, 17);
            this.kryptonStatusLabel.Text = "就绪";
            // 
            // HelpSystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 580);
            this.Controls.Add(this.kryptonSplitContainerMain);
            this.Name = "HelpSystemForm";
            this.Text = "帮助系统";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain.Panel1)).EndInit();
            this.kryptonSplitContainerMain.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain.Panel2)).EndInit();
            this.kryptonSplitContainerMain.Panel2.ResumeLayout(false);
            this.kryptonSplitContainerMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerMain)).EndInit();
            this.kryptonSplitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerLeft.Panel1)).EndInit();
            this.kryptonSplitContainerLeft.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerLeft.Panel2)).EndInit();
            this.kryptonSplitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainerLeft)).EndInit();
            this.kryptonSplitContainerLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelSearch)).EndInit();
            this.kryptonPanelSearch.ResumeLayout(false);
            this.kryptonPanelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorTabs)).EndInit();
            this.kryptonNavigatorTabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageSearch)).EndInit();
            this.kryptonPageSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageRecommendations)).EndInit();
            this.kryptonPageRecommendations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageHistory)).EndInit();
            this.kryptonPageHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelHistory)).EndInit();
            this.kryptonPanelHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.webView2)).EndInit();
            this.kryptonStatusStrip.ResumeLayout(false);
            this.kryptonStatusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainerMain;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainerLeft;
        private Krypton.Navigator.KryptonNavigator kryptonNavigatorTabs;
        private Krypton.Toolkit.KryptonPanel kryptonPanelSearch;
        private Krypton.Toolkit.KryptonTextBox kryptonTextBoxSearch;
        private Krypton.Toolkit.KryptonButton kryptonButtonSearch;
        private Krypton.Navigator.KryptonPage kryptonPageSearch;
        private Krypton.Toolkit.KryptonListBox kryptonListBoxSearchResults;
        private Krypton.Navigator.KryptonPage kryptonPageRecommendations;
        private Krypton.Toolkit.KryptonListBox kryptonListBoxRecommendations;
        private Krypton.Navigator.KryptonPage kryptonPageHistory;
        private Krypton.Toolkit.KryptonPanel kryptonPanelHistory;
        private Krypton.Toolkit.KryptonListBox kryptonListBoxHistory;
        private Krypton.Toolkit.KryptonButton kryptonButtonClearHistory;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView2;
        private Krypton.Toolkit.KryptonStatusStrip kryptonStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel kryptonStatusLabel;
    }
}