using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using HLH.Lib.Draw;
using System.Reflection;

namespace RUINOR.WinFormsUI.CustomPictureBox
{
    /// <summary>
    /// 自定义PictureBox控件
    /// </summary>
    public class MagicPictureBox : PictureBox
    {
        private bool isPanning = false;
        private Point panOffset;
        private float rotationAngle = 0f;
        private bool isCropping = false;
        private Rectangle cropRectangle;
        private Cursor cropCursor = new Cursor(System.Windows.Forms.Cursors.Cross.Handle);
        private float zoomFactor = 1.0F; // 初始缩放比例
        private float minX = 0; // 滚动的最小 X 值
        private float minY = 0; // 滚动的最小 Y 值
        private bool isDragging = false; // 是否正在拖动
        private Point dragOffset; // 拖动偏移量
        private Point lastMousePosition; // 上一次鼠标位置

        // 设置右键菜单
        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        ImageCroppingBox imageCroppingBox1 = new ImageCroppingBox();

        ContextMenuStrip contextMenuStripForCrop = new ContextMenuStrip();
        public MagicPictureBox() : base()
        {
            // 设置允许拖拽
            this.AllowDrop = true;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.SizeMode = PictureBoxSizeMode.Normal; // 确保图片可以移动

            // 设置双击事件
            this.DoubleClick += CustomPictureBox_DoubleClick;

            contextMenuStrip.Items.Add("粘贴图片", null, PasteImage);

            this.ContextMenuStrip = contextMenuStrip;


            this.Paint += new PaintEventHandler(PictureBoxViewer_Paint);

            this.MouseDown += CustomPictureBox_MouseDown;
            this.MouseMove += CustomPictureBox_MouseMove;
            this.MouseUp += CustomPictureBox_MouseUp;
            this.MouseWheel += CustomPictureBox_MouseWheel;


        }


        //动态添加右键菜单
        private void AddContextMenuItems()
        {
            if (!contextMenuStrip.Items.ContainsKey("查看大图"))
            {
                contextMenuStrip.Items.Add(new ToolStripMenuItem("查看大图", null, new EventHandler(ViewLargeImage), "查看大图"));
            }
            if (!contextMenuStrip.Items.ContainsKey("清除图片"))
            {
                contextMenuStrip.Items.Add(new ToolStripMenuItem("清除图片", null, new EventHandler(ClearImage), "清除图片"));
            }
            if (!contextMenuStrip.Items.ContainsKey("裁剪图片"))
            {
                imageCroppingBox1.Size = this.Size;
                //imageCroppingBox1.Location = this.Location;
                imageCroppingBox1.Visible = false;
                imageCroppingBox1.ContextMenuStrip = contextMenuStripForCrop;
                imageCroppingBox1.DoubleClick += new EventHandler(SaveCrop);
                this.Controls.Add(imageCroppingBox1);
                this.imageCroppingBox1.TabIndex = 3;
                contextMenuStrip.Items.Add(new ToolStripMenuItem("裁剪图片", null, new EventHandler(StartCrop), "裁剪图片"));
            }
            if (!contextMenuStripForCrop.Items.ContainsKey("取消裁剪"))
            {
                contextMenuStripForCrop.Items.Add(new ToolStripMenuItem("取消裁剪", null, new EventHandler(StopCrop), "取消裁剪"));
            }
        }

        private void ClearImage(object sender, EventArgs e)
        {
            this.Image = null;
            // 重绘 PictureBox
            this.Invalidate();
        }

        private void PictureBoxViewer_Paint(object sender, PaintEventArgs e)
        {
            if (this.Image != null)
            {
                // 清除 PictureBox 的背景
                e.Graphics.Clear(this.BackColor);
                // 计算新的图片大小
                int width = (int)(this.Image.Width * zoomFactor);
                int height = (int)(this.Image.Height * zoomFactor);

                // 计算图像的绘制位置，确保图像在 PictureBox 中居中
                int x = (this.Width - width) / 2 + dragOffset.X;
                int y = (this.Height - height) / 2 + dragOffset.Y;

                // 确保图片不会移动出可视范围 如果加上这两行。则会缩定图片的可能移动的范围。不加更自然
                // x = Math.Max(0, Math.Min(x, PictureBoxViewer.Width - width));
                //y = Math.Max(0, Math.Min(y, PictureBoxViewer.Height - height));
                // 绘制缩放后的图像
                e.Graphics.DrawImage(this.Image, new Rectangle(x, y, width, height));

            }
        }


        /// <summary>
        /// 裁剪
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartCrop(object sender, EventArgs e)
        {
            if (this.Image != null)
            {
                isCropping = true;
                cropRectangle = new Rectangle(new Point(0, 0), new Size(this.Image.Width, this.Image.Height));
                this.Cursor = cropCursor;
                this.imageCroppingBox1.Visible = true;
                this.imageCroppingBox1.TabIndex = this.TabIndex + 1;
                this.imageCroppingBox1.Image = this.Image;
            }
        }


