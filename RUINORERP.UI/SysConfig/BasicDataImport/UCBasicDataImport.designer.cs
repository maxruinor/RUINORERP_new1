namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    partial class UCBasicDataImport
    {
        /// <summary>
        /// 必需的设计器变量
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false</param>
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
        /// 使用代码编辑器修改此方法的内容
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvImportData = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoAll = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonPanel2 = new Krypton.Toolkit.KryptonPanel();
            this.dgvRawExcelData = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonNavigatorMain = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPageDynamicImport = new Krypton.Navigator.KryptonPage();
            this.kryptonGroupBoxDynamicImport = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonPanel4 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonNavigatorDynamic = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPageRawData = new Krypton.Navigator.KryptonPage();
            this.kryptonPageParsedData = new Krypton.Navigator.KryptonPage();
            this.dgvParsedImportData = new RUINORERP.UI.UControls.NewSumDataGridView();
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
            this.dgvDynamicImportData = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonPanel6 = new Krypton.Toolkit.KryptonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImportData)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRawExcelData)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageParsedData)).BeginInit();
            this.kryptonPageParsedData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParsedImportData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel5)).BeginInit();
            this.kryptonPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicEntityType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicSheetName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicMappingName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDynamicImportData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel6)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvImportData
            // 
            this.dgvImportData.AllowUserToAddRows = false;
            this.dgvImportData.AllowUserToDeleteRows = false;
            this.dgvImportData.AllowUserToOrderColumns = true;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Beige;
            this.dgvImportData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvImportData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvImportData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvImportData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvImportData.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvImportData.CustomRowNo = false;
            this.dgvImportData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvImportData.EnableFiltering = false;
            this.dgvImportData.EnablePagination = false;
            this.dgvImportData.IsShowSumRow = false;
            this.dgvImportData.Location = new System.Drawing.Point(0, 0);
            this.dgvImportData.Name = "dgvImportData";
            this.dgvImportData.NeedSaveColumnsXml = true;
            this.dgvImportData.RowTemplate.Height = 23;
            this.dgvImportData.Size = new System.Drawing.Size(863, 394);
            this.dgvImportData.SumColumns = null;
            this.dgvImportData.SummaryDescription = "数据预览";
            this.dgvImportData.SumRowCellFormat = "N2";
            this.dgvImportData.TabIndex = 0;
            this.dgvImportData.UseBatchEditColumn = false;
            this.dgvImportData.UseCustomColumnDisplay = true;
            this.dgvImportData.UseSelectedColumn = false;
            this.dgvImportData.Use是否使用内置右键功能 = true;
            this.dgvImportData.XmlFileName = "UCBasicDataImport";
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
            // kryptonPanel2
            // 
            this.kryptonPanel2.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(100, 100);
            this.kryptonPanel2.TabIndex = 0;
            // 
            // dgvRawExcelData
            // 
            this.dgvRawExcelData.AllowUserToAddRows = false;
            this.dgvRawExcelData.AllowUserToDeleteRows = false;
            this.dgvRawExcelData.AllowUserToOrderColumns = true;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Beige;
            this.dgvRawExcelData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvRawExcelData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRawExcelData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRawExcelData.CustomRowNo = false;
            this.dgvRawExcelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRawExcelData.EnableFiltering = false;
            this.dgvRawExcelData.EnablePagination = false;
            this.dgvRawExcelData.IsShowSumRow = false;
            this.dgvRawExcelData.Location = new System.Drawing.Point(0, 0);
            this.dgvRawExcelData.Name = "dgvRawExcelData";
            this.dgvRawExcelData.NeedSaveColumnsXml = true;
            this.dgvRawExcelData.Size = new System.Drawing.Size(867, 341);
            this.dgvRawExcelData.SumColumns = null;
            this.dgvRawExcelData.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dgvRawExcelData.SumRowCellFormat = "N2";
            this.dgvRawExcelData.TabIndex = 0;
            this.dgvRawExcelData.UseBatchEditColumn = false;
            this.dgvRawExcelData.UseCustomColumnDisplay = true;
            this.dgvRawExcelData.UseSelectedColumn = false;
            this.dgvRawExcelData.Use是否使用内置右键功能 = true;
            this.dgvRawExcelData.XmlFileName = "";
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
            this.kryptonNavigatorMain.Size = new System.Drawing.Size(875, 535);
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
            this.kryptonPageDynamicImport.Size = new System.Drawing.Size(873, 504);
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
            this.kryptonGroupBoxDynamicImport.Size = new System.Drawing.Size(873, 504);
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
            this.kryptonPanel4.Size = new System.Drawing.Size(869, 480);
            this.kryptonPanel4.TabIndex = 0;
            // 
            // kryptonNavigatorDynamic
            // 
            this.kryptonNavigatorDynamic.ControlKryptonFormFeatures = false;
            this.kryptonNavigatorDynamic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonNavigatorDynamic.Location = new System.Drawing.Point(0, 112);
            this.kryptonNavigatorDynamic.Name = "kryptonNavigatorDynamic";
            this.kryptonNavigatorDynamic.NavigatorMode = Krypton.Navigator.NavigatorMode.BarTabGroup;
            this.kryptonNavigatorDynamic.Owner = null;
            this.kryptonNavigatorDynamic.PageBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonNavigatorDynamic.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageRawData,
            this.kryptonPageParsedData});
            this.kryptonNavigatorDynamic.SelectedIndex = 1;
            this.kryptonNavigatorDynamic.Size = new System.Drawing.Size(869, 368);
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
            this.kryptonPageRawData.Size = new System.Drawing.Size(867, 341);
            this.kryptonPageRawData.Text = "预览数据";
            this.kryptonPageRawData.TextTitle = "原始数据";
            this.kryptonPageRawData.ToolTipTitle = "Page ToolTip";
            this.kryptonPageRawData.UniqueName = "原始数据";
            // 
            // kryptonPageParsedData
            // 
            this.kryptonPageParsedData.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageParsedData.Controls.Add(this.dgvParsedImportData);
            this.kryptonPageParsedData.Flags = 65534;
            this.kryptonPageParsedData.LastVisibleSet = true;
            this.kryptonPageParsedData.MinimumSize = new System.Drawing.Size(150, 50);
            this.kryptonPageParsedData.Name = "kryptonPageParsedData";
            this.kryptonPageParsedData.Size = new System.Drawing.Size(867, 341);
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
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Beige;
            this.dgvParsedImportData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvParsedImportData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvParsedImportData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvParsedImportData.CustomRowNo = false;
            this.dgvParsedImportData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvParsedImportData.EnableFiltering = false;
            this.dgvParsedImportData.EnablePagination = false;
            this.dgvParsedImportData.IsShowSumRow = false;
            this.dgvParsedImportData.Location = new System.Drawing.Point(0, 0);
            this.dgvParsedImportData.Name = "dgvParsedImportData";
            this.dgvParsedImportData.NeedSaveColumnsXml = true;
            this.dgvParsedImportData.Size = new System.Drawing.Size(867, 341);
            this.dgvParsedImportData.SumColumns = null;
            this.dgvParsedImportData.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dgvParsedImportData.SumRowCellFormat = "N2";
            this.dgvParsedImportData.TabIndex = 0;
            this.dgvParsedImportData.UseBatchEditColumn = false;
            this.dgvParsedImportData.UseCustomColumnDisplay = true;
            this.dgvParsedImportData.UseSelectedColumn = false;
            this.dgvParsedImportData.Use是否使用内置右键功能 = true;
            this.dgvParsedImportData.XmlFileName = "";
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
            this.kryptonPanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanel5.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel5.Name = "kryptonPanel5";
            this.kryptonPanel5.Size = new System.Drawing.Size(869, 112);
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
            this.ktxtDynamicFilePath.Size = new System.Drawing.Size(450, 23);
            this.ktxtDynamicFilePath.TabIndex = 1;
            // 
            // kbtnDynamicBrowse
            // 
            this.kbtnDynamicBrowse.Location = new System.Drawing.Point(580, 7);
            this.kbtnDynamicBrowse.Name = "kbtnDynamicBrowse";
            this.kbtnDynamicBrowse.Size = new System.Drawing.Size(80, 25);
            this.kbtnDynamicBrowse.TabIndex = 2;
            this.kbtnDynamicBrowse.Values.Text = "浏览";
            // 
            // klblDynamicEntityType
            // 
            this.klblDynamicEntityType.Location = new System.Drawing.Point(9, 38);
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
            this.kcmbDynamicEntityType.Location = new System.Drawing.Point(106, 37);
            this.kcmbDynamicEntityType.Name = "kcmbDynamicEntityType";
            this.kcmbDynamicEntityType.Size = new System.Drawing.Size(200, 21);
            this.kcmbDynamicEntityType.TabIndex = 11;
            // 
            // klblDynamicSheetName
            // 
            this.klblDynamicSheetName.Location = new System.Drawing.Point(13, 68);
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
            this.kcmbDynamicSheetName.Location = new System.Drawing.Point(106, 67);
            this.kcmbDynamicSheetName.Name = "kcmbDynamicSheetName";
            this.kcmbDynamicSheetName.Size = new System.Drawing.Size(200, 21);
            this.kcmbDynamicSheetName.TabIndex = 4;
            // 
            // klblDynamicMappingName
            // 
            this.klblDynamicMappingName.Location = new System.Drawing.Point(385, 69);
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
            this.kcmbDynamicMappingName.Location = new System.Drawing.Point(456, 68);
            this.kcmbDynamicMappingName.Name = "kcmbDynamicMappingName";
            this.kcmbDynamicMappingName.Size = new System.Drawing.Size(204, 21);
            this.kcmbDynamicMappingName.TabIndex = 6;
            // 
            // kbtnDynamicParse
            // 
            this.kbtnDynamicParse.Enabled = false;
            this.kbtnDynamicParse.Location = new System.Drawing.Point(678, 63);
            this.kbtnDynamicParse.Name = "kbtnDynamicParse";
            this.kbtnDynamicParse.Size = new System.Drawing.Size(80, 25);
            this.kbtnDynamicParse.TabIndex = 7;
            this.kbtnDynamicParse.Values.Text = "解析";
            // 
            // kbtnDynamicMap
            // 
            this.kbtnDynamicMap.Enabled = false;
            this.kbtnDynamicMap.Location = new System.Drawing.Point(322, 36);
            this.kbtnDynamicMap.Name = "kbtnDynamicMap";
            this.kbtnDynamicMap.Size = new System.Drawing.Size(80, 25);
            this.kbtnDynamicMap.TabIndex = 8;
            this.kbtnDynamicMap.Values.Text = "解析配置";
            this.kbtnDynamicMap.Click += new System.EventHandler(this.KbtnDynamicMap_Click);
            // 
            // kbtnDynamicImport
            // 
            this.kbtnDynamicImport.Enabled = false;
            this.kbtnDynamicImport.Location = new System.Drawing.Point(768, 63);
            this.kbtnDynamicImport.Name = "kbtnDynamicImport";
            this.kbtnDynamicImport.Size = new System.Drawing.Size(80, 25);
            this.kbtnDynamicImport.TabIndex = 9;
            this.kbtnDynamicImport.Values.Text = "导入";
            this.kbtnDynamicImport.Click += new System.EventHandler(this.kbtnDynamicImport_Click);
            // 
            // dgvDynamicImportData
            // 
            this.dgvDynamicImportData.AllowUserToAddRows = false;
            this.dgvDynamicImportData.AllowUserToDeleteRows = false;
            this.dgvDynamicImportData.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.dgvDynamicImportData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDynamicImportData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDynamicImportData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDynamicImportData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDynamicImportData.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvDynamicImportData.CustomRowNo = false;
            this.dgvDynamicImportData.EnableFiltering = false;
            this.dgvDynamicImportData.EnablePagination = false;
            this.dgvDynamicImportData.IsShowSumRow = false;
            this.dgvDynamicImportData.Location = new System.Drawing.Point(10, 132);
            this.dgvDynamicImportData.Name = "dgvDynamicImportData";
            this.dgvDynamicImportData.NeedSaveColumnsXml = true;
            this.dgvDynamicImportData.RowTemplate.Height = 23;
            this.dgvDynamicImportData.Size = new System.Drawing.Size(774, 307);
            this.dgvDynamicImportData.SumColumns = null;
            this.dgvDynamicImportData.SummaryDescription = "动态导入数据预览";
            this.dgvDynamicImportData.SumRowCellFormat = "N2";
            this.dgvDynamicImportData.TabIndex = 10;
            this.dgvDynamicImportData.UseBatchEditColumn = false;
            this.dgvDynamicImportData.UseCustomColumnDisplay = true;
            this.dgvDynamicImportData.UseSelectedColumn = false;
            this.dgvDynamicImportData.Use是否使用内置右键功能 = true;
            this.dgvDynamicImportData.XmlFileName = "UCBasicDataImport_Dynamic";
            // 
            // kryptonPanel6
            // 
            this.kryptonPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel6.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel6.Name = "kryptonPanel6";
            this.kryptonPanel6.Size = new System.Drawing.Size(794, 445);
            this.kryptonPanel6.TabIndex = 0;
            // 
            // UCBasicDataImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonNavigatorMain);
            this.Name = "UCBasicDataImport";
            this.Size = new System.Drawing.Size(875, 535);
            ((System.ComponentModel.ISupportInitialize)(this.dgvImportData)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRawExcelData)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageParsedData)).EndInit();
            this.kryptonPageParsedData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvParsedImportData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel5)).EndInit();
            this.kryptonPanel5.ResumeLayout(false);
            this.kryptonPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicEntityType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicSheetName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDynamicMappingName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDynamicImportData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private RUINORERP.UI.UControls.NewSumDataGridView dgvImportData;
        private RUINORERP.UI.UControls.NewSumDataGridView dgvRawExcelData;
        private RUINORERP.UI.UControls.NewSumDataGridView dgvParsedImportData;
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
        private Krypton.Toolkit.KryptonLabel klblDynamicFilePath;
        private Krypton.Toolkit.KryptonTextBox ktxtDynamicFilePath;
        private Krypton.Toolkit.KryptonLabel klblDynamicSheetName;
        private Krypton.Toolkit.KryptonComboBox kcmbDynamicSheetName;
        private Krypton.Toolkit.KryptonLabel klblDynamicMappingName;
        private Krypton.Toolkit.KryptonComboBox kcmbDynamicMappingName;
        private Krypton.Toolkit.KryptonButton kbtnSaveMapping;
        private Krypton.Toolkit.KryptonButton kbtnLoadMapping;
        private Krypton.Toolkit.KryptonButton kbtnDeleteMapping;
        private RUINORERP.UI.UControls.NewSumDataGridView dgvDynamicImportData;
        private Krypton.Navigator.KryptonNavigator kryptonNavigatorMain;
        private Krypton.Navigator.KryptonNavigator kryptonNavigatorDynamic;
        private Krypton.Navigator.KryptonPage kryptonPageRawData;
        private Krypton.Navigator.KryptonPage kryptonPageParsedData;
        private Krypton.Toolkit.KryptonPanel kryptonPanel6;
        private Krypton.Toolkit.KryptonLabel klblDynamicEntityType;
        private Krypton.Toolkit.KryptonComboBox kcmbDynamicEntityType;
    }
}