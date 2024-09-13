using SqlSugar;

namespace RUINORERP.UI.BI
{
    partial class UCSysGlobalDynamicConfigEdit
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
            this.txtConfigValue = new Krypton.Toolkit.KryptonTextBox();
            this.lblConfigKey = new Krypton.Toolkit.KryptonLabel();
            this.txtConfigKey = new Krypton.Toolkit.KryptonTextBox();
            this.lblConfigValue = new Krypton.Toolkit.KryptonLabel();
            this.txtDescription = new Krypton.Toolkit.KryptonTextBox();
            this.lblDescription = new Krypton.Toolkit.KryptonLabel();
            this.lblValueType = new Krypton.Toolkit.KryptonLabel();
            this.lblConfigType = new Krypton.Toolkit.KryptonLabel();
            this.txtConfigType = new Krypton.Toolkit.KryptonTextBox();
            this.lblIsActive = new Krypton.Toolkit.KryptonLabel();
            this.chkIsActive = new Krypton.Toolkit.KryptonCheckBox();
            this.cmbValueType = new Krypton.Toolkit.KryptonComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbValueType)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(213, 476);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(376, 476);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.cmbValueType);
            this.kryptonPanel1.Controls.Add(this.txtConfigValue);
            this.kryptonPanel1.Controls.Add(this.lblConfigKey);
            this.kryptonPanel1.Controls.Add(this.txtConfigKey);
            this.kryptonPanel1.Controls.Add(this.lblConfigValue);
            this.kryptonPanel1.Controls.Add(this.txtDescription);
            this.kryptonPanel1.Controls.Add(this.lblDescription);
            this.kryptonPanel1.Controls.Add(this.lblValueType);
            this.kryptonPanel1.Controls.Add(this.lblConfigType);
            this.kryptonPanel1.Controls.Add(this.txtConfigType);
            this.kryptonPanel1.Controls.Add(this.lblIsActive);
            this.kryptonPanel1.Controls.Add(this.chkIsActive);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(682, 522);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // txtConfigValue
            // 
            this.txtConfigValue.Location = new System.Drawing.Point(129, 97);
            this.txtConfigValue.Multiline = true;
            this.txtConfigValue.Name = "txtConfigValue";
            this.txtConfigValue.Size = new System.Drawing.Size(472, 23);
            this.txtConfigValue.TabIndex = 31;
            // 
            // lblConfigKey
            // 
            this.lblConfigKey.Location = new System.Drawing.Point(74, 66);
            this.lblConfigKey.Name = "lblConfigKey";
            this.lblConfigKey.Size = new System.Drawing.Size(49, 20);
            this.lblConfigKey.TabIndex = 11;
            this.lblConfigKey.Values.Text = "配置项";
            // 
            // txtConfigKey
            // 
            this.txtConfigKey.Location = new System.Drawing.Point(129, 65);
            this.txtConfigKey.Multiline = true;
            this.txtConfigKey.Name = "txtConfigKey";
            this.txtConfigKey.Size = new System.Drawing.Size(472, 21);
            this.txtConfigKey.TabIndex = 12;
            // 
            // lblConfigValue
            // 
            this.lblConfigValue.Location = new System.Drawing.Point(74, 100);
            this.lblConfigValue.Name = "lblConfigValue";
            this.lblConfigValue.Size = new System.Drawing.Size(49, 20);
            this.lblConfigValue.TabIndex = 13;
            this.lblConfigValue.Values.Text = "配置值";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(129, 196);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(472, 211);
            this.txtDescription.TabIndex = 14;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(61, 196);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(62, 20);
            this.lblDescription.TabIndex = 15;
            this.lblDescription.Values.Text = "配置描述";
            // 
            // lblValueType
            // 
            this.lblValueType.Location = new System.Drawing.Point(22, 130);
            this.lblValueType.Name = "lblValueType";
            this.lblValueType.Size = new System.Drawing.Size(101, 20);
            this.lblValueType.TabIndex = 17;
            this.lblValueType.Values.Text = "配置项的值类型";
            // 
            // lblConfigType
            // 
            this.lblConfigType.Location = new System.Drawing.Point(61, 34);
            this.lblConfigType.Name = "lblConfigType";
            this.lblConfigType.Size = new System.Drawing.Size(62, 20);
            this.lblConfigType.TabIndex = 20;
            this.lblConfigType.Values.Text = "配置类型";
            // 
            // txtConfigType
            // 
            this.txtConfigType.Location = new System.Drawing.Point(129, 31);
            this.txtConfigType.Name = "txtConfigType";
            this.txtConfigType.Size = new System.Drawing.Size(472, 23);
            this.txtConfigType.TabIndex = 19;
            // 
            // lblIsActive
            // 
            this.lblIsActive.Location = new System.Drawing.Point(87, 165);
            this.lblIsActive.Name = "lblIsActive";
            this.lblIsActive.Size = new System.Drawing.Size(36, 20);
            this.lblIsActive.TabIndex = 21;
            this.lblIsActive.Values.Text = "启用";
            // 
            // chkIsActive
            // 
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Location = new System.Drawing.Point(129, 165);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(19, 13);
            this.chkIsActive.TabIndex = 22;
            this.chkIsActive.Values.Text = "";
            // 
            // cmbValueType
            // 
            this.cmbValueType.DropDownWidth = 100;
            this.cmbValueType.IntegralHeight = false;
            this.cmbValueType.Location = new System.Drawing.Point(129, 130);
            this.cmbValueType.Name = "cmbValueType";
            this.cmbValueType.Size = new System.Drawing.Size(472, 21);
            this.cmbValueType.TabIndex = 74;
            // 
            // UCSysGlobalDynamicConfigEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 522);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCSysGlobalDynamicConfigEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbValueType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonTextBox txtConfigValue;
        private Krypton.Toolkit.KryptonLabel lblConfigKey;
        private Krypton.Toolkit.KryptonTextBox txtConfigKey;
        private Krypton.Toolkit.KryptonLabel lblConfigValue;
        private Krypton.Toolkit.KryptonTextBox txtDescription;
        private Krypton.Toolkit.KryptonLabel lblDescription;
        private Krypton.Toolkit.KryptonLabel lblValueType;
        private Krypton.Toolkit.KryptonLabel lblConfigType;
        private Krypton.Toolkit.KryptonTextBox txtConfigType;
        private Krypton.Toolkit.KryptonLabel lblIsActive;
        private Krypton.Toolkit.KryptonCheckBox chkIsActive;
        private Krypton.Toolkit.KryptonComboBox cmbValueType;
    }
}
