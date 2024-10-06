using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.IIS
{
    partial class frmMain
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnStartServer = new System.Windows.Forms.ToolStripButton();
            this.btnStopServer = new System.Windows.Forms.ToolStripButton();
            this.logViewer1 = new RUINORERP.IIS.LogViewer();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 315);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(688, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStartServer,
            this.btnStopServer});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(688, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Image = global::RUINORERP.IIS.Properties.Resources.player_play;
            this.btnStartServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(52, 22);
            this.btnStartServer.Text = "启动";
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // btnStopServer
            // 
            this.btnStopServer.Image = global::RUINORERP.IIS.Properties.Resources.player_stop;
            this.btnStopServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(52, 22);
            this.btnStopServer.Text = "停止";
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // logViewer1
            // 
            this.logViewer1.BackColor = System.Drawing.Color.White;
            this.logViewer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logViewer1.Font = new System.Drawing.Font("Consolas", 9F);
            this.logViewer1.Location = new System.Drawing.Point(0, 206);
            this.logViewer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.logViewer1.Name = "logViewer1";
            this.logViewer1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logViewer1.Size = new System.Drawing.Size(688, 109);
            this.logViewer1.TabIndex = 0;
            this.logViewer1.Text = "";
            this.logViewer1.WordWrap = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 337);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.logViewer1);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmMain";
            this.Text = "Web服务器";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal LogViewer logViewer1;
        private StatusStrip statusStrip1;
        private ToolStrip toolStrip1;
        private ToolStripButton btnStartServer;
        private ToolStripButton btnStopServer;
    }
}