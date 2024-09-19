using RUINORERP.Global;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    public class GDIHelper
    {
        private static GDIHelper m_instance;

        public static GDIHelper Instance
        {
            get
            {
                if (m_instance == null)
                {
                    Initialize();
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }


        /// <summary>
        /// 对象实例化
        /// </summary>
        public static void Initialize()
        {
            m_instance = new GDIHelper();
        }


        /// <summary>
        /// 判断行的数据是否有箱规信息的依据
        /// 并且返回箱规信息，如果指定到了SKU，则以SKU为标准。否则是他的上级。
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public BoxRuleBasis CheckForBoxSpecBasis(DataGridViewRow dr, out tb_Packing packing)
        {
            //给个默认值
            packing = new tb_Packing();

            BoxRuleBasis reuleBasis = BoxRuleBasis.None;
            if (dr.DataBoundItem is View_ProdDetail prodDetail)
            {
                if (prodDetail.tb_Packing_forSku.Count > 0)
                {
                    reuleBasis = BoxRuleBasis.Attributes;
                    packing= prodDetail.tb_Packing_forSku.FirstOrDefault();
                }
                if (prodDetail.tb_prod.tb_Packings.Count > 0)
                {
                    reuleBasis = BoxRuleBasis.Product;
                    packing = prodDetail.tb_prod.tb_Packings.FirstOrDefault();
                }
                if (prodDetail.tb_Packing_forSku.Count > 0 && prodDetail.tb_prod.tb_Packings.Count > 0)
                {
                    reuleBasis = BoxRuleBasis.Product | BoxRuleBasis.Attributes;
                    //如果都有，如果指定到了SKU，则以SKU为标准。否则是他的上级。
                    packing = prodDetail.tb_Packing_forSku.FirstOrDefault();
                }
            }

            if (dr.DataBoundItem is tb_Prod prod)
            {
                if (prod.tb_Packings == null || prod.tb_Packings.Count == 0)
                {
                    reuleBasis = BoxRuleBasis.None;
                }
                else
                {
                    if (prod.tb_Packings.Count > 0)
                    {
                        reuleBasis = BoxRuleBasis.Product;
                        packing = prod.tb_Packings.FirstOrDefault();
                    }
                }
            }
            return reuleBasis;
        }



        /// <summary>
        /// 画正方形
        /// </summary>
        /// <param name="e"></param>
        public void DrawPattern(DataGridViewCellPaintingEventArgs e)
        {
            SolidBrush brush = new SolidBrush(Color.DarkViolet); // 选择你的颜色
            Pen pen = new Pen(brush);

            // 定义正方形的边长
            int size = 10; // 正方形的边长
                           // 整体向左边平移10像素
            int horizontalShift = 5;

            // 计算正方形的新位置，左边界加上平移量
            int left = e.CellBounds.Left + horizontalShift;
            int top = e.CellBounds.Top + (e.CellBounds.Height - size) / 2; // 垂直居中

            // 计算正方形的四个顶点
            Point point1 = new Point(left, top);
            Point point2 = new Point(left + size - 1, top); // -1 为了避免边界绘制问题
            Point point3 = new Point(left + size - 1, top + size - 1);
            Point point4 = new Point(left, top + size - 1);

            // 绘制并填充正方形
            e.Graphics.FillPolygon(brush, new Point[] { point1, point2, point3, point4 });
            e.Graphics.DrawPolygon(pen, new Point[] { point1, point2, point3, point4 });

            // 阻止DataGridView继续绘制该单元格
            e.Handled = true;
        }

        public void DrawPattern(DataGridViewCellPaintingEventArgs e, Color color)
        {
            //SolidBrush brush = new SolidBrush(Color.DarkGreen); // 选择你的颜色
            SolidBrush brush = new SolidBrush(color); // 选择你的颜色
            Pen pen = new Pen(brush);

            // 绘制图案，例如一个简单的三角形
            {
                #region  绘制图案



                // 定义三角形的尺寸
                int size = 10; // 三角形的底边长度

                // 整体向左平移5像素
                int horizontalShift = 5;

                // 计算三角形的中心点坐标，整体下移7个像素
                int centerX = (e.CellBounds.Left + e.CellBounds.Width / 2) - horizontalShift;
                int topY = e.CellBounds.Top + 7; // 顶点在上方，整体下移7个像素

                // 绘制正三角形
                Point[] points = new Point[]
                {
                    // 顶点位于顶部中心，整体下移7个像素
                    new Point(centerX, topY),
                    // 底边的两个角分别位于单元格的左右两侧，整体下移7个像素
                    new Point(centerX - size / 2, topY + size), // 左边的点
                    new Point(centerX + size / 2, topY + size)  // 右边的点
                };
                e.Graphics.FillPolygon(brush, points);
                e.Graphics.DrawPolygon(pen, points);


                #endregion

                // 阻止DataGridView继续绘制该单元格
                e.Handled = true;
            }
        }


        public void DrawPatternTick(DataGridViewCellPaintingEventArgs e, Color color)
        {
            // 创建一个画笔用于填充勾的颜色
            SolidBrush brush = new SolidBrush(color);
            // 创建一个钢笔用于勾的边框
            Pen pen = new Pen(brush);

            // 定义勾的尺寸和位置
            int 勾的宽度 = 6; // 勾的宽度
            int 勾的高度 = 10; // 勾的高度
            int 勾的X = e.CellBounds.Left + (e.CellBounds.Width - 勾的宽度) / 2; // 勾的X坐标
            int 勾的Y = e.CellBounds.Top + (e.CellBounds.Height - 勾的高度) / 2; // 勾的Y坐标

            // 创建一个GraphicsPath对象用于定义勾的路径
            GraphicsPath path = new GraphicsPath();
            // 定义勾的起点
            path.StartFigure();
            path.AddLine(勾的X, 勾的Y + 勾的高度 / 2, 勾的X + 勾的宽度 / 2, 勾的Y);
            path.AddLine(勾的X + 勾的宽度 / 2, 勾的Y, 勾的X + 勾的宽度, 勾的Y + 勾的高度 / 2);
            path.CloseFigure();

            // 绘制勾
            e.Graphics.FillPath(brush, path);
            e.Graphics.DrawPath(pen, path);

            // 阻止DataGridView继续绘制该单元格
            e.Handled = true;
        }

    }
}
