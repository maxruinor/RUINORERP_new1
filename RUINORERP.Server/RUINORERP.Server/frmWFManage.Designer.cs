namespace RUINORERP.Server
{
    partial class frmWFManage
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ServerBizDataBindingSource = new System.Windows.Forms.BindingSource(components);
            statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            priorityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            sendTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            senderIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            senderNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            reminderContentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            entityTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            billDataDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            startTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            endTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            remindSubjectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            remindTimesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            remindIntervalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            bizPrimaryKeyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            bizTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            workflowIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            reminderTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            messageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ServerBizDataBindingSource).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { statusDataGridViewTextBoxColumn, priorityDataGridViewTextBoxColumn, sendTimeDataGridViewTextBoxColumn, senderIDDataGridViewTextBoxColumn, senderNameDataGridViewTextBoxColumn, reminderContentDataGridViewTextBoxColumn, entityTypeDataGridViewTextBoxColumn, billDataDataGridViewTextBoxColumn, startTimeDataGridViewTextBoxColumn, endTimeDataGridViewTextBoxColumn, remindSubjectDataGridViewTextBoxColumn, remindTimesDataGridViewTextBoxColumn, remindIntervalDataGridViewTextBoxColumn, idDataGridViewTextBoxColumn, bizPrimaryKeyDataGridViewTextBoxColumn, bizTypeDataGridViewTextBoxColumn, workflowIdDataGridViewTextBoxColumn, reminderTimeDataGridViewTextBoxColumn, messageDataGridViewTextBoxColumn });
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.DataSource = ServerBizDataBindingSource;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(1295, 450);
            dataGridView1.TabIndex = 0;
            dataGridView1.DataError += dataGridView1_DataError;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(137, 26);
            contextMenuStrip1.ItemClicked += contextMenuStrip1_ItemClicked;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(136, 22);
            toolStripMenuItem1.Text = "工作流测试";
            // 
            // ServerBizDataBindingSource
            // 
            ServerBizDataBindingSource.DataSource = typeof(Model.TransModel.ServerReminderData);
            // 
            // statusDataGridViewTextBoxColumn
            // 
            statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            statusDataGridViewTextBoxColumn.HeaderText = "Status";
            statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            // 
            // priorityDataGridViewTextBoxColumn
            // 
            priorityDataGridViewTextBoxColumn.DataPropertyName = "Priority";
            priorityDataGridViewTextBoxColumn.HeaderText = "Priority";
            priorityDataGridViewTextBoxColumn.Name = "priorityDataGridViewTextBoxColumn";
            // 
            // sendTimeDataGridViewTextBoxColumn
            // 
            sendTimeDataGridViewTextBoxColumn.DataPropertyName = "SendTime";
            sendTimeDataGridViewTextBoxColumn.HeaderText = "SendTime";
            sendTimeDataGridViewTextBoxColumn.Name = "sendTimeDataGridViewTextBoxColumn";
            // 
            // senderIDDataGridViewTextBoxColumn
            // 
            senderIDDataGridViewTextBoxColumn.DataPropertyName = "SenderID";
            senderIDDataGridViewTextBoxColumn.HeaderText = "SenderID";
            senderIDDataGridViewTextBoxColumn.Name = "senderIDDataGridViewTextBoxColumn";
            // 
            // senderNameDataGridViewTextBoxColumn
            // 
            senderNameDataGridViewTextBoxColumn.DataPropertyName = "SenderName";
            senderNameDataGridViewTextBoxColumn.HeaderText = "SenderName";
            senderNameDataGridViewTextBoxColumn.Name = "senderNameDataGridViewTextBoxColumn";
            // 
            // reminderContentDataGridViewTextBoxColumn
            // 
            reminderContentDataGridViewTextBoxColumn.DataPropertyName = "ReminderContent";
            reminderContentDataGridViewTextBoxColumn.HeaderText = "ReminderContent";
            reminderContentDataGridViewTextBoxColumn.Name = "reminderContentDataGridViewTextBoxColumn";
            // 
            // entityTypeDataGridViewTextBoxColumn
            // 
            entityTypeDataGridViewTextBoxColumn.DataPropertyName = "EntityType";
            entityTypeDataGridViewTextBoxColumn.HeaderText = "EntityType";
            entityTypeDataGridViewTextBoxColumn.Name = "entityTypeDataGridViewTextBoxColumn";
            // 
            // billDataDataGridViewTextBoxColumn
            // 
            billDataDataGridViewTextBoxColumn.DataPropertyName = "BillData";
            billDataDataGridViewTextBoxColumn.HeaderText = "BillData";
            billDataDataGridViewTextBoxColumn.Name = "billDataDataGridViewTextBoxColumn";
            // 
            // startTimeDataGridViewTextBoxColumn
            // 
            startTimeDataGridViewTextBoxColumn.DataPropertyName = "StartTime";
            startTimeDataGridViewTextBoxColumn.HeaderText = "StartTime";
            startTimeDataGridViewTextBoxColumn.Name = "startTimeDataGridViewTextBoxColumn";
            // 
            // endTimeDataGridViewTextBoxColumn
            // 
            endTimeDataGridViewTextBoxColumn.DataPropertyName = "EndTime";
            endTimeDataGridViewTextBoxColumn.HeaderText = "EndTime";
            endTimeDataGridViewTextBoxColumn.Name = "endTimeDataGridViewTextBoxColumn";
            // 
            // remindSubjectDataGridViewTextBoxColumn
            // 
            remindSubjectDataGridViewTextBoxColumn.DataPropertyName = "RemindSubject";
            remindSubjectDataGridViewTextBoxColumn.HeaderText = "RemindSubject";
            remindSubjectDataGridViewTextBoxColumn.Name = "remindSubjectDataGridViewTextBoxColumn";
            // 
            // remindTimesDataGridViewTextBoxColumn
            // 
            remindTimesDataGridViewTextBoxColumn.DataPropertyName = "RemindTimes";
            remindTimesDataGridViewTextBoxColumn.HeaderText = "RemindTimes";
            remindTimesDataGridViewTextBoxColumn.Name = "remindTimesDataGridViewTextBoxColumn";
            // 
            // remindIntervalDataGridViewTextBoxColumn
            // 
            remindIntervalDataGridViewTextBoxColumn.DataPropertyName = "RemindInterval";
            remindIntervalDataGridViewTextBoxColumn.HeaderText = "RemindInterval";
            remindIntervalDataGridViewTextBoxColumn.Name = "remindIntervalDataGridViewTextBoxColumn";
            // 
            // idDataGridViewTextBoxColumn
            // 
            idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn.HeaderText = "Id";
            idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            // 
            // bizPrimaryKeyDataGridViewTextBoxColumn
            // 
            bizPrimaryKeyDataGridViewTextBoxColumn.DataPropertyName = "BizPrimaryKey";
            bizPrimaryKeyDataGridViewTextBoxColumn.HeaderText = "BizPrimaryKey";
            bizPrimaryKeyDataGridViewTextBoxColumn.Name = "bizPrimaryKeyDataGridViewTextBoxColumn";
            // 
            // bizTypeDataGridViewTextBoxColumn
            // 
            bizTypeDataGridViewTextBoxColumn.DataPropertyName = "BizType";
            bizTypeDataGridViewTextBoxColumn.HeaderText = "BizType";
            bizTypeDataGridViewTextBoxColumn.Name = "bizTypeDataGridViewTextBoxColumn";
            // 
            // workflowIdDataGridViewTextBoxColumn
            // 
            workflowIdDataGridViewTextBoxColumn.DataPropertyName = "WorkflowId";
            workflowIdDataGridViewTextBoxColumn.HeaderText = "WorkflowId";
            workflowIdDataGridViewTextBoxColumn.Name = "workflowIdDataGridViewTextBoxColumn";
            // 
            // reminderTimeDataGridViewTextBoxColumn
            // 
            reminderTimeDataGridViewTextBoxColumn.DataPropertyName = "ReminderTime";
            reminderTimeDataGridViewTextBoxColumn.HeaderText = "ReminderTime";
            reminderTimeDataGridViewTextBoxColumn.Name = "reminderTimeDataGridViewTextBoxColumn";
            // 
            // messageDataGridViewTextBoxColumn
            // 
            messageDataGridViewTextBoxColumn.DataPropertyName = "Message";
            messageDataGridViewTextBoxColumn.HeaderText = "Message";
            messageDataGridViewTextBoxColumn.Name = "messageDataGridViewTextBoxColumn";
            // 
            // frmWFManage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1295, 450);
            Controls.Add(dataGridView1);
            Name = "frmWFManage";
            Text = "工作流列表";
            FormClosing += frmUserManage_FormClosing;
            Load += frmUserManage_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ServerBizDataBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.BindingSource ServerBizDataBindingSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn onlineDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn serverAuthenticationDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isProcessedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nextProcessorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subjectDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn stopRemindDataGridViewCheckBoxColumn;
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