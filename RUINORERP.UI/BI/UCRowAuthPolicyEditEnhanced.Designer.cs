namespace RUINORERP.UI.BI
{
    partial class UCRowAuthPolicyEditEnhanced
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
            this.txtPolicyDescription = new Krypton.Toolkit.KryptonTextBox();
            this.lblPolicyDescription = new Krypton.Toolkit.KryptonLabel();
            this.chkIsEnabled = new Krypton.Toolkit.KryptonCheckBox();
            this.txtEntityType = new Krypton.Toolkit.KryptonTextBox();
            this.lblEntityType = new Krypton.Toolkit.KryptonLabel();
            this.chkIsJoinRequired = new Krypton.Toolkit.KryptonCheckBox();
            this.cmbBizType = new Krypton.Toolkit.KryptonComboBox();
            this.lblBizType = new Krypton.Toolkit.KryptonLabel();
            this.cmbTargetTable = new Krypton.Toolkit.KryptonComboBox();
            this.lblSmartTargetTable = new Krypton.Toolkit.KryptonLabel();
            this.grpJoinTable = new Krypton.Toolkit.KryptonGroupBox();
            this.cmbJoinTable = new Krypton.Toolkit.KryptonComboBox();
            this.cmbJoinField = new Krypton.Toolkit.KryptonComboBox();
            this.lblJoinField = new Krypton.Toolkit.KryptonLabel();
            this.lblJoinTable = new Krypton.Toolkit.KryptonLabel();
            this.txtJoinType = new Krypton.Toolkit.KryptonTextBox();
            this.txtFilterClause = new Krypton.Toolkit.KryptonTextBox();
            this.lblJoinType = new Krypton.Toolkit.KryptonLabel();
            this.lblFilterClause = new Krypton.Toolkit.KryptonLabel();
            this.lblJoinOnClause = new Krypton.Toolkit.KryptonLabel();
            this.txtJoinOnClause = new Krypton.Toolkit.KryptonTextBox();
            this.grpFilterCondition = new Krypton.Toolkit.KryptonGroupBox();
            this.btnGenerateFilterClause = new Krypton.Toolkit.KryptonButton();
            this.txtFilterValue = new Krypton.Toolkit.KryptonTextBox();
            this.lblFilterValue = new Krypton.Toolkit.KryptonLabel();
            this.cmbOperator = new Krypton.Toolkit.KryptonComboBox();
            this.lblOperator = new Krypton.Toolkit.KryptonLabel();
            this.cmbFilterField = new Krypton.Toolkit.KryptonComboBox();
            this.lblFilterField = new Krypton.Toolkit.KryptonLabel();
            this.txtPolicyName = new Krypton.Toolkit.KryptonTextBox();
            this.lblPolicyName = new Krypton.Toolkit.KryptonLabel();
            this.lblTargetTable = new Krypton.Toolkit.KryptonLabel();
            this.txtTargetTable = new Krypton.Toolkit.KryptonTextBox();
            this.lblTargetEntity = new Krypton.Toolkit.KryptonLabel();
            this.txtTargetEntity = new Krypton.Toolkit.KryptonTextBox();
            this.txtJoinTable = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBizType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTargetTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpJoinTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpJoinTable.Panel)).BeginInit();
            this.grpJoinTable.Panel.SuspendLayout();
            this.grpJoinTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJoinTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJoinField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpFilterCondition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpFilterCondition.Panel)).BeginInit();
            this.grpFilterCondition.Panel.SuspendLayout();
            this.grpFilterCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOperator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFilterField)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(250, 642);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(368, 642);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.txtJoinTable);
            this.kryptonPanel1.Controls.Add(this.txtPolicyDescription);
            this.kryptonPanel1.Controls.Add(this.lblPolicyDescription);
            this.kryptonPanel1.Controls.Add(this.chkIsEnabled);
            this.kryptonPanel1.Controls.Add(this.txtEntityType);
            this.kryptonPanel1.Controls.Add(this.lblEntityType);
            this.kryptonPanel1.Controls.Add(this.chkIsJoinRequired);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Controls.Add(this.cmbBizType);
            this.kryptonPanel1.Controls.Add(this.lblBizType);
            this.kryptonPanel1.Controls.Add(this.cmbTargetTable);
            this.kryptonPanel1.Controls.Add(this.lblSmartTargetTable);
            this.kryptonPanel1.Controls.Add(this.grpJoinTable);
            this.kryptonPanel1.Controls.Add(this.grpFilterCondition);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(748, 689);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // txtPolicyDescription
            // 
            this.txtPolicyDescription.Location = new System.Drawing.Point(144, 527);
            this.txtPolicyDescription.Multiline = true;
            this.txtPolicyDescription.Name = "txtPolicyDescription";
            this.txtPolicyDescription.Size = new System.Drawing.Size(416, 79);
            this.txtPolicyDescription.TabIndex = 22;
            // 
            // lblPolicyDescription
            // 
            this.lblPolicyDescription.Location = new System.Drawing.Point(74, 527);
            this.lblPolicyDescription.Name = "lblPolicyDescription";
            this.lblPolicyDescription.Size = new System.Drawing.Size(62, 20);
            this.lblPolicyDescription.TabIndex = 21;
            this.lblPolicyDescription.Values.Text = "规则描述";
            // 
            // chkIsEnabled
            // 
            this.chkIsEnabled.Location = new System.Drawing.Point(612, 642);
            this.chkIsEnabled.Name = "chkIsEnabled";
            this.chkIsEnabled.Size = new System.Drawing.Size(75, 20);
            this.chkIsEnabled.TabIndex = 20;
            this.chkIsEnabled.Values.Text = "是否启用";
            // 
            // txtEntityType
            // 
            this.txtEntityType.Location = new System.Drawing.Point(142, 612);
            this.txtEntityType.Name = "txtEntityType";
            this.txtEntityType.Size = new System.Drawing.Size(416, 23);
            this.txtEntityType.TabIndex = 19;
            // 
            // lblEntityType
            // 
            this.lblEntityType.Location = new System.Drawing.Point(35, 612);
            this.lblEntityType.Name = "lblEntityType";
            this.lblEntityType.Size = new System.Drawing.Size(101, 20);
            this.lblEntityType.TabIndex = 18;
            this.lblEntityType.Values.Text = "实体全限定类名";
            // 
            // chkIsJoinRequired
            // 
            this.chkIsJoinRequired.Location = new System.Drawing.Point(72, 267);
            this.chkIsJoinRequired.Name = "chkIsJoinRequired";
            this.chkIsJoinRequired.Size = new System.Drawing.Size(101, 20);
            this.chkIsJoinRequired.TabIndex = 9;
            this.chkIsJoinRequired.Values.Text = "是否需要联表";
            // 
            // cmbBizType
            // 
            this.cmbBizType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBizType.DropDownWidth = 416;
            this.cmbBizType.IntegralHeight = false;
            this.cmbBizType.Location = new System.Drawing.Point(142, 12);
            this.cmbBizType.Name = "cmbBizType";
            this.cmbBizType.Size = new System.Drawing.Size(223, 21);
            this.cmbBizType.TabIndex = 23;
            // 
            // lblBizType
            // 
            this.lblBizType.Location = new System.Drawing.Point(72, 12);
            this.lblBizType.Name = "lblBizType";
            this.lblBizType.Size = new System.Drawing.Size(62, 20);
            this.lblBizType.TabIndex = 24;
            this.lblBizType.Values.Text = "业务类型";
            // 
            // cmbTargetTable
            // 
            this.cmbTargetTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetTable.DropDownWidth = 416;
            this.cmbTargetTable.IntegralHeight = false;
            this.cmbTargetTable.Location = new System.Drawing.Point(477, 12);
            this.cmbTargetTable.Name = "cmbTargetTable";
            this.cmbTargetTable.Size = new System.Drawing.Size(210, 21);
            this.cmbTargetTable.TabIndex = 25;
            // 
            // lblSmartTargetTable
            // 
            this.lblSmartTargetTable.Location = new System.Drawing.Point(407, 12);
            this.lblSmartTargetTable.Name = "lblSmartTargetTable";
            this.lblSmartTargetTable.Size = new System.Drawing.Size(49, 20);
            this.lblSmartTargetTable.TabIndex = 26;
            this.lblSmartTargetTable.Values.Text = "选择表";
            // 
            // grpJoinTable
            // 
            this.grpJoinTable.Location = new System.Drawing.Point(72, 293);
            this.grpJoinTable.Name = "grpJoinTable";
            // 
            // grpJoinTable.Panel
            // 
            this.grpJoinTable.Panel.Controls.Add(this.cmbJoinTable);
            this.grpJoinTable.Panel.Controls.Add(this.cmbJoinField);
            this.grpJoinTable.Panel.Controls.Add(this.lblJoinField);
            this.grpJoinTable.Panel.Controls.Add(this.lblJoinTable);
            this.grpJoinTable.Panel.Controls.Add(this.txtJoinType);
            this.grpJoinTable.Panel.Controls.Add(this.txtFilterClause);
            this.grpJoinTable.Panel.Controls.Add(this.lblJoinType);
            this.grpJoinTable.Panel.Controls.Add(this.lblFilterClause);
            this.grpJoinTable.Panel.Controls.Add(this.lblJoinOnClause);
            this.grpJoinTable.Panel.Controls.Add(this.txtJoinOnClause);
            this.grpJoinTable.Size = new System.Drawing.Size(530, 214);
            this.grpJoinTable.TabIndex = 27;
            this.grpJoinTable.Values.Heading = "联表配置";
            // 
            // cmbJoinTable
            // 
            this.cmbJoinTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJoinTable.DropDownWidth = 416;
            this.cmbJoinTable.IntegralHeight = false;
            this.cmbJoinTable.Location = new System.Drawing.Point(84, 14);
            this.cmbJoinTable.Name = "cmbJoinTable";
            this.cmbJoinTable.Size = new System.Drawing.Size(416, 21);
            this.cmbJoinTable.TabIndex = 31;
            // 
            // cmbJoinField
            // 
            this.cmbJoinField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJoinField.DropDownWidth = 416;
            this.cmbJoinField.IntegralHeight = false;
            this.cmbJoinField.Location = new System.Drawing.Point(84, 153);
            this.cmbJoinField.Name = "cmbJoinField";
            this.cmbJoinField.Size = new System.Drawing.Size(416, 21);
            this.cmbJoinField.TabIndex = 28;
            // 
            // lblJoinField
            // 
            this.lblJoinField.Location = new System.Drawing.Point(24, 153);
            this.lblJoinField.Name = "lblJoinField";
            this.lblJoinField.Size = new System.Drawing.Size(62, 20);
            this.lblJoinField.TabIndex = 29;
            this.lblJoinField.Values.Text = "关联字段";
            // 
            // lblJoinTable
            // 
            this.lblJoinTable.Location = new System.Drawing.Point(14, 14);
            this.lblJoinTable.Name = "lblJoinTable";
            this.lblJoinTable.Size = new System.Drawing.Size(62, 20);
            this.lblJoinTable.TabIndex = 10;
            this.lblJoinTable.Values.Text = "关联表名";
            // 
            // txtJoinType
            // 
            this.txtJoinType.Location = new System.Drawing.Point(84, 51);
            this.txtJoinType.Name = "txtJoinType";
            this.txtJoinType.Size = new System.Drawing.Size(416, 23);
            this.txtJoinType.TabIndex = 13;
            // 
            // txtFilterClause
            // 
            this.txtFilterClause.Location = new System.Drawing.Point(84, 115);
            this.txtFilterClause.Name = "txtFilterClause";
            this.txtFilterClause.Size = new System.Drawing.Size(416, 23);
            this.txtFilterClause.TabIndex = 17;
            // 
            // lblJoinType
            // 
            this.lblJoinType.Location = new System.Drawing.Point(14, 51);
            this.lblJoinType.Name = "lblJoinType";
            this.lblJoinType.Size = new System.Drawing.Size(62, 20);
            this.lblJoinType.TabIndex = 12;
            this.lblJoinType.Values.Text = "关联类型";
            // 
            // lblFilterClause
            // 
            this.lblFilterClause.Location = new System.Drawing.Point(14, 115);
            this.lblFilterClause.Name = "lblFilterClause";
            this.lblFilterClause.Size = new System.Drawing.Size(62, 20);
            this.lblFilterClause.TabIndex = 16;
            this.lblFilterClause.Values.Text = "过滤条件";
            // 
            // lblJoinOnClause
            // 
            this.lblJoinOnClause.Location = new System.Drawing.Point(14, 83);
            this.lblJoinOnClause.Name = "lblJoinOnClause";
            this.lblJoinOnClause.Size = new System.Drawing.Size(62, 20);
            this.lblJoinOnClause.TabIndex = 14;
            this.lblJoinOnClause.Values.Text = "关联条件";
            // 
            // txtJoinOnClause
            // 
            this.txtJoinOnClause.Location = new System.Drawing.Point(84, 83);
            this.txtJoinOnClause.Name = "txtJoinOnClause";
            this.txtJoinOnClause.Size = new System.Drawing.Size(416, 23);
            this.txtJoinOnClause.TabIndex = 15;
            // 
            // grpFilterCondition
            // 
            this.grpFilterCondition.Location = new System.Drawing.Point(72, 48);
            this.grpFilterCondition.Name = "grpFilterCondition";
            // 
            // grpFilterCondition.Panel
            // 
            this.grpFilterCondition.Panel.Controls.Add(this.btnGenerateFilterClause);
            this.grpFilterCondition.Panel.Controls.Add(this.txtFilterValue);
            this.grpFilterCondition.Panel.Controls.Add(this.lblFilterValue);
            this.grpFilterCondition.Panel.Controls.Add(this.cmbOperator);
            this.grpFilterCondition.Panel.Controls.Add(this.lblOperator);
            this.grpFilterCondition.Panel.Controls.Add(this.cmbFilterField);
            this.grpFilterCondition.Panel.Controls.Add(this.lblFilterField);
            this.grpFilterCondition.Panel.Controls.Add(this.txtPolicyName);
            this.grpFilterCondition.Panel.Controls.Add(this.lblPolicyName);
            this.grpFilterCondition.Panel.Controls.Add(this.lblTargetTable);
            this.grpFilterCondition.Panel.Controls.Add(this.txtTargetTable);
            this.grpFilterCondition.Panel.Controls.Add(this.lblTargetEntity);
            this.grpFilterCondition.Panel.Controls.Add(this.txtTargetEntity);
            this.grpFilterCondition.Size = new System.Drawing.Size(615, 200);
            this.grpFilterCondition.TabIndex = 30;
            this.grpFilterCondition.Values.Heading = "过滤条件生成器";
            // 
            // btnGenerateFilterClause
            // 
            this.btnGenerateFilterClause.Location = new System.Drawing.Point(380, 47);
            this.btnGenerateFilterClause.Name = "btnGenerateFilterClause";
            this.btnGenerateFilterClause.Size = new System.Drawing.Size(106, 25);
            this.btnGenerateFilterClause.TabIndex = 36;
            this.btnGenerateFilterClause.Values.Text = "生成条件";
            // 
            // txtFilterValue
            // 
            this.txtFilterValue.Location = new System.Drawing.Point(250, 47);
            this.txtFilterValue.Name = "txtFilterValue";
            this.txtFilterValue.Size = new System.Drawing.Size(120, 23);
            this.txtFilterValue.TabIndex = 35;
            // 
            // lblFilterValue
            // 
            this.lblFilterValue.Location = new System.Drawing.Point(200, 47);
            this.lblFilterValue.Name = "lblFilterValue";
            this.lblFilterValue.Size = new System.Drawing.Size(23, 20);
            this.lblFilterValue.TabIndex = 34;
            this.lblFilterValue.Values.Text = "值";
            // 
            // cmbOperator
            // 
            this.cmbOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperator.DropDownWidth = 64;
            this.cmbOperator.IntegralHeight = false;
            this.cmbOperator.Items.AddRange(new object[] {
            "=",
            "<>",
            ">",
            "<",
            ">=",
            "<=",
            "LIKE",
            "IN"});
            this.cmbOperator.Location = new System.Drawing.Point(130, 47);
            this.cmbOperator.Name = "cmbOperator";
            this.cmbOperator.Size = new System.Drawing.Size(64, 21);
            this.cmbOperator.TabIndex = 33;
            this.cmbOperator.Text = "=";
            // 
            // lblOperator
            // 
            this.lblOperator.Location = new System.Drawing.Point(80, 47);
            this.lblOperator.Name = "lblOperator";
            this.lblOperator.Size = new System.Drawing.Size(49, 20);
            this.lblOperator.TabIndex = 32;
            this.lblOperator.Values.Text = "操作符";
            // 
            // cmbFilterField
            // 
            this.cmbFilterField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterField.DropDownWidth = 416;
            this.cmbFilterField.IntegralHeight = false;
            this.cmbFilterField.Location = new System.Drawing.Point(70, 12);
            this.cmbFilterField.Name = "cmbFilterField";
            this.cmbFilterField.Size = new System.Drawing.Size(416, 21);
            this.cmbFilterField.TabIndex = 31;
            // 
            // lblFilterField
            // 
            this.lblFilterField.Location = new System.Drawing.Point(10, 12);
            this.lblFilterField.Name = "lblFilterField";
            this.lblFilterField.Size = new System.Drawing.Size(62, 20);
            this.lblFilterField.TabIndex = 28;
            this.lblFilterField.Values.Text = "过滤字段";
            // 
            // txtPolicyName
            // 
            this.txtPolicyName.Location = new System.Drawing.Point(80, 78);
            this.txtPolicyName.Name = "txtPolicyName";
            this.txtPolicyName.Size = new System.Drawing.Size(406, 23);
            this.txtPolicyName.TabIndex = 4;
            // 
            // lblPolicyName
            // 
            this.lblPolicyName.Location = new System.Drawing.Point(10, 78);
            this.lblPolicyName.Name = "lblPolicyName";
            this.lblPolicyName.Size = new System.Drawing.Size(62, 20);
            this.lblPolicyName.TabIndex = 3;
            this.lblPolicyName.Values.Text = "规则名称";
            // 
            // lblTargetTable
            // 
            this.lblTargetTable.Location = new System.Drawing.Point(10, 110);
            this.lblTargetTable.Name = "lblTargetTable";
            this.lblTargetTable.Size = new System.Drawing.Size(62, 20);
            this.lblTargetTable.TabIndex = 5;
            this.lblTargetTable.Values.Text = "查询主表";
            // 
            // txtTargetTable
            // 
            this.txtTargetTable.Location = new System.Drawing.Point(80, 110);
            this.txtTargetTable.Name = "txtTargetTable";
            this.txtTargetTable.Size = new System.Drawing.Size(143, 23);
            this.txtTargetTable.TabIndex = 6;
            // 
            // lblTargetEntity
            // 
            this.lblTargetEntity.Location = new System.Drawing.Point(255, 110);
            this.lblTargetEntity.Name = "lblTargetEntity";
            this.lblTargetEntity.Size = new System.Drawing.Size(62, 20);
            this.lblTargetEntity.TabIndex = 7;
            this.lblTargetEntity.Values.Text = "查询实体";
            // 
            // txtTargetEntity
            // 
            this.txtTargetEntity.Location = new System.Drawing.Point(325, 110);
            this.txtTargetEntity.Name = "txtTargetEntity";
            this.txtTargetEntity.Size = new System.Drawing.Size(161, 23);
            this.txtTargetEntity.TabIndex = 8;
            // 
            // txtJoinTable
            // 
            this.txtJoinTable.Location = new System.Drawing.Point(179, 267);
            this.txtJoinTable.Name = "txtJoinTable";
            this.txtJoinTable.Size = new System.Drawing.Size(406, 23);
            this.txtJoinTable.TabIndex = 37;
            // 
            // UCRowAuthPolicyEditEnhanced
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 689);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCRowAuthPolicyEditEnhanced";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBizType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTargetTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpJoinTable.Panel)).EndInit();
            this.grpJoinTable.Panel.ResumeLayout(false);
            this.grpJoinTable.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpJoinTable)).EndInit();
            this.grpJoinTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbJoinTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJoinField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpFilterCondition.Panel)).EndInit();
            this.grpFilterCondition.Panel.ResumeLayout(false);
            this.grpFilterCondition.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpFilterCondition)).EndInit();
            this.grpFilterCondition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbOperator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFilterField)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonTextBox txtPolicyName;
        private Krypton.Toolkit.KryptonLabel lblPolicyName;
        private Krypton.Toolkit.KryptonTextBox txtPolicyDescription;
        private Krypton.Toolkit.KryptonLabel lblPolicyDescription;
        private Krypton.Toolkit.KryptonCheckBox chkIsEnabled;
        private Krypton.Toolkit.KryptonTextBox txtEntityType;
        private Krypton.Toolkit.KryptonLabel lblEntityType;
        private Krypton.Toolkit.KryptonTextBox txtFilterClause;
        private Krypton.Toolkit.KryptonLabel lblFilterClause;
        private Krypton.Toolkit.KryptonTextBox txtJoinOnClause;
        private Krypton.Toolkit.KryptonLabel lblJoinOnClause;
        private Krypton.Toolkit.KryptonTextBox txtJoinType;
        private Krypton.Toolkit.KryptonLabel lblJoinType;
        private Krypton.Toolkit.KryptonLabel lblJoinTable;
        private Krypton.Toolkit.KryptonCheckBox chkIsJoinRequired;
        private Krypton.Toolkit.KryptonTextBox txtTargetEntity;
        private Krypton.Toolkit.KryptonLabel lblTargetEntity;
        private Krypton.Toolkit.KryptonTextBox txtTargetTable;
        private Krypton.Toolkit.KryptonLabel lblTargetTable;
        // 智能UI组件
        private Krypton.Toolkit.KryptonComboBox cmbBizType;
        private Krypton.Toolkit.KryptonLabel lblBizType;
        private Krypton.Toolkit.KryptonComboBox cmbTargetTable;
        private Krypton.Toolkit.KryptonLabel lblSmartTargetTable;
        private Krypton.Toolkit.KryptonGroupBox grpJoinTable;
        private Krypton.Toolkit.KryptonComboBox cmbJoinField;
        private Krypton.Toolkit.KryptonLabel lblJoinField;
        private Krypton.Toolkit.KryptonGroupBox grpFilterCondition;
        private Krypton.Toolkit.KryptonButton btnGenerateFilterClause;
        private Krypton.Toolkit.KryptonTextBox txtFilterValue;
        private Krypton.Toolkit.KryptonLabel lblFilterValue;
        private Krypton.Toolkit.KryptonComboBox cmbOperator;
        private Krypton.Toolkit.KryptonLabel lblOperator;
        private Krypton.Toolkit.KryptonComboBox cmbFilterField;
        private Krypton.Toolkit.KryptonLabel lblFilterField;
        private Krypton.Toolkit.KryptonComboBox cmbJoinTable;
        private Krypton.Toolkit.KryptonTextBox txtJoinTable;
    }
}