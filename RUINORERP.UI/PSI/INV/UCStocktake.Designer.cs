using RUINORERP.Model;
using RUINORERP.Model.QueryDto;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.PSI.INV
{
    partial class UCStocktake
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
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
            this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
            this.lblReview = new Krypton.Toolkit.KryptonLabel();
            this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();
            this.btnImportCheckProd = new Krypton.Toolkit.KryptonButton();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.dtpCarryingDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.cmbCheckMode = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.cmb调整类型 = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblEmployee = new Krypton.Toolkit.KryptonLabel();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.lblstatus = new Krypton.Toolkit.KryptonLabel();
            this.txtstatus = new Krypton.Toolkit.KryptonTextBox();
            this.lblCheckNo = new Krypton.Toolkit.KryptonLabel();
            this.txtCheckNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblcheck_date = new Krypton.Toolkit.KryptonLabel();
            this.dtpcheck_date = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lbl盘点单 = new Krypton.Toolkit.KryptonLabel();
            this.SplitContainerGridAndSub = new Krypton.Toolkit.KryptonSplitContainer();
            this.grid1 = new SourceGrid.Grid();
            this.lblDiffQty = new Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            this.txtCheckTotalQty = new Krypton.Toolkit.KryptonTextBox();
            this.txtCheckTotalAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblCheckTotalAmount = new Krypton.Toolkit.KryptonLabel();
            this.lblCheckTotalQty = new Krypton.Toolkit.KryptonLabel();
            this.txtDiffAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblDiffAmount = new Krypton.Toolkit.KryptonLabel();
            this.txtCarryingTotalQty = new Krypton.Toolkit.KryptonTextBox();
            this.txtCarryingTotalAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblCarryingTotalAmount = new Krypton.Toolkit.KryptonLabel();
            this.lblCarryingTotalQty = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocation_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCheckMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb调整类型)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerGridAndSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerGridAndSub.Panel1)).BeginInit();
            this.SplitContainerGridAndSub.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerGridAndSub.Panel2)).BeginInit();
            this.SplitContainerGridAndSub.Panel2.SuspendLayout();
            this.SplitContainerGridAndSub.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 25);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            this.kryptonSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer1.Panel1
            // 
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblDataStatus);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblPrintStatus);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblReview);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblLocation_ID);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.cmbLocation_ID);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.btnImportCheckProd);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonLabel3);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.dtpCarryingDate);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.cmbCheckMode);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonLabel2);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.cmb调整类型);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonLabel1);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.cmbEmployee_ID);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblEmployee);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblNotes);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.txtNotes);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblstatus);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.txtstatus);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblCheckNo);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.txtCheckNo);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lblcheck_date);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.dtpcheck_date);
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.lbl盘点单);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.SplitContainerGridAndSub);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(927, 545);
            this.kryptonSplitContainer1.SplitterDistance = 189;
            this.kryptonSplitContainer1.TabIndex = 4;
            // 
            // lblDataStatus
            // 
            this.lblDataStatus.Location = new System.Drawing.Point(799, 6);
            this.lblDataStatus.Name = "lblDataStatus";
            this.lblDataStatus.Size = new System.Drawing.Size(125, 38);
            this.lblDataStatus.StateNormal.ShortText.Color1 = System.Drawing.Color.Red;
            this.lblDataStatus.StateNormal.ShortText.Font = new System.Drawing.Font("长城行楷体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDataStatus.TabIndex = 103;
            this.lblDataStatus.Values.Text = "数据状态";
            // 
            // lblPrintStatus
            // 
            this.lblPrintStatus.Location = new System.Drawing.Point(455, 6);
            this.lblPrintStatus.Name = "lblPrintStatus";
            this.lblPrintStatus.Size = new System.Drawing.Size(125, 38);
            this.lblPrintStatus.StateNormal.ShortText.Color1 = System.Drawing.Color.Red;
            this.lblPrintStatus.StateNormal.ShortText.Font = new System.Drawing.Font("长城行楷体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPrintStatus.TabIndex = 102;
            this.lblPrintStatus.Values.Text = "打印状态";
            // 
            // lblReview
            // 
            this.lblReview.Location = new System.Drawing.Point(628, 6);
            this.lblReview.Name = "lblReview";
            this.lblReview.Size = new System.Drawing.Size(125, 38);
            this.lblReview.StateNormal.ShortText.Color1 = System.Drawing.Color.Red;
            this.lblReview.StateNormal.ShortText.Font = new System.Drawing.Font("长城行楷体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblReview.TabIndex = 101;
            this.lblReview.Values.Text = "审核状态";
            // 
            // lblLocation_ID
            // 
            this.lblLocation_ID.Location = new System.Drawing.Point(50, 103);
            this.lblLocation_ID.Name = "lblLocation_ID";
            this.lblLocation_ID.Size = new System.Drawing.Size(62, 20);
            this.lblLocation_ID.TabIndex = 85;
            this.lblLocation_ID.Values.Text = "盘点仓库";
            // 
            // cmbLocation_ID
            // 
            this.cmbLocation_ID.DropDownWidth = 100;
            this.cmbLocation_ID.IntegralHeight = false;
            this.cmbLocation_ID.Location = new System.Drawing.Point(123, 99);
            this.cmbLocation_ID.Name = "cmbLocation_ID";
            this.cmbLocation_ID.Size = new System.Drawing.Size(132, 21);
            this.cmbLocation_ID.TabIndex = 86;
            // 
            // btnImportCheckProd
            // 
            this.btnImportCheckProd.Location = new System.Drawing.Point(388, 98);
            this.btnImportCheckProd.Name = "btnImportCheckProd";
            this.btnImportCheckProd.Size = new System.Drawing.Size(137, 25);
            this.btnImportCheckProd.TabIndex = 83;
            this.btnImportCheckProd.Values.Text = "导入产品(&I)";
            this.btnImportCheckProd.Click += new System.EventHandler(this.btnImportCheckProd_Click);
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(315, 71);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel3.TabIndex = 80;
            this.kryptonLabel3.Values.Text = "载账日期";
            // 
            // dtpCarryingDate
            // 
            this.dtpCarryingDate.CalendarTodayDate = new System.DateTime(2023, 10, 13, 0, 0, 0, 0);
            this.dtpCarryingDate.Location = new System.Drawing.Point(388, 71);
            this.dtpCarryingDate.Name = "dtpCarryingDate";
            this.dtpCarryingDate.Size = new System.Drawing.Size(137, 21);
            this.dtpCarryingDate.TabIndex = 81;
            // 
            // cmbCheckMode
            // 
            this.cmbCheckMode.DropDownWidth = 100;
            this.cmbCheckMode.IntegralHeight = false;
            this.cmbCheckMode.Location = new System.Drawing.Point(388, 42);
            this.cmbCheckMode.Name = "cmbCheckMode";
            this.cmbCheckMode.Size = new System.Drawing.Size(137, 21);
            this.cmbCheckMode.TabIndex = 79;
            this.cmbCheckMode.SelectedIndexChanged += new System.EventHandler(this.cmbCheckMode_SelectedIndexChanged);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(315, 42);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel2.TabIndex = 78;
            this.kryptonLabel2.Values.Text = "盘点方式";
            // 
            // cmb调整类型
            // 
            this.cmb调整类型.DropDownWidth = 100;
            this.cmb调整类型.IntegralHeight = false;
            this.cmb调整类型.Location = new System.Drawing.Point(653, 71);
            this.cmb调整类型.Name = "cmb调整类型";
            this.cmb调整类型.Size = new System.Drawing.Size(131, 21);
            this.cmb调整类型.TabIndex = 77;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(592, 71);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel1.TabIndex = 76;
            this.kryptonLabel1.Values.Text = "调整类型";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(653, 42);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(131, 21);
            this.cmbEmployee_ID.TabIndex = 75;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(579, 42);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(75, 20);
            this.lblEmployee.TabIndex = 74;
            this.lblEmployee.Values.Text = "盘点责任人";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(50, 127);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 57;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(123, 127);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(661, 57);
            this.txtNotes.TabIndex = 58;
            // 
            // lblstatus
            // 
            this.lblstatus.Location = new System.Drawing.Point(592, 98);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(62, 20);
            this.lblstatus.TabIndex = 59;
            this.lblstatus.Values.Text = "单据状态";
            // 
            // txtstatus
            // 
            this.txtstatus.Location = new System.Drawing.Point(653, 98);
            this.txtstatus.Name = "txtstatus";
            this.txtstatus.Size = new System.Drawing.Size(131, 23);
            this.txtstatus.TabIndex = 60;
            // 
            // lblCheckNo
            // 
            this.lblCheckNo.Location = new System.Drawing.Point(50, 43);
            this.lblCheckNo.Name = "lblCheckNo";
            this.lblCheckNo.Size = new System.Drawing.Size(62, 20);
            this.lblCheckNo.TabIndex = 45;
            this.lblCheckNo.Values.Text = "盘点单号";
            // 
            // txtCheckNo
            // 
            this.txtCheckNo.Location = new System.Drawing.Point(123, 43);
            this.txtCheckNo.Name = "txtCheckNo";
            this.txtCheckNo.Size = new System.Drawing.Size(132, 23);
            this.txtCheckNo.TabIndex = 46;
            // 
            // lblcheck_date
            // 
            this.lblcheck_date.Location = new System.Drawing.Point(50, 72);
            this.lblcheck_date.Name = "lblcheck_date";
            this.lblcheck_date.Size = new System.Drawing.Size(62, 20);
            this.lblcheck_date.TabIndex = 51;
            this.lblcheck_date.Values.Text = "盘点日期";
            // 
            // dtpcheck_date
            // 
            this.dtpcheck_date.CalendarTodayDate = new System.DateTime(2023, 10, 13, 0, 0, 0, 0);
            this.dtpcheck_date.Location = new System.Drawing.Point(123, 72);
            this.dtpcheck_date.Name = "dtpcheck_date";
            this.dtpcheck_date.Size = new System.Drawing.Size(132, 21);
            this.dtpcheck_date.TabIndex = 52;
            // 
            // lbl盘点单
            // 
            this.lbl盘点单.LabelStyle = Krypton.Toolkit.LabelStyle.TitlePanel;
            this.lbl盘点单.Location = new System.Drawing.Point(361, 6);
            this.lbl盘点单.Name = "lbl盘点单";
            this.lbl盘点单.Size = new System.Drawing.Size(71, 29);
            this.lbl盘点单.StateCommon.LongText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lbl盘点单.StateCommon.LongText.Color2 = System.Drawing.Color.Lime;
            this.lbl盘点单.TabIndex = 44;
            this.lbl盘点单.Values.Text = "盘点单";
            // 
            // SplitContainerGridAndSub
            // 
            this.SplitContainerGridAndSub.Cursor = System.Windows.Forms.Cursors.Default;
            this.SplitContainerGridAndSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainerGridAndSub.Location = new System.Drawing.Point(0, 0);
            this.SplitContainerGridAndSub.Name = "SplitContainerGridAndSub";
            this.SplitContainerGridAndSub.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainerGridAndSub.Panel1
            // 
            this.SplitContainerGridAndSub.Panel1.Controls.Add(this.grid1);
            // 
            // SplitContainerGridAndSub.Panel2
            // 
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.lblDiffQty);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.kryptonLabel4);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.txtCheckTotalQty);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.txtCheckTotalAmount);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.lblCheckTotalAmount);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.lblCheckTotalQty);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.txtDiffAmount);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.lblDiffAmount);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.txtCarryingTotalQty);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.txtCarryingTotalAmount);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.lblCarryingTotalAmount);
            this.SplitContainerGridAndSub.Panel2.Controls.Add(this.lblCarryingTotalQty);
            this.SplitContainerGridAndSub.Size = new System.Drawing.Size(927, 351);
            this.SplitContainerGridAndSub.SplitterDistance = 250;
            this.SplitContainerGridAndSub.TabIndex = 2;
            // 
            // grid1
            // 
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EnableSort = true;
            this.grid1.HasSummary = true;
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.grid1.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.grid1.Size = new System.Drawing.Size(927, 250);
            this.grid1.TabIndex = 1;
            this.grid1.TabStop = true;
            this.grid1.ToolTipText = "";
            // 
            // lblDiffQty
            // 
            this.lblDiffQty.Location = new System.Drawing.Point(669, 21);
            this.lblDiffQty.Name = "lblDiffQty";
            this.lblDiffQty.Size = new System.Drawing.Size(17, 20);
            this.lblDiffQty.TabIndex = 71;
            this.lblDiffQty.Values.Text = "0";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(579, 21);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(75, 20);
            this.kryptonLabel4.TabIndex = 69;
            this.kryptonLabel4.Values.Text = "差异总数量";
            // 
            // txtCheckTotalQty
            // 
            this.txtCheckTotalQty.Location = new System.Drawing.Point(114, 17);
            this.txtCheckTotalQty.Name = "txtCheckTotalQty";
            this.txtCheckTotalQty.Size = new System.Drawing.Size(122, 23);
            this.txtCheckTotalQty.TabIndex = 66;
            // 
            // txtCheckTotalAmount
            // 
            this.txtCheckTotalAmount.Location = new System.Drawing.Point(114, 42);
            this.txtCheckTotalAmount.Name = "txtCheckTotalAmount";
            this.txtCheckTotalAmount.Size = new System.Drawing.Size(122, 23);
            this.txtCheckTotalAmount.TabIndex = 68;
            // 
            // lblCheckTotalAmount
            // 
            this.lblCheckTotalAmount.Location = new System.Drawing.Point(41, 46);
            this.lblCheckTotalAmount.Name = "lblCheckTotalAmount";
            this.lblCheckTotalAmount.Size = new System.Drawing.Size(75, 20);
            this.lblCheckTotalAmount.TabIndex = 67;
            this.lblCheckTotalAmount.Values.Text = "盘点总成本";
            // 
            // lblCheckTotalQty
            // 
            this.lblCheckTotalQty.Location = new System.Drawing.Point(41, 21);
            this.lblCheckTotalQty.Name = "lblCheckTotalQty";
            this.lblCheckTotalQty.Size = new System.Drawing.Size(75, 20);
            this.lblCheckTotalQty.TabIndex = 65;
            this.lblCheckTotalQty.Values.Text = "盘点总数量";
            // 
            // txtDiffAmount
            // 
            this.txtDiffAmount.Location = new System.Drawing.Point(653, 46);
            this.txtDiffAmount.Name = "txtDiffAmount";
            this.txtDiffAmount.Size = new System.Drawing.Size(131, 23);
            this.txtDiffAmount.TabIndex = 64;
            // 
            // lblDiffAmount
            // 
            this.lblDiffAmount.Location = new System.Drawing.Point(579, 50);
            this.lblDiffAmount.Name = "lblDiffAmount";
            this.lblDiffAmount.Size = new System.Drawing.Size(75, 20);
            this.lblDiffAmount.TabIndex = 63;
            this.lblDiffAmount.Values.Text = "差异总金额";
            // 
            // txtCarryingTotalQty
            // 
            this.txtCarryingTotalQty.Location = new System.Drawing.Point(361, 17);
            this.txtCarryingTotalQty.Name = "txtCarryingTotalQty";
            this.txtCarryingTotalQty.Size = new System.Drawing.Size(123, 23);
            this.txtCarryingTotalQty.TabIndex = 48;
            // 
            // txtCarryingTotalAmount
            // 
            this.txtCarryingTotalAmount.Location = new System.Drawing.Point(361, 42);
            this.txtCarryingTotalAmount.Name = "txtCarryingTotalAmount";
            this.txtCarryingTotalAmount.Size = new System.Drawing.Size(123, 23);
            this.txtCarryingTotalAmount.TabIndex = 50;
            // 
            // lblCarryingTotalAmount
            // 
            this.lblCarryingTotalAmount.Location = new System.Drawing.Point(288, 46);
            this.lblCarryingTotalAmount.Name = "lblCarryingTotalAmount";
            this.lblCarryingTotalAmount.Size = new System.Drawing.Size(75, 20);
            this.lblCarryingTotalAmount.TabIndex = 49;
            this.lblCarryingTotalAmount.Values.Text = "载账总成本";
            // 
            // lblCarryingTotalQty
            // 
            this.lblCarryingTotalQty.Location = new System.Drawing.Point(288, 21);
            this.lblCarryingTotalQty.Name = "lblCarryingTotalQty";
            this.lblCarryingTotalQty.Size = new System.Drawing.Size(75, 20);
            this.lblCarryingTotalQty.TabIndex = 47;
            this.lblCarryingTotalQty.Values.Text = "载账总数量";
            // 
            // UCStocktake
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Name = "UCStocktake";
            this.Load += new System.EventHandler(this.UCStocktake_Load);
            this.Controls.SetChildIndex(this.kryptonSplitContainer1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            this.kryptonSplitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocation_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCheckMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb调整类型)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerGridAndSub.Panel1)).EndInit();
            this.SplitContainerGridAndSub.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerGridAndSub.Panel2)).EndInit();
            this.SplitContainerGridAndSub.Panel2.ResumeLayout(false);
            this.SplitContainerGridAndSub.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerGridAndSub)).EndInit();
            this.SplitContainerGridAndSub.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private SourceGrid.Grid grid1;
        private Krypton.Toolkit.KryptonLabel lbl盘点单;
        private Krypton.Toolkit.KryptonLabel lblCheckNo;
        private Krypton.Toolkit.KryptonTextBox txtCheckNo;
        private Krypton.Toolkit.KryptonLabel lblCarryingTotalQty;
        private Krypton.Toolkit.KryptonTextBox txtCarryingTotalQty;
        private Krypton.Toolkit.KryptonLabel lblCarryingTotalAmount;
        private Krypton.Toolkit.KryptonTextBox txtCarryingTotalAmount;
        private Krypton.Toolkit.KryptonLabel lblcheck_date;
        private Krypton.Toolkit.KryptonDateTimePicker dtpcheck_date;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonLabel lblstatus;
        private Krypton.Toolkit.KryptonTextBox txtstatus;
        private Krypton.Toolkit.KryptonLabel lblDiffAmount;
        private Krypton.Toolkit.KryptonTextBox txtDiffAmount;
        private Krypton.Toolkit.KryptonLabel lblCheckTotalQty;
        private Krypton.Toolkit.KryptonTextBox txtCheckTotalQty;
        private Krypton.Toolkit.KryptonLabel lblCheckTotalAmount;
        private Krypton.Toolkit.KryptonTextBox txtCheckTotalAmount;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
        private Krypton.Toolkit.KryptonLabel lblEmployee;
        private Krypton.Toolkit.KryptonComboBox cmb调整类型;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonComboBox cmbCheckMode;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonDateTimePicker dtpCarryingDate;
        private Krypton.Toolkit.KryptonButton btnImportCheckProd;
        private Krypton.Toolkit.KryptonSplitContainer SplitContainerGridAndSub;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonLabel lblDiffQty;
        private Krypton.Toolkit.KryptonLabel lblLocation_ID;
        private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;
        private Krypton.Toolkit.KryptonLabel lblDataStatus;
        private Krypton.Toolkit.KryptonLabel lblPrintStatus;
        private Krypton.Toolkit.KryptonLabel lblReview;
    }
}
