namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    partial class FrmWideTableProfileEditor
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

        private void InitializeComponent()
        {
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonNavigatorProfile = new Krypton.Navigator.KryptonNavigator();
            this.kryptonPageBasicInfo = new Krypton.Navigator.KryptonPage();
            this.ktxtDescription = new Krypton.Toolkit.KryptonTextBox();
            this.klblDescription = new Krypton.Toolkit.KryptonLabel();
            this.ktxtProfileName = new Krypton.Toolkit.KryptonTextBox();
            this.klblProfileName = new Krypton.Toolkit.KryptonLabel();
            this.kryptonPageMasterTable = new Krypton.Navigator.KryptonPage();
            this.kbtnDeleteColumn = new Krypton.Toolkit.KryptonButton();
            this.kbtnEditColumn = new Krypton.Toolkit.KryptonButton();
            this.kbtnAddColumn = new Krypton.Toolkit.KryptonButton();
            this.dgvMasterColumns = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.ktxtBusinessKeys = new Krypton.Toolkit.KryptonTextBox();
            this.klblBusinessKeys = new Krypton.Toolkit.KryptonLabel();
            this.kcmbMasterTable = new Krypton.Toolkit.KryptonComboBox();
            this.klblMasterTable = new Krypton.Toolkit.KryptonLabel();
            this.kryptonPageDependencyTables = new Krypton.Navigator.KryptonPage();
            this.kbtnAddDependency = new Krypton.Toolkit.KryptonButton();
            this.dgvDependencyTables = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonPageChildTables = new Krypton.Navigator.KryptonPage();
            this.kbtnAddChild = new Krypton.Toolkit.KryptonButton();
            this.dgvChildTables = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.kryptonPanel2 = new Krypton.Toolkit.KryptonPanel();
            this.kbtnTest = new Krypton.Toolkit.KryptonButton();
            this.kbtnSave = new Krypton.Toolkit.KryptonButton();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorProfile)).BeginInit();
            this.kryptonNavigatorProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageBasicInfo)).BeginInit();
            this.kryptonPageBasicInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMasterTable)).BeginInit();
            this.kryptonPageMasterTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbMasterTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageDependencyTables)).BeginInit();
            this.kryptonPageDependencyTables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDependencyTables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageChildTables)).BeginInit();
            this.kryptonPageChildTables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChildTables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonNavigatorProfile);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(900, 600);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kryptonNavigatorProfile
            // 
            this.kryptonNavigatorProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonNavigatorProfile.Location = new System.Drawing.Point(0, 0);
            this.kryptonNavigatorProfile.Name = "kryptonNavigatorProfile";
            this.kryptonNavigatorProfile.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageBasicInfo,
            this.kryptonPageMasterTable,
            this.kryptonPageDependencyTables,
            this.kryptonPageChildTables});
            this.kryptonNavigatorProfile.SelectedIndex = 0;
            this.kryptonNavigatorProfile.Size = new System.Drawing.Size(900, 540);
            this.kryptonNavigatorProfile.TabIndex = 0;
            // 
            // kryptonPageBasicInfo
            // 
            this.kryptonPageBasicInfo.Controls.Add(this.ktxtDescription);
            this.kryptonPageBasicInfo.Controls.Add(this.klblDescription);
            this.kryptonPageBasicInfo.Controls.Add(this.ktxtProfileName);
            this.kryptonPageBasicInfo.Controls.Add(this.klblProfileName);
            this.kryptonPageBasicInfo.Flags = 65534;
            this.kryptonPageBasicInfo.LastVisibleSet = true;
            this.kryptonPageBasicInfo.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageBasicInfo.Name = "kryptonPageBasicInfo";
            this.kryptonPageBasicInfo.Size = new System.Drawing.Size(898, 509);
            this.kryptonPageBasicInfo.Text = "基本信息";
            // 
            // ktxtDescription
            // 
            this.ktxtDescription.Location = new System.Drawing.Point(100, 80);
            this.ktxtDescription.Multiline = true;
            this.ktxtDescription.Name = "ktxtDescription";
            this.ktxtDescription.Size = new System.Drawing.Size(700, 100);
            this.ktxtDescription.TabIndex = 3;
            // 
            // klblDescription
            // 
            this.klblDescription.Location = new System.Drawing.Point(20, 82);
            this.klblDescription.Name = "klblDescription";
            this.klblDescription.Size = new System.Drawing.Size(60, 22);
            this.klblDescription.TabIndex = 2;
            this.klblDescription.Values.Text = "描述:";
            // 
            // ktxtProfileName
            // 
            this.ktxtProfileName.Location = new System.Drawing.Point(100, 40);
            this.ktxtProfileName.Name = "ktxtProfileName";
            this.ktxtProfileName.Size = new System.Drawing.Size(400, 25);
            this.ktxtProfileName.TabIndex = 1;
            // 
            // klblProfileName
            // 
            this.klblProfileName.Location = new System.Drawing.Point(20, 42);
            this.klblProfileName.Name = "klblProfileName";
            this.klblProfileName.Size = new System.Drawing.Size(70, 22);
            this.klblProfileName.TabIndex = 0;
            this.klblProfileName.Values.Text = "Profile名称:";
            // 
            // kryptonPageMasterTable
            // 
            this.kryptonPageMasterTable.Controls.Add(this.kbtnDeleteColumn);
            this.kryptonPageMasterTable.Controls.Add(this.kbtnEditColumn);
            this.kryptonPageMasterTable.Controls.Add(this.kbtnAddColumn);
            this.kryptonPageMasterTable.Controls.Add(this.dgvMasterColumns);
            this.kryptonPageMasterTable.Controls.Add(this.ktxtBusinessKeys);
            this.kryptonPageMasterTable.Controls.Add(this.klblBusinessKeys);
            this.kryptonPageMasterTable.Controls.Add(this.kcmbMasterTable);
            this.kryptonPageMasterTable.Controls.Add(this.klblMasterTable);
            this.kryptonPageMasterTable.Flags = 65534;
            this.kryptonPageMasterTable.LastVisibleSet = true;
            this.kryptonPageMasterTable.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMasterTable.Name = "kryptonPageMasterTable";
            this.kryptonPageMasterTable.Size = new System.Drawing.Size(898, 509);
            this.kryptonPageMasterTable.Text = "主表配置";
            // 
            // kbtnDeleteColumn
            // 
            this.kbtnDeleteColumn.Location = new System.Drawing.Point(800, 450);
            this.kbtnDeleteColumn.Name = "kbtnDeleteColumn";
            this.kbtnDeleteColumn.Size = new System.Drawing.Size(80, 30);
            this.kbtnDeleteColumn.TabIndex = 7;
            this.kbtnDeleteColumn.Values.Text = "删除列";
            this.kbtnDeleteColumn.Click += new System.EventHandler(this.kbtnDeleteColumn_Click);
            // 
            // kbtnEditColumn
            // 
            this.kbtnEditColumn.Location = new System.Drawing.Point(700, 450);
            this.kbtnEditColumn.Name = "kbtnEditColumn";
            this.kbtnEditColumn.Size = new System.Drawing.Size(80, 30);
            this.kbtnEditColumn.TabIndex = 6;
            this.kbtnEditColumn.Values.Text = "编辑列";
            this.kbtnEditColumn.Click += new System.EventHandler(this.kbtnEditColumn_Click);
            // 
            // kbtnAddColumn
            // 
            this.kbtnAddColumn.Location = new System.Drawing.Point(600, 450);
            this.kbtnAddColumn.Name = "kbtnAddColumn";
            this.kbtnAddColumn.Size = new System.Drawing.Size(80, 30);
            this.kbtnAddColumn.TabIndex = 5;
            this.kbtnAddColumn.Values.Text = "添加列";
            this.kbtnAddColumn.Click += new System.EventHandler(this.kbtnAddColumn_Click);
            // 
            // dgvMasterColumns
            // 
            this.dgvMasterColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMasterColumns.Location = new System.Drawing.Point(20, 120);
            this.dgvMasterColumns.Name = "dgvMasterColumns";
            this.dgvMasterColumns.Size = new System.Drawing.Size(860, 320);
            this.dgvMasterColumns.TabIndex = 4;
            // 
            // ktxtBusinessKeys
            // 
            this.ktxtBusinessKeys.Location = new System.Drawing.Point(100, 80);
            this.ktxtBusinessKeys.Name = "ktxtBusinessKeys";
            this.ktxtBusinessKeys.Size = new System.Drawing.Size(400, 25);
            this.ktxtBusinessKeys.TabIndex = 3;
            // 
            // klblBusinessKeys
            // 
            this.klblBusinessKeys.Location = new System.Drawing.Point(20, 82);
            this.klblBusinessKeys.Name = "klblBusinessKeys";
            this.klblBusinessKeys.Size = new System.Drawing.Size(70, 22);
            this.klblBusinessKeys.TabIndex = 2;
            this.klblBusinessKeys.Values.Text = "业务键:";
            // 
            // kcmbMasterTable
            // 
            this.kcmbMasterTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbMasterTable.Location = new System.Drawing.Point(100, 40);
            this.kcmbMasterTable.Name = "kcmbMasterTable";
            this.kcmbMasterTable.Size = new System.Drawing.Size(300, 25);
            this.kcmbMasterTable.TabIndex = 1;
            // 
            // klblMasterTable
            // 
            this.klblMasterTable.Location = new System.Drawing.Point(20, 42);
            this.klblMasterTable.Name = "klblMasterTable";
            this.klblMasterTable.Size = new System.Drawing.Size(70, 22);
            this.klblMasterTable.TabIndex = 0;
            this.klblMasterTable.Values.Text = "目标表:";
            // 
            // kryptonPageDependencyTables
            // 
            this.kryptonPageDependencyTables.Controls.Add(this.kbtnAddDependency);
            this.kryptonPageDependencyTables.Controls.Add(this.dgvDependencyTables);
            this.kryptonPageDependencyTables.Flags = 65534;
            this.kryptonPageDependencyTables.LastVisibleSet = true;
            this.kryptonPageDependencyTables.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageDependencyTables.Name = "kryptonPageDependencyTables";
            this.kryptonPageDependencyTables.Size = new System.Drawing.Size(898, 509);
            this.kryptonPageDependencyTables.Text = "依赖表配置";
            // 
            // kbtnAddDependency
            // 
            this.kbtnAddDependency.Location = new System.Drawing.Point(20, 450);
            this.kbtnAddDependency.Name = "kbtnAddDependency";
            this.kbtnAddDependency.Size = new System.Drawing.Size(100, 30);
            this.kbtnAddDependency.TabIndex = 1;
            this.kbtnAddDependency.Values.Text = "添加依赖表";
            this.kbtnAddDependency.Click += new System.EventHandler(this.kbtnAddDependency_Click);
            // 
            // dgvDependencyTables
            // 
            this.dgvDependencyTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDependencyTables.Location = new System.Drawing.Point(20, 20);
            this.dgvDependencyTables.Name = "dgvDependencyTables";
            this.dgvDependencyTables.Size = new System.Drawing.Size(860, 420);
            this.dgvDependencyTables.TabIndex = 0;
            // 
            // kryptonPageChildTables
            // 
            this.kryptonPageChildTables.Controls.Add(this.kbtnAddChild);
            this.kryptonPageChildTables.Controls.Add(this.dgvChildTables);
            this.kryptonPageChildTables.Flags = 65534;
            this.kryptonPageChildTables.LastVisibleSet = true;
            this.kryptonPageChildTables.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageChildTables.Name = "kryptonPageChildTables";
            this.kryptonPageChildTables.Size = new System.Drawing.Size(898, 509);
            this.kryptonPageChildTables.Text = "子表配置";
            // 
            // kbtnAddChild
            // 
            this.kbtnAddChild.Location = new System.Drawing.Point(20, 450);
            this.kbtnAddChild.Name = "kbtnAddChild";
            this.kbtnAddChild.Size = new System.Drawing.Size(100, 30);
            this.kbtnAddChild.TabIndex = 1;
            this.kbtnAddChild.Values.Text = "添加子表";
            this.kbtnAddChild.Click += new System.EventHandler(this.kbtnAddChild_Click);
            // 
            // dgvChildTables
            // 
            this.dgvChildTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonPageChildTables.Controls.Add(this.dgvChildTables);
            this.dgvChildTables.Location = new System.Drawing.Point(20, 20);
            this.dgvChildTables.Name = "dgvChildTables";
            this.dgvChildTables.Size = new System.Drawing.Size(860, 420);
            this.dgvChildTables.TabIndex = 0;
            // 
            // kryptonPanel2
            // 
            this.kryptonPanel2.Controls.Add(this.kbtnAutoFill);
            this.kryptonPanel2.Controls.Add(this.kbtnTest);
            this.kryptonPanel2.Controls.Add(this.kbtnSave);
            this.kryptonPanel2.Controls.Add(this.kbtnCancel);
            this.kryptonPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanel2.Location = new System.Drawing.Point(0, 540);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(900, 60);
            this.kryptonPanel2.TabIndex = 1;
            // 
            // kbtnAutoFill
            // 
            this.kbtnAutoFill.Location = new System.Drawing.Point(500, 15);
            this.kbtnAutoFill.Name = "kbtnAutoFill";
            this.kbtnAutoFill.Size = new System.Drawing.Size(80, 30);
            this.kbtnAutoFill.TabIndex = 3;
            this.kbtnAutoFill.Values.Text = "自动填充";
            this.kbtnAutoFill.Click += new System.EventHandler(this.kbtnAutoFill_Click);
            // 
            // kbtnTest
            // 
            this.kbtnTest.Location = new System.Drawing.Point(600, 15);
            this.kbtnTest.Name = "kbtnTest";
            this.kbtnTest.Size = new System.Drawing.Size(80, 30);
            this.kbtnTest.TabIndex = 2;
            this.kbtnTest.Values.Text = "测试配置";
            this.kbtnTest.Click += new System.EventHandler(this.kbtnTest_Click);
            // 
            // kbtnSave
            // 
            this.kbtnSave.Location = new System.Drawing.Point(700, 15);
            this.kbtnSave.Name = "kbtnSave";
            this.kbtnSave.Size = new System.Drawing.Size(80, 30);
            this.kbtnSave.TabIndex = 1;
            this.kbtnSave.Values.Text = "保存";
            this.kbtnSave.Click += new System.EventHandler(this.kbtnSave_Click);
            // 
            // kbtnCancel
            // 
            this.kbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kbtnCancel.Location = new System.Drawing.Point(800, 15);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(80, 30);
            this.kbtnCancel.TabIndex = 0;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.kbtnCancel_Click);
            // 
            // FrmWideTableProfileEditor
            // 
            this.AcceptButton = this.kbtnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.kbtnCancel;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.kryptonPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmWideTableProfileEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "宽表Profile配置编辑器";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorProfile)).EndInit();
            this.kryptonNavigatorProfile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageBasicInfo)).EndInit();
            this.kryptonPageBasicInfo.ResumeLayout(false);
            this.kryptonPageBasicInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMasterTable)).EndInit();
            this.kryptonPageMasterTable.ResumeLayout(false);
            this.kryptonPageMasterTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbMasterTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageDependencyTables)).EndInit();
            this.kryptonPageDependencyTables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDependencyTables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageChildTables)).EndInit();
            this.kryptonPageChildTables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChildTables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Navigator.KryptonNavigator kryptonNavigatorProfile;
        private Krypton.Navigator.KryptonPage kryptonPageBasicInfo;
        private Krypton.Navigator.KryptonPage kryptonPageMasterTable;
        private Krypton.Navigator.KryptonPage kryptonPageDependencyTables;
        private Krypton.Navigator.KryptonPage kryptonPageChildTables;
        private Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private Krypton.Toolkit.KryptonButton kbtnSave;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonButton kbtnTest;
        private Krypton.Toolkit.KryptonTextBox ktxtProfileName;
        private Krypton.Toolkit.KryptonLabel klblProfileName;
        private Krypton.Toolkit.KryptonTextBox ktxtDescription;
        private Krypton.Toolkit.KryptonLabel klblDescription;
        private Krypton.Toolkit.KryptonComboBox kcmbMasterTable;
        private Krypton.Toolkit.KryptonLabel klblMasterTable;
        private Krypton.Toolkit.KryptonTextBox ktxtBusinessKeys;
        private Krypton.Toolkit.KryptonLabel klblBusinessKeys;
        private RUINORERP.UI.UControls.NewSumDataGridView dgvMasterColumns;
        private Krypton.Toolkit.KryptonButton kbtnAddColumn;
        private Krypton.Toolkit.KryptonButton kbtnEditColumn;
        private Krypton.Toolkit.KryptonButton kbtnDeleteColumn;
        private RUINORERP.UI.UControls.NewSumDataGridView dgvDependencyTables;
        private Krypton.Toolkit.KryptonButton kbtnAddDependency;
        private RUINORERP.UI.UControls.NewSumDataGridView dgvChildTables;
        private Krypton.Toolkit.KryptonButton kbtnAddChild;
        private Krypton.Toolkit.KryptonButton kbtnAutoFill;
    }
}
