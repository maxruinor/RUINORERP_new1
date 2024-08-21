using RUINOR.WinFormsUI;

namespace RUINORERP.UI.BI
{
    partial class UCProductCategoriesEdit
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
            this.lblparent_subject_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbTreeParent_id = new RUINOR.WinFormsUI.ComboBoxTreeView();
            this.txtSort = new Krypton.Toolkit.KryptonNumericUpDown();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.lblImages = new Krypton.Toolkit.KryptonLabel();
            this.txtImages = new Krypton.Toolkit.KryptonTextBox();
            this.lblSort = new Krypton.Toolkit.KryptonLabel();
            this.lblIsLock = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.lblCategory_name = new Krypton.Toolkit.KryptonLabel();
            this.txtcategory_name = new Krypton.Toolkit.KryptonTextBox();
            this.lblCategoryCode = new Krypton.Toolkit.KryptonLabel();
            this.txtcategoryCode = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(118, 325);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(236, 325);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblCategory_name);
            this.kryptonPanel1.Controls.Add(this.txtcategory_name);
            this.kryptonPanel1.Controls.Add(this.lblCategoryCode);
            this.kryptonPanel1.Controls.Add(this.txtcategoryCode);
            this.kryptonPanel1.Controls.Add(this.lblparent_subject_id);
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
            this.kryptonPanel1.Size = new System.Drawing.Size(430, 383);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblparent_subject_id
            // 
            this.lblparent_subject_id.Location = new System.Drawing.Point(32, 27);
            this.lblparent_subject_id.Name = "lblparent_subject_id";
            this.lblparent_subject_id.Size = new System.Drawing.Size(60, 20);
            this.lblparent_subject_id.TabIndex = 25;
            this.lblparent_subject_id.Values.Text = "上级类目";
            // 
            // cmbTreeParent_id
            // 
            this.cmbTreeParent_id.FormattingEnabled = true;
            this.cmbTreeParent_id.Location = new System.Drawing.Point(118, 27);
            this.cmbTreeParent_id.Name = "cmbTreeParent_id";
            this.cmbTreeParent_id.Size = new System.Drawing.Size(208, 20);
            this.cmbTreeParent_id.TabIndex = 22;
            // 
            // txtSort
            // 
            this.txtSort.Location = new System.Drawing.Point(118, 133);
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(208, 22);
            this.txtSort.TabIndex = 21;
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Location = new System.Drawing.Point(118, 172);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 20;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(57, 210);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(35, 20);
            this.lblNotes.TabIndex = 12;
            this.lblNotes.Values.Text = "备注";
            // 
            // lblImages
            // 
            this.lblImages.Location = new System.Drawing.Point(45, 106);
            this.lblImages.Name = "lblImages";
            this.lblImages.Size = new System.Drawing.Size(60, 20);
            this.lblImages.TabIndex = 14;
            this.lblImages.Values.Text = "类目图片";
            // 
            // txtImages
            // 
            this.txtImages.Location = new System.Drawing.Point(118, 104);
            this.txtImages.Name = "txtImages";
            this.txtImages.Size = new System.Drawing.Size(208, 20);
            this.txtImages.TabIndex = 15;
            // 
            // lblSort
            // 
            this.lblSort.Location = new System.Drawing.Point(57, 135);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(35, 20);
            this.lblSort.TabIndex = 16;
            this.lblSort.Values.Text = "排序";
            // 
            // lblIsLock
            // 
            this.lblIsLock.Location = new System.Drawing.Point(45, 168);
            this.lblIsLock.Name = "lblIsLock";
            this.lblIsLock.Size = new System.Drawing.Size(60, 20);
            this.lblIsLock.TabIndex = 18;
            this.lblIsLock.Values.Text = "是否启用";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(118, 210);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(208, 90);
            this.txtNotes.TabIndex = 5;
            // 
            // lblCategory_name
            // 
            this.lblCategory_name.Location = new System.Drawing.Point(45, 57);
            this.lblCategory_name.Name = "lblCategory_name";
            this.lblCategory_name.Size = new System.Drawing.Size(60, 20);
            this.lblCategory_name.TabIndex = 26;
            this.lblCategory_name.Values.Text = "类别名称";
            // 
            // txtcategory_name
            // 
            this.txtcategory_name.Location = new System.Drawing.Point(118, 53);
            this.txtcategory_name.Name = "txtcategory_name";
            this.txtcategory_name.Size = new System.Drawing.Size(208, 20);
            this.txtcategory_name.TabIndex = 27;
            // 
            // lblCategoryCode
            // 
            this.lblCategoryCode.Location = new System.Drawing.Point(45, 82);
            this.lblCategoryCode.Name = "lblCategoryCode";
            this.lblCategoryCode.Size = new System.Drawing.Size(60, 20);
            this.lblCategoryCode.TabIndex = 28;
            this.lblCategoryCode.Values.Text = "类别代码";
            // 
            // txtcategoryCode
            // 
            this.txtcategoryCode.Location = new System.Drawing.Point(118, 78);
            this.txtcategoryCode.Name = "txtcategoryCode";
            this.txtcategoryCode.Size = new System.Drawing.Size(208, 20);
            this.txtcategoryCode.TabIndex = 29;
            // 
            // UCProductCategoriesEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 383);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCProductCategoriesEdit";
            this.Load += new System.EventHandler(this.UCProductCategoriesEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonLabel lblImages;
        private Krypton.Toolkit.KryptonTextBox txtImages;
        private Krypton.Toolkit.KryptonLabel lblSort;
        private Krypton.Toolkit.KryptonLabel lblIsLock;
        private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;
        private Krypton.Toolkit.KryptonNumericUpDown txtSort;
        private ComboBoxTreeView cmbTreeParent_id;
        private Krypton.Toolkit.KryptonLabel lblparent_subject_id;
        private Krypton.Toolkit.KryptonLabel lblCategory_name;
        private Krypton.Toolkit.KryptonTextBox txtcategory_name;
        private Krypton.Toolkit.KryptonLabel lblCategoryCode;
        private Krypton.Toolkit.KryptonTextBox txtcategoryCode;
    }
}
