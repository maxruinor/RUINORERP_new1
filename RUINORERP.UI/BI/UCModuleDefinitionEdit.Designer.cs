namespace RUINORERP.UI.BI
{
    partial class UCModuleDefinitionEdit
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
            this.lblModuleNo = new Krypton.Toolkit.KryptonLabel();
            this.txtModuleNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblModuleName = new Krypton.Toolkit.KryptonLabel();
            this.txtModuleName = new Krypton.Toolkit.KryptonTextBox();
            this.lblVisible = new Krypton.Toolkit.KryptonLabel();
            this.chkVisible = new Krypton.Toolkit.KryptonCheckBox();
            this.lblAvailable = new Krypton.Toolkit.KryptonLabel();
            this.chkAvailable = new Krypton.Toolkit.KryptonCheckBox();
            this.lblIconFile_Path = new Krypton.Toolkit.KryptonLabel();
            this.txtIconFile_Path = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonButton1 = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(152, 254);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(270, 254);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonButton1);
            this.kryptonPanel1.Controls.Add(this.lblModuleNo);
            this.kryptonPanel1.Controls.Add(this.txtModuleNo);
            this.kryptonPanel1.Controls.Add(this.lblModuleName);
            this.kryptonPanel1.Controls.Add(this.txtModuleName);
            this.kryptonPanel1.Controls.Add(this.lblVisible);
            this.kryptonPanel1.Controls.Add(this.chkVisible);
            this.kryptonPanel1.Controls.Add(this.lblAvailable);
            this.kryptonPanel1.Controls.Add(this.chkAvailable);
            this.kryptonPanel1.Controls.Add(this.lblIconFile_Path);
            this.kryptonPanel1.Controls.Add(this.txtIconFile_Path);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(573, 407);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // lblModuleNo
            // 
            this.lblModuleNo.Location = new System.Drawing.Point(106, 88);
            this.lblModuleNo.Name = "lblModuleNo";
            this.lblModuleNo.Size = new System.Drawing.Size(62, 20);
            this.lblModuleNo.TabIndex = 6;
            this.lblModuleNo.Values.Text = "模块编号";
            // 
            // txtModuleNo
            // 
            this.txtModuleNo.Location = new System.Drawing.Point(179, 84);
            this.txtModuleNo.Name = "txtModuleNo";
            this.txtModuleNo.Size = new System.Drawing.Size(280, 23);
            this.txtModuleNo.TabIndex = 7;
            // 
            // lblModuleName
            // 
            this.lblModuleName.Location = new System.Drawing.Point(106, 117);
            this.lblModuleName.Name = "lblModuleName";
            this.lblModuleName.Size = new System.Drawing.Size(62, 20);
            this.lblModuleName.TabIndex = 8;
            this.lblModuleName.Values.Text = "模块名称";
            // 
            // txtModuleName
            // 
            this.txtModuleName.Location = new System.Drawing.Point(179, 113);
            this.txtModuleName.Name = "txtModuleName";
            this.txtModuleName.Size = new System.Drawing.Size(280, 23);
            this.txtModuleName.TabIndex = 9;
            // 
            // lblVisible
            // 
            this.lblVisible.Location = new System.Drawing.Point(106, 142);
            this.lblVisible.Name = "lblVisible";
            this.lblVisible.Size = new System.Drawing.Size(62, 20);
            this.lblVisible.TabIndex = 10;
            this.lblVisible.Values.Text = "是否可见";
            // 
            // chkVisible
            // 
            this.chkVisible.Location = new System.Drawing.Point(179, 138);
            this.chkVisible.Name = "chkVisible";
            this.chkVisible.Size = new System.Drawing.Size(19, 13);
            this.chkVisible.TabIndex = 11;
            this.chkVisible.Values.Text = "";
            // 
            // lblAvailable
            // 
            this.lblAvailable.Location = new System.Drawing.Point(106, 167);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(62, 20);
            this.lblAvailable.TabIndex = 12;
            this.lblAvailable.Values.Text = "是否可用";
            // 
            // chkAvailable
            // 
            this.chkAvailable.Location = new System.Drawing.Point(179, 163);
            this.chkAvailable.Name = "chkAvailable";
            this.chkAvailable.Size = new System.Drawing.Size(19, 13);
            this.chkAvailable.TabIndex = 13;
            this.chkAvailable.Values.Text = "";
            // 
            // lblIconFile_Path
            // 
            this.lblIconFile_Path.Location = new System.Drawing.Point(106, 192);
            this.lblIconFile_Path.Name = "lblIconFile_Path";
            this.lblIconFile_Path.Size = new System.Drawing.Size(62, 20);
            this.lblIconFile_Path.TabIndex = 14;
            this.lblIconFile_Path.Values.Text = "图标路径";
            // 
            // txtIconFile_Path
            // 
            this.txtIconFile_Path.Location = new System.Drawing.Point(179, 188);
            this.txtIconFile_Path.Name = "txtIconFile_Path";
            this.txtIconFile_Path.Size = new System.Drawing.Size(280, 23);
            this.txtIconFile_Path.TabIndex = 15;
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(471, 188);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(35, 24);
            this.kryptonButton1.TabIndex = 16;
            this.kryptonButton1.Values.Text = "...";
            // 
            // UCModuleDefinitionEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 407);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCModuleDefinitionEdit";
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
        private Krypton.Toolkit.KryptonLabel lblModuleNo;
        private Krypton.Toolkit.KryptonTextBox txtModuleNo;
        private Krypton.Toolkit.KryptonLabel lblModuleName;
        private Krypton.Toolkit.KryptonTextBox txtModuleName;
        private Krypton.Toolkit.KryptonLabel lblVisible;
        private Krypton.Toolkit.KryptonCheckBox chkVisible;
        private Krypton.Toolkit.KryptonLabel lblAvailable;
        private Krypton.Toolkit.KryptonCheckBox chkAvailable;
        private Krypton.Toolkit.KryptonLabel lblIconFile_Path;
        private Krypton.Toolkit.KryptonTextBox txtIconFile_Path;
        private Krypton.Toolkit.KryptonButton kryptonButton1;
    }
}
