using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RUINOR.WinFormsUI.CustomPictureBox
{
    /// <summary>
    /// 可以放大缩小以及拖动的图片查看器
    /// </summary>
    public partial class frmPictureViewer : Form
    {
        private float zoomFactor = 1.0F; // 初始缩放比例
        private float minX = 0; // 滚动的最小 X 值
        private float minY = 0; // 滚动的最小 Y 值
        private bool isDragging = false; // 是否正在拖动
        private Point dragOffset; // 拖动偏移量
        private Point lastMousePosition; // 上一次鼠标位置
        
        // 新增功能变量
        private string currentImagePath = ""; // 当前图片路径
        private StatusStrip statusBar; // 状态栏
        private ToolStripStatusLabel zoomLabel; // 缩放比例标签
        private ToolStripStatusLabel sizeLabel; // 图片尺寸标签
        private ToolStripStatusLabel positionLabel; // 鼠标位置标签
        
        // 多图片支持
        private List<string> imagePaths = new List<string>(); // 图片路径列表
        private int currentImageIndex = 0; // 当前图片索引

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
            
            // 初始化状态栏
            InitializeStatusBar();
        }
        
        // 新增构造函数，支持多图片查看
        public frmPictureViewer(List<string> paths, int startIndex = 0) : this()
        {
            if (paths != null && paths.Count > 0)
            {
                imagePaths = new List<string>(paths);
                currentImageIndex = Math.Max(0, Math.Min(startIndex, paths.Count - 1));
                LoadCurrentImage();
            }
        }
        
        /// <summary>
        /// 加载当前图片
        /// </summary>
        private void LoadCurrentImage()
        {
            if (currentImageIndex >= 0 && currentImageIndex < imagePaths.Count)
            {
                currentImagePath = imagePaths[currentImageIndex];
                try
                {
                    if (File.Exists(currentImagePath))
                    {
                        PictureBoxViewer.Image = Image.FromFile(currentImagePath);
                        UpdateStatusBar();
                        // 重置缩放和拖动
                        zoomFactor = 1.0F;
                        dragOffset = Point.Empty;
                        PictureBoxViewer.Invalidate();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        /// <summary>
        /// 初始化状态栏
        /// </summary>
        private void InitializeStatusBar()
        {
            statusBar = new StatusStrip();
            
            zoomLabel = new ToolStripStatusLabel("缩放: 100%");
            sizeLabel = new ToolStripStatusLabel("尺寸: 0 x 0");
            positionLabel = new ToolStripStatusLabel("位置: 0, 0");
            
            statusBar.Items.Add(zoomLabel);
            statusBar.Items.Add(sizeLabel);
            statusBar.Items.Add(positionLabel);
            
            this.Controls.Add(statusBar);
        }
        
        /// <summary>
        /// 更新状态栏信息
        /// </summary>
        private void UpdateStatusBar()
        {
            if (PictureBoxViewer.Image != null)
            {
                // 更新缩放比例
                zoomLabel.Text = $"缩放: {zoomFactor * 100:F0}%";
                
                // 更新图片尺寸
                sizeLabel.Text = $"尺寸: {PictureBoxViewer.Image.Width} x {PictureBoxViewer.Image.Height}";
                
                // 更新图片文件信息
                if (!string.IsNullOrEmpty(currentImagePath) && File.Exists(currentImagePath))
                {
                    var fileInfo = new FileInfo(currentImagePath);
                    var fileSize = fileInfo.Length;
                    string sizeText;
                    if (fileSize < 1024)
                        sizeText = $"{fileSize} B";
                    else if (fileSize < 1024 * 1024)
                        sizeText = $"{fileSize / 1024.0:F1} KB";
                    else
                        sizeText = $"{fileSize / (1024.0 * 1024.0):F1} MB";
                }
            }
        }
        
        /// <summary>
        /// 加载上一张图片
        /// </summary>
        private void LoadPreviousImage()
        {
            if (imagePaths.Count > 1)
            {
                currentImageIndex = (currentImageIndex - 1 + imagePaths.Count) % imagePaths.Count;
                LoadCurrentImage();
            }
        }
        
        /// <summary>
        /// 加载下一张图片
        /// </summary>
        private void LoadNextImage()
        {
            if (imagePaths.Count > 1)
            {
                currentImageIndex = (currentImageIndex + 1) % imagePaths.Count;
                LoadCurrentImage();
            }
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
            
            // 更新鼠标位置信息
            positionLabel.Text = $"位置: {e.X}, {e.Y}";
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
                
                // 更新状态栏
                UpdateStatusBar();
            }
        }
        
        /// <summary>
        /// 重置视图
        /// </summary>
        private void ResetView()
        {
            zoomFactor = 1.0F;
            dragOffset = Point.Empty;
            PictureBoxViewer.Invalidate();
            UpdateStatusBar();
        }
        
        /// <summary>
        /// 旋转图片
        /// </summary>
        /// <param name="angle">旋转角度</param>
        private void RotateImage(float angle)
        {
            if (PictureBoxViewer.Image != null)
            {
                // 创建新的位图
                var bmp = new Bitmap(PictureBoxViewer.Image.Width, PictureBoxViewer.Image.Height);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
                    g.RotateTransform(angle);
                    g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2);
                    g.DrawImage(PictureBoxViewer.Image, new Point(0, 0));
                }
                
                PictureBoxViewer.Image = bmp;
                UpdateStatusBar();
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
            
            // 添加 "视图" 菜单
            ToolStripMenuItem viewMenu = new ToolStripMenuItem("视图");
            menuStrip1.Items.Add(viewMenu);
            
            // 添加重置视图子菜单项
            ToolStripMenuItem resetViewMenuItem = new ToolStripMenuItem("重置视图");
            viewMenu.DropDownItems.Add(resetViewMenuItem);
            
            // 添加 "导航" 菜单
            ToolStripMenuItem navigateMenu = new ToolStripMenuItem("导航");
            menuStrip1.Items.Add(navigateMenu);
            
            // 添加上一张子菜单项
            ToolStripMenuItem previousMenuItem = new ToolStripMenuItem("上一张");
            navigateMenu.DropDownItems.Add(previousMenuItem);
            
            // 添加下一张子菜单项
            ToolStripMenuItem nextMenuItem = new ToolStripMenuItem("下一张");
            navigateMenu.DropDownItems.Add(nextMenuItem);
            
            // 添加 "旋转" 菜单
            ToolStripMenuItem rotateMenu = new ToolStripMenuItem("旋转");
            menuStrip1.Items.Add(rotateMenu);
            
            // 添加旋转子菜单项
            ToolStripMenuItem rotateLeftMenuItem = new ToolStripMenuItem("向左旋转90度");
            ToolStripMenuItem rotateRightMenuItem = new ToolStripMenuItem("向右旋转90度");
            rotateMenu.DropDownItems.Add(rotateLeftMenuItem);
            rotateMenu.DropDownItems.Add(rotateRightMenuItem);

            PictureBoxViewer.SizeMode = PictureBoxSizeMode.Zoom;

            // 添加事件处理程序
            normalMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.Normal;
            autoSizeMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.AutoSize;
            centerImageMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.CenterImage;
            stretchImageMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.StretchImage;
            zoomMenuItem.Click += (sender, e) => PictureBoxViewer.SizeMode = PictureBoxSizeMode.Zoom;
            
            // 添加新功能事件处理程序
            resetViewMenuItem.Click += (sender, e) => ResetView();
            previousMenuItem.Click += (sender, e) => LoadPreviousImage();
            nextMenuItem.Click += (sender, e) => LoadNextImage();
            rotateLeftMenuItem.Click += (sender, e) => RotateImage(-90);
            rotateRightMenuItem.Click += (sender, e) => RotateImage(90);
        }
        
        private void frmPictureViewer_Load(object sender, EventArgs e)
        {
            PictureBoxViewer.Invalidate();
            UpdateStatusBar();
        }
    }
}