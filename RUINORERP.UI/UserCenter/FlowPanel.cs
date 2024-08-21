using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using RUINORERP.Model;
using RUINORERP.Common.Helper;
using RUINORERP.Common.Extensions;
namespace RUINORERP.UI.UserCenter
{
    /// <summary>
    /// FlowPanel 的摘要说明。
    /// </summary>
    public class FlowPanel : PanelView
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

        public FlowPanel()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();

            // TODO: 在 InitializeComponent 调用后添加任何初始化

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
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
            this.SuspendLayout();
            // 
            // FlowPanel
            // 
            this.Name = "FlowPanel";
            this.Size = new System.Drawing.Size(829, 602);
            this.Load += new System.EventHandler(this.FlowPanel_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FlowPanel_Paint);
            this.ResumeLayout(false);

        }
        #endregion

        private void FlowPanel_Load(object sender, System.EventArgs e)
        {
            // 流程图定义.创建采购流程();
            //	define=(流程图定义)流程图定义.Flows[0];

        }
        //画 对应的 流程图
        //流程图定义 define = null;
        tb_FlowchartDefinition define;


        public void SetFlowName(tb_FlowchartDefinition _flow)
        {
            define = _flow;
            //GetFlowCharts
            /*
            流程图定义 nn = (流程图定义)FastObject.PickObject(typeof(流程图定义).FullName, "流程图名称", name);

            DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP 1000 [流程图编号],[流程图名称]  FROM [jx5000].[dbo].[企业管理@自定义桌面@流程图定义] where [流程图名称] ='{0}'", name));
            nn = new 流程图定义();
            nn.流程图编号 = ds.Tables[0].Rows[0][0].ToString().Trim();
            nn.流程图名称 = ds.Tables[0].Rows[0][1].ToString().Trim();

            DataSet dss = DbHelperSQL.Query(string.Format("SELECT TOP 1000 [项目编号],[流程图编号],[PointToString1],[PointToString2]  FROM [jx5000].[dbo].[企业管理@自定义桌面@流程连线项目] where [流程图编号] ='{0}'", nn.流程图编号));
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                流程连线项目 xm = new 流程连线项目();
                xm.项目编号 = dr[0].ToString().Trim();
                xm.流程图编号 = dr[1].ToString().Trim();
                xm.PointToString1 = dr[2].ToString().Trim();
                xm.PointToString2 = dr[3].ToString().Trim();
                nn.连线定义.Add(xm);
            }

            nn.图形项目 = new ArrayList();

            DataSet dssxm = DbHelperSQL.Query(string.Format("SELECT TOP 1000 [项目编号],[流程图编号],[Image],[Title],[SizeString],[PointToString]  FROM [jx5000].[dbo].[企业管理@自定义桌面@流程图项目] where [流程图编号] ='{0}'", nn.流程图编号));
            foreach (DataRow dr in dssxm.Tables[0].Rows)
            {
                流程图项目 xm = new 流程图项目();
                xm.Image = dr[2].ToString().Trim();
                xm.Location = new Point(int.Parse(dr[5].ToString().Trim().Split(':')[0]), int.Parse(dr[5].ToString().Trim().Split(':')[0]));
                xm.SizeString= dr[4].ToString().Trim();
                xm.Title = dr[3].ToString().Trim();
                xm.PointToString= dr[5].ToString().Trim();
                xm.流程图编号= dr[1].ToString().Trim();
                xm.项目编号= dr[0].ToString().Trim();
                nn.图形项目.Add(xm);
            }

            if (nn != null) define = nn;
            */
        }
        private string focusitem = null;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point p = new Point(e.X, e.Y);
    
            //找到 焦点的项目
            int sy = this.HeadHeight;
           
            if (this.define == null) return;

        
            foreach (tb_FlowchartItem item in define.tb_FlowchartItems)
            {
                Point sp = item.PointToString.ToPoint();
                sp.Offset(0, sy);
                // Rectangle rect = new Rectangle(sp.X, sp.Y, xm.Size.Width, xm.Size.Height);
                Rectangle rect = new Rectangle(sp.X, sp.Y, item.SizeString.ToPoint().X, item.SizeString.ToPoint().Y);
                //画标题
                Rectangle r = new Rectangle(sp.X, sp.Y + item.SizeString.ToPoint().Y, item.SizeString.ToPoint().X, this.Font.Height + 3);
                if (rect.Contains(p) || r.Contains(p))
                {
                    this.focusitem = item.Title;
                    this.Invalidate();
                    return;
                }
            }
            if (this.focusitem != null)
            {
                this.focusitem = null;
                this.Invalidate();
            }
             
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (this.ItemClick != null) this.ItemClick(this.focusitem, EventArgs.Empty);

        }
        public event EventHandler ItemClick;


        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);
            if (this.DesignMode) return;
            if (this.define == null) return;
            //在 内容区域 画 图形
            int sy = this.HeadHeight;


            //首先画 流程

            foreach (tb_FlowchartLine line in define.tb_FlowchartLines)
            {
                Point sp = line.PointToString1.ToPoint();// new Point(Convert.ToInt32(stringtools.partof(line.PointToString1, ':', 0)), Convert.ToInt32(stringtools.partof(line.PointToString1, ':', 1));
                Point ep = line.PointToString2.ToPoint();//new Point(Convert.ToInt32(stringtools.partof(line.PointToString2, ':', 0)), Convert.ToInt32(stringtools.partof(line.PointToString2, ':', 1));

                // Point sp = new Point(165, 100);
                // Point ep = new Point(165, 170);

                sp.Offset(0, sy);
                ep.Offset(0, sy);
                this.DrawLine(sp, ep, e);
            }

            /*
            for (int i = 0; i < flow.tb_FlowchartLineses.Count; i++)
            {
            // 流程连线项目 xm = (流程连线项目)define.连线定义[i];
            Point sp = xm.StartPoint;
            Point ep = xm.EndPoint;
            Point sp = new Point(165,100) ;
            Point ep = new Point(165, 170);

            sp.Offset(0, sy);
                ep.Offset(0, sy);
                this.DrawLine(sp, ep, e);
            }
            */



            //画 图形
            foreach (tb_FlowchartItem item in define.tb_FlowchartItems)
            {


                //    流程图项目 xm = (流程图项目)define.图形项目[i];
                //    Point sp = xm.Location;
                Point sp = item.PointToString.ToPoint();//   new Point(48, 48);
                sp.Offset(0, sy);
                Image im = Image.FromFile(Application.StartupPath + "\\" + @"UserCenterUI\" + item.IconFile_Path);
                ImageAttributes ima = new ImageAttributes();
                //Console.WriteLine(xm.Title+":"+this.focusitem);
                string xmTitle = item.Title.Trim();
                 if (this.focusitem == item.Title)
                ima.SetGamma(1.5f);
                e.Graphics.DrawImage(im, new Rectangle(sp.X, sp.Y, item.SizeString.ToPoint().X, item.SizeString.ToPoint().Y), 0, 0, im.Width, im.Height, GraphicsUnit.Pixel, ima);
                //画标题
                Rectangle r = new Rectangle(sp.X, sp.Y + item.SizeString.ToPoint().Y, item.SizeString.ToPoint().X, this.Font.Height + 3);
                r.Inflate(new Size(20, 0));
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                if (xmTitle != null)
                    e.Graphics.DrawString(xmTitle, this.Font, Brushes.Black, r, sf);
            }
            //在 标题区域 画 流程图的名称 
            e.Graphics.DrawString(define.FlowchartName.Trim(), this.Font, Brushes.Black, 3, 3);

        }
        private int linewidth = 7;
        private int arrowwidth = 14;
        public int ArrowWidth
        {
            get
            {
                return this.arrowwidth;
            }
            set
            {
                this.arrowwidth = value;
                this.Invalidate();
            }
        }

        public int LineWidth
        {
            get
            {
                return this.linewidth;
            }
            set
            {
                this.linewidth = value;
                this.Invalidate();
            }
        }
        private Color arrowbordercolor = Color.FromArgb(253, 225, 163);
        private void DrawLine(Point startPoint, Point endPoint, PaintEventArgs e)
        {
            //根据方向 
            Pen p = new Pen(this.arrowbordercolor);
            Brush sb = new LinearGradientBrush(new Point(0, 0), new Point(0, 100), Color.FromArgb(255, 230, 170), Color.FromArgb(255, 255, 230));//,90,false);
            GraphicsPath gp = new GraphicsPath();
            Color ShadowColor = Color.FromArgb(213, 214, 219);
            Color ShadowColor2 = Color.FromArgb(233, 234, 239);
            if (startPoint.Y == endPoint.Y)
            {

                gp.AddLine(startPoint.X, startPoint.Y - this.LineWidth, endPoint.X - this.ArrowWidth, endPoint.Y - this.LineWidth);
                //画 向上的边
                gp.AddLine(endPoint.X - this.ArrowWidth, endPoint.Y - this.LineWidth, endPoint.X - this.ArrowWidth, endPoint.Y - this.ArrowWidth);
                //画 三角的边缘
                gp.AddLine(endPoint.X - this.ArrowWidth, endPoint.Y - this.ArrowWidth, endPoint.X, endPoint.Y);
                //画 下边缘
                gp.AddLine(endPoint.X, endPoint.Y, endPoint.X - this.ArrowWidth, endPoint.Y + this.ArrowWidth);
                gp.AddLine(endPoint.X - this.ArrowWidth, endPoint.Y + this.ArrowWidth, endPoint.X - this.ArrowWidth, endPoint.Y + this.LineWidth);
                gp.AddLine(endPoint.X - this.ArrowWidth, endPoint.Y + this.LineWidth, startPoint.X, endPoint.Y + this.LineWidth);
                gp.AddLine(startPoint.X, startPoint.Y + this.LineWidth, startPoint.X, startPoint.Y - this.LineWidth);

                //在下面画阴影
                e.Graphics.TranslateTransform(0, 1);
                e.Graphics.DrawPath(new Pen(ShadowColor), gp);
                e.Graphics.TranslateTransform(0, 1);
                e.Graphics.DrawPath(new Pen(ShadowColor2), gp);
                e.Graphics.TranslateTransform(0, -2);

            }
            else
            {

                //画  竖直方向的 箭头
                gp.AddLine(startPoint.X - this.LineWidth, startPoint.Y, endPoint.X - this.LineWidth, endPoint.Y - this.ArrowWidth);
                //画 左侧的边
                gp.AddLine(endPoint.X - this.LineWidth, endPoint.Y - this.ArrowWidth, endPoint.X - this.ArrowWidth, endPoint.Y - this.ArrowWidth);
                //画 三角的边缘
                gp.AddLine(endPoint.X - this.ArrowWidth, endPoint.Y - this.ArrowWidth, endPoint.X, endPoint.Y);
                //画 下边缘
                gp.AddLine(endPoint.X, endPoint.Y, endPoint.X + this.ArrowWidth, endPoint.Y - this.ArrowWidth);

                gp.AddLine(endPoint.X + this.ArrowWidth, endPoint.Y - this.ArrowWidth, endPoint.X + this.LineWidth, endPoint.Y - this.ArrowWidth);
                gp.AddLine(endPoint.X + this.LineWidth, endPoint.Y - this.ArrowWidth, endPoint.X + this.LineWidth, startPoint.Y);
                gp.AddLine(endPoint.X + this.LineWidth, startPoint.Y, startPoint.X - this.LineWidth, startPoint.Y);

                e.Graphics.TranslateTransform(1, 0);
                e.Graphics.DrawPath(new Pen(ShadowColor), gp);
                e.Graphics.TranslateTransform(1, 0);
                e.Graphics.DrawPath(new Pen(ShadowColor2), gp);
                e.Graphics.TranslateTransform(-2, 0);
            }

            e.Graphics.FillPath(sb, gp);
            e.Graphics.DrawPath(p, gp);
        }

        private void FlowPanel_Paint(object sender, PaintEventArgs e)
        {
            //OnPaint(e);
        }
    }

}
