using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using RUINORERP.Business;
using RUINORERP.Model;
using System.Collections.Generic;

namespace RUINORERP.UI.UserCenter
{
    /// <summary>
    /// RightPanel ��ժҪ˵����
    /// </summary>
    public class RightPanel : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.Panel panel1;
        /// <summary> 
        /// ����������������
        /// </summary>
        private System.ComponentModel.Container components = null;

        public RightPanel()
        {
            // �õ����� Windows.Forms ���������������ġ�
            InitializeComponent();

            // TODO: �� InitializeComponent ���ú������κγ�ʼ��

        }

        /// <summary> 
        /// ������������ʹ�õ���Դ��
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region �����������ɵĴ���
        /// <summary> 
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
        /// �޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(838, 600);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // RightPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "RightPanel";
            this.Size = new System.Drawing.Size(838, 600);
            this.ResumeLayout(false);

        }
        #endregion
        //private Color titlecolor=Color.Green;
        private Color titlecolor = Color.LightSkyBlue;
        public Color TitleColor
        {
            get
            {
                return titlecolor;
            }
            set
            {
                titlecolor = value;
                this.Invalidate();
            }
        }
        private int headheight = 35;
        public int HeadHeight
        {
            get
            {
                return this.headheight;
            }
            set
            {
                this.headheight = value;
                this.Invalidate();
            }
        }
        private int bottomheight = 20;
        public int BottomHeight
        {
            get
            {
                return this.bottomheight;
            }
            set
            {
                this.bottomheight = value;
                this.Invalidate();
            }
        }
        private Color Mix(Color color, double rate)
        {

            int r = Colorx(color.A, 255, rate);
            int g = Colorx(color.G, 255, rate);
            int b = Colorx(color.B, 255, rate);
            return Color.FromArgb(r, g, b);

        }
        private int Colorx(int a, int b, double rate)
        {
            int d = Convert.ToInt32(a * (1 - rate) + b * rate);
            if (d > 255) d = 255;
            if (d < 0) d = 0;
            return d;
        }

        private int arcsize = 5;
        public int ArcSize
        {
            get
            {
                return this.arcsize;
            }
            set
            {
                this.arcsize = value;
                this.Invalidate();
            }
        }
        private string titletext = "��ҵ����";
        public string TitleText
        {
            get
            {
                return this.titletext;
            }
            set
            {
                this.titletext = value;
                this.Invalidate();
            }
        }
        private Font titlefont = new Font("����", 12);
        public Font TitleFont
        {
            get
            {
                return this.titlefont;
            }
            set
            {
                this.titlefont = value;
                this.Invalidate();
            }
        }
        private bool showhead = true;
        public bool ShowHead
        {
            get
            {
                return this.showhead;
            }
            set
            {
                this.showhead = value;
            }
        }

        private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //�趨 ������ɫ
            if (this.ShowHead)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddArc(0, 0, this.ArcSize, this.ArcSize, 180, 90);
                gp.AddLine(this.ArcSize, 0, this.Width, 0);
                gp.AddLine(this.Width, 0, this.Width, this.HeadHeight);
                gp.AddLine(this.Width, this.HeadHeight, 0, this.HeadHeight);
                gp.AddLine(0, this.Height, 0, this.ArcSize);
                //Brush sb=new LinearGradientBrush(new Rectangle(0,0,this.panel1.Width,this.panel1.Width),this.TitleColor,Color.LightGray,0,false);
                Brush sb = new SolidBrush(this.TitleColor);
                e.Graphics.FillPath(sb, gp);

                //�� ������
                e.Graphics.DrawLine(Pens.Gray, 0, this.HeadHeight, this.Width, this.HeadHeight);
                e.Graphics.DrawLine(Pens.White, 0, this.HeadHeight + 1, this.Width, this.HeadHeight + 1);
                e.Graphics.DrawString(this.titletext, this.TitleFont, Brushes.White, 10, 7);

                Color scolor = this.Mix(this.TitleColor, 0.7);
                //�� ���� �� ��ɫ��Ե

                gp = new GraphicsPath();
                gp.AddLine(0, this.Height, 0, this.ArcSize);
                gp.AddArc(0, 0, this.ArcSize, this.ArcSize, 180, 90);
                gp.AddLine(this.ArcSize, 0, this.Width, 0);
                e.Graphics.DrawPath(new Pen(scolor), gp);

                scolor = this.TitleColor;
                //�� ���� �� ��ɫ��Ե

                gp = new GraphicsPath();
                //			gp.AddLine(this.Width,this.HeadHeight-this.BottomHeight,this.Width-this.ArcSize,this.HeadHeight-this.BottomHeight);
                //			gp.AddArc(0,this.Height-this.BottomHeight-this.ArcSize,ArcSize,ArcSize,90,90);
                gp.AddLine(1, this.Height - this.BottomHeight - this.ArcSize, 1, this.ArcSize);
                gp.AddArc(1, 1, this.ArcSize, this.ArcSize, 180, 90);
                gp.AddLine(this.ArcSize, 1, this.Width, 1);

                e.Graphics.DrawPath(new Pen(scolor), gp);
            }
        }
        /*
		public void LoadPage(ҳ�涨�� pagedefine)
		{
			//װ�� ����

			this.panel1.Controls.Clear();
			//
			int sy=this.HeadHeight-3;
			int lsy=sy;
			int rsy=sy;
			int lsx=25;
			//int rsx=250;

			for (int i=0;i<pagedefine.ģ��.Count;i++)
			{
				ҳ��ģ�鶨��  md=(ҳ��ģ�鶨��)pagedefine.ģ��[i];
				//���� ���Ҳ�
				switch (md.����ģ������)
				{
					case "FlowPanel":
						FlowPanel fp=new FlowPanel();
						fp.Size=new Size(480,320);
						fp.Location=new Point(lsx,lsy);
						fp.SetFlowName(md.����);
						this.panel1.Controls.Add(fp);
						fp.ItemClick+=new EventHandler(fp_ItemClick);
						break;
					default:break;
				}
			}

			
//			//����һ�� OfficePanel
//			Office2003Header mh=new  Office2003Header();
//			mh.TitleText="������ʾ";
//			mh.Size=new Size(200,150);
//			mh.Location=new Point(600,20);
//			this.panel1.Controls.Add(mh);
//
//			Office2003HeaderSub  oh=new Office2003HeaderSub();
//			oh.TitleText="��ʾ";
//			oh.Size=new Size(200,150);
//			oh.Location=new Point(0,25);
//			oh.BackColor=Color.White;
//			mh.Controls.Add(oh);
//			Label l=new Label();
//			l.Text="ʹ������ͼֱ�ӷ��ʵ���";
//			l.Dock=DockStyle.Fill;
//			oh.Controls.Add(l);
//			l.TextAlign=ContentAlignment.MiddleCenter;
			
		}
		*/
        tb_ModuleDefinitionController<tb_ModuleDefinition> ctr = Startup.GetFromFac<tb_ModuleDefinitionController<tb_ModuleDefinition>>();
        tb_FlowchartDefinitionController<tb_FlowchartDefinition> ctrFlowChart = Startup.GetFromFac<tb_FlowchartDefinitionController<tb_FlowchartDefinition>>();

        /// <summary>
        /// 通过流程名称加载流程图（原有方法）
        /// </summary>
        public void LoadPage(string FlowName)
        {
            //װ
            this.panel1.Controls.Clear();

            //tb_ModuleDefinition entity=ctr.QueryByIdAsync()
            List<tb_FlowchartDefinition> flows = ctrFlowChart.GetFlowCharts(FlowName);
            tb_FlowchartDefinition flow = flows[0];
            LoadFlowPanel(flow);
        }

        /// <summary>
        /// 直接加载流程图对象（新增方法）
        /// </summary>
        public void LoadPage(tb_FlowchartDefinition flow)
        {
            this.panel1.Controls.Clear();
            LoadFlowPanel(flow);
        }

        private void LoadFlowPanel(tb_FlowchartDefinition flow)
        {
            if (flow == null) return;

            FlowPanel fp = new FlowPanel();
            fp.Size = new Size(480, 320);
            fp.Location = new Point(25, this.HeadHeight - 3);
            fp.Dock = DockStyle.Fill;
            fp.Invalidate();
            fp.SetFlowName(flow);
            this.panel1.Controls.Add(fp);
            fp.ItemClick += new EventHandler(fp_ItemClick);
        }


        public event EventHandler ItemClick;

        private void fp_ItemClick(object sender, EventArgs e)
        {
            if (this.ItemClick != null)
                this.ItemClick(sender, e);

        }
    }
}
