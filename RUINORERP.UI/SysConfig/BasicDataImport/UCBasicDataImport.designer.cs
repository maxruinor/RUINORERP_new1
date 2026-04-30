using RUINORERP.UI.UControls;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    partial class UCBasicDataImport
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoAll = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonNavigatorMain = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPageDynamicImport = new Krypton.Navigator.KryptonPage();
            this.kryptonGroupBoxDynamicImport = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonPanel4 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonNavigatorDynamic = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPageRawData = new Krypton.Navigator.KryptonPage();
            this.dgvRawExcelData = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonPageParsedData = new Krypton.Navigator.KryptonPage();
            this.dgvParsedImportData = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonPageFinalPreview = new Krypton.Navigator.KryptonPage();
            this.dgvFinalPreview = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonPanel5 = new Krypton.Toolkit.KryptonPanel();
            this.klblDynamicFilePath = new Krypton.Toolkit.KryptonLabel();
            this.ktxtDynamicFilePath = new Krypton.Toolkit.KryptonTextBox();
            this.kbtnDynamicBrowse = new Krypton.Toolkit.KryptonButton();
            this.klblDynamicEntityType = new Krypton.Toolkit.KryptonLabel();
            this.kcmbDynamicEntityType = new Krypton.Toolkit.KryptonComboBox();
            this.klblDynamicSheetName = new Krypton.Toolkit.KryptonLabel();
            this.kcmbDynamicSheetName = new Krypton.Toolkit.KryptonComboBox();
            this.klblDynamicMappingName = new Krypton.Toolkit.KryptonLabel();
            this.kcmbDynamicMappingName = new Krypton.Toolkit.KryptonComboBox();
            this.kbtnDynamicParse = new Krypton.Toolkit.KryptonButton();
            this.kbtnDynamicMap = new Krypton.Toolkit.KryptonButton();
            this.kbtnDynamicImport = new Krypton.Toolkit.KryptonButton();
            this.kbtnGeneratePreview = new Krypton.Toolkit.KryptonButton();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorMain)).BeginInit();
            this.kryptonNavigatorMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageDynamicImport)).BeginInit();
            this.kryptonPageDynamicImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxDynamicImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxDynamicImport.Panel)).BeginInit();
            this.kryptonGroupBoxDynamicImport.Panel.SuspendLayout();
            this.kryptonGroupBoxDynamicImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel4)).BeginInit();
            this.kryptonPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorDynamic)).BeginInit();
            this.kryptonNavigatorDynamic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageRawData)).BeginInit();
            this.kryptonPageRawData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRawExcelData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageParsedData)).BeginInit();
            this.kryptonPageParsedData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParsedImportData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageFinalPreview)).BeginInit();
            this.kryptonPageFinalPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinalPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel5)).BeginInit();
            this.kryptonPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicEntityType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicSheetName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicMappingName)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAll,
            this.selectNoAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 48);
            // 
            // selectAll
            // 
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(112, 22);
            this.selectAll.Text = "全选";
            // 
            // selectNoAll
            // 
            this.selectNoAll.Name = "selectNoAll";
            this.selectNoAll.Size = new System.Drawing.Size(112, 22);
            this.selectNoAll.Text = "全不选";
            // 
            // kryptonNavigatorMain
            // 
            this.kryptonNavigatorMain.Bar.BarMapExtraText = Krypton.Navigator.MapKryptonPageText.None;
            this.kryptonNavigatorMain.Bar.BarMapImage = Krypton.Navigator.MapKryptonPageImage.Small;
            this.kryptonNavigatorMain.Bar.BarMapText = Krypton.Navigator.MapKryptonPageText.TextTitle;
            this.kryptonNavigatorMain.Bar.BarMultiline = Krypton.Navigator.BarMultiline.Singleline;
            this.kryptonNavigatorMain.Bar.BarOrientation = Krypton.Toolkit.VisualOrientation.Top;
            this.kryptonNavigatorMain.Bar.CheckButtonStyle = Krypton.Toolkit.ButtonStyle.Standalone;
            this.kryptonNavigatorMain.Bar.ItemAlignment = Krypton.Toolkit.RelativePositionAlign.Near;
            this.kryptonNavigatorMain.Bar.ItemMaximumSize = new System.Drawing.Size(200, 200);
            this.kryptonNavigatorMain.Bar.ItemMinimumSize = new System.Drawing.Size(20, 20);
            this.kryptonNavigatorMain.Bar.ItemOrientation = Krypton.Toolkit.ButtonOrientation.Auto;
            this.kryptonNavigatorMain.Bar.ItemSizing = Krypton.Navigator.BarItemSizing.SameHeight;
            this.kryptonNavigatorMain.Bar.TabBorderStyle = Krypton.Toolkit.TabBorderStyle.OneNote;
            this.kryptonNavigatorMain.Bar.TabStyle = Krypton.Toolkit.TabStyle.HighProfile;
            this.kryptonNavigatorMain.ControlKryptonFormFeatures = false;
            this.kryptonNavigatorMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonNavigatorMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonNavigatorMain.Name = "kryptonNavigatorMain";
            this.kryptonNavigatorMain.NavigatorMode = Krypton.Navigator.NavigatorMode.BarTabGroup;
            this.kryptonNavigatorMain.Owner = null;
            this.kryptonNavigatorMain.PageBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonNavigatorMain.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageDynamicImport});
            this.kryptonNavigatorMain.SelectedIndex = 0;
            this.kryptonNavigatorMain.Size = new System.Drawing.Size(1087, 597);
            this.kryptonNavigatorMain.TabIndex = 0;
            // 
            // kryptonPageDynamicImport
            // 
            this.kryptonPageDynamicImport.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageDynamicImport.Controls.Add(this.kryptonGroupBoxDynamicImport);
            this.kryptonPageDynamicImport.Flags = 65534;
            this.kryptonPageDynamicImport.LastVisibleSet = true;
            this.kryptonPageDynamicImport.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageDynamicImport.Name = "kryptonPageDynamicImport";
            this.kryptonPageDynamicImport.Size = new System.Drawing.Size(1085, 566);
            this.kryptonPageDynamicImport.Text = "动态导入";
            this.kryptonPageDynamicImport.TextDescription = "动态Excel文件导入";
            this.kryptonPageDynamicImport.ToolTipTitle = "Page ToolTip";
            this.kryptonPageDynamicImport.UniqueName = "c226ac29ba0740e3b74b2d55fdeb008c";
            // 
            // kryptonGroupBoxDynamicImport
            // 
            this.kryptonGroupBoxDynamicImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBoxDynamicImport.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBoxDynamicImport.Name = "kryptonGroupBoxDynamicImport";
            // 
            // kryptonGroupBoxDynamicImport.Panel
            // 
            this.kryptonGroupBoxDynamicImport.Panel.Controls.Add(this.kryptonPanel4);
            this.kryptonGroupBoxDynamicImport.Size = new System.Drawing.Size(1085, 566);
            this.kryptonGroupBoxDynamicImport.TabIndex = 0;
            this.kryptonGroupBoxDynamicImport.Values.Heading = "动态Excel导入";
            // 
            // kryptonPanel4
            // 
            this.kryptonPanel4.Controls.Add(this.kryptonNavigatorDynamic);
            this.kryptonPanel4.Controls.Add(this.kryptonPanel5);
            this.kryptonPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel4.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel4.Name = "kryptonPanel4";
            this.kryptonPanel4.Size = new System.Drawing.Size(1081, 542);
            this.kryptonPanel4.TabIndex = 0;
            // 
            // kryptonNavigatorDynamic
            // 
            this.kryptonNavigatorDynamic.ControlKryptonFormFeatures = false;
            this.kryptonNavigatorDynamic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonNavigatorDynamic.Location = new System.Drawing.Point(0, 115);
            this.kryptonNavigatorDynamic.Name = "kryptonNavigatorDynamic";
            this.kryptonNavigatorDynamic.NavigatorMode = Krypton.Navigator.NavigatorMode.BarTabGroup;
            this.kryptonNavigatorDynamic.Owner = null;
            this.kryptonNavigatorDynamic.PageBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonNavigatorDynamic.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageRawData,
            this.kryptonPageParsedData,
            this.kryptonPageFinalPreview});
            this.kryptonNavigatorDynamic.SelectedIndex = 1;
            this.kryptonNavigatorDynamic.Size = new System.Drawing.Size(1081, 427);
            this.kryptonNavigatorDynamic.TabIndex = 0;
            // 
            // kryptonPageRawData
            // 
            this.kryptonPageRawData.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageRawData.Controls.Add(this.dgvRawExcelData);
            this.kryptonPageRawData.Flags = 65534;
            this.kryptonPageRawData.LastVisibleSet = true;
            this.kryptonPageRawData.MinimumSize = new System.Drawing.Size(150, 50);
            this.kryptonPageRawData.Name = "kryptonPageRawData";
            this.kryptonPageRawData.Size = new System.Drawing.Size(1079, 366);
            this.kryptonPageRawData.Text = "预览原始数据";
            this.kryptonPageRawData.TextTitle = "原始数据";
            this.kryptonPageRawData.ToolTipTitle = "Page ToolTip";
            this.kryptonPageRawData.UniqueName = "原始数据";
            // 
            // dgvRawExcelData
            // 
            this.dgvRawExcelData.AllowUserToAddRows = false;
            this.dgvRawExcelData.AllowUserToDeleteRows = false;
            this.dgvRawExcelData.AllowUserToOrderColumns = true;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Beige;
            this.dgvRawExcelData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvRawExcelData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRawExcelData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRawExcelData.CustomRowNo = false;
            this.dgvRawExcelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRawExcelData.EnableFiltering = false;
            this.dgvRawExcelData.EnablePagination = false;
            this.dgvRawExcelData.EnableVirtualMode = true;
            this.dgvRawExcelData.IsShowSumRow = false;
            this.dgvRawExcelData.Location = new System.Drawing.Point(0, 0);
            this.dgvRawExcelData.Name = "dgvRawExcelData";
            this.dgvRawExcelData.NeedSaveColumnsXml = true;
            this.dgvRawExcelData.Size = new System.Drawing.Size(1079, 366);
            this.dgvRawExcelData.SumColumns = null;
            this.dgvRawExcelData.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dgvRawExcelData.SumRowCellFormat = "N2";
            this.dgvRawExcelData.TabIndex = 0;
            this.dgvRawExcelData.UseBatchEditColumn = false;
            this.dgvRawExcelData.UseCustomColumnDisplay = true;
            this.dgvRawExcelData.UseSelectedColumn = false;
            this.dgvRawExcelData.Use是否使用内置右键功能 = true;
            this.dgvRawExcelData.VirtualModeThreshold = 5000;
            this.dgvRawExcelData.XmlFileName = "";
            // 
            // kryptonPageParsedData
            // 
            this.kryptonPageParsedData.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageParsedData.Controls.Add(this.dgvParsedImportData);
            this.kryptonPageParsedData.Flags = 65534;
            this.kryptonPageParsedData.LastVisibleSet = true;
            this.kryptonPageParsedData.MinimumSize = new System.Drawing.Size(150, 50);
            this.kryptonPageParsedData.Name = "kryptonPageParsedData";
            this.kryptonPageParsedData.Size = new System.Drawing.Size(1079, 400);
            this.kryptonPageParsedData.Text = "解析结果";
            this.kryptonPageParsedData.TextTitle = "解析数据";
            this.kryptonPageParsedData.ToolTipTitle = "Page ToolTip";
            this.kryptonPageParsedData.UniqueName = "解析数据";
            // 
            // dgvParsedImportData
            // 
            this.dgvParsedImportData.AllowUserToAddRows = false;
            this.dgvParsedImportData.AllowUserToDeleteRows = false;
            this.dgvParsedImportData.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.dgvParsedImportData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvParsedImportData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvParsedImportData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvParsedImportData.CustomRowNo = false;
            this.dgvParsedImportData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvParsedImportData.EnableFiltering = false;
            this.dgvParsedImportData.EnablePagination = false;
            this.dgvParsedImportData.EnableVirtualMode = true;
            this.dgvParsedImportData.IsShowSumRow = false;
            this.dgvParsedImportData.Location = new System.Drawing.Point(0, 0);
            this.dgvParsedImportData.Name = "dgvParsedImportData";
            this.dgvParsedImportData.NeedSaveColumnsXml = true;
            this.dgvParsedImportData.Size = new System.Drawing.Size(1079, 400);
            this.dgvParsedImportData.SumColumns = null;
            this.dgvParsedImportData.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dgvParsedImportData.SumRowCellFormat = "N2";
            this.dgvParsedImportData.TabIndex = 0;
            this.dgvParsedImportData.UseBatchEditColumn = false;
            this.dgvParsedImportData.UseCustomColumnDisplay = true;
            this.dgvParsedImportData.UseSelectedColumn = false;
            this.dgvParsedImportData.Use是否使用内置右键功能 = true;
            this.dgvParsedImportData.VirtualModeThreshold = 5000;
            this.dgvParsedImportData.XmlFileName = "";
            // 
            // kryptonPageFinalPreview
            // 
            this.kryptonPageFinalPreview.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageFinalPreview.Controls.Add(this.dgvFinalPreview);
            this.kryptonPageFinalPreview.Flags = 65534;
            this.kryptonPageFinalPreview.LastVisibleSet = true;
            this.kryptonPageFinalPreview.MinimumSize = new System.Drawing.Size(150, 50);
            this.kryptonPageFinalPreview.Name = "kryptonPageFinalPreview";
            this.kryptonPageFinalPreview.Size = new System.Drawing.Size(1079, 366);
            this.kryptonPageFinalPreview.Text = "最终预览";
            this.kryptonPageFinalPreview.TextTitle = "最终数据";
            this.kryptonPageFinalPreview.ToolTipTitle = "Page ToolTip";
            this.kryptonPageFinalPreview.UniqueName = "最终数据";
            // 
            // dgvFinalPreview
            // 
            this.dgvFinalPreview.AllowUserToAddRows = false;
            this.dgvFinalPreview.AllowUserToDeleteRows = false;
            this.dgvFinalPreview.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige;
            this.dgvFinalPreview.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvFinalPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFinalPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFinalPreview.CustomRowNo = false;
            this.dgvFinalPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFinalPreview.EnableFiltering = false;
            this.dgvFinalPreview.EnablePagination = false;
            this.dgvFinalPreview.EnableVirtualMode = true;
            this.dgvFinalPreview.IsShowSumRow = false;
            this.dgvFinalPreview.Location = new System.Drawing.Point(0, 0);
            this.dgvFinalPreview.Name = "dgvFinalPreview";
            this.dgvFinalPreview.NeedSaveColumnsXml = true;
            this.dgvFinalPreview.Size = new System.Drawing.Size(1079, 366);
            this.dgvFinalPreview.SumColumns = null;
            this.dgvFinalPreview.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dgvFinalPreview.SumRowCellFormat = "N2";
            this.dgvFinalPreview.TabIndex = 0;
            this.dgvFinalPreview.UseBatchEditColumn = false;
            this.dgvFinalPreview.UseCustomColumnDisplay = true;
            this.dgvFinalPreview.UseSelectedColumn = true;
            this.dgvFinalPreview.Use是否使用内置右键功能 = true;
            this.dgvFinalPreview.VirtualModeThreshold = 5000;
            this.dgvFinalPreview.XmlFileName = "";
            // 
            // kryptonPanel5
            // 
            this.kryptonPanel5.Controls.Add(this.klblDynamicFilePath);
            this.kryptonPanel5.Controls.Add(this.ktxtDynamicFilePath);
            this.kryptonPanel5.Controls.Add(this.kbtnDynamicBrowse);
            this.kryptonPanel5.Controls.Add(this.klblDynamicEntityType);
            this.kryptonPanel5.Controls.Add(this.kcmbDynamicEntityType);
            this.kryptonPanel5.Controls.Add(this.klblDynamicSheetName);
            this.kryptonPanel5.Controls.Add(this.kcmbDynamicSheetName);
            this.kryptonPanel5.Controls.Add(this.klblDynamicMappingName);
            this.kryptonPanel5.Controls.Add(this.kcmbDynamicMappingName);
            this.kryptonPanel5.Controls.Add(this.kbtnDynamicParse);
            this.kryptonPanel5.Controls.Add(this.kbtnDynamicMap);
            this.kryptonPanel5.Controls.Add(this.kbtnDynamicImport);
            this.kryptonPanel5.Controls.Add(this.kbtnGeneratePreview);
            this.kryptonPanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanel5.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel5.Name = "kryptonPanel5";
            this.kryptonPanel5.Size = new System.Drawing.Size(1081, 115);
            this.kryptonPanel5.TabIndex = 0;
            // 
            // klblDynamicFilePath
            // 
            this.klblDynamicFilePath.Location = new System.Drawing.Point(35, 7);
            this.klblDynamicFilePath.Name = "klblDynamicFilePath";
            this.klblDynamicFilePath.Size = new System.Drawing.Size(65, 20);
            this.klblDynamicFilePath.TabIndex = 0;
            this.klblDynamicFilePath.Values.Text = "文件路径:";
            // 
            // ktxtDynamicFilePath
            // 
            this.ktxtDynamicFilePath.Location = new System.Drawing.Point(106, 7);
            this.ktxtDynamicFilePath.Name = "ktxtDynamicFilePath";
            this.ktxtDynamicFilePath.Size = new System.Drawing.Size(818, 23);
            this.ktxtDynamicFilePath.TabIndex = 1;
            // 
            // kbtnDynamicBrowse
            // 
            this.kbtnDynamicBrowse.Location = new System.Drawing.Point(948, 7);
            this.kbtnDynamicBrowse.Name = "kbtnDynamicBrowse";
            this.kbtnDynamicBrowse.Size = new System.Drawing.Size(80, 25);
            this.kbtnDynamicBrowse.TabIndex = 2;
            this.kbtnDynamicBrowse.Values.Text = "浏览";
            this.kbtnDynamicBrowse.Click += new System.EventHandler(this.KbtnDynamicBrowse_Click);
            // 
            // klblDynamicEntityType
            // 
            this.klblDynamicEntityType.Location = new System.Drawing.Point(9, 69);
            this.klblDynamicEntityType.Name = "klblDynamicEntityType";
            this.klblDynamicEntityType.Size = new System.Drawing.Size(91, 20);
            this.klblDynamicEntityType.TabIndex = 10;
            this.klblDynamicEntityType.Values.Text = "目标数据类型:";
            // 
            // kcmbDynamicEntityType
            // 
            this.kcmbDynamicEntityType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbDynamicEntityType.DropDownWidth = 200;
            this.kcmbDynamicEntityType.IntegralHeight = false;
            this.kcmbDynamicEntityType.Location = new System.Drawing.Point(106, 68);
            this.kcmbDynamicEntityType.Name = "kcmbDynamicEntityType";
            this.kcmbDynamicEntityType.Size = new System.Drawing.Size(279, 21);
            this.kcmbDynamicEntityType.TabIndex = 11;
            // 
            // klblDynamicSheetName
            // 
            this.klblDynamicSheetName.Location = new System.Drawing.Point(13, 42);
            this.klblDynamicSheetName.Name = "klblDynamicSheetName";
            this.klblDynamicSheetName.Size = new System.Drawing.Size(87, 20);
            this.klblDynamicSheetName.TabIndex = 3;
            this.klblDynamicSheetName.Values.Text = "Excel Sheet表:";
            // 
            // kcmbDynamicSheetName
            // 
            this.kcmbDynamicSheetName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbDynamicSheetName.DropDownWidth = 200;
            this.kcmbDynamicSheetName.IntegralHeight = false;
            this.kcmbDynamicSheetName.Location = new System.Drawing.Point(106, 41);
            this.kcmbDynamicSheetName.Name = "kcmbDynamicSheetName";
            this.kcmbDynamicSheetName.Size = new System.Drawing.Size(279, 21);
            this.kcmbDynamicSheetName.TabIndex = 4;
            // 
            // klblDynamicMappingName
            // 
            this.klblDynamicMappingName.Location = new System.Drawing.Point(436, 41);
            this.klblDynamicMappingName.Name = "klblDynamicMappingName";
            this.klblDynamicMappingName.Size = new System.Drawing.Size(65, 20);
            this.klblDynamicMappingName.TabIndex = 5;
            this.klblDynamicMappingName.Values.Text = "解析配置:";
            // 
            // kcmbDynamicMappingName
            // 
            this.kcmbDynamicMappingName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbDynamicMappingName.DropDownWidth = 170;
            this.kcmbDynamicMappingName.IntegralHeight = false;
            this.kcmbDynamicMappingName.Location = new System.Drawing.Point(507, 40);
            this.kcmbDynamicMappingName.Name = "kcmbDynamicMappingName";
            this.kcmbDynamicMappingName.Size = new System.Drawing.Size(417, 21);
            this.kcmbDynamicMappingName.TabIndex = 6;
            // 
            // kbtnDynamicParse
            // 
            this.kbtnDynamicParse.Enabled = false;
            this.kbtnDynamicParse.Location = new System.Drawing.Point(675, 72);
            this.kbtnDynamicParse.Name = "kbtnDynamicParse";
            this.kbtnDynamicParse.Size = new System.Drawing.Size(99, 25);
            this.kbtnDynamicParse.TabIndex = 7;
            this.kbtnDynamicParse.Values.Text = "按配置解析数据";
            this.kbtnDynamicParse.Click += new System.EventHandler(this.KbtnDynamicParse_Click);
            // 
            // kbtnDynamicMap
            // 
            this.kbtnDynamicMap.Enabled = false;
            this.kbtnDynamicMap.Location = new System.Drawing.Point(948, 42);
            this.kbtnDynamicMap.Name = "kbtnDynamicMap";
            this.kbtnDynamicMap.Size = new System.Drawing.Size(80, 25);
            this.kbtnDynamicMap.TabIndex = 8;
            this.kbtnDynamicMap.Values.Text = "解析配置";
            this.kbtnDynamicMap.Click += new System.EventHandler(this.KbtnDynamicMap_Click);
            // 
            // kbtnDynamicImport
            // 
            this.kbtnDynamicImport.Enabled = false;
            this.kbtnDynamicImport.Location = new System.Drawing.Point(948, 73);
            this.kbtnDynamicImport.Name = "kbtnDynamicImport";
            this.kbtnDynamicImport.Size = new System.Drawing.Size(80, 25);
            this.kbtnDynamicImport.TabIndex = 9;
            this.kbtnDynamicImport.Values.Text = "导入";
            this.kbtnDynamicImport.Click += new System.EventHandler(this.kbtnDynamicImport_Click);
            // 
            // kbtnGeneratePreview
            // 
            this.kbtnGeneratePreview.Enabled = false;
            this.kbtnGeneratePreview.Location = new System.Drawing.Point(804, 73);
            this.kbtnGeneratePreview.Name = "kbtnGeneratePreview";
            this.kbtnGeneratePreview.Size = new System.Drawing.Size(120, 25);
            this.kbtnGeneratePreview.TabIndex = 10;
            this.kbtnGeneratePreview.Values.Text = "生成结果预览";
            this.kbtnGeneratePreview.Click += new System.EventHandler(this.kbtnGeneratePreview_Click);
            // 
            // UCBasicDataImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonNavigatorMain);
            this.Name = "UCBasicDataImport";
            this.Size = new System.Drawing.Size(1087, 597);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorMain)).EndInit();
            this.kryptonNavigatorMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageDynamicImport)).EndInit();
            this.kryptonPageDynamicImport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxDynamicImport.Panel)).EndInit();
            this.kryptonGroupBoxDynamicImport.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxDynamicImport)).EndInit();
            this.kryptonGroupBoxDynamicImport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel4)).EndInit();
            this.kryptonPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorDynamic)).EndInit();
            this.kryptonNavigatorDynamic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageRawData)).EndInit();
            this.kryptonPageRawData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRawExcelData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageParsedData)).EndInit();
            this.kryptonPageParsedData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvParsedImportData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageFinalPreview)).EndInit();
            this.kryptonPageFinalPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinalPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel5)).EndInit();
            this.kryptonPanel5.ResumeLayout(false);
            this.kryptonPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicEntityType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicSheetName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicMappingName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private RUINORERP.UI.UControls.NewSumDataGridView dgvRawExcelData;
        private RUINORERP.UI.UControls.NewSumDataGridView dgvParsedImportData;
        private Krypton.Navigator.KryptonPage kryptonPageFinalPreview;
        private RUINORERP.UI.UControls.NewSumDataGridView dgvFinalPreview;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAll;
        private System.Windows.Forms.ToolStripMenuItem selectNoAll;
        private Krypton.Navigator.KryptonPage kryptonPageDynamicImport;
        private Krypton.Toolkit.KryptonPanel kryptonPanel4;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxDynamicImport;
        private Krypton.Toolkit.KryptonPanel kryptonPanel5;
        private Krypton.Toolkit.KryptonButton kbtnDynamicBrowse;
        private Krypton.Toolkit.KryptonButton kbtnDynamicParse;
        private Krypton.Toolkit.KryptonButton kbtnDynamicMap;
        private Krypton.Toolkit.KryptonButton kbtnDynamicImport;
        private Krypton.Toolkit.KryptonButton kbtnGeneratePreview;
        private Krypton.Toolkit.KryptonLabel klblDynamicFilePath;
        private Krypton.Toolkit.KryptonTextBox ktxtDynamicFilePath;
        private Krypton.Toolkit.KryptonLabel klblDynamicSheetName;
        private Krypton.Toolkit.KryptonComboBox kcmbDynamicSheetName;
        private Krypton.Toolkit.KryptonLabel klblDynamicMappingName;
        private Krypton.Toolkit.KryptonComboBox kcmbDynamicMappingName;
        private Krypton.Navigator.KryptonNavigator kryptonNavigatorMain;
        private Krypton.Navigator.KryptonNavigator kryptonNavigatorDynamic;
        private Krypton.Navigator.KryptonPage kryptonPageRawData;
        private Krypton.Navigator.KryptonPage kryptonPageParsedData;
        private Krypton.Toolkit.KryptonLabel klblDynamicEntityType;
        private Krypton.Toolkit.KryptonComboBox kcmbDynamicEntityType;
    }
}
