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
    /// FlowPanel ��ժҪ˵����
    /// </summary>
    public class FlowPanel : PanelView
    {
        /// <summary> 
        /// ����������������
        /// </summary>
        private System.ComponentModel.Container components = null;

        public FlowPanel()
        {
            // �õ����� Windows.Forms ���������������ġ�
            InitializeComponent();

            // TODO: �� InitializeComponent ���ú������κγ�ʼ��

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
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
            // ����ͼ����.�����ɹ�����();
            //	define=(����ͼ����)����ͼ����.Flows[0];

        }
        //�� ��Ӧ�� ����ͼ
        //����ͼ���� define = null;
        tb_FlowchartDefinition define;


        public void SetFlowName(tb_FlowchartDefinition _flow)
        {
            define = _flow;
            //GetFlowCharts
            /*
            ����ͼ���� nn = (����ͼ����)FastObject.PickObject(typeof(����ͼ����).FullName, "����ͼ����", name);

            DataSet ds = DbHelperSQL.Query(string.Format("SELECT TOP 1000 [����ͼ���],[����ͼ����]  FROM [jx5000].[dbo].[��ҵ����@�Զ�������@����ͼ����] where [����ͼ����] ='{0}'", name));
            nn = new ����ͼ����();
            nn.����ͼ��� = ds.Tables[0].Rows[0][0].ToString().Trim();
            nn.����ͼ���� = ds.Tables[0].Rows[0][1].ToString().Trim();

            DataSet dss = DbHelperSQL.Query(string.Format("SELECT TOP 1000 [��Ŀ���],[����ͼ���],[PointToString1],[PointToString2]  FROM [jx5000].[dbo].[��ҵ����@�Զ�������@����������Ŀ] where [����ͼ���] ='{0}'", nn.����ͼ���));
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                ����������Ŀ xm = new ����������Ŀ();
                xm.��Ŀ��� = dr[0].ToString().Trim();
                xm.����ͼ��� = dr[1].ToString().Trim();
                xm.PointToString1 = dr[2].ToString().Trim();
                xm.PointToString2 = dr[3].ToString().Trim();
                nn.���߶���.Add(xm);
            }

            nn.ͼ����Ŀ = new ArrayList();

            DataSet dssxm = DbHelperSQL.Query(string.Format("SELECT TOP 1000 [��Ŀ���],[����ͼ���],[Image],[Title],[SizeString],[PointToString]  FROM [jx5000].[dbo].[��ҵ����@�Զ�������@����ͼ��Ŀ] where [����ͼ���] ='{0}'", nn.����ͼ���));
            foreach (DataRow dr in dssxm.Tables[0].Rows)
            {
                ����ͼ��Ŀ xm = new ����ͼ��Ŀ();
                xm.Image = dr[2].ToString().Trim();
                xm.Location = new Point(int.Parse(dr[5].ToString().Trim().Split(':')[0]), int.Parse(dr[5].ToString().Trim().Split(':')[0]));
                xm.SizeString= dr[4].ToString().Trim();
                xm.Title = dr[3].ToString().Trim();
                xm.PointToString= dr[5].ToString().Trim();
                xm.����ͼ���= dr[1].ToString().Trim();
                xm.��Ŀ���= dr[0].ToString().Trim();
                nn.ͼ����Ŀ.Add(xm);
            }

            if (nn != null) define = nn;
            */
        }
        private tb_FlowchartItem _focusedItem = null;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point p = new Point(e.X, e.Y);
    
            //ҵ Ŀ
            int sy = this.HeadHeight;
           
            if (this.define == null) return;

        
            foreach (tb_FlowchartItem item in define.tb_FlowchartItems)
            {
                Point sp = item.PointToString.ToPoint();
                sp.Offset(0, sy);
                Rectangle rect = new Rectangle(sp.X, sp.Y, item.SizeString.ToPoint().X, item.SizeString.ToPoint().Y);
                Rectangle r = new Rectangle(sp.X, sp.Y + item.SizeString.ToPoint().Y, item.SizeString.ToPoint().X, this.Font.Height + 3);
                if (rect.Contains(p) || r.Contains(p))
                {
                    _focusedItem = item;
                    this.Invalidate();
                    return;
                }
            }
            if (_focusedItem != null)
            {
                _focusedItem = null;
                this.Invalidate();
            }
              
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            // 传递完整的tb_FlowchartItem对象，包含MenuID信息
            if (this.ItemClick != null && _focusedItem != null) 
                this.ItemClick(_focusedItem, EventArgs.Empty);
        }
        public event EventHandler ItemClick;


        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);
            if (this.DesignMode) return;
            if (this.define == null) return;
            //�� �������� �� ͼ��
            int sy = this.HeadHeight;


            //���Ȼ� ����

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
            // ����������Ŀ xm = (����������Ŀ)define.���߶���[i];
            Point sp = xm.StartPoint;
            Point ep = xm.EndPoint;
            Point sp = new Point(165,100) ;
            Point ep = new Point(165, 170);

            sp.Offset(0, sy);
                ep.Offset(0, sy);
                this.DrawLine(sp, ep, e);
            }
            */



            //�� ͼ��
            foreach (tb_FlowchartItem item in define.tb_FlowchartItems)
            {


                //    ����ͼ��Ŀ xm = (����ͼ��Ŀ)define.ͼ����Ŀ[i];
                //    Point sp = xm.Location;
                Point sp = item.PointToString.ToPoint();//   new Point(48, 48);
                sp.Offset(0, sy);
                
                // 构建图片路径
                string imagePath = Application.StartupPath + "\\" + @"UserCenterUI\" + item.IconFile_Path;
                
                // 检查图片文件是否存在
                Image im = null;
                if (System.IO.File.Exists(imagePath))
                {
                    im = Image.FromFile(imagePath);
                }
                else
                {
                    // 文件不存在时使用默认图标
                    im = SystemIcons.Application.ToBitmap();
                }
                ImageAttributes ima = new ImageAttributes();
                //System.Diagnostics.Debug.WriteLine(xm.Title+":"+this.focusitem);
                string xmTitle = item.Title.Trim();
                 if (_focusedItem != null && _focusedItem.ID == item.ID)
                ima.SetGamma(1.5f);
                e.Graphics.DrawImage(im, new Rectangle(sp.X, sp.Y, item.SizeString.ToPoint().X, item.SizeString.ToPoint().Y), 0, 0, im.Width, im.Height, GraphicsUnit.Pixel, ima);
                //������
                Rectangle r = new Rectangle(sp.X, sp.Y + item.SizeString.ToPoint().Y, item.SizeString.ToPoint().X, this.Font.Height + 3);
                r.Inflate(new Size(20, 0));
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                if (xmTitle != null)
                    e.Graphics.DrawString(xmTitle, this.Font, Brushes.Black, r, sf);
            }
            //�� �������� �� ����ͼ������ 
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
            //���ݷ��� 
            Pen p = new Pen(this.arrowbordercolor);
            Brush sb = new LinearGradientBrush(new Point(0, 0), new Point(0, 100), Color.FromArgb(255, 230, 170), Color.FromArgb(255, 255, 230));//,90,false);
            GraphicsPath gp = new GraphicsPath();
            Color ShadowColor = Color.FromArgb(213, 214, 219);
            Color ShadowColor2 = Color.FromArgb(233, 234, 239);
            if (startPoint.Y == endPoint.Y)
            {

                gp.AddLine(startPoint.X, startPoint.Y - this.LineWidth, endPoint.X - this.ArrowWidth, endPoint.Y - this.LineWidth);
                //�� ���ϵı�
                gp.AddLine(endPoint.X - this.ArrowWidth, endPoint.Y - this.LineWidth, endPoint.X - this.ArrowWidth, endPoint.Y - this.ArrowWidth);
                //�� ���ǵı�Ե
                gp.AddLine(endPoint.X - this.ArrowWidth, endPoint.Y - this.ArrowWidth, endPoint.X, endPoint.Y);
                //�� �±�Ե
                gp.AddLine(endPoint.X, endPoint.Y, endPoint.X - this.ArrowWidth, endPoint.Y + this.ArrowWidth);
                gp.AddLine(endPoint.X - this.ArrowWidth, endPoint.Y + this.ArrowWidth, endPoint.X - this.ArrowWidth, endPoint.Y + this.LineWidth);
                gp.AddLine(endPoint.X - this.ArrowWidth, endPoint.Y + this.LineWidth, startPoint.X, endPoint.Y + this.LineWidth);
                gp.AddLine(startPoint.X, startPoint.Y + this.LineWidth, startPoint.X, startPoint.Y - this.LineWidth);

                //�����滭��Ӱ
                e.Graphics.TranslateTransform(0, 1);
                e.Graphics.DrawPath(new Pen(ShadowColor), gp);
                e.Graphics.TranslateTransform(0, 1);
                e.Graphics.DrawPath(new Pen(ShadowColor2), gp);
                e.Graphics.TranslateTransform(0, -2);

            }
            else
            {

                //��  ��ֱ����� ��ͷ
                gp.AddLine(startPoint.X - this.LineWidth, startPoint.Y, endPoint.X - this.LineWidth, endPoint.Y - this.ArrowWidth);
                //�� ���ı�
                gp.AddLine(endPoint.X - this.LineWidth, endPoint.Y - this.ArrowWidth, endPoint.X - this.ArrowWidth, endPoint.Y - this.ArrowWidth);
                //�� ���ǵı�Ե
                gp.AddLine(endPoint.X - this.ArrowWidth, endPoint.Y - this.ArrowWidth, endPoint.X, endPoint.Y);
                //�� �±�Ե
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
