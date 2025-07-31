namespace RUINORERP.UI.BI
{
    partial class UCFMAuditLogsEdit
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
            this.txtActionTime = new Krypton.Toolkit.KryptonTextBox();
            this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblUserName = new Krypton.Toolkit.KryptonLabel();
            this.txtUserName = new Krypton.Toolkit.KryptonTextBox();
            this.lblActionTime = new Krypton.Toolkit.KryptonLabel();
            this.lblActionType = new Krypton.Toolkit.KryptonLabel();
            this.txtActionType = new Krypton.Toolkit.KryptonTextBox();
            this.lblObjectType = new Krypton.Toolkit.KryptonLabel();
            this.txtObjectType = new Krypton.Toolkit.KryptonTextBox();
            this.lblObjectId = new Krypton.Toolkit.KryptonLabel();
            this.txtObjectId = new Krypton.Toolkit.KryptonTextBox();
            this.lblObjectNo = new Krypton.Toolkit.KryptonLabel();
            this.txtObjectNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblOldState = new Krypton.Toolkit.KryptonLabel();
            this.txtOldState = new Krypton.Toolkit.KryptonTextBox();
            this.lblNewState = new Krypton.Toolkit.KryptonLabel();
            this.txtNewState = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            this.lblDataContent = new Krypton.Toolkit.KryptonLabel();
            this.auditLogViewer1 = new RUINORERP.UI.Monitoring.Auditing.JsonViewer();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.auditLogViewer1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(278, 720);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(525, 720);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.auditLogViewer1);
            this.kryptonPanel1.Controls.Add(this.txtActionTime);
            this.kryptonPanel1.Controls.Add(this.lblEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.cmbEmployee_ID);
            this.kryptonPanel1.Controls.Add(this.lblUserName);
            this.kryptonPanel1.Controls.Add(this.txtUserName);
            this.kryptonPanel1.Controls.Add(this.lblActionTime);
            this.kryptonPanel1.Controls.Add(this.lblActionType);
            this.kryptonPanel1.Controls.Add(this.txtActionType);
            this.kryptonPanel1.Controls.Add(this.lblObjectType);
            this.kryptonPanel1.Controls.Add(this.txtObjectType);
            this.kryptonPanel1.Controls.Add(this.lblObjectId);
            this.kryptonPanel1.Controls.Add(this.txtObjectId);
            this.kryptonPanel1.Controls.Add(this.lblObjectNo);
            this.kryptonPanel1.Controls.Add(this.txtObjectNo);
            this.kryptonPanel1.Controls.Add(this.lblOldState);
            this.kryptonPanel1.Controls.Add(this.txtOldState);
            this.kryptonPanel1.Controls.Add(this.lblNewState);
            this.kryptonPanel1.Controls.Add(this.txtNewState);
            this.kryptonPanel1.Controls.Add(this.lblNotes);
            this.kryptonPanel1.Controls.Add(this.txtNotes);
            this.kryptonPanel1.Controls.Add(this.lblDataContent);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(977, 757);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // txtActionTime
            // 
            this.txtActionTime.Location = new System.Drawing.Point(817, 10);
            this.txtActionTime.Name = "txtActionTime";
            this.txtActionTime.Size = new System.Drawing.Size(131, 23);
            this.txtActionTime.TabIndex = 34;
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(6, 13);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(62, 20);
            this.lblEmployee_ID.TabIndex = 12;
            this.lblEmployee_ID.Values.Text = "员工信息";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(79, 13);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(160, 21);
            this.cmbEmployee_ID.TabIndex = 13;
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(6, 43);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(49, 20);
            this.lblUserName.TabIndex = 14;
            this.lblUserName.Values.Text = "用户名";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(79, 43);
            this.txtUserName.Multiline = true;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(160, 21);
            this.txtUserName.TabIndex = 15;
            // 
            // lblActionTime
            // 
            this.lblActionTime.Location = new System.Drawing.Point(744, 11);
            this.lblActionTime.Name = "lblActionTime";
            this.lblActionTime.Size = new System.Drawing.Size(62, 20);
            this.lblActionTime.TabIndex = 16;
            this.lblActionTime.Values.Text = "发生时间";
            // 
            // lblActionType
            // 
            this.lblActionType.Location = new System.Drawing.Point(744, 41);
            this.lblActionType.Name = "lblActionType";
            this.lblActionType.Size = new System.Drawing.Size(36, 20);
            this.lblActionType.TabIndex = 18;
            this.lblActionType.Values.Text = "动作";
            // 
            // txtActionType
            // 
            this.txtActionType.Location = new System.Drawing.Point(817, 40);
            this.txtActionType.Name = "txtActionType";
            this.txtActionType.Size = new System.Drawing.Size(131, 23);
            this.txtActionType.TabIndex = 19;
            // 
            // lblObjectType
            // 
            this.lblObjectType.Location = new System.Drawing.Point(362, 11);
            this.lblObjectType.Name = "lblObjectType";
            this.lblObjectType.Size = new System.Drawing.Size(62, 20);
            this.lblObjectType.TabIndex = 20;
            this.lblObjectType.Values.Text = "单据类型";
            // 
            // txtObjectType
            // 
            this.txtObjectType.Location = new System.Drawing.Point(435, 10);
            this.txtObjectType.Name = "txtObjectType";
            this.txtObjectType.Size = new System.Drawing.Size(160, 23);
            this.txtObjectType.TabIndex = 21;
            // 
            // lblObjectId
            // 
            this.lblObjectId.Location = new System.Drawing.Point(362, 41);
            this.lblObjectId.Name = "lblObjectId";
            this.lblObjectId.Size = new System.Drawing.Size(48, 20);
            this.lblObjectId.TabIndex = 23;
            this.lblObjectId.Values.Text = "单据ID";
            // 
            // txtObjectId
            // 
            this.txtObjectId.Location = new System.Drawing.Point(435, 40);
            this.txtObjectId.Name = "txtObjectId";
            this.txtObjectId.Size = new System.Drawing.Size(160, 23);
            this.txtObjectId.TabIndex = 22;
            // 
            // lblObjectNo
            // 
            this.lblObjectNo.Location = new System.Drawing.Point(6, 72);
            this.lblObjectNo.Name = "lblObjectNo";
            this.lblObjectNo.Size = new System.Drawing.Size(62, 20);
            this.lblObjectNo.TabIndex = 24;
            this.lblObjectNo.Values.Text = "单据编号";
            // 
            // txtObjectNo
            // 
            this.txtObjectNo.Location = new System.Drawing.Point(79, 71);
            this.txtObjectNo.Name = "txtObjectNo";
            this.txtObjectNo.Size = new System.Drawing.Size(160, 23);
            this.txtObjectNo.TabIndex = 25;
            // 
            // lblOldState
            // 
            this.lblOldState.Location = new System.Drawing.Point(362, 70);
            this.lblOldState.Name = "lblOldState";
            this.lblOldState.Size = new System.Drawing.Size(75, 20);
            this.lblOldState.TabIndex = 26;
            this.lblOldState.Values.Text = "操作前状态";
            // 
            // txtOldState
            // 
            this.txtOldState.Location = new System.Drawing.Point(435, 69);
            this.txtOldState.Name = "txtOldState";
            this.txtOldState.Size = new System.Drawing.Size(160, 23);
            this.txtOldState.TabIndex = 27;
            // 
            // lblNewState
            // 
            this.lblNewState.Location = new System.Drawing.Point(744, 70);
            this.lblNewState.Name = "lblNewState";
            this.lblNewState.Size = new System.Drawing.Size(75, 20);
            this.lblNewState.TabIndex = 28;
            this.lblNewState.Values.Text = "操作后状态";
            // 
            // txtNewState
            // 
            this.txtNewState.Location = new System.Drawing.Point(817, 69);
            this.txtNewState.Name = "txtNewState";
            this.txtNewState.Size = new System.Drawing.Size(131, 23);
            this.txtNewState.TabIndex = 29;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(6, 106);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(62, 20);
            this.lblNotes.TabIndex = 30;
            this.lblNotes.Values.Text = "备注说明";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(79, 102);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(869, 98);
            this.txtNotes.TabIndex = 31;
            // 
            // lblDataContent
            // 
            this.lblDataContent.Location = new System.Drawing.Point(6, 210);
            this.lblDataContent.Name = "lblDataContent";
            this.lblDataContent.Size = new System.Drawing.Size(62, 20);
            this.lblDataContent.TabIndex = 32;
            this.lblDataContent.Values.Text = "数据内容";
            // 
            // auditLogViewer1
            // 
            this.auditLogViewer1.Location = new System.Drawing.Point(79, 210);
            this.auditLogViewer1.Name = "auditLogViewer1";
            this.auditLogViewer1.Size = new System.Drawing.Size(849, 504);
            this.auditLogViewer1.TabIndex = 35;
            // 
            // UCAuditLogsEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 757);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCAuditLogsEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.auditLogViewer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
        private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
        private Krypton.Toolkit.KryptonLabel lblUserName;
        private Krypton.Toolkit.KryptonTextBox txtUserName;
        private Krypton.Toolkit.KryptonLabel lblActionTime;
        private Krypton.Toolkit.KryptonLabel lblActionType;
        private Krypton.Toolkit.KryptonTextBox txtActionType;
        private Krypton.Toolkit.KryptonLabel lblObjectType;
        private Krypton.Toolkit.KryptonTextBox txtObjectType;
        private Krypton.Toolkit.KryptonLabel lblObjectId;
        private Krypton.Toolkit.KryptonTextBox txtObjectId;
        private Krypton.Toolkit.KryptonLabel lblObjectNo;
        private Krypton.Toolkit.KryptonTextBox txtObjectNo;
        private Krypton.Toolkit.KryptonLabel lblOldState;
        private Krypton.Toolkit.KryptonTextBox txtOldState;
        private Krypton.Toolkit.KryptonLabel lblNewState;
        private Krypton.Toolkit.KryptonTextBox txtNewState;
        private Krypton.Toolkit.KryptonLabel lblNotes;
        private Krypton.Toolkit.KryptonTextBox txtNotes;
        private Krypton.Toolkit.KryptonLabel lblDataContent;
        private Krypton.Toolkit.KryptonTextBox txtActionTime;
        private Monitoring.Auditing.JsonViewer auditLogViewer1;
    }
}
