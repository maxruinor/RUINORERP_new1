namespace RUINORERP.UI.BI
{
    partial class UCPayMethodAccountMapperEdit
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
            this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbPaytype_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
            this.cmbAccount_id = new Krypton.Toolkit.KryptonComboBox();
            this.lblDescription = new Krypton.Toolkit.KryptonLabel();
            this.txtDescription = new Krypton.Toolkit.KryptonTextBox();
            this.lblEffectiveDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpEffectiveDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblExpiryDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpExpiryDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPaytype_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccount_id)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(162, 425);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(304, 425);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.lblPaytype_ID);
            this.kryptonPanel1.Controls.Add(this.cmbPaytype_ID);
            this.kryptonPanel1.Controls.Add(this.lblAccount_id);
            this.kryptonPanel1.Controls.Add(this.cmbAccount_id);
            this.kryptonPanel1.Controls.Add(this.lblDescription);
            this.kryptonPanel1.Controls.Add(this.txtDescription);
            this.kryptonPanel1.Controls.Add(this.lblEffectiveDate);
            this.kryptonPanel1.Controls.Add(this.dtpEffectiveDate);
            this.kryptonPanel1.Controls.Add(this.lblExpiryDate);
            this.kryptonPanel1.Controls.Add(this.dtpExpiryDate);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(568, 511);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblPaytype_ID
            // 
            this.lblPaytype_ID.Location = new System.Drawing.Point(78, 24);
            this.lblPaytype_ID.Name = "lblPaytype_ID";
            this.lblPaytype_ID.Size = new System.Drawing.Size(62, 20);
            this.lblPaytype_ID.TabIndex = 6;
            this.lblPaytype_ID.Values.Text = "付款方式";
            // 
            // cmbPaytype_ID
            // 
            this.cmbPaytype_ID.DropDownWidth = 100;
            this.cmbPaytype_ID.IntegralHeight = false;
            this.cmbPaytype_ID.Location = new System.Drawing.Point(151, 24);
            this.cmbPaytype_ID.Name = "cmbPaytype_ID";
            this.cmbPaytype_ID.Size = new System.Drawing.Size(295, 21);
            this.cmbPaytype_ID.TabIndex = 7;
            // 
            // lblAccount_id
            // 
            this.lblAccount_id.Location = new System.Drawing.Point(78, 63);
            this.lblAccount_id.Name = "lblAccount_id";
            this.lblAccount_id.Size = new System.Drawing.Size(62, 20);
            this.lblAccount_id.TabIndex = 8;
            this.lblAccount_id.Values.Text = "公司账户";
            // 
            // cmbAccount_id
            // 
            this.cmbAccount_id.DropDownWidth = 100;
            this.cmbAccount_id.IntegralHeight = false;
            this.cmbAccount_id.Location = new System.Drawing.Point(151, 63);
            this.cmbAccount_id.Name = "cmbAccount_id";
            this.cmbAccount_id.Size = new System.Drawing.Size(295, 21);
            this.cmbAccount_id.TabIndex = 9;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(104, 128);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(36, 20);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Values.Text = "描述";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(151, 124);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(295, 78);
            this.txtDescription.TabIndex = 11;
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.Location = new System.Drawing.Point(78, 235);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.Size = new System.Drawing.Size(62, 20);
            this.lblEffectiveDate.TabIndex = 12;
            this.lblEffectiveDate.Values.Text = "生效日期";
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.Location = new System.Drawing.Point(151, 235);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(132, 21);
            this.dtpEffectiveDate.TabIndex = 13;
            // 
            // lblExpiryDate
            // 
            this.lblExpiryDate.Location = new System.Drawing.Point(78, 283);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new System.Drawing.Size(62, 20);
            this.lblExpiryDate.TabIndex = 14;
            this.lblExpiryDate.Values.Text = "失效日期";
            // 
            // dtpExpiryDate
            // 
            this.dtpExpiryDate.Location = new System.Drawing.Point(151, 283);
            this.dtpExpiryDate.Name = "dtpExpiryDate";
            this.dtpExpiryDate.Size = new System.Drawing.Size(132, 21);
            this.dtpExpiryDate.TabIndex = 15;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(87, 358);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(373, 20);
            this.kryptonLabel1.TabIndex = 16;
            this.kryptonLabel1.Values.Text = "当前配置为在具体业务中的付款方式能指向到具体的收付账号。";
            // 
            // UCPayMethodAccountMapperEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 511);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCPayMethodAccountMapperEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPaytype_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccount_id)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
        private Krypton.Toolkit.KryptonComboBox cmbPaytype_ID;
        private Krypton.Toolkit.KryptonLabel lblAccount_id;
        private Krypton.Toolkit.KryptonComboBox cmbAccount_id;
        private Krypton.Toolkit.KryptonLabel lblDescription;
        private Krypton.Toolkit.KryptonTextBox txtDescription;
        private Krypton.Toolkit.KryptonLabel lblEffectiveDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;
        private Krypton.Toolkit.KryptonLabel lblExpiryDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpExpiryDate;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
    }
}
