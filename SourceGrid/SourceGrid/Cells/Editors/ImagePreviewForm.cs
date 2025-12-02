using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace SourceGrid.Cells.Editors
{
    /// <summary>
    /// 图片预览窗体
    /// 支持图片查看、缩放、旋转、保存等基本操作
    /// 独立于业务逻辑，专注于图片显示功能
    /// </summary>
    public partial class ImagePreviewForm : Form
    {
        #region 私有字段

        private System.Drawing.Image _originalImage;
        private System.Drawing.Image _displayImage;
        private float _zoomFactor = 1.0f;
        private Point _lastMousePos;
        private bool _isDragging = false;
        private int _rotationAngle = 0;

        // 缩放相关常量
        private const float ZoomStep = 0.1f;
        private const float MinZoom = 0.1f;
        private const float MaxZoom = 5.0f;

        #endregion

        #region 构造函数

        public ImagePreviewForm()
        {
            InitializeComponent();
            SetupForm();
        }

        public ImagePreviewForm(System.Drawing.Image image) : this()
        {
            SetImage(image);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 设置要显示的图片
        /// </summary>
        /// <param name="image">图片对象</param>
        public void SetImage(System.Drawing.Image image)
        {
            try
            {
                if (_originalImage != null)
                {
                    _originalImage.Dispose();
                }

                _originalImage = image;
                _displayImage = (System.Drawing.Image)image.Clone();
                _zoomFactor = 1.0f;
                _rotationAngle = 0;

                UpdateDisplay();
                UpdateInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设置图片失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 从文件路径加载图片
        /// </summary>
        /// <param name="filePath">图片文件路径</param>
        /// <returns>是否加载成功</returns>
        public bool LoadImageFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("图片文件不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                using (var tempImage = System.Drawing.Image.FromFile(filePath))
                {
                    SetImage(tempImage);
                }

                Text = $"图片预览 - {Path.GetFileName(filePath)}";
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图片文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 设置窗体基本属性
        /// </summary>
        private void SetupForm()
        {
            this.Text = "图片预览";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.DoubleBuffered = true;
            this.MinimumSize = new Size(400, 300);
        }

        /// <summary>
        /// 更新显示
        /// </summary>
        private void UpdateDisplay()
        {
            if (_displayImage != null)
            {
                pictureBox.Image = _displayImage;
                CenterImage();
            }
        }

        /// <summary>
        /// 居中显示图片
        /// </summary>
        private void CenterImage()
        {
            if (pictureBox.Image == null) return;

            var panelSize = panelMain.ClientSize;
            var imageSize = new Size(
                (int)(pictureBox.Image.Width * _zoomFactor),
                (int)(pictureBox.Image.Height * _zoomFactor));

            // 计算居中位置
            int x = Math.Max(0, (panelSize.Width - imageSize.Width) / 2);
            int y = Math.Max(0, (panelSize.Height - imageSize.Height) / 2);

            pictureBox.Location = new Point(x, y);
            pictureBox.Size = imageSize;
        }

        /// <summary>
        /// 更新信息显示
        /// </summary>
        private void UpdateInfo()
        {
            if (_originalImage != null)
            {
                lblInfo.Text = $"尺寸: {_originalImage.Width} × {_originalImage.Height} | " +
                              $"缩放: {_zoomFactor:P0} | " +
                              $"旋转: {_rotationAngle}°";
            }
            else
            {
                lblInfo.Text = "无图片";
            }
        }

        /// <summary>
        /// 应用变换（缩放和旋转）
        /// </summary>
        private void ApplyTransform()
        {
            if (_originalImage == null) return;

            if (_displayImage != null)
            {
                _displayImage.Dispose();
            }

            // 创建新的位图
            var originalSize = _originalImage.Size;
            var rotatedSize = CalculateRotatedSize(originalSize, _rotationAngle);
            _displayImage = new Bitmap(rotatedSize.Width, rotatedSize.Height);

            using (var graphics = Graphics.FromImage(_displayImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // 应用变换
                graphics.TranslateTransform(rotatedSize.Width / 2.0f, rotatedSize.Height / 2.0f);
                graphics.RotateTransform(_rotationAngle);
                graphics.ScaleTransform(_zoomFactor, _zoomFactor);
                graphics.TranslateTransform(-originalSize.Width / 2.0f, -originalSize.Height / 2.0f);

                graphics.DrawImage(_originalImage, 0, 0, originalSize.Width, originalSize.Height);
            }

            UpdateDisplay();
            UpdateInfo();
        }

        /// <summary>
        /// 计算旋转后的尺寸
        /// </summary>
        /// <param name="originalSize">原始尺寸</param>
        /// <param name="angle">旋转角度</param>
        /// <returns>旋转后的尺寸</returns>
        private Size CalculateRotatedSize(Size originalSize, int angle)
        {
            double radians = Math.Abs(angle * Math.PI / 180.0);
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            int width = (int)Math.Ceiling(Math.Abs(originalSize.Width * cos) + Math.Abs(originalSize.Height * sin));
            int height = (int)Math.Ceiling(Math.Abs(originalSize.Width * sin) + Math.Abs(originalSize.Height * cos));

            return new Size(width, height);
        }

        /// <summary>
        /// 重置图片到原始状态
        /// </summary>
        private void ResetImage()
        {
            _zoomFactor = 1.0f;
            _rotationAngle = 0;
            ApplyTransform();
        }

        #endregion

        #region 事件处理

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            _zoomFactor = Math.Min(_zoomFactor + ZoomStep, MaxZoom);
            ApplyTransform();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            _zoomFactor = Math.Max(_zoomFactor - ZoomStep, MinZoom);
            ApplyTransform();
        }

        private void btnActualSize_Click(object sender, EventArgs e)
        {
            _zoomFactor = 1.0f;
            ApplyTransform();
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            _rotationAngle = (_rotationAngle - 90) % 360;
            ApplyTransform();
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            _rotationAngle = (_rotationAngle + 90) % 360;
            ApplyTransform();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_displayImage == null)
            {
                MessageBox.Show("没有可保存的图片", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "JPEG 图片|*.jpg|PNG 图片|*.png|BMP 图片|*.bmp|所有文件|*.*",
                FilterIndex = 1,
                Title = "保存图片",
                FileName = $"Image_{DateTime.Now:yyyyMMddHHmmss}.jpg"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 根据文件扩展名选择保存格式
                    string extension = Path.GetExtension(saveDialog.FileName).ToLowerInvariant();
                    ImageFormat format;
                    switch (extension)
                    {
                        case ".png":
                            format = ImageFormat.Png;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                        default:
                            format = ImageFormat.Jpeg;
                            break;
                    }

                    _displayImage.Save(saveDialog.FileName, format);
                    MessageBox.Show("图片保存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存图片失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panelMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _zoomFactor > 1.0f)
            {
                _isDragging = true;
                _lastMousePos = e.Location;
                panelMain.Cursor = Cursors.Hand;
            }
        }

        private void panelMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && pictureBox.Image != null)
            {
                int dx = e.X - _lastMousePos.X;
                int dy = e.Y - _lastMousePos.Y;

                pictureBox.Location = new Point(
                    pictureBox.Location.X + dx,
                    pictureBox.Location.Y + dy);

                _lastMousePos = e.Location;
            }
        }

        private void panelMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
                panelMain.Cursor = Cursors.Default;
            }
        }

        private void panelMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                // 根据滚轮方向缩放
                float zoomDelta = e.Delta > 0 ? ZoomStep : -ZoomStep;
                _zoomFactor = Math.Max(MinZoom, Math.Min(MaxZoom, _zoomFactor + zoomDelta));
                ApplyTransform();
            }
        }

        /// <summary>
        /// 处理窗体级别的鼠标滚轮事件，支持在任何位置缩放
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta != 0 && _originalImage != null)
            {
                // 根据滚轮方向缩放
                float zoomDelta = e.Delta > 0 ? ZoomStep : -ZoomStep;
                _zoomFactor = Math.Max(MinZoom, Math.Min(MaxZoom, _zoomFactor + zoomDelta));
                ApplyTransform();
                
                // 标记为已处理，避免默认滚动行为
                ((HandledMouseEventArgs)e).Handled = true;
            }
            base.OnMouseWheel(e);
        }

        private void ImagePreviewForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.Add:
                case Keys.Oemplus:
                    btnZoomIn_Click(null, null);
                    break;
                case Keys.Subtract:
                case Keys.OemMinus:
                    btnZoomOut_Click(null, null);
                    break;
                case Keys.D0:
                    btnActualSize_Click(null, null);
                    break;
                case Keys.Left:
                    btnRotateLeft_Click(null, null);
                    break;
                case Keys.Right:
                    btnRotateRight_Click(null, null);
                    break;
                case Keys.S:
                    if (e.Control)
                        btnSave_Click(null, null);
                    break;
            }
        }

        #endregion

        #region 窗体初始化

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 创建工具栏
            var toolPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.LightGray
            };

            btnZoomIn = new System.Windows.Forms.Button { Text = "放大+", Width = 60, Top = 5, Left = 10 };
            btnZoomOut = new System.Windows.Forms.Button { Text = "缩小-", Width = 60, Top = 5, Left = 80 };
            btnActualSize = new System.Windows.Forms.Button { Text = "原始", Width = 60, Top = 5, Left = 150 };
            btnRotateLeft = new System.Windows.Forms.Button { Text = "↶", Width = 40, Top = 5, Left = 220 };
            btnRotateRight = new System.Windows.Forms.Button { Text = "↷", Width = 40, Top = 5, Left = 270 };
            btnSave = new System.Windows.Forms.Button { Text = "保存", Width = 60, Top = 5, Left = 320 };
            btnClose = new System.Windows.Forms.Button { Text = "关闭", Width = 60, Top = 5, Left = 390 };

            toolPanel.Controls.AddRange(new Control[] { 
                btnZoomIn, btnZoomOut, btnActualSize, 
                btnRotateLeft, btnRotateRight, btnSave, btnClose 
            });

            // 创建主面板
            panelMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.DarkGray,
                AutoScroll = true
            };

            pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.AutoSize,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelMain.Controls.Add(pictureBox);

            // 创建信息栏
            var infoPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblInfo = new Label
            {
                Text = "无图片",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 5, 10, 5)
            };
            infoPanel.Controls.Add(lblInfo);

            // 添加控件到窗体
            this.Controls.Add(toolPanel);
            this.Controls.Add(panelMain);
            this.Controls.Add(infoPanel);

            // 绑定事件
            btnZoomIn.Click += btnZoomIn_Click;
            btnZoomOut.Click += btnZoomOut_Click;
            btnActualSize.Click += btnActualSize_Click;
            btnRotateLeft.Click += btnRotateLeft_Click;
            btnRotateRight.Click += btnRotateRight_Click;
            btnSave.Click += btnSave_Click;
            btnClose.Click += btnClose_Click;

            panelMain.MouseDown += panelMain_MouseDown;
            panelMain.MouseMove += panelMain_MouseMove;
            panelMain.MouseUp += panelMain_MouseUp;
            panelMain.MouseWheel += panelMain_MouseWheel;

            this.KeyDown += ImagePreviewForm_KeyDown;

            this.ResumeLayout(false);
        }

        #endregion

        #region 资源清理

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_originalImage != null)
            {
                _originalImage.Dispose();
                _originalImage = null;
            }
            if (_displayImage != null)
            {
                _displayImage.Dispose();
                _displayImage = null;
            }
            base.OnFormClosed(e);
        }

        #endregion

        #region 控件声明

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnActualSize;
        private System.Windows.Forms.Button btnRotateLeft;
        private System.Windows.Forms.Button btnRotateRight;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblInfo;

        #endregion
    }
}