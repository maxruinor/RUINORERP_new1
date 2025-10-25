using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using HLH.Lib.Draw;
using System.Reflection;
using RUINORERP.Global.Model;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace RUINOR.WinFormsUI.CustomPictureBox
{
    /// <summary>
    /// 自定义PictureBox控件
    /// 支持多张图片显示，路径用;隔开
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

        private DataRowImage _RowImage = new DataRowImage();
        public DataRowImage RowImage { get => _RowImage; set => _RowImage = value; }
        
        // 多图片支持
        private List<Image> images = new List<Image>();
        private int currentImageIndex = 0;
        private string imagePaths = ""; // 存储图片路径，用;分隔
        
        // 导航控件
        private Panel navigationPanel;
        private Button prevButton;
        private Button nextButton;
        private Label pageInfoLabel;
        
        // 信息显示面板
        private Panel infoPanel;
        private Label fileNameLabel;
        private Label fileSizeLabel;
        private Label createTimeLabel;
        private System.Windows.Forms.Timer infoPanelTimer;
        
        // 图片信息
        private List<ImageInfo> imageInfos = new List<ImageInfo>();

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("是否支持多图片显示")]
        public bool MultiImageSupport { get; set; } = false;

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("图片路径，用;分隔")]
        public string ImagePaths
        {
            get { return imagePaths; }
            set 
            { 
                imagePaths = value;
                if (MultiImageSupport)
                {
                    LoadImagesFromPaths();
                }
            }
        }

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("是否显示图片信息面板")]
        public bool ShowImageInfo { get; set; } = true;

        [Browsable(true)]
        [Category("自定义属性")]
        [Description("信息面板显示时间（毫秒），0表示一直显示")]
        public int InfoPanelDisplayTime { get; set; } = 3000; // 默认3秒

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
            this.MouseEnter += MagicPictureBox_MouseEnter;
            this.MouseLeave += MagicPictureBox_MouseLeave;
        }

        /// <summary>
        /// 图片信息类
        /// </summary>
        public class ImageInfo
        {
            public string FileName { get; set; }
            public long FileSize { get; set; }
            public DateTime CreateTime { get; set; }
            public string FilePath { get; set; }
            public string Description { get; set; }
            
            // 添加来自tb_FS_FileStorageInfo表的字段
            public long FileId { get; set; }
            public int BusinessType { get; set; }
            public string FileType { get; set; }
            public string HashValue { get; set; }
            public string StorageProvider { get; set; }
            public string StoragePath { get; set; }
            public int CurrentVersion { get; set; }
            public int Status { get; set; }
            public DateTime ExpireTime { get; set; }
            public bool IsRegistered { get; set; }
            public string Metadata { get; set; }
            public long? CreatedBy { get; set; }
            public DateTime? ModifiedAt { get; set; }
            public long? ModifiedBy { get; set; }
        }

        /// <summary>
        /// 从字节数组加载多张图片
        /// </summary>
        /// <param name="imageBytesList">图片字节数组列表</param>
        public void LoadImagesFromBytes(List<byte[]> imageBytesList)
        {
            if (!MultiImageSupport)
            {
                throw new InvalidOperationException("多图片支持未启用");
            }
            
            images.Clear();
            imageInfos.Clear();
            
            if (imageBytesList == null || imageBytesList.Count == 0)
            {
                this.Image = null;
                return;
            }
            
            foreach (var imageBytes in imageBytesList)
            {
                try
                {
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        images.Add(Image.FromStream(ms));
                        // 添加默认的图片信息
                        imageInfos.Add(new ImageInfo
                        {
                            FileName = $"图片{imageInfos.Count + 1}",
                            FileSize = imageBytes.Length,
                            CreateTime = DateTime.Now,
                            FilePath = "",
                            Description = "",
                            FileId = 0,
                            BusinessType = 0,
                            FileType = "",
                            HashValue = "",
                            StorageProvider = "",
                            StoragePath = "",
                            CurrentVersion = 1,
                            Status = 0,
                            ExpireTime = DateTime.MaxValue,
                            IsRegistered = false,
                            Metadata = "",
                            CreatedBy = null,
                            ModifiedAt = null,
                            ModifiedBy = null
                        });
                    }
                }
                catch
                {
                    // 加载失败，跳过
                }
            }
            
            currentImageIndex = 0;
            ShowCurrentImage();
            CreateNavigationControls();
            CreateInfoPanel();
        }

        /// <summary>
        /// 从字节数组加载单张图片
        /// </summary>
        /// <param name="imageBytes">图片字节数组</param>
        public void LoadImageFromBytes(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                this.Image = null;
                return;
            }
            
            try
            {
                using (var ms = new MemoryStream(imageBytes))
                {
                    if (MultiImageSupport)
                    {
                        images.Clear();
                        imageInfos.Clear();
                        images.Add(Image.FromStream(ms));
                        // 添加默认的图片信息
                        imageInfos.Add(new ImageInfo
                        {
                            FileName = "图片1",
                            FileSize = imageBytes.Length,
                            CreateTime = DateTime.Now,
                            FilePath = "",
                            Description = "",
                            FileId = 0,
                            BusinessType = 0,
                            FileType = "",
                            HashValue = "",
                            StorageProvider = "",
                            StoragePath = "",
                            CurrentVersion = 1,
                            Status = 0,
                            ExpireTime = DateTime.MaxValue,
                            IsRegistered = false,
                            Metadata = "",
                            CreatedBy = null,
                            ModifiedAt = null,
                            ModifiedBy = null
                        });
                        currentImageIndex = 0;
                        ShowCurrentImage();
                        CreateNavigationControls();
                        CreateInfoPanel();
                    }
                    else
                    {
                        this.Image = Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                // 加载失败
                MessageBox.Show($"加载图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置图片信息
        /// </summary>
        /// <param name="imageInfoList">图片信息列表</param>
        public void SetImageInfos(List<ImageInfo> imageInfoList)
        {
            if (imageInfoList != null && imageInfoList.Count > 0)
            {
                imageInfos = new List<ImageInfo>(imageInfoList);
                UpdateInfoPanel();
            }
        }

        /// <summary>
        /// 设置单个图片的完整信息
        /// </summary>
        /// <param name="index">图片索引</param>
        /// <param name="imageInfo">图片信息</param>
        public void SetImageInfo(int index, ImageInfo imageInfo)
        {
            if (index >= 0 && index < imageInfos.Count && imageInfo != null)
            {
                imageInfos[index] = imageInfo;
                if (index == currentImageIndex)
                {
                    UpdateInfoPanel();
                }
            }
        }

        /// <summary>
        /// 更新当前显示图片的信息
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        public void UpdateCurrentImageInfo(ImageInfo imageInfo)
        {
            if (imageInfo != null && imageInfos.Count > 0 && currentImageIndex < imageInfos.Count)
            {
                imageInfos[currentImageIndex] = imageInfo;
                UpdateInfoPanel();
            }
        }

        /// <summary>
        /// 获取当前图片信息
        /// </summary>
        /// <returns>当前图片信息</returns>
        public ImageInfo GetCurrentImageInfo()
        {
            if (imageInfos.Count > 0 && currentImageIndex < imageInfos.Count)
            {
                return imageInfos[currentImageIndex];
            }
            return null;
        }

        /// <summary>
        /// 从路径加载多张图片
        /// </summary>
        private void LoadImagesFromPaths()
        {
            images.Clear();
            imageInfos.Clear();
            
            if (string.IsNullOrEmpty(imagePaths))
                return;

            var paths = imagePaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        images.Add(Image.FromFile(path));
                        // 获取文件信息
                        var fileInfo = new FileInfo(path);
                        imageInfos.Add(new ImageInfo
                        {
                            FileName = fileInfo.Name,
                            FileSize = fileInfo.Length,
                            CreateTime = fileInfo.CreationTime,
                            FilePath = path,
                            Description = "",
                            FileId = 0,
                            BusinessType = 0,
                            FileType = Path.GetExtension(path).TrimStart('.'),
                            HashValue = "",
                            StorageProvider = "",
                            StoragePath = path,
                            CurrentVersion = 1,
                            Status = 0,
                            ExpireTime = DateTime.MaxValue,
                            IsRegistered = false,
                            Metadata = "",
                            CreatedBy = null,
                            ModifiedAt = null,
                            ModifiedBy = null
                        });
                    }
                    catch
                    {
                        // 加载失败，跳过
                    }
                }
            }
            
            currentImageIndex = 0;
            ShowCurrentImage();
            CreateNavigationControls();
            CreateInfoPanel();
        }

        /// <summary>
        /// 显示当前图片
        /// </summary>
        private void ShowCurrentImage()
        {
            if (images.Count > 0 && currentImageIndex < images.Count)
            {
                this.Image = images[currentImageIndex];
            }
            else
            {
                this.Image = null;
            }
            
            UpdatePageInfo();
            UpdateInfoPanel();
        }

        /// <summary>
        /// 更新图片路径字符串
        /// </summary>
        private void UpdateImagePathsFromImages()
        {
            if (MultiImageSupport)
            {
                // 注意：这里只是示例实现，实际应用中可能需要保存图片到指定位置并更新路径
                // 当前实现仅用于同步图片数量信息
                var pathList = new List<string>();
                for (int i = 0; i < images.Count; i++)
                {
                    // 如果原来有路径且图片存在，则保留原路径
                    var originalPaths = imagePaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i < originalPaths.Length && File.Exists(originalPaths[i]))
                    {
                        pathList.Add(originalPaths[i]);
                    }
                    else
                    {
                        pathList.Add($"image_{i}.png"); // 占位符路径
                    }
                }
                imagePaths = string.Join(";", pathList);
            }
        }

        /// <summary>
        /// 创建导航控件
        /// </summary>
        private void CreateNavigationControls()
        {
            if (!MultiImageSupport || images.Count <= 1)
            {
                // 隐藏导航控件
                if (navigationPanel != null)
                {
                    navigationPanel.Visible = false;
                }
                return;
            }

            // 如果已经创建过导航控件，直接更新
            if (navigationPanel != null)
            {
                navigationPanel.Visible = true;
                UpdatePageInfo();
                return;
            }

            // 创建导航面板
            navigationPanel = new Panel
            {
                Height = 30,
                Dock = DockStyle.Bottom,
                BackColor = Color.LightGray,
                Visible = true
            };

            // 上一张按钮
            prevButton = new Button
            {
                Text = "<",
                Width = 30,
                Height = 25,
                Location = new Point(10, 2),
                Enabled = currentImageIndex > 0
            };
            prevButton.Click += PrevButton_Click;

            // 下一张按钮
            nextButton = new Button
            {
                Text = ">",
                Width = 30,
                Height = 25,
                Location = new Point(70, 2),
                Enabled = currentImageIndex < images.Count - 1
            };
            nextButton.Click += NextButton_Click;

            // 页码信息
            pageInfoLabel = new Label
            {
                Text = "1/1",
                AutoSize = true,
                Location = new Point(110, 7)
            };

            navigationPanel.Controls.Add(prevButton);
            navigationPanel.Controls.Add(nextButton);
            navigationPanel.Controls.Add(pageInfoLabel);

            // 添加到当前控件中
            this.Controls.Add(navigationPanel);
            
            UpdatePageInfo();
        }

        /// <summary>
        /// 创建信息面板
        /// </summary>
        private void CreateInfoPanel()
        {
            if (!ShowImageInfo)
            {
                if (infoPanel != null)
                {
                    infoPanel.Visible = false;
                }
                return;
            }

            // 如果已经创建过信息面板，直接更新
            if (infoPanel != null)
            {
                infoPanel.Visible = true;
                UpdateInfoPanel();
                return;
            }

            // 创建信息面板
            infoPanel = new Panel
            {
                Height = 80, // 增加高度以容纳更多信息
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(200, 240, 240, 240), // 半透明背景
                Visible = true,
                Padding = new Padding(5)
            };

            // 文件名标签
            fileNameLabel = new Label
            {
                AutoSize = true,
                Location = new Point(5, 5),
                Font = new Font(this.Font.FontFamily, 9, FontStyle.Bold),
                ForeColor = Color.Black,
                MaximumSize = new Size(300, 0),
                AutoEllipsis = true
            };

            // 文件大小标签
            fileSizeLabel = new Label
            {
                AutoSize = true,
                Location = new Point(5, 25),
                Font = new Font(this.Font.FontFamily, 8),
                ForeColor = Color.Gray
            };

            // 创建时间标签
            createTimeLabel = new Label
            {
                AutoSize = true,
                Location = new Point(150, 25),
                Font = new Font(this.Font.FontFamily, 8),
                ForeColor = Color.Gray
            };

            // 文件类型标签
            Label fileTypeLabel = new Label
            {
                AutoSize = true,
                Location = new Point(5, 45),
                Font = new Font(this.Font.FontFamily, 8),
                ForeColor = Color.Gray
            };

            // 业务类型标签
            Label businessTypeLabel = new Label
            {
                AutoSize = true,
                Location = new Point(150, 45),
                Font = new Font(this.Font.FontFamily, 8),
                ForeColor = Color.Gray
            };

            infoPanel.Controls.Add(fileNameLabel);
            infoPanel.Controls.Add(fileSizeLabel);
            infoPanel.Controls.Add(createTimeLabel);
            infoPanel.Controls.Add(fileTypeLabel);
            infoPanel.Controls.Add(businessTypeLabel);

            // 添加到当前控件中
            this.Controls.Add(infoPanel);
            infoPanel.BringToFront(); // 确保信息面板在最前面

            // 如果设置了显示时间，创建计时器
            if (InfoPanelDisplayTime > 0)
            {
                infoPanelTimer = new System.Windows.Forms.Timer();
                infoPanelTimer.Interval = InfoPanelDisplayTime;
                infoPanelTimer.Tick += (s, e) =>
                {
                    if (infoPanel != null)
                    {
                        infoPanel.Visible = false;
                    }
                    infoPanelTimer.Stop();
                };
            }

            UpdateInfoPanel();
        }

        /// <summary>
        /// 根据业务类型获取描述信息
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <returns>业务类型描述</returns>
        private string GetBusinessTypeDescription(int businessType)
        {
            // 这里可以根据实际的业务类型定义返回相应的描述
            switch (businessType)
            {
                case 1:
                    return "付款凭证";
                case 2:
                    return "产品图片";
                case 3:
                    return "BOM手册";
                default:
                    return $"业务类型 {businessType}";
            }
        }
        
        /// <summary>
        /// 根据状态值获取状态描述
        /// </summary>
        /// <param name="status">状态值</param>
        /// <returns>状态描述</returns>
        private string GetStatusDescription(int status)
        {
            switch (status)
            {
                case 0:
                    return "正常";
                case 1:
                    return "已删除";
                case 2:
                    return "已过期";
                default:
                    return $"状态 {status}";
            }
        }
        
        /// <summary>
        /// 更新信息面板
        /// </summary>
        private void UpdateInfoPanel()
        {
            if (infoPanel == null || !ShowImageInfo)
                return;

            if (imageInfos.Count > 0 && currentImageIndex < imageInfos.Count)
            {
                var imageInfo = imageInfos[currentImageIndex];
                
                if (fileNameLabel != null)
                {
                    fileNameLabel.Text = imageInfo.FileName;
                }
                
                if (fileSizeLabel != null)
                {
                    fileSizeLabel.Text = $"大小: {FormatFileSize(imageInfo.FileSize)}";
                }
                
                if (createTimeLabel != null)
                {
                    createTimeLabel.Text = $"创建时间: {imageInfo.CreateTime:yyyy-MM-dd HH:mm:ss}";
                }
                
                // 更新文件类型信息
                var fileTypeLabel = infoPanel.Controls.Cast<Control>().FirstOrDefault(c => c is Label && c.Location.Y == 45 && c.Location.X == 5) as Label;
                if (fileTypeLabel != null)
                {
                    fileTypeLabel.Text = $"类型: {imageInfo.FileType}";
                }
                
                // 更新业务类型信息
                var businessTypeLabel = infoPanel.Controls.Cast<Control>().FirstOrDefault(c => c is Label && c.Location.Y == 45 && c.Location.X == 150) as Label;
                if (businessTypeLabel != null)
                {
                    businessTypeLabel.Text = $"业务类型: {GetBusinessTypeDescription(imageInfo.BusinessType)}";
                }
                
                infoPanel.Visible = true;
                
                // 如果设置了显示时间，启动计时器
                if (infoPanelTimer != null)
                {
                    infoPanelTimer.Stop();
                    infoPanelTimer.Start();
                }
            }
            else
            {
                infoPanel.Visible = false;
            }
        }

        /// <summary>
        /// 格式化文件大小显示
        /// </summary>
        /// <param name="size">文件大小（字节）</param>
        /// <returns>格式化后的文件大小字符串</returns>
        private string FormatFileSize(long size)
        {
            if (size < 1024)
                return $"{size} B";
            if (size < 1024 * 1024)
                return $"{size / 1024.0:F1} KB";
            if (size < 1024 * 1024 * 1024)
                return $"{size / (1024.0 * 1024.0):F1} MB";
            return $"{size / (1024.0 * 1024.0 * 1024.0):F1} GB";
        }

        /// <summary>
        /// 更新页码信息
        /// </summary>
        private void UpdatePageInfo()
        {
            if (pageInfoLabel != null)
            {
                pageInfoLabel.Text = $"{currentImageIndex + 1}/{images.Count}";
            }
            
            if (prevButton != null)
            {
                prevButton.Enabled = currentImageIndex > 0;
            }
            
            if (nextButton != null)
            {
                nextButton.Enabled = currentImageIndex < images.Count - 1;
            }
        }

        /// <summary>
        /// 上一张图片
        /// </summary>
        private void PrevButton_Click(object sender, EventArgs e)
        {
            if (currentImageIndex > 0)
            {
                currentImageIndex--;
                ShowCurrentImage();
            }
        }

        /// <summary>
        /// 下一张图片
        /// </summary>
        private void NextButton_Click(object sender, EventArgs e)
        {
            if (currentImageIndex < images.Count - 1)
            {
                currentImageIndex++;
                ShowCurrentImage();
            }
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
            if (MultiImageSupport && !contextMenuStrip.Items.ContainsKey("添加图片"))
            {
                contextMenuStrip.Items.Add(new ToolStripMenuItem("添加图片", null, new EventHandler(AddImage), "添加图片"));
            }
            if (MultiImageSupport && !contextMenuStrip.Items.ContainsKey("删除当前图片"))
            {
                contextMenuStrip.Items.Add(new ToolStripMenuItem("删除当前图片", null, new EventHandler(DeleteCurrentImage), "删除当前图片"));
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

        /// <summary>
        /// 删除当前图片
        /// </summary>
        private void DeleteCurrentImage(object sender, EventArgs e)
        {
            if (MultiImageSupport && images.Count > 0)
            {
                // 删除当前图片
                images.RemoveAt(currentImageIndex);
                imageInfos.RemoveAt(currentImageIndex);
                
                // 调整当前索引
                if (currentImageIndex >= images.Count && images.Count > 0)
                {
                    currentImageIndex = images.Count - 1;
                }
                
                // 显示新图片或清空
                if (images.Count > 0)
                {
                    ShowCurrentImage();
                }
                else
                {
                    this.Image = null;
                    imagePaths = "";
                }
                
                // 更新导航控件
                CreateNavigationControls();
                UpdatePageInfo();
                UpdateImagePathsFromImages(); // 更新图片路径
            }
        }

        private void ClearImage(object sender, EventArgs e)
        {
            this.Image = null;
            if (MultiImageSupport)
            {
                images.Clear();
                imageInfos.Clear();
                currentImageIndex = 0;
                imagePaths = "";
            }
            // 重绘 PictureBox
            this.Invalidate();
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        private void AddImage(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "选择图片";
                openFileDialog.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                openFileDialog.Multiselect = true; // 允许多选
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var fileName in openFileDialog.FileNames)
                    {
                        try
                        {
                            var image = Image.FromFile(fileName);
                            images.Add(image);
                            
                            // 获取文件信息
                            var fileInfo = new FileInfo(fileName);
                            imageInfos.Add(new ImageInfo
                            {
                                FileName = fileInfo.Name,
                                FileSize = fileInfo.Length,
                                CreateTime = fileInfo.CreationTime,
                                FilePath = fileName,
                                Description = ""
                            });
                            
                            // 如果是第一张图片，显示它
                            if (images.Count == 1)
                            {
                                currentImageIndex = 0;
                                ShowCurrentImage();
                                CreateNavigationControls();
                                CreateInfoPanel();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"加载图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    
                    UpdatePageInfo();
                    UpdateImagePathsFromImages(); // 更新图片路径
                }
            }
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
                
                // 更新多图片列表中的当前图片
                if (MultiImageSupport && currentImageIndex < images.Count)
                {
                    images[currentImageIndex] = this.Image;
                }
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
        protected override void OnDragDrop(DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                if (MultiImageSupport)
                {
                    // 多图片模式，添加所有拖拽的图片
                    foreach (var file in files)
                    {
                        try
                        {
                            var image = Image.FromFile(file);
                            images.Add(image);
                            
                            // 获取文件信息
                            var fileInfo = new FileInfo(file);
                            imageInfos.Add(new ImageInfo
                            {
                                FileName = fileInfo.Name,
                                FileSize = fileInfo.Length,
                                CreateTime = fileInfo.CreationTime,
                                FilePath = file,
                                Description = ""
                            });
                            
                            // 如果是第一张图片，显示它
                            if (images.Count == 1)
                            {
                                currentImageIndex = 0;
                                ShowCurrentImage();
                                CreateNavigationControls();
                                CreateInfoPanel();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"加载图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    
                    UpdatePageInfo();
                    UpdateImagePathsFromImages(); // 更新图片路径
                }
                else
                {
                    // 单图片模式，只加载第一张
                    Image image = Image.FromFile(files[0]);
                    string newhash = ImageHelper.GetImageHash(this.Image);
                    RowImage.SetImageNewHash(newhash);
                    this.Image = image;
                }
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
                string newhash = ImageHelper.GetImageHash(image);
                RowImage.SetImageNewHash(newhash);
                this.Image = image;

                // 如果是多图片模式，添加到列表中
                if (MultiImageSupport)
                {
                    images.Add(image);
                    // 添加默认的图片信息
                    imageInfos.Add(new ImageInfo
                    {
                        FileName = $"粘贴图片{images.Count}",
                        FileSize = 0, // 粘贴的图片无法获取文件大小
                        CreateTime = DateTime.Now,
                        FilePath = "",
                        Description = ""
                    });
                    currentImageIndex = images.Count - 1;
                    CreateNavigationControls();
                    UpdatePageInfo();
                    UpdateImagePathsFromImages(); // 更新图片路径
                    CreateInfoPanel();
                }

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
                        string newhash = ImageHelper.GetImageHash(this.Image);
                        RowImage.SetImageNewHash(newhash);
                        
                        // 如果是多图片模式，添加到列表中
                        if (MultiImageSupport)
                        {
                            images.Add(this.Image);
                            // 获取文件信息
                            var fileInfo = new FileInfo(openFileDialog.FileName);
                            imageInfos.Add(new ImageInfo
                            {
                                FileName = fileInfo.Name,
                                FileSize = fileInfo.Length,
                                CreateTime = fileInfo.CreationTime,
                                FilePath = openFileDialog.FileName,
                                Description = ""
                            });
                            currentImageIndex = images.Count - 1;
                            CreateNavigationControls();
                            UpdatePageInfo();
                            UpdateImagePathsFromImages(); // 更新图片路径
                            CreateInfoPanel();
                        }
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
                    string newhash = ImageHelper.GetImageHash(this.Image);
                    RowImage.SetImageNewHash(newhash);
                    
                    // 更新多图片列表中的当前图片
                    if (MultiImageSupport && currentImageIndex < images.Count)
                    {
                        images[currentImageIndex] = this.Image;
                        UpdateImagePathsFromImages(); // 更新图片路径
                    }
                    
                    isCropping = false;
                }
                else
                {
                    ViewLargeImage(sender, e);
                }

            }
        }

        /// <summary>
        /// 鼠标进入控件事件
        /// </summary>
        private void MagicPictureBox_MouseEnter(object sender, EventArgs e)
        {
            // 鼠标进入时显示信息面板
            if (infoPanel != null && ShowImageInfo)
            {
                infoPanel.Visible = true;
                // 如果设置了显示时间，重启计时器
                if (infoPanelTimer != null)
                {
                    infoPanelTimer.Stop();
                    infoPanelTimer.Start();
                }
            }
        }

        /// <summary>
        /// 鼠标离开控件事件
        /// </summary>
        private void MagicPictureBox_MouseLeave(object sender, EventArgs e)
        {
            // 鼠标离开时根据设置决定是否隐藏信息面板
            if (infoPanel != null && InfoPanelDisplayTime > 0)
            {
                // 如果设置了显示时间，信息面板会在计时器结束后自动隐藏
                // 否则一直显示
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
                    string newhash = ImageHelper.GetImageHash(this.Image);
                    RowImage.SetImageNewHash(newhash);
                    
                    // 更新多图片列表中的当前图片
                    if (MultiImageSupport && currentImageIndex < images.Count)
                    {
                        images[currentImageIndex] = this.Image;
                    }
                    
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
                
                // 更新多图片列表中的当前图片
                if (MultiImageSupport && currentImageIndex < images.Count)
                {
                    images[currentImageIndex] = this.Image;
                }
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
        
        /// <summary>
        /// 从文件存储信息加载图片
        /// </summary>
        /// <param name="fileStorageInfos">文件存储信息列表</param>
        /// <param name="imageBytesList">对应的图片字节数据列表</param>
        public void LoadImagesFromFileStorageInfos(List<ImageInfo> fileStorageInfos, List<byte[]> imageBytesList)
        {
            if (!MultiImageSupport)
            {
                throw new InvalidOperationException("多图片支持未启用");
            }
            
            if (fileStorageInfos == null || imageBytesList == null || 
                fileStorageInfos.Count != imageBytesList.Count)
            {
                throw new ArgumentException("文件信息和图片数据数量不匹配");
            }
            
            images.Clear();
            imageInfos.Clear();
            
            for (int i = 0; i < fileStorageInfos.Count; i++)
            {
                try
                {
                    using (var ms = new MemoryStream(imageBytesList[i]))
                    {
                        images.Add(Image.FromStream(ms));
                        imageInfos.Add(fileStorageInfos[i]);
                    }
                }
                catch
                {
                    // 加载失败，跳过
                }
            }
            
            currentImageIndex = 0;
            ShowCurrentImage();
            CreateNavigationControls();
            CreateInfoPanel();
        }
        
        /// <summary>
        /// 从文件存储信息加载单张图片
        /// </summary>
        /// <param name="fileStorageInfo">文件存储信息</param>
        /// <param name="imageBytes">图片字节数据</param>
        public void LoadImageFromFileStorageInfo(ImageInfo fileStorageInfo, byte[] imageBytes)
        {
            if (fileStorageInfo == null || imageBytes == null)
            {
                throw new ArgumentException("文件信息或图片数据不能为空");
            }
            
            try
            {
                using (var ms = new MemoryStream(imageBytes))
                {
                    if (MultiImageSupport)
                    {
                        images.Clear();
                        imageInfos.Clear();
                        images.Add(Image.FromStream(ms));
                        imageInfos.Add(fileStorageInfo);
                        currentImageIndex = 0;
                        ShowCurrentImage();
                        CreateNavigationControls();
                        CreateInfoPanel();
                    }
                    else
                    {
                        this.Image = Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                // 加载失败
                MessageBox.Show($"加载图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 获取当前图片路径
        /// </summary>
        /// <returns>当前图片路径</returns>
        public string GetCurrentImagePath()
        {
            if (MultiImageSupport)
            {
                if (!string.IsNullOrEmpty(imagePaths))
                {
                    var paths = imagePaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (currentImageIndex >= 0 && currentImageIndex < paths.Length)
                    {
                        return paths[currentImageIndex];
                    }
                }
            }
            return "";
        }
        
        /// <summary>
        /// 获取所有图片
        /// </summary>
        /// <returns>图片列表</returns>
        public List<Image> GetImages()
        {
            return new List<Image>(images);
        }
        
        /// <summary>
        /// 设置图片列表
        /// </summary>
        /// <param name="imageList">图片列表</param>
        public void SetImages(List<Image> imageList)
        {
            if (MultiImageSupport)
            {
                images = new List<Image>(imageList);
                currentImageIndex = 0;
                ShowCurrentImage();
                CreateNavigationControls();
                UpdateImagePathsFromImages(); // 更新图片路径
            }
        }
        
        /// <summary>
        /// 将当前图片转换为byte[]
        /// </summary>
        /// <param name="format">图片格式，默认为JPEG</param>
        /// <returns>图片的字节数组</returns>
        public byte[] GetCurrentImageBytes(ImageFormat format = null)
        {
            // 如果没有指定格式，默认使用JPEG格式
            if (format == null)
                format = ImageFormat.Jpeg;
                
            // 获取当前显示的图片
            Image currentImage = null;
            if (MultiImageSupport && images.Count > 0 && currentImageIndex < images.Count)
            {
                currentImage = images[currentImageIndex];
            }
            else
            {
                currentImage = this.Image;
            }
            
            // 如果没有图片，返回null
            if (currentImage == null)
                return null;
                
            try
            {
                using (var ms = new MemoryStream())
                {
                    currentImage.Save(ms, format);
                    return ms.ToArray();
                }
            }
            catch
            {
                // 转换失败，返回null
                return null;
            }
        }
        
        /// <summary>
        /// 将所有图片转换为byte[]数组列表
        /// </summary>
        /// <param name="format">图片格式，默认为JPEG</param>
        /// <returns>所有图片的字节数组列表</returns>
        public List<byte[]> GetAllImageBytes(ImageFormat format = null)
        {
            List<byte[]> imageBytesList = new List<byte[]>();
            
            // 如果没有指定格式，默认使用JPEG格式
            if (format == null)
                format = ImageFormat.Jpeg;
                
            // 获取所有图片
            List<Image> allImages = new List<Image>();
            if (MultiImageSupport && images.Count > 0)
            {
                allImages = new List<Image>(images);
            }
            else if (this.Image != null)
            {
                allImages.Add(this.Image);
            }
            
            // 转换每张图片为byte[]
            foreach (var image in allImages)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        image.Save(ms, format);
                        imageBytesList.Add(ms.ToArray());
                    }
                }
                catch
                {
                    // 转换失败，添加null或跳过
                    imageBytesList.Add(null);
                }
            }
            
            return imageBytesList;
        }
        
        /// <summary>
        /// 获取所有图片及其对应信息的组合
        /// </summary>
        /// <param name="format">图片格式，默认为JPEG</param>
        /// <returns>图片和信息的组合列表</returns>
        public List<Tuple<byte[], ImageInfo>> GetAllImageBytesWithInfo(ImageFormat format = null)
        {
            List<Tuple<byte[], ImageInfo>> imageBytesWithInfoList = new List<Tuple<byte[], ImageInfo>>();
            
            // 如果没有指定格式，默认使用JPEG格式
            if (format == null)
                format = ImageFormat.Jpeg;
                
            // 检查图片和信息数量是否匹配
            if (MultiImageSupport && images.Count > 0)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    try
                    {
                        // 转换图片为byte[]
                        byte[] imageBytes = null;
                        using (var ms = new MemoryStream())
                        {
                            images[i].Save(ms, format);
                            imageBytes = ms.ToArray();
                        }
                        
                        // 获取对应的图片信息
                        ImageInfo imageInfo = null;
                        if (i < imageInfos.Count)
                        {
                            imageInfo = imageInfos[i];
                        }
                        else
                        {
                            // 如果没有对应的信息，创建默认信息
                            imageInfo = new ImageInfo
                            {
                                FileName = $"图片{i + 1}",
                                FileSize = imageBytes?.Length ?? 0,
                                CreateTime = DateTime.Now,
                                FilePath = "",
                                Description = ""
                            };
                        }
                        
                        imageBytesWithInfoList.Add(new Tuple<byte[], ImageInfo>(imageBytes, imageInfo));
                    }
                    catch
                    {
                        // 转换失败，跳过该图片
                        continue;
                    }
                }
            }
            else if (this.Image != null)
            {
                try
                {
                    // 转换单张图片为byte[]
                    byte[] imageBytes = null;
                    using (var ms = new MemoryStream())
                    {
                        this.Image.Save(ms, format);
                        imageBytes = ms.ToArray();
                    }
                    
                    // 获取图片信息
                    ImageInfo imageInfo = null;
                    if (imageInfos.Count > 0)
                    {
                        imageInfo = imageInfos[0];
                    }
                    else
                    {
                        // 如果没有信息，创建默认信息
                        imageInfo = new ImageInfo
                        {
                            FileName = "图片1",
                            FileSize = imageBytes?.Length ?? 0,
                            CreateTime = DateTime.Now,
                            FilePath = "",
                            Description = ""
                        };
                    }
                    
                    imageBytesWithInfoList.Add(new Tuple<byte[], ImageInfo>(imageBytes, imageInfo));
                }
                catch
                {
                    // 转换失败，跳过
                }
            }
            
            return imageBytesWithInfoList;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 释放计时器资源
                if (infoPanelTimer != null)
                {
                    infoPanelTimer.Stop();
                    infoPanelTimer.Dispose();
                    infoPanelTimer = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}