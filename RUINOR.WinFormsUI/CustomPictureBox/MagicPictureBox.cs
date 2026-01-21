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
using System.Collections.Concurrent;
using System.Collections.Specialized;
using RUINOR.WinFormsUI.CustomPictureBox.Implementations;
using RUINORERP.Common.Helper;

namespace RUINOR.WinFormsUI.CustomPictureBox
{
    /// <summary>
    /// 自定义PictureBox控件1
    /// 支持多张图片显示，路径用;隔开
    /// </summary>
    public class MagicPictureBox : PictureBox
    {
        // 图片处理相关的组件
        private readonly ImageProcessor _imageProcessor;
        private readonly SHA256HashCalculator _hashCalculator;
        private readonly LRUImageCache _imageCache;
        private readonly ImageUpdateManager _updateManager;
        private readonly OptimizedImageLoader _imageLoader;


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

        // 菜单相关字段
        private ContextMenuStrip _mainContextMenu; // 主上下文菜单
        private ContextMenuStrip _cropContextMenu;  // 裁剪模式上下文菜单
        private ImageCroppingBox _imageCroppingBox; // 裁剪框控件

        // 菜单项ID常量定义
        private const string MENU_VIEW_LARGE = "查看大图";
        private const string MENU_CROP = "裁剪图片";
        private const string MENU_ADD_IMAGE = "添加图片";
        private const string MENU_PASTE_IMAGE = "粘贴图片";
        private const string MENU_CLEAR_IMAGE = "清除图片";
        private const string MENU_DELETE_CURRENT = "删除当前图片";
        private const string MENU_CANCEL_CROP = "取消裁剪";

        #region 菜单配置属性
        /// <summary>
        /// 菜单配置属性区域
        /// 这些属性允许用户在设计时或运行时自定义上下文菜单中显示的功能
        /// 更改这些属性后，需要调用UpdateContextMenu方法使更改生效
        /// </summary>

        /// <summary>
        /// 是否显示"查看大图"菜单项
        /// 启用此选项允许用户在新窗口中查看图片的原始大小
        /// </summary>
        [Browsable(true)]
        [Category("菜单配置")]
        [Description("控制是否显示查看大图菜单项")]
        [DefaultValue(true)]
        public bool ShowViewLargeImageMenuItem { get; set; } = true;

        /// <summary>
        /// 是否显示"裁剪图片"菜单项
        /// </summary>
        [Browsable(true)]
        [Category("菜单配置")]
        [Description("控制是否显示裁剪图片菜单项")]
        [DefaultValue(true)]
        public bool ShowCropImageMenuItem { get; set; } = true;

        /// <summary>
        /// 是否显示"添加图片"菜单项
        /// </summary>
        [Browsable(true)]
        [Category("菜单配置")]
        [Description("控制是否显示添加图片菜单项")]
        [DefaultValue(true)]
        public bool ShowAddImageMenuItem { get; set; } = true;

        /// <summary>
        /// 是否显示"粘贴图片"菜单项
        /// </summary>
        [Browsable(true)]
        [Category("菜单配置")]
        [Description("控制是否显示粘贴图片菜单项")]
        [DefaultValue(true)]
        public bool ShowPasteImageMenuItem { get; set; } = true;

        /// <summary>
        /// 是否显示"清除图片"菜单项
        /// </summary>
        [Browsable(true)]
        [Category("菜单配置")]
        [Description("控制是否显示清除图片菜单项")]
        [DefaultValue(true)]
        public bool ShowClearImageMenuItem { get; set; } = true;

        /// <summary>
        /// 是否显示"删除当前图片"菜单项
        /// </summary>
        [Browsable(true)]
        [Category("菜单配置")]
        [Description("控制是否显示删除当前图片菜单项")]
        [DefaultValue(true)]
        public bool ShowDeleteCurrentImageMenuItem { get; set; } = true;

        #endregion

        private DataRowImage _RowImage = new DataRowImage();
        public DataRowImage RowImage { get => _RowImage; set => _RowImage = value; }

        // 多图片支持
        private List<Image> images = new List<Image>();
        private List<ImageInfo> imageInfos = new List<ImageInfo>();
        private List<ImageInfo> _deletedImages = new List<ImageInfo>(); // 已删除但未同步到服务器的图片
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

        // 使用ImageUpdateManager中的UPDATE_MARKER常量

        #region 图片哈希计算功能

