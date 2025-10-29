using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using Krypton.Toolkit;

namespace RUINORERP.UI.IM
{
    partial class BusinessMessagePrompt
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.txtContent = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonPanel2 = new Krypton.Toolkit.KryptonPanel();
            this.lblSendTime = new Krypton.Toolkit.KryptonLabel();
            this.txtSubject = new Krypton.Toolkit.KryptonTextBox();
            this.txtSender = new Krypton.Toolkit.KryptonTextBox();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnNavigate = new Krypton.Toolkit.KryptonButton();
            this.btnWaitReminder = new Krypton.Toolkit.KryptonDropButton();
            this.kryptonContextMenu1 = new Krypton.Toolkit.KryptonContextMenu();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.txtContent);
            this.kryptonPanel1.Controls.Add(this.kryptonPanel2);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(540, 300);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Location = new System.Drawing.Point(0, 98);
            this.txtContent.Margin = new System.Windows.Forms.Padding(4);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ReadOnly = true;
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Size = new System.Drawing.Size(540, 202);
            this.txtContent.TabIndex = 1;
            // 
            // kryptonPanel2
            // 
            this.kryptonPanel2.Controls.Add(this.lblSendTime);
            this.kryptonPanel2.Controls.Add(this.txtSubject);
            this.kryptonPanel2.Controls.Add(this.txtSender);
            this.kryptonPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanel2.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(540, 98);
            this.kryptonPanel2.TabIndex = 0;
            // 
            // lblSendTime
            // 
            this.lblSendTime.Location = new System.Drawing.Point(360, 60);
            this.lblSendTime.Margin = new System.Windows.Forms.Padding(4);
            this.lblSendTime.Name = "lblSendTime";
            this.lblSendTime.Size = new System.Drawing.Size(99, 20);
            this.lblSendTime.TabIndex = 2;
            this.lblSendTime.Values.Text = "yyyy-MM-dd";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(100, 50);
            this.txtSubject.Margin = new System.Windows.Forms.Padding(4);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.ReadOnly = true;
            this.txtSubject.Size = new System.Drawing.Size(230, 27);
            this.txtSubject.TabIndex = 1;
            this.txtSubject.Text = "主题";
            // 
            // txtSender
            // 
            this.txtSender.Location = new System.Drawing.Point(100, 15);
            this.txtSender.Margin = new System.Windows.Forms.Padding(4);
            this.txtSender.Name = "txtSender";
            this.txtSender.ReadOnly = true;
            this.txtSender.Size = new System.Drawing.Size(230, 27);
            this.txtSender.TabIndex = 0;
            this.txtSender.Text = "发送人";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(420, 260);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "关闭";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(310, 260);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.TabIndex = 2;
            this.btnOk.Values.Text = "已读";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnNavigate
            // 
            this.btnNavigate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNavigate.Location = new System.Drawing.Point(200, 260);
            this.btnNavigate.Margin = new System.Windows.Forms.Padding(4);
            this.btnNavigate.Name = "btnNavigate";
            this.btnNavigate.Size = new System.Drawing.Size(100, 30);
            this.btnNavigate.TabIndex = 3;
            this.btnNavigate.Values.Text = "查看单据";
            this.btnNavigate.Visible = false;
            this.btnNavigate.Click += new System.EventHandler(this.btnNavigate_Click);
            // 
            // btnWaitReminder
            // 
            this.btnWaitReminder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWaitReminder.KryptonContextMenu = this.kryptonContextMenu1;
            this.btnWaitReminder.Location = new System.Drawing.Point(90, 260);
            this.btnWaitReminder.Margin = new System.Windows.Forms.Padding(4);
            this.btnWaitReminder.Name = "btnWaitReminder";
            this.btnWaitReminder.Size = new System.Drawing.Size(100, 30);
            this.btnWaitReminder.TabIndex = 4;
            this.btnWaitReminder.Values.Text = "稍后提醒";
            this.btnWaitReminder.Click += new System.EventHandler(this.btnWaitReminder_Click);
            // 
            // kryptonContextMenu1
            // 
            this.kryptonContextMenu1.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {
            });
            // 
            // BusinessMessagePrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 300);
            this.Controls.Add(this.btnWaitReminder);
            this.Controls.Add(this.btnNavigate);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BusinessMessagePrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "业务消息";
            this.Load += new System.EventHandler(this.BusinessMessagePrompt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            this.kryptonPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        private KryptonPanel kryptonPanel1;
        private KryptonTextBox txtContent;
        private KryptonPanel kryptonPanel2;
        private KryptonLabel lblSendTime;
        private KryptonTextBox txtSubject;
        private KryptonTextBox txtSender;
        private KryptonButton btnCancel;
        private KryptonButton btnOk;
        private KryptonButton btnNavigate;
        private KryptonDropButton btnWaitReminder;
        private KryptonContextMenu kryptonContextMenu1;
    }
}