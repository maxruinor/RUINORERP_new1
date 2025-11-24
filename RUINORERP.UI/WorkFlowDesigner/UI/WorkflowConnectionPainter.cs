using Netron.GraphLib;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RUINORERP.UI.WorkFlowDesigner.UI
{
 
    [Serializable]
    public class WorkflowConnectionPainter : ConnectionPainter
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="connection"></param>
        public WorkflowConnectionPainter(Connection connection) : base(connection) { }
        #endregion

        #region Methods
        /// <summary>
        /// Returns whether the default connection is hit by the mouse
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Hit(System.Drawing.PointF p)
        {
            bool join = false;
            //			points = new PointF[2+insertionPoints.Count];
            //			points[0] = p1;
            //			points[2+insertionPoints.Count-1] = p2;
            //			for(int m=0; m<insertionPoints.Count; m++)
            //			{
            //				points[1+m] = (PointF)  insertionPoints[m];
            //			}
            PointF[] points = Points;

            PointF p1 = this.Connection.From.AdjacentPoint;
            PointF p2 = this.Connection.To.AdjacentPoint;

            PointF s;
            float o, u;
            RectangleF r1 = RectangleF.Empty, r2 = RectangleF.Empty, r3 = RectangleF.Empty;

            for (int v = 0; v < points.Length - 1; v++)
            {

                //this is the usual segment test
                //you can do this because the PointF object is a value type!
                p1 = points[v]; p2 = points[v + 1];

                // p1 must be the leftmost point.
                if (p1.X > p2.X) { s = p2; p2 = p1; p1 = s; }

                r1 = new RectangleF(p1.X, p1.Y, 0, 0);
                r2 = new RectangleF(p2.X, p2.Y, 0, 0);
                r1.Inflate(3, 3);
                r2.Inflate(3, 3);
                //this is like a topological neighborhood
                //the connection is shifted left and right
                //and the point under consideration has to be in between.						
                if (RectangleF.Union(r1, r2).Contains(p))
                {
                    if (p1.Y < p2.Y) //SWNE
                    {
                        o = r1.Left + (((r2.Left - r1.Left) * (p.Y - r1.Bottom)) / (r2.Bottom - r1.Bottom));
                        u = r1.Right + (((r2.Right - r1.Right) * (p.Y - r1.Top)) / (r2.Top - r1.Top));
                        join |= ((p.X > o) && (p.X < u));
                    }
                    else //NWSE
                    {
                        o = r1.Left + (((r2.Left - r1.Left) * (p.Y - r1.Top)) / (r2.Top - r1.Top));
                        u = r1.Right + (((r2.Right - r1.Right) * (p.Y - r1.Bottom)) / (r2.Bottom - r1.Bottom));
                        join |= ((p.X > o) && (p.X < u));
                    }
                }


            }
            return join;
        }

        /// <summary>
        /// Paints the connection on the canvas
        /// </summary>
        /// <param name="g"></param>
        public override void Paint(System.Drawing.Graphics g)
        {
            // 获取工作流连接对象
            var workflowConnection = Connection as WorkflowConnection;
            if (workflowConnection == null)
            {
                // 默认绘制方式
                g.DrawLines(Pen, Points);
                return;
            }

            switch (workflowConnection.LineStyleType)
            {
                case ConnectionLineStyle.Straight:
                    PaintStraightLine(g);
                    break;
                case ConnectionLineStyle.Orthogonal:
                    PaintOrthogonalLine(g);
                    break;
                case ConnectionLineStyle.Bezier:
                    PaintBezierCurve(g);
                    break;
                case ConnectionLineStyle.Curved:
                    PaintSmoothCurve(g);
                    break;
                case ConnectionLineStyle.Rounded:
                    PaintRoundedLine(g);
                    break;
                case ConnectionLineStyle.Elbow:
                    PaintElbowLine(g);
                    break;
                case ConnectionLineStyle.Arc:
                    PaintArcLine(g);
                    break;
                default:
                    g.DrawLines(Pen, Points);
                    break;
            }
        }

        /// <summary>
        /// 绘制直线连接
        /// </summary>
        /// <param name="g"></param>
        private void PaintStraightLine(Graphics g)
        {
            if (Points.Length < 2) return;
            
            // 创建画笔
            Pen pen = CreatePen();
            
            // 绘制直线
            g.DrawLines(pen, Points);
        }

        /// <summary>
        /// 绘制直角连接线
        /// </summary>
        /// <param name="g"></param>
        private void PaintOrthogonalLine(Graphics g)
        {
            if (Points.Length < 2) return;
            
            // 创建画笔
            Pen pen = CreatePen();
            
            // 绘制直角连接线
            for (int i = 0; i < Points.Length - 1; i++)
            {
                PointF start = Points[i];
                PointF end = Points[i + 1];
                
                // 绘制水平线段
                g.DrawLine(pen, start.X, start.Y, end.X, start.Y);
                // 绘制垂直线段
                g.DrawLine(pen, end.X, start.Y, end.X, end.Y);
            }
        }

        /// <summary>
        /// 绘制贝塞尔曲线
        /// </summary>
        /// <param name="g"></param>
        private void PaintBezierCurve(Graphics g)
        {
            if (Points.Length < 4) 
            {
                // 点数不足时使用直线绘制
                g.DrawLines(CreatePen(), Points);
                return;
            }
            
            // 创建画笔
            Pen pen = CreatePen();
            
            // 绘制贝塞尔曲线
            g.DrawBeziers(pen, Points);
        }

        /// <summary>
        /// 绘制平滑曲线
        /// </summary>
        /// <param name="g"></param>
        private void PaintSmoothCurve(Graphics g)
        {
            if (Points.Length < 2) return;
            
            // 创建画笔
            Pen pen = CreatePen();
            
            // 绘制平滑曲线
            if (Points.Length == 2)
            {
                g.DrawLine(pen, Points[0], Points[1]);
            }
            else
            {
                g.DrawCurve(pen, Points);
            }
        }

        /// <summary>
        /// 绘制圆角连接线
        /// </summary>
        /// <param name="g"></param>
        private void PaintRoundedLine(Graphics g)
        {
            if (Points.Length < 2) return;
            
            // 获取工作流连接对象
            var workflowConnection = Connection as WorkflowConnection;
            float cornerRadius = workflowConnection?.CornerRadius ?? 10f;
            
            // 创建画笔
            Pen pen = CreatePen();
            
            // 绘制圆角连接线
            using (GraphicsPath path = new GraphicsPath())
            {
                if (Points.Length == 2)
                {
                    path.AddLine(Points[0], Points[1]);
                }
                else
                {
                    // 添加第一个点
                    path.AddLine(Points[0], Points[1]);
                    
                    // 添加中间点（带圆角）
                    for (int i = 1; i < Points.Length - 2; i++)
                    {
                        PointF prev = Points[i - 1];
                        PointF current = Points[i];
                        PointF next = Points[i + 1];
                        
                        // 计算方向向量
                        SizeF dir1 = new SizeF(current.X - prev.X, current.Y - prev.Y);
                        SizeF dir2 = new SizeF(next.X - current.X, next.Y - current.Y);
                        
                        // 计算线段长度
                        float len1 = (float)Math.Sqrt(dir1.Width * dir1.Width + dir1.Height * dir1.Height);
                        float len2 = (float)Math.Sqrt(dir2.Width * dir2.Width + dir2.Height * dir2.Height);
                        
                        // 计算单位向量
                        if (len1 > 0)
                        {
                            dir1.Width /= len1;
                            dir1.Height /= len1;
                        }
                        
                        if (len2 > 0)
                        {
                            dir2.Width /= len2;
                            dir2.Height /= len2;
                        }
                        
                        // 计算圆角控制点
                        float radius = Math.Min(cornerRadius, Math.Min(len1, len2) / 2);
                        PointF start = new PointF(current.X - dir1.Width * radius, current.Y - dir1.Height * radius);
                        PointF end = new PointF(current.X + dir2.Width * radius, current.Y + dir2.Height * radius);
                        
                        // 添加线段和圆弧
                        path.AddLine(prev, start);
                        // 这里简化处理，实际应该计算圆弧
                        path.AddLine(start, end);
                    }
                    
                    // 添加最后一个点
                    path.AddLine(Points[Points.Length - 2], Points[Points.Length - 1]);
                }
                
                g.DrawPath(pen, path);
            }
        }

        /// <summary>
        /// 绘制肘形连接线
        /// </summary>
        /// <param name="g"></param>
        private void PaintElbowLine(Graphics g)
        {
            if (Points.Length < 2) return;
            
            // 创建画笔
            Pen pen = CreatePen();
            
            // 绘制肘形连接线
            for (int i = 0; i < Points.Length - 1; i++)
            {
                PointF start = Points[i];
                PointF end = Points[i + 1];
                
                // 计算肘形点
                PointF elbow;
                if (Math.Abs(start.X - end.X) > Math.Abs(start.Y - end.Y))
                {
                    // 水平方向为主
                    elbow = new PointF(end.X, start.Y);
                }
                else
                {
                    // 垂直方向为主
                    elbow = new PointF(start.X, end.Y);
                }
                
                // 绘制肘形连接
                g.DrawLine(pen, start, elbow);
                g.DrawLine(pen, elbow, end);
            }
        }

        /// <summary>
        /// 绘制弧形连接线
        /// </summary>
        /// <param name="g"></param>
        private void PaintArcLine(Graphics g)
        {
            if (Points.Length < 2) return;
            
            // 获取工作流连接对象
            var workflowConnection = Connection as WorkflowConnection;
            float curvature = workflowConnection?.Curvature ?? 0.5f;
            
            // 创建画笔
            Pen pen = CreatePen();
            
            // 绘制弧形连接线
            for (int i = 0; i < Points.Length - 1; i++)
            {
                PointF start = Points[i];
                PointF end = Points[i + 1];
                
                // 计算控制点
                float dx = end.X - start.X;
                float dy = end.Y - start.Y;
                PointF control = new PointF(
                    start.X + dx / 2 - dy * curvature,
                    start.Y + dy / 2 + dx * curvature);
                
                // 绘制弧线
                g.DrawBezier(pen, start, control, control, end);
            }
        }

        /// <summary>
        /// 创建画笔
        /// </summary>
        /// <returns></returns>
        private Pen CreatePen()
        {
            // 获取工作流连接对象
            var workflowConnection = Connection as WorkflowConnection;
            
            // 创建基础画笔
            Pen pen;
            if (workflowConnection != null && workflowConnection.UseGradient)
            {
                // 使用渐变画笔（简化处理）
                pen = new Pen(workflowConnection.LineColor, workflowConnection.LineWeight == ConnectionWeight.Thin ? 1 :
                    workflowConnection.LineWeight == ConnectionWeight.Medium ? 2 : 3);
            }
            else
            {
                pen = (Pen)Pen.Clone();
            }
            
            return pen;
        }

        #endregion

    }
}