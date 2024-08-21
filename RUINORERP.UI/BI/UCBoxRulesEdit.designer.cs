namespace RUINORERP.UI.BI
{
    partial class UCBoxRulesEdit
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
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.lblVolume = new Krypton.Toolkit.KryptonLabel();
            this.lblGrossWeight = new Krypton.Toolkit.KryptonLabel();
            this.lblNetWeight = new Krypton.Toolkit.KryptonLabel();
            this.cmbPack_ID = new Krypton.Toolkit.KryptonComboBox();
            this.txtVolume = new Krypton.Toolkit.KryptonTextBox();
            this.txtGrossWeight = new Krypton.Toolkit.KryptonTextBox();
            this.txtNetWeight = new Krypton.Toolkit.KryptonTextBox();
            this.lblPackingMethod = new Krypton.Toolkit.KryptonLabel();
            this.txtPackingMethod = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.txtLength = new Krypton.Toolkit.KryptonTextBox();
            this.txtWidth = new Krypton.Toolkit.KryptonTextBox();
            this.txtHeight = new Krypton.Toolkit.KryptonTextBox();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblBoxRuleName = new Krypton.Toolkit.KryptonLabel();
            this.txtBoxRuleName = new Krypton.Toolkit.KryptonTextBox();
            this.lblPack_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbCartonID = new Krypton.Toolkit.KryptonComboBox();
            this.lblQuantityPerBox = new Krypton.Toolkit.KryptonLabel();
            this.txtQuantityPerBox = new Krypton.Toolkit.KryptonTextBox();
            this.lblLength = new Krypton.Toolkit.KryptonLabel();
            this.lblWidth = new Krypton.Toolkit.KryptonLabel();
            this.lblHeight = new Krypton.Toolkit.KryptonLabel();
            this.lblCartonID = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPack_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCartonID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(196, 466);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(314, 466);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblCartonID);
            this.kryptonPanel1.Controls.Add(this.lblLength);
            this.kryptonPanel1.Controls.Add(this.lblWidth);
            this.kryptonPanel1.Controls.Add(this.lblHeight);
            this.kryptonPanel1.Controls.Add(this.lblVolume);
            this.kryptonPanel1.Controls.Add(this.lblGrossWeight);
            this.kryptonPanel1.Controls.Add(this.lblNetWeight);
            this.kryptonPanel1.Controls.Add(this.cmbPack_ID);
            this.kryptonPanel1.Controls.Add(this.txtVolume);
            this.kryptonPanel1.Controls.Add(this.txtGrossWeight);
            this.kryptonPanel1.Controls.Add(this.txtNetWeight);
            this.kryptonPanel1.Controls.Add(this.lblPackingMethod);
            this.kryptonPanel1.Controls.Add(this.txtPackingMethod);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.txtLength);
            this.kryptonPanel1.Controls.Add(this.txtWidth);
            this.kryptonPanel1.Controls.Add(this.txtHeight);
            this.kryptonPanel1.Controls.Add(this.lblIs_enabled);
            this.kryptonPanel1.Controls.Add(this.chkIs_enabled);
            this.kryptonPanel1.Controls.Add(this.lblBoxRuleName);
            this.kryptonPanel1.Controls.Add(this.txtBoxRuleName);
            this.kryptonPanel1.Controls.Add(this.lblPack_ID);
            this.kryptonPanel1.Controls.Add(this.cmbCartonID);
            this.kryptonPanel1.Controls.Add(this.lblQuantityPerBox);
            this.kryptonPanel1.Controls.Add(this.txtQuantityPerBox);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(643, 543);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblVolume
            // 
            this.lblVolume.Location = new System.Drawing.Point(312, 123);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(83, 20);
            this.lblVolume.TabIndex = 42;
            this.lblVolume.Values.Text = "体积Vol(cm³)";
            // 
            // lblGrossWeight
            // 
            this.lblGrossWeight.Location = new System.Drawing.Point(312, 148);
            this.lblGrossWeight.Name = "lblGrossWeight";
            this.lblGrossWeight.Size = new System.Drawing.Size(87, 20);
            this.lblGrossWeight.TabIndex = 43;
            this.lblGrossWeight.Values.Text = "毛重G.Wt.(kg)";
            // 
            // lblNetWeight
            // 
            this.lblNetWeight.Location = new System.Drawing.Point(312, 173);
            this.lblNetWeight.Name = "lblNetWeight";
            this.lblNetWeight.Size = new System.Drawing.Size(88, 20);
            this.lblNetWeight.TabIndex = 44;
            this.lblNetWeight.Values.Text = "净重N.Wt.(kg)";
            // 
            // cmbPack_ID
            // 
            this.cmbPack_ID.DropDownWidth = 100;
            this.cmbPack_ID.IntegralHeight = false;
            this.cmbPack_ID.Location = new System.Drawing.Point(81, 16);
            this.cmbPack_ID.Name = "cmbPack_ID";
            this.cmbPack_ID.Size = new System.Drawing.Size(214, 21);
            this.cmbPack_ID.TabIndex = 41;
            // 
            // txtVolume
            // 
            this.txtVolume.Location = new System.Drawing.Point(405, 123);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(214, 23);
            this.txtVolume.TabIndex = 22;
            // 
            // txtGrossWeight
            // 
            this.txtGrossWeight.Location = new System.Drawing.Point(405, 150);
            this.txtGrossWeight.Name = "txtGrossWeight";
            this.txtGrossWeight.Size = new System.Drawing.Size(214, 23);
            this.txtGrossWeight.TabIndex = 24;
            // 
            // txtNetWeight
            // 
            this.txtNetWeight.Location = new System.Drawing.Point(405, 177);
            this.txtNetWeight.Name = "txtNetWeight";
            this.txtNetWeight.Size = new System.Drawing.Size(214, 23);
            this.txtNetWeight.TabIndex = 26;
            // 
            // lblPackingMethod
            // 
            this.lblPackingMethod.Location = new System.Drawing.Point(333, 94);
            this.lblPackingMethod.Name = "lblPackingMethod";
            this.lblPackingMethod.Size = new System.Drawing.Size(62, 20);
            this.lblPackingMethod.TabIndex = 28;
            this.lblPackingMethod.Values.Text = "装箱方式";
            // 
            // txtPackingMethod
            // 
            this.txtPackingMethod.Location = new System.Drawing.Point(405, 93);
            this.txtPackingMethod.Name = "txtPackingMethod";
            this.txtPackingMethod.Size = new System.Drawing.Size(214, 23);
            this.txtPackingMethod.TabIndex = 27;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(35, 276);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 29;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(81, 276);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(538, 129);
            this.txtNotes.TabIndex = 30;
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(81, 120);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(214, 23);
            this.txtLength.TabIndex = 32;
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(81, 147);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(214, 23);
            this.txtWidth.TabIndex = 34;
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(81, 176);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(214, 23);
            this.txtHeight.TabIndex = 36;
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(8, 217);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 37;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Location = new System.Drawing.Point(81, 217);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 38;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lblBoxRuleName
            // 
            this.lblBoxRuleName.Location = new System.Drawing.Point(8, 54);
            this.lblBoxRuleName.Name = "lblBoxRuleName";
            this.lblBoxRuleName.Size = new System.Drawing.Size(62, 20);
            this.lblBoxRuleName.TabIndex = 7;
            this.lblBoxRuleName.Values.Text = "箱规名称";
            // 
            // txtBoxRuleName
            // 
            this.txtBoxRuleName.Location = new System.Drawing.Point(81, 54);
            this.txtBoxRuleName.Multiline = true;
            this.txtBoxRuleName.Name = "txtBoxRuleName";
            this.txtBoxRuleName.Size = new System.Drawing.Size(538, 21);
            this.txtBoxRuleName.TabIndex = 8;
            // 
            // lblPack_ID
            // 
            this.lblPack_ID.Location = new System.Drawing.Point(8, 16);
            this.lblPack_ID.Name = "lblPack_ID";
            this.lblPack_ID.Size = new System.Drawing.Size(62, 20);
            this.lblPack_ID.TabIndex = 13;
            this.lblPack_ID.Values.Text = "产品包装";
            // 
            // cmbCartonID
            // 
            this.cmbCartonID.DropDownWidth = 100;
            this.cmbCartonID.IntegralHeight = false;
            this.cmbCartonID.Location = new System.Drawing.Point(405, 16);
            this.cmbCartonID.Name = "cmbCartonID";
            this.cmbCartonID.Size = new System.Drawing.Size(214, 21);
            this.cmbCartonID.TabIndex = 16;
            // 
            // lblQuantityPerBox
            // 
            this.lblQuantityPerBox.Location = new System.Drawing.Point(8, 94);
            this.lblQuantityPerBox.Name = "lblQuantityPerBox";
            this.lblQuantityPerBox.Size = new System.Drawing.Size(62, 20);
            this.lblQuantityPerBox.TabIndex = 17;
            this.lblQuantityPerBox.Values.Text = "每箱数量";
            // 
            // txtQuantityPerBox
            // 
            this.txtQuantityPerBox.Location = new System.Drawing.Point(81, 91);
            this.txtQuantityPerBox.Name = "txtQuantityPerBox";
            this.txtQuantityPerBox.Size = new System.Drawing.Size(214, 23);
            this.txtQuantityPerBox.TabIndex = 18;
            // 
            // lblLength
            // 
            this.lblLength.Location = new System.Drawing.Point(12, 120);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(60, 20);
            this.lblLength.TabIndex = 45;
            this.lblLength.Values.Text = "长度(cm)";
            // 
            // lblWidth
            // 
            this.lblWidth.Location = new System.Drawing.Point(12, 145);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(60, 20);
            this.lblWidth.TabIndex = 46;
            this.lblWidth.Values.Text = "宽度(cm)";
            // 
            // lblHeight
            // 
            this.lblHeight.Location = new System.Drawing.Point(12, 170);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(60, 20);
            this.lblHeight.TabIndex = 47;
            this.lblHeight.Values.Text = "高度(cm)";
            // 
            // lblCartonID
            // 
            this.lblCartonID.Location = new System.Drawing.Point(333, 17);
            this.lblCartonID.Name = "lblCartonID";
            this.lblCartonID.Size = new System.Drawing.Size(62, 20);
            this.lblCartonID.TabIndex = 48;
            this.lblCartonID.Values.Text = "纸箱规格";
            // 
            // UCBoxRulesEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 543);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCBoxRulesEdit";
            this.Load += new System.EventHandler(this.UCBoxRulesEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPack_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCartonID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblBoxRuleName;
        private Krypton.Toolkit.KryptonTextBox txtBoxRuleName;
        private Krypton.Toolkit.KryptonLabel lblQuantityPerBox;
        private Krypton.Toolkit.KryptonTextBox txtQuantityPerBox;
        private Krypton.Toolkit.KryptonTextBox txtVolume;
        private Krypton.Toolkit.KryptonTextBox txtGrossWeight;
        private Krypton.Toolkit.KryptonTextBox txtNetWeight;
        private Krypton.Toolkit.KryptonLabel lblPackingMethod;
        private Krypton.Toolkit.KryptonTextBox txtPackingMethod;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonTextBox txtLength;
        private Krypton.Toolkit.KryptonTextBox txtWidth;
        private Krypton.Toolkit.KryptonTextBox txtHeight;
        private Krypton.Toolkit.KryptonLabel lblIs_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;
        public Krypton.Toolkit.KryptonLabel lblPack_ID;
        public Krypton.Toolkit.KryptonComboBox cmbPack_ID;
        private Krypton.Toolkit.KryptonComboBox cmbCartonID;
        private Krypton.Toolkit.KryptonLabel lblVolume;
        private Krypton.Toolkit.KryptonLabel lblGrossWeight;
        private Krypton.Toolkit.KryptonLabel lblNetWeight;
        private Krypton.Toolkit.KryptonLabel lblLength;
        private Krypton.Toolkit.KryptonLabel lblWidth;
        private Krypton.Toolkit.KryptonLabel lblHeight;
        private Krypton.Toolkit.KryptonLabel lblCartonID;
    }
}
