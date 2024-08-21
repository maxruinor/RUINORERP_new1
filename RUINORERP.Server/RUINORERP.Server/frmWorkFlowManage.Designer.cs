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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.btn外部事件 = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtworkflowid = new System.Windows.Forms.TextBox();
            this.txteventName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txteventPara = new System.Windows.Forms.TextBox();
            this.txtworkflowParam = new System.Windows.Forms.TextBox();
            this.btnPushTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(676, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(474, 69);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 23);
            this.textBox1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(676, 99);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "事件";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(474, 141);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 23);
            this.textBox2.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(657, 378);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 59);
            this.button3.TabIndex = 8;
            this.button3.Text = "外部UserTAsk";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btn外部事件
            // 
            this.btn外部事件.Location = new System.Drawing.Point(657, 303);
            this.btn外部事件.Name = "btn外部事件";
            this.btn外部事件.Size = new System.Drawing.Size(121, 50);
            this.btn外部事件.TabIndex = 2;
            this.btn外部事件.Text = "外部事件";
            this.btn外部事件.UseVisualStyleBackColor = true;
            this.btn外部事件.Click += new System.EventHandler(this.btn外部事件_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(657, 240);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(121, 43);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "启动工作流";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(434, 396);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(154, 23);
            this.textBox3.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(320, 396);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "UserTask参数值";
            // 
            // txtworkflowid
            // 
            this.txtworkflowid.Location = new System.Drawing.Point(133, 250);
            this.txtworkflowid.Name = "txtworkflowid";
            this.txtworkflowid.Size = new System.Drawing.Size(246, 23);
            this.txtworkflowid.TabIndex = 7;
            // 
            // txteventName
            // 
            this.txteventName.Location = new System.Drawing.Point(121, 317);
            this.txteventName.Name = "txteventName";
            this.txteventName.Size = new System.Drawing.Size(100, 23);
            this.txteventName.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(360, 310);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "事件参数值";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "workflowid";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 317);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "事件名";
            // 
            // txteventPara
            // 
            this.txteventPara.Location = new System.Drawing.Point(434, 311);
            this.txteventPara.Name = "txteventPara";
            this.txteventPara.Size = new System.Drawing.Size(154, 23);
            this.txteventPara.TabIndex = 3;
            // 
            // txtworkflowParam
            // 
            this.txtworkflowParam.Location = new System.Drawing.Point(434, 250);
            this.txtworkflowParam.Name = "txtworkflowParam";
            this.txtworkflowParam.Size = new System.Drawing.Size(154, 23);
            this.txtworkflowParam.TabIndex = 1;
            // 
            // btnPushTest
            // 
            this.btnPushTest.Location = new System.Drawing.Point(271, 12);
            this.btnPushTest.Name = "btnPushTest";
            this.btnPushTest.Size = new System.Drawing.Size(130, 46);
            this.btnPushTest.TabIndex = 9;
            this.btnPushTest.Text = "推送工作测试";
            this.btnPushTest.UseVisualStyleBackColor = true;
            this.btnPushTest.Click += new System.EventHandler(this.btnPushTest_Click);
            // 
            // frmWorkFlowManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 540);
            this.Controls.Add(this.btnPushTest);
            this.Controls.Add(this.txtworkflowParam);
            this.Controls.Add(this.txteventPara);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txteventName);
            this.Controls.Add(this.txtworkflowid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btn外部事件);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "frmWorkFlowManage";
            this.Text = "frmWorkFlowManage";
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}