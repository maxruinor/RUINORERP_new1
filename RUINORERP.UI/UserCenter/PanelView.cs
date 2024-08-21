using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using RUINORERP.Common.Helper;

namespace RUINORERP.UI.UserCenter
{
	/// <summary>
	/// PanelView ��ժҪ˵����
	/// </summary>
	public class PanelView : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PanelView()
		{
			// �õ����� Windows.Forms ���������������ġ�
			InitializeComponent();

			// TODO: �� InitializeComponent ���ú�����κγ�ʼ��

		}

		/// <summary> 
		/// ������������ʹ�õ���Դ��
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

		#region �����������ɵĴ���
		/// <summary> 
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
		/// �޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // PanelView
            // 
            this.Name = "PanelView";
            this.Size = new System.Drawing.Size(837, 650);
            this.Load += new System.EventHandler(this.PanelView_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void PanelView_Load(object sender, System.EventArgs e)
		{
		
		}
		private int headheight=20;
		public int HeadHeight
		{
			get
			{
				return this.headheight;
			}
			set
			{
				this.headheight=value;
				this.Invalidate();
			}
		}
		//Color.FromArgb(200,223,251)
		//private Color  lightcolor=Color.FromArgb(224,245,226);
		private Color  lightcolor=Color.FromArgb(245,242,210);
		private Color  darkcolor=Color.FromArgb(220,220,160);
		public Color LightColor
		{
			get
			{
				return this.lightcolor;
			}
			set
			{
				this.lightcolor=value;
				this.Invalidate();
			}
		}
		public Color DarkColor
		{
			get
			{
				return this.darkcolor;
			}
			set
			{
				this.darkcolor=value;
				this.Invalidate();
			}
		}
		private int bottomheight=15;
		public int BottomHeight
		{
			get
			{
				return this.bottomheight;
			}
			set
			{
				this.bottomheight=value;
				this.Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Color c=ColorTools.Mix(Color.LightGray,Color.Gray,0.7f);
			Pen p=new Pen(c);
			// �� ����
			Brush  sb=new LinearGradientBrush(new  Rectangle(0,0,this.Width,this.HeadHeight),this.darkcolor,this.lightcolor,90,false);
			e.Graphics.FillRectangle(sb,0,0,this.Width,this.HeadHeight);
		//	e.Graphics.DrawLine(Pens.White,0,1,this.Width,1);
			e.Graphics.DrawLine(p,0,this.HeadHeight,this.Width,this.HeadHeight);
			//�� ����� ��
			
			//sb=new LinearGradientBrush(new Rectangle(0,0,this.HeadHeight,this.HeadHeight),this.lightcolor,this.darkcolor,90,false);
			sb=new SolidBrush(this.lightcolor);
			e.Graphics.FillRectangle(sb,0,this.Height-this.BottomHeight,this.Width,this.BottomHeight);

			//�� 
			e.Graphics.DrawLine(p,0,this.Height-this.BottomHeight,this.Width,this.Height-this.BottomHeight);
			e.Graphics.DrawRectangle(p,0,0,this.Width-1,this.Height-1);

			e.Graphics.DrawString(HeadTitle,this.Font,Brushes.Black,3,3);
		
		}

		private string headtitle;
		public  string HeadTitle
		{
			get
			{
				return this.headtitle;
			}
			set
			{
				this.headtitle=value;
			}
		}
	}
}
