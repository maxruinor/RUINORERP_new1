namespace RUINORERP.Server
{
    partial class frmWorkFlowManage
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
            button1 = new System.Windows.Forms.Button();
            textBox1 = new System.Windows.Forms.TextBox();
            button2 = new System.Windows.Forms.Button();
            textBox2 = new System.Windows.Forms.TextBox();
            button3 = new System.Windows.Forms.Button();
            btn外部事件 = new System.Windows.Forms.Button();
            btnStart = new System.Windows.Forms.Button();
            textBox3 = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            txtworkflowid = new System.Windows.Forms.TextBox();
            txteventName = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            txteventPara = new System.Windows.Forms.TextBox();
            txtworkflowParam = new System.Windows.Forms.TextBox();
            btnPushTest = new System.Windows.Forms.Button();
            btn缓存测试 = new System.Windows.Forms.Button();
            btnStartReminderWF = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(569, 70);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(429, 70);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(100, 23);
            textBox1.TabIndex = 1;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(569, 142);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 0;
            button2.Text = "事件";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(429, 142);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(100, 23);
            textBox2.TabIndex = 1;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(550, 384);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(121, 59);
            button3.TabIndex = 8;
            button3.Text = "外部UserTAsk";
            button3.UseVisualStyleBackColor = true;
            // 
            // btn外部事件
            // 
            btn外部事件.Location = new System.Drawing.Point(550, 309);
            btn外部事件.Name = "btn外部事件";
            btn外部事件.Size = new System.Drawing.Size(121, 50);
            btn外部事件.TabIndex = 2;
            btn外部事件.Text = "外部事件";
            btn外部事件.UseVisualStyleBackColor = true;
            btn外部事件.Click += btn外部事件_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new System.Drawing.Point(550, 246);
            btnStart.Name = "btnStart";
            btnStart.Size = new System.Drawing.Size(121, 43);
            btnStart.TabIndex = 0;
            btnStart.Text = "启动工作流";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(389, 397);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(154, 23);
            textBox3.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(281, 397);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(98, 17);
            label1.TabIndex = 5;
            label1.Text = "UserTask参数值";
            // 
            // txtworkflowid
            // 
            txtworkflowid.Location = new System.Drawing.Point(133, 250);
            txtworkflowid.Name = "txtworkflowid";
            txtworkflowid.Size = new System.Drawing.Size(246, 23);
            txtworkflowid.TabIndex = 7;
            // 
            // txteventName
            // 
            txteventName.Location = new System.Drawing.Point(121, 317);
            txteventName.Name = "txteventName";
            txteventName.Size = new System.Drawing.Size(100, 23);
            txteventName.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(321, 311);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(68, 17);
            label2.TabIndex = 5;
            label2.Text = "事件参数值";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(44, 253);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(72, 17);
            label3.TabIndex = 5;
            label3.Text = "workflowid";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(55, 317);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(44, 17);
            label4.TabIndex = 5;
            label4.Text = "事件名";
            // 
            // txteventPara
            // 
            txteventPara.Location = new System.Drawing.Point(389, 312);
            txteventPara.Name = "txteventPara";
            txteventPara.Size = new System.Drawing.Size(154, 23);
            txteventPara.TabIndex = 3;
            // 
            // txtworkflowParam
            // 
            txtworkflowParam.Location = new System.Drawing.Point(389, 251);
            txtworkflowParam.Name = "txtworkflowParam";
            txtworkflowParam.Size = new System.Drawing.Size(154, 23);
            txtworkflowParam.TabIndex = 1;
            // 
            // btnPushTest
            // 
            btnPushTest.Location = new System.Drawing.Point(12, 57);
            btnPushTest.Name = "btnPushTest";
            btnPushTest.Size = new System.Drawing.Size(130, 46);
            btnPushTest.TabIndex = 9;
            btnPushTest.Text = "推送工作测试";
            btnPushTest.UseVisualStyleBackColor = true;
            btnPushTest.Click += btnPushTest_Click;
            // 
            // btn缓存测试
            // 
            btn缓存测试.Location = new System.Drawing.Point(24, 12);
            btn缓存测试.Name = "btn缓存测试";
            btn缓存测试.Size = new System.Drawing.Size(75, 23);
            btn缓存测试.TabIndex = 10;
            btn缓存测试.Text = "缓存测试";
            btn缓存测试.UseVisualStyleBackColor = true;
            btn缓存测试.Click += btn缓存测试_Click;
            // 
            // btnStartReminderWF
            // 
            btnStartReminderWF.Location = new System.Drawing.Point(55, 551);
            btnStartReminderWF.Name = "btnStartReminderWF";
            btnStartReminderWF.Size = new System.Drawing.Size(130, 34);
            btnStartReminderWF.TabIndex = 11;
            btnStartReminderWF.Text = "启动提醒工作流";
            btnStartReminderWF.UseVisualStyleBackColor = true;
            btnStartReminderWF.Click += btnStartReminderWF_Click;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(582, 551);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(191, 34);
            button4.TabIndex = 11;
            button4.Text = "启动安全库存动态计算工作流";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // frmWorkFlowManage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1069, 626);
            Controls.Add(button4);
            Controls.Add(btnStartReminderWF);
            Controls.Add(btn缓存测试);
            Controls.Add(btnPushTest);
            Controls.Add(txtworkflowParam);
            Controls.Add(txteventPara);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txteventName);
            Controls.Add(txtworkflowid);
            Controls.Add(label1);
            Controls.Add(textBox3);
            Controls.Add(btnStart);
            Controls.Add(btn外部事件);
            Controls.Add(button3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "frmWorkFlowManage";
            Text = "frmWorkFlowManage";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btn外部事件;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtworkflowid;
        private System.Windows.Forms.TextBox txteventName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txteventPara;
#pragma warning disable CS0169 // 从不使用字段“frmWorkFlowManage.textBox7”
        private System.Windows.Forms.TextBox textBox7;
#pragma warning restore CS0169 // 从不使用字段“frmWorkFlowManage.textBox7”
        private System.Windows.Forms.TextBox txtworkflowParam;
        private System.Windows.Forms.Button btnPushTest;
        private System.Windows.Forms.Button btn缓存测试;
        private System.Windows.Forms.Button btnStartReminderWF;
        private System.Windows.Forms.Button button4;
    }
}