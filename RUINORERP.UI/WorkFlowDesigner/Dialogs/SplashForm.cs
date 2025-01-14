using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
namespace RUINORERP.UI.WorkFlowDesigner
{
	/// <summary>
	/// Original SplashForm running on a single thread and not with parallell loading.
	/// The SplashScreen form performs much better.
	/// </summary>
	public class SplashForm : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Timer timer1;
		private Bitmap bmp;
		public SplashForm(bool timeout)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Cobalt.NetronSplash.jpg");
					
			bmp= Bitmap.FromStream(stream) as Bitmap;
			stream.Close();
			stream=null;

			if(timeout)
			{
				timer1.Start();
			}

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			// 
			// timer1
			// 
			this.timer1.Interval = 1500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// SplashForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(530, 228);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SplashForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SplashForm";
			this.Click += new System.EventHandler(this.SplashForm_Click);

		}
		#endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			Graphics g=e.Graphics;
			g.DrawImage(bmp,0,0,530,228);
		}

		private void SplashForm_Click(object sender, System.EventArgs e)
		{
			this.Close();
			this.Dispose();
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			this.timer1.Stop();
			this.Close();
			this.Dispose();

		}

	}
}
