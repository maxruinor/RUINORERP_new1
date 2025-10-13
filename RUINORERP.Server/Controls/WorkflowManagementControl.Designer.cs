namespace RUINORERP.Server.Controls
{
    partial class WorkflowManagementControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.groupBoxWorkflowList = new System.Windows.Forms.GroupBox();
            this.dataGridViewWorkflows = new System.Windows.Forms.DataGridView();
            this.groupBoxWorkflowActions = new System.Windows.Forms.GroupBox();
            this.btnClearCompleted = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabControlWorkflowTests = new System.Windows.Forms.TabControl();
            this.tabPageApproval = new System.Windows.Forms.TabPage();
            this.btnStartApprovalWorkflow = new System.Windows.Forms.Button();
            this.textBoxBillId = new System.Windows.Forms.TextBox();
            this.labelBillId = new System.Windows.Forms.Label();
            this.tabPageEvent = new System.Windows.Forms.TabPage();
            this.btnPublishEvent = new System.Windows.Forms.Button();
            this.textBoxWorkflowId = new System.Windows.Forms.TextBox();
            this.labelWorkflowId = new System.Windows.Forms.Label();
            this.textBoxEventName = new System.Windows.Forms.TextBox();
            this.labelEventName = new System.Windows.Forms.Label();
            this.tabPageOtherTests = new System.Windows.Forms.TabPage();
            this.btnCacheTest = new System.Windows.Forms.Button();
            this.btnPushTest = new System.Windows.Forms.Button();
            this.btnStartReminderWorkflow = new System.Windows.Forms.Button();
            this.btnStartSafetyStockWorkflow = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priorityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sendTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.senderIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.senderNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reminderContentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.billDataDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remindSubjectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remindTimesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remindIntervalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bizPrimaryKeyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bizTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workflowIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reminderTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.messageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.groupBoxWorkflowList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWorkflows)).BeginInit();
            this.groupBoxWorkflowActions.SuspendLayout();
            this.tabControlWorkflowTests.SuspendLayout();
            this.tabPageApproval.SuspendLayout();
            this.tabPageEvent.SuspendLayout();
            this.tabPageOtherTests.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.groupBoxWorkflowList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.groupBoxWorkflowActions);
            this.splitContainerMain.Size = new System.Drawing.Size(800, 600);
            this.splitContainerMain.SplitterDistance = 350;
            this.splitContainerMain.TabIndex = 0;
            // 
            // groupBoxWorkflowList
            // 
            this.groupBoxWorkflowList.Controls.Add(this.dataGridViewWorkflows);
            this.groupBoxWorkflowList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxWorkflowList.Location = new System.Drawing.Point(0, 0);
            this.groupBoxWorkflowList.Name = "groupBoxWorkflowList";
            this.groupBoxWorkflowList.Size = new System.Drawing.Size(800, 350);
            this.groupBoxWorkflowList.TabIndex = 0;
            this.groupBoxWorkflowList.TabStop = false;
            this.groupBoxWorkflowList.Text = "工作流列表";
            // 
            // dataGridViewWorkflows
            // 
            this.dataGridViewWorkflows.AllowUserToAddRows = false;
            this.dataGridViewWorkflows.AllowUserToOrderColumns = true;
            this.dataGridViewWorkflows.AutoGenerateColumns = false;
            this.dataGridViewWorkflows.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewWorkflows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWorkflows.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.statusDataGridViewTextBoxColumn,
            this.priorityDataGridViewTextBoxColumn,
            this.sendTimeDataGridViewTextBoxColumn,
            this.senderIDDataGridViewTextBoxColumn,
            this.senderNameDataGridViewTextBoxColumn,
            this.reminderContentDataGridViewTextBoxColumn,
            this.entityTypeDataGridViewTextBoxColumn,
            this.billDataDataGridViewTextBoxColumn,
            this.startTimeDataGridViewTextBoxColumn,
            this.endTimeDataGridViewTextBoxColumn,
            this.remindSubjectDataGridViewTextBoxColumn,
            this.remindTimesDataGridViewTextBoxColumn,
            this.remindIntervalDataGridViewTextBoxColumn,
            this.idDataGridViewTextBoxColumn,
            this.bizPrimaryKeyDataGridViewTextBoxColumn,
            this.bizTypeDataGridViewTextBoxColumn,
            this.workflowIdDataGridViewTextBoxColumn,
            this.reminderTimeDataGridViewTextBoxColumn,
            this.messageDataGridViewTextBoxColumn});
            this.dataGridViewWorkflows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewWorkflows.Location = new System.Drawing.Point(3, 19);
            this.dataGridViewWorkflows.Name = "dataGridViewWorkflows";
            this.dataGridViewWorkflows.Size = new System.Drawing.Size(794, 328);
            this.dataGridViewWorkflows.TabIndex = 0;
            this.dataGridViewWorkflows.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewWorkflows_CellDoubleClick);
            this.dataGridViewWorkflows.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewWorkflows_DataError);
            // 
            // groupBoxWorkflowActions
            // 
            this.groupBoxWorkflowActions.Controls.Add(this.btnClearCompleted);
            this.groupBoxWorkflowActions.Controls.Add(this.btnRefresh);
            this.groupBoxWorkflowActions.Controls.Add(this.tabControlWorkflowTests);
            this.groupBoxWorkflowActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxWorkflowActions.Location = new System.Drawing.Point(0, 0);
            this.groupBoxWorkflowActions.Name = "groupBoxWorkflowActions";
            this.groupBoxWorkflowActions.Size = new System.Drawing.Size(800, 246);
            this.groupBoxWorkflowActions.TabIndex = 0;
            this.groupBoxWorkflowActions.TabStop = false;
            this.groupBoxWorkflowActions.Text = "工作流操作";
            // 
            // btnClearCompleted
            // 
            this.btnClearCompleted.Location = new System.Drawing.Point(93, 19);
            this.btnClearCompleted.Name = "btnClearCompleted";
            this.btnClearCompleted.Size = new System.Drawing.Size(84, 23);
            this.btnClearCompleted.TabIndex = 2;
            this.btnClearCompleted.Text = "清理已完成";
            this.btnClearCompleted.UseVisualStyleBackColor = true;
            this.btnClearCompleted.Click += new System.EventHandler(this.btnClearCompleted_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 19);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tabControlWorkflowTests
            // 
            this.tabControlWorkflowTests.Controls.Add(this.tabPageApproval);
            this.tabControlWorkflowTests.Controls.Add(this.tabPageEvent);
            this.tabControlWorkflowTests.Controls.Add(this.tabPageOtherTests);
            this.tabControlWorkflowTests.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControlWorkflowTests.Location = new System.Drawing.Point(3, 48);
            this.tabControlWorkflowTests.Name = "tabControlWorkflowTests";
            this.tabControlWorkflowTests.SelectedIndex = 0;
            this.tabControlWorkflowTests.Size = new System.Drawing.Size(794, 195);
            this.tabControlWorkflowTests.TabIndex = 0;
            // 
            // tabPageApproval
            // 
            this.tabPageApproval.Controls.Add(this.btnStartApprovalWorkflow);
            this.tabPageApproval.Controls.Add(this.textBoxBillId);
            this.tabPageApproval.Controls.Add(this.labelBillId);
            this.tabPageApproval.Location = new System.Drawing.Point(4, 22);
            this.tabPageApproval.Name = "tabPageApproval";
            this.tabPageApproval.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageApproval.Size = new System.Drawing.Size(786, 169);
            this.tabPageApproval.TabIndex = 0;
            this.tabPageApproval.Text = "审批工作流";
            this.tabPageApproval.UseVisualStyleBackColor = true;
            // 
            // btnStartApprovalWorkflow
            // 
            this.btnStartApprovalWorkflow.Location = new System.Drawing.Point(240, 20);
            this.btnStartApprovalWorkflow.Name = "btnStartApprovalWorkflow";
            this.btnStartApprovalWorkflow.Size = new System.Drawing.Size(100, 23);
            this.btnStartApprovalWorkflow.TabIndex = 2;
            this.btnStartApprovalWorkflow.Text = "启动审批工作流";
            this.btnStartApprovalWorkflow.UseVisualStyleBackColor = true;
            this.btnStartApprovalWorkflow.Click += new System.EventHandler(this.btnStartApprovalWorkflow_Click);
            // 
            // textBoxBillId
            // 
            this.textBoxBillId.Location = new System.Drawing.Point(70, 21);
            this.textBoxBillId.Name = "textBoxBillId";
            this.textBoxBillId.Size = new System.Drawing.Size(150, 21);
            this.textBoxBillId.TabIndex = 1;
            // 
            // labelBillId
            // 
            this.labelBillId.AutoSize = true;
            this.labelBillId.Location = new System.Drawing.Point(15, 24);
            this.labelBillId.Name = "labelBillId";
            this.labelBillId.Size = new System.Drawing.Size(49, 12);
            this.labelBillId.TabIndex = 0;
            this.labelBillId.Text = "单据ID:";
            // 
            // tabPageEvent
            // 
            this.tabPageEvent.Controls.Add(this.btnPublishEvent);
            this.tabPageEvent.Controls.Add(this.textBoxWorkflowId);
            this.tabPageEvent.Controls.Add(this.labelWorkflowId);
            this.tabPageEvent.Controls.Add(this.textBoxEventName);
            this.tabPageEvent.Controls.Add(this.labelEventName);
            this.tabPageEvent.Location = new System.Drawing.Point(4, 22);
            this.tabPageEvent.Name = "tabPageEvent";
            this.tabPageEvent.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvent.Size = new System.Drawing.Size(786, 169);
            this.tabPageEvent.TabIndex = 1;
            this.tabPageEvent.Text = "事件发布";
            this.tabPageEvent.UseVisualStyleBackColor = true;
            // 
            // btnPublishEvent
            // 
            this.btnPublishEvent.Location = new System.Drawing.Point(300, 50);
            this.btnPublishEvent.Name = "btnPublishEvent";
            this.btnPublishEvent.Size = new System.Drawing.Size(80, 23);
            this.btnPublishEvent.TabIndex = 4;
            this.btnPublishEvent.Text = "发布事件";
            this.btnPublishEvent.UseVisualStyleBackColor = true;
            this.btnPublishEvent.Click += new System.EventHandler(this.btnPublishEvent_Click);
            // 
            // textBoxWorkflowId
            // 
            this.textBoxWorkflowId.Location = new System.Drawing.Point(75, 52);
            this.textBoxWorkflowId.Name = "textBoxWorkflowId";
            this.textBoxWorkflowId.Size = new System.Drawing.Size(200, 21);
            this.textBoxWorkflowId.TabIndex = 3;
            // 
            // labelWorkflowId
            // 
            this.labelWorkflowId.AutoSize = true;
            this.labelWorkflowId.Location = new System.Drawing.Point(15, 55);
            this.labelWorkflowId.Name = "labelWorkflowId";
            this.labelWorkflowId.Size = new System.Drawing.Size(59, 12);
            this.labelWorkflowId.TabIndex = 2;
            this.labelWorkflowId.Text = "工作流ID:";
            // 
            // textBoxEventName
            // 
            this.textBoxEventName.Location = new System.Drawing.Point(75, 20);
            this.textBoxEventName.Name = "textBoxEventName";
            this.textBoxEventName.Size = new System.Drawing.Size(200, 21);
            this.textBoxEventName.TabIndex = 1;
            // 
            // labelEventName
            // 
            this.labelEventName.AutoSize = true;
            this.labelEventName.Location = new System.Drawing.Point(15, 23);
            this.labelEventName.Name = "labelEventName";
            this.labelEventName.Size = new System.Drawing.Size(59, 12);
            this.labelEventName.TabIndex = 0;
            this.labelEventName.Text = "事件名称:";
            // 
            // tabPageOtherTests
            // 
            this.tabPageOtherTests.Controls.Add(this.btnCacheTest);
            this.tabPageOtherTests.Controls.Add(this.btnPushTest);
            this.tabPageOtherTests.Controls.Add(this.btnStartReminderWorkflow);
            this.tabPageOtherTests.Controls.Add(this.btnStartSafetyStockWorkflow);
            this.tabPageOtherTests.Location = new System.Drawing.Point(4, 22);
            this.tabPageOtherTests.Name = "tabPageOtherTests";
            this.tabPageOtherTests.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOtherTests.Size = new System.Drawing.Size(786, 169);
            this.tabPageOtherTests.TabIndex = 2;
            this.tabPageOtherTests.Text = "其他测试";
            this.tabPageOtherTests.UseVisualStyleBackColor = true;
            // 
            // btnCacheTest
            // 
            this.btnCacheTest.Location = new System.Drawing.Point(320, 20);
            this.btnCacheTest.Name = "btnCacheTest";
            this.btnCacheTest.Size = new System.Drawing.Size(75, 23);
            this.btnCacheTest.TabIndex = 3;
            this.btnCacheTest.Text = "缓存测试";
            this.btnCacheTest.UseVisualStyleBackColor = true;
            this.btnCacheTest.Click += new System.EventHandler(this.btnCacheTest_Click);
            // 
            // btnPushTest
            // 
            this.btnPushTest.Location = new System.Drawing.Point(220, 20);
            this.btnPushTest.Name = "btnPushTest";
            this.btnPushTest.Size = new System.Drawing.Size(90, 23);
            this.btnPushTest.TabIndex = 2;
            this.btnPushTest.Text = "推送测试";
            this.btnPushTest.UseVisualStyleBackColor = true;
            this.btnPushTest.Click += new System.EventHandler(this.btnPushTest_Click);
            // 
            // btnStartReminderWorkflow
            // 
            this.btnStartReminderWorkflow.Location = new System.Drawing.Point(110, 20);
            this.btnStartReminderWorkflow.Name = "btnStartReminderWorkflow";
            this.btnStartReminderWorkflow.Size = new System.Drawing.Size(100, 23);
            this.btnStartReminderWorkflow.TabIndex = 1;
            this.btnStartReminderWorkflow.Text = "启动提醒工作流";
            this.btnStartReminderWorkflow.UseVisualStyleBackColor = true;
            this.btnStartReminderWorkflow.Click += new System.EventHandler(this.btnStartReminderWorkflow_Click);
            // 
            // btnStartSafetyStockWorkflow
            // 
            this.btnStartSafetyStockWorkflow.Location = new System.Drawing.Point(15, 20);
            this.btnStartSafetyStockWorkflow.Name = "btnStartSafetyStockWorkflow";
            this.btnStartSafetyStockWorkflow.Size = new System.Drawing.Size(85, 23);
            this.btnStartSafetyStockWorkflow.TabIndex = 0;
            this.btnStartSafetyStockWorkflow.Text = "安全库存计算";
            this.btnStartSafetyStockWorkflow.UseVisualStyleBackColor = true;
            this.btnStartSafetyStockWorkflow.Click += new System.EventHandler(this.btnStartSafetyStockWorkflow_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Status";
            this.dataGridViewTextBoxColumn1.HeaderText = "Status";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Priority";
            this.dataGridViewTextBoxColumn2.HeaderText = "Priority";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "SendTime";
            this.dataGridViewTextBoxColumn3.HeaderText = "SendTime";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "SenderID";
            this.dataGridViewTextBoxColumn4.HeaderText = "SenderID";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "SenderName";
            this.dataGridViewTextBoxColumn5.HeaderText = "SenderName";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "ReminderContent";
            this.dataGridViewTextBoxColumn6.HeaderText = "ReminderContent";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "EntityType";
            this.dataGridViewTextBoxColumn7.HeaderText = "EntityType";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "BillData";
            this.dataGridViewTextBoxColumn8.HeaderText = "BillData";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "StartTime";
            this.dataGridViewTextBoxColumn9.HeaderText = "StartTime";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "EndTime";
            this.dataGridViewTextBoxColumn10.HeaderText = "EndTime";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "RemindSubject";
            this.dataGridViewTextBoxColumn11.HeaderText = "RemindSubject";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "RemindTimes";
            this.dataGridViewTextBoxColumn12.HeaderText = "RemindTimes";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "RemindInterval";
            this.dataGridViewTextBoxColumn13.HeaderText = "RemindInterval";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn14.HeaderText = "Id";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "BizPrimaryKey";
            this.dataGridViewTextBoxColumn15.HeaderText = "BizPrimaryKey";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "BizType";
            this.dataGridViewTextBoxColumn16.HeaderText = "BizType";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "WorkflowId";
            this.dataGridViewTextBoxColumn17.HeaderText = "WorkflowId";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.DataPropertyName = "ReminderTime";
            this.dataGridViewTextBoxColumn18.HeaderText = "ReminderTime";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.DataPropertyName = "Message";
            this.dataGridViewTextBoxColumn19.HeaderText = "Message";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "状态";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            // 
            // priorityDataGridViewTextBoxColumn
            // 
            this.priorityDataGridViewTextBoxColumn.DataPropertyName = "Priority";
            this.priorityDataGridViewTextBoxColumn.HeaderText = "优先级";
            this.priorityDataGridViewTextBoxColumn.Name = "priorityDataGridViewTextBoxColumn";
            // 
            // sendTimeDataGridViewTextBoxColumn
            // 
            this.sendTimeDataGridViewTextBoxColumn.DataPropertyName = "SendTime";
            this.sendTimeDataGridViewTextBoxColumn.HeaderText = "发送时间";
            this.sendTimeDataGridViewTextBoxColumn.Name = "sendTimeDataGridViewTextBoxColumn";
            // 
            // senderIDDataGridViewTextBoxColumn
            // 
            this.senderIDDataGridViewTextBoxColumn.DataPropertyName = "SenderID";
            this.senderIDDataGridViewTextBoxColumn.HeaderText = "发送者ID";
            this.senderIDDataGridViewTextBoxColumn.Name = "senderIDDataGridViewTextBoxColumn";
            // 
            // senderNameDataGridViewTextBoxColumn
            // 
            this.senderNameDataGridViewTextBoxColumn.DataPropertyName = "SenderName";
            this.senderNameDataGridViewTextBoxColumn.HeaderText = "发送者名称";
            this.senderNameDataGridViewTextBoxColumn.Name = "senderNameDataGridViewTextBoxColumn";
            // 
            // reminderContentDataGridViewTextBoxColumn
            // 
            this.reminderContentDataGridViewTextBoxColumn.DataPropertyName = "ReminderContent";
            this.reminderContentDataGridViewTextBoxColumn.HeaderText = "提醒内容";
            this.reminderContentDataGridViewTextBoxColumn.Name = "reminderContentDataGridViewTextBoxColumn";
            // 
            // entityTypeDataGridViewTextBoxColumn
            // 
            this.entityTypeDataGridViewTextBoxColumn.DataPropertyName = "EntityType";
            this.entityTypeDataGridViewTextBoxColumn.HeaderText = "实体类型";
            this.entityTypeDataGridViewTextBoxColumn.Name = "entityTypeDataGridViewTextBoxColumn";
            // 
            // billDataDataGridViewTextBoxColumn
            // 
            this.billDataDataGridViewTextBoxColumn.DataPropertyName = "BillData";
            this.billDataDataGridViewTextBoxColumn.HeaderText = "单据数据";
            this.billDataDataGridViewTextBoxColumn.Name = "billDataDataGridViewTextBoxColumn";
            // 
            // startTimeDataGridViewTextBoxColumn
            // 
            this.startTimeDataGridViewTextBoxColumn.DataPropertyName = "StartTime";
            this.startTimeDataGridViewTextBoxColumn.HeaderText = "开始时间";
            this.startTimeDataGridViewTextBoxColumn.Name = "startTimeDataGridViewTextBoxColumn";
            // 
            // endTimeDataGridViewTextBoxColumn
            // 
            this.endTimeDataGridViewTextBoxColumn.DataPropertyName = "EndTime";
            this.endTimeDataGridViewTextBoxColumn.HeaderText = "结束时间";
            this.endTimeDataGridViewTextBoxColumn.Name = "endTimeDataGridViewTextBoxColumn";
            // 
            // remindSubjectDataGridViewTextBoxColumn
            // 
            this.remindSubjectDataGridViewTextBoxColumn.DataPropertyName = "RemindSubject";
            this.remindSubjectDataGridViewTextBoxColumn.HeaderText = "提醒主题";
            this.remindSubjectDataGridViewTextBoxColumn.Name = "remindSubjectDataGridViewTextBoxColumn";
            // 
            // remindTimesDataGridViewTextBoxColumn
            // 
            this.remindTimesDataGridViewTextBoxColumn.DataPropertyName = "RemindTimes";
            this.remindTimesDataGridViewTextBoxColumn.HeaderText = "提醒次数";
            this.remindTimesDataGridViewTextBoxColumn.Name = "remindTimesDataGridViewTextBoxColumn";
            // 
            // remindIntervalDataGridViewTextBoxColumn
            // 
            this.remindIntervalDataGridViewTextBoxColumn.DataPropertyName = "RemindInterval";
            this.remindIntervalDataGridViewTextBoxColumn.HeaderText = "提醒间隔";
            this.remindIntervalDataGridViewTextBoxColumn.Name = "remindIntervalDataGridViewTextBoxColumn";
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "ID";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            // 
            // bizPrimaryKeyDataGridViewTextBoxColumn
            // 
            this.bizPrimaryKeyDataGridViewTextBoxColumn.DataPropertyName = "BizPrimaryKey";
            this.bizPrimaryKeyDataGridViewTextBoxColumn.HeaderText = "业务主键";
            this.bizPrimaryKeyDataGridViewTextBoxColumn.Name = "bizPrimaryKeyDataGridViewTextBoxColumn";
            // 
            // bizTypeDataGridViewTextBoxColumn
            // 
            this.bizTypeDataGridViewTextBoxColumn.DataPropertyName = "BizType";
            this.bizTypeDataGridViewTextBoxColumn.HeaderText = "业务类型";
            this.bizTypeDataGridViewTextBoxColumn.Name = "bizTypeDataGridViewTextBoxColumn";
            // 
            // workflowIdDataGridViewTextBoxColumn
            // 
            this.workflowIdDataGridViewTextBoxColumn.DataPropertyName = "WorkflowId";
            this.workflowIdDataGridViewTextBoxColumn.HeaderText = "工作流ID";
            this.workflowIdDataGridViewTextBoxColumn.Name = "workflowIdDataGridViewTextBoxColumn";
            // 
            // reminderTimeDataGridViewTextBoxColumn
            // 
            this.reminderTimeDataGridViewTextBoxColumn.DataPropertyName = "ReminderTime";
            this.reminderTimeDataGridViewTextBoxColumn.HeaderText = "提醒时间";
            this.reminderTimeDataGridViewTextBoxColumn.Name = "reminderTimeDataGridViewTextBoxColumn";
            // 
            // messageDataGridViewTextBoxColumn
            // 
            this.messageDataGridViewTextBoxColumn.DataPropertyName = "Message";
            this.messageDataGridViewTextBoxColumn.HeaderText = "消息";
            this.messageDataGridViewTextBoxColumn.Name = "messageDataGridViewTextBoxColumn";
            // 
            // WorkflowManagementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Name = "WorkflowManagementControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.WorkflowManagementControl_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.groupBoxWorkflowList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWorkflows)).EndInit();
            this.groupBoxWorkflowActions.ResumeLayout(false);
            this.tabControlWorkflowTests.ResumeLayout(false);
            this.tabPageApproval.ResumeLayout(false);
            this.tabPageApproval.PerformLayout();
            this.tabPageEvent.ResumeLayout(false);
            this.tabPageEvent.PerformLayout();
            this.tabPageOtherTests.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.GroupBox groupBoxWorkflowList;
        private System.Windows.Forms.DataGridView dataGridViewWorkflows;
        private System.Windows.Forms.GroupBox groupBoxWorkflowActions;
        private System.Windows.Forms.TabControl tabControlWorkflowTests;
        private System.Windows.Forms.TabPage tabPageApproval;
        private System.Windows.Forms.TabPage tabPageEvent;
        private System.Windows.Forms.TabPage tabPageOtherTests;
        private System.Windows.Forms.Button btnStartApprovalWorkflow;
        private System.Windows.Forms.TextBox textBoxBillId;
        private System.Windows.Forms.Label labelBillId;
        private System.Windows.Forms.Button btnPublishEvent;
        private System.Windows.Forms.TextBox textBoxWorkflowId;
        private System.Windows.Forms.Label labelWorkflowId;
        private System.Windows.Forms.TextBox textBoxEventName;
        private System.Windows.Forms.Label labelEventName;
        private System.Windows.Forms.Button btnStartSafetyStockWorkflow;
        private System.Windows.Forms.Button btnStartReminderWorkflow;
        private System.Windows.Forms.Button btnPushTest;
        private System.Windows.Forms.Button btnCacheTest;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClearCompleted;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priorityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sendTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn senderIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn senderNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reminderContentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn entityTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn billDataDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remindSubjectDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remindTimesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remindIntervalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bizPrimaryKeyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bizTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn workflowIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reminderTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn messageDataGridViewTextBoxColumn;
    }
}