        /// <summary>
        /// 比较两张图片是否相同（基于哈希值）
        /// </summary>
        /// <param name="hash1">第一张图片的哈希值</param>
        /// <param name="hash2">第二张图片的哈希值</param>
        /// <returns>如果相同返回true，否则返回false</returns>
        private bool AreImagesEqual(string hash1, string hash2)
        {
            if (string.IsNullOrEmpty(hash1) || string.IsNullOrEmpty(hash2))
                return false;

            return string.Equals(hash1, hash2, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region 图片更新管理功能

        /// <summary>
        /// 重置图片的更新状态
        /// </summary>
        /// <param name="imageIndex">图片索引</param>
        private void ResetImageUpdateStatus(int imageIndex)
        {
            if (MultiImageSupport && imageIndex >= 0 && imageIndex < imageInfos.Count)
            {
                imageInfos[imageIndex].IsUpdated = false;
                // 清除更新标记
                if (imageInfos[imageIndex].Metadata.ContainsKey("UpdateMarker"))
                {
                    imageInfos[imageIndex].Metadata["UpdateMarker"] = "";
                }
            }
        }

        /// <summary>
        /// 重置所有图片的更新状态
        /// </summary>
        private void ResetAllImageUpdateStatuses()
        {
            if (MultiImageSupport)
            {
                foreach (var info in imageInfos)
                {
                    if (info != null)
                    {
                        info.IsUpdated = false;
                        // 清除更新标记
                        if (info.Metadata.ContainsKey("UpdateMarker"))
                        {
                            info.Metadata["UpdateMarker"] = "";
                        }
                        info.Metadata.Clear();
                    }
                }
            }
        }

        #endregion

        #region 图片缓存功能

        /// <summary>
        /// 图片缓存的最大容量
        /// </summary>
        [Category("缓存设置")]
        [Description("设置图片缓存的最大容量，超过容量时将根据LRU策略移除最少使用的图片")]
        [DefaultValue(100)]
        public int MaxCacheSize
        {
            get { return _imageCache.MaxCapacity; }
            set
            {
                if (value > 0)
                {
                    _imageCache.MaxCapacity = value;
                }
            }
        }



        /// <summary>
        /// 添加图片到缓存
        /// </summary>
        /// <param name="hashValue">图片哈希值</param>
        /// <param name="image">Image对象</param>
        private void AddToCache(string hashValue, Image image)
        {
            if (string.IsNullOrEmpty(hashValue) || image == null)
                return;

            try
            {
                _imageCache.Add(hashValue, image);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"添加图片到缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从缓存中获取图片
        /// </summary>
        /// <param name="hashValue">图片哈希值</param>
        /// <returns>缓存的Image对象，如果不存在则返回null</returns>
        private Image GetFromCache(string hashValue)
        {
            if (string.IsNullOrEmpty(hashValue))
                return null;

            try
            {
                return _imageCache.Get(hashValue);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从缓存获取图片失败: {ex.Message}");
                return null;
            }
        }

        // TrimCache方法已由LRUImageCache内部实现，不再需要

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache()
        {
            try
            {
                _imageCache.Clear();
                System.Diagnostics.Debug.WriteLine("缓存已清空");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"清空缓存失败: {ex.Message}");
            }
        }

        #endregion

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
        /// <summary>
        /// 获取所有已更新图片及其对应信息的组合
        /// </summary>
        /// <param name="format">图片格式，默认为JPEG</param>
        /// <returns>已更新图片和信息的组合列表</returns>
        public List<Tuple<byte[], ImageInfo>> GetUpdatedImageBytesWithInfo(ImageFormat format = null)
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
                        // 只处理需要更新的图片，使用统一的判断方法
                        // 修复：将!IsImageNeedingUpdate(i)改为IsImageNeedingUpdate(i)，确保需要更新的图片被正确处理
                        bool needsUpdate = i < imageInfos.Count && imageInfos[i] != null &&
                                          (IsImageNeedingUpdate(i) ||
                                           imageInfos[i].FileId == 0 ||
                                           string.IsNullOrEmpty(imageInfos[i].HashValue));

                        if (!needsUpdate)
                        {
                            continue;
                        }

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
                            // 确保文件大小正确更新
                            if (imageInfo != null)
                            {
                                imageInfo.FileSize = imageBytes?.Length ?? 0;
                                // 更新哈希值
                                imageInfo.HashValue = CalculateImageHash(imageBytes);
                            }
                            else
                            {
                                // 如果没有对应的信息，创建更有意义的默认信息
                                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                string formatExtension = ImageUtils.GetFormatExtension(format).TrimStart('.'); // 获取不含点号的扩展名
                                string defaultFileName = $"图片_{timestamp}_{i + 1}.{formatExtension}";

                                imageInfo = new ImageInfo
                                {
                                    OriginalFileName = defaultFileName,
                                    FileSize = imageBytes?.Length ?? 0,
                                    CreateTime = DateTime.Now,
                                    Metadata = new Dictionary<string, string> { { "Description", "自动生成的图片信息" } },
                                    FileExtension = formatExtension,
                                    FileType = formatExtension,
                                    HashValue = CalculateImageHash(imageBytes),
                                    IsUpdated = true,
                                    Width = images[i]?.Width ?? 0,
                                    Height = images[i]?.Height ?? 0
                                };
                            }

                            imageBytesWithInfoList.Add(new Tuple<byte[], ImageInfo>(imageBytes, imageInfo));
                        }
                    }
                    catch (Exception ex)
                    {
                        // 转换失败，记录错误并跳过该图片
                        System.Diagnostics.Debug.WriteLine($"获取图片数据失败 (索引 {i}): {ex.Message}");
                        continue;
                    }
                }
            }
            else if (this.Image != null && (imageInfos.Count == 0 ||
                                           (imageInfos.Count > 0 &&
                                            (imageInfos[0].IsUpdated ||
                                             IsImageNeedingUpdate(0) ||
                                             imageInfos[0].FileId == 0 ||
                                             string.IsNullOrEmpty(imageInfos[0].HashValue)))))
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
                        // 确保文件大小正确更新
                        if (imageInfo != null)
                        {
                            imageInfo.FileSize = imageBytes?.Length ?? 0;
                            // 更新哈希值
                            imageInfo.HashValue = CalculateImageHash(imageBytes);
                        }
                    }
                    else
                    {
                        // 如果没有信息，创建更有意义的默认信息
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string formatExtension = ImageUtils.GetFormatExtension(format).TrimStart('.'); // 获取不含点号的扩展名
                        string defaultFileName = $"图片_{timestamp}.{formatExtension}";

                        imageInfo = new ImageInfo
                        {
                            OriginalFileName = defaultFileName,
                            FileSize = imageBytes?.Length ?? 0,
                            CreateTime = DateTime.Now,
                            Metadata = new Dictionary<string, string> { { "Description", "自动生成的图片信息" } },
                            FileType = formatExtension,
                            FileExtension = formatExtension,
                            HashValue = CalculateImageHash(imageBytes),
                            IsUpdated = true,
                            Width = this.Image?.Width ?? 0,
                            Height = this.Image?.Height ?? 0
                        };
                    }

                    imageBytesWithInfoList.Add(new Tuple<byte[], ImageInfo>(imageBytes, imageInfo));
                }
                catch (Exception ex)
                {
                    // 转换失败，记录错误
                    System.Diagnostics.Debug.WriteLine($"获取单张图片数据失败: {ex.Message}");
                }
            }

            return imageBytesWithInfoList;
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
            // 初始化图片处理相关组件
            _imageProcessor = new ImageProcessor();
            _hashCalculator = new SHA256HashCalculator(_imageProcessor);
            _imageCache = new LRUImageCache(100); // 默认缓存大小100
            _updateManager = new ImageUpdateManager(_hashCalculator, _imageProcessor);
            _imageLoader = new OptimizedImageLoader(_imageCache, _imageProcessor);

            // 设置允许拖拽
            this.AllowDrop = true;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.SizeMode = PictureBoxSizeMode.Normal; // 确保图片可以移动

            // 初始化菜单系统
            InitializeMenuSystem();

            // 设置事件处理
            this.DoubleClick += CustomPictureBox_DoubleClick;
            this.Paint += new PaintEventHandler(PictureBoxViewer_Paint);
            this.MouseDown += CustomPictureBox_MouseDown;
            this.MouseMove += CustomPictureBox_MouseMove;
            this.MouseUp += CustomPictureBox_MouseUp;
            this.MouseWheel += CustomPictureBox_MouseWheel;
            this.MouseEnter += MagicPictureBox_MouseEnter;
            this.MouseLeave += MagicPictureBox_MouseLeave;
        }

        /// <summary>
        /// 初始化菜单系统
        /// </summary>
        private void InitializeMenuSystem()
        {
            // 创建菜单实例
            _mainContextMenu = new ContextMenuStrip();
            _cropContextMenu = new ContextMenuStrip();
            _imageCroppingBox = new ImageCroppingBox();

            // 配置裁剪框控件
            _imageCroppingBox.Size = this.Size;
            _imageCroppingBox.Visible = false;
            _imageCroppingBox.ContextMenuStrip = _cropContextMenu;
            _imageCroppingBox.DoubleClick += new EventHandler(SaveCrop);

            if (!this.Controls.Contains(_imageCroppingBox))
            {
                this.Controls.Add(_imageCroppingBox);
                this._imageCroppingBox.TabIndex = 3;
            }

            // 初始化裁剪菜单
            InitializeCropContextMenu();

            // 设置主上下文菜单
            this.ContextMenuStrip = _mainContextMenu;

            // 初始更新菜单内容
            // 在构造函数中调用一次，后续状态变化时需手动调用
            UpdateContextMenu();
        }

        /// <summary>
        /// 初始化裁剪上下文菜单
        /// 创建并配置裁剪操作专用的上下文菜单
        /// 此方法应在InitializeMenuSystem中调用，仅需初始化一次
        /// </summary>
        private void InitializeCropContextMenu()
        {
            _cropContextMenu.Items.Clear();
            _cropContextMenu.Items.Add(new ToolStripMenuItem(MENU_CANCEL_CROP, null, new EventHandler(StopCrop), MENU_CANCEL_CROP));
        }

        /// <summary>
        /// 更新主上下文菜单内容
        /// 根据当前控件状态（有无图片）动态生成相应的菜单项
        /// 此方法应在以下情况调用：
        /// 1. 图片加载完成后
        /// 2. 图片被清除或删除后
        /// 3. 菜单配置属性更改后
        /// 4. 构造函数初始化时
        /// </summary>
        private void UpdateContextMenu()
        {
            _mainContextMenu.Items.Clear();

            bool hasImages = this.Image != null || (MultiImageSupport && images.Count > 0);

            if (hasImages)
            {
                AddImageAvailableMenuItems();
            }
            else
            {
                AddEmptyStateMenuItems();
            }
        }

        /// <summary>
        /// 添加有图片时的菜单项
        /// 根据配置属性动态添加适用于有图片状态的菜单项
        /// 包括查看、编辑、添加和删除等功能选项
        /// </summary>
        private void AddImageAvailableMenuItems()
        {
            bool hasAddedMenuItem = false;

            // 查看和编辑相关菜单项
            if (ShowViewLargeImageMenuItem)
            {
                _mainContextMenu.Items.Add(new ToolStripMenuItem(MENU_VIEW_LARGE, null, new EventHandler(ViewLargeImage), MENU_VIEW_LARGE));
                hasAddedMenuItem = true;
            }

            if (ShowCropImageMenuItem)
            {
                _mainContextMenu.Items.Add(new ToolStripMenuItem(MENU_CROP, null, new EventHandler(StartCrop), MENU_CROP));
                hasAddedMenuItem = true;
            }

            // 添加图片选项
            if (ShowAddImageMenuItem)
            {
                _mainContextMenu.Items.Add(new ToolStripMenuItem(MENU_ADD_IMAGE, null, new EventHandler(AddImage), MENU_ADD_IMAGE));
                hasAddedMenuItem = true;
            }

            // 如果添加了菜单项，则添加分隔线
            if (hasAddedMenuItem && (ShowClearImageMenuItem || (MultiImageSupport && images.Count > 1 && ShowDeleteCurrentImageMenuItem)))
            {
                _mainContextMenu.Items.Add(new ToolStripSeparator());
            }

            // 清除选项
            if (ShowClearImageMenuItem)
            {
                _mainContextMenu.Items.Add(new ToolStripMenuItem(MENU_CLEAR_IMAGE, null, new EventHandler(ClearImage), MENU_CLEAR_IMAGE));
            }

            // 多图片模式下的额外选项
            if (MultiImageSupport && images.Count > 1 && ShowDeleteCurrentImageMenuItem)
            {
                _mainContextMenu.Items.Add(new ToolStripMenuItem(MENU_DELETE_CURRENT, null, new EventHandler(DeleteCurrentImage), MENU_DELETE_CURRENT));
            }
        }

        /// <summary>
        /// 添加无图片时的菜单项
        /// 根据配置属性动态添加适用于无图片状态的菜单项
        /// 主要包括添加和粘贴图片的选项
        /// </summary>
        private void AddEmptyStateMenuItems()
        {
            if (ShowPasteImageMenuItem)
            {
                _mainContextMenu.Items.Add(new ToolStripMenuItem(MENU_PASTE_IMAGE, null, new EventHandler(PasteImage), MENU_PASTE_IMAGE));
            }

            if (ShowAddImageMenuItem)
            {
                _mainContextMenu.Items.Add(new ToolStripMenuItem(MENU_ADD_IMAGE, null, new EventHandler(AddImage), MENU_ADD_IMAGE));
            }
        }








        /// <summary>
        /// 优化的单张图片加载方法，支持byte[]和可选的ImageInfo
        /// </summary>
        /// <param name="imageBytes">图片字节数据</param>
        /// <param name="imageInfo">可选的图片信息对象</param>
        /// <param name="isFromServer">是否从服务器加载</param>
        public void LoadImage(byte[] imageBytes, ImageInfo imageInfo = null, bool isFromServer = false)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                this.Image = null;
                return;
            }

            // 处理ImageInfo参数
            List<ImageInfo> imageInfosList = null;
            if (imageInfo != null)
            {
                imageInfosList = new List<ImageInfo> { imageInfo };
            }

            // 调用内部统一加载方法
            LoadImagesInternal(new List<byte[]> { imageBytes }, imageInfosList, isFromServer);
        }

        /// <summary>
        /// 优化的多张图片加载方法，支持byte[]列表和可选的ImageInfo列表
        /// </summary>
        /// <param name="imageBytesList">图片字节数据列表</param>
        /// <param name="imageInfosList">可选的图片信息对象列表</param>
        /// <param name="isFromServer">是否从服务器加载</param>
        public void LoadImages(List<byte[]> imageBytesList, List<ImageInfo> imageInfosList = null, bool isFromServer = false)
        {
            if (imageBytesList == null || imageBytesList.Count == 0)
            {
                this.Image = null;
                return;
            }

            // 调用内部统一加载方法
            LoadImagesInternal(imageBytesList, imageInfosList, isFromServer);
        }

        /// <summary>
        /// 内部统一的图片加载实现方法
        /// </summary>
        /// <param name="imageBytesList">图片字节数组列表</param>
        /// <param name="imageInfosList">图片信息列表，包含原始文件名等详细信息</param>
        /// <param name="isFromServer">是否从服务器加载，影响IsUpdated初始值</param>
        private void LoadImagesInternal(List<byte[]> imageBytesList, List<ImageInfo> imageInfosList = null, bool isFromServer = false)
        {
            images.Clear();
            imageInfos.Clear();

            // 自动启用多图片支持
            MultiImageSupport = imageBytesList != null && imageBytesList.Count > 1;

            if (imageBytesList == null || imageBytesList.Count == 0)
            {
                this.Image = null;
                return;
            }

            // 收集加载失败的图片信息
            List<int> failedImages = new List<int>();
            List<string> errorMessages = new List<string>();

            for (int i = 0; i < imageBytesList.Count; i++)
            {
                try
                {
                    byte[] imageBytes = imageBytesList[i];

                    // 计算哈希值，用于缓存查找
                    string hashValue = CalculateImageHash(imageBytes);

                    // 尝试从缓存中获取图片
                    Image cachedImage = GetFromCache(hashValue);
                    Image loadedImage;

                    if (cachedImage != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"从缓存加载图片中的第{i + 1}张: {hashValue}");
                        // 创建图片副本，避免修改原始缓存图片
                        using (var ms = new MemoryStream())
                        {
                            cachedImage.Save(ms, ImageFormat.Png);
                            loadedImage = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"缓存未命中，加载新图片: {hashValue}");
                        // 缓存未命中，从字节数组加载
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            loadedImage = Image.FromStream(ms);
                        }

                        // 添加到缓存
                        AddToCache(hashValue, new Bitmap(loadedImage));
                    }

                    // 保存图片引用
                    images.Add(loadedImage);

                    // 创建ImageInfo对象，优先使用传入的图片信息
                    ImageInfo newImageInfo;

                    // 如果提供了图片信息列表且索引有效，则使用传入的ImageInfo对象并更新相关属性
                    if (imageInfosList != null && imageInfosList.Count > i && imageInfosList[i] != null)
                    {
                        // 创建一个新的ImageInfo实例，复制传入的所有属性
                        newImageInfo = new ImageInfo
                        {
                            FileId = imageInfosList[i].FileId,
                            OriginalFileName = imageInfosList[i].OriginalFileName,
                            FileSize = imageBytes.Length, // 更新为实际加载的字节大小
                            CreateTime = imageInfosList[i].CreateTime, // 保留原始创建时间
                            ModifiedAt = imageInfosList[i].ModifiedAt,
                            FileType = imageInfosList[i].FileType,
                            FileExtension = imageInfosList[i].FileExtension,
                            HashValue = hashValue, // 更新哈希值
                            Metadata = imageInfosList[i].Metadata != null ? new Dictionary<string, string>(imageInfosList[i].Metadata) : new Dictionary<string, string>(),
                            IsUpdated = !isFromServer // 如果是从服务器加载，标记为未更新
                        };
                    }
                    else
                    {
                        // 否则创建新的ImageInfo对象，使用默认命名
                        string fileName = $"图片{i + 1}";
                        newImageInfo = new ImageInfo
                        {
                            OriginalFileName = fileName, // 使用默认文件名
                            FileSize = imageBytes.Length,
                            CreateTime = DateTime.Now,
                            HashValue = hashValue, // 设置哈希值
                            Metadata = new Dictionary<string, string>(),
                            FileType = "",
                            ModifiedAt = null,
                            IsUpdated = !isFromServer // 如果是从服务器加载，标记为未更新
                        };
                    }

                    // 添加到图片信息列表
                    imageInfos.Add(newImageInfo);
                }
                catch (Exception ex)
                {
                    failedImages.Add(i);
                    errorMessages.Add(ex.Message);
                    System.Diagnostics.Debug.WriteLine($"加载图片失败 (索引 {i}): {ex.Message}");
                }
            }

            // 处理加载结果
            if (images.Count > 0)
            {
                // 如果是单张图片，直接设置Image属性
                if (images.Count == 1)
                {
                    this.Image = images[0];
                    // 确保图片信息正确设置
                    if (imageInfos.Count > 0)
                    {
                        imageInfos[0].Width = this.Image.Width;
                        imageInfos[0].Height = this.Image.Height;
                    }
                }
                else
                {
                    // 多张图片，显示第一张并创建导航控件
                    currentImageIndex = 0;
                    ShowCurrentImage();
                    CreateNavigationControls();
                }

                // 创建信息面板
                CreateInfoPanel();
                UpdateInfoPanel();
            }
            else
            {
                this.Image = null;
            }

            // 更新上下文菜单,确保显示正确的菜单项
            UpdateContextMenu();

            // 如果有加载失败的图片，记录日志
            if (failedImages.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"部分图片加载失败，共 {failedImages.Count} 张");
            }
        }




        /// <summary>
        /// 处理已加载的图片
        /// </summary>
        /// <summary>
        /// 处理已加载的图片
        /// </summary>
        /// <param name="image">已加载的Image对象</param>
        /// <param name="hashValue">图片哈希值</param>
        /// <param name="imageBytes">图片字节数组</param>
        /// <param name="fileName">文件名</param>
        /// <param name="isFromServer">是否从服务器加载</param>
        private void ProcessLoadedImage(Image image, string hashValue, byte[] imageBytes, string fileName = null, bool isFromServer = false)
        {
            if (image == null || string.IsNullOrEmpty(hashValue))
                return;

            try
            {
                // 确保使用提供的原始文件名
                string originalFileName = !string.IsNullOrWhiteSpace(fileName) ? fileName : "图片1";

                if (MultiImageSupport)
                {
                    images.Add(image);

                    imageInfos.Add(new ImageInfo
                    {
                        OriginalFileName = originalFileName,
                        FileSize = imageBytes?.Length ?? 0,
                        CreateTime = DateTime.Now,
                        FileType = string.IsNullOrEmpty(originalFileName) ? "" : Path.GetExtension(originalFileName).TrimStart('.'),
                        HashValue = hashValue,
                        Metadata = new Dictionary<string, string>(),
                        ModifiedAt = null,
                        IsUpdated = !isFromServer,
                        Width = image?.Width ?? 0,
                        Height = image?.Height ?? 0
                    });

                    currentImageIndex = images.Count - 1;
                    ShowCurrentImage();
                    CreateNavigationControls();
                    CreateInfoPanel();
                }
                else
                {
                    this.Image = image;

                    // 在单图片模式下也保存文件名信息
                    if (imageInfos.Count > 0)
                    {
                        imageInfos[0].OriginalFileName = originalFileName;
                        imageInfos[0].FileSize = imageBytes?.Length ?? 0;
                        imageInfos[0].CreateTime = DateTime.Now;
                        imageInfos[0].HashValue = hashValue;
                        imageInfos[0].IsUpdated = !isFromServer;
                        if (!string.IsNullOrWhiteSpace(originalFileName))
                        {
                            imageInfos[0].FileType = Path.GetExtension(originalFileName).TrimStart('.');
                        }
                    }
                    else
                    {
                        // 如果没有信息列表，创建一个
                        imageInfos.Add(new ImageInfo
                        {
                            OriginalFileName = originalFileName,
                            FileSize = imageBytes?.Length ?? 0,
                            CreateTime = DateTime.Now,
                            FileType = !string.IsNullOrWhiteSpace(originalFileName) ? Path.GetExtension(originalFileName).TrimStart('.') : "",
                            HashValue = hashValue,
                            IsUpdated = !isFromServer,
                            Width = image?.Width ?? 0,
                            Height = image?.Height ?? 0
                        });
                    }

                    UpdateInfoPanel();
                }
            }
            catch (Exception ex)
            {
                // 处理失败，释放图片资源
                // 使用ImageUtils工具类安全释放图片资源
                ImageUtils.SafeDispose(ref image);
                System.Diagnostics.Debug.WriteLine($"处理图片失败: {ex.Message}");
                throw;
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

                // 确保ImageUpdateManager中的更新状态与imageInfos同步
                foreach (var imageInfo in imageInfos)
                {
                    if (imageInfo.IsUpdated)
                    {
                        _updateManager.MarkImageAsUpdated(imageInfo);
                    }
                }

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
                        // 读取图片字节数组，用于计算哈希值
                        byte[] imageBytes = File.ReadAllBytes(path);
                        images.Add(Image.FromStream(new MemoryStream(imageBytes)));

                        // 获取文件信息
                        var fileInfo = new FileInfo(path);

                        // 计算哈希值
                        string hashValue = CalculateImageHash(imageBytes);

                        if (MultiImageSupport)
                        {
                            imageInfos.Add(new ImageInfo
                            {
                                OriginalFileName = fileInfo.Name,
                                FileSize = imageBytes.Length, // 使用字节数组长度作为文件大小
                                CreateTime = fileInfo.CreationTime,
                                FileType = Path.GetExtension(path).TrimStart('.'),
                                HashValue = hashValue,
                                Metadata = new Dictionary<string, string>(),
                                ModifiedAt = null,
                                IsUpdated = true // 标记为已更新
                            });
                        }
                        else
                        {
                            // 单图片模式下也保存文件名信息
                            if (imageInfos.Count > 0)
                            {
                                imageInfos[0].OriginalFileName = fileInfo.Name;
                                imageInfos[0].FileSize = imageBytes.Length;
                                imageInfos[0].CreateTime = fileInfo.CreationTime;
                                imageInfos[0].FileType = Path.GetExtension(path).TrimStart('.');
                                imageInfos[0].HashValue = hashValue;
                                imageInfos[0].Metadata = new Dictionary<string, string>();
                                imageInfos[0].ModifiedAt = null;
                                imageInfos[0].IsUpdated = true;
                                // 添加Width和Height属性
                                if (images.Count > 0)
                                {
                                    imageInfos[0].Width = images[0]?.Width ?? 0;
                                    imageInfos[0].Height = images[0]?.Height ?? 0;
                                }
                            }
                            else
                            {
                                // 如果没有信息列表，创建一个
                                imageInfos.Add(new ImageInfo
                                {
                                    OriginalFileName = fileInfo.Name,
                                    FileSize = imageBytes.Length, // 使用字节数组长度作为文件大小
                                    CreateTime = fileInfo.CreationTime,
                                    FileType = Path.GetExtension(path).TrimStart('.'),
                                    HashValue = hashValue,
                                    Metadata = new Dictionary<string, string>(),
                                    ModifiedAt = null,
                                    IsUpdated = true, // 标记为已更新
                                    Width = images[images.Count - 1]?.Width ?? 0,
                                    Height = images[images.Count - 1]?.Height ?? 0
                                });
                            }
                            UpdateInfoPanel();
                        }
                    }
                    catch (Exception ex)
                    {
                        // 加载失败，记录错误信息
                        System.Diagnostics.Debug.WriteLine($"加载图片失败: {ex.Message}");
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
                BackColor = Color.FromArgb(64, 128, 128, 255), // 半透明蓝色背景
                Visible = true,
                BorderStyle = BorderStyle.FixedSingle // 添加边框
            };

            // 上一张按钮
            prevButton = new Button
            {
                Text = "<",
                Width = 30,
                Height = 25,
                Location = new Point(10, 2),
                Enabled = currentImageIndex > 0,
                BackColor = Color.FromArgb(100, 150, 255), // 蓝色背景
                ForeColor = Color.White, // 白色文字
                FlatStyle = FlatStyle.Flat
            };
            prevButton.Click += PrevButton_Click;

            // 下一张按钮
            nextButton = new Button
            {
                Text = ">",
                Width = 30,
                Height = 25,
                Location = new Point(70, 2),
                Enabled = currentImageIndex < images.Count - 1,
                BackColor = Color.FromArgb(100, 150, 255), // 蓝色背景
                ForeColor = Color.White, // 白色文字
                FlatStyle = FlatStyle.Flat
            };
            nextButton.Click += NextButton_Click;

            // 页码信息
            pageInfoLabel = new Label
            {
                Text = "1/1",
                AutoSize = true,
                Location = new Point(110, 7),
                Font = new Font(this.Font.FontFamily, 9, FontStyle.Bold), // 粗体字体
                ForeColor = Color.FromArgb(0, 64, 128) // 深蓝色文字
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
                BackColor = Color.FromArgb(200, 230, 240, 255), // 更美观的半透明蓝色背景
                Visible = true,
                Padding = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle // 添加边框
            };

            // 文件名标签
            fileNameLabel = new Label
            {
                AutoSize = true,
                Location = new Point(5, 5),
                Font = new Font(this.Font.FontFamily, 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 64, 128), // 深蓝色文字
                MaximumSize = new Size(300, 0),
                AutoEllipsis = true
            };

            // 文件大小标签
            fileSizeLabel = new Label
            {
                AutoSize = true,
                Location = new Point(5, 25),
                Font = new Font(this.Font.FontFamily, 8),
                ForeColor = Color.FromArgb(64, 64, 64) // 深灰色文字
            };

            // 创建时间标签
            createTimeLabel = new Label
            {
                AutoSize = true,
                Location = new Point(150, 25),
                Font = new Font(this.Font.FontFamily, 8),
                ForeColor = Color.FromArgb(64, 64, 64) // 深灰色文字
            };

            // 文件类型标签
            Label fileTypeLabel = new Label
            {
                AutoSize = true,
                Location = new Point(5, 45),
                Font = new Font(this.Font.FontFamily, 8),
                ForeColor = Color.FromArgb(64, 64, 64) // 深灰色文字
            };

            // 业务类型标签
            Label businessTypeLabel = new Label
            {
                AutoSize = true,
                Location = new Point(150, 45),
                Font = new Font(this.Font.FontFamily, 8),
                ForeColor = Color.FromArgb(64, 64, 64) // 深灰色文字
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
                    fileNameLabel.Text = imageInfo.OriginalFileName;
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
                // 没有图片时，清空所有标签的文本内容
                if (fileNameLabel != null)
                {
                    fileNameLabel.Text = "";
                }

                if (fileSizeLabel != null)
                {
                    fileSizeLabel.Text = "";
                }

                if (createTimeLabel != null)
                {
                    createTimeLabel.Text = "";
                }

                // 清空文件类型信息
                var fileTypeLabel = infoPanel.Controls.Cast<Control>().FirstOrDefault(c => c is Label && c.Location.Y == 45 && c.Location.X == 5) as Label;
                if (fileTypeLabel != null)
                {
                    fileTypeLabel.Text = "";
                }

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

        /// <summary>
        /// 动态添加右键菜单，根据是否有图片智能显示不同菜单项
        /// 保留此方法以向后兼容，内部调用新的UpdateContextMenu方法
        /// </summary>
        private void AddContextMenuItems()
        {
            // 调用新的菜单更新方法
            UpdateContextMenu();
        }

        /// <summary>
        /// 删除当前图片
        /// 
        /// 功能说明：
        /// - 从显示列表中删除当前选中的图片
        /// - 如果图片有 FileId（已上传到服务器），则标记为待删除
        /// - 将删除的图片信息保存到 _deletedImages 列表，用于后续服务器同步删除
        /// 
        /// 处理逻辑：
        /// 1. 保存当前图片的信息（在删除前）
        /// 2. 如果图片有 FileId，设置 IsDeleted=true 和 IsUpdated=true
        /// 3. 将图片信息添加到 _deletedImages 列表（避免重复添加）
        /// 4. 从显示列表中移除图片（images 和 imageInfos）
        /// 5. 调整当前索引（确保指向有效图片）
        /// 6. 更新UI显示（显示下一张图片或清空）
        /// 
        /// 特殊情况处理：
        /// - 删除的是最后一张图片：索引减1，显示前一张
        /// - 删除的是唯一一张图片：清空显示
        /// - 新添加的图片（无 FileId）：直接从列表中删除，不标记为待删除
        /// 
        /// 数据一致性：
        /// - 已删除的图片信息保存在 _deletedImages 中，不会丢失
        /// - 保存时会调用 FileBusinessService.DeleteFileAsync 删除服务器文件
        /// - 成功删除后，调用 ClearDeletedImagesList() 清空列表
        /// </summary>
        private void DeleteCurrentImage(object sender, EventArgs e)
        {
            if (MultiImageSupport && images.Count > 0 && currentImageIndex < imageInfos.Count)
            {
                // 保存被删除图片的信息（必须在删除前保存）
                ImageInfo deletedImageInfo = imageInfos[currentImageIndex];
                
                // 标记图片为已删除（如果有FileId表示是已上传的图片）
                if (deletedImageInfo != null && deletedImageInfo.FileId > 0)
                {
                    deletedImageInfo.IsDeleted = true;
                    deletedImageInfo.IsUpdated = true; // 标记为需要更新
                    
                    // 保存到删除列表，用于后续处理
                    // 使用 Contains 检查避免重复添加同一图片引用
                    if (!_deletedImages.Contains(deletedImageInfo))
                    {
                        _deletedImages.Add(deletedImageInfo);
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"标记图片为已删除: {deletedImageInfo.OriginalFileName}, FileId: {deletedImageInfo.FileId}");
                }
                else
                {
                    // 新添加的图片（无 FileId），直接删除，不需要标记为待删除
                    System.Diagnostics.Debug.WriteLine($"删除新图片（未上传）: {deletedImageInfo?.OriginalFileName}");
                }
                
                // 从显示列表中删除当前图片
                images.RemoveAt(currentImageIndex);
                imageInfos.RemoveAt(currentImageIndex);

                // 调整当前索引：确保索引指向有效的图片
                if (currentImageIndex >= images.Count && images.Count > 0)
                {
                    currentImageIndex = images.Count - 1;
                }

                // 更新UI显示
                if (images.Count > 0)
                {
                    // 还有其他图片，显示当前索引的图片
                    ShowCurrentImage();
                }
                else
                {
                    // 没有图片了，清空显示
                    this.Image = null;
                    imagePaths = "";
                    UpdateInfoPanel();
                }

                // 更新导航控件和页面信息
                CreateNavigationControls();
                UpdatePageInfo();
                UpdateImagePathsFromImages();
                // 更新上下文菜单
                UpdateContextMenu();
            }
        }

        private void ClearImage(object sender, EventArgs e)
        {
            ClearImage();
        }
        
        /// <summary>
        /// 清空所有图片
        /// 标记所有已有图片为已删除状态
        /// 注意：此方法会标记所有图片为删除，但不清空删除列表（用于后续服务器删除）
        /// </summary>
        public void ClearImage()
        {
            this.Image = null;
            if (MultiImageSupport)
            {
                // 在多图片模式下，清除所有图片
                // 标记所有有FileId的图片为已删除（这些图片需要从服务器删除）
                foreach (var imageInfo in imageInfos)
                {
                    if (imageInfo != null && imageInfo.FileId > 0)
                    {
                        imageInfo.IsDeleted = true;
                        imageInfo.IsUpdated = true;
                        
                        // 保存到删除列表，用于后续处理
                        if (!_deletedImages.Contains(imageInfo))
                        {
                            _deletedImages.Add(imageInfo);
                        }
                        
                        System.Diagnostics.Debug.WriteLine($"ClearImage: 标记图片为已删除: {imageInfo.OriginalFileName}, FileId: {imageInfo.FileId}");
                    }
                }
                
                // 清空图片列表，但保留删除列表（因为需要上传删除操作到服务器）
                images.Clear();
                imageInfos.Clear();
                currentImageIndex = 0;
                // 不要清空 _deletedImages 列表！这些图片需要被删除
                imagePaths = "";

                // 更新导航控件和页面信息
                CreateNavigationControls();
                UpdatePageInfo();
                UpdateImagePathsFromImages();
            }
            else
            {
                // 单图片模式下也需要清空imageInfos
                if (imageInfos.Count > 0)
                {
                    imageInfos.Clear();
                }
            }
            // 更新信息面板，确保清空
            UpdateInfoPanel();
            // 隐藏信息面板
            if (infoPanel != null)
            {
                infoPanel.Visible = false;
            }
            // 更新上下文菜单
            UpdateContextMenu();
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
                            // 读取图片字节数组，用于计算哈希值
                            byte[] imageBytes = File.ReadAllBytes(fileName);
                            var image = Image.FromStream(new MemoryStream(imageBytes));
                            images.Add(image);

                            // 获取文件信息
                            var fileInfo = new FileInfo(fileName);

                            // 计算哈希值
                            string hashValue = CalculateImageHash(imageBytes);

                            if (MultiImageSupport)
                            {
                                imageInfos.Add(new ImageInfo
                                {
                                    OriginalFileName = fileInfo.Name,
                                    FileSize = imageBytes.Length, // 使用字节数组长度作为文件大小
                                    CreateTime = fileInfo.CreationTime,
                                    FileType = Path.GetExtension(fileName).TrimStart('.'),
                                    HashValue = hashValue,
                                    IsUpdated = true // 标记为已更新
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
                            else
                            {
                                // 单图片模式下也保存文件名信息
                                if (imageInfos.Count > 0)
                                {
                                    imageInfos[0].OriginalFileName = fileInfo.Name;
                                    imageInfos[0].FileSize = imageBytes.Length;
                                    imageInfos[0].CreateTime = fileInfo.CreationTime;
                                    imageInfos[0].FileType = Path.GetExtension(fileName).TrimStart('.');
                                    imageInfos[0].HashValue = hashValue;
                                    imageInfos[0].IsUpdated = true;
                                }
                                else
                                {
                                    // 如果没有信息列表，创建一个
                                    imageInfos.Add(new ImageInfo
                                    {
                                        OriginalFileName = fileInfo.Name,
                                        FileSize = imageBytes.Length,
                                        CreateTime = fileInfo.CreationTime,
                                        FileType = Path.GetExtension(fileName).TrimStart('.'),
                                        HashValue = hashValue,
                                        IsUpdated = true
                                    });
                                }
                                UpdateInfoPanel();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"加载图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    UpdatePageInfo();
                    UpdateImagePathsFromImages(); // 更新图片路径
                    UpdateContextMenu(); // 更新上下文菜单
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
                _imageCroppingBox.Visible = true;
                _imageCroppingBox.TabIndex = this.TabIndex + 1;
                _imageCroppingBox.Image = this.Image;
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
                _imageCroppingBox.Visible = false;

                // 保存原始图片信息
                ImageInfo originalInfo = null;
                if (MultiImageSupport && currentImageIndex < imageInfos.Count)
                {
                    originalInfo = imageInfos[currentImageIndex];
                }
                else if (imageInfos.Count > 0)
                {
                    originalInfo = imageInfos[0];
                }

                // 获取裁剪后的图片
                this.Image = _imageCroppingBox.GetSelectedImage();

                // 更新多图片列表中的当前图片
                if (MultiImageSupport && currentImageIndex < images.Count)
                {
                    images[currentImageIndex] = this.Image;

                    // 更新图片信息，保留原始文件名
                    if (currentImageIndex < imageInfos.Count && originalInfo != null)
                    {
                        imageInfos[currentImageIndex] = new ImageInfo
                        {
                            OriginalFileName = originalInfo.OriginalFileName, // 保留原始文件名
                            FileSize = GetImageFileSize(this.Image), // 更新文件大小
                            CreateTime = originalInfo.CreateTime, // 保留创建时间
                            Metadata = originalInfo.Metadata, // 保留元数据
                            FileType = originalInfo.FileType, // 保留文件类型
                            FileExtension = originalInfo.FileExtension, // 保留文件扩展名
                            HashValue = CalculateImageHash(this.Image), // 更新哈希值
                            Width = this.Image?.Width ?? 0,
                            Height = this.Image?.Height ?? 0
                        };
                    }
                    else if (originalInfo != null) // 如果列表中没有对应的信息，则添加
                    {
                        imageInfos.Add(new ImageInfo
                        {
                            OriginalFileName = originalInfo.OriginalFileName,
                            FileSize = GetImageFileSize(this.Image),
                            CreateTime = originalInfo.CreateTime,
                            Metadata = originalInfo.Metadata,
                            FileType = originalInfo.FileType,
                            FileExtension = originalInfo.FileExtension,
                            HashValue = CalculateImageHash(this.Image), // 添加哈希值
                            Width = this.Image?.Width ?? 0,
                            Height = this.Image?.Height ?? 0
                        });
                    }
                }
                else if (originalInfo != null) // 单图片模式
                {
                    // 在单图片模式下，我们也使用UpdateImageWithOriginalInfo，但需要特殊处理
                    try
                    {
                        // 保存原始图片信息
                        ImageInfo tempInfo = imageInfos.Count > 0 ? imageInfos[0] : null;

                        // 为单图片模式临时启用多图片支持以便使用更新方法
                        bool wasMultiImageSupported = MultiImageSupport;
                        MultiImageSupport = true;

                        // 确保图片列表不为空
                        if (images == null || images.Count == 0)
                        {
                            images = new List<Image> { this.Image };
                            currentImageIndex = 0;
                        }
                        else if (images.Count > 0)
                        {
                            images[0] = this.Image;
                            currentImageIndex = 0;
                        }

                        // 更新图片信息
                        UpdateImageWithOriginalInfo(0, this.Image, "旋转图片");

                        // 标记为已更新
                        MarkImageAsUpdated(0);

                        // 恢复多图片支持设置
                        MultiImageSupport = wasMultiImageSupported;

                        // 确保单图片模式下的Image属性也被更新
                        if (!wasMultiImageSupported && images.Count > 0)
                        {
                            this.Image = images[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"单图片模式下更新图片信息失败: {ex.Message}");
                    }
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
                this._imageCroppingBox.Visible = false;
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
                            // 读取图片字节数组，用于计算哈希值
                            byte[] imageBytes = File.ReadAllBytes(file);
                            var image = Image.FromStream(new MemoryStream(imageBytes));
                            images.Add(image);

                            // 获取文件信息
                            var fileInfo = new FileInfo(file);

                            // 创建ImageInfo并设置哈希值
                            string hashValue = CalculateImageHash(imageBytes);
                            if (imageInfos != null)
                            {
                                imageInfos.Add(new ImageInfo
                                {
                                    OriginalFileName = fileInfo.Name,
                                    FileSize = imageBytes.Length,
                                    CreateTime = fileInfo.CreationTime,
                                    FileType = Path.GetExtension(fileInfo.Name).TrimStart('.'),
                                    HashValue = hashValue,
                                    IsUpdated = true,
                                    Width = image?.Width ?? 0,
                                    Height = image?.Height ?? 0
                                });
                            }
                            else
                            {
                                // 如果没有信息列表，创建一个
                                imageInfos.Add(new ImageInfo
                                {
                                    OriginalFileName = fileInfo.Name,
                                    FileSize = imageBytes.Length,
                                    CreateTime = fileInfo.CreationTime,
                                    FileType = Path.GetExtension(fileInfo.Name).TrimStart('.'),
                                    HashValue = hashValue,
                                    IsUpdated = true,
                                    Width = image?.Width ?? 0,
                                    Height = image?.Height ?? 0
                                });
                            }

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
                    // 单图片模式，替换第一张图片
                    byte[] imageBytes = File.ReadAllBytes(files[0]);
                    Image image = Image.FromStream(new MemoryStream(imageBytes));

                    // 计算哈希值
                    string newhash = CalculateImageHash(imageBytes);
                    RowImage.SetImageNewHash(newhash);
                    this.Image = image;

                    // 获取文件信息并更新ImageInfo
                    var fileInfo = new FileInfo(files[0]);
                    if (images.Count > 0)
                    {
                        // 替换现有图片
                        images[0] = image;
                        imageInfos[0] = new ImageInfo
                        {
                            OriginalFileName = fileInfo.Name,
                            FileSize = imageBytes.Length,
                            CreateTime = fileInfo.CreationTime,
                            Metadata = new Dictionary<string, string>(),
                            FileExtension = Path.GetExtension(fileInfo.Name).TrimStart('.').ToLower(),
                            HashValue = newhash,
                            IsUpdated = true,
                            Width = image?.Width ?? 0,
                            Height = image?.Height ?? 0
                        };
                    }
                    else
                    {
                        // 如果还没有图片，添加新图片
                        images.Add(image);
                        imageInfos.Add(new ImageInfo
                        {
                            OriginalFileName = fileInfo.Name,
                            FileSize = imageBytes.Length,
                            CreateTime = fileInfo.CreationTime,
                            Metadata = new Dictionary<string, string>(),
                            FileExtension = Path.GetExtension(fileInfo.Name).TrimStart('.').ToLower(),
                            HashValue = newhash,
                            IsUpdated = true,
                            Width = image?.Width ?? 0,
                            Height = image?.Height ?? 0
                        });
                    }

                    UpdateInfoPanel();
                }
            }
            base.OnDragDrop(e);
        }


        // 粘贴图片事件
        private void PasteImage(object sender, EventArgs e)
        {
            try
            {
                // this.CanFocus
                if (Clipboard.ContainsImage())
                {
                    Image image = Clipboard.GetImage();

                    // 获取图片的实际格式扩展名
                    string imageExtension = GetImageFormatExtension(image);

                    // 转换为字节数组以计算哈希值
                    byte[] imageBytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // 根据图片实际格式保存
                        ImageFormat format = GetImageFormatFromExtension(imageExtension);
                        image.Save(ms, format);
                        imageBytes = ms.ToArray();
                    }

                    string newhash = CalculateImageHash(imageBytes);
                    RowImage.SetImageNewHash(newhash);
                    this.Image = image;

                    // 尝试从剪贴板获取文件名
                    string fileName = GetFileNameFromClipboard();
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        // 如果无法获取,使用默认名称并添加正确的扩展名
                        fileName = $"粘贴图片{(MultiImageSupport ? images.Count + 1 : 1)}.{imageExtension}";
                    }
                    else
                    {
                        // 确保文件名包含扩展名
                        if (!fileName.Contains('.'))
                        {
                            fileName = $"{fileName}.{imageExtension}";
                        }
                    }

                    // 如果是多图片模式，添加到列表中
                    if (MultiImageSupport)
                    {
                        // 列表不为空时，将粘贴的图片添加到列表末尾，实现第二张、第三张依次递增
                        if (images.Count > 0)
                        {
                            images.Add(image);
                            // 添加图片信息，优先使用从剪贴板获取的文件名
                            ImageInfo newImageInfo = new ImageInfo
                            {
                                OriginalFileName = fileName,
                                FileSize = GetImageFileSize(image), // 尝试获取图片大小
                                CreateTime = DateTime.Now,
                                FileType = GetImageFormatExtension(image),
                                FileExtension = GetImageFormatExtension(image),
                                Width = image?.Width ?? 0,
                                Height = image?.Height ?? 0,
                                IsUpdated = true, // 标记为需要更新
                                HashValue = newhash // 设置哈希值
                            };
                            imageInfos.Add(newImageInfo);
                            _updateManager.MarkImageAsUpdated(newImageInfo); // 标记为需要更新
                            currentImageIndex = images.Count - 1; // 设置为新添加的图片
                        }
                        else
                        {
                            // 列表为空时，正常添加到末尾
                            images.Add(image);
                            // 添加图片信息，优先使用从剪贴板获取的文件名
                            ImageInfo newImageInfo = new ImageInfo
                            {
                                OriginalFileName = fileName,
                                FileSize = GetImageFileSize(image), // 尝试获取图片大小
                                CreateTime = DateTime.Now,
                                FileType = GetImageFormatExtension(image),
                                FileExtension = GetImageFormatExtension(image),
                                Width = image?.Width ?? 0,
                                Height = image?.Height ?? 0,
                                IsUpdated = true, // 标记为需要更新
                                HashValue = newhash // 设置哈希值
                            };
                            imageInfos.Add(newImageInfo);
                            _updateManager.MarkImageAsUpdated(newImageInfo); // 标记为需要更新
                            currentImageIndex = 0;
                        }
                        CreateNavigationControls();
                        UpdatePageInfo();
                        UpdateImagePathsFromImages(); // 更新图片路径
                        CreateInfoPanel();
                    }
                    else
                    {
                        // 单图片模式下也保存文件名信息
                        if (imageInfos.Count > 0)
                        {
                            imageInfos[0].OriginalFileName = fileName;
                            imageInfos[0].FileSize = GetImageFileSize(image);
                            imageInfos[0].CreateTime = DateTime.Now;
                            imageInfos[0].FileType = GetImageFormatExtension(image);
                            imageInfos[0].FileExtension = GetImageFormatExtension(image);
                            imageInfos[0].HashValue = newhash; // 设置哈希值
                            imageInfos[0].IsUpdated = true; // 标记为需要更新
                            _updateManager.MarkImageAsUpdated(imageInfos[0]); // 标记为需要更新
                        }
                        else
                        {
                            // 如果没有信息列表，创建一个
                            ImageInfo newImageInfo = new ImageInfo
                            {
                                OriginalFileName = fileName,
                                FileExtension = GetImageFormatExtension(image),
                                FileSize = GetImageFileSize(image),
                                CreateTime = DateTime.Now,
                                FileType = GetImageFormatExtension(image),
                                Width = image?.Width ?? 0,
                                Height = image?.Height ?? 0,
                                IsUpdated = true, // 标记为需要更新
                                HashValue = newhash // 设置哈希值
                            };
                            imageInfos.Add(newImageInfo);
                            _updateManager.MarkImageAsUpdated(newImageInfo); // 标记为需要更新
                        }
                        UpdateInfoPanel();
                    }

                    UpdateContextMenu();
                }
            }
            catch (Exception ex)
            {
                // 粘贴失败时的错误处理
                MessageBox.Show($"粘贴图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"图片粘贴错误: {ex.ToString()}");
            }
        }

        /// <summary>
        /// 尝试从剪贴板获取文件名
        /// </summary>
        /// <returns>文件名，如果无法获取则返回null</returns>
        private string GetFileNameFromClipboard()
        {
            try
            {
                // 检查剪贴板是否包含文件
                if (Clipboard.ContainsFileDropList())
                {
                    StringCollection fileDropList = Clipboard.GetFileDropList();
                    if (fileDropList != null && fileDropList.Count > 0)
                    {
                        string filePath = fileDropList[0];
                        if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                        {
                            return System.IO.Path.GetFileName(filePath);
                        }
                    }
                }

                // 如果没有文件列表，尝试从DataObject中获取其他可能的文件名信息
                IDataObject dataObject = Clipboard.GetDataObject();
                if (dataObject != null)
                {
                    // 检查是否包含文件名相关的格式
                    foreach (string format in dataObject.GetFormats())
                    {
                        // 某些应用程序可能在自定义格式中包含文件名信息
                        if (format.Contains("FileName") || format.Contains("FilePath") || format.Contains("FileDrop"))
                        {
                            try
                            {
                                object data = dataObject.GetData(format);
                                if (data != null)
                                {
                                    // 尝试解析可能的文件名信息
                                    string dataStr = data.ToString();
                                    if (!string.IsNullOrEmpty(dataStr))
                                    {
                                        // 如果数据中包含路径分隔符，尝试提取文件名
                                        if (dataStr.Contains("\\") || dataStr.Contains("/"))
                                        {
                                            return System.IO.Path.GetFileName(dataStr);
                                        }
                                        return dataStr; // 直接返回可能的文件名
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                // 忽略此格式的错误，继续尝试其他格式
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // 忽略剪贴板操作的错误
            }

            return null; // 无法获取文件名
        }

        /// <summary>
        /// 获取图片的字节大小
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <returns>字节大小，如果无法计算则返回0</returns>
        private int GetImageFileSize(Image image)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // 检查RawFormat是否有效，如果无效则使用默认格式PNG
                    if (image.RawFormat.Guid == ImageFormat.MemoryBmp.Guid)
                    {
                        image.Save(ms, ImageFormat.Png);
                    }
                    else
                    {
                        image.Save(ms, image.RawFormat);
                    }
                    return (int)ms.Length;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取图片格式的扩展名
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <returns>扩展名（不含点号），如果无法确定则返回空字符串</returns>
        private string GetImageFormatExtension(Image image)
        {
            try
            {
                if (image == null)
                    return "";

                // 使用ImageUtils工具类获取格式扩展名，并去掉前面的点号
                string extensionWithDot = ImageUtils.GetFormatExtension(image.RawFormat);
                return extensionWithDot.TrimStart('.');
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 根据扩展名获取图片格式
        /// </summary>
        /// <param name="extension">图片扩展名（不含点号）</param>
        /// <returns>对应的ImageFormat，默认返回Jpeg</returns>
        private ImageFormat GetImageFormatFromExtension(string extension)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(extension))
                    return ImageFormat.Jpeg;

                // 转换为小写进行比较
                string extLower = extension.ToLowerInvariant();

                switch (extLower)
                {
                    case "jpg":
                    case "jpeg":
                        return ImageFormat.Jpeg;
                    case "png":
                        return ImageFormat.Png;
                    case "gif":
                        return ImageFormat.Gif;
                    case "bmp":
                        return ImageFormat.Bmp;
                    case "tiff":
                        return ImageFormat.Tiff;
                    default:
                        return ImageFormat.Jpeg; // 默认返回Jpeg格式
                }
            }
            catch (Exception)
            {
                return ImageFormat.Jpeg;
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
            // 如果处于裁剪状态，处理裁剪操作
            if (isCropping)
            {
                ProcessCropOperation();
                return;
            }

            // 检查是否已有图片
            bool hasImage = (this.Image != null) || (MultiImageSupport && images.Count > 0);

            if (hasImage)
            {
                // 如果已有图片，双击显示大图
                ViewLargeImage(sender, e);
            }
            else
            {
                // 如果没有图片，双击添加新图片
                AddImageFromFileDialog();
            }
        }

        /// <summary>
        /// 处理裁剪操作
        /// </summary>
        private void ProcessCropOperation()
        {
            // 保存原始图片信息
            ImageInfo originalInfo = null;
            if (MultiImageSupport && currentImageIndex < imageInfos.Count)
            {
                originalInfo = imageInfos[currentImageIndex];
            }
            else if (imageInfos.Count > 0)
            {
                originalInfo = imageInfos[0];
            }

            this.Image = CropImage(this.Image, cropRectangle);
            string newhash = ImageHelper.GetImageHash(this.Image);
            RowImage.SetImageNewHash(newhash);

            // 更新多图片列表中的当前图片
            if (MultiImageSupport && currentImageIndex < images.Count)
            {
                images[currentImageIndex] = this.Image;
                UpdateImagePathsFromImages(); // 更新图片路径

                // 使用新方法更新图片及其信息，自动处理哈希值比较和版本管理
                UpdateImageWithOriginalInfo(currentImageIndex, this.Image, "旋转图片");

                // 标记图片为已更新，以便业务层处理时能识别出需要上传的图片
                MarkImageAsUpdated(currentImageIndex);
            }
            else if (originalInfo != null) // 单图片模式
            {
                // 在单图片模式下，我们也使用UpdateImageWithOriginalInfo，但需要特殊处理
                try
                {
                    // 为单图片模式临时启用多图片支持以便使用更新方法
                    bool wasMultiImageSupported = MultiImageSupport;
                    MultiImageSupport = true;

                    // 确保图片列表不为空
                    if (images == null || images.Count == 0)
                    {
                        images = new List<Image> { this.Image };
                        currentImageIndex = 0;
                    }
                    else if (images.Count > 0)
                    {
                        images[0] = this.Image;
                        currentImageIndex = 0;
                    }

                    // 更新图片信息
                    UpdateImageWithOriginalInfo(0, this.Image, "裁剪图片");

                    // 标记为已更新
                    MarkImageAsUpdated(0);

                    // 恢复多图片支持设置
                    MultiImageSupport = wasMultiImageSupported;

                    // 确保单图片模式下的Image属性也被更新
                    if (!wasMultiImageSupported && images.Count > 0)
                    {
                        this.Image = images[0];
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"单图片模式下更新图片信息失败: {ex.Message}");
                }
            }

            isCropping = false;
        }

        /// <summary>
        /// 从文件对话框添加图片
        /// </summary>
        private void AddImageFromFileDialog()
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

                    // 获取文件信息
                    var fileInfo = new FileInfo(openFileDialog.FileName);

                    // 如果是多图片模式，添加到列表中
                    if (MultiImageSupport)
                    {
                        images.Add(this.Image);
                        ImageInfo newImageInfo = new ImageInfo
                        {
                            OriginalFileName = fileInfo.Name,
                            FileSize = fileInfo.Length,
                            CreateTime = fileInfo.CreationTime,
                            FileType = Path.GetExtension(openFileDialog.FileName).TrimStart('.'),
                            HashValue = CalculateImageHash(this.Image),
                            Width = this.Image?.Width ?? 0,
                            Height = this.Image?.Height ?? 0,
                            IsUpdated = true // 标记为已更新
                        };
                        imageInfos.Add(newImageInfo);
                        // 标记图片需要更新
                        _updateManager.MarkImageAsUpdated(newImageInfo);
                        currentImageIndex = images.Count - 1;
                        CreateNavigationControls();
                        UpdatePageInfo();
                        UpdateImagePathsFromImages(); // 更新图片路径
                        CreateInfoPanel();
                    }
                    else
                    {
                        // 单图片模式，替换第一张图片
                        if (images.Count > 0)
                        {
                            // 替换现有图片
                            images[0] = this.Image;
                            imageInfos[0] = new ImageInfo
                            {
                                OriginalFileName = fileInfo.Name,
                                FileSize = fileInfo.Length,
                                IsUpdated = true, // 标记为已更新
                                CreateTime = fileInfo.CreationTime,
                                FileType = Path.GetExtension(openFileDialog.FileName).TrimStart('.'),
                                HashValue = CalculateImageHash(this.Image),
                                Width = this.Image?.Width ?? 0,
                                Height = this.Image?.Height ?? 0
                            };
                            // 标记图片需要更新
                            _updateManager.MarkImageAsUpdated(imageInfos[0]);
                            currentImageIndex = 0;
                        }
                        else
                        {
                            // 如果还没有图片，添加新图片
                            images.Add(this.Image);
                            ImageInfo newImageInfo = new ImageInfo
                            {
                                OriginalFileName = fileInfo.Name,
                                FileSize = fileInfo.Length,
                                IsUpdated = true, // 标记为已更新
                                CreateTime = fileInfo.CreationTime,
                                FileType = Path.GetExtension(openFileDialog.FileName).TrimStart('.'),
                                HashValue = CalculateImageHash(this.Image),
                                Width = this.Image?.Width ?? 0,
                                Height = this.Image?.Height ?? 0
                            };
                            imageInfos.Add(newImageInfo);
                            // 标记图片需要更新
                            _updateManager.MarkImageAsUpdated(newImageInfo);
                            currentImageIndex = 0;
                        }
                        UpdateInfoPanel();
                    }

                    // 添加右键菜单项
                    UpdateContextMenu();
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
                    // 保存原始图片信息
                    ImageInfo originalInfo = null;
                    if (MultiImageSupport && currentImageIndex < imageInfos.Count)
                    {
                        originalInfo = imageInfos[currentImageIndex];
                    }
                    else if (imageInfos.Count > 0)
                    {
                        originalInfo = imageInfos[0];
                    }

                    this.Image = CropImage(this.Image, cropRectangle);
                    string newhash = ImageHelper.GetImageHash(this.Image);
                    RowImage.SetImageNewHash(newhash);

                    // 更新多图片列表中的当前图片
                    if (MultiImageSupport && currentImageIndex < images.Count)
                    {
                        images[currentImageIndex] = this.Image;

                        // 使用新方法更新图片及其信息，自动处理哈希值比较和版本管理
                        UpdateImageWithOriginalInfo(currentImageIndex, this.Image, "裁剪图片");

                        // 标记图片为已更新，以便业务层处理时能识别出需要上传的图片
                        MarkImageAsUpdated(currentImageIndex);
                    }
                    else if (originalInfo != null) // 单图片模式
                    {
                        // 更新单图片信息，保留原始文件名
                        imageInfos[0] = new ImageInfo
                        {
                            OriginalFileName = originalInfo.OriginalFileName,
                            FileSize = GetImageFileSize(this.Image),
                            CreateTime = originalInfo.CreateTime,
                            Metadata = originalInfo.Metadata,
                            FileType = originalInfo.FileType,
                            FileExtension = originalInfo.FileExtension,

                        };
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
                // 保存原始图片信息
                ImageInfo originalInfo = null;
                if (MultiImageSupport && currentImageIndex < imageInfos.Count)
                {
                    originalInfo = imageInfos[currentImageIndex];
                }
                else if (imageInfos.Count > 0)
                {
                    originalInfo = imageInfos[0];
                }

                rotationAngle += 90;
                this.Image = RotateImage(this.Image, rotationAngle);

                // 更新多图片列表中的当前图片
                if (MultiImageSupport && currentImageIndex < images.Count)
                {
                    images[currentImageIndex] = this.Image;

                    // 更新图片信息，保留原始文件名
                    if (currentImageIndex < imageInfos.Count && originalInfo != null)
                    {
                        imageInfos[currentImageIndex] = new ImageInfo
                        {
                            OriginalFileName = originalInfo.OriginalFileName, // 保留原始文件名
                            FileSize = GetImageFileSize(this.Image), // 更新文件大小
                            CreateTime = originalInfo.CreateTime,
                            Metadata = originalInfo.Metadata,
                            FileType = originalInfo.FileType,

                        };
                    }
                    else if (originalInfo != null)
                    {
                        imageInfos.Add(new ImageInfo
                        {
                            OriginalFileName = originalInfo.OriginalFileName,
                            FileSize = GetImageFileSize(this.Image),
                            CreateTime = originalInfo.CreateTime,
                            Metadata = originalInfo.Metadata,
                            FileType = originalInfo.FileType,
                            FileExtension = originalInfo.FileExtension,

                        });
                    }
                }
                else if (originalInfo != null) // 单图片模式
                {
                    // 更新单图片信息，保留原始文件名
                    imageInfos[0] = new ImageInfo
                    {
                        OriginalFileName = originalInfo.OriginalFileName,
                        FileSize = GetImageFileSize(this.Image),
                        CreateTime = originalInfo.CreateTime,
                        Metadata = originalInfo.Metadata,
                        FileType = originalInfo.FileType,
                        FileExtension = originalInfo.FileExtension,

                    };
                }
            }
        }

        /// <summary>
        /// 旋转图像
        /// </summary>
        /// <param name="img">原始图像</param>
        /// <param name="angle">旋转角度</param>
        /// <returns>旋转后的图像</returns>
        private Image RotateImage(Image img, float angle)
        {
            // 创建一个新的位图用于旋转，避免修改原图
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                // 设置高质量渲染
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                // 旋转图像
                graphics.TranslateTransform(img.Width / 2, img.Height / 2);
                graphics.RotateTransform(angle);
                graphics.TranslateTransform(-img.Width / 2, -img.Height / 2);
                graphics.DrawImage(img, new Point(0, 0));
            }
            return bmp;
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
            if (MultiImageSupport && imageList != null)
            {
                images = new List<Image>(imageList);
                imageInfos.Clear(); // 清空旧的图片信息

                // 为每个图片创建ImageInfo对象并计算哈希值
                for (int i = 0; i < imageList.Count; i++)
                {
                    try
                    {
                        // 将图片转换为字节数组
                        byte[] imageBytes;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            imageList[i].Save(ms, ImageFormat.Jpeg);
                            imageBytes = ms.ToArray();
                        }

                        // 计算哈希值
                        string hashValue = CalculateImageHash(imageBytes);

                        // 创建ImageInfo对象
                        ImageInfo newImageInfo = new ImageInfo
                        {
                            OriginalFileName = $"图片{i + 1}",
                            FileSize = imageBytes.Length,
                            CreateTime = DateTime.Now,
                            Metadata = new Dictionary<string, string>(),
                            HashValue = hashValue,
                            IsUpdated = true, // 标记为已更新
                            Width = imageList[i]?.Width ?? 0,
                            Height = imageList[i]?.Height ?? 0
                        };
                        imageInfos.Add(newImageInfo);
                        _updateManager.MarkImageAsUpdated(newImageInfo); // 标记为需要更新
                    }
                    catch (Exception ex)
                    {
                        // 处理异常，记录错误信息
                        System.Diagnostics.Debug.WriteLine($"处理图片{i + 1}时出错: {ex.Message}");
                        // 为出错的图片创建一个基本的ImageInfo对象
                        imageInfos.Add(new ImageInfo
                        {
                            OriginalFileName = $"图片{i + 1}",
                            FileSize = 0,
                            CreateTime = DateTime.Now,
                            Metadata = new Dictionary<string, string> { { "Error", "加载失败" } },
                            HashValue = "",
                            IsUpdated = false,
                            Width = 0,
                            Height = 0
                        });
                    }
                }

                currentImageIndex = 0;
                ShowCurrentImage();
                CreateNavigationControls();
                CreateInfoPanel(); // 创建信息面板
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
                            // 确保文件大小正确更新
                            if (imageInfo != null)
                            {
                                imageInfo.FileSize = imageBytes?.Length ?? 0;
                            }
                        }
                        else
                        {
                            // 如果没有对应的信息，创建更有意义的默认信息
                            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                            string formatExtension = ImageUtils.GetFormatExtension(format);
                            string defaultFileName = $"图片_{timestamp}_{i + 1}.{formatExtension}";

                            imageInfo = new ImageInfo
                            {
                                OriginalFileName = defaultFileName,
                                FileSize = imageBytes?.Length ?? 0,
                                CreateTime = DateTime.Now,
                                Metadata = new Dictionary<string, string> { { "Description", "自动生成的图片信息" } },
                                FileType = formatExtension,
                                Width = this.Image?.Width ?? 0,
                                Height = this.Image?.Height ?? 0
                            };
                        }

                        imageBytesWithInfoList.Add(new Tuple<byte[], ImageInfo>(imageBytes, imageInfo));
                    }
                    catch (Exception ex)
                    {
                        // 转换失败，记录错误并跳过该图片
                        System.Diagnostics.Debug.WriteLine($"获取图片数据失败 (索引 {i}): {ex.Message}");
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
                        // 确保文件大小正确更新
                        if (imageInfo != null)
                        {
                            imageInfo.FileSize = imageBytes?.Length ?? 0;
                        }
                    }
                    else
                    {
                        // 如果没有信息，创建更有意义的默认信息
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string formatExtension = ImageUtils.GetFormatExtension(format);
                        string defaultFileName = $"图片_{timestamp}.{formatExtension}";

                        imageInfo = new ImageInfo
                        {
                            OriginalFileName = defaultFileName,
                            FileSize = imageBytes?.Length ?? 0,
                            CreateTime = DateTime.Now,
                            Metadata = new Dictionary<string, string> { { "Description", "自动生成的图片信息" } },
                            FileType = formatExtension,
                        };
                    }

                    imageBytesWithInfoList.Add(new Tuple<byte[], ImageInfo>(imageBytes, imageInfo));
                }
                catch (Exception ex)
                {
                    // 转换失败，记录错误
                    System.Diagnostics.Debug.WriteLine($"获取单张图片数据失败: {ex.Message}");
                }
            }

            return imageBytesWithInfoList;
        }

        /// <summary>
        /// 计算图片内容的哈希值
        /// </summary>
        #region 图片处理功能

        /// <summary>
        /// 将Image对象转换为字节数组
        /// </summary>
        /// <param name="image">Image对象</param>
        /// <param name="format">图片格式，如果为null则默认使用JPEG</param>
        /// <returns>转换后的字节数组</returns>
        public byte[] ImageToBytes(Image image, ImageFormat format = null)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Image对象不能为空");
            }

            // 使用ImageProcessor组件处理图片转换
            return _imageProcessor.ImageToBytes(image);
        }

        /// <summary>
        /// 将字节数组转换为Image对象
        /// </summary>
        /// <param name="bytes">图片字节数组</param>
        /// <returns>转换后的Image对象</returns>
        public Image BytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentNullException(nameof(bytes), "图片字节数组不能为空");
            }

            // 使用ImageProcessor组件处理图片转换
            return _imageProcessor.BytesToImage(bytes);
        }

        /// <summary>
        /// 将字节数组转换为Image对象
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>Image对象</returns>
        // 已移除重复的私有BytesToImage方法，使用下面的公有方法替代

        #endregion

        #region 图片缓存功能

        /// <summary>
        /// 获取图片编码器
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns>对应的图片编码器</returns>
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        #region 哈希计算功能

        /// <summary>
        /// 计算图片字节数据的哈希值
        /// </summary>
        /// <param name="imageBytes">图片字节数据</param>
        /// <returns>哈希值字符串</returns>
        public string CalculateImageHash(byte[] imageBytes)
        {
            try
            {
                if (imageBytes == null || imageBytes.Length == 0)
                {
                    throw new ArgumentException("图片字节数组不能为空");
                }
                return _hashCalculator.CalculateHash(imageBytes);
            }
            catch (Exception ex)
            {
                // 记录错误并返回空字符串，避免影响后续操作
                System.Diagnostics.Debug.WriteLine($"计算图片哈希值失败: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 计算Image对象的哈希值
        /// </summary>
        /// <param name="image">Image对象</param>
        /// <returns>哈希值字符串</returns>
        public string CalculateImageHash(Image image)
        {
            try
            {
                if (image == null)
                {
                    throw new ArgumentNullException("image", "图片对象不能为空");
                }
                return _hashCalculator.CalculateHash(image);
            }
            catch (Exception ex)
            {
                // 记录错误并返回空字符串，避免影响后续操作
                System.Diagnostics.Debug.WriteLine($"计算图片哈希值失败: {ex.Message}");
                return string.Empty;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// 比较两张图片是否相同（基于哈希值）
        /// </summary>
        /// <param name="imageBytes1">第一张图片的字节数据</param>
        /// <param name="imageBytes2">第二张图片的字节数据</param>
        /// <returns>如果相同返回true，否则返回false</returns>
        public bool AreImagesEqual(byte[] imageBytes1, byte[] imageBytes2)
        {
            try
            {
                // 验证参数
                if (imageBytes1 == null || imageBytes2 == null)
                {
                    // 两个都是null视为相等，只有一个是null视为不相等
                    return imageBytes1 == imageBytes2;
                }

                if (imageBytes1.Length == 0 || imageBytes2.Length == 0)
                {
                    // 两个都是空数组视为相等，只有一个是空数组视为不相等
                    return imageBytes1.Length == imageBytes2.Length;
                }

                // 使用哈希计算器组件比较图片
                return _hashCalculator.AreImagesEqual(imageBytes1, imageBytes2);
            }
            catch (Exception ex)
            {
                // 记录错误并返回false，避免影响后续操作
                System.Diagnostics.Debug.WriteLine($"比较图片失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 比较两个Image对象是否相同（基于哈希值）
        /// </summary>
        /// <param name="image1">第一个Image对象</param>
        /// <param name="image2">第二个Image对象</param>
        /// <returns>如果相同返回true，否则返回false</returns>
        public bool AreImagesEqual(Image image1, Image image2)
        {
            try
            {
                // 验证参数
                if (image1 == null || image2 == null)
                {
                    // 两个都是null视为相等，只有一个是null视为不相等
                    return image1 == image2;
                }

                // 使用哈希计算器组件比较图片
                return _hashCalculator.AreImagesEqual(image1, image2);
            }
            catch (Exception ex)
            {
                // 记录错误并返回false，避免影响后续操作
                System.Diagnostics.Debug.WriteLine($"比较图片失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 更新图片时保留原始信息
        /// </summary>
        /// <param name="index">图片索引</param>
        /// <param name="newImage">新图片</param>
        /// <param name="updateReason">更新原因</param>
        public void UpdateImageWithOriginalInfo(int index, Image newImage, string updateReason = "")
        {
            try
            {
                if (MultiImageSupport && index >= 0 && index < images.Count)
                {
                    // 保存原始信息
                    ImageInfo originalInfo = index < imageInfos.Count ? imageInfos[index] : null;

                    // 检查图片是否有变化
                    bool hasChanged = true;
                    byte[] oldImageBytes = null;
                    byte[] newImageBytes = null;

                    using (var ms = new MemoryStream())
                    {
                        newImage.Save(ms, ImageFormat.Jpeg);
                        newImageBytes = ms.ToArray();
                    }

                    if (originalInfo != null && !string.IsNullOrEmpty(originalInfo.HashValue))
                    {
                        // 计算新图片的哈希值并与原始哈希值比较
                        string newHash = CalculateImageHash(newImageBytes);
                        hasChanged = newHash != originalInfo.HashValue;
                    }
                    else if (images[index] != null)
                    {
                        // 如果没有原始哈希值，比较两个图片的内容
                        using (var ms = new MemoryStream())
                        {
                            images[index].Save(ms, ImageFormat.Jpeg);
                            oldImageBytes = ms.ToArray();
                        }
                        hasChanged = !AreImagesEqual(oldImageBytes, newImageBytes);
                    }

                    // 更新图片
                    images[index] = newImage;

                    // 如果图片内容有变化或没有原始信息，更新信息
                    if (hasChanged || originalInfo == null)
                        if (hasChanged || originalInfo == null)
                        {
                            if (originalInfo != null)
                            {
                                // 保留原始信息但更新文件大小和哈希值
                                imageInfos[index] = new ImageInfo
                                {
                                    OriginalFileName = originalInfo.OriginalFileName, // 保留原始文件名
                                    FileSize = GetImageFileSize(newImage), // 更新文件大小
                                    CreateTime = originalInfo.CreateTime, // 保留创建时间
                                    Metadata = originalInfo.Metadata,
                                    FileType = originalInfo.FileType,
                                    HashValue = CalculateImageHash(newImageBytes), // 更新哈希值
                                    ModifiedAt = DateTime.Now, // 更新修改时间
                                    Width = newImage?.Width ?? 0,
                                    Height = newImage?.Height ?? 0
                                };
                            }
                            else
                            {
                                // 创建新的信息
                                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                string defaultFileName = $"图片_{timestamp}_{index + 1}.jpg";

                                imageInfos[index] = new ImageInfo
                                {
                                    OriginalFileName = defaultFileName,
                                    FileSize = GetImageFileSize(newImage),
                                    CreateTime = DateTime.Now,
                                    Metadata = new Dictionary<string, string>(),
                                    FileType = "jpg",
                                    HashValue = CalculateImageHash(newImageBytes),

                                    ModifiedAt = DateTime.Now,
                                    Width = newImage?.Width ?? 0,
                                    Height = newImage?.Height ?? 0
                                };
                            }
                        }

                    ShowCurrentImage();
                    UpdateInfoPanel();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"更新图片信息失败: {ex.Message}");
                MessageBox.Show($"更新图片失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 标记图片为已更新（用于在业务层处理时识别需要上传的图片）
        /// </summary>
        /// <param name="index">图片索引</param>
        public void MarkImageAsUpdated(int index)
        {
            if (index >= 0 && index < imageInfos.Count)
            {
                // 使用UpdateManager标记图片为需要更新，确保与ImageUpdateManager中的实现保持一致
                _updateManager.MarkImageAsUpdated(imageInfos[index]);
                imageInfos[index].ModifiedAt = DateTime.Now;
                imageInfos[index].IsUpdated = true; // 确保同时设置IsUpdated为true
            }
        }

        /// <summary>
        /// 检查图片是否需要更新
        /// </summary>
        /// <param name="index">图片索引，从0开始</param>
        /// <returns>如果需要更新返回true，否则返回false</returns>
        public bool IsImageNeedingUpdate(int index)
        {
            if (index >= 0 && index < imageInfos.Count)
            {
                // 统一使用ImageUpdateManager的判断逻辑
                return _updateManager.IsImageNeedingUpdate(imageInfos[index]);
            }
            return false;
        }

        /// <summary>
        /// 获取需要更新的图片及其信息
        /// </summary>
        /// <returns>需要更新的图片和信息列表</returns>
        public List<Tuple<byte[], ImageInfo>> GetImagesNeedingUpdate()
        {
            List<Tuple<byte[], ImageInfo>> updateList = new List<Tuple<byte[], ImageInfo>>();

            if (MultiImageSupport && images.Count > 0)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    // 检查是否需要更新，同时确保索引有效
                    if (i < imageInfos.Count && imageInfos[i] != null && IsImageNeedingUpdate(i))
                    {
                        try
                        {
                            byte[] imageBytes = null;
                            using (var ms = new MemoryStream())
                            {
                                images[i].Save(ms, ImageFormat.Jpeg);
                                imageBytes = ms.ToArray();
                            }

                            // 更新图片信息中的文件大小
                            imageInfos[i].FileSize = imageBytes.Length;

                            updateList.Add(new Tuple<byte[], ImageInfo>(imageBytes, imageInfos[i]));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"获取待更新图片数据失败 (索引 {i}): {ex.Message}");
                        }
                    }
                }
            }
            else if (this.Image != null && imageInfos.Count > 0 && imageInfos[0] != null)
            {
                // 单图片模式下，检查是否需要更新（包括新添加的图片）
                bool needsUpdate = IsImageNeedingUpdate(0) ||
                                    imageInfos[0].FileId == 0 ||
                                   string.IsNullOrEmpty(imageInfos[0].HashValue) ||
                                   imageInfos[0].IsUpdated;

                if (needsUpdate)
                {
                    try
                    {
                        byte[] imageBytes = null;
                        using (var ms = new MemoryStream())
                        {
                            this.Image.Save(ms, ImageFormat.Jpeg);
                            imageBytes = ms.ToArray();
                        }

                        // 更新图片信息中的文件大小
                        imageInfos[0].FileSize = imageBytes.Length;

                        updateList.Add(new Tuple<byte[], ImageInfo>(imageBytes, imageInfos[0]));
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"获取待更新单张图片数据失败: {ex.Message}");
                    }
                }
            }

            return updateList;
        }

        /// <summary>
        /// 获取已删除但尚未同步到服务器的图片列表
        /// 
        /// 功能说明：
        /// - 返回已被用户删除的图片信息列表
        /// - 这些图片有 FileId（已上传到服务器），但已从显示列表中移除
        /// - 需要在保存时从服务器上删除这些文件
        /// 
        /// 返回值说明：
        /// - 返回列表的副本（new List<ImageInfo>()），避免外部修改内部列表
        /// - 每个图片信息的 IsDeleted=true 和 IsUpdated=true
        /// - 每个图片信息的 FileId > 0（表示已上传到服务器）
        /// 
        /// 使用场景：
        /// - 在 BaseBillEditGeneric.UploadUpdatedImagesAsync 中调用
        /// - 获取需要从服务器删除的图片列表
        /// - 调用 FileBusinessService 删除这些文件
        /// 
        /// 注意事项：
        /// - 此方法只返回已删除的图片，不包含新添加的图片
        /// - 新添加的图片通过 GetImagesNeedingUpdate() 获取
        /// - 删除操作成功后应调用 ClearDeletedImagesList() 清空列表
        /// </summary>
        /// <returns>已删除的图片信息列表（副本）</returns>
        public List<ImageInfo> GetDeletedImages()
        {
            // 返回副本，避免外部代码直接修改内部列表
            return new List<ImageInfo>(_deletedImages);
        }

        /// <summary>
        /// 清空已删除图片列表
        /// 
        /// 功能说明：
        /// - 清空 _deletedImages 列表
        /// - 表示所有已删除的图片都已成功从服务器删除
        /// 
        /// 调用时机：
        /// - 在成功调用 FileBusinessService.DeleteFileAsync 后
        /// - 确保服务器删除操作成功后
        /// - 通常与 ResetImageChangeStatus() 一起调用
        /// 
        /// 数据一致性：
        /// - 调用此方法后，无法再访问之前删除的图片信息
        /// - 如果保存失败，不应调用此方法（否则会丢失删除记录）
        /// - 正常流程：保存成功 -> 删除服务器文件成功 -> 清空列表
        /// 
        /// 异常处理：
        /// - 如果服务器删除失败，不应清空列表
        /// - 用户再次保存时会重试删除操作
        /// - 如果用户取消编辑，列表会在窗体关闭时自然清空
        /// </summary>
        public void ClearDeletedImagesList()
        {
            _deletedImages.Clear();
        }

        /// <summary>
        /// 重置所有图片的更新和删除状态
        /// 在成功保存并同步到服务器后调用此方法
        /// 
        /// 功能说明：
        /// 1. 重置当前图片列表中所有图片的 IsUpdated 和 IsDeleted 标记
        /// 2. 清空删除图片列表（因为这些图片已经成功从服务器删除）
        /// 
        /// 调用时机：
        /// - 在 BaseBillEditGeneric.UploadUpdatedImagesAsync 成功执行后
        /// - 确保所有图片变更（上传、更新、删除）都已同步到服务器
        /// 
        /// 特殊说明：
        /// - 此方法不应在保存失败时调用，否则会丢失删除记录
        /// - 已删除的图片不会出现在 imageInfos 列表中，所以只需处理当前存在的图片
        /// </summary>
        public void ResetImageChangeStatus()
        {
            foreach (var imageInfo in imageInfos)
            {
                if (imageInfo != null)
                {
                    imageInfo.IsUpdated = false;
                    imageInfo.IsDeleted = false;
                    _updateManager.ResetImageUpdateStatus(imageInfo);
                }
            }
            // 清空删除列表：因为所有已删除的图片都已经成功从服务器删除
            _deletedImages.Clear();
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

                // 清空图片缓存，释放所有缓存的图片资源
                ClearCache();

                // 释放图片列表中的所有图片资源
                if (MultiImageSupport && images != null)
                {
                    foreach (var image in images)
                    {
                        // 使用临时变量避免foreach迭代变量作为ref参数
                        var tempImage = image;
                        // 使用ImageUtils工具类安全释放图片资源
                        ImageUtils.SafeDispose(ref tempImage);
                    }
                    images.Clear();
                }
            }
            base.Dispose(disposing);
        }
    }
}