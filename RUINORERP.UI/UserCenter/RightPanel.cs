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
    /// RightPanel 的摘要说明。
    /// </summary>
    public class RightPanel : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.Panel panel1;
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

        public RightPanel()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();

            // TODO: 在 InitializeComponent 调用后添加任何初始化

        }

        /// <summary> 
        /// 清理所有正在使用的资源。
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

        #region 组件设计器生成的代码
        /// <summary> 
        /// 设计器支持所需的方法 - 不要使用代码编辑器 
        /// 修改此方法的内容。
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
        private string titletext = "企业管理";
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
        private Font titlefont = new Font("楷体", 12);
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
            //设定 画的颜色
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

                //画 两条线
                e.Graphics.DrawLine(Pens.Gray, 0, this.HeadHeight, this.Width, this.HeadHeight);
                e.Graphics.DrawLine(Pens.White, 0, this.HeadHeight + 1, this.Width, this.HeadHeight + 1);
                e.Graphics.DrawString(this.titletext, this.TitleFont, Brushes.White, 10, 7);

                Color scolor = this.Mix(this.TitleColor, 0.7);
                //画 左上 的 深色边缘

                gp = new GraphicsPath();
                gp.AddLine(0, this.Height, 0, this.ArcSize);
                gp.AddArc(0, 0, this.ArcSize, this.ArcSize, 180, 90);
                gp.AddLine(this.ArcSize, 0, this.Width, 0);
                e.Graphics.DrawPath(new Pen(scolor), gp);

                scolor = this.TitleColor;
                //画 左上 的 深色边缘

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
		public void LoadPage(页面定义 pagedefine)
		{
			//装入 对象

			this.panel1.Controls.Clear();
			//
			int sy=this.HeadHeight-3;
			int lsy=sy;
			int rsy=sy;
			int lsx=25;
			//int rsx=250;

			for (int i=0;i<pagedefine.模块.Count;i++)
			{
				页面模块定义  md=(页面模块定义)pagedefine.模块[i];
				//添加 到右侧
				switch (md.基础模块名称)
				{
					case "FlowPanel":
						FlowPanel fp=new FlowPanel();
						fp.Size=new Size(480,320);
						fp.Location=new Point(lsx,lsy);
						fp.SetFlowName(md.参数);
						this.panel1.Controls.Add(fp);
						fp.ItemClick+=new EventHandler(fp_ItemClick);
						break;
					default:break;
				}
			}

			
//			//添加一个 OfficePanel
//			Office2003Header mh=new  Office2003Header();
//			mh.TitleText="操作提示";
//			mh.Size=new Size(200,150);
//			mh.Location=new Point(600,20);
//			this.panel1.Controls.Add(mh);
//
//			Office2003HeaderSub  oh=new Office2003HeaderSub();
//			oh.TitleText="提示";
//			oh.Size=new Size(200,150);
//			oh.Location=new Point(0,25);
//			oh.BackColor=Color.White;
//			mh.Controls.Add(oh);
//			Label l=new Label();
//			l.Text="使用流程图直接访问单据";
//			l.Dock=DockStyle.Fill;
//			oh.Controls.Add(l);
//			l.TextAlign=ContentAlignment.MiddleCenter;
			
		}
		*/
        tb_ModuleDefinitionController<tb_ModuleDefinition> ctr = Startup.GetFromFac<tb_ModuleDefinitionController<tb_ModuleDefinition>>();
        tb_FlowchartDefinitionController<tb_FlowchartDefinition> ctrFlowChart = Startup.GetFromFac<tb_FlowchartDefinitionController<tb_FlowchartDefinition>>();
        public void LoadPage(string FlowName)
        {
            //装入 对象
            this.panel1.Controls.Clear();

            //tb_ModuleDefinition entity=ctr.QueryByIdAsync()
            List<tb_FlowchartDefinition> flows = ctrFlowChart.GetFlowCharts(FlowName);
            tb_FlowchartDefinition flow = flows[0];
            int sy = this.HeadHeight - 3;
            int lsy = sy;
            int rsy = sy;
            int lsx = 25;
            //int rsx=250;

            //for (int i = 0; i < pagedefine.模块.Count; i++)
            //{
            //	页面模块定义 md = (页面模块定义)pagedefine.模块[i];
            //	//添加 到右侧
            //	switch (md.基础模块名称)
            //	{
            //		case "FlowPanel":
            FlowPanel fp = new FlowPanel();
            fp.Size = new Size(480, 320);
            fp.Location = new Point(lsx, lsy);
            fp.Dock = DockStyle.Fill;
            fp.Invalidate();
            fp.SetFlowName(flow);
            this.panel1.Controls.Add(fp);
            fp.ItemClick += new EventHandler(fp_ItemClick);
            //			break;
            //		default: break;
            //	}
            //}


            //			//添加一个 OfficePanel
            //			Office2003Header mh=new  Office2003Header();
            //			mh.TitleText="操作提示";
            //			mh.Size=new Size(200,150);
            //			mh.Location=new Point(600,20);
            //			this.panel1.Controls.Add(mh);
            //
            //			Office2003HeaderSub  oh=new Office2003HeaderSub();
            //			oh.TitleText="提示";
            //			oh.Size=new Size(200,150);
            //			oh.Location=new Point(0,25);
            //			oh.BackColor=Color.White;
            //			mh.Controls.Add(oh);
            //			Label l=new Label();
            //			l.Text="使用流程图直接访问单据";
            //			l.Dock=DockStyle.Fill;
            //			oh.Controls.Add(l);
            //			l.TextAlign=ContentAlignment.MiddleCenter;

        }


        public event EventHandler ItemClick;

        private void fp_ItemClick(object sender, EventArgs e)
        {
            if (this.ItemClick != null)
                this.ItemClick(sender, e);

        }
    }
}