        private void SaveCrop(object sender, EventArgs e)
        {
            if (this.Image != null)
            {
                if (isCropping)
                {
                    isCropping = false;
                    this.Cursor = Cursors.Default;
                    cropRectangle = new Rectangle(0, 0, 0, 0);
                    cropRectangle = Rectangle.Empty;
                }
                this.Visible = true;
                this.imageCroppingBox1.Visible = false;
                this.Image = this.imageCroppingBox1.GetSelectedImage();
            }
        }

        private void StopCrop(object sender, EventArgs e)
        {
            if (this.Image != null)
            {
                if (isCropping)
                {
                    isCropping = false;
                    this.Cursor = Cursors.Default;
                    cropRectangle = new Rectangle(0, 0, 0, 0);
                    cropRectangle = Rectangle.Empty;
                }
                this.Visible = true;
                this.imageCroppingBox1.Visible = false;
            }
        }
        // 拖拽事件
        protected override void OnDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            base.OnDragEnter(e);
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                Image image = Image.FromFile(files[0]);
                this.Image = image;
            }
            base.OnDragDrop(e);
        }


        // 粘贴图片事件
        private void PasteImage(object sender, EventArgs e)
        {
            // this.CanFocus
            if (Clipboard.ContainsImage())
            {
                Image image = Clipboard.GetImage();
                this.Image = image;
                AddContextMenuItems();
            }
        }

        private void ViewLargeImage(object sender, EventArgs e)
        {
            if (this.Image != null)
            {
                frmPictureViewer frmShow = new frmPictureViewer();
                frmShow.PictureBoxViewer.Image = this.Image;
                frmShow.ShowDialog();
            }
        }

        // 双击事件
        private void CustomPictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (this.Image == null)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "选择图片";
                    openFileDialog.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.Image = Image.FromFile(openFileDialog.FileName);
                    }
                }
            }
            else
            {
                // 显示图片，这里只是简单地在PictureBox中显示，你可以根据需要实现更复杂的显示逻辑
                AddContextMenuItems();
                //如果是裁剪状态，则不显示大图。双击将截取的图片替换到控件中。并退出裁剪状态
                if (isCropping)
                {
                    this.Image = CropImage(this.Image, cropRectangle);
                    isCropping = false;
                }
                else
                {
                    ViewLargeImage(sender, e);
                }

            }
        }


        /// <summary>
        /// 根据鼠标位置，计算裁剪框的位置和大小
        /// </summary>
        /// <param name="img"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private Image CropImage(Image img, Rectangle rect)
        {
            if (rect.Width > 0 && rect.Height > 0)
            {
                ImageHelper.MakeThumbnailfromImage(img, rect.Width, rect.Height);
                //return img.Clone(rect, img.PixelFormat);
            }
            return img;
        }

        private void StartPan(object sender, EventArgs e)
        {
            isPanning = true;
        }

        private void CustomPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPanning = true;
                if (isPanning && this.Image != null)
                {
                    panOffset = e.Location;
                    lastMousePosition = e.Location;
                }
                else if (isCropping)
                {
                    cropRectangle.Location = e.Location;
                }
            }

        }


        private void CustomPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPanning)
            {
                // 计算鼠标移动的距离
                int newX = e.X - lastMousePosition.X;
                int newY = e.Y - lastMousePosition.Y;

                // 更新拖动偏移量
                dragOffset.X += newX;
                dragOffset.Y += newY;

                // 更新鼠标位置
                lastMousePosition = e.Location;

                // 重绘 PictureBox
                this.Invalidate();


            }
            else if (isCropping && e.Button == MouseButtons.Left)
            {
                cropRectangle.Size = new Size(Math.Abs(e.X - cropRectangle.Left), Math.Abs(e.Y - cropRectangle.Top));
                this.Invalidate(); // Redraw the control to show the crop rectangle
            }
        }

        private void CustomPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (isPanning)
            {
                isPanning = false;
            }
            else if (isCropping && e.Button == MouseButtons.Left)
            {
                isCropping = false;
                this.Cursor = Cursors.Default;
                if (cropRectangle.Width > 0 && cropRectangle.Height > 0)
                {
                    this.Image = CropImage(this.Image, cropRectangle);
                    cropRectangle = Rectangle.Empty;
                }
            }
        }

        /// <summary>
        /// 缩放图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomPictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            // 检查鼠标是否在 PictureBox 上
            if (this.Bounds.Contains(e.X, e.Y))
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
                this.Invalidate();
            }

        }

        /// <summary>
        /// 裁剪图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RotateImage(object sender, EventArgs e)
        {
            if (this.Image != null)
            {
                rotationAngle += 90;
                this.Image = RotateImage(this.Image, rotationAngle);
            }
        }

        /// <summary>
        /// 旋转图像
        /// </summary>
        /// <param name="img"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Image RotateImage(Image img, float angle)
        {
            // Rotate the image around its center
            using (var graphics = Graphics.FromImage(img))
            {
                graphics.TranslateTransform(img.Width / 2, img.Height / 2);
                graphics.RotateTransform(angle);
                graphics.TranslateTransform(-img.Width / 2, -img.Height / 2);
                var bmp = new Bitmap(img.Width, img.Height);
                graphics.DrawImage(img, new Point(0, 0));
                return bmp;
            }
        }
    }
}