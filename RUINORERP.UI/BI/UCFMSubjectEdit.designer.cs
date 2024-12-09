using RUINOR.WinFormsUI;

namespace RUINORERP.UI.BI
{
    partial class UCFMSubjectEdit
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
            this.cmbSubject_Type = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonGroupBox4 = new Krypton.Toolkit.KryptonGroupBox();
            this.rdb贷 = new RUINORERP.UI.UControls.RadioButtonBind();
            this.rdb借 = new RUINORERP.UI.UControls.RadioButtonBind();
            this.lblparent_subject_id = new Krypton.Toolkit.KryptonLabel();
            this.lblsubject_code = new Krypton.Toolkit.KryptonLabel();
            this.txtsubject_code = new Krypton.Toolkit.KryptonTextBox();
            this.lblsubject_name = new Krypton.Toolkit.KryptonLabel();
            this.txtsubject_name = new Krypton.Toolkit.KryptonTextBox();
            this.lblsubject_en_name = new Krypton.Toolkit.KryptonLabel();
            this.txtsubject_en_name = new Krypton.Toolkit.KryptonTextBox();
            this.lblSubject_Type = new Krypton.Toolkit.KryptonLabel();
            this.lblBalance_direction = new Krypton.Toolkit.KryptonLabel();
            this.cmbTreeParent_id = new RUINOR.WinFormsUI.ComboBoxTreeView();
            this.txtSort = new Krypton.Toolkit.KryptonNumericUpDown();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.lblImages = new Krypton.Toolkit.KryptonLabel();
            this.txtImages = new Krypton.Toolkit.KryptonTextBox();
            this.lblSort = new Krypton.Toolkit.KryptonLabel();
            this.lblIsLock = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSubject_Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4.Panel)).BeginInit();
            this.kryptonGroupBox4.Panel.SuspendLayout();
            this.kryptonGroupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(211, 440);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(329, 440);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.cmbSubject_Type);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox4);
            this.kryptonPanel1.Controls.Add(this.lblparent_subject_id);
            this.kryptonPanel1.Controls.Add(this.lblsubject_code);
            this.kryptonPanel1.Controls.Add(this.txtsubject_code);
            this.kryptonPanel1.Controls.Add(this.lblsubject_name);
            this.kryptonPanel1.Controls.Add(this.txtsubject_name);
            this.kryptonPanel1.Controls.Add(this.lblsubject_en_name);
            this.kryptonPanel1.Controls.Add(this.txtsubject_en_name);
            this.kryptonPanel1.Controls.Add(this.lblSubject_Type);
            this.kryptonPanel1.Controls.Add(this.lblBalance_direction);
            this.kryptonPanel1.Controls.Add(this.cmbTreeParent_id);
            this.kryptonPanel1.Controls.Add(this.txtSort);
            this.kryptonPanel1.Controls.Add(this.chkIs_enabled);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.lblImages);
            this.kryptonPanel1.Controls.Add(this.txtImages);
            this.kryptonPanel1.Controls.Add(this.lblSort);
            this.kryptonPanel1.Controls.Add(this.lblIsLock);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(609, 500);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // cmbSubject_Type
            // 
            this.cmbSubject_Type.DropDownWidth = 100;
            this.cmbSubject_Type.IntegralHeight = false;
            this.cmbSubject_Type.Location = new System.Drawing.Point(211, 114);
            this.cmbSubject_Type.Name = "cmbSubject_Type";
            this.cmbSubject_Type.Size = new System.Drawing.Size(208, 21);
            this.cmbSubject_Type.TabIndex = 153;
            // 
            // kryptonGroupBox4
            // 
            this.kryptonGroupBox4.CaptionVisible = false;
            this.kryptonGroupBox4.Location = new System.Drawing.Point(211, 155);
            this.kryptonGroupBox4.Name = "kryptonGroupBox4";
            // 
            // kryptonGroupBox4.Panel
            // 
            this.kryptonGroupBox4.Panel.Controls.Add(this.rdb贷);
            this.kryptonGroupBox4.Panel.Controls.Add(this.rdb借);
            this.kryptonGroupBox4.Size = new System.Drawing.Size(133, 37);
            this.kryptonGroupBox4.TabIndex = 152;
            // 
            // rdb贷
            // 
            this.rdb贷.Location = new System.Drawing.Point(75, 8);
            this.rdb贷.Name = "rdb贷";
            this.rdb贷.SelectValue = "支出";
            this.rdb贷.Size = new System.Drawing.Size(35, 20);
            this.rdb贷.TabIndex = 69;
            this.rdb贷.Values.Text = "贷";
            // 
            // rdb借
            // 
            this.rdb借.Location = new System.Drawing.Point(15, 8);
            this.rdb借.Name = "rdb借";
            this.rdb借.SelectValue = "";
            this.rdb借.Size = new System.Drawing.Size(35, 20);
            this.rdb借.TabIndex = 68;
            this.rdb借.Values.Text = "借";
            // 
            // lblparent_subject_id
            // 
            this.lblparent_subject_id.Location = new System.Drawing.Point(145, 12);
            this.lblparent_subject_id.Name = "lblparent_subject_id";
            this.lblparent_subject_id.Size = new System.Drawing.Size(62, 20);
            this.lblparent_subject_id.TabIndex = 25;
            this.lblparent_subject_id.Values.Text = "上级科目";
            // 
            // lblsubject_code
            // 
            this.lblsubject_code.Location = new System.Drawing.Point(145, 42);
            this.lblsubject_code.Name = "lblsubject_code";
            this.lblsubject_code.Size = new System.Drawing.Size(62, 20);
            this.lblsubject_code.TabIndex = 27;
            this.lblsubject_code.Values.Text = "科目代码";
            // 
            // txtsubject_code
            // 
            this.txtsubject_code.Location = new System.Drawing.Point(211, 38);
            this.txtsubject_code.Name = "txtsubject_code";
            this.txtsubject_code.Size = new System.Drawing.Size(208, 23);
            this.txtsubject_code.TabIndex = 28;
            // 
            // lblsubject_name
            // 
            this.lblsubject_name.Location = new System.Drawing.Point(145, 67);
            this.lblsubject_name.Name = "lblsubject_name";
            this.lblsubject_name.Size = new System.Drawing.Size(62, 20);
            this.lblsubject_name.TabIndex = 29;
            this.lblsubject_name.Values.Text = "科目名称";
            // 
            // txtsubject_name
            // 
            this.txtsubject_name.Location = new System.Drawing.Point(211, 63);
            this.txtsubject_name.Name = "txtsubject_name";
            this.txtsubject_name.Size = new System.Drawing.Size(208, 23);
            this.txtsubject_name.TabIndex = 30;
            // 
            // lblsubject_en_name
            // 
            this.lblsubject_en_name.Location = new System.Drawing.Point(111, 91);
            this.lblsubject_en_name.Name = "lblsubject_en_name";
            this.lblsubject_en_name.Size = new System.Drawing.Size(94, 20);
            this.lblsubject_en_name.TabIndex = 31;
            this.lblsubject_en_name.Values.Text = "科目名称（EN)";
            // 
            // txtsubject_en_name
            // 
            this.txtsubject_en_name.Location = new System.Drawing.Point(211, 88);
            this.txtsubject_en_name.Name = "txtsubject_en_name";
            this.txtsubject_en_name.Size = new System.Drawing.Size(208, 23);
            this.txtsubject_en_name.TabIndex = 32;
            // 
            // lblSubject_Type
            // 
            this.lblSubject_Type.Location = new System.Drawing.Point(145, 117);
            this.lblSubject_Type.Name = "lblSubject_Type";
            this.lblSubject_Type.Size = new System.Drawing.Size(62, 20);
            this.lblSubject_Type.TabIndex = 33;
            this.lblSubject_Type.Values.Text = "科目类型";
            // 
            // lblBalance_direction
            // 
            this.lblBalance_direction.Location = new System.Drawing.Point(87, 165);
            this.lblBalance_direction.Name = "lblBalance_direction";
            this.lblBalance_direction.Size = new System.Drawing.Size(122, 20);
            this.lblBalance_direction.TabIndex = 35;
            this.lblBalance_direction.Values.Text = "(科目性质)余额方向";
            // 
            // cmbTreeParent_id
            // 
            this.cmbTreeParent_id.FormattingEnabled = true;
            this.cmbTreeParent_id.Location = new System.Drawing.Point(211, 12);
            this.cmbTreeParent_id.Name = "cmbTreeParent_id";
            this.cmbTreeParent_id.Size = new System.Drawing.Size(208, 20);
            this.cmbTreeParent_id.TabIndex = 22;
            // 
            // txtSort
            // 
            this.txtSort.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtSort.Location = new System.Drawing.Point(211, 229);
            this.txtSort.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtSort.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(208, 22);
            this.txtSort.TabIndex = 21;
            this.txtSort.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Location = new System.Drawing.Point(211, 271);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 20;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(170, 309);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 12;
            this.lblNotes.Values.Text = "备注";
            // 
            // lblImages
            // 
            this.lblImages.Location = new System.Drawing.Point(145, 200);
            this.lblImages.Name = "lblImages";
            this.lblImages.Size = new System.Drawing.Size(62, 20);
            this.lblImages.TabIndex = 14;
            this.lblImages.Values.Text = "类目图片";
            // 
            // txtImages
            // 
            this.txtImages.Location = new System.Drawing.Point(211, 198);
            this.txtImages.Name = "txtImages";
            this.txtImages.Size = new System.Drawing.Size(208, 23);
            this.txtImages.TabIndex = 15;
            // 
            // lblSort
            // 
            this.lblSort.Location = new System.Drawing.Point(170, 232);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(36, 20);
            this.lblSort.TabIndex = 16;
            this.lblSort.Values.Text = "排序";
            // 
            // lblIsLock
            // 
            this.lblIsLock.Location = new System.Drawing.Point(145, 267);
            this.lblIsLock.Name = "lblIsLock";
            this.lblIsLock.Size = new System.Drawing.Size(62, 20);
            this.lblIsLock.TabIndex = 18;
            this.lblIsLock.Values.Text = "是否启用";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(211, 309);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(208, 90);
            this.txtNotes.TabIndex = 5;
            // 
            // UCFMSubjectEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 500);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCFMSubjectEdit";
            this.Load += new System.EventHandler(this.UCProductCategoriesEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSubject_Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4.Panel)).EndInit();
            this.kryptonGroupBox4.Panel.ResumeLayout(false);
            this.kryptonGroupBox4.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4)).EndInit();
            this.kryptonGroupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonLabel lblSort;
        private Krypton.Toolkit.KryptonLabel lblIsLock;
        private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;
        private Krypton.Toolkit.KryptonNumericUpDown txtSort;
        private ComboBoxTreeView cmbTreeParent_id;
        private Krypton.Toolkit.KryptonLabel lblparent_subject_id;
        private Krypton.Toolkit.KryptonLabel lblsubject_code;
        private Krypton.Toolkit.KryptonTextBox txtsubject_code;
        private Krypton.Toolkit.KryptonLabel lblsubject_name;
        private Krypton.Toolkit.KryptonTextBox txtsubject_name;
        private Krypton.Toolkit.KryptonLabel lblsubject_en_name;
        private Krypton.Toolkit.KryptonTextBox txtsubject_en_name;
        private Krypton.Toolkit.KryptonLabel lblSubject_Type;
        private Krypton.Toolkit.KryptonLabel lblBalance_direction;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox4;
        private UControls.RadioButtonBind rdb贷;
        private UControls.RadioButtonBind rdb借;
        private Krypton.Toolkit.KryptonLabel lblImages;
        private Krypton.Toolkit.KryptonTextBox txtImages;
        private Krypton.Toolkit.KryptonComboBox cmbSubject_Type;
    }
}
