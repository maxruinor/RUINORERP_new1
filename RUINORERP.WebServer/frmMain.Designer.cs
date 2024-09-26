namespace RUINORERP.WebServer
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
            logViewer1 = new LogViewer();
            statusStrip1 = new StatusStrip();
            toolStrip1 = new ToolStrip();
            btnStartServer = new ToolStripButton();
            btnStopServer = new ToolStripButton();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // logViewer1
            // 
            logViewer1.BackColor = Color.White;
            logViewer1.Dock = DockStyle.Bottom;
            logViewer1.Font = new Font("Consolas", 9F);
            logViewer1.Location = new Point(0, 303);
            logViewer1.Name = "logViewer1";
            logViewer1.ReadOnly = true;
            logViewer1.ScrollBars = RichTextBoxScrollBars.Vertical;
            logViewer1.Size = new Size(803, 153);
            logViewer1.TabIndex = 0;
            logViewer1.Text = "";
            logViewer1.WordWrap = false;
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(0, 456);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(803, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { btnStartServer, btnStopServer });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(803, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnStartServer
            // 
            btnStartServer.Image = Properties.Resources.player_play;
            btnStartServer.ImageTransparentColor = Color.Magenta;
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new Size(52, 22);
            btnStartServer.Text = "启动";
            btnStartServer.Click += btnStartServer_Click;
            // 
            // btnStopServer
            // 
            btnStopServer.Image = Properties.Resources.player_stop;
            btnStopServer.ImageTransparentColor = Color.Magenta;
            btnStopServer.Name = "btnStopServer";
            btnStopServer.Size = new Size(52, 22);
            btnStopServer.Text = "停止";
            btnStopServer.Click += btnStopServer_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(803, 478);
            Controls.Add(toolStrip1);
            Controls.Add(logViewer1);
            Controls.Add(statusStrip1);
            Name = "frmMain";
            Text = "Web服务器";
            FormClosing += frmMain_FormClosing;
            Load += frmMain_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        internal LogViewer logViewer1;
        private StatusStrip statusStrip1;
        private ToolStrip toolStrip1;
        private ToolStripButton btnStartServer;
        private ToolStripButton btnStopServer;
    }
}