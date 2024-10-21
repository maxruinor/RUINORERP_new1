using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RUINOR.WinFormsUI.CustomPictureBox
{

   /// <summary>
   /// 可以放大缩小以及拖动的图片查看器
   /// </summary>
    public partial class frmPictureViewer : Form
    {
        private float zoomFactor = 1.0F; // 初始缩放比例
      //  private Point? zoomOrigin = null; // 缩放的原点
        private float minX = 0; // 滚动的最小 X 值
        private float minY = 0; // 滚动的最小 Y 值
        private bool isDragging = false; // 是否正在拖动
        private Point dragOffset; // 拖动偏移量
        private Point lastMousePosition; // 上一次鼠标位置
     
   

        public frmPictureViewer()
            {
            InitializeComponent();
            // 初始化菜单和 PictureBox
            InitializeControls();
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            PictureBoxViewer.Paint += new PaintEventHandler(PictureBoxViewer_Paint);
            // 注册鼠标事件
            PictureBoxViewer.MouseDown += new MouseEventHandler(pbImage_MouseDown);
            PictureBoxViewer.MouseMove += new MouseEventHandler(pbImage_MouseMove);
            PictureBoxViewer.MouseUp += new MouseEventHandler(pbImage_MouseUp);
            PictureBoxViewer.SizeMode = PictureBoxSizeMode.Normal; // 确保图片可以移动
        }
        private void pbImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePosition = e.Location;
            }
        }

        private void pbImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // 计算鼠标移动的距离
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;

                // 更新拖动偏移量
                dragOffset.X += deltaX;
                dragOffset.Y += deltaY;

                // 更新鼠标位置
                lastMousePosition = e.Location;

                // 重绘 PictureBox
                PictureBoxViewer.Invalidate();
            }
        }

        private void pbImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void PictureBoxViewer_Paint(object sender, PaintEventArgs e)
        {
            if (PictureBoxViewer.Image != null)
            { 
                // 清除 PictureBox 的背景
                e.Graphics.Clear(PictureBoxViewer.BackColor);
                // 计算新的图片大小
                int width = (int)(PictureBoxViewer.Image.Width * zoomFactor);
                int height = (int)(PictureBoxViewer.Image.Height * zoomFactor);
                
                // 计算图像的绘制位置，确保图像在 PictureBox 中居中
                 int x = (PictureBoxViewer.Width - width) / 2 + dragOffset.X;
                 int y = (PictureBoxViewer.Height - height) / 2 + dragOffset.Y;
               
                // 确保图片不会移动出可视范围 如果加上这两行。则会缩定图片的可能移动的范围。不加更自然
                // x = Math.Max(0, Math.Min(x, PictureBoxViewer.Width - width));
                //y = Math.Max(0, Math.Min(y, PictureBoxViewer.Height - height));
                // 绘制缩放后的图像
                 e.Graphics.DrawImage(PictureBoxViewer.Image, new Rectangle(x, y, width, height));
                
            }
        }

   

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            // 检查鼠标是否在 PictureBox 上
            if (PictureBoxViewer.Bounds.Contains(e.X, e.Y))
            {
                if (e.Delta > 0 && zoomFactor < 10.0F) // 向上滚动放大
                {
                    zoomFactor *= 1.1F; // 增加缩放比例
                }
                else if (e.Delta < 0 && zoomFactor > 0.1F) // 向下滚动缩小
                {
                    zoomFactor *= 0.9F; // 减少缩放比例
                }
                // 重绘 PictureBox
                PictureBoxViewer.Invalidate();
            }
        }

      
        private void InitializeControls()
        {
            // 初始化 MenuStrip
            MenuStrip menuStrip1 = new MenuStrip();
            this.Controls.Add(menuStrip1);
            this.MainMenuStrip = menuStrip1;

            // 添加 "Image Size Mode" 菜单项
            ToolStripMenuItem imageSizeModeMenu = new ToolStripMenuItem("图像模式");
            menuStrip1.Items.Add(imageSizeModeMenu);

            // 添加 Normal 子菜单项
            ToolStripMenuItem normalMenuItem = new ToolStripMenuItem("正常");
            imageSizeModeMenu.DropDownItems.Add(normalMenuItem);

            // 添加 AutoSize 子菜单项
            ToolStripMenuItem autoSizeMenuItem = new ToolStripMenuItem("自动");
            imageSizeModeMenu.DropDownItems.Add(autoSizeMenuItem);

            // 添加 CenterImage 子菜单项
            ToolStripMenuItem centerImageMenuItem = new ToolStripMenuItem("居中");
            imageSizeModeMenu.DropDownItems.Add(centerImageMenuItem);

            // 添加 StretchImage 子菜单项
            ToolStripMenuItem stretchImageMenuItem = new ToolStripMenuItem("拉伸");
            imageSizeModeMenu.DropDownItems.Add(stretchImageMenuItem);

            // 添加 Zoom 子菜单项
            ToolStripMenuItem zoomMenuItem = new ToolStripMenuItem("缩放");
            imageSizeModeMenu.DropDownItems.Add(zoomMenuItem);


            PictureBoxViewer.SizeMode = PictureBoxSizeMode.Zoom;


            // 添加事件处理程序
            normalMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.Normal;
            autoSizeMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.AutoSize;
            centerImageMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.CenterImage;
            stretchImageMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.StretchImage;
            zoomMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.Zoom;
        }
        private void frmPictureViewer_Load(object sender, EventArgs e)
        {
            PictureBoxViewer.Invalidate();
        }
    }
}
