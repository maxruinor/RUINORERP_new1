namespace RUINORERP.UI.SmartReminderClient
{
    partial class UCSafetyStockConfigEdit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCSafetyStockConfigEdit));
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonLabel3 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonTextBox1 = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.txtCheckIntervalByMinutes = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.clbLocation_IDs = new Krypton.Toolkit.KryptonCheckedListBox();
            this.btnSeleted = new Krypton.Toolkit.KryptonButton();
            this.lblMaxStock = new Krypton.Toolkit.KryptonLabel();
            this.txtMaxStock = new Krypton.Toolkit.KryptonTextBox();
            this.lb作用对象列表 = new Krypton.Toolkit.KryptonLabel();
            this.lblMinStock = new Krypton.Toolkit.KryptonLabel();
            this.txtMinStock = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonCheckBox1 = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabel4 = new Krypton.Toolkit.KryptonLabel();
            this.txtCalculateSafetyStockIntervalByDays = new Krypton.Toolkit.KryptonTextBox();
            this.btnClear = new Krypton.Toolkit.KryptonButton();
            this.dataGridView1 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.bindingSourceList = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(305, 504);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(423, 504);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.dataGridView1);
            this.kryptonPanel1.Controls.Add(this.btnClear);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.txtCalculateSafetyStockIntervalByDays);
            this.kryptonPanel1.Controls.Add(this.kryptonCheckBox1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.kryptonTextBox1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.txtCheckIntervalByMinutes);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.clbLocation_IDs);
            this.kryptonPanel1.Controls.Add(this.btnSeleted);
            this.kryptonPanel1.Controls.Add(this.lblMaxStock);
            this.kryptonPanel1.Controls.Add(this.txtMaxStock);
            this.kryptonPanel1.Controls.Add(this.lb作用对象列表);
            this.kryptonPanel1.Controls.Add(this.lblMinStock);
            this.kryptonPanel1.Controls.Add(this.txtMinStock);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(774, 553);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(540, 394);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(75, 20);
            this.kryptonLabel3.TabIndex = 58;
            this.kryptonLabel3.Values.Text = "最小库存量";
            // 
            // kryptonTextBox1
            // 
            this.kryptonTextBox1.Location = new System.Drawing.Point(625, 391);
            this.kryptonTextBox1.Name = "kryptonTextBox1";
            this.kryptonTextBox1.Size = new System.Drawing.Size(97, 23);
            this.kryptonTextBox1.TabIndex = 59;
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(519, 194);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(96, 20);
            this.kryptonLabel2.TabIndex = 56;
            this.kryptonLabel2.Values.Text = "检测频率(分钟)";
            this.kryptonLabel2.Click += new System.EventHandler(this.kryptonLabel2_Click);
            // 
            // txtCheckIntervalByMinutes
            // 
            this.txtCheckIntervalByMinutes.Location = new System.Drawing.Point(625, 191);
            this.txtCheckIntervalByMinutes.Name = "txtCheckIntervalByMinutes";
            this.txtCheckIntervalByMinutes.Size = new System.Drawing.Size(97, 23);
            this.txtCheckIntervalByMinutes.TabIndex = 57;
            this.txtCheckIntervalByMinutes.TextChanged += new System.EventHandler(this.txtCheckIntervalByMinutes_TextChanged);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(553, 12);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel1.TabIndex = 55;
            this.kryptonLabel1.Values.Text = "提醒库位";
            // 
            // clbLocation_IDs
            // 
            this.clbLocation_IDs.Location = new System.Drawing.Point(625, 12);
            this.clbLocation_IDs.Name = "clbLocation_IDs";
            this.clbLocation_IDs.Size = new System.Drawing.Size(97, 117);
            this.clbLocation_IDs.TabIndex = 54;
            // 
            // btnSeleted
            // 
            this.btnSeleted.Location = new System.Drawing.Point(385, 420);
            this.btnSeleted.Name = "btnSeleted";
            this.btnSeleted.Size = new System.Drawing.Size(90, 25);
            this.btnSeleted.TabIndex = 51;
            this.btnSeleted.Values.Text = "选择产品对象";
            this.btnSeleted.Click += new System.EventHandler(this.btnSeleted_Click);
            // 
            // lblMaxStock
            // 
            this.lblMaxStock.Location = new System.Drawing.Point(540, 317);
            this.lblMaxStock.Name = "lblMaxStock";
            this.lblMaxStock.Size = new System.Drawing.Size(75, 20);
            this.lblMaxStock.TabIndex = 49;
            this.lblMaxStock.Values.Text = "最大库存量";
            // 
            // txtMaxStock
            // 
            this.txtMaxStock.Location = new System.Drawing.Point(625, 314);
            this.txtMaxStock.Name = "txtMaxStock";
            this.txtMaxStock.Size = new System.Drawing.Size(97, 23);
            this.txtMaxStock.TabIndex = 50;
            // 
            // lb作用对象列表
            // 
            this.lb作用对象列表.Location = new System.Drawing.Point(12, 12);
            this.lb作用对象列表.Name = "lb作用对象列表";
            this.lb作用对象列表.Size = new System.Drawing.Size(114, 20);
            this.lb作用对象列表.TabIndex = 7;
            this.lb作用对象列表.Values.Text = "要检测的产品对象";
            // 
            // lblMinStock
            // 
            this.lblMinStock.Location = new System.Drawing.Point(540, 276);
            this.lblMinStock.Name = "lblMinStock";
            this.lblMinStock.Size = new System.Drawing.Size(75, 20);
            this.lblMinStock.TabIndex = 17;
            this.lblMinStock.Values.Text = "最小库存量";
            // 
            // txtMinStock
            // 
            this.txtMinStock.Location = new System.Drawing.Point(625, 273);
            this.txtMinStock.Name = "txtMinStock";
            this.txtMinStock.Size = new System.Drawing.Size(97, 23);
            this.txtMinStock.TabIndex = 18;
            // 
            // kryptonCheckBox1
            // 
            this.kryptonCheckBox1.Location = new System.Drawing.Point(625, 247);
            this.kryptonCheckBox1.Name = "kryptonCheckBox1";
            this.kryptonCheckBox1.Size = new System.Drawing.Size(127, 20);
            this.kryptonCheckBox1.TabIndex = 60;
            this.kryptonCheckBox1.Values.Text = "手动指定安全库存";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(481, 165);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(134, 20);
            this.kryptonLabel4.TabIndex = 61;
            this.kryptonLabel4.Values.Text = "安全库存计算频率(天)";
            // 
            // txtCalculateSafetyStockIntervalByDays
            // 
            this.txtCalculateSafetyStockIntervalByDays.Location = new System.Drawing.Point(625, 162);
            this.txtCalculateSafetyStockIntervalByDays.Name = "txtCalculateSafetyStockIntervalByDays";
            this.txtCalculateSafetyStockIntervalByDays.Size = new System.Drawing.Size(97, 23);
            this.txtCalculateSafetyStockIntervalByDays.TabIndex = 62;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(12, 420);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(51, 25);
            this.btnClear.TabIndex = 64;
            this.btnClear.Values.Text = "清除";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.CustomRowNo = false;
            this.dataGridView1.EnableFiltering = false;
            this.dataGridView1.FieldNameList = null;
            this.dataGridView1.IsShowSumRow = false;
            this.dataGridView1.Location = new System.Drawing.Point(12, 37);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.NeedSaveColumnsXml = false;
            this.dataGridView1.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007Blue;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(463, 377);
            this.dataGridView1.SumColumns = null;
            this.dataGridView1.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView1.SumRowCellFormat = "N2";
            this.dataGridView1.TabIndex = 65;
            this.dataGridView1.UseBatchEditColumn = false;
            this.dataGridView1.UseCustomColumnDisplay = true;
            this.dataGridView1.UseSelectedColumn = false;
            this.dataGridView1.Use是否使用内置右键功能 = true;
            this.dataGridView1.XmlFileName = "";
            // 
            // UCSafetyStockConfigEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 553);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCSafetyStockConfigEdit";
            this.Load += new System.EventHandler(this.UCBoxRulesEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblMinStock;
        private Krypton.Toolkit.KryptonTextBox txtMinStock;
        private Krypton.Toolkit.KryptonLabel lblMaxStock;
        private Krypton.Toolkit.KryptonTextBox txtMaxStock;
        private Krypton.Toolkit.KryptonButton btnSeleted;
        private Krypton.Toolkit.KryptonLabel lb作用对象列表;
        private Krypton.Toolkit.KryptonCheckedListBox clbLocation_IDs;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonTextBox txtCheckIntervalByMinutes;
        private Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private Krypton.Toolkit.KryptonTextBox kryptonTextBox1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private Krypton.Toolkit.KryptonTextBox txtCalculateSafetyStockIntervalByDays;
        private Krypton.Toolkit.KryptonCheckBox kryptonCheckBox1;
        private Krypton.Toolkit.KryptonButton btnClear;
        internal UControls.NewSumDataGridView dataGridView1;
        internal System.Windows.Forms.BindingSource bindingSourceList;
    }
}